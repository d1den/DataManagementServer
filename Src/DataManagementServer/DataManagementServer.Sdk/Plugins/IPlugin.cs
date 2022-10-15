using DataManagementServer.Sdk.Devices;
using System;

namespace DataManagementServer.Sdk.Plugins
{
    /// <summary>
    /// Интерфейс плагина системы
    /// </summary>
    public interface IPlugin : IDisposable
    {
        /// <summary>
        /// Id плагина
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// Тип плагина
        /// </summary>
        PluginType Type { get; }

        /// <summary>
        /// Менеджер устройств
        /// </summary>
        IDeviceManager DeviceManager { get; }

        /// <summary>
        /// Создать плагин
        /// </summary>
        /// <param name="serviceProvider">Провайдер сервисов ядра</param>
        void Initialize(IServiceProvider serviceProvider);
    }
}
