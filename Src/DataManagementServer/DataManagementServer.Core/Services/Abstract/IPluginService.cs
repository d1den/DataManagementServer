using System;

namespace DataManagementServer.Core.Services.Abstract
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
    }
}
