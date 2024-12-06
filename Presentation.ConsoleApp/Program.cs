using Presentation.ConsoleApp.Services;
using Business.Services;
using Business.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Presentation.ConsoleApp.Interfaces;


var host = Host.CreateDefaultBuilder()
    .ConfigureServices(services =>
    {
        services.AddSingleton<IFileService>(new FileService(fileName: "contacts.json"));
        services.AddScoped<IContactService, ContactService>();
        services.AddTransient<IMenuService, MenuService>();
    })
    .Build();

using var scope = host.Services.CreateScope();
var menuService = scope.ServiceProvider.GetRequiredService<IMenuService>();

menuService.MainMenu();
