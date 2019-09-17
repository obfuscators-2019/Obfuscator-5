using Newtonsoft.Json;
using Obfuscator.Entities;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace Obfuscator.Domain
{
    public class FileSerializer
    {
        public void SaveObfuscationOps(IEnumerable<Obfuscation> obfuscationOps, string fileName)
        {
            var jsonContent = JsonConvert.SerializeObject(obfuscationOps);
            var textWriter = new StreamWriter(fileName);
            textWriter.WriteLine(jsonContent);
            textWriter.Close();
        }

        public IEnumerable<Obfuscation> LoadObfuscationOps(string fileName)
        {
            var textReader = new StreamReader(fileName);
            var jsonContent = textReader.ReadToEnd();
            textReader.Close();
            var obfuscationOps = JsonConvert.DeserializeObject<List<Obfuscation>>(jsonContent);
            return obfuscationOps;
        }
    }
}
