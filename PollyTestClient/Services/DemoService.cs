using PollyTestClient.DTO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PollyTestClient.Services
{
    public class DemoService : IServiceProvider
    {
        private readonly HttpClient _httpClient;

        public DemoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<BoardMessageDTO>> GetMessages()
        {
            var data = await _httpClient.GetFromJsonAsync<IEnumerable<BoardMessageDTO>>("/api/SlowServer");
            return data;
        }

        public static Task<HttpResponseMessage> FallbackValueFactory(Polly.Context context, CancellationToken cancellationToken)
        {
            var items = Enumerable.Empty<BoardMessageDTO>();
            var json = System.Text.Json.JsonSerializer.Serialize(items);
            var response = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };

            return Task.FromResult(response);
        }

        public object GetService(Type serviceType)
        {
            throw new NotImplementedException();
        }
    }
}
