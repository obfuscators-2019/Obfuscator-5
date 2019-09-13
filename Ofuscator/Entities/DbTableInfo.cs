using System.Collections.Generic;

namespace Obfuscator.Entities
{
    public class DbTableInfo
    {
        public string Name { get; set; }
        public List<DbColumnInfo> Columns { get; set; }
    }
}