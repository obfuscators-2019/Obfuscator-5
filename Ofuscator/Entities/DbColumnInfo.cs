using System;

namespace Obfuscator.Entities
{
    public class DbColumnInfo
    {
        public string Name { get; set; }
        public int Index { get; set; }
        public bool IsNullable { get; set; }
        public string DataType { get; set; }
        public int CharacterMaxLength { get; set; }
        public bool IsGroupColumn { get; set; } = false;
    }
}