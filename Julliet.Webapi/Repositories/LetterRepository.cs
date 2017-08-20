using Julliet.Engine;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Julliet.Webapi.Repositories
{
    public class LetterRepository
    {

        public void WriteLetter(string content, string path, string name)
        {
            if (!Directory.Exists(path)) {
                Directory.CreateDirectory(path);
            }

            using (var fileStream = new FileStream(Path.Combine(path, name), FileMode.OpenOrCreate))
            using (var streamWriter = new StreamWriter(fileStream))
            {
                streamWriter.BaseStream.Position = 0;
                streamWriter.WriteLine(content);
            }

        }
    }
}
