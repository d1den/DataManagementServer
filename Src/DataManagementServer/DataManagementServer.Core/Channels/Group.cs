using DataManagementServer.Common.Models;
using DataManagementServer.Common.Schemes;
using DataManagementServer.Core.Resources;
using DataManagementServer.Sdk.Channels;
using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;

namespace DataManagementServer.Core.Channels
{
    /// <summary>
    /// Группа каналов
    /// </summary>
    public class Group : IDisposable
    {
        public static Group RootGroup { get; private set; } = new Group(Guid.Empty, Guid.Empty, "RootGroup");
        /// <summary>
        /// Id группы
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Id родительской группы
        /// </summary>
        public Guid ParentId { get; private set; }

        /// <summary>
        /// Название группы
        /// </summary>
        private string Name { get; set; } = Constants.DefaultGroupName;

        /// <summary>
        /// Событие обновления
        /// </summary>
        private event EventHandler<UpdateEventArgs> _UpdateEvent;

        /// <summary>
        /// Уведомитель об обновлении
        /// </summary>
        public IObservable<EventPattern<UpdateEventArgs>> ObservableUpdate { get; }

        /// <summary>
        /// Объект для синхронизации потоков
        /// </summary>
        private readonly ReaderWriterLockSlim _Lock = new();

        /// <summary>
        /// Уже уничтожен?
        /// </summary>
        private bool _IsDisposed = false;

        /// <summary>
        /// Конструктор
        /// </summary>
        public Group()
        {
            Id = Guid.NewGuid();
            ObservableUpdate = Observable.FromEventPattern<UpdateEventArgs>(
                handler => _UpdateEvent += handler,
                handler => _UpdateEvent -= handler);
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="model">Модель группы</param>
        /// <exception cref="ArgumentNullException">Ошибка при Null модели</exception>
        public Group(GroupModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            Id = model.Id == Guid.Empty 
                ? Guid.NewGuid() 
                : model.Id;

            SetFieldsByModel(model);
            ObservableUpdate = Observable.FromEventPattern<UpdateEventArgs>(
                handler => _UpdateEvent += handler,
                handler => _UpdateEvent -= handler);
        }

        private Group(Guid Id, Guid ParentId, string Name)
        {
            ObservableUpdate = Observable.FromEventPattern<UpdateEventArgs>(
                 handler => _UpdateEvent += handler,
                 handler => _UpdateEvent -= handler);
        }

        /// <summary>
        /// Преобразовать к модели
        /// </summary>
        /// <param name="allFields">Получить все поля или только Id</param>
        /// <returns>Модель группы</returns>
        public GroupModel ToModel(bool allFields = false)
        {
            if (!allFields)
            {
                return new GroupModel(Id);
            }
            _Lock.EnterReadLock();
            try
            {
                return new GroupModel(Id) { ParentId = ParentId, Name = Name };
            }
            finally
            {
                _Lock.ExitReadLock();
            }
        }

        /// <summary>
        /// Обновление группы
        /// </summary>
        /// <param name="model">Модель группы</param>
        /// <exception cref="ArgumentNullException">Ошибка при Null модели</exception>
        /// <exception cref="ArgumentException">Ошибка при неверном Id модели</exception>
        public void Update(GroupModel model)
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
                _UpdateEvent?.Invoke(this,
                    new UpdateEventArgs(Id, model.Fields.Clone() as FieldValueCollection));
            }
            finally
            {
                _Lock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Установить поля группы по модели
        /// </summary>
        /// <param name="model">Модель группы</param>
        /// <exception cref="ArgumentNullException">Ошибка, при Null значении</exception>
        private void SetFieldsByModel(GroupModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            foreach (var field in model.Fields)
            {
                switch (field.Key)
                {
                    case GroupScheme.ParentId:
                        ParentId = model.ParentId ?? Guid.Empty;
                        continue;
                    case GroupScheme.Name:
                        Name = model.Name;
                        continue;
                    default: continue;
                }
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
