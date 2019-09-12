namespace Ofuscator.Entities
{
    public class ColumnInfo
    {
        public string Name { get; set; }
        public int Index { get; set; }
        public bool IsNullable { get; set; }
        public string DataType { get; set; }
        public int CharacterMaxLength { get; set; }
    }
}