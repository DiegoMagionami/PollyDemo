using PollyTestClient.DTO;
using System;
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

        public async Task<ResponseDTO> GetFastResponse()
        {
            var data = await _httpClient.GetFromJsonAsync<ResponseDTO>("/api/SlowServer");
            return data;
        }

        public static Task<HttpResponseMessage> FallbackValueFactory(Polly.Context context, CancellationToken cancellationToken)
        {
            var item = new ResponseDTO();
            var json = System.Text.Json.JsonSerializer.Serialize(item);
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
