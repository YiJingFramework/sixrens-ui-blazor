using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SixRens.UI.Blazor;
using SixRens.UI.Blazor.Services.DivinationCaseStorager;
using SixRens.UI.Blazor.Services.FirstTimeUseChecker;
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

            RegisterIndexedDb(builder.Services);
            RegisterHttpClientServices(builder.Services, builder.HostEnvironment.BaseAddress);
            RegisterProjectServices(builder.Services);

            await builder.Build().RunAsync();
        }

        private static void RegisterIndexedDb(IServiceCollection services)
        {
            _ = services.AddIndexedDB(options => {
                options.DbName = "sixrens-indexed-db";
                options.Version = 1;
                options.Stores.Add(DivinationCaseStorager.IndexedDbStoreSchema);
                options.Stores.Add(FirstTimeUseChecker.IndexedDbStoreSchema);
                options.Stores.AddRange(ServiceOfSixRens.IndexedDbStoreSchemas);
            });
        }

        private static void RegisterHttpClientServices(IServiceCollection services, string baseAddress)
        {
            _ = services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(baseAddress) });
        }

        private static void RegisterProjectServices(IServiceCollection services)
        {
            _ = services.AddScoped<FirstTimeUseChecker>();
            _ = services.AddScoped<ServiceOfSixRens>();
            _ = services.AddScoped<DivinationCaseStorager>();
        }
    }
}