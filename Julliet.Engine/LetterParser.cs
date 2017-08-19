using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Julliet.Engine
{
    public class LetterParser
    {
        private string template { get; set; }

        //public RestRequest Request { get; private set; }

        public RestClient client { get; set; }

        public LetterParser(string template)
        {
            this.template = template;

            this.client = new RestClient();
        }        

        public RestRequest ParseToWebRequest(Dictionary<string, string> propertyCollection)
        {

            var request = new RestRequest();

            StringBuilder bodyBuilder = new StringBuilder();

            foreach (string line in template.Split('\n'))
            {
                var fmtLine = line.Trim();

                if (fmtLine.StartsWith("->"))
                {
                    client.BaseUrl = new Uri(GetUrlByLine(fmtLine));
                }
                else if (fmtLine.StartsWith("##"))
                {
                    var headerSplitted = fmtLine.Substring(2, fmtLine.Length - 2).Trim().Replace('\"', ' ').Split(':');
                    request.AddHeader(headerSplitted[0], headerSplitted[1]);
                }
                else if (fmtLine.StartsWith("#"))
                {
                    Method metodo;
                    Enum.TryParse<Method>(fmtLine.Substring(1, fmtLine.Length - 1).Trim(), out metodo);
                    request.Method = metodo;
                }
                else
                {
                    bodyBuilder.AppendLine(fmtLine);
                }
            };
            request.AddParameter("application/json", BindBody(bodyBuilder, propertyCollection), ParameterType.RequestBody); 

            return request;
        }

        private string GetUrlByLine(string line)
        {
            int startUrlPosition = line.IndexOf("->", 0);
            int finishUrlPosition = line.IndexOf(" ", startUrlPosition);

            return line.Substring(startUrlPosition + 2, line.Length - 2);
        }

        private string BindBody(StringBuilder bodyBuilder, Dictionary<string, string> propertyCollection)
        {
            string body = bodyBuilder.ToString();

            foreach (var prop in propertyCollection)
            {
                body = body.Replace($"{{{{{prop.Key}}}}}", $"{prop.Value}");
            }

            return body;
        }

        public RestRequest ParseToWebResponse(Dictionary<string, string> propertyCollection)
        {
            var request = new RestRequest();

            StringBuilder bodyBuilder = new StringBuilder();

            foreach (string line in template.Split('\n'))
            {
                var fmtLine = line.Trim();

                if (fmtLine.StartsWith("->"))
                {
                    client.BaseUrl = new Uri(GetUrlByLine(fmtLine));
                }
                else if (fmtLine.StartsWith("##"))
                {
                    var headerSplitted = fmtLine.Substring(2, fmtLine.Length - 2).Trim().Replace('\"', ' ').Split(':');
                    request.AddHeader(headerSplitted[0], headerSplitted[1]);
                }
                else if (fmtLine.StartsWith("#"))
                {
                    Method metodo;
                    Enum.TryParse<Method>(fmtLine.Substring(1, fmtLine.Length - 1).Trim(), out metodo);
                    request.Method = metodo;
                }
                else
                {
                    bodyBuilder.AppendLine(fmtLine);
                }
            };
            request.AddParameter("application/json", Testando(bodyBuilder, propertyCollection), ParameterType.RequestBody);

            return request;
        }

        private string Testando(StringBuilder bodyBuilder, Dictionary<string, string> propertyCollection)
        {
            string body = bodyBuilder.ToString();
            Dictionary<string, string> propertyLetter = new Dictionary<string, string>();

            JObject jsonR = JObject.Parse(body);
            foreach (var no in jsonR.Values())
            {
                propertyLetter.Add(no.Path, no.Value<string>());
            }

            foreach (var keyLetter in propertyLetter.Keys)
            {
                string conteudoInterno;
                propertyCollection.TryGetValue(keyLetter, out conteudoInterno);

                string conteudoName;
                propertyLetter.TryGetValue(keyLetter, out conteudoName);

                body = body.Replace(conteudoName, $"{conteudoInterno}");
                body = body.Replace(keyLetter, conteudoName.Substring(2, conteudoName.Length - 4));
            }

            return body;
        }

        //private Dictionary<string, string> ParseToDictionary(string template, string rawResponse)
        //{
        //    Dictionary<string, string> navigate = new Dictionary<string, string>();

        //    JObject data = JsonConvert.DeserializeObject<JObject>(template);

        //    data.Value()

        //    foreach (var item in data.Descendants())
        //    {

        //    }
        //}
    }
}
