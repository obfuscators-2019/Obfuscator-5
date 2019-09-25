using System;
using System.Collections.Generic;
using System.Data;
using Obfuscator.Domain;
using Obfuscator.Entities;

namespace Obfuscator.Interfaces
{
    public interface IDataPersistence
    {
        EventHandler StatusChanged { get; set; }

        string ConnectionString { get; set; }

        List<DbTableInfo> RetrieveTables();

        void RetrieveAllTableColumnsAsync(List<DbTableInfo> tables);

        void RetrieveTableColumns(DbTableInfo table, StatusInformation statusInfo);

        string GetDatabaseName();

        DataSet GetTableData(ObfuscationInfo obfuscationOperation);

        void PersistOfuscation(ObfuscationInfo obfuscationOperation, DataSet dataSet);
    }
}
