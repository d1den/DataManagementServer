using DataManagementServer.Common.Models;
using DataManagementServer.Core.Channels;
using DataManagementServer.Core.Resources;
using DataManagementServer.Sdk.Channels;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;

namespace DataManagementServer.Core.Services.Concrete
{
    /// <summary>
    /// Сервис организации работы с каналами и группам
    /// </summary>
    public class OrganizationService : IGroupService, IChannelService, IDisposable
    {
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
        private readonly IObservable<CollectionChangeEventArgs> _GroupsObservable;

        /// <summary>
        /// Соединение уведомителя коллекции групп
        /// </summary>
        private readonly IDisposable _GroupsObservableConnection;

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
        private readonly IObservable<CollectionChangeEventArgs> _ChannelsObservable;

        /// <summary>
        /// Соединение уведомителя коллекции каналов
        /// </summary>
        private readonly IDisposable _ChannelsObservableConnection;

        /// <summary>
        /// Уже уничтожен?
        /// </summary>
        private bool _IsDisposed = false;

        /// <summary>
        /// Конструктор
        /// </summary>
        public OrganizationService()
        {
            _Groups.AddOrUpdate(Guid.Empty, Group.RootGroup, (id, old) => Group.RootGroup);
            
            // Созданём уведомителя об обновлении с возможностью его отключения
            var groupsConnectableObservable = Observable.FromEventPattern<CollectionChangeEventArgs>(
                handler => _GroupsChangeEvent += handler,
                handler => _GroupsChangeEvent -= handler).Select(e => e.EventArgs)
                .Publish();
            _GroupsObservableConnection = groupsConnectableObservable.Connect();
            _GroupsObservable = groupsConnectableObservable;

            var cchannelCnnectableObservable = Observable.FromEventPattern<CollectionChangeEventArgs>(
                handler => _ChannelsChangeEvent += handler,
                handler => _ChannelsChangeEvent -= handler).Select(e => e.EventArgs)
                .Publish();
            _ChannelsObservableConnection = cchannelCnnectableObservable.Connect();
            _ChannelsObservable = cchannelCnnectableObservable;
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
            _Groups.Clear();
            _GroupsObservableConnection?.Dispose();

            foreach(var channel in _Channels.Values)
            {
                channel.Dispose();
            }
            _Channels.Clear();
            _ChannelsObservableConnection?.Dispose();

            _IsDisposed = true;
        }

        #region Groups
        int IGroupService.Count => _Groups.Count;

        IObservable<CollectionChangeEventArgs> IGroupService.ObservableChange => _GroupsObservable;

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
            (this as IGroupService).ExistOrTrown(parentId);
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

            (this as IGroupService).ExistOrTrown(model.ParentId ?? Guid.Empty);

            var group = new Group(model);
            _Groups[group.Id] = group;

            _GroupsChangeEvent?.Invoke(this,
                new CollectionChangeEventArgs(group.Id, CollectionChangeType.AddElement));

            return group.Id;
        }
        #endregion

        #region
        void IGroupService.ExistOrTrown(Guid groupId)
        {
            (this as IGroupService).Retrieve(groupId);
        }
        #endregion
        #region Retrieve
        GroupModel IGroupService.Retrieve(Guid id)
        {
            if (_Groups.TryGetValue(id, out var group))
            {
                return group.ToModel(true);
            }
            throw new KeyNotFoundException(string.Format(ErrorMessages.GroupNotExistError, id));
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

            throw new KeyNotFoundException(string.Format(ErrorMessages.GroupNotExistError, model.Id));
        }
        #endregion

        #region Delete
        void IGroupService.Delete(Guid id, bool withChildren)
        {
            if(Group.RootGroup.Id == id)
            {
                throw new ArgumentException(ErrorMessages.ForbiddenToDeleteRootGroup);
            }
            if (!_Groups.Remove(id, out Group baseGroup))
            {
                throw new KeyNotFoundException(string.Format(ErrorMessages.GroupNotExistError, id));
            }
            baseGroup.Dispose();

            var childGroups = _Groups
                    .Select(pair => pair.Value)
                    .Where(group => group.ParentId == id);

            var childChannels = _Channels
                    .Select(pair => pair.Value)
                    .Where(channel => channel.GroupId == id);

            foreach (var group in childGroups)
            {
                if (withChildren)
                {
                    (this as IGroupService).Delete(group.Id, withChildren);
                }
                else
                {
                    var model = group.ToModel();
                    model.ParentId = Group.RootGroup.Id;

                    (this as IGroupService).Update(model);
                }
            }
            foreach (var channel in childChannels)
            {
                if (withChildren)
                {
                    (this as IChannelService).Delete(channel.Id);
                }
                else
                {
                    var model = channel.ToModel();
                    model.GroupId = Group.RootGroup.Id;

                    (this as IChannelService).Update(model);
                }
            }

            _GroupsChangeEvent?.Invoke(this,
                new CollectionChangeEventArgs(id, CollectionChangeType.DeleteElement));
        }
        #endregion

        IObservable<UpdateEventArgs> IGroupService.GetObservableUpdate(Guid id)
        {
            if (_Groups.TryGetValue(id, out var group))
            {
                return group.ObservableUpdate;
            }

            throw new KeyNotFoundException(string.Format(ErrorMessages.GroupNotExistError, id));
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
                    .Subscribe(handler, token);
                return;
            }
            throw new KeyNotFoundException(string.Format(ErrorMessages.GroupNotExistError, id));
        }

        void IGroupService.SubscribeOnCollectionChange(Action<CollectionChangeEventArgs> handler, CancellationToken token)
        {
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }
            (this as IGroupService).ObservableChange
                .Subscribe(handler, token);
        }
        #endregion

        #region Channels
        int IChannelService.Count => _Channels.Count;

        IObservable<CollectionChangeEventArgs> IChannelService.ObservableChange => _ChannelsObservable;

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
            (this as IGroupService).ExistOrTrown(groupId);
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


            (this as IGroupService).ExistOrTrown(model.GroupId ?? Guid.Empty);
            

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
            throw new KeyNotFoundException(string.Format(ErrorMessages.ChannelNotExistError, id));
        }

        ChannelModel IChannelService.Retrieve(Guid id, params string[] fields)
        {
            if (_Channels.TryGetValue(id, out var channel))
            {
                return channel.ToModel(fields);
            }
            throw new KeyNotFoundException(string.Format(ErrorMessages.ChannelNotExistError, id));
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
            throw new KeyNotFoundException(string.Format(ErrorMessages.ChannelNotExistError, id));
        }

        T IChannelService.RetrieveValue<T>(Guid id)
        {
            if (_Channels.TryGetValue(id, out var channel))
            {
                return channel.GetValue<T>();
            }
            throw new KeyNotFoundException(string.Format(ErrorMessages.ChannelNotExistError, id));
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
            throw new KeyNotFoundException(string.Format(ErrorMessages.ChannelNotExistError, model.Id));
        }

        void IChannelService.UpdateValue(Guid id, object value)
        {
            if (_Channels.TryGetValue(id, out var channel))
            {
                channel.UpdateValue(value);
                return;
            }
            throw new KeyNotFoundException(string.Format(ErrorMessages.ChannelNotExistError, id));
        }
        #endregion

        #region Delete
        void IChannelService.Delete(Guid id)
        {
            if (!_Channels.TryRemove(id, out var channel))
            {
                throw new KeyNotFoundException(string.Format(ErrorMessages.ChannelNotExistError, id));
            }
            _ChannelsChangeEvent?.Invoke(this,
                new CollectionChangeEventArgs(channel.Id, CollectionChangeType.DeleteElement));

            channel.Dispose();
        }
        #endregion

        IObservable<UpdateEventArgs> IChannelService.GetObservableUpdate(Guid id)
        {
            if (_Channels.TryGetValue(id, out var channel))
            {
                return channel.ObservableUpdate;
            }
            throw new KeyNotFoundException(string.Format(ErrorMessages.ChannelNotExistError, id));
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
                    .Subscribe(handler, token);
                return;
            }
            throw new KeyNotFoundException(string.Format(ErrorMessages.ChannelNotExistError, id));
        }

        void IChannelService.SubscribeOnCollectionChange(Action<CollectionChangeEventArgs> handler, CancellationToken token)
        {
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }
            (this as IChannelService).ObservableChange
                .Subscribe(handler, token);
        }
        #endregion
    }
}
