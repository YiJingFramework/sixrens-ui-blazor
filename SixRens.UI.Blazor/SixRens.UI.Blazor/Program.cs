using BlazorDownloadFile;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SixRens.UI.Blazor.Services.DivinationCaseStorager;
using SixRens.UI.Blazor.Services.SixRens;
using TG.Blazor.IndexedDB;

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
            _ = services.AddBlazorDownloadFile(ServiceLifetime.Singleton);
            _ = services.AddIndexedDB(options => {
                options.DbName = "sixrens-indexed-db";
            });
        }

        private static void RegisterProjectServices(IServiceCollection services)
        {
            _ = services.AddScoped<ServiceOfSixRens>();
            _ = services.AddScoped<DivinationCaseStorager>();
        }
    }
}