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

        public abstract PluginType Type { get; }

        public abstract IDeviceManager DeviceManager { get; }

        /// <summary>
        /// Уже уничтожен?
        /// </summary>
        protected bool _IsDisposed = false;

        public virtual void Dispose()
        {
            if (_IsDisposed)
            {
                return;
            }

            DeviceManager.Dispose();
            _IsDisposed = true;
        }

        public abstract void Initialize(IServiceProvider serviceProvider);
    }
}
