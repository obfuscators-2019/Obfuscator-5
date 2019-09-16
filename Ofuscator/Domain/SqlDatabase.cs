using Obfuscator.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

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
                    $" WHERE Table_Name = '{table.Name}'" +
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
            command.CommandText = "SELECT Table_Name FROM Information_Schema.Tables WHERE Table_Type = 'BASE TABLE'";
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                this.Tables.Add(new DbTableInfo
                {
                    Name = reader[0].ToString(),
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

            foreach (var dataRow in data)
            {
                string sqlQuery = BuildUpdateSentence(obfuscationOperation, dataRow);

            }
        }

        private IEnumerable<string> GetSourceData(Obfuscation obfuscationOperation)
        {
            var csvFile = new CsvFile();
            csvFile.ReadFile(obfuscationOperation.Origin.FileName);
            var columnContent = csvFile.GetContent(obfuscationOperation.Origin.ColumnIndex);
            return columnContent;
        }

        private string BuildUpdateSentence(Obfuscation obfuscationOperation, string dataRow)
        {
            var sqlQuery = $"UPDATE {obfuscationOperation.Destination.TableName} SET {obfuscationOperation.Destination.ColumnInfo.Name}=";
            if (obfuscationOperation.Destination.ColumnInfo.DataType.Contains("char")
                || obfuscationOperation.Destination.ColumnInfo.DataType.Contains("xml")
                || obfuscationOperation.Destination.ColumnInfo.DataType.Contains("string"))
                sqlQuery += $"'{dataRow}'";
            else
                sqlQuery += dataRow;

            return sqlQuery;
        }
    }
}

/*
 Select * From (Select Row_Number() Over (Order By FirstName) As RowNum, * From UserInformation) t2 Where RowNum = 3
 
 ;WITH RowNbrs AS (
    SELECT  ID
            , ROW_NUMBER() OVER (ORDER BY ID) AS RowNbr
    FROM    MyTab
    WHERE   a = b
)
UPDATE  t 
SET     t.MyNo = 123 +  r.RowNbr
FROM    MyTab t
        JOIN RowNbrs r ON t.ID = r.ID;
 */
