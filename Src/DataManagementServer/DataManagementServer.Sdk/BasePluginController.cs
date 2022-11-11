using DataManagementServer.Sdk.PluginInterfaces;
using Microsoft.AspNetCore.Mvc;
using System;

namespace DataManagementServer.Sdk
{
    /// <summary>
    /// Базовый контроллер api плагина
    /// </summary>
    public abstract class BasePluginController : ControllerBase
    {
        /// <summary>
        /// Тип класса плагина
        /// </summary>
        protected abstract Type PluginClassType { get; }

        /// <summary>
        /// Сервис доступа к плагинам
        /// </summary>
        protected readonly IPluginService PluginService;

        /// <summary>
        /// Базовый конструктор
        /// </summary>
        /// <param name="pluginService">Сервис доступа к плагинам</param>
        /// <exception cref="ArgumentNullException">При null параметре</exception>
        public BasePluginController(IPluginService pluginService)
        {
            PluginService = pluginService ?? throw new ArgumentNullException(nameof(pluginService));
        }
    }
}
