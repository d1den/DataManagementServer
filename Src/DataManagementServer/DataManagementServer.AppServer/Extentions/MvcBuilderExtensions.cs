using DataManagementServer.Core.Services.Abstract;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DataManagementServer.AppServer.Extentions
{
    /// <summary>
    /// Расширения для сборщика MVC сервисов
    /// </summary>
    public static class MvcBuilderExtensions
    {
        /// <summary>
        /// Добавление MVC контроллеров из сборок
        /// </summary>
        /// <param name="mvcBuilder">Сборщик MVC сервисов</param>
        /// <param name="assemblyLoader">Загрузчик сборок</param>
        /// <returns>Сборщик MVC сервисов</returns>
        /// <exception cref="ArgumentNullException">При Null параметрах</exception>
        public static IMvcBuilder AddControllersFromAssemblies(this IMvcBuilder mvcBuilder, IAssemblyLoader assemblyLoader)
        {
            _ = mvcBuilder ?? throw new ArgumentNullException(nameof(mvcBuilder));
            _ = assemblyLoader ?? throw new ArgumentNullException(nameof(assemblyLoader));

            var assemblies = assemblyLoader.GetAssemblies();
            foreach(var assembly in assemblies)
            {
                mvcBuilder.AddApplicationPart(assembly);
            }
            mvcBuilder.AddControllersAsServices();

            return mvcBuilder;
        }
    }
}
