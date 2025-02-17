using Application;
// using Application.Source.Core;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

var host = builder.Build();
// var context = host.Services.GetRequiredService<Context>();

// // TEMAS
// context.Themes.Add(new("dark", ThemeType.DARK, "black", "wite"));
// context.Themes.Add(new("light", ThemeType.LIGHT, "wite", "black"));
// context.Themes.Add(new("high_contrast", ThemeType.HIGH_CONTRAST, "wite", "black"));

await host.RunAsync();