using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PollyTestServer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SlowServerController : ControllerBase
    {
        static int _getRequestCount = 0;
        static int _deleteRequestCount = 0;

        // GET: api/NonThrottledFaulting
        [HttpGet(Name = "GetSlowResponse")]
        public async Task<ActionResult> Get()
        {
            await Task.Delay(100);
            _getRequestCount++;
            List<BoardMessageDTO> messages = new List<BoardMessageDTO>();
            if (_getRequestCount % 4 == 0) // only one of out four requests will succeed
            {

                messages.Add(new BoardMessageDTO()
                {
                    Title = "New account",
                    Message = "Your new account has been created",
                    Date = DateTime.Now.AddDays(-40)
                });

                return Ok(messages);
            }

            return StatusCode((int)HttpStatusCode.InternalServerError, "Something went wrong.");
        }

        // GET: api/GetSlowResponse/1
        [HttpGet("{id}", Name = "GetSlowResponseById")]
        public async Task<string> Get(int id)
        {
            await Task.Delay(100);
            return "Slow, faulting response from server to request #" + id;
        }
    }
}
