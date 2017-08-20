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
using System.Xml;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc.Formatters.Xml.Extensions;

namespace Julliet.Webapi.Controllers
{
    [Route("api/[controller]")]
    public class DocumentsController : Controller
    {
        // POST api/values
        [HttpPost]
        public void Post([FromBody] dynamic body)
        {
            LetterReader letter = new LetterReader(HttpContext.Request.Headers["Letter"], @"C:\Letter\", HttpContext.Request.Headers["Accept"]);

            if (HttpContext.Request.ContentType.Equals("application/json", StringComparison.CurrentCultureIgnoreCase))
            {
                JsoRequestHandle(body, letter);
            }
            else
            {
                XmlrequestHandle(body, letter);
            }
        }

        // POST api/[controller]/xml
        [HttpPost("xml")]
        public void PostDcXml([FromXmlBody(XmlSerializerType = XmlSerializerType.XmlSeriralizer)]XmlDocument body)
        {
            LetterReader letter = new LetterReader(HttpContext.Request.Headers["Letter"], @"C:\Letter\", HttpContext.Request.Headers["Accept"]);

            XmlrequestHandle(body, letter);
        }


        private void XmlrequestHandle(XmlDocument xmlDocument, LetterReader letter)
        {
            
            Dictionary<string, string> parametersCollection = new Dictionary<string, string>();

            handleNode(xmlDocument, parametersCollection);

            var request = letter.GetRequestByLetter(parametersCollection, "text/xml");

            var response = letter.Client.Execute(request);

            var dictionary = letter.GetValuesFromResponseToXml(JObject.Parse(response.Content));

            string retVal = letter.responseLetter;

            foreach (var item in dictionary)
            {
                retVal = retVal.Replace(item.Key, item.Value);
            }

            Response.WriteAsync(retVal);

        }

        private void JsoRequestHandle(dynamic body, LetterReader letter)
        {
            JObject jRequest = JObject.Parse(Convert.ToString(body));

            Dictionary<string, string> parametersCollection = new Dictionary<string, string>();

            foreach (var parameters in jRequest.Values())
            {
                parametersCollection.Add(parameters.Path, parameters.Value<string>());
            }

            var request = letter.GetRequestByLetter(parametersCollection, "application/json");

            var response = letter.Client.Execute(request);

            var dictionary = letter.GetValuesFromResponse(JObject.Parse(response.Content));

            JObject jObject = new JObject();

            foreach (var item in dictionary)
            {
                jObject.Add(item.Key, item.Value);
            }

            Response.WriteAsync(jObject.ToString());
        }

        private static void handleNode(XmlNode node, Dictionary<string, string> parametersCollection)
        {
            if (node.HasChildNodes)
            {
                foreach (XmlNode child in node.ChildNodes)
                {
                    handleNode(child, parametersCollection);
                }
            }
            else if (string.IsNullOrEmpty(node.Value?.Trim()) == false)
                parametersCollection.Add(node.ParentNode.LocalName, node.Value);
            
        }

    }
}
