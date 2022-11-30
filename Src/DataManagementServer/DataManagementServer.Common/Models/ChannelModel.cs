using DataManagementServer.Common.Schemes;
using System;
using System.Text.Json.Serialization;

namespace DataManagementServer.Common.Models
{
    /// <summary>
    /// Модель Канала системы
    /// </summary>
    public class ChannelModel : BaseModel
    {
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
        /// Конструктор
        /// </summary>
        public ChannelModel() { }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="id">Id Канала</param>
        public ChannelModel(Guid id) : base(id) { }
    }
}
