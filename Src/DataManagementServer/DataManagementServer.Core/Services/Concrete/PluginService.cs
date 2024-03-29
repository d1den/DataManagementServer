﻿using DataManagementServer.Core.Extentions;
using DataManagementServer.Core.Resources;
using DataManagementServer.Core.Services.Abstract;
using DataManagementServer.Sdk.PluginInterfaces;
using System;
using System.Collections.Concurrent;
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
        /// Словарь плагинов
        /// </summary>
        private readonly ConcurrentDictionary<Guid, IPlugin> _Plugins = new ();

        /// <summary>
        /// Уже уничтожен?
        /// </summary>
        private bool _IsDisposed = false;

        /// <summary>
        /// Уже инициализирован?
        /// </summary>
        private bool IsInitialize = false;

        /// <summary>
        /// Объект для синхронизации потоков
        /// </summary>
        private readonly object _Lock = new ();

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
        /// <returns>Список типов плагинов</returns>
        private IEnumerable<Type> LoadPluginTypes()
        {
            var pluginTypes = new List<Type>();

            var assemlies = _AssemblyLoader.GetAssemblies();
            foreach (var assembly in assemlies)
            {
                pluginTypes.AddRange(assembly.GetTypesByInterface(typeof(IPlugin)));
            }

            return pluginTypes;
        }

        public void Initialize()
        {
            lock (_Lock)
            {
                if (IsInitialize)
                {
                    return;
                }

                var plugins = LoadPluginTypes()
                    .Select(type => Activator.CreateInstance(type) as IPlugin);

                foreach (var plugin in plugins)
                {
                    plugin.Initialize(_ServiceProvider);
                    _Plugins.TryAdd(plugin.Id, plugin);
                }

                IsInitialize = true;
            }
        }

        public bool TryGetPlugin(Guid id, out IPlugin plugin)
        {
            return _Plugins.TryGetValue(id, out plugin);
        }

        public bool TryGetPlugin(Type pluginType, out IPlugin plugin)
        {
            _ = pluginType ?? throw new ArgumentNullException(nameof(pluginType));

            plugin =  _Plugins.Values
                .FirstOrDefault(p => p.GetType() == pluginType);

            return plugin != null;
        }

        public bool TryGetPlugin(string pluginTypeName, out IPlugin plugin)
        {
            if (string.IsNullOrWhiteSpace(pluginTypeName))
            {
                throw new ArgumentNullException(nameof(pluginTypeName));
            }

            plugin = _Plugins.Values
                .FirstOrDefault(p => p.GetType().Name == pluginTypeName);

            return plugin != null;
        }

        public IPlugin GetPlugin(Guid id)
        {
            if (!_Plugins.TryGetValue(id, out var plugin))
            {
                throw new KeyNotFoundException(string.Format(ErrorMessages.PluginNotExistError, id));
            }

            return plugin;
        }

        public IPlugin GetPlugin(Type pluginType)
        {
            _ = pluginType ?? throw new ArgumentNullException(nameof(pluginType));

            var plugin = _Plugins.Values
                .FirstOrDefault(p => p.GetType() == pluginType);

            if (plugin == null)
            {
                throw new KeyNotFoundException(string.Format(ErrorMessages.PluginNotExistError, pluginType.Name));
            }
            return plugin;
        }

        public IPlugin GetPlugin(string pluginTypeName)
        {
            if (string.IsNullOrWhiteSpace(pluginTypeName))
            {
                throw new ArgumentNullException(nameof(pluginTypeName));
            }

            var plugin = _Plugins.Values
                .FirstOrDefault(p => p.GetType().Name == pluginTypeName);

            if (plugin == null)
            {
                throw new KeyNotFoundException(string.Format(ErrorMessages.PluginNotExistError, pluginTypeName));
            }
            return plugin;
        }

        public List<IPlugin> RetrieveAll()
        {
            return _Plugins.Values.ToList();
        }

        public void Dispose()
        {
            lock (_Lock)
            {
                if (_IsDisposed)
                {
                    return;
                }

                foreach (var plugin in _Plugins.Values)
                {
                    plugin.Dispose();
                }
                _Plugins.Clear();

                _IsDisposed = true;
            }
        }
    }
}
