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
                + $" WHERE COLUMNPROPERTY(id , name, 'IsIdentity') = 1 AND TableName = {tableName}"
                + $" ORDER BY 1, 2";
            var reader = command.ExecuteReader();
            while (reader.Read())
                columns.Add((string)reader["IdentityColumn"]);

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

            var idColumns = GetIdentityColumns(sqlConnection, obfuscationOperation.Destination.TableName);

            sqlConnection.Open();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter($"SELECT * FROM {obfuscationOperation.Destination.TableName}", sqlConnection);

            var valueParameter = (obfuscationOperation.Destination.ColumnInfo.DataType.ToLower().Contains("char") || true ? "'@value'" : "@value");
            string updateCommandText = $"UPDATE {obfuscationOperation.Destination.TableName}" +
                $" SET {obfuscationOperation.Destination.ColumnInfo.Name}={valueParameter}";

            sqlDataAdapter.UpdateCommand = new SqlCommand();

            var builder = new SqlCommandBuilder(sqlDataAdapter);
            var updateCommand = builder.GetUpdateCommand();
            Trace.WriteLine(updateCommand.Parameters);

            sqlDataAdapter.Update(dataSet);

            sqlConnection.Close();
        }

        private DataSet OfuscateDataset(Obfuscation obfuscationOperation, IEnumerable<string> data, DataSet dataSet)
        {
            var dataListMaxIndex = data.Count() - 1;
            var dataListIndex = 0;
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
