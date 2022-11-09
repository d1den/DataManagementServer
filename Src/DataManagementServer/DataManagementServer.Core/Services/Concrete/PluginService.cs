﻿using DataManagementServer.Core.Extentions;
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

            var assemlies = _AssemblyLoader.Load();
            foreach (var assembly in assemlies)
            {
                pluginTypes.AddRange(assembly.GetTypesByInterface(typeof(IPlugin)));
            }

            return pluginTypes;
        }

        public void Initialize()
        {
            if (IsInitialize)
            {
                return;
            }

            _Plugins.Clear();
            var plugins = LoadPluginTypes()
                .Select(type => Activator.CreateInstance(type) as IPlugin);

            foreach(var plugin in plugins)
            {
                plugin.Initialize(_ServiceProvider);
                _Plugins.TryAdd(plugin.Id, plugin);
            }

            IsInitialize = true;
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