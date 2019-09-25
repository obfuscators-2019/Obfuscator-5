using Obfuscator.Domain;
using System;

namespace Obfuscator.Entities
{
    public class ObfuscationInfo
    {
        public DataSourceInformation Origin { get; set; }

        public DbTableInfo Destination { get; set; }
    }
}
