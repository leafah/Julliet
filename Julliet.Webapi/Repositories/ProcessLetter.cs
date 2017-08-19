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

        private readonly string _letter;

        public ProcessLetter(string letter)
        {
            this._letter = letter;
        }

        public void ReadLetter(string request)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            var letter = System.IO.File.ReadAllText(this._letter);
            JObject json = JObject.Parse(request);
            foreach (var no in json.Values())
            {
                dic.Add(no.Path, no.Value<string>());
            }
            LetterParser lp = new LetterParser(letter);
            var a = lp.ParseToWebRequest(dic);
            var response = lp.client.Execute(a);
            
            //CONSIDERANDO ESSE RESPONSE
            //Pock p = new Pock();
            Dictionary<string, string> dicR = new Dictionary<string, string>();
            var jsonResponse = System.IO.File.ReadAllText(@"C:\Yuri\jsonResponse.txt");
            JObject jsonR = JObject.Parse(jsonResponse);
            foreach (var no in jsonR.Values())
            {
                dicR.Add(no.Path, no.Value<string>());
            }

            letter = System.IO.File.ReadAllText(@"C:\Yuri\LetterResponse.txt");
            lp = new LetterParser(letter);
            var r = lp.ParseToWebResponse(dicR);
            var retornandoResponse = lp.client.Execute(r);
        }

        public void WriteLetter()
        {
            throw new NotImplementedException();
        }
    }
}
