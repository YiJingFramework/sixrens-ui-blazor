using BlazorDownloadFile;
using IndexedDB.Blazor;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SixRens.UI.Blazor.Services.DivinationCaseStorager;
using SixRens.UI.Blazor.Services.SixRens;

namespace SixRens.UI.Blazor
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");
            _ = builder.Services
                .AddScoped((sp) => {
                    return new HttpClient {
                        BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
                    };
                })
                .AddLogging();
            RegisterThirdPartyServices(builder.Services);
            RegisterProjectServices(builder.Services);
            await builder.Build().RunAsync();
        }

        private static void RegisterThirdPartyServices(IServiceCollection services)
        {
            _ = services.AddSingleton<IIndexedDbFactory, IndexedDbFactory>();
            _ = services.AddBlazorDownloadFile(ServiceLifetime.Singleton);
        }

        private static void RegisterProjectServices(IServiceCollection services)
        {
            _ = services.AddSingleton<ServiceOfSixRens>();
            _ = services.AddSingleton<DivinationCaseStorager>();
        }
    }
}