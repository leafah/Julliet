using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Julliet.Engine;
using Julliet.Webapi.Repositories;
using System.IO;
using RestSharp;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Julliet.Webapi.Controllers
{
    [Route("api/[controller]")]
    public class LettersController : Controller
    {
        private readonly LetterRepository _repository;

        public LettersController()
        {
            this._repository = new LetterRepository();
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]dynamic body)
        {
            JObject jObject = JObject.Parse(Convert.ToString(body));
            var name = jObject.GetValue("name").Value<string>();
            var letterType = jObject.GetValue("letterType").Value<string>();
            var letter = jObject.GetValue("letter").Value<string>();

            this._repository.WriteLetter(letter, Path.Combine(@"C:\Julliet", name), letterType + ".ltr");
            
            Response.WriteAsync($"The letter {name} was created com sucess");
        }

    }
}
