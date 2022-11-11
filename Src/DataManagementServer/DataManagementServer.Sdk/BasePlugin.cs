using DataManagementServer.Common.Models;
using DataManagementServer.Sdk.Channels;
using DataManagementServer.Sdk.PluginInterfaces;
using System;

namespace DataManagementServer.Sdk
{
    /// <summary>
    /// Базовый тип плагина
    /// </summary>
    /// <typeparam name="TDevice">Тип устройства</typeparam>
    /// <typeparam name="TDeviceModel">Тип модели устройства</typeparam>
    /// <typeparam name="TController">Тип контроллера api плагина</typeparam>
    public abstract class BasePlugin<TDevice, TDeviceModel, TController> : IPlugin
        where TDevice : IDevice
        where TDeviceModel : BaseDeviceModel
        where TController : BasePluginController
    {
        public Guid Id => Guid.NewGuid();

        public IDeviceManager DeviceManager => ConcreteDeviceManager;

        /// <summary>
        /// Сервис доступа к группам каналов
        /// </summary>
        protected IGroupService GroupService;

        /// <summary>
        /// Сервис доступа к каналам
        /// </summary>
        protected IChannelService ChannelService;

        /// <summary>
        /// Уже уничтожен?
        /// </summary>
        protected bool IsDisposed = false;

        /// <summary>
        /// Уже инициализирован?
        /// </summary>
        protected bool IsInitialize = false;

        /// <summary>
        /// Объект синхронизации потоков
        /// </summary>
        private readonly object _Lock = new ();

        #region Виртуальные и абстрактные методы
        public virtual void Dispose()
        {
            lock (_Lock)
            {
                if (IsDisposed)
                {
                    return;
                }

                DeviceManager?.Dispose();
                IsDisposed = true;
            }
        }

        public virtual void Initialize(IServiceProvider serviceProvider)
        {
            _ = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

            lock (_Lock)
            {
                if (IsInitialize)
                {
                    return;
                }

                GroupService = serviceProvider.GetService(typeof(IGroupService)) as IGroupService;
                ChannelService = serviceProvider.GetService(typeof(IChannelService)) as IChannelService;
                IsInitialize = true;
            }
        }

        public abstract PluginType Type { get; }

        /// <summary>
        /// Менеджер устройств плагина
        /// </summary>
        protected abstract BaseDeviceManager<TDevice, TDeviceModel> ConcreteDeviceManager { get; set; }
        #endregion
    }
}
