using Microsoft.Extensions.Logging;
using Polly;
using Polly.Extensions.Http;
using PollyTestClient.Services;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;

namespace PollyTestClient
{
    public static class HttpClientPolicies
    {
        private static readonly Random _jitterer = new Random();

        public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(IServiceProvider serviceProvider, int retryCount = 3) =>
            HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(retryCount,
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                                  + TimeSpan.FromMilliseconds(_jitterer.Next(0, 100)),
                    onRetry: (result, span, index, ctx) =>
                    {
                        var logger = serviceProvider.GetService<ILogger<DemoService>>();
                        logger.LogWarning($"tentative #{index}, received {result.Result.StatusCode}, retrying...");
                    });

        public static IAsyncPolicy<HttpResponseMessage> GetFallbackPolicy(IServiceProvider serviceProvider, Func<Context, CancellationToken, Task<HttpResponseMessage>> valueFactory) =>
            HttpPolicyExtensions
            .HandleTransientHttpError()
            .FallbackAsync(valueFactory,
                (res, ctx) =>
                {
                    var logger = serviceProvider.GetService<ILogger<DemoService>>();
                    logger.LogWarning($"returning fallback value...");
                    return Task.CompletedTask;
                });
    }
}
