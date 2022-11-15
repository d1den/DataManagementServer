using DataManagementServer.Core.Extentions;
using DataManagementServer.Core.Services.Abstract;
using DataManagementServer.Sdk;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DataManagementServer.AppServer.Extentions
{
    /// <summary>
    /// Расширения для коллекции сервисов
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Добавить Singleton сервисы из сборок
        /// </summary>
        /// <param name="services">Коллекция сервисов</param>
        /// <param name="assemblyLoader">Загрузчик сборок</param>
        /// <returns>Коллекция сервисов</returns>
        /// <exception cref="ArgumentNullException">При Null параметрах</exception>
        public static IServiceCollection AddSingletonsFromAsseblies(this IServiceCollection services, IAssemblyLoader assemblyLoader)
        {
            _ = services ?? throw new ArgumentNullException(nameof(services));
            _ = assemblyLoader ?? throw new ArgumentNullException(nameof(assemblyLoader));

            var assemblies = assemblyLoader.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                var singletonServices = assembly.GetTypesByInterface(typeof(ISingletonService));
                if (singletonServices == null)
                {
                    continue;
                }

                foreach(var singletonService in singletonServices)
                {
                    services.AddSingleton(singletonService);
                }
            }

            return services;
        }
    }
}
