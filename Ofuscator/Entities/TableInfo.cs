using System.Collections.Generic;

namespace Ofuscator.Entities
{
    public class TableInfo
    {
        public string Name { get; set; }
        public List<ColumnInfo> Columns { get; set; }
    }
}