using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Xml;

namespace Julliet.Engine
{
    public class LetterReader
    {
        string requestLetter = string.Empty;
        public string responseLetter = string.Empty;

        public RestClient Client = new RestClient();

        Dictionary<string, string> navigatorCompass = new Dictionary<string, string>();

        //#1
        public LetterReader(string letter, string letterPath, string accept)
        {
            using (StreamReader sr = new StreamReader(File.OpenRead(Path.Combine(letterPath, letter, "Request.ltr"))))
            {
                requestLetter = sr.ReadToEnd();
            }

            using (StreamReader sr = new StreamReader(File.OpenRead(Path.Combine(letterPath, letter, "Response.ltr"))))
            {
                responseLetter = sr.ReadToEnd();
            }

            if (accept.Equals("text/xml"))
            {
                ParseResponseXmlLetter();

            }
            else
            {
                ParseResponseLetter();
            }
        }

        //#2
        private void ParseResponseLetter()
        {
            NavigateToMoustache(JObject.Parse(responseLetter));
        }

        //#2.1
        private void ParseResponseXmlLetter()
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(responseLetter);

            

            NavigateToMoustache(xDoc); 

                
            
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

        private void NavigateToMoustache(XmlNode node)
        {
            if (node.HasChildNodes)
            {
                foreach (XmlNode child in node.ChildNodes)
                {
                    NavigateToMoustache(child);
                }
            }
            else if(node.Value != null && node.Value.StartsWith("{{"))
                navigatorCompass.Add(node.ParentNode.LocalName, node.Value);
        }

        //#4
        public RestRequest GetRequestByLetter(Dictionary<string, string> requestValuesCollection, string contentType)
        {
            RestRequest request = new RestRequest();
            StringBuilder bodyBuilder = new StringBuilder();
            WebHeaderCollection headerCollection = new WebHeaderCollection();

            string url = string.Empty;
            string httpMethod = string.Empty;
            string headerContentType = string.Empty;
            string accept = string.Empty;

            foreach (string line in requestLetter.Split('\n'))
            {
                var _line = line.Trim();

                if (_line.StartsWith("->"))
                {
                    url = GetUrlByLine(_line);
                }
                else if (_line.StartsWith("##"))
                {
                    int ix = _line.IndexOf(":");

                    var headKey = _line.Substring(2, ix - 2);
                    var headValue = _line.Substring(ix + 1, _line.Length - (ix + 1));

                    if (headKey.ToLower().Equals("content-type", StringComparison.CurrentCultureIgnoreCase))
                    {
                        headerContentType = headValue;
                    }

                    request.AddHeader(headKey, headValue);
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

            contentType = string.IsNullOrEmpty(headerContentType) ? contentType : headerContentType;

            request.Method = (Method)Enum.Parse(typeof(Method), httpMethod, true);
            request.AddParameter(contentType, BindBody(bodyBuilder, requestValuesCollection), ParameterType.RequestBody);

            if (contentType.Contains("xml"))
            {
                request.RequestFormat = DataFormat.Xml;
            }
            else
            {
                request.RequestFormat = DataFormat.Json;
            }

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

        //#5.1
        public Dictionary<string, string> GetValuesFromResponseToXml(JObject jObject)
        {
            MockSetup();
            Dictionary<string, string> retVal = new Dictionary<string, string>();

            foreach (var item in navigatorCompasss)
            {
                retVal.Add(item.Key, jObject.SelectToken(item.Value)?.ToString() ?? string.Empty);
            }

            return retVal;
        }

        private void Seek(JToken jObject)
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

        private Dictionary<string, string> navigatorCompasss = new Dictionary<string, string>();

        private void MockSetup()
        {
            navigatorCompasss.Add("{{BuyerKey}}", "BuyerKey");
            navigatorCompasss.Add("{{MerchantKey}}", "MerchantKey");
            navigatorCompasss.Add("{{MundiPaggTimeInMilliseconds}}", "MundiPaggTimeInMilliseconds");
            navigatorCompasss.Add("{{OrderKey}}", "OrderKey");

            navigatorCompasss.Add("{{OrderReference}}", "OrderReference");
            navigatorCompasss.Add("{{OrderStatusEnum}}", "OrderStatusEnum");
            navigatorCompasss.Add("{{RequestKey}}", "RequestKey");
            navigatorCompasss.Add("{{Success}}", "Success");
            navigatorCompasss.Add("{{Version}}", "Version");


            navigatorCompasss.Add("{{AcquirerMessage}}", "CreditCardTransactionResultCollection[0].AcquirerMessage");
            navigatorCompasss.Add("{{AcquirerReturnCode}}", "CreditCardTransactionResultCollection[0].AcquirerReturnCode");
            navigatorCompasss.Add("{{AmountInCents}}", "CreditCardTransactionResultCollection[0].AmountInCents");
            navigatorCompasss.Add("{{AuthorizationCode}}", "CreditCardTransactionResultCollection[0].AuthorizationCode");
            navigatorCompasss.Add("{{AuthorizedAmountInCents}}", "CreditCardTransactionResultCollection[0].AuthorizedAmountInCents");
            navigatorCompasss.Add("{{MaskedCreditCardNumber}}", "CreditCardTransactionResultCollection[0].CreditCardNumber");
            navigatorCompasss.Add("{{CreditCardOperation}}", "CreditCardTransactionResultCollection[0].CreditCardOperationEnum");
            navigatorCompasss.Add("{{ExternalTimeInMilliseconds}}", "CreditCardTransactionResultCollection[0].ExternalTimeInMilliseconds");
            navigatorCompasss.Add("{{InstantBuyKey}}", "CreditCardTransactionResultCollection[0].InstantBuyKey");
            navigatorCompasss.Add("{{TransactionIdentifier}}", "CreditCardTransactionResultCollection[0].TransactionIdentifier");

            navigatorCompasss.Add("{{TransactionKey}}", "CreditCardTransactionResultCollection[0].TransactionKey");
            navigatorCompasss.Add("{{TransactionReference}}", "CreditCardTransactionResultCollection[0].TransactionReference");
            navigatorCompasss.Add("{{UniqueSequentialNumber}}", "CreditCardTransactionResultCollection[0].UniqueSequentialNumber");

        }
    }
}
