using DataManagementServer.AppServer.Extentions;
using DataManagementServer.Core.Services.Concrete;
using DataManagementServer.Sdk.Channels;
using DataManagementServer.Sdk.PluginInterfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Converters;

var pluginFolderPath = "/Plugins";
var assemblyLoader = new AssemblyLoader(pluginFolderPath);
assemblyLoader.Load();

var builder = WebApplication.CreateBuilder(args);

// ��������� � ������� ��� ����������� � ������� ������ � �� ����������� ������
builder.Services
    .AddControllers()
    .AddControllersFromAssemblies(assemblyLoader)
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.Converters.Add(new StringEnumConverter());
    });

// ��������� ������� ����
builder.Services.AddSingleton(assemblyLoader)
    .AddSingleton<OrganizationService>()
    .AddSingleton<IGroupService>(provider => provider.GetService<OrganizationService>())
    .AddSingleton<IChannelService>(provider => provider.GetService<OrganizationService>())
    .AddSingleton<IPluginService>(provider => new PluginService(assemblyLoader, provider));

// ��������� Singleton �������, ����������� � ������� ��������
builder.Services.AddSingletonsFromAsseblies(assemblyLoader);

var app = builder.Build();

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

app.Run();
