using Obfuscator.Domain;
using Obfuscator.Entities;
using Obfuscator.Services;
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
            var ofuscation = new Obfuscation { DataPersistence = new SqlDataPersistence() };
            ofuscation.StatusChanged += ReportProgressToConsole;
            IEnumerable<ObfuscationInfo> obfuscationOps = LoadOperations();
            ofuscation.RunOperations(obfuscationOps);
        }

        private void ReportProgressToConsole(object callbackInformation, EventArgs e)
        {
            var statusInformation = (StatusInformation)callbackInformation;

            Console.WriteLine($"{statusInformation.Message}: {statusInformation.Progress}/{statusInformation.Total}");
        }

        private IEnumerable<Entities.ObfuscationInfo> LoadOperations()
        {
            var serializer = new FileSerializer();
            var obfuscationOps = serializer.LoadObfuscationOps(fileOfuscationOps);
            return obfuscationOps;
        }
    }
}
