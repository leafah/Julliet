using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;

namespace Julliet.Engine
{
    public class LetterReader
    {
        string requestLetter = string.Empty;
        string responseLetter = string.Empty;

        public RestClient Client = new RestClient();

        Dictionary<string, string> navigatorCompass = new Dictionary<string, string>();

        //#1
        public LetterReader(string letter, string letterPath)
        {
            using (StreamReader sr = new StreamReader(File.OpenRead(Path.Combine(letterPath, letter,"Request.ltr"))))
            {
                requestLetter = sr.ReadToEnd();
            }

            using (StreamReader sr = new StreamReader(File.OpenRead(Path.Combine(letterPath, letter, "Response.ltr"))))
            {
                responseLetter = sr.ReadToEnd();
            }
            

            ParseResponseLetter();
        }        

        //#2
        private void ParseResponseLetter()
        {
            NavigateToMoustache(JObject.Parse(responseLetter));
        }

        //#3
        private void NavigateToMoustache(JToken jObject)
        {
            foreach (var item in jObject.Children<JToken>())
            {
                if (item.HasValues && item.First.ToString().StartsWith("{{"))
                {
                    var moustacheToken = item.First;
                    navigatorCompass.Add(moustacheToken.ToString().Replace("{", "").Replace("}", "").Trim(), moustacheToken.Path);
                }
                else
                {
                    NavigateToMoustache(item);
                }
            }
        }

        //#4
        public RestRequest GetRequestByLetter(Dictionary<string,string> requestValuesCollection)
        {
            RestRequest request = new RestRequest();
            StringBuilder bodyBuilder = new StringBuilder();
            WebHeaderCollection headerCollection = new WebHeaderCollection();

            string url = string.Empty;
            string httpMethod = string.Empty;

            foreach (string line in requestLetter.Split('\n'))
            {
                var _line = line.Trim();

                if (_line.StartsWith("->"))
                {
                    url = GetUrlByLine(_line);
                }
                else if (_line.StartsWith("##"))
                {
                    var headerSplitted = _line.Substring(2, _line.Length - 2).Trim().Split(':');

                    request.AddHeader(headerSplitted[0], headerSplitted[1]);
                }
                else if (_line.StartsWith("#"))
                {
                    httpMethod = _line.Substring(1, _line.Length - 1).Trim();
                }
                else
                {
                    bodyBuilder.AppendLine(_line);
                }
            };

            Client.BaseUrl = new Uri(url);

            request.Method = (Method)Enum.Parse(typeof(Method), httpMethod, true);
            request.AddParameter("application/json",BindBody(bodyBuilder, requestValuesCollection), ParameterType.RequestBody);

            return request;
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

        private string GetUrlByLine(string line)
        {
            int startUrlPosition = line.IndexOf("->", 0);
            int finishUrlPosition = line.IndexOf(" ", startUrlPosition);

            return line.Substring(startUrlPosition + 2, line.Length - 2);
        }

        //#5
        public Dictionary<string, string> GetValuesFromResponse(JObject jObject)
        {
            Dictionary<string, string> retVal = new Dictionary<string, string>();

            foreach (var item in navigatorCompass)
            {
                retVal.Add(item.Key, jObject.SelectToken(item.Value).ToString());
            }
            return retVal;
        }
    }
}
