using System;
using System.Collections.Generic;

namespace DataManagementServer.Sdk.PluginInterfaces
{
    /// <summary>
    /// Сервис работы с плагинами
    /// </summary>
    public interface IPluginService : IDisposable
    {
        /// <summary>
        /// Инициализировать сервис плагинов
        /// </summary
        /// <remarks>Инициализирует словарь плагинов на основе сборок в папке расположения плагинов</remarks>
        void Initialize();

        /// <summary>
        /// Получить плагин
        /// </summary>
        /// <param name="id">Id плагина</param>
        /// <param name="plugin">Возвращаем плагин</param>
        /// <returns>Удалось получить?</returns>
        bool TryGetPlugin(Guid id, out IPlugin plugin);

        /// <summary>
        /// Получить плагин
        /// </summary>
        /// <param name="pluginType">Тип плагина</param>
        /// <param name="plugin">Возвращаем плагин</param>
        /// <returns>Удалось получить?</returns>
        bool TryGetPlugin(Type pluginType, out IPlugin plugin);

        /// <summary>
        /// Получить плагин
        /// </summary>
        /// <param name="pluginTypeName">Имя типа плагина</param>
        /// <param name="plugin">Возвращаем плагин</param>
        /// <returns>Удалось получить?</returns>
        bool TryGetPlugin(string pluginTypeName, out IPlugin plugin);

        /// <summary>
        /// Получить плагин или null
        /// </summary>
        /// <param name="id">Id плагина</param>
        /// <returns>Плагин</returns>
        IPlugin GetPlugin(Guid id);


        /// <summary>
        /// Получить плагин или null
        /// </summary>
        /// <param name="pluginType">Тип плагина</param>
        /// <returns>Плагин</returns>
        IPlugin GetPlugin(Type pluginType);

        /// <summary>
        /// Получить плагин или null
        /// </summary>
        /// <param name="pluginTypeName">Название типа плагина</param>
        /// <returns>Плагин</returns>
        IPlugin GetPlugin(string pluginTypeName);

        /// <summary>
        /// Получить все плагины
        /// </summary>
        /// <returns>Список плагинов</returns>
        List<IPlugin> RetrieveAll();
    }
}
