using Application;
using Application.Source.Core;
using Application.Source.Core.Storage;
using Microsoft.AspNetCore.Components.Web;
using Application.Source.Core.Storage.IndexedDb;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddSingleton<Context>();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

var host = builder.Build();

var context = host.Services.GetRequiredService<Context>();

// TEMAS
context.Themes.Add(new("dark", ThemeType.DARK, "black", "wite"));
context.Themes.Add(new("light", ThemeType.LIGHT, "wite", "black"));
context.Themes.Add(new("high_contrast", ThemeType.HIGH_CONTRAST, "wite", "black"));

// BANCO DE DADOS
var upgrade = new Upgrade(1, [
    new Storage("files", [
        new Field("id", [
            FieldProperty.KEY,
            FieldProperty.DEFAULT_VALUE_AUTO_INCREMENT,
        ]),
        new Field("name", [
            FieldProperty.INDEXABLE,
        ]),
    ]),
]);
var connection = context.IndexedDb.GetConnection("storage");
connection.Upgrade(upgrade);
var transaction = connection.Transaction("files");

await host.RunAsync();
