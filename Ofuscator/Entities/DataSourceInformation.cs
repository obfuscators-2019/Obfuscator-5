namespace Obfuscator.Entities
{
    public class DataSourceInformation
    {
        public string DataSourceName { get; set; }
        public bool HasHeaders { get; set; } = true;
        public int ColumnIndex { get; set; }
        public string ColumnName { get; set; }
    }
}
