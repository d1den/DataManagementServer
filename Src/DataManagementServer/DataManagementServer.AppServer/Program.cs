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

// ��������� � ������� ��� ����������� � ������� ������ � �� ����������� ������
builder.Services
    .AddControllers()
    .AddControllersFromAssemblies(assemblyLoader);

// ��������� ������� SignalR - ����������� ����
builder.Services.AddSignalR();

// ��������� �������� ���� ������� ��� ������������� ��������� �������� ����
builder.Services
    .AddSingleton(provider => provider.GetService<IHubContext<PluginHub>>() as IHubContext);

// ��������� ������� ����
builder.Services.AddSingleton(assemblyLoader)
    .AddSingleton<OrganizationService>()
    .AddSingleton<IGroupService>(provider => provider.GetService<OrganizationService>())
    .AddSingleton<IChannelService>(provider => provider.GetService<OrganizationService>())
    .AddSingleton<IPluginService>(provider => new PluginService(assemblyLoader, provider));

// ��������� ������ ����������� ����� �������
builder.Services.AddSingleton<OrganizationNotificationService>();

// ��������� Singleton �������, ����������� � ������� ��������
builder.Services.AddSingletonsFromAsseblies(assemblyLoader);

var app = builder.Build();

// �������� ���-�� ������ ��� ������ ������������� (������ ������ � ������������ �������� ��� ����� DI)
app.Services.GetService<OrganizationNotificationService>();

// ������������� � ������
app.UseDefaultFiles();
app.UseStaticFiles();

// ��������� �������������
app.UseRouting();

// ����������� �������� �����
app.UseEndpoints(endpoints =>
{
    // ����������� ������ ������������ �������� ������������
    endpoints.MapControllerRoute("defaultApi", "api/{controller}/{action}");
    // ����������� ������������� �� ������ ��������� �����������
    endpoints.MapControllers();
});

// ��������� ������������� � ����
app.MapHub<OrganizationHub>("/hub");
app.MapHub<PluginHub>("/pluginHub");

app.Run();
