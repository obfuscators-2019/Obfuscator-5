using Obfuscator.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Obfuscator.Domain
{
    public class SqlDatabase
    {
        public EventHandler StatusChanged;

        public struct StatusInformation
        {
            public string Message { get; set; }
            public int Progress { get; set; }
            public int Total { get; set; }
        }

        public string ConnectionString { get; set; }

        public List<DbTableInfo> Tables { get; set; }

        public List<DbTableInfo> RetrieveDatabaseInfo()
        {
            var connection = new SqlConnection(this.ConnectionString);
            connection.Open();

            RetrieveTables(connection);
            RetrieveTableColumns(connection);

            connection.Close();

            return this.Tables;
        }

        private void RetrieveTableColumns(SqlConnection connection)
        {
            foreach (var table in this.Tables)
            {
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
            }
        }

        private void RetrieveTables(SqlConnection connection)
        {
            this.Tables = new List<DbTableInfo>();

            var command = connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = "SELECT Table_Schema, Table_Name FROM Information_Schema.Tables WHERE Table_Type = 'BASE TABLE'";
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var tableName = $"{reader["Table_Schema"]}.{reader["Table_Name"]}";
                var tableInfo = new DbTableInfo
                {
                    Name = tableName,
                    Columns = new List<DbColumnInfo>()
                };
                this.Tables.Add(tableInfo);
            }
            reader.Close();
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
            command.CommandText = $"SELECT tc.TABLE_SCHEMA,tc.TABLE_NAME,ccu.COLUMN_NAME"
                + $" FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS tc"
                + $"     JOIN INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE ccu ON tc.CONSTRAINT_NAME = ccu.Constraint_name"
                + $" WHERE tc.CONSTRAINT_TYPE = 'Primary Key'"
                + $"     AND tc.TABLE_SCHEMA + '.' + tc.TABLE_NAME = '{tableName}'";
            reader = command.ExecuteReader();
            while (reader.Read())
                if (!columns.Contains((string)reader["COLUMN_NAME"])) columns.Add((string)reader["COLUMN_NAME"]);

            reader.Close();


            return columns;
        }

        public string GetDatabaseName()
        {
            var connection = new SqlConnection(this.ConnectionString);
            return connection.Database;
        }

        public void RunOperation(Obfuscation obfuscationOperation)
        {
            var data = GetSourceData(obfuscationOperation);
            var dataSet = GetTableData(obfuscationOperation);
            dataSet = OfuscateDataset(obfuscationOperation, data, dataSet);
            PersistOfuscation(obfuscationOperation, dataSet);
        }

        public void RunOperations(IEnumerable<Obfuscation> obfuscationOps)
        {
            ReportStatusAsync(new StatusInformation
            {
                Message = $"",
                Progress = 0,
                Total = 0
            });

            var operationIndex = 0;
            var numberOfOperations = obfuscationOps.Count();

            foreach (var obfuscation in obfuscationOps)
            {
                ReportStatusAsync(new StatusInformation
                {
                    Message = $"Ofuscating {obfuscation.Destination.TableName}.{obfuscation.Destination.ColumnInfo.Name}",
                    Progress = operationIndex++,
                    Total = numberOfOperations
                });
                RunOperation(obfuscation);
            }

            ReportStatusAsync(new StatusInformation
            {
                Message = $"DONE",
                Progress = 0,
                Total = 0
            });
        }

        private void ReportStatusAsync(StatusInformation statusInformation)
        {
            var task = new Task(() =>
            {
                StatusChanged(statusInformation, null);
            });
            task.Start();
        }

        private void PersistOfuscation(Obfuscation obfuscationOperation, DataSet dataSet)
        {
            var sqlConnection = new SqlConnection(obfuscationOperation.Destination.ConnectionString);
            sqlConnection.Open();

            var idColumns = GetIdentityColumns(sqlConnection, obfuscationOperation.Destination.TableName);
            var updateQuery = $"UPDATE {obfuscationOperation.Destination.TableName}" +
                $" SET {obfuscationOperation.Destination.ColumnInfo.Name} = @param_{obfuscationOperation.Destination.ColumnInfo.Name}" +
                $" WHERE {obfuscationOperation.Destination.ColumnInfo.Name} = @param_old_{obfuscationOperation.Destination.ColumnInfo.Name}";
            if (idColumns.Any()) 
                foreach (var idColumn in idColumns) updateQuery += $" AND {idColumn}=@param_{idColumn}";

            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                var updateCommand = sqlConnection.CreateCommand();
                updateCommand.CommandType = CommandType.Text;
                updateCommand.CommandText = updateQuery;
                updateCommand.Parameters.AddWithValue($"param_{obfuscationOperation.Destination.ColumnInfo.Name}", row[obfuscationOperation.Destination.ColumnInfo.Name]);
                updateCommand.Parameters.AddWithValue($"param_old_{obfuscationOperation.Destination.ColumnInfo.Name}", row[obfuscationOperation.Destination.ColumnInfo.Name, DataRowVersion.Original]);
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
                dataRow[obfuscationOperation.Destination.ColumnInfo.Name] = data.ElementAt(dataListIndex);

                dataListIndex++;
                if (dataListIndex > dataListMaxIndex) dataListIndex = 0;
            }

            return dataSet;
        }

        private DataSet GetTableData(Obfuscation obfuscationOperation)
        {
            var sqlConnection = new SqlConnection(obfuscationOperation.Destination.ConnectionString);
            DataSet dataSet = new DataSet();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter($"SELECT * FROM {obfuscationOperation.Destination.TableName}", sqlConnection);
            sqlDataAdapter.Fill(dataSet);
            return dataSet;
        }

        private IEnumerable<string> GetSourceData(Obfuscation obfuscationOperation)
        {
            var csvFile = new CsvFile();
            csvFile.ReadFile(obfuscationOperation.Origin.FileName);
            var columnContent = csvFile.GetContent(obfuscationOperation.Origin.ColumnIndex);
            return columnContent;
        }
    }
}
