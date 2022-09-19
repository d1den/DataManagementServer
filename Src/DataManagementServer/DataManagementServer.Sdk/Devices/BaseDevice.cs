using DataManagementServer.Common.Models;
using DataManagementServer.Sdk.Channels;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DataManagementServer.Sdk.Devices
{
    /// <summary>
    /// Базовый класс устройства
    /// </summary>
    public abstract class BaseDevice : IDevice
    {
        #region Константы
        /// <summary>
        /// Базовое название устройства
        /// </summary>
        public const string BaseDeviceName = "NewDevice";

        /// <summary>
        /// Базовый период опроса = 250 мс
        /// </summary>
        public const int BasePollingPeriod = 250;
        #endregion

        #region Свойства
        public Guid Id { get; }

        public string Name { get; protected set; }

        public DeviceStatus Status { get; protected set; }

        public int PollingPeriod { get; protected set; }
        #endregion

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
        /// Источник токенов отмены
        /// </summary>
        private CancellationTokenSource _CancellationTokenSource;

        /// <summary>
        /// Рабочая задача
        /// </summary>
        private Task _WorkTask;

        /// <summary>
        /// Объект для синхронизации потоков
        /// </summary>
        protected readonly object _Mutex = new object();

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="groupService">Сервис доступа к группам каналов</param>
        /// <param name="channelService">Сервис доступа к каналам</param>
        /// <exception cref="ArgumentNullException">Ошибка при null параметре</exception>
        public BaseDevice(IGroupService groupService, IChannelService channelService)
        {
            GroupService = groupService ?? throw new ArgumentNullException(nameof(groupService));
            ChannelService = channelService ?? throw new ArgumentNullException(nameof(channelService));
            Id = Guid.NewGuid();
            Status = DeviceStatus.Created;
            Name = BaseDeviceName;
            PollingPeriod = BasePollingPeriod;
        }

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="groupService">Сервис доступа к группам каналов</param>
        /// <param name="channelService">Сервис доступа к каналам</param>
        /// <param name="model">Модель устройства</param>
        /// <exception cref="ArgumentNullException">Ошибка при null параметре</exception>
        public BaseDevice(IGroupService groupService, IChannelService channelService, BaseDeviceModel model)
        {
            GroupService = groupService ?? throw new ArgumentNullException(nameof(groupService));
            ChannelService = channelService ?? throw new ArgumentNullException(nameof(channelService));
            _ = model ?? throw new ArgumentNullException(nameof(model));
            Id = model.Id == default ? Guid.NewGuid() : model.Id;
            Name = model.Name == default ? BaseDeviceName : model.Name;
            Status = DeviceStatus.Created;
            PollingPeriod = model.PollingPeriod == default ? BasePollingPeriod : model.PollingPeriod;
        }

        public void Start()
        {
            if (Status == DeviceStatus.Runnig 
                && _WorkTask != null && _WorkTask.Status == TaskStatus.Running)
            {
                return;
            }
            _CancellationTokenSource = new CancellationTokenSource();
            var token = _CancellationTokenSource.Token;
            Status = DeviceStatus.Runnig;
            _WorkTask = Task.Run(() => DoWork(token), token);
        }

        public void Stop()
        {
            try
            {
                if (Status == DeviceStatus.Stoped
                    && _WorkTask == null 
                    && (_WorkTask.Status == TaskStatus.Canceled || _WorkTask.Status == TaskStatus.RanToCompletion))
                {
                    return;
                }
                _CancellationTokenSource?.Cancel();
                _WorkTask?.Wait();
                Status = DeviceStatus.Stoped;
            }
            catch(AggregateException ex)
            {
                ex.Handle(e => e is OperationCanceledException);
            }
        }

        public Task StopAsync()
        {
            _CancellationTokenSource?.Cancel();
            Status = DeviceStatus.Stoped;
            return _WorkTask;
        }

        /// <summary>
        /// Выполнение работы устройства в своём потоке
        /// </summary>
        /// <param name="cancellationToken">Токен отмены выполнения работы</param>
        private void DoWork(CancellationToken cancellationToken)
        {
            try
            {
                Initialize(cancellationToken);
                while (true)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    IterateWorkLoop(cancellationToken);
                    Task.Delay(PollingPeriod, cancellationToken).Wait();
                }
            }
            catch (OperationCanceledException)
            {

            }
            catch (Exception)
            {
                Status = DeviceStatus.Error;
                throw;
            }
        }

        /// <summary>
        /// Инициализация устройства
        /// </summary>
        /// <param name="cancellationToken">Токен отмены выполнения работы</param>
        protected abstract void Initialize(CancellationToken cancellationToken);

        /// <summary>
        /// Выполнить одну итерацию работы устройства в своём потоке
        /// </summary>
        /// <param name="cancellationToken">Токен отмены выполнения работы</param>
        protected abstract void IterateWorkLoop(CancellationToken cancellationToken);

        public virtual BaseDeviceModel ToModel()
        {
            lock (_Mutex)
            {
                return ToModelWithSync();
            }
        }

        /// <summary>
        /// Получение модели с синхронизацией доступа
        /// </summary>
        /// <returns>Модель устройства</returns>
        protected abstract BaseDeviceModel ToModelWithSync();

        public virtual void Update(BaseDeviceModel model)
        {
            lock (_Mutex)
            {
                UpdateWithSync(model);
            }
        }

        /// <summary>
        /// Обновление устройства с синхронизацией доступа
        /// </summary>
        /// <param name="model">Модель устройства</param>
        protected abstract void UpdateWithSync(BaseDeviceModel model);
    }
}
