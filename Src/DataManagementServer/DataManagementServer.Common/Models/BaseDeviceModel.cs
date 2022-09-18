using System;

namespace DataManagementServer.Common.Models
{
    public class BaseDeviceModel : BaseModel
    {
        /// <summary>
        /// Id устройства
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Название устройства
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Статус устройства
        /// </summary>
        public DeviceStatus Status { get; set; }

        /// <summary>
        /// Период опроса устройства, мс
        /// </summary>
        public int PollingPeriod { get; set; }
    }
}
