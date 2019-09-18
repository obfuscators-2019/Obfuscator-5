using Obfuscator.Domain;
using System;

namespace Obfuscator.Entities
{
    public class Obfuscation
    {
        public DataSourceInformation Origin { get; set; }
        public DbInfo Destination { get; set; }
    }
}
