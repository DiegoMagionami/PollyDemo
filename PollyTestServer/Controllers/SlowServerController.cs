using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PollyTestServer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SlowServerController : ControllerBase
    {
        private readonly TimeSpan delay = TimeSpan.FromSeconds(20);

        // GET: api/NonThrottledFaulting
        [HttpGet(Name = "GetSlowResponse")]
        public async Task<ResponseDTO> Get()
        {
            throw new Exception();
            await Task.Delay(delay);
            return new ResponseDTO() { Message = "Slow response from server" };
        }

        // GET: api/GetSlowResponse/1
        [HttpGet("{id}", Name = "GetSlowResponseById")]
        public async Task<string> Get(int id)
        {
            await Task.Delay(delay);
            return "Slow, faulting response from server to request #" + id;
        }
    }
}
