using IndexedDB.Blazor;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
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
                .AddSingleton<IIndexedDbFactory, IndexedDbFactory>()
                .AddSingleton<ServiceOfSixRens>();

            await builder.Build().RunAsync();
        }
    }
}