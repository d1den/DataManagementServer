using DataManagementServer.Common.Models;
using DataManagementServer.Common.Schemes;
using DataManagementServer.Sdk.Channels;
using DataManagementServer.Sdk.Extensions;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;

namespace DataManagementServer.Core.Channels
{
    /// <summary>
    /// Канал системы - основная сущность данных
    /// </summary>
    public class Channel : IDisposable
    {
        #region Константы
        /// <summary>
        /// Шаблон ошибки о неверном типа значения
        /// </summary>
        private const string _ValueTypeErrorTemplate = "Value {0} has wrong type (Current type {1}).";

        /// <summary>
        /// Шаблон ошибки о несоответвии типов
        /// </summary>
        private const string _TypeErrorTemplate = "Type {0} does not match value type {1}";

        #endregion

        #region Поля
        /// <summary>
        /// Поле тип данных значения
        /// </summary>
        private TypeCode _ValueType;

        /// <summary>
        /// Поле значения канала
        /// </summary>
        private object _Value;

        /// <summary>
        /// Дополнительные поля канала
        /// </summary>
        private readonly Dictionary<string, object> _AdditionalFields = new();

        /// <summary>
        /// Событие обновления канала
        /// </summary>
        private event EventHandler<UpdateEventArgs> _UpdateEvent;

        /// <summary>
        /// Объект для синхронизации потоков
        /// </summary>
        private readonly ReaderWriterLockSlim _Lock = new();

        /// <summary>
        /// Уже уничтожен?
        /// </summary>
        private bool _IsDisposed = false;
        #endregion

        #region Свойства
        /// <summary>
        /// Id канала
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Id группы канала
        /// </summary>
        public Guid GroupId { get; private set; }

        /// <summary>
        /// Название канала
        /// </summary>
        private string Name { get; set; } = "New channel";

        /// <summary>
        /// Описание канала
        /// </summary>
        private string Description { get; set; } = string.Empty;

        /// <summary>
        /// Тип данных значения
        /// </summary>
        private TypeCode ValueType
        {
            get { return _ValueType; }
            set
            {
                _ValueType = value;
                Value = TypeExtensions.GetTypeByCode(value)?.GetDefaultValue();
            }
        }

        /// <summary>
        /// Значение канала
        /// </summary>
        public object Value
        {
            get
            {
                return _Value;
            }
            private set
            {
                if (ValueType == TypeCode.Object
                    || ((value == null
                    && (ValueType == TypeCode.Empty
                    || ValueType == TypeCode.DBNull
                    || ValueType == TypeCode.Object
                    || ValueType == TypeCode.String))
                    || Type.GetTypeCode(value?.GetType()) == ValueType))
                {
                    _Value = value;
                    UpdateOn = DateTime.UtcNow;
                }
                else
                {
                    throw new ArgumentException(string
                        .Format(_ValueTypeErrorTemplate, value, ValueType));
                }
            }
        }

        /// <summary>
        /// Дата обновления значения в UTC
        /// </summary>
        private DateTime UpdateOn { get; set; }

        /// <summary>
        /// Статус канала
        /// </summary>
        private ChannelStatus Status { get; set; } = ChannelStatus.Good;

        /// <summary>
        /// Уведомитель об обновлении канала
        /// </summary>
        public IObservable<EventPattern<UpdateEventArgs>> ObservableUpdate { get; }
        #endregion

        /// <summary>
        /// Конструктор
        /// </summary>
        public Channel()
        {
            Id = Guid.NewGuid();
            ValueType = TypeCode.Object;
            ObservableUpdate = Observable.FromEventPattern<UpdateEventArgs>(
                handler => _UpdateEvent += handler,
                handler => _UpdateEvent -= handler);
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="model">Модель канала</param>
        /// <exception cref="ArgumentNullException">Ошибка при пустой модели</exception>
        public Channel(ChannelModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            Id = model.Id == Guid.Empty
                ? Guid.NewGuid()
                : model.Id;

            ValueType = TypeCode.Object;
            SetFieldsByModel(model);
            ObservableUpdate = Observable.FromEventPattern<UpdateEventArgs>(
                handler => _UpdateEvent += handler,
                handler => _UpdateEvent -= handler);
        }

        /// <summary>
        /// Обновление значения канала
        /// </summary>
        /// <param name="value">Значение</param>
        public void UpdateValue(object value)
        {
            _Lock.EnterWriteLock();
            try
            {
                Value = value;
                var eventArgs = new UpdateEventArgs(Id,
                    new FieldValueCollection()
                    {
                        [ChannelScheme.Value] = Value,
                        [ChannelScheme.UpdateOn] = UpdateOn
                    });

                _UpdateEvent?.Invoke(this, eventArgs);
            }
            finally
            {
                _Lock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Обновление канала
        /// </summary>
        /// <param name="model">Модель канала</param>
        /// <exception cref="ArgumentNullException">Ошибка при Null значении модели</exception>
        /// <exception cref="ArgumentException">Ошибка при неерном значении Id</exception>
        public void Update(ChannelModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            if (model.Id == Guid.Empty || model.Id != Id)
            {
                throw new ArgumentException(nameof(model.Id));
            }
            _Lock.EnterWriteLock();
            try
            {
                SetFieldsByModel(model);

                var eventArgs = new UpdateEventArgs(Id,
                    model.Fields.Clone() as FieldValueCollection);
                if (model.Fields.ContainsKey(ChannelScheme.ValueType))
                {
                    eventArgs.UpdatedFields[ChannelScheme.Value] = Value;
                }
                if (model.Fields.ContainsKey(ChannelScheme.Value))
                {
                    eventArgs.UpdatedFields[ChannelScheme.UpdateOn] = UpdateOn;
                }

                _UpdateEvent?.Invoke(this, eventArgs);
            }
            finally
            {
                _Lock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Установить поля канала по модели
        /// </summary>
        /// <param name="model">Модель канала</param>
        /// <exception cref="ArgumentNullException">Ошибка, при Null значении</exception>
        private void SetFieldsByModel(ChannelModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (model.Fields.ContainsKey(ChannelScheme.ValueType))
            {
                ValueType = model.ValueType ?? TypeCode.Object;
            }

            foreach (var field in model.Fields)
            {
                switch (field.Key)
                {
                    case ChannelScheme.GroupId: 
                        GroupId = model.GroupId ?? Guid.Empty; 
                        continue;
                    case ChannelScheme.Name:
                        Name = model.Name;
                        continue;
                    case ChannelScheme.Description:
                        Description = model.Description;
                        continue;
                    case ChannelScheme.ValueType: continue;
                    case ChannelScheme.Value:
                        Value = model.Value;
                        continue;
                    case ChannelScheme.UpdateOn:
                        UpdateOn = model.UpdateOn?.ToUniversalTime() 
                            ?? DateTime.MinValue.ToUniversalTime();
                        continue;
                    case ChannelScheme.Status:
                        Status = model.Status ?? ChannelStatus.Good;
                        continue;
                    default:
                        _AdditionalFields[field.Key] = field.Value;
                        continue;
                }
            }
        }

        /// <summary>
        /// Получить значение канала
        /// </summary>
        /// <typeparam name="T">Тип значения</typeparam>
        /// <returns>Значение канала</returns>
        /// <exception cref="ArgumentException">Ошибка при несоответсвии типа и типа значения канала</exception>
        public T GetValue<T>()
        {
            _Lock.EnterReadLock();
            try
            {
                if (Nullable.GetUnderlyingType(typeof(T)) == null
                    && typeof(T) != TypeExtensions.GetTypeByCode(ValueType))
                {
                    throw new ArgumentException(string
                        .Format(_TypeErrorTemplate, typeof(T).Name, ValueType));
                }
                if (Value is T value)
                {
                    return value;
                }
            }
            finally
            {
                _Lock.ExitReadLock();
            }

            return default;
        }

        /// <summary>
        /// Попытка получения значения канала
        /// </summary>
        /// <typeparam name="T">Тип значения</typeparam>
        /// <param name="value">Значение</param>
        /// <returns>Результат попытки</returns>
        public bool TryGetValue<T>(out T value)
        {
            value = default;
            _Lock.EnterReadLock();
            try
            {
                if (Nullable.GetUnderlyingType(typeof(T)) == null
                        && typeof(T) != TypeExtensions.GetTypeByCode(ValueType))
                {
                    return false;
                }
                if (Value is T val)
                {
                    value = val;
                }
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                _Lock.ExitReadLock();
            }
        }

        /// <summary>
        /// Получить модель канала
        /// </summary>
        /// <param name="allFields">Получить все поля или никаких</param>
        /// <returns>Модель канала</returns>
        public ChannelModel ToModel(bool allFields)
        {
            if (!allFields)
            {
                return new ChannelModel(Id);
            }
            _Lock.EnterReadLock();
            try
            {
                var model = new ChannelModel(Id)
                {
                    GroupId = GroupId,
                    Name = Name,
                    Description = Description,
                    ValueType = ValueType,
                    Value = Value,
                    UpdateOn = UpdateOn,
                    Status = Status
                };
                foreach (var field in _AdditionalFields)
                {
                    model.Fields.Add(field.Key, field.Value);
                }

                return model;
            }
            finally
            {
                _Lock.ExitReadLock();
            }
        }

        /// <summary>
        /// Получить модель канала
        /// </summary>
        /// <param name="fields">Желаемые поля</param>
        /// <returns>Модель канала</returns>
        public ChannelModel ToModel(IEnumerable<string> fields = null)
        {
            var model = new ChannelModel(Id);
            if (fields == null)
            {
                return model;
            }
            _Lock.EnterReadLock();
            try
            {
                foreach (var field in fields)
                {
                    switch (field)
                    {
                        case ChannelScheme.GroupId: model.GroupId = GroupId; continue;
                        case ChannelScheme.Name: model.Name = Name; continue;
                        case ChannelScheme.Description: model.Description = Description; continue;
                        case ChannelScheme.ValueType: model.ValueType = ValueType; continue;
                        case ChannelScheme.Value: model.Value = Value; continue;
                        case ChannelScheme.UpdateOn: model.UpdateOn = UpdateOn; continue;
                        case ChannelScheme.Status: model.Status = Status; continue;
                        default:
                            if (_AdditionalFields.TryGetValue(field, out object value))
                            {
                                model.Fields.Add(field, value);
                            }
                            break;
                    }
                }

                return model;
            }
            finally
            {
                _Lock.ExitReadLock();
            }
        }

        public void Dispose()
        {
            if (_IsDisposed)
            {
                return;
            }

            _Lock.Dispose();
            _IsDisposed = true;
        }
    }
}