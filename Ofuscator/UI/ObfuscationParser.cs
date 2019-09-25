using Newtonsoft.Json;
using Obfuscator.Domain;
using Obfuscator.Entities;
using Obfuscator.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obfuscator.UI
{
    public class ObfuscationParser : ObfuscationInfo
    {
        public ObfuscationParser() {}

        public ObfuscationParser(ObfuscationInfo obfuscationOperation)
        {
            this.Origin = obfuscationOperation.Origin;
            this.Destination = obfuscationOperation.Destination;
        }

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

                var sqlDatabase = new SqlDataPersistence { ConnectionString = Destination.ConnectionString };
                string databaseName = sqlDatabase.GetDatabaseName();
                string tableName = Destination.Name.ToUpper();

                var groupingInformation = string.Empty;
                var groupingFields = Destination.Columns.Where(g => g.IsGroupColumn).Select(c => c.Name);
                if (groupingFields.Any()) groupingInformation = $".GroupBy({string.Join(",", groupingFields)})";

                var fieldNames = string.Join(",", Destination.Columns.Where(x => !x.IsGroupColumn).Select(c => c.Name));

                readableText = $"{originInformation} =>REPLACES=> Db({databaseName}).Table({tableName}){groupingInformation}.Fields({fieldNames})";

                return readableText;
            }
        }
    }
}
