﻿using DataManagementServer.Common.Models;
using DataManagementServer.Core.Channels;
using DataManagementServer.Sdk.Channels;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;

namespace DataManagementServer.Core.Services
{
    /// <summary>
    /// Сервис организации работы с каналами и группам
    /// </summary>
    public class OrganizationService : IGroupService, IChannelService, IDisposable
    {
        /// <summary>
        /// Шаблон ошибки об отсуствии группы с запрашиваемым Id
        /// </summary>
        private const string _GroupNotExistErrorTemplate = "Group with id = {0} not exist!";

        /// <summary>
        /// Шаблон ошибки об отсуствии канала с запрашиваемым Id
        /// </summary>
        private const string _ChannelNotExistErrorTemplate = "Channel with id = {0} not exist!";

        /// <summary>
        /// Словарь групп
        /// </summary>
        private readonly ConcurrentDictionary<Guid, Group> _Groups = new ();

        /// <summary>
        /// Событие изменения коллекции групп
        /// </summary>
        private event EventHandler<CollectionChangeEventArgs> _GroupsChangeEvent;

        /// <summary>
        /// Уведомитель об изменении коллекции групп
        /// </summary>
        private readonly IObservable<EventPattern<CollectionChangeEventArgs>> _GroupsObservable;

        /// <summary>
        /// Словарь каналов
        /// </summary>
        private readonly ConcurrentDictionary<Guid, Channel> _Channels = new ();

        /// <summary>
        /// Событие изменения коллекции каналов
        /// </summary>
        private event EventHandler<CollectionChangeEventArgs> _ChannelsChangeEvent;

        /// <summary>
        /// Уведомитель об изменении коллекции каналов
        /// </summary>
        private readonly IObservable<EventPattern<CollectionChangeEventArgs>> _ChannelsObservable;

        /// <summary>
        /// Уже уничтожен?
        /// </summary>
        private bool _IsDisposed = false;

        /// <summary>
        /// Конструктор
        /// </summary>
        public OrganizationService()
        {
            _GroupsObservable = Observable
                .FromEventPattern<CollectionChangeEventArgs>(
                handler => _GroupsChangeEvent += handler,
                handler => _GroupsChangeEvent -= handler);

            _ChannelsObservable = Observable
                .FromEventPattern<CollectionChangeEventArgs>(
                handler => _ChannelsChangeEvent += handler,
                handler => _ChannelsChangeEvent -= handler);
        }

        public void Dispose()
        {
            if (_IsDisposed)
            {
                return;
            }

            foreach(var group in _Groups.Values)
            {
                group.Dispose();
            }

            foreach(var channel in _Channels.Values)
            {
                channel.Dispose();
            }

            _IsDisposed = true;
        }

        #region Groups
        int IGroupService.Count => _Groups.Count;

        Guid IGroupService.RootId => Guid.Empty;

        IObservable<EventPattern<CollectionChangeEventArgs>> IGroupService.ObservableChange => _GroupsObservable;

        #region Create
        Guid IGroupService.Create()
        {
            var group = new Group();
            _Groups[group.Id] = group;

            _GroupsChangeEvent?.Invoke(this,
                new CollectionChangeEventArgs(group.Id, CollectionChangeType.AddElement));

            return group.Id;
        }

        Guid IGroupService.Create(Guid parentId)
        {
            var model = new GroupModel() { ParentId = parentId };
            var group = new Group(model);
            _Groups[group.Id] = group;

            _GroupsChangeEvent?.Invoke(this,
                new CollectionChangeEventArgs(group.Id, CollectionChangeType.AddElement));

            return group.Id;
        }

        Guid IGroupService.Create(GroupModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            var group = new Group(model);
            _Groups[group.Id] = group;

            _GroupsChangeEvent?.Invoke(this,
                new CollectionChangeEventArgs(group.Id, CollectionChangeType.AddElement));

            return group.Id;
        }
        #endregion

        #region Retrieve
        GroupModel IGroupService.Retrieve(Guid id)
        {
            if (_Groups.TryGetValue(id, out var group))
            {
                return group.ToModel(true);
            }
            throw new KeyNotFoundException(string.Format(_GroupNotExistErrorTemplate, id));
        }

        List<GroupModel> IGroupService.RetrieveAll(bool allFields)
        {
            return _Groups
                .Select(pair => pair.Value.ToModel(allFields)).ToList();
        }

        List<GroupModel> IGroupService.RetrieveByParent(Guid parentId, bool allFields)
        {
            return _Groups
                .Where(pair => pair.Value.ParentId == parentId)
                .Select(pair => pair.Value.ToModel(allFields)).ToList();
        }
        #endregion

        #region Update
        void IGroupService.Update(GroupModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            if (_Groups.TryGetValue(model.Id, out Group group))
            {
                group.Update(model);
                return;
            }

            throw new KeyNotFoundException(string.Format(_GroupNotExistErrorTemplate, model.Id));
        }
        #endregion

        #region Delete
        void IGroupService.Delete(Guid id, bool withChildren)
        {
            if (!_Groups.Remove(id, out Group baseGroup))
            {
                throw new KeyNotFoundException(string.Format(_GroupNotExistErrorTemplate, id));
            }
            baseGroup.Dispose();

            _GroupsChangeEvent?.Invoke(this,
                new CollectionChangeEventArgs(id, CollectionChangeType.DeleteElement));

            if (withChildren)
            {
                var deleteChannels = _Channels
                    .Select(pair => pair.Value)
                    .Where(channel => channel.GroupId == id);
                foreach (var channel in deleteChannels)
                {
                    (this as IChannelService).Delete(channel.Id);
                }

                var deleteGroups = _Groups
                    .Select(pair => pair.Value)
                    .Where(group => group.ParentId == id);
                foreach (var group in deleteGroups)
                {
                    (this as IGroupService).Delete(group.Id);
                }
            }
        }
        #endregion

        IObservable<EventPattern<UpdateEventArgs>> IGroupService.GetObservableUpdate(Guid id)
        {
            if (_Groups.TryGetValue(id, out var group))
            {
                return group.ObservableUpdate;
            }
            throw new KeyNotFoundException(string.Format(_GroupNotExistErrorTemplate, id));
        }

        void IGroupService.SubscribeOnUpdate(Guid id, Action<UpdateEventArgs> handler, CancellationToken token)
        {
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }
            if (_Groups.TryGetValue(id, out var group))
            {
                group.ObservableUpdate
                    .Select(data => data.EventArgs)
                    .Subscribe(handler, token);
                return;
            }
            throw new KeyNotFoundException(string.Format(_GroupNotExistErrorTemplate, id));
        }

        void IGroupService.SubscribeOnCollectionChange(Action<CollectionChangeEventArgs> handler, CancellationToken token)
        {
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }
            (this as IGroupService).ObservableChange
                .Select(data => data.EventArgs)
                .Subscribe(handler, token);
        }
        #endregion

        #region Channels
        int IChannelService.Count => _Channels.Count;

        IObservable<EventPattern<CollectionChangeEventArgs>> IChannelService.ObservableChange => _ChannelsObservable;

        #region Create
        Guid IChannelService.Create()
        {
            var channel = new Channel();
            _Channels[channel.Id] = channel;

            _ChannelsChangeEvent?.Invoke(this,
                new CollectionChangeEventArgs(channel.Id, CollectionChangeType.AddElement));

            return channel.Id;
        }

        Guid IChannelService.Create(Guid groupId)
        {
            var model = new ChannelModel() { GroupId = groupId };
            var channel = new Channel(model);
            _Channels[channel.Id] = channel;

            _ChannelsChangeEvent?.Invoke(this,
                new CollectionChangeEventArgs(channel.Id, CollectionChangeType.AddElement));

            return channel.Id;
        }

        Guid IChannelService.Create(ChannelModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            var channel = new Channel(model);
            _Channels[channel.Id] = channel;

            _ChannelsChangeEvent?.Invoke(this,
                new CollectionChangeEventArgs(channel.Id, CollectionChangeType.AddElement));

            return channel.Id;
        }
        #endregion

        #region Retrieve
        ChannelModel IChannelService.Retrieve(Guid id, bool allFields)
        {
            if (_Channels.TryGetValue(id, out var channel))
            {
                return channel.ToModel(allFields);
            }
            throw new KeyNotFoundException(string.Format(_ChannelNotExistErrorTemplate, id));
        }

        ChannelModel IChannelService.Retrieve(Guid id, params string[] fields)
        {
            if (_Channels.TryGetValue(id, out var channel))
            {
                return channel.ToModel(fields);
            }
            throw new KeyNotFoundException(string.Format(_ChannelNotExistErrorTemplate, id));
        }

        List<ChannelModel> IChannelService.RetrieveAll(bool allFields)
        {
            return _Channels
                .Select(pair => pair.Value.ToModel(allFields)).ToList();
        }

        List<ChannelModel> IChannelService.RetrieveAll(params string[] fields)
        {
            return _Channels
                .Select(pair => pair.Value.ToModel(fields)).ToList();
        }

        List<ChannelModel> IChannelService.RetrieveByGroup(Guid groupId, bool allFields)
        {
            return _Channels
                .Where(pair => pair.Value.GroupId == groupId)
                .Select(pair => pair.Value.ToModel(allFields)).ToList();
        }

        List<ChannelModel> IChannelService.RetrieveByGroup(Guid groupId, params string[] fields)
        {
            return _Channels
                .Where(pair => pair.Value.GroupId == groupId)
                .Select(pair => pair.Value.ToModel(fields)).ToList();
        }

        object IChannelService.RetrieveValue(Guid id)
        {
            if (_Channels.TryGetValue(id, out var channel))
            {
                return channel.Value;
            }
            throw new KeyNotFoundException(string.Format(_ChannelNotExistErrorTemplate, id));
        }

        T IChannelService.RetrieveValue<T>(Guid id)
        {
            if (_Channels.TryGetValue(id, out var channel))
            {
                return channel.GetValue<T>();
            }
            throw new KeyNotFoundException(string.Format(_ChannelNotExistErrorTemplate, id));
        }

        bool IChannelService.TryRetrieveValue<T>(Guid id, out T value)
        {
            value = default;
            if (_Channels.TryGetValue(id, out var channel))
            {
                return channel.TryGetValue(out value);
            }
            return false;
        }
        #endregion

        #region Update
        void IChannelService.Update(ChannelModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            if (_Channels.TryGetValue(model.Id, out var channel))
            {
                channel.Update(model);
                return;
            }
            throw new KeyNotFoundException(string.Format(_ChannelNotExistErrorTemplate, model.Id));
        }

        void IChannelService.UpdateValue(Guid id, object value)
        {
            if (_Channels.TryGetValue(id, out var channel))
            {
                channel.UpdateValue(value);
                return;
            }
            throw new KeyNotFoundException(string.Format(_ChannelNotExistErrorTemplate, id));
        }
        #endregion

        #region Delete
        void IChannelService.Delete(Guid id)
        {
            if (!_Channels.TryRemove(id, out var channel))
            {
                throw new KeyNotFoundException(string.Format(_ChannelNotExistErrorTemplate, id));
            }
            channel.Dispose();

            _ChannelsChangeEvent?.Invoke(this,
                new CollectionChangeEventArgs(channel.Id, CollectionChangeType.DeleteElement));
        }
        #endregion

        IObservable<EventPattern<UpdateEventArgs>> IChannelService.GetObservableUpdate(Guid id)
        {
            if (_Channels.TryGetValue(id, out var channel))
            {
                return channel.ObservableUpdate;
            }
            throw new KeyNotFoundException(string.Format(_ChannelNotExistErrorTemplate, id));
        }

        void IChannelService.SubscribeOnUpdate(Guid id, Action<UpdateEventArgs> handler, CancellationToken token)
        {
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }
            if (_Channels.TryGetValue(id, out var channel))
            {
                channel.ObservableUpdate
                    .Select(data => data.EventArgs)
                    .Subscribe(handler, token);
                return;
            }
            throw new KeyNotFoundException(string.Format(_ChannelNotExistErrorTemplate, id));
        }

        void IChannelService.SubscribeOnCollectionChange(Action<CollectionChangeEventArgs> handler, CancellationToken token)
        {
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }
            (this as IChannelService).ObservableChange
                .Select(data => data.EventArgs)
                .Subscribe(handler, token);
        }
        #endregion
    }
}