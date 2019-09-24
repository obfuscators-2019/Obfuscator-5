using Obfuscator.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace Obfuscator.Domain
{
    public static class CustomExtensions
    {
        public static void Shuffle<T>(this IList<T> list)
        {
            RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
            int n = list.Count;
            while (n > 1)
            {
                byte[] box = new byte[1];
                do provider.GetBytes(box);
                while (!(box[0] < n * (Byte.MaxValue / n)));
                int k = (box[0] % n);
                n--;
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }

    internal class DataRowComparer : IEqualityComparer<DataRow>
    {
        public bool Equals(DataRow x, DataRow y)
        {
            if (Object.ReferenceEquals(x, y)) return true;

            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null)) return false;

            if (x.ItemArray.Count() != y.ItemArray.Count()) return false;

            for (int i = 0; i < x.ItemArray.Count(); i++)
                if (!x.ItemArray[i].Equals(y.ItemArray[i])) return false;

            return true;
        }

        public int GetHashCode(DataRow dataRow)
        {
            if (Object.ReferenceEquals(dataRow, null)) return 0;
            if (dataRow.ItemArray.Count() == 0) return dataRow.GetHashCode();

            int hashCode = 0;

            for (int i = 0; i < dataRow.ItemArray.Count(); i++)
                hashCode ^= (dataRow.ItemArray[i] == null ? 0 : dataRow.ItemArray[i].GetHashCode());

            return hashCode;
        }
    }

    public class SqlDatabase
    {
        private static List<string> _knownDateTimeFormats = null;

        private List<string> KnownDateTimeFormats
        {
            get
            {
                if (_knownDateTimeFormats == null)
                {
                    var knownWeekDayFormats = new String[] { "", "ddd, ", "dddd, ", }; // "", Fri, Friday};

                    var knownDateFormats = new String[]
                    {
                        "d/M/yyyy", "dd/MM/yyyy",
                        "d-M-yyyy", "dd-MM-yyyy",

                        "M/d/yyyy", "MM/dd/yyyy",
                        "M-d-yyyy", "MM-dd-yyyy",

                        "yyyy/M/d", "yyyy/MM/dd",
                        "yyyy-M-d", "yyyy-MM-dd",

                        "dd MMMM yyyy", "yyyy MMMM dd"
                    };
                    var knownTimeFormats = new String[]
                    {
                        "",

                        " H:mm",
                        " HH:mm",
                        " HH:mm:ss",

                        " h:mm tt",
                        " hh:mm tt",
                        " hh:mm:ss tt",
                    };

                    _knownDateTimeFormats = knownWeekDayFormats.SelectMany(w => knownDateFormats.SelectMany(d => knownTimeFormats.Select(t => w + d + t))).ToList();
                }
                return _knownDateTimeFormats;
            }
        }

        /// <summary>
        /// PARAMETER object sender: SqlDatabase.StatusInformation
        /// </summary>
        public EventHandler StatusChanged;

        public class StatusInformation
        {
            public string Message { get; set; }
            public int Progress { get; set; }
            public int Total { get; set; }
        }

        public string ConnectionString { get; set; }

        public List<DbTableInfo> Tables { get; set; }

        public List<DbTableInfo> RetrieveDatabaseInfo()
        {
            StatusChanged?.Invoke(new StatusInformation { Message = "Opening connection to database" }, null);

            var connection = new SqlConnection(this.ConnectionString);
            connection.Open();

            StatusChanged?.Invoke(new StatusInformation { Message = "Retrieving tables" }, null);

            RetrieveTables(connection);

            // connection is closed inside
            RetrieveAllTableColumnsAsync(connection);

            return this.Tables;
        }

        public void RetrieveTableColumns(DbTableInfo table)
        {
            RetrieveTableColumns(table, null, null);
        }

        public string GetDatabaseName()
        {
            var connection = new SqlConnection(this.ConnectionString);
            return connection.Database;
        }

        public void RunOperations(IEnumerable<Obfuscation> obfuscationOps)
        {
            StatusChanged?.Invoke(new StatusInformation {Message = $"Starting operations..."}, null);

            var operationIndex = 0;
            var numberOfOperations = obfuscationOps.Count();

            foreach (var obfuscation in obfuscationOps)
            {
                var status = new StatusInformation
                {
                    Message = $"Obfuscating {obfuscation.Destination.Name}.({string.Join(",", obfuscation.Destination.Columns.Select(c => c.Name))})",
                    Progress = ++operationIndex,
                    Total = numberOfOperations
                };
                StatusChanged?.Invoke(status, null);
                RunOperation(obfuscation, status);
            }

            StatusChanged?.Invoke(new StatusInformation { Message = $"DONE" }, null);
        }

        public void RunOperation(Obfuscation obfuscationOperation, StatusInformation status = null)
        {
            IEnumerable<string> originData = null;

            var dataSet = GetTableData(obfuscationOperation);

            switch (obfuscationOperation.Origin.DataSourceType)
            {
                case DataSourceType.CSV:
                    originData = GetSourceData(obfuscationOperation);
                    break;
                case DataSourceType.DNIGenerator:
                    originData = DniNie.GenerateDNI(dataSet.Tables[0].Rows.Count);
                    break;
                case DataSourceType.NIEGenerator:
                    originData = DniNie.GenerateNIE(dataSet.Tables[0].Rows.Count);
                    break;
                case DataSourceType.NIFGenerator:
                    originData = DniNie.GenerateNIF(dataSet.Tables[0].Rows.Count);
                    break;
                case DataSourceType.Scramble:
                    var dataTableScrambled = ScrambleDataSet(obfuscationOperation.Destination, dataSet);
                    return;
                    break;
                default:
                    originData = new List<string>();
                    break;
            }

            if (status == null) status = new StatusInformation();

            dataSet = OfuscateDataset(obfuscationOperation, originData, dataSet);
            status.Message = $"...Saving obfuscation on {obfuscationOperation.Destination.Name}";
            StatusChanged?.Invoke(status, null);
            PersistOfuscation(obfuscationOperation, dataSet);
        }

        private void RetrieveTables(SqlConnection connection)
        {
            this.Tables = new List<DbTableInfo>();

            var command = connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = "SELECT Table_Schema, Table_Name FROM Information_Schema.Tables WHERE Table_Type = 'BASE TABLE' ORDER BY Table_Schema, Table_Name";
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var tableName = $"{reader["Table_Schema"]}.{reader["Table_Name"]}";
                var tableInfo = new DbTableInfo
                {
                    Name = tableName,
                    Columns = new List<DbColumnInfo>(),
                    ConnectionString = this.ConnectionString
                };
                this.Tables.Add(tableInfo);
            }
            reader.Close();
        }

        private void RetrieveAllTableColumnsAsync(SqlConnection connection)
        {
            var task = new Task(() =>
            {
                RetrieveTableColumns(connection);
                connection.Close();
                StatusChanged?.Invoke(new StatusInformation { Message = $"DONE" }, null);
            });
            task.Start();
        }

        private void RetrieveTableColumns(SqlConnection connection)
        {
            var status = new StatusInformation { Total = this.Tables.Count() };
            foreach (var table in this.Tables)
            {
                RetrieveTableColumns(table, connection, status);
                status.Progress++;
            }
        }

        private void RetrieveTableColumns(DbTableInfo table, SqlConnection connection, StatusInformation statusInfo)
        {
            if (table.Columns != null && table.Columns.Count > 0) return;

            if (statusInfo == null) statusInfo = new StatusInformation();
            statusInfo.Message = $"Retrieving columns for table: {table.Name}";
            StatusChanged?.Invoke(statusInfo, null);

            var connectionWasNull = (connection == null);
            if (connectionWasNull)
            {
                connection = new SqlConnection(this.ConnectionString);
                connection.Open();
            }

            table.Columns = new List<DbColumnInfo>();
            var command = connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = $"SELECT Column_Name, Ordinal_Position, Is_Nullable, Data_Type, Character_Maximum_Length" +
                $" FROM Information_Schema.Columns" +
                $" WHERE TABLE_SCHEMA + '.' + TABLE_NAME = '{table.Name}'" +
                $" ORDER BY ORDINAL_POSITION";
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                table.Columns.Add(new DbColumnInfo
                {
                    Name = (string)reader["Column_Name"],
                    Index = (int)reader["Ordinal_Position"],
                    IsNullable = ((string)reader["Is_Nullable"] == "YES"),
                    DataType = (string)reader["Data_Type"],
                    CharacterMaxLength = (reader["Character_Maximum_Length"] is DBNull ? 0 : (int)reader["Character_Maximum_Length"])
                });
            }
            reader.Close();

            statusInfo.Message = $"Retrieving columns for table: {table.Name} - DONE";
            StatusChanged?.Invoke(statusInfo, null);

            if (connectionWasNull) connection.Close();
        }

        private List<string> GetIdentityColumns(SqlConnection connection, string tableName)
        {
            var columns = new List<string>();

            var command = connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = $"SELECT OBJECT_SCHEMA_NAME(id) + '.' + OBJECT_NAME(id) as TableName, Name as IdentityColumn"
                + $" FROM syscolumns"
                + $" WHERE COLUMNPROPERTY(id , name, 'IsIdentity') = 1 AND OBJECT_SCHEMA_NAME(id) + '.' + OBJECT_NAME(id) = '{tableName}'"
                + $" ORDER BY 1, 2";
            var reader = command.ExecuteReader();
            while (reader.Read())
                columns.Add((string)reader["IdentityColumn"]);

            reader.Close();

            command = connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = $"SELECT schema_name(ta.schema_id) + '.' + ta.name as TableName, col.name ColumnName"
                + $" FROM sys.tables ta"
                + $" INNER JOIN sys.indexes ind ON ind.object_id = ta.object_id"
                + $" INNER JOIN sys.index_columns indcol ON indcol.object_id = ta.object_id AND indcol.index_id = ind.index_id"
                + $" INNER JOIN sys.columns col ON col.object_id = ta.object_id AND col.column_id = indcol.column_id"
                + $" WHERE schema_name(ta.schema_id) + '.' + ta.name = '{tableName}' AND (ind.is_primary_key = 1 OR ind.is_unique = 1)";

            reader = command.ExecuteReader();
            while (reader.Read())
                if (!columns.Contains((string)reader["ColumnName"])) columns.Add((string)reader["ColumnName"]);

            reader.Close();

            return columns;
        }

        private void PersistOfuscation(Obfuscation obfuscationOperation, DataSet dataSet)
        {
            var sqlConnection = new SqlConnection(obfuscationOperation.Destination.ConnectionString);
            sqlConnection.Open();

            var idColumns = GetIdentityColumns(sqlConnection, obfuscationOperation.Destination.Name);
            var updateQuery = $"UPDATE {obfuscationOperation.Destination.Name}" +
                $" SET {obfuscationOperation.Destination.Columns[0].Name} = @param_{obfuscationOperation.Destination.Columns[0].Name}" +
                $" WHERE {obfuscationOperation.Destination.Columns[0].Name} = @param_old_{obfuscationOperation.Destination.Columns[0].Name}";
            if (idColumns.Any()) 
                foreach (var idColumn in idColumns) updateQuery += $" AND {idColumn}=@param_{idColumn}";

            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                var updateCommand = sqlConnection.CreateCommand();
                updateCommand.CommandType = CommandType.Text;
                updateCommand.CommandText = updateQuery;
                updateCommand.Parameters.AddWithValue($"param_{obfuscationOperation.Destination.Columns[0].Name}", row[obfuscationOperation.Destination.Columns[0].Name]);
                updateCommand.Parameters.AddWithValue($"param_old_{obfuscationOperation.Destination.Columns[0].Name}", row[obfuscationOperation.Destination.Columns[0].Name, DataRowVersion.Original]);
                if (idColumns.Any())
                    foreach (var idColumn in idColumns) updateCommand.Parameters.AddWithValue($"param_{idColumn}", row[idColumn]);
                updateCommand.ExecuteNonQuery();
            }

            sqlConnection.Close();
        }

        private DataSet OfuscateDataset(Obfuscation obfuscationOperation, IEnumerable<string> data, DataSet dataSet)
        {            
            var dataListIndex = 0;
            var dataListMaxIndex = data.Count() - 1;
            foreach (var row in dataSet.Tables[0].Rows)
            {
                var dataRow = (DataRow)row;
                if (dataRow[obfuscationOperation.Destination.Columns[0].Name] is DateTime)
                    dataRow[obfuscationOperation.Destination.Columns[0].Name] = ParseDateTime(data.ElementAt(dataListIndex));
                else
                    dataRow[obfuscationOperation.Destination.Columns[0].Name] = data.ElementAt(dataListIndex);

                dataListIndex++;
                if (dataListIndex > dataListMaxIndex) dataListIndex = 0;
            }

            return dataSet;
        }

        private DataTable ScrambleDataSet(DbTableInfo destination, DataSet dataSet)
        {
            var scrambledResult = dataSet.Tables[0].Copy();
            scrambledResult.Clear();
            var scrambleColumns = destination.Columns.Where(c => !c.IsGroupColumn).ToList();

            if (destination.Columns.Any(c => c.IsGroupColumn))
            {
                var groupColumns = destination.Columns.Where(c => c.IsGroupColumn).ToList();
                var distinctValueGroups = GetColumns(dataSet.Tables[0], groupColumns)
                    .AsEnumerable()
                    .Distinct(new DataRowComparer());

                foreach (var valueGroup in distinctValueGroups)
                {
                    var filter = string.Empty;
                    for (int i = 0; i < groupColumns.Count(); i++)
                        filter += $"AND {groupColumns[i].Name}={valueGroup[i].ToString()} ";

                    var filteredDataTable = dataSet.Tables[0].Select(filter.Substring(3)).CopyToDataTable();
                    ScrambleColumnValues(scrambleColumns, filteredDataTable, scrambledResult);
                }
            }
            else
                ScrambleColumnValues(scrambleColumns, dataSet.Tables[0], scrambledResult);

            return scrambledResult;
        }

        private void ScrambleColumnValues(List<DbColumnInfo> columnsToScramble, DataTable dataTableToScramble, DataTable scrambledResult)
        {
            var scrambledRows = GetColumns(dataTableToScramble, columnsToScramble)
                .AsEnumerable()
                .OrderBy(dr => Guid.NewGuid())
                .ToList();

            for (int i = 0; i < dataTableToScramble.Rows.Count; i++)
            {
                foreach (var column in columnsToScramble)
                    dataTableToScramble.Rows[i][column.Name] = scrambledRows[i][column.Name];

                scrambledResult.Rows.Add(dataTableToScramble.Rows[i].ItemArray);
            }
        }

        private DataTable GetColumns(DataTable dataTable, IEnumerable<DbColumnInfo> groupColumns)
        {
            var groupColumnsTable = dataTable.AsEnumerable().CopyToDataTable();
            foreach (DataColumn dataColumn in dataTable.Columns)
            {
                if (!groupColumns.Any(gc => gc.Name == dataColumn.ColumnName))
                    groupColumnsTable.Columns.Remove(dataColumn.ColumnName);
            }

            return groupColumnsTable;
        }        

        private DateTime ParseDateTime(String value)
        {
            DateTime dateValue;
            foreach (var format in KnownDateTimeFormats)
            {
                if (DateTime.TryParseExact(value, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateValue))
                {
                    MoveFormatToFirstInTheList(format);
                    return dateValue;
                }
            }
            throw new FormatException($"Unknown Date-Time format: {value}. Known formats: {string.Join(",", KnownDateTimeFormats)}");
        }

        private void MoveFormatToFirstInTheList(string format)
        {
            KnownDateTimeFormats.Remove(format);
            KnownDateTimeFormats.Insert(0, format);
        }

        private DataSet GetTableData(Obfuscation obfuscationOperation)
        {
            var sqlQuery = $"SELECT * FROM {obfuscationOperation.Destination.Name}";

            sqlQuery = AddOrderClauseToQuery(obfuscationOperation, sqlQuery);

            var sqlConnection = new SqlConnection(obfuscationOperation.Destination.ConnectionString);
            DataSet dataSet = new DataSet();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlQuery, sqlConnection);
            sqlDataAdapter.Fill(dataSet);

            return dataSet;
        }

        private static string AddOrderClauseToQuery(Obfuscation obfuscationOperation, string sqlQuery)
        {
            var orderByClause = string.Empty;
            foreach (var columnInfo in obfuscationOperation.Destination.Columns.Where(c => c.IsGroupColumn))
                orderByClause += $", " + columnInfo.Name;
            if (orderByClause.Length > 0)
                sqlQuery += " ORDER BY " + orderByClause.Substring(1);

            return sqlQuery;
        }

        private IEnumerable<string> GetSourceData(Obfuscation obfuscationOperation)
        {
            if (obfuscationOperation.Origin.DataSourceType != DataSourceType.CSV)
                return null;

            var csvFile = new CsvFile();
            csvFile.ReadFile(obfuscationOperation.Origin.DataSourceName);
            var columnContent = csvFile.GetContent(obfuscationOperation.Origin.ColumnIndex);
            return columnContent;
        }
    }

}
