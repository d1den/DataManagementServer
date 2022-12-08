using DataManagementServer.Common.Models;
using System;
using System.Collections.Generic;
using System.Threading;

namespace DataManagementServer.Sdk.Channels
{
    /// <summary>
    /// Сервис доступа к группам каналов
    /// </summary>
    public interface IGroupService
    {
        /// <summary>
        /// Количество групп
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Уведомитель об изменении коллекции
        /// </summary>
        IObservable<CollectionChangeEventArgs> ObservableChange { get; }

        #region Create
        /// <summary>
        /// Создать группу в корне
        /// </summary>
        /// <returns>Id группы</returns>
        Guid Create();

        /// <summary>
        /// Создать группы в родительской группе
        /// </summary>
        /// <param name="parentId">Id родителя</param>
        /// <returns>Id созданной группы</returns>
        Guid Create(Guid parentId);

        /// <summary>
        /// Создать группу на основе модели
        /// </summary>
        /// <param name="model">Модель группы</param>
        /// <returns>Id созданной группы</returns>
        Guid Create(GroupModel model);
        #endregion

        #region Exist

        void ExistOrTrown(Guid groupId);
        #endregion
        #region Retrieve
        /// <summary>
        /// Получить группу
        /// </summary>
        /// <param name="id">Id группы</param>
        /// <returns>Модель группы</returns>
        GroupModel Retrieve(Guid id);

        /// <summary>
        /// Получить набор групп по родителю
        /// </summary>
        /// <param name="parentId">Id родителской группы</param>
        /// <param name="allFields">Получить все поля или только Id</param>
        /// <returns>Список моделей групп</returns>
        List<GroupModel> RetrieveByParent(Guid parentId = default, bool allFields = false);

        /// <summary>
        /// Получить все группы
        /// </summary>
        /// <param name="allFields">Получить все поля или только Id</param>
        /// <returns>Список моделей групп</returns>
        List<GroupModel> RetrieveAll(bool allFields = false);
        #endregion

        #region Update
        /// <summary>
        /// Обновить группу
        /// </summary>
        /// <param name="model">Модель группы</param>
        void Update(GroupModel model);
        #endregion

        #region Delete
        /// <summary>
        /// Удалить группу
        /// </summary>
        /// <param name="id">Id группы</param>
        /// <param name="withChildren">С дочерними элементами или без</param>
        void Delete(Guid id, bool withChildren = false);
        #endregion

        /// <summary>
        /// Получить Rx объект события обновления группы
        /// </summary>
        /// <param name="id">Id группы</param>
        /// <returns>Rx объект события обновления</returns>
        IObservable<UpdateEventArgs> GetObservableUpdate(Guid id);

        /// <summary>
        /// Подписаться на обновление группы
        /// </summary>
        /// <param name="id">Id группы</param>
        /// <param name="handler">Обработчик события</param>
        /// <param name="token">Токен для отписки</param>
        void SubscribeOnUpdate(Guid id, Action<UpdateEventArgs> handler, CancellationToken token = default);
        
        /// <summary>
        /// Подписаться на изменение коллекции
        /// </summary>
        /// <param name="handler">Обработчик события</param>
        /// <param name="token">Токен для отписки</param>
        void SubscribeOnCollectionChange(Action<CollectionChangeEventArgs> handler, CancellationToken token = default);
    }
}
