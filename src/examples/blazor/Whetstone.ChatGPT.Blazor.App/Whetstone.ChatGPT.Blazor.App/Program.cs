using Blazored.LocalStorage;
using Blazorise;
using Blazorise.Bootstrap5;
using Blazorise.Icons.FontAwesome;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Whetstone.ChatGPT;
using Whetstone.ChatGPT.Blazor.App;
using Whetstone.ChatGPT.Blazor.App.State;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient<IChatGPTClient, ChatGPTClient>();

builder.Services.AddSingleton<ApplicationState>();

builder.Services.AddScoped<IChatGPTClient, ChatGPTClient>();

builder.Services.AddScoped<IOpenAICredentialValidator, OpenAICredentialValidator>();

AddBlazorise(builder.Services);

await builder.Build().RunAsync();

void AddBlazorise(IServiceCollection services)
{
    services
        .AddBlazorise();

    services.AddBlazoredLocalStorage();
    
    services
        .AddBootstrap5Providers()
        .AddFontAwesomeIcons();

}
