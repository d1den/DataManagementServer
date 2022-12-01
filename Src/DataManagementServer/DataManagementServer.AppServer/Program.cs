using DataManagementServer.AppServer.Extentions;
using DataManagementServer.AppServer.Hubs;
using DataManagementServer.AppServer.Services;
using DataManagementServer.Core.Services.Concrete;
using DataManagementServer.Sdk.Channels;
using DataManagementServer.Sdk.PluginInterfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;

var pluginFolderPath = "/Plugins";
var assemblyLoader = new AssemblyLoader(pluginFolderPath);
assemblyLoader.Load();

var builder = WebApplication.CreateBuilder(args);

// Добавляем в сервисы все контроллеры в текущем домене и из загруженных сборок
builder.Services
    .AddControllers()
    .AddControllersFromAssemblies(assemblyLoader);

// Добавляем сервисы SignalR - подтягиваем хабы
builder.Services.AddSignalR();

// Добавляем контекст хаба плагина как универсальный интерфейс контекта хаба
builder.Services
    .AddSingleton(provider => provider.GetService<IHubContext<PluginHub>>() as IHubContext);

// Добавляем сервисы ядра
builder.Services.AddSingleton(assemblyLoader)
    .AddSingleton<OrganizationService>()
    .AddSingleton<IGroupService>(provider => provider.GetService<OrganizationService>())
    .AddSingleton<IChannelService>(provider => provider.GetService<OrganizationService>())
    .AddSingleton<IPluginService>(provider => new PluginService(assemblyLoader, provider));

// Добавляем сервис уведомлений групп каналов
builder.Services.AddSingleton<OrganizationNotificationService>();

// Добавляем Singleton сервисы, определённые в сборках плагинов
builder.Services.AddSingletonsFromAsseblies(assemblyLoader);

var app = builder.Build();

// Вызываем где-то сервис для первой инициализации (дальше просто в контроллерах получать его через DI)
app.Services.GetService<OrganizationNotificationService>();

// Маршрутизация к файлам
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

// Добавляем маршрутизацию к хабу
app.MapHub<OrganizationHub>("/hub");
app.MapHub<PluginHub>("/pluginHub");

app.Run();
