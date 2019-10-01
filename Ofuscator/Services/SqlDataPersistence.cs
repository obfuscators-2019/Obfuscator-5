using Obfuscator.Domain;
using Obfuscator.Entities;
using Obfuscator.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Obfuscator.Services
{
    public class SqlDataPersistence : IDataPersistence
    {
        private SqlConnection _connection = null;

        public string ConnectionString { get; set; }

        public EventHandler StatusChanged { get; set; }

        public List<DbTableInfo> RetrieveTables()
        {
            StatusChanged?.Invoke(new StatusInformation { Message = "Opening connection to database" }, null);
            OpenConnection();

            StatusChanged?.Invoke(new StatusInformation { Message = "Retrieving tables" }, null);
            List<DbTableInfo> tables = RetrieveTables(_connection);

            StatusChanged?.Invoke(new StatusInformation { Message = "Retrieving tables - DONE" }, null);
            CloseConnection();

            return tables;
        }

        private List<DbTableInfo> RetrieveTables(SqlConnection connection)
        {
            var tables = new List<DbTableInfo>();

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
                tables.Add(tableInfo);
            }
            reader.Close();

            return tables;
        }

        public string GetDatabaseName()
        {
            var connection = new SqlConnection(this.ConnectionString);
            return connection.Database;
        }

        public void RetrieveAllTableColumnsAsync(List<DbTableInfo> tables, CancellationTokenSource cancellationTokenSource)
        {
            var task = new Task(() =>
            {
                RetrieveTableColumns(tables, cancellationTokenSource);
                var statusMessage = (cancellationTokenSource.IsCancellationRequested ? "CANCELLED" : "DONE");
                StatusChanged?.Invoke(new StatusInformation { Message = statusMessage }, null);
            }, cancellationTokenSource.Token);
            task.Start();
        }

        public void RetrieveTableColumns(DbTableInfo table, StatusInformation statusInfo)
        {
            if (table.Columns != null && table.Columns.Count > 0) return;

            if (statusInfo == null) statusInfo = new StatusInformation();
            statusInfo.Message = $"Retrieving columns for table: {table.Name}";
            StatusChanged?.Invoke(statusInfo, null);

            var connectionWasNull = (_connection == null);
            if (connectionWasNull) OpenConnection();

            table.Columns = new List<DbColumnInfo>();
            var command = _connection.CreateCommand();
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
                    Index = ((int)reader["Ordinal_Position"]) - 1,
                    IsNullable = ((string)reader["Is_Nullable"] == "YES"),
                    DataType = (string)reader["Data_Type"],
                    CharacterMaxLength = (reader["Character_Maximum_Length"] is DBNull ? 0 : (int)reader["Character_Maximum_Length"])
                });
            }
            reader.Close();

            statusInfo.Message = $"Retrieving columns for table: {table.Name} - DONE";
            StatusChanged?.Invoke(statusInfo, null);

            if (connectionWasNull) CloseConnection();
        }

        public DataSet GetTableData(ObfuscationInfo obfuscationOperation)
        {
            var sqlQuery = $"SELECT * FROM {obfuscationOperation.Destination.Name}";

            sqlQuery = AddOrderClauseToQuery(obfuscationOperation, sqlQuery);

            OpenConnection();

            DataSet dataSet = new DataSet();
            var sqlDataAdapter = new SqlDataAdapter(sqlQuery, _connection);
            sqlDataAdapter.Fill(dataSet);
            
            CloseConnection();

            return dataSet;
        }

        public void PersistOfuscation(ObfuscationInfo obfuscationOperation, DataSet dataSet)
        {
            OpenConnection();

            var idColumns = GetIdentityColumns( obfuscationOperation.Destination.Name);
            var updateQuery = $"UPDATE {obfuscationOperation.Destination.Name} SET ";

            foreach (var valueColumn in obfuscationOperation.Destination.Columns.Where(c => !c.IsGroupColumn))
                updateQuery += $", {valueColumn.Name}=@param_{valueColumn.Name}";

            updateQuery = updateQuery.Replace("SET ,", "SET");
            updateQuery += " WHERE";

            foreach (var valueColumn in obfuscationOperation.Destination.Columns.Where(c => !c.IsGroupColumn))
                updateQuery += $" AND {valueColumn.Name}=@param_old_{valueColumn.Name}";

            foreach (var groupColumn in obfuscationOperation.Destination.Columns.Where(gc => gc.IsGroupColumn))
                updateQuery += $" AND {groupColumn.Name}=@param_group_{groupColumn.Name}";

            foreach (var idColumn in idColumns)
                updateQuery += $" AND {idColumn}=@param_id_{idColumn}";

            updateQuery = updateQuery.Replace("WHERE AND", "WHERE");

            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                var updateCommand = _connection.CreateCommand();
                updateCommand.CommandType = CommandType.Text;
                updateCommand.CommandText = updateQuery;

                foreach (var valueColumn in obfuscationOperation.Destination.Columns.Where(c => !c.IsGroupColumn))
                    updateCommand.Parameters.AddWithValue($"param_{valueColumn.Name}", row[valueColumn.Name]);

                foreach (var valueColumn in obfuscationOperation.Destination.Columns.Where(c => !c.IsGroupColumn))
                    updateCommand.Parameters.AddWithValue($"param_old_{valueColumn.Name}", row[valueColumn.Name, DataRowVersion.Original]);

                foreach (var groupColumn in obfuscationOperation.Destination.Columns.Where(gc => gc.IsGroupColumn))
                    updateCommand.Parameters.AddWithValue($"param_group_{groupColumn.Name}", row[groupColumn.Name]);

                foreach (var idColumn in idColumns)
                    updateCommand.Parameters.AddWithValue($"param_id_{idColumn}", row[idColumn]);

                updateCommand.ExecuteNonQuery();
            }

            CloseConnection();
        }

        private void RetrieveTableColumns(List<DbTableInfo> tables, CancellationTokenSource cancellationTokenSource)
        {
            if (cancellationTokenSource == null) cancellationTokenSource = new CancellationTokenSource();

            OpenConnection();
            var status = new StatusInformation { Total = tables.Count() };
            try
            {
                foreach (var table in tables)
                {
                    RetrieveTableColumns(table, status);
                    status.Progress++;
                    if (cancellationTokenSource.IsCancellationRequested) break;
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                CloseConnection();
            }
        }

        private List<string> GetIdentityColumns(string tableName)
        {
            var idColumns = new List<string>();

            var command = _connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = $"SELECT OBJECT_SCHEMA_NAME(id) + '.' + OBJECT_NAME(id) as TableName, Name as IdentityColumn"
                + $" FROM syscolumns"
                + $" WHERE COLUMNPROPERTY(id , name, 'IsIdentity') = 1 AND OBJECT_SCHEMA_NAME(id) + '.' + OBJECT_NAME(id) = '{tableName}'"
                + $" ORDER BY 1, 2";
            var reader = command.ExecuteReader();
            while (reader.Read())
                idColumns.Add((string)reader["IdentityColumn"]);

            reader.Close();

            command = _connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = $"SELECT schema_name(ta.schema_id) + '.' + ta.name as TableName, col.name ColumnName"
                + $" FROM sys.tables ta"
                + $" INNER JOIN sys.indexes ind ON ind.object_id = ta.object_id"
                + $" INNER JOIN sys.index_columns indcol ON indcol.object_id = ta.object_id AND indcol.index_id = ind.index_id"
                + $" INNER JOIN sys.columns col ON col.object_id = ta.object_id AND col.column_id = indcol.column_id"
                + $" WHERE schema_name(ta.schema_id) + '.' + ta.name = '{tableName}' AND (ind.is_primary_key = 1 OR ind.is_unique = 1)";

            reader = command.ExecuteReader();
            while (reader.Read())
                if (!idColumns.Contains((string)reader["ColumnName"])) idColumns.Add((string)reader["ColumnName"]);

            reader.Close();

            return idColumns;
        }

        private string AddOrderClauseToQuery(ObfuscationInfo obfuscationOperation, string sqlQuery)
        {
            var orderByClause = string.Empty;
            foreach (var columnInfo in obfuscationOperation.Destination.Columns.Where(c => c.IsGroupColumn))
                orderByClause += $", " + columnInfo.Name;
            if (orderByClause.Length > 0)
                sqlQuery += " ORDER BY " + orderByClause.Substring(1);

            return sqlQuery;
        }

        private void CloseConnection()
        {
            _connection.Close();
            _connection = null;
        }

        private void OpenConnection()
        {
            _connection = new SqlConnection(ConnectionString);
            _connection.Open();
        }
    }
}
