using DataManagementServer.Sdk.Devices;
using System;

namespace DataManagementServer.Sdk.Plugins
{
    /// <summary>
    /// Базовый тип плагина
    /// </summary>
    public abstract class BasePlugin : IPlugin
    {
        public Guid Id => Guid.NewGuid();

        public abstract string Name { get; }

        public abstract PluginType Type { get; }

        public abstract IDeviceManager DeviceManager { get; }

        public abstract void Initialize(IServiceProvider serviceProvider);
    }
}
