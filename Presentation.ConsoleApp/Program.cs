using Presentation.ConsoleApp.Services;
using Business.Services;
using Business.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Presentation.ConsoleApp.Interfaces;
using Business.Helpers;
using Business.Repositories;
using Business.Models;


var host = Host.CreateDefaultBuilder()
    .ConfigureServices(services =>
    {
        services.AddSingleton<IFileService>(new FileService(fileName: "contacts.json"));
        services.AddScoped<IContactService, ContactService>();
        services.AddTransient<IMenuService, MenuService>();
        services.AddTransient<InputValidator>();
        services.AddScoped<IContactRepository<StoredContact>, ContactRepository<StoredContact>>();
    })
    .Build();

using var scope = host.Services.CreateScope();
var menuService = scope.ServiceProvider.GetRequiredService<IMenuService>();

await menuService.MainMenu();
