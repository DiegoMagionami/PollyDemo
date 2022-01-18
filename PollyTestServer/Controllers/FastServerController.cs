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
    public class FastServerController : ControllerBase
    {
        // GET: api/GetFastResponse
        [HttpGet(Name = "GetFastResponse")]
        public ResponseDTO Get()
        {
            return new ResponseDTO()
            {
                Message = "Fast response from server"
            };        
        }

        // GET: api/GetFastResponse/1
        [HttpGet("{id}", Name = "GetFastResponseById")]
        public ResponseDTO Get(int id)
        {
            return new ResponseDTO()
            {
                Message = "Fast response from server to request #" + id
            };
        }
    }
}
