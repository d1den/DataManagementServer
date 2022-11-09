using DataManagementServer.Common.Models;
using System;

namespace DataManagementServer.Sdk.PluginInterfaces
{
    /// <summary>
    /// Менеджер устройств
    /// </summary>
    public interface IDeviceManager : IDisposable
    {
        /// <summary>
        /// Количество устройств
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Создать устройство и запустить
        /// </summary>
        /// <returns>Id устройства</returns>
        Guid CreateAndStart();

        /// <summary>
        /// Создать устройство и запустить
        /// </summary>
        /// <param name="model">Модель устройства</param>
        /// <returns>Id устройства</returns>
        Guid CreateAndStart(BaseDeviceModel model);

        /// <summary>
        /// Инициализировать устройства по json строке
        /// </summary>
        /// <param name="jsonDevicesConfig">Json строка с конфигурацией</param>
        void InitializeByConfig(string jsonDevicesConfig);

        /// <summary>
        /// Обновить устройство по конфигу устройства
        /// </summary>
        /// <param name="jsonDeviceConfig">Json строка с конфигурацией</param>
        void UpldateDeviceByConfig(string jsonDeviceConfig);

        /// <summary>
        /// Получить строку с конфигурацией устройств
        /// </summary>
        /// <returns>Json строка с конфигурацией</returns>
        string GetConfig();

        /// <summary>
        /// Получить конфигурацию устройства
        /// </summary>
        /// <param name="id">Id устройства</param>
        /// <returns>Json строка с конфигурацией</returns>
        string GetDeviceConfig(Guid id);

        /// <summary>
        /// Попытка удаления устройства
        /// </summary>
        /// <param name="id">Id устройства</param>
        /// <returns>Результат удаления</returns>
        bool TryRemove(Guid id);

        /// <summary>
        /// Попытка получения устройства
        /// </summary>
        /// <param name="id">Id устройства</param>
        /// <param name="model">Модель устройства</param>
        /// <returns>Результат получения</returns>
        bool TryRetrieve(Guid id, out BaseDeviceModel model);

        /// <summary>
        /// Запустить устройство
        /// </summary>
        /// <param name="id">Id устройства</param>
        void Start(Guid id);

        /// <summary>
        /// Остановить устройство
        /// </summary>
        /// <param name="id">Id устройства</param>
        void Stop(Guid id);

        /// <summary>
        /// Остановить все устройства
        /// </summary>
        void StopAll();
    }
}
