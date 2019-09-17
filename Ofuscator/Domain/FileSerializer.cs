using Newtonsoft.Json;
using Obfuscator.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obfuscator.Domain
{
    public class FileSerializer
    {
        public void SaveObfuscationOperations(IEnumerable<Obfuscation> obfuscationOperations, string fileName)
        {
            var jsonContent = JsonConvert.SerializeObject(obfuscationOperations);
            var textWriter = new StreamWriter(fileName);
            textWriter.WriteLine(jsonContent);
            textWriter.Close();
        }
    }
}
