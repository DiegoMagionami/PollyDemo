using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        public ExternalMessageController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }


        [HttpGet]
        public async Task<ActionResult> Get()
        {
            BoardMessageDTO message = new BoardMessageDTO { Date = new DateTime(2021, 12, 16), Title = "Test title"};

            string requestEndpoint = $"/api/message";

            var httpClient = _httpClientFactory.CreateClient("BoardApiServer");

            HttpResponseMessage response = await httpClient.GetAsync(requestEndpoint);

            if (response.IsSuccessStatusCode)
            {
                BoardMessageDTO messageContent = await response.Content.ReadAsAsync<BoardMessageDTO>();
                // set the cost on the order with info from the invoice
                message.Message = messageContent.Message;
                return Ok(message);
            }

            return StatusCode((int)response.StatusCode, response.Content.ReadAsStringAsync());
        }
    }
}
