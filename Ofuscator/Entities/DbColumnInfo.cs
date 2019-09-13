namespace Obfuscator.Entities
{
    public class DbColumnInfo
    {
        public string Name { get; set; }
        public int Index { get; set; }
        public bool IsNullable { get; set; }
        public string DataType { get; set; }
        public int CharacterMaxLength { get; set; }
    }
}