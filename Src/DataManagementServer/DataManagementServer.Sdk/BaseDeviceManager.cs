using DataManagementServer.Common.Models;
using DataManagementServer.Sdk.Channels;
using DataManagementServer.Sdk.PluginInterfaces;
using DataManagementServer.Sdk.Resources;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataManagementServer.Sdk
{
    /// <summary>
    /// Базовый менеджер устройств
    /// </summary>
    /// <typeparam name="TDevice">Тип устройства</typeparam>
    /// <typeparam name="TModel">Тип модели устройства</typeparam>
    public abstract class BaseDeviceManager<TDevice, TModel> : IDeviceManager
        where TDevice : IDevice
        where TModel : DeviceModel
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
        protected readonly ConcurrentDictionary<Guid, TDevice> _Devices = new();

        /// <summary>
        /// Уже уничтожен?
        /// </summary>
        protected bool _IsDisposed = false;

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
        protected abstract TDevice Create(DeviceModel model = default);


        public Guid CreateAndStart()
        {
            return CreateAndStart(null);
        }

        public Guid CreateAndStart(DeviceModel model)
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

        public string GetDeviceConfig(Guid id)
        {
            if (!_Devices.TryGetValue(id, out TDevice device))
            {
                throw new KeyNotFoundException(string.Format(Constants.DeviceNotExistError, id));
            }
            var model = device.ToModel();

            return JsonConvert.SerializeObject(model);
        }

        public void InitializeByConfig(string jsonDevicesConfig)
        {
            _ = jsonDevicesConfig ?? throw new ArgumentNullException(nameof(jsonDevicesConfig));

            var deviceModels = JsonConvert.DeserializeObject<List<TModel>>(jsonDevicesConfig);
            foreach (var deviceModel in deviceModels)
            {
                CreateAndStart(deviceModel);
            }
        }

        public void UpldateDeviceByConfig(string jsonDeviceConfig)
        {
            _ = jsonDeviceConfig ?? throw new ArgumentNullException(nameof(jsonDeviceConfig));

            var model = JsonConvert.DeserializeObject<TModel>(jsonDeviceConfig);
            UpdateDevice(model);
        }

        public void UpdateDevice(DeviceModel model)
        {
            _ = model ?? throw new ArgumentNullException(nameof(model));

            if (!_Devices.TryGetValue(model.Id, out var device))
            {
                throw new KeyNotFoundException(string.Format(Constants.DeviceNotExistError, model.Id));
            }
            device.Update(model);
        }

        public void StartDevice(Guid id)
        {
            if (!_Devices.TryGetValue(id, out TDevice device))
            {
                throw new KeyNotFoundException(string.Format(Constants.DeviceNotExistError, id));
            }

            device.Start();
        }

        public void StopDevice(Guid id)
        {
            if (!_Devices.TryGetValue(id, out TDevice device))
            {
                throw new KeyNotFoundException(string.Format(Constants.DeviceNotExistError, id));
            }

            device.Stop();
        }

        public void StopAll()
        {
            var tasks = _Devices.Values.Select(d => d.StopAsync()).ToArray();
            Task.WaitAll(tasks);
        }

        public bool TryGetDevice(Guid id, out DeviceModel model)
        {
            model = default;
            if (!_Devices.TryGetValue(id, out TDevice device))
            {
                return false;
            }

            model = device.ToModel();
            return true;
        }

        public DeviceModel GeDevice(Guid id)
        {
            if (!_Devices.TryGetValue(id, out TDevice device))
            {
                throw new KeyNotFoundException(string.Format(Constants.DeviceNotExistError, id));
            }

            return device.ToModel();
        }

        public List<DeviceModel> GetAll()
        {
            return _Devices.Values.Select(device => device.ToModel())
                .ToList();
        }

        public bool TryRemoveDevice(Guid id)
        {
            if (!_Devices.TryRemove(id, out var device))
            {
                return false;
            }

            device.Dispose();
            return true;
        }

        public virtual void Dispose()
        {
            if (_IsDisposed)
            {
                return;
            }

            foreach (var device in _Devices.Values)
            {
                device.Dispose();
            }
            _Devices.Clear();

            _IsDisposed = true;
        }
    }
}
