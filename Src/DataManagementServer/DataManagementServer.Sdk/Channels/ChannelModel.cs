using System;
using Newtonsoft.Json;

namespace DataManagementServer.Sdk.Channels
{
    /// <summary>
    /// Модель Канала системы
    /// </summary>
    public class ChannelModel
    {
        /// <summary>
        /// Id канала
        /// </summary>
        public Guid Id { get; set; }

        #region Свойства для доступа к стандартным полям Каналов
        /// <summary>
        /// Id группы канала
        /// </summary>
        [JsonIgnore]
        public Guid? GroupId
        {
            get
            {
                if (TryGetFieldValue(ChannelScheme.GroupId, out Guid? value))
                {
                    return value;
                }
                else 
                { 
                    return null;
                }
            }
            set
            {
                Fields[ChannelScheme.GroupId] = value;
            }
        }

        /// <summary>
        /// Название Канала
        /// </summary>
        [JsonIgnore]
        public string Name
        {
            get
            {
                if (TryGetFieldValue(ChannelScheme.Name, out string value))
                {
                    return value;
                }
                else 
                { 
                    return null;
                }
            }
            set
            {
                Fields[ChannelScheme.Name] = value;
            }
        }

        /// <summary>
        /// Описание Канала
        /// </summary>
        [JsonIgnore]
        public string Description
        {
            get
            {
                if (TryGetFieldValue(ChannelScheme.Description, out string value))
                {
                    return value;
                }
                else
                { 
                    return null;
                }
            }
            set
            {
                Fields[ChannelScheme.Description] = value;
            }
        }

        /// <summary>
        /// Тип данных значения Канала
        /// </summary>
        [JsonIgnore]
        public TypeCode? ValueType
        {
            get
            {
                if (TryGetFieldValue(ChannelScheme.ValueType, out TypeCode? value))
                {
                    return value;
                }
                else 
                {
                    return null; 
                }
            }
            set
            {
                Fields[ChannelScheme.ValueType] = value;
            }
        }

        /// <summary>
        /// Значение Канала
        /// </summary>
        [JsonIgnore]
        public object Value
        {
            get
            {
                if (TryGetFieldValue(ChannelScheme.Value, out object value))
                {
                    return value;
                }
                else 
                { 
                    return null;
                }
            }
            set
            {
                Fields[ChannelScheme.Value] = value;
            }
        }

        /// <summary>
        /// Дата обновления Канала
        /// </summary>
        [JsonIgnore]
        public DateTime? UpdateOn
        {
            get
            {
                if (TryGetFieldValue(ChannelScheme.UpdateOn, out DateTime? value))
                {
                    return value;
                }
                else 
                { 
                    return null;
                }
            }
            set
            {
                Fields[ChannelScheme.UpdateOn] = value;
            }
        }

        /// <summary>
        /// Статус Канала
        /// </summary>
        [JsonIgnore]
        public ChannelStatus? Status
        {
            get
            {
                if (TryGetFieldValue(ChannelScheme.Status, out ChannelStatus? value))
                {
                    return value;
                }
                else 
                { 
                    return null; 
                }
            }
            set
            {
                Fields[ChannelScheme.Status] = value;
            }
        }
        #endregion

        /// <summary>
        /// Поля канала
        /// </summary>
        public FieldValueCollection Fields { get; } = new ();

        /// <summary>
        /// Свойство доступа к поля модели
        /// </summary>
        /// <param name="fieldName">Название поля</param>
        /// <returns>Значение поля</returns>
        [JsonIgnore]
        public object this[string fieldName]
        {
            get { return Fields[fieldName]; }
            set { Fields.Add(fieldName, value); }
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public ChannelModel()
        {

        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="id">Id Канала</param>
        public ChannelModel(Guid id)
        {
            Id = id;
        }

        #region Методы доступа к полям
        /// <summary>
        /// Попытка получения типизированного значения поля
        /// </summary>
        /// <typeparam name="T">Тип значения поля</typeparam>
        /// <param name="fieldName">Имя поля</param>
        /// <param name="fieldValue">Значение поля</param>
        /// <returns>Результат получения значения</returns>
        /// <exception cref="ArgumentNullException">Ошибка при передаче пустого имени поля</exception>
        public bool TryGetFieldValue<T>(string fieldName, out T fieldValue)
        {
            return Fields.TryGetFieldValue(fieldName, out fieldValue);
        }

        /// <summary>
        /// Получение типизированного значения поля
        /// </summary>
        /// <typeparam name="T">Тип значения поля</typeparam>
        /// <param name="fieldName">Имя поля</param>
        /// <returns>Значение поля</returns>
        /// <exception cref="ArgumentNullException">Ошибка при передаче пустого имени поля</exception>
        /// <exception cref="Exception">Ошибка при несоответвии типа поля</exception>
        public T GetFieldValue<T>(string fieldName)
        {
            return Fields.GetFieldValue<T>(fieldName);
        }
        #endregion
    }
}
