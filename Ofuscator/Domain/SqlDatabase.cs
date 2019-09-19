﻿using Obfuscator.Entities;
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
            StatusChanged?.Invoke(new StatusInformation {Message = "Opening connection to database"}, null);

            var connection = new SqlConnection(this.ConnectionString);
            connection.Open();

            StatusChanged?.Invoke(new StatusInformation { Message = "Retrieving tables" }, null);

            RetrieveTables(connection);

            RetrieveTableColumns(connection);

            connection.Close();

            StatusChanged?.Invoke(new StatusInformation { Message = $"DONE" }, null);

            return this.Tables;
        }

        private void RetrieveTableColumns(SqlConnection connection)
        {
            var tableIndex = 0;
            var numberOfTables = this.Tables.Count();

            foreach (var table in this.Tables)
            {
                StatusChanged?.Invoke(new StatusInformation { Message = $"Retrieving columns for table: {table.Name}", Progress = ++tableIndex, Total = numberOfTables }, null);

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
            command.CommandText = "SELECT Table_Schema, Table_Name FROM Information_Schema.Tables WHERE Table_Type = 'BASE TABLE' ORDER BY Table_Schema, Table_Name";
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

        public string GetDatabaseName()
        {
            var connection = new SqlConnection(this.ConnectionString);
            return connection.Database;
        }

        public void RunOperation(Obfuscation obfuscationOperation, StatusInformation status = null)
        {
            IEnumerable<string> originData;

            var dataSet = GetTableData(obfuscationOperation);

            if (obfuscationOperation.Origin.DataSourceType == DataSourceType.NIFGenerator)
                originData = DniNie.GenerateNIF(dataSet.Tables[0].Rows.Count);
            else
                originData = GetSourceData(obfuscationOperation);

            if (status == null) status = new StatusInformation();

            dataSet = OfuscateDataset(obfuscationOperation, originData, dataSet);
            status.Message = $"...Saving obfuscation on {obfuscationOperation.Destination.TableName}";
            StatusChanged?.Invoke(status, null);
            PersistOfuscation(obfuscationOperation, dataSet);
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
                    Message = $"Obfuscating {obfuscation.Destination.TableName}.{obfuscation.Destination.ColumnInfo.Name}",
                    Progress = ++operationIndex,
                    Total = numberOfOperations
                };
                StatusChanged?.Invoke(status, null);
                RunOperation(obfuscation, status);
            }

            StatusChanged?.Invoke(new StatusInformation { Message = $"DONE" }, null);
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
            if (obfuscationOperation.Origin.DataSourceType == DataSourceType.CSV)
            {
                var csvFile = new CsvFile();
                csvFile.ReadFile(obfuscationOperation.Origin.DataSourceName);
                var columnContent = csvFile.GetContent(obfuscationOperation.Origin.ColumnIndex);
                return columnContent;
            }
            else
            {
                return null;
            }
        }
    }
}
