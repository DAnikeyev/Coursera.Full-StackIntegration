using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ClientApp;
using ClientApp.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped(_ =>
{
    var apiBaseUrl = builder.Configuration["ApiBaseUrl"] ?? "http://localhost:5158";
    var normalizedApiBaseUrl = $"{apiBaseUrl.TrimEnd('/')}/";
    var apiTimeoutSeconds = builder.Configuration.GetValue<int?>("ApiTimeoutSeconds") ?? 10;

    return new ProductApiClient(
        new HttpClient
        {
            BaseAddress = new Uri(normalizedApiBaseUrl, UriKind.Absolute),
            Timeout = TimeSpan.FromSeconds(apiTimeoutSeconds)
        });
});

await builder.Build().RunAsync();
