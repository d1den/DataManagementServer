using DataManagementServer.AppServer.Extentions;
using DataManagementServer.Core.Services.Concrete;
using DataManagementServer.Sdk.Channels;
using DataManagementServer.Sdk.PluginInterfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

var pluginFolderPath = "/Plugins";
var assemblyLoader = new AssemblyLoader(pluginFolderPath);
assemblyLoader.Load();

var builder = WebApplication.CreateBuilder(args);

// Добавляем в сервисы все контроллеры в текущем домене и из загруженных сборок
builder.Services
    .AddControllers()
    .AddControllersFromAssemblies(assemblyLoader);

// Добавляем сервисы ядра
builder.Services.AddSingleton(assemblyLoader)
    .AddSingleton<OrganizationService>()
    .AddSingleton<IGroupService>(provider => provider.GetService<OrganizationService>())
    .AddSingleton<IChannelService>(provider => provider.GetService<OrganizationService>())
    .AddSingleton<IPluginService>(provider => new PluginService(assemblyLoader, provider));

// Добавляем Singleton сервисы, определённые в сборках плагинов
builder.Services.AddSingletonsFromAsseblies(assemblyLoader);

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// Добавляем маршрутизацию
app.UseRouting();

// Настраиваем конечные точки
app.UseEndpoints(endpoints =>
{
    // Настраиваем шаблон стандартного маршрута контроллеров
    endpoints.MapControllerRoute("defaultApi", "api/{controller}/{action}");
    // Настраиваем маршрутизацию на основе атрибутов контроллера
    endpoints.MapControllers();
});

app.Run();
