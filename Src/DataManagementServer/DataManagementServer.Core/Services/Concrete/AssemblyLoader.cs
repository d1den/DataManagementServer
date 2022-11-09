using DataManagementServer.Core.Services.Abstract;
using DataManagementServer.Core.Resources;
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

            return Directory.EnumerateFiles(_AssembliesDirectoryPath, Constants.DllFileNamePattern)
                .Select(file => Assembly.LoadFrom(file)).ToList();
        }
    }
}
