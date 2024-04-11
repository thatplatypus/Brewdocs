using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Brewdocs;
using Syncfusion.Blazor;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Syncfusion.Licensing;
using Brewdocs.Services;
using Brewdocs.Services.Cache;
using Blazored.LocalStorage;
using Doodle;
using Toolbelt.Blazor.Extensions.DependencyInjection;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) }.EnableIntercept(sp));

builder.Services.AddSyncfusionBlazor();
//builder.Services.Add

//FusionLicenseProvider.GetLicenseType(Platform.Blazor);

builder.Services.AddBlazorBootstrap();

builder.Services.AddScoped<ThemeService>();
builder.Services.AddScoped<Brewdocs.Components.Breadcrumbs.BreadcrumbService>();
builder.Services.AddScoped<ILRUCacheService, ListCacheService>();
builder.Services.AddScoped<IActiveDocumentService, ActiveDocumentService>();

builder.Services.AddBlazoredLocalStorage();

builder.Services.UseDoodle((config) =>
{
    config.DefaultStrokeColor = "#FF0000";
    config.DefaultStrokeSize = 2;

    config.ToolbarConfig = new Doodle.Abstractions.Config.ToolbarConfig
    {
        ShowDrawActions = false,
        ShowDrawActionsUndo = false,
        ShowDrawActionsRedo = false,
        ShowDrawActionsClear = false,
        ShowDrawActionsExport = false,
    };
});

builder.Services.AddLoadingBarService();

builder.UseLoadingBar();

SyncfusionLicenseProvider.RegisterLicense("");

await builder.Build().RunAsync();