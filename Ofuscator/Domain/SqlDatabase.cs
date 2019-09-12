using Ofuscator.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Ofuscator.Domain
{
    public class SqlDatabase
    {
        public string ConnectionString { get; set; }

        public List<TableInfo> Tables { get; set; }

        public List<TableInfo> RetrieveDatabaseInfo()
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
                table.Columns = new List<ColumnInfo>();
                var command = connection.CreateCommand();
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = $"SELECT Column_Name, Ordinal_Position, Is_Nullable, Data_Type, Character_Maximum_Length" +
                    $" FROM Information_Schema.Columns" +
                    $" WHERE Table_Name = '{table.Name}'" +
                    $" ORDER BY ORDINAL_POSITION";
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    table.Columns.Add(new ColumnInfo
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
            this.Tables = new List<TableInfo>();

            var command = connection.CreateCommand();
            command.CommandType = System.Data.CommandType.Text;
            command.CommandText = "SELECT Table_Name FROM Information_Schema.Tables WHERE Table_Type = 'BASE TABLE'";
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                this.Tables.Add(new TableInfo
                {
                    Name = reader[0].ToString(),
                    Columns = new List<ColumnInfo>()
                });
            }
            reader.Close();
        }
    }
}
