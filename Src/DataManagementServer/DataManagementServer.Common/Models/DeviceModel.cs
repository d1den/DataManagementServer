using DataManagementServer.Common.Schemes;
using System;

namespace DataManagementServer.Common.Models
{
    /// <summary>
    /// Базовая модель устройства
    /// </summary>
    public class DeviceModel : BaseModel
    {
        /// <summary>
        /// Название устройства
        /// </summary>
        public string Name
        {
            get
            {
                if (TryGetFieldValue(DeviceScheme.Name, out string value))
                {
                    return value;
                }

                return null;
            }
            set
            {
                Fields[DeviceScheme.Name] = value;
            }
        }

        /// <summary>
        /// Статус устройства
        /// </summary>
        public DeviceStatus? Status
        {
            get
            {
                if (TryGetFieldValue(DeviceScheme.Status, out DeviceStatus value))
                {
                    return value;
                }

                return null;
            }
            set
            {
                Fields[DeviceScheme.Status] = value;
            }
        }

        /// <summary>
        /// Период опроса устройства, мс
        /// </summary>
        public int? PollingPeriod
        {
            get
            {
                if (TryGetFieldValue(DeviceScheme.PollingPeriod, out int value))
                {
                    return value;
                }

                return null;
            }
            set
            {
                Fields[DeviceScheme.PollingPeriod] = value;
            }
        }

        /// <summary>
        /// Конструктор модели
        /// </summary>
        public DeviceModel() { }

        /// <summary>
        /// Конструктор модели
        /// </summary>
        /// <param name="id">Id устройства</param>
        public DeviceModel(Guid id) : base(id) { }
    }
}
