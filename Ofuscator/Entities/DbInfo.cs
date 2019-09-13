using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obfuscator.Entities
{
    public class DbInfo
    {
        public string ConnectionString { get; set; }
        public string TableName { get; set; }
        public DbColumnInfo ColumnInfo { get; set; }
    }
}
