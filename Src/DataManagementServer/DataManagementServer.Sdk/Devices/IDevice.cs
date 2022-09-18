using DataManagementServer.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataManagementServer.Sdk.Devices
{
    /// <summary>
    /// Интерфейс устройства плагина
    /// </summary>
    public interface IDevice
    {
        /// <summary>
        /// Id устройства
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// Название
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Статус устройства
        /// </summary>
        DeviceStatus Status { get; }

        /// <summary>
        /// Период опроса устройства, мс
        /// </summary>
        int PollingPeriod { get; }

        /// <summary>
        /// Начать работу
        /// </summary>
        void Start();

        /// <summary>
        /// Закончить работу
        /// </summary>
        void Stop();

        /// <summary>
        /// Асинхронное завершение работы
        /// </summary>
        /// <returns>Задача выполнение работы</returns>
        Task StopAsync();

        /// <summary>
        /// Обновить устройство
        /// </summary>
        /// <param name="model">Базовая модель устройства</param>
        void Update(BaseDeviceModel model);

        /// <summary>
        /// Получить модель устройства
        /// </summary>
        /// <returns>Базовая модель устройства</returns>
        BaseDeviceModel ToModel();
    }
}
