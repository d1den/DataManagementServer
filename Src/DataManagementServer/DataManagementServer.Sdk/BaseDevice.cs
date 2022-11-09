using DataManagementServer.Common.Models;
using DataManagementServer.Sdk.Channels;
using DataManagementServer.Sdk.Resources;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DataManagementServer.Sdk
{
    /// <summary>
    /// Базовый класс устройства
    /// </summary>
    public abstract class BaseDevice : IDevice
    {
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
        protected readonly ReaderWriterLockSlim Lock = new();

        /// <summary>
        /// Объект синхронизации метода Dispose
        /// </summary>
        private readonly object _DisposeLock = new();

        /// <summary>
        /// Уже уничтожен?
        /// </summary>
        protected bool IsDisposed = false;

        /// <summary>
        /// Базовый период опроса = 250 мс
        /// </summary>
        public const int BasePollingPeriod = 250;

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
            Name = Constants.DefaultDeviceName;
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
            Name = model.Name == default ? Constants.DefaultDeviceName : model.Name;
            Status = DeviceStatus.Created;
            PollingPeriod = model.PollingPeriod == default ? BasePollingPeriod : model.PollingPeriod;
        }

        public void Start()
        {
            Lock.EnterWriteLock();
            try
            {
                if (Status == DeviceStatus.Runnig
                    && _WorkTask?.Status == TaskStatus.Running)
                {
                    return;
                }

                _CancellationTokenSource = new CancellationTokenSource();
                var token = _CancellationTokenSource.Token;
                Status = DeviceStatus.Runnig;
                _WorkTask = Task.Run(() => DoWork(token), token);
                _WorkTask.ConfigureAwait(false);
            }
            finally
            {
                Lock.ExitWriteLock();
            }
        }

        public void Stop()
        {
            Lock.EnterWriteLock();
            try
            {
                if (Status == DeviceStatus.Stoped
                    && (_WorkTask == null || _WorkTask.Status == TaskStatus.Canceled
                    || _WorkTask.Status == TaskStatus.RanToCompletion))
                {
                    return;
                }

                _CancellationTokenSource?.Cancel();
                _WorkTask?.Wait();
                Status = DeviceStatus.Stoped;
            }
            catch (AggregateException ex)
            {
                ex.Handle(e => e is OperationCanceledException);
            }
            finally
            {
                Lock.ExitWriteLock();
            }
        }

        public Task StopAsync()
        {
            Lock.EnterWriteLock();
            try
            {
                _CancellationTokenSource?.Cancel();
                Status = DeviceStatus.Stoped;

                return _WorkTask;
            }
            finally
            {
                Lock.ExitWriteLock();
            }
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
                    Thread.Sleep(PollingPeriod);
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
            Lock.EnterReadLock();
            try
            {
                return ToModelWithSync();
            }
            finally
            {
                Lock.ExitReadLock();
            }
        }

        /// <summary>
        /// Получение модели с синхронизацией доступа
        /// </summary>
        /// <returns>Модель устройства</returns>
        protected abstract BaseDeviceModel ToModelWithSync();

        public virtual void Update(BaseDeviceModel model)
        {
            Lock.EnterWriteLock();
            try
            {
                UpdateWithSync(model);
            }
            finally
            {
                Lock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Обновление устройства с синхронизацией доступа
        /// </summary>
        /// <param name="model">Модель устройства</param>
        protected abstract void UpdateWithSync(BaseDeviceModel model);

        public virtual void Dispose()
        {
            lock (_DisposeLock)
            {
                if (IsDisposed)
                {
                    return;
                }

                if (_CancellationTokenSource != null)
                {
                    _CancellationTokenSource.Dispose();
                    _CancellationTokenSource = null;
                }
                if (_WorkTask != null)
                {
                    _WorkTask.Dispose();
                    _WorkTask = null;
                }
                Lock.Dispose();

                IsDisposed = true;
            }
        }
    }
}
