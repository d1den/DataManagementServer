using System.Collections.Generic;
using System.Reflection;

namespace DataManagementServer.Core.Services.Abstract
{
    /// <summary>
    /// Логика динамической загрузки сборок
    /// </summary>
    public interface IAssemblyLoader
    {
        /// <summary>
        /// Загрузить сборки
        /// </summary>
        /// <returns>Список сборок</returns>
        IList<Assembly> Load();

        /// <summary>
        /// Получить сборки
        /// </summary>
        /// <returns>Список сборок</returns>
        IList<Assembly> GetAssemblies();
    }
}
