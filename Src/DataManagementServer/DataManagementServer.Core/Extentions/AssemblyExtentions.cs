using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DataManagementServer.Core.Extentions
{
    /// <summary>
    /// Методы расширения к классу Assembly
    /// </summary>
    public static class AssemblyExtentions
    {
        /// <summary>
        /// Получить набор типов в сборке, реализующих заданный интерфейс
        /// </summary>
        /// <param name="assembly">Сборка</param>
        /// <param name="interfaceFilter">Тип интерфейса</param>
        /// <returns>Список типов</returns>
        /// <exception cref="ArgumentNullException">При параметрах Null</exception>
        public static IList<Type> GetTypesByInterface(this Assembly assembly, Type interfaceFilter)
        {
            _ = assembly ?? throw new ArgumentNullException(nameof(assembly));
            _ = interfaceFilter ?? throw new ArgumentNullException(nameof(interfaceFilter));

            return assembly.ExportedTypes.Where(type => type.IsClass 
                && !type.IsAbstract
                && type.GetInterface(interfaceFilter.FullName) != null).ToList();
        }
    }
}
