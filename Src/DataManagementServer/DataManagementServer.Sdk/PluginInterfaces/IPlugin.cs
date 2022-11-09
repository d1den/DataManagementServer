﻿using System;

namespace DataManagementServer.Sdk.PluginInterfaces
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
        /// Инициализация плагина
        /// </summary>
        /// <param name="serviceProvider">Провайдер сервисов ядра</param>
        void Initialize(IServiceProvider serviceProvider);
    }
}