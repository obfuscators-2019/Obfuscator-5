using Newtonsoft.Json;
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
        public ObfuscationParser() {}

        public ObfuscationParser(Obfuscation obfuscationOperation)
        {
            this.Origin = obfuscationOperation.Origin;
            this.Destination = obfuscationOperation.Destination;
        }

        [JsonIgnore]
        public string ReadableContent
        {
            get
            {
                var readableText = string.Empty;
                string originInformation = $"[{Origin.DataSourceType}]";
                if (Origin.DataSourceType == DataSourceType.CSV)
                {
                    var csvFileName = Path.GetFileName(Origin.DataSourceName).ToUpper();
                    var columnIndex = Origin.ColumnIndex;
                    var columnName = (Origin.HasHeaders ? $" [{Origin.ColumnName}]" : string.Empty).ToUpper();
                    originInformation += $" File(\"{csvFileName}\").Column({columnIndex}{columnName})";
                }
                var sqlDatabase = new SqlDatabase { ConnectionString = Destination.ConnectionString };
                string databaseName = sqlDatabase.GetDatabaseName();
                string tableName = Destination.TableName.ToUpper();
                string fieldName = Destination.ColumnInfo.Name.ToUpper();

                readableText = $"{originInformation} =>REPLACES=> Db({databaseName}).Table({tableName}).Field({fieldName})";

                return readableText;
            }
        }
    }
}
