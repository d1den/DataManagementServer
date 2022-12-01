using DataManagementServer.Common.Models;
using System;
using System.Collections.Generic;
using System.Threading;

namespace DataManagementServer.Sdk.Channels
{
    /// <summary>
    /// Сервис доступа к каналам
    /// </summary>
    public interface IChannelService
    {
        /// <summary>
        /// Количество каналов
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Уведомитель об изменении коллекции
        /// </summary>
        IObservable<CollectionChangeEventArgs> ObservableChange { get; }

        #region Create
        /// <summary>
        /// Создать канал в корне
        /// </summary>
        /// <returns>Id канала</returns>
        Guid Create();

        /// <summary>
        /// Создать канал в группе
        /// </summary>
        /// <param name="groupId">Id группы</param>
        /// <returns>Id канала</returns>
        Guid Create(Guid groupId);

        /// <summary>
        /// Создать канал на основе модели
        /// </summary>
        /// <param name="model">Модель канала</param>
        /// <returns>Id канала</returns>
        Guid Create(ChannelModel model);
        #endregion

        #region Retrieve
        /// <summary>
        /// Получить канал
        /// </summary>
        /// <param name="id">Id канала</param>
        /// <param name="allFields">Получить все поля или только Id</param>
        /// <returns>Модель канала</returns>
        ChannelModel Retrieve(Guid id, bool allFields);

        /// <summary>
        /// Получить канала
        /// </summary>
        /// <param name="id">Id канала</param>
        /// <param name="fields">Список полей для модели</param>
        /// <returns>Модель канала</returns>
        ChannelModel Retrieve(Guid id, params string[] fields);

        /// <summary>
        /// Получить набор каналов по группе
        /// </summary>
        /// <param name="groupId">Id группы</param>
        /// <param name="allFields">Получить все поля или только Id</param>
        /// <returns>Список моделей каналов</returns>
        List<ChannelModel> RetrieveByGroup(Guid groupId, bool allFields);

        /// <summary>
        /// Получить набор каналов по группе
        /// </summary>
        /// <param name="groupId">Id группы</param>
        /// <param name="fields">Список полей для модели</param>
        /// <returns>Список моделей каналов</returns>
        List<ChannelModel> RetrieveByGroup(Guid groupId, params string[] fields);

        /// <summary>
        /// Получить все каналы
        /// </summary>
        /// <param name="allFields">Получить все поля или только Id</param>
        /// <returns>Список моделей каналов</returns>
        List<ChannelModel> RetrieveAll(bool allFields);

        /// <summary>
        /// Получить все каналы
        /// </summary>
        /// <param name="fields">Список полей для модели</param>
        /// <returns>Список моделей каналов</returns>
        List<ChannelModel> RetrieveAll(params string[] fields);

        /// <summary>
        /// Получить значение канала
        /// </summary>
        /// <param name="id">Id канала</param>
        /// <returns>Значение канала в object</returns>
        object RetrieveValue(Guid id);

        /// <summary>
        /// Получить значение канала
        /// </summary>
        /// <typeparam name="T">Тип значения</typeparam>
        /// <param name="id">Id канала</param>
        /// <returns>Типизированное значение канала</returns>
        T RetrieveValue<T>(Guid id);

        /// <summary>
        /// Попытка получения значения канала
        /// </summary>
        /// <typeparam name="T">Тип значения</typeparam>
        /// <param name="id">Id канала</param>
        /// <param name="value">Типизированное значение канала</param>
        /// <returns>Результат попытки</returns>
        bool TryRetrieveValue<T>(Guid id, out T value);
        #endregion

        #region Update
        /// <summary>
        /// Обновить значение канала
        /// </summary>
        /// <param name="id">Id канала</param>
        /// <param name="value">Новое значение</param>
        void UpdateValue(Guid id, object value);

        /// <summary>
        /// Обновить канал на основе модели
        /// </summary>
        /// <param name="model">Модель канала</param>
        void Update(ChannelModel model);
        #endregion

        #region Delete
        /// <summary>
        /// Удалить канал
        /// </summary>
        /// <param name="id">Id канала</param>
        void Delete(Guid id);
        #endregion

        /// <summary>
        /// Получить Rx объект события обновления канала
        /// </summary>
        /// <param name="id">Id канала</param>
        /// <returns>Rx объект события обновления</returns>
        IObservable<UpdateEventArgs> GetObservableUpdate(Guid id);


        /// <summary>
        /// Подписаться на обновление канала
        /// </summary>
        /// <param name="id">Id канала</param>
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
