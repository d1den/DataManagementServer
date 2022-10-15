using DataManagementServer.Core.Services.Abstract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace DataManagementServer.Core.Services.Concrete
{
    /// <summary>
    /// Логика динамической загрузки сборок из папки
    /// </summary>
    public class AssemblyLoader : IAssemblyLoader
    {
        /// <summary>
        /// Шаблон названия для файла .dll
        /// </summary>
        private const string _DllFileNamePattern = "*.dll";

        /// <summary>
        /// Путь к папке со сборками
        /// </summary>
        private readonly string _AssembliesDirectoryPath;

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="assembliesDirectoryPath">Путь к папке со сборками</param>
        public AssemblyLoader(string assembliesDirectoryPath)
        {
            if (string.IsNullOrWhiteSpace(assembliesDirectoryPath))
            {
                throw new ArgumentNullException(nameof(assembliesDirectoryPath));
            }

            _AssembliesDirectoryPath = assembliesDirectoryPath;
        }

        public IList<Assembly> Load()
        {
            if (!Directory.Exists(_AssembliesDirectoryPath))
            {
                return new List<Assembly>();
            }

            return Directory.EnumerateFiles(_AssembliesDirectoryPath, _DllFileNamePattern)
                .Select(file => Assembly.LoadFrom(file)).ToList();
        }
    }
}
