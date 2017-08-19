using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace Julliet.Engine
{
    public class Pock
    {
        private string template { get; set; }
        public RestClient client { get; set; }

        public Pock(string template)
        {
            this.template = template;
            this.client = new RestClient();
        }

        public RestRequest ParseToWebResponse(Dictionary<string, string> propertyLetter, Dictionary<string, string> propertyCollection)
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
            request.AddParameter("application/json", Testando(bodyBuilder, propertyLetter, propertyCollection), ParameterType.RequestBody);

            return request;
        }

        private string GetUrlByLine(string line)
        {
            int startUrlPosition = line.IndexOf("->", 0);
            int finishUrlPosition = line.IndexOf(" ", startUrlPosition);

            return line.Substring(startUrlPosition + 2, line.Length - 2);
        }

        private string Testando(StringBuilder bodyBuilder, Dictionary<string, string> propertyLetter, Dictionary<string, string> propertyCollection)
        {
            string body = bodyBuilder.ToString();

            foreach (var keyLetter in propertyLetter.Keys)
            {
                string conteudoInterno;
                propertyCollection.TryGetValue(keyLetter, out conteudoInterno);

                string conteudoName;
                propertyLetter.TryGetValue(keyLetter, out conteudoName);

                body = body.Replace($"{{{{{conteudoName}}}}}", $"{conteudoInterno}");
            }

            return body;
        }
        private string BindBody(StringBuilder bodyBuilder, Dictionary<string, string> propertyCollection)
        {
            string body = bodyBuilder.ToString();

            foreach (var prop in propertyCollection)
            {
                body += body.Replace($"{{{{{prop.Key}}}}}", $"\"{prop.Key}\":\"{prop.Value}\"");
            }

            return body;
        }

        public void teste(string json, string type)
        {
            if(type == "json")
            {
                JObject j = JObject.Parse(json);
                //foreach(var ab in j.Values())
                //{
                //    var parente = ab.Parent.Parent;
                //    JContainer abc;
                //    string path = "";
                //    while((abc = ab.Parent) != null)
                //    {
                //        path += abc.ToString();
                //    }
                //}
                //Dictionary<string, string> dic = new Dictionary<string, string>();
                //foreach (var no in j.Values())
                //{
                //    dic.Add(no.Path, no.Value<string>());
                //}
                //LetterParser lp = new LetterParser(letter);
                //var a = lp.ParseToWebRequest(dic);
            }
        }
    }
}
