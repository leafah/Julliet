using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Julliet.Engine;
using Julliet.Webapi.Repositories;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using RestSharp;

namespace Julliet.Webapi.Controllers
{
    [Route("api/[controller]")]
    public class DocumentsController : Controller
    {
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] dynamic body)
        {
            var cl = HttpContext.Request;
            var letter = cl.Headers.Where(a => a.Key == "letter").ToList();
            var processLetter = new ProcessLetter(letter.FirstOrDefault().Value.ToString());
            processLetter.ReadLetter(Convert.ToString(body));
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
