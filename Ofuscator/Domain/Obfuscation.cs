using Obfuscator.Entities;
using Obfuscator.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace Obfuscator.Domain
{
    public class Obfuscation
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

        public List<DbTableInfo> RetrieveDatabaseInfo(CancellationTokenSource cancellationTokenSource = null)
        {
            this.Tables = DataPersistence.RetrieveTables();
            DataPersistence.RetrieveAllTableColumnsAsync(this.Tables, cancellationTokenSource);
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
            DataTable scrambledDataTable = null;

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
                    scrambledDataTable = ScrambleDataSet(obfuscationOperation.Destination, dataSet);
                    break;
                default:
                    originData = new List<string>();
                    break;
            }

            if (status == null) status = new StatusInformation();

            if (obfuscationOperation.Origin.DataSourceType == DataSourceType.Scramble)
                UpdateDataSet(dataSet, scrambledDataTable, obfuscationOperation);
            else
                UpdateDataset(dataSet, originData, obfuscationOperation.Destination.Columns[0].Name);

            status.Message = $"...Saving obfuscation on {obfuscationOperation.Destination.Name}";
            StatusChanged?.Invoke(status, null);
            DataPersistence.PersistOfuscation(obfuscationOperation, dataSet);
        }

        private DataSet UpdateDataSet(DataSet dataSet, DataTable scrambledDataTable, ObfuscationInfo obfuscationOperation)
        {
            var tableToUpdate = dataSet.Tables[scrambledDataTable.TableName];
            var totalRows = tableToUpdate.Rows.Count;
            var columnsToUpdate = obfuscationOperation.Destination.Columns.Where(c => !c.IsGroupColumn);

            for (int i = 0; i < totalRows; i++)
            {
                var rowToUpdate = tableToUpdate.Rows[i];
                var scrambledRow = scrambledDataTable.Rows[i];

                foreach (var column in columnsToUpdate)
                    rowToUpdate[column.Index] = scrambledRow[column.Index];
            }

            return dataSet;
        }

        private DataSet UpdateDataset(DataSet dataSet, IEnumerable<string> data, string columnName)
        {            
            var dataListIndex = 0;
            var dataListMaxIndex = data.Count() - 1;
            foreach (var row in dataSet.Tables[0].Rows)
            {
                var dataRow = (DataRow)row;
                if (dataRow[columnName] is DateTime)
                    dataRow[columnName] = ParseDateTime(data.ElementAt(dataListIndex));
                else
                    dataRow[columnName] = data.ElementAt(dataListIndex);

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
                var distinctGroups = GetColumns(dataSet.Tables[0], groupColumns)
                    .AsEnumerable()
                    .Distinct(new DataRowComparer());

                foreach (var group in distinctGroups)
                {
                    var filter = string.Empty;
                    for (int i = 0; i < groupColumns.Count(); i++)
                        filter += $"AND {groupColumns[i].Name}={ParseForDataSetFilter(group[i])} ";

                    var filteredDataTable = dataSet.Tables[0].Select(filter.Substring(3)).CopyToDataTable();
                    ScrambleColumnValues(scrambleColumns, filteredDataTable, scrambledResult);
                }
            }
            else
                ScrambleColumnValues(scrambleColumns, dataSet.Tables[0], scrambledResult);

            return scrambledResult;
        }

        private string ParseForDataSetFilter(object value)
        {
            string formattedResult = value.ToString();

            if (value is string || value is Guid || value is DateTime) formattedResult = $"'{formattedResult}'";

            return formattedResult;
        }

        private void ScrambleColumnValues(List<DbColumnInfo> columnsToScramble, DataTable dataTableToScramble, DataTable scrambledResult)
        {
            List<DataRow> originalRows = GetColumns(dataTableToScramble, columnsToScramble).AsEnumerable().ToList();
            List<DataRow> scrambledRows = GetBestPossibleScramble(originalRows);
            
            IEnumerable<DataColumn> columnsWithSameValue = dataTableToScramble.Columns
                .Cast<DataColumn>()
                .Where(c => !columnsToScramble.Any(cs => cs.Name == c.ColumnName));

            for (int i = 0; i < dataTableToScramble.Rows.Count; i++)
            {
                var newRow = scrambledResult.NewRow();
                foreach (var column in columnsWithSameValue) newRow[column.ColumnName] = dataTableToScramble.Rows[i][column.ColumnName];
                foreach (var column in columnsToScramble) newRow[column.Name] = scrambledRows[i][column.Name];
                scrambledResult.Rows.Add(newRow);
            }
        }

        private static List<DataRow> GetBestPossibleScramble(List<DataRow> originalRows)
        {
            var maxTries = 20;
            var possibleResults = new Dictionary<int, List<DataRow>>();
            List<DataRow> scrambledRows = originalRows;

            for (int iteration = 0; iteration < maxTries; iteration++)
            {
                scrambledRows = originalRows.OrderBy(dr => Guid.NewGuid()).ToList();
                var numberOfNotSuffledValues = originalRows.Count(ov => (originalRows.IndexOf(ov) == scrambledRows.IndexOf(ov)));
                if (numberOfNotSuffledValues > 0)
                {
                    if (!possibleResults.Keys.Contains(numberOfNotSuffledValues))
                        possibleResults.Add(numberOfNotSuffledValues, scrambledRows);

                    scrambledRows = possibleResults[possibleResults.Min(dict => dict.Key)];
                }
                else
                    break;
            }

            return scrambledRows;
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
                    // moving the format to the first position will probably speed up next datetime parsing operations 
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
