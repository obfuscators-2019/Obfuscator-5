using Obfuscator.Entities;
using Obfuscator.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;

namespace Obfuscator.Domain
{
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

    public partial class Obfuscation
    { 
        public IDataPersistence DataPersistence { get; set; }

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
        /// PARAMETER object sender: StatusInformation
        /// </summary>
        public EventHandler StatusChanged { get; set; }

        public List<DbTableInfo> Tables { get; set; }

        public List<DbTableInfo> RetrieveDatabaseInfo()
        {
            this.Tables = DataPersistence.RetrieveTables();
            DataPersistence.RetrieveAllTableColumnsAsync(this.Tables);

            return this.Tables;
        }

        public void RetrieveTableColumns(DbTableInfo table)
        {
            DataPersistence.RetrieveTableColumns(table, null);
        }

        public void RunOperations(IEnumerable<ObfuscationInfo> obfuscationOps)
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

        private void RunOperation(ObfuscationInfo obfuscationOperation, StatusInformation status = null)
        {
            IEnumerable<string> originData = null;

            DataPersistence.ConnectionString = obfuscationOperation.Destination.ConnectionString;
            var dataSet = DataPersistence.GetTableData(obfuscationOperation);

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
            DataPersistence.PersistOfuscation(obfuscationOperation, dataSet);
        }

        private DataSet OfuscateDataset(ObfuscationInfo obfuscationOperation, IEnumerable<string> data, DataSet dataSet)
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

        private IEnumerable<string> GetSourceData(ObfuscationInfo obfuscationOperation)
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
