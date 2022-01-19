using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PollyTestClient.Services;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PollyTestClient
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            int retryAttempts = int.Parse(builder.Configuration["retryAttempts"]);

            builder.Services.AddHttpClient<DemoService>((sp, client) =>
            {
                client.BaseAddress = new Uri(builder.Configuration["demoApi"]);
            }).AddPolicyHandler((sp, msg) => Polly.Policy.WrapAsync(HttpClientPolicies.GetFallbackPolicy(sp, DemoService.FallbackValueFactory),
                                                                    HttpClientPolicies.GetRetryPolicy(sp, retryAttempts)));

            await builder.Build().RunAsync();
        }
    }
}
