namespace Obfuscator.Entities
{
    public class CsvInformation
    {
        public string FileName { get; set; }
        public bool HasHeaders { get; set; } = true;
        public int ColumnIndex { get; set; }
        public string ColumnName { get; set; }
    }
}
