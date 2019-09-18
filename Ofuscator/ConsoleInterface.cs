using Obfuscator.Domain;
using Obfuscator.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obfuscator
{
    public class ConsoleInterface
    {
        private string fileOfuscationOps;
        public ConsoleInterface()
        {

        }

        public ConsoleInterface(string fileOfuscationOps)
        {
            this.fileOfuscationOps = fileOfuscationOps;
        }

        internal void RunOperations()
        {
            var sqlDb = new SqlDatabase();
            sqlDb.StatusChanged += ReportProgressToConsole;
            IEnumerable<Obfuscation> obfuscationOps = LoadOperations();
            sqlDb.RunOperations(obfuscationOps);
        }

        private void ReportProgressToConsole(object sender, EventArgs e)
        {
            var statusInformation = (SqlDatabase.StatusInformation)sender;

            Console.WriteLine($"{statusInformation.Message}: {statusInformation.Progress}/{statusInformation.Total}");
        }

        private IEnumerable<Entities.Obfuscation> LoadOperations()
        {
            var serializer = new FileSerializer();
            var obfuscationOps = serializer.LoadObfuscationOps(fileOfuscationOps);
            return obfuscationOps;
        }
    }
}
