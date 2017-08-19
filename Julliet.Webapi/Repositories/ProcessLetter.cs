using Julliet.Engine;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Julliet.Webapi.Repositories
{
    public class ProcessLetter
    {
        public void ReadLetter(string request)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            var letter = System.IO.File.ReadAllText(@"C:\Yuri\WriteText.txt");
            JObject json = JObject.Parse(request);
            foreach (var no in json.Values())
            {
                dic.Add(no.Path, no.Value<string>());
            }

            LetterParser lp = new LetterParser(letter);
            var a = lp.ParseToWebRequest(dic);
            var response = lp.client.Execute(a);
        }

        public void WriteLetter()
        {
            throw new NotImplementedException();
        }
    }
}
