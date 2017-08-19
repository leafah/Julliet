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
using Newtonsoft.Json.Linq;
using System.IO;

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
            LetterReader letter = new LetterReader(HttpContext.Request.Headers["Letter"], @"C:\Letter\");

            JObject jRequest = JObject.Parse(Convert.ToString(body));

            Dictionary<string, string> parametersCollection = new Dictionary<string, string>();

            foreach (var parameters in jRequest.Values())
            {
                parametersCollection.Add(parameters.Path, parameters.Value<string>());
            }

            var request = letter.GetRequestByLetter(parametersCollection);

            var response = letter.Client.Execute(request);

            var dictionary = letter.GetValuesFromResponse(JObject.Parse(response.Content));

            JObject jObject = new JObject();

            foreach (var item in dictionary)
            {
                jObject.Add(item.Key, item.Value);
            }

            Response.WriteAsync(jObject.ToString());
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
