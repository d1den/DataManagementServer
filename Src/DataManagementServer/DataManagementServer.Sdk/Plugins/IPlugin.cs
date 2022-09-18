using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataManagementServer.Sdk.Plugins
{
    /// <summary>
    /// Интерфейс плагина системы
    /// </summary>
    public interface IPlugin
    {
        /// <summary>
        /// Id плагина
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// Название плагина
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Тип плагина
        /// </summary>
        PluginType Type { get; }

        /// <summary>
        /// Получить строку с конфигурацией плагина
        /// </summary>
        /// <returns>Json строка с конфигурацией</returns>
        string GetConfig();

        /// <summary>
        /// Инициализировать плагин по json строке
        /// </summary>
        /// <param name="jsonDevicesConfig">Json строка с конфигурацией</param>
        void InitByConfig(string jsonDevicesConfig);

        /// <summary>
        /// Создать плагин
        /// </summary>
        /// <param name="serviceProvider">Провайдер сервисов ядра</param>
        void CreatePlugin(IServiceProvider serviceProvider);
    }
}
