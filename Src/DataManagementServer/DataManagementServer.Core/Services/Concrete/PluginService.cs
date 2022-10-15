using DataManagementServer.Core.Extentions;
using DataManagementServer.Core.Services.Abstract;
using DataManagementServer.Sdk.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataManagementServer.Core.Services.Concrete
{
    /// <summary>
    /// Сервис работы с плагинами
    /// </summary>
    /// <remarks>Используется для динамической загрузки сборок плагинов, создания плагинов и доступа к ним</remarks>
    public class PluginService : IPluginService
    {
        /// <summary>
        /// Плагины
        /// </summary>
        private readonly Dictionary<Guid, IPlugin> _Plugins = new ();

        /// <summary>
        /// Уже уничтожен?
        /// </summary>
        private bool _IsDisposed = false;

        /// <summary>
        /// Провайдер сервисов
        /// </summary>
        private readonly IServiceProvider _ServiceProvider;

        /// <summary>
        /// Логика загрузки сборок из папки
        /// </summary>
        private readonly IAssemblyLoader _AssemblyLoader;

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="assemblyLoader">Логика загрузки сборок из папки</param>
        /// <param name="serviceProvider">Провайдер сервисов</param>
        /// <exception cref="ArgumentNullException">Ошибка Null аргумента</exception>
        public PluginService(IAssemblyLoader assemblyLoader, IServiceProvider serviceProvider)
        {
            _AssemblyLoader = assemblyLoader ?? throw new ArgumentNullException(nameof(assemblyLoader));
            _ServiceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        /// <summary>
        /// Загрузить типы плагинов
        /// </summary>
        /// <returns>Набор типов</returns>
        private IEnumerable<Type> LoadPluginTypes()
        {
            var pluginTypes = new List<Type>();
            var assemlies = _AssemblyLoader.Load();
            foreach (var assembly in assemlies)
            {
                pluginTypes.AddRange(assembly.GetTypesByInterface(typeof(IPlugin)));
            }

            return pluginTypes;
        }

        /// <summary>
        /// Загрузить новые плагины
        /// </summary
        public void Load()
        {
            var plugins = LoadPluginTypes()
                .Where(type => !_Plugins.Values.Any(plugin => plugin.GetType() == type))
                .Select(type => Activator.CreateInstance(type) as IPlugin);

            foreach(var plugin in plugins)
            {
                plugin.Initialize(_ServiceProvider);
                _Plugins.Add(plugin.Id, plugin);
            }
        }

        public void Dispose()
        {
            if (_IsDisposed)
            {
                return;
            }

            foreach(var plugin in _Plugins.Values)
            {
                plugin.Dispose();
            }
            _Plugins.Clear();

            _IsDisposed = true;
        }
    }
}
