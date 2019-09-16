using Obfuscator.Domain;
using Obfuscator.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obfuscator.UI
{
    public class ObfuscationParser : Obfuscation
    {
        public string ReadableContent
        {
            get
            {
                var readableText = string.Empty;

                var csvFileName = Path.GetFileName(Origin.FileName).ToUpper();
                var columnIndex = Origin.ColumnIndex;
                var columnName = (Origin.HasHeaders ? $" [{Origin.ColumnName}]" : string.Empty).ToUpper();

                var sqlDatabase = new SqlDatabase { ConnectionString = Destination.ConnectionString };
                string databaseName = sqlDatabase.GetDatabaseName();
                string tableName = Destination.TableName.ToUpper();
                string fieldName = Destination.ColumnInfo.Name.ToUpper();

                readableText = $"File(\"{csvFileName}\").Column({columnIndex}{columnName}) =>REPLACES=> Db({databaseName}).Table({tableName}).Field({fieldName})";

                return readableText;

            }
        }
    }
}
