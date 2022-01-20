using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using PollyTestServer.Config;
using PollyTestServer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExternalMessageController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IOptions<GeneralConfig> _generalConfig;

        public ExternalMessageController(IHttpClientFactory httpClientFactory, IOptions<GeneralConfig> generalConfig)
        {
            _httpClientFactory = httpClientFactory;
            _generalConfig = generalConfig;
        }


        [HttpGet]
        public async Task<ActionResult> Get()
        {
            List<BoardMessageDTO> messages = new List<BoardMessageDTO>();
            BoardMessageDTO message = new BoardMessageDTO { Date = new DateTime(2021, 12, 16), Title = "Test title"};
            
            string requestEndpoint = $"/api/message";

            var httpClient = _httpClientFactory.CreateClient("BoardApiServer");
            httpClient.BaseAddress = new Uri(_generalConfig.Value.BaseAddress);
            HttpResponseMessage response = await httpClient.GetAsync(requestEndpoint);

            if (response.IsSuccessStatusCode)
            {
                IEnumerable<BoardMessageDTO> messageContent = await response.Content.ReadAsAsync<IEnumerable<BoardMessageDTO>>();
                // set the cost on the order with info from the invoice
                if (messageContent.Count() > 0)
                {
                    message.Message = messageContent.FirstOrDefault().Message;
                }
                messages.Add(message);
                return Ok(messages);
            }

            return StatusCode((int)response.StatusCode, response.Content.ReadAsStringAsync());
        }
    }
}
