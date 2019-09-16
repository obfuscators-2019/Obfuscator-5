using Obfuscator.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Obfuscator.Domain
{
    public class SqlDatabase
    {
        public string ConnectionString { get; set; }

        public List<DbTableInfo> Tables { get; set; }

        public List<DbTableInfo> RetrieveDatabaseInfo()
        {
            var connection = new SqlConnection(this.ConnectionString);
            connection.Open();

            RetrieveTableNames(connection);
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
                command.CommandType = System.Data.CommandType.Text;
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

        private void RetrieveTableNames(SqlConnection connection)
        {
            this.Tables = new List<DbTableInfo>();

            var command = connection.CreateCommand();
            command.CommandType = System.Data.CommandType.Text;
            command.CommandText = "SELECT Table_Schema, Table_Name FROM Information_Schema.Tables WHERE Table_Type = 'BASE TABLE'";
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                this.Tables.Add(new DbTableInfo
                {
                    Name = $"{reader[0]}.{reader[1]}",
                    Columns = new List<DbColumnInfo>()
                });
            }
            reader.Close();
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

        private void PersistOfuscation(Obfuscation obfuscationOperation, DataSet dataSet)
        {
            var sqlConnection = new SqlConnection(this.ConnectionString);
            sqlConnection.Open();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter($"SELECT * FROM {obfuscationOperation.Destination.TableName}", sqlConnection);
            new SqlCommandBuilder(sqlDataAdapter);
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
            var sqlConnection = new SqlConnection(this.ConnectionString);
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
