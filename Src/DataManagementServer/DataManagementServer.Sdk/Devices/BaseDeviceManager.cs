using DataManagementServer.Common.Models;
using DataManagementServer.Sdk.Channels;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataManagementServer.Sdk.Devices
{
    /// <summary>
    /// Базовый менеджер устройств
    /// </summary>
    /// <typeparam name="TDevice">Тип устройства</typeparam>
    /// <typeparam name="TModel">Тип модели устройства</typeparam>
    public abstract class BaseDeviceManager<TDevice, TModel> : IDeviceManager
        where TDevice : IDevice
        where TModel : BaseDeviceModel
    {
        #region Зависимости
        /// <summary>
        /// Сервис доступа к группам каналов
        /// </summary>
        protected readonly IGroupService GroupService;

        /// <summary>
        /// Сервис доступа к каналам
        /// </summary>
        protected readonly IChannelService ChannelService;
        #endregion

        /// <summary>
        /// Устройства
        /// </summary>
        protected readonly ConcurrentDictionary<Guid, TDevice> _Devices = new ();

        public int Count => _Devices.Count;

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="groupService">Сервис доступа к группам каналов</param>
        /// <param name="channelService">Сервис доступа к каналам</param>
        /// <exception cref="ArgumentNullException">Ошибка при null параметре</exception>
        public BaseDeviceManager(IGroupService groupService, IChannelService channelService)
        {
            GroupService = groupService ?? throw new ArgumentNullException(nameof(groupService));
            ChannelService = channelService ?? throw new ArgumentNullException(nameof(channelService));
        }

        /// <summary>
        /// Метод создания устройства на основе модели или без
        /// </summary>
        /// <param name="model">Модель устройства</param>
        /// <returns>Устройство</returns>
        protected abstract TDevice Create(BaseDeviceModel model = default);


        public Guid CreateAndStart()
        {
            return CreateAndStart(null);
        }

        public Guid CreateAndStart(BaseDeviceModel model)
        {
            var device = Create(model);
            _Devices.TryAdd(device.Id, device);
            device.Start();

            return device.Id;
        }

        public string GetConfig()
        {
            var deviceModels = _Devices.Values.Select(d => d.ToModel()).ToList();
            return JsonConvert.SerializeObject(deviceModels);
        }

        public void InitializeByConfig(string jsonDevicesConfig)
        {
            var deviceModels = JsonConvert.DeserializeObject<List<TModel>>(jsonDevicesConfig);
            foreach (var deviceModel in deviceModels)
            {
                CreateAndStart(deviceModel);
            }
        }

        public void Start(Guid id)
        {
            if (_Devices.TryGetValue(id, out TDevice device))
            {
                device.Start();
            }
        }

        public void Stop(Guid id)
        {
            if (_Devices.TryGetValue(id, out TDevice device))
            {
                device.Stop();
            }
        }

        public void StopAll()
        {
            var tasks = _Devices.Values.Select(d => d.StopAsync()).ToArray();
            Task.WaitAll(tasks);
        }

        public bool TryRetrieve(Guid id, out BaseDeviceModel model)
        {
            model = default;
            if (_Devices.TryGetValue(id, out TDevice device))
            {
                model = device.ToModel() as TModel;
                return true;
            }

            return false;
        }

        public bool TryRemove(Guid id)
        {
            return _Devices.TryRemove(id, out _);
        }
    }
}
