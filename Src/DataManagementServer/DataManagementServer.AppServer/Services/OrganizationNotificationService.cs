using DataManagementServer.AppServer.Hubs;
using DataManagementServer.Sdk.Channels;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Reactive.Linq;

namespace DataManagementServer.AppServer.Services
{
    /// <summary>
    /// Сервис уведомлений об изменениях в каналах и группах
    /// </summary>
    public class OrganizationNotificationService
    {
        /// <summary>
        /// Контекст хаба для уведомлений о группах и каналах
        /// </summary>
        private readonly IHubContext<OrganizationHub> _HubContext;

        /// <summary>
        /// Сервис групп
        /// </summary>
        private readonly IGroupService _GroupService;

        /// <summary>
        /// Сервис каналов
        /// </summary>
        private readonly IChannelService _ChannelService;

        /// <summary>
        /// Конструктор сервиса
        /// </summary>
        /// <param name="hubContext">Контекст хаба</param>
        /// <param name="groupService">Сервис групп</param>
        /// <param name="channelService">Сервис каналов</param>
        /// <exception cref="ArgumentNullException">Ошибка при Null параметре</exception>
        public OrganizationNotificationService(IHubContext<OrganizationHub> hubContext,
            IGroupService groupService, IChannelService channelService)
        {
            _HubContext = hubContext ?? throw new ArgumentNullException(nameof(hubContext));
            _GroupService = groupService ?? throw new ArgumentNullException(nameof(groupService));
            _ChannelService = channelService ?? throw new ArgumentNullException(nameof(channelService));

            // Выполняем подписку на все имеющиеся группы и каналы, и на изменения коллекций
            SubscribeOnExistingEntities();
            _GroupService.ObservableChange.Subscribe(GroupsCollectionChanged);
            _ChannelService.ObservableChange.Subscribe(ChannelsCollectionChanged);
        }

        /// <summary>
        /// Подписка на все имеющиеся группы и каналы
        /// </summary>
        private void SubscribeOnExistingEntities()
        {
            var groups = _GroupService.RetrieveAll();
            foreach (var group in groups)
            {
                _GroupService.GetObservableUpdate(group.Id).Subscribe(GroupUpdated);
            }

            var channels = _ChannelService.RetrieveAll(false);
            foreach(var channel in channels)
            {
                _ChannelService.GetObservableUpdate(channel.Id).Subscribe(ChannelUpdated);
            }
        }

        /// <summary>
        /// Обработчик события изменения коллекции групп
        /// </summary>
        /// <param name="e">Аргументы события</param>
        private async void GroupsCollectionChanged(CollectionChangeEventArgs e)
        {
            if (e == null)
            {
                return;
            }

            // Если в коллекцию добавился элемент, то подписываемся на его изменения
            if (e.Type == CollectionChangeType.AddElement)
            {
                _GroupService.GetObservableUpdate(e.Id).Subscribe(GroupUpdated);
            }
            // Если элемент удалился, то подписки автоматически деактивируются в сервисе

            await _HubContext.Clients.All.SendAsync(nameof(GroupsCollectionChanged), e);
        }

        /// <summary>
        /// Обработчик события изменения группы
        /// </summary>
        /// <param name="e">Аргументы события</param>
        private async void GroupUpdated(UpdateEventArgs e)
        {
            await _HubContext.Clients.All.SendAsync(nameof(GroupUpdated), e);
        }


        /// <summary>
        /// Обработчик события изменения коллекции каналов
        /// </summary>
        /// <param name="e">Аргументы события</param>
        private async void ChannelsCollectionChanged(CollectionChangeEventArgs e)
        {
            if (e == null)
            {
                return;
            }

            // Если в коллекцию добавился элемент, то подписываемся на его изменения
            if (e.Type == CollectionChangeType.AddElement)
            {
                _ChannelService.GetObservableUpdate(e.Id).Subscribe(ChannelUpdated);
            }
            // Если элемент удалился, то подписки автоматически деактивируются в сервисе

            await _HubContext.Clients.All.SendAsync(nameof(ChannelsCollectionChanged), e);
        }

        /// <summary>
        /// Обработчик события изменения канала
        /// </summary>
        /// <param name="e">Аргументы события</param>
        private async void ChannelUpdated(UpdateEventArgs e)
        {
            await _HubContext.Clients.All.SendAsync(nameof(ChannelUpdated), e);
        }
    }
}
