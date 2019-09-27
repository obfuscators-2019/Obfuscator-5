using FakeItEasy;
using NUnit.Framework;
using Obfuscator.Domain;
using Obfuscator.Entities;
using Obfuscator.Interfaces;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Tests
{
    public class ObfuscationTests
    {
        private ObfuscationInfo _obfuscationOperation;
        private DataSet _dataSet;

        [SetUp]
        public void Setup()
        {
            var testDataTable = "TestDataTable";
            var testColumn = new DataColumn
            {
                ColumnName = "TestDataColumn",
                DataType = typeof(string),
                AllowDBNull = true,
                MaxLength = 50,
            };

            _obfuscationOperation = new ObfuscationInfo
            {
                Origin = new DataSourceInformation
                {
                    DataSourceName = "TestDataSource",
                    DataSourceType = DataSourceType.DNIGenerator,
                    ColumnIndex = 0,
                    HasHeaders = false,
                },
                Destination = new DbTableInfo
                {
                    Name = testDataTable,
                    ConnectionString = "test connection string",
                    Columns = new List<DbColumnInfo>
                    {
                        new DbColumnInfo
                        {
                            Name = testColumn.ColumnName,
                            Index = 0,
                            DataType = "string",
                            CharacterMaxLength = testColumn.MaxLength,
                            IsNullable = testColumn.AllowDBNull,
                            IsGroupColumn = false
                        }
                    }
                }
            };

            _dataSet = new DataSet();

            var table = _dataSet.Tables.Add(testDataTable);
            table.Columns.Add(testColumn);
        }

        [Test]
        public void UsingDniGeneratorObfuscation_ReplacesAllCurrentValues()
        {
            List<string> originalValues = AddValues_ToDataSet();

            var fakeDataPersistence = A.Fake<IDataPersistence>();
            A.CallTo(() => fakeDataPersistence.GetTableData(_obfuscationOperation)).Returns(_dataSet);

            var obfuscation = new Obfuscation { DataPersistence = fakeDataPersistence };
            obfuscation.RunOperations(new List<ObfuscationInfo> { _obfuscationOperation });

            var currentValues = _dataSet.Tables[0].Rows.Cast<DataRow>().Select(dr => dr[0].ToString());

            Assert.IsFalse(originalValues.All(ov => currentValues.Any(cv => cv == ov)));
        }

        private List<string> AddValues_ToDataSet()
        {
            var originalValues = new List<string> { "uno", "dos", "tres" };

            foreach (var value in originalValues)
            {
                var row = _dataSet.Tables[0].NewRow();
                row[0] = value;
                _dataSet.Tables[0].Rows.Add(row);
            }

            return originalValues;
        }

        [Test]
        public void UsingScramblingGeneratorObfuscation_ShufflesExistingValues()
        {
            List<string> originalValues = AddValues_ToDataSet();

            var fakeDataPersistence = A.Fake<IDataPersistence>();
            A.CallTo(() => fakeDataPersistence.GetTableData(_obfuscationOperation)).Returns(_dataSet);

            _obfuscationOperation.Origin.DataSourceType = DataSourceType.Scramble;

            var obfuscation = new Obfuscation { DataPersistence = fakeDataPersistence };
            obfuscation.RunOperations(new List<ObfuscationInfo> { _obfuscationOperation });

            var currentValues = _dataSet.Tables[0].Rows.Cast<DataRow>().Select(dr => dr[0].ToString()).ToList();

            Assert.IsTrue(originalValues.All(ov => currentValues.IndexOf(ov) >= 0 && (originalValues.IndexOf(ov) != currentValues.IndexOf(ov))));
        }

        [Test]
        public void UsingScramblingGeneratorObfuscation_ShufflesExistingValuesWithinGroups()
        {
            var testGroupingColumn = AddColumn_TestGrouping_ToObfuscationOperation_AndDataSet();
            var originalRows = AddValues_AndGroup_ToDataSet();

            var fakeDataPersistence = A.Fake<IDataPersistence>();
            A.CallTo(() => fakeDataPersistence.GetTableData(_obfuscationOperation)).Returns(_dataSet);

            _obfuscationOperation.Origin.DataSourceType = DataSourceType.Scramble;

            var obfuscation = new Obfuscation { DataPersistence = fakeDataPersistence };
            obfuscation.RunOperations(new List<ObfuscationInfo> { _obfuscationOperation });

            var currentValues = _dataSet.Tables[0].Rows.Cast<DataRow>().ToList();
            var distinctGroups = originalRows.Select(dr => (int)dr[testGroupingColumn.Index]).Distinct();

            foreach (var groupIndex in distinctGroups)
            {
                var expectedValuesInGroup = originalRows.Where(dr => (int)dr[testGroupingColumn.Index] == groupIndex).ToList();
                var currentValuesInGroup = currentValues.Where(dr => ((string)dr[0]).StartsWith($"{groupIndex}-")).Select(x => x.ItemArray).ToList();

                Assert.AreEqual(expectedValuesInGroup.Count(), currentValuesInGroup.Count());
                Assert.IsTrue(expectedValuesInGroup.All(ev => currentValuesInGroup.IndexOf(ev) >= 0 && (expectedValuesInGroup.IndexOf(ev) != currentValuesInGroup.IndexOf(ev))));
            }
        }

        private List<object[]> AddValues_AndGroup_ToDataSet()
        {
            var valuesOnGroup = new List<string> { "uno", "dos", "tres" };
            var rowsAdded = new List<object[]>();

            for (int groupIndex = 0; groupIndex < 3; groupIndex++)
                foreach (var originalValue in valuesOnGroup)
                {
                    var row = new object[] { $"{groupIndex}-{originalValue}", groupIndex };
                    _dataSet.Tables[0].Rows.Add(row);
                    rowsAdded.Add(row);
                }

            return rowsAdded;
        }

        private DbColumnInfo AddColumn_TestGrouping_ToObfuscationOperation_AndDataSet()
        {
            var columnIndex = _obfuscationOperation.Destination.Columns.Last().Index + 1;

            _obfuscationOperation.Destination.Columns.Add(new DbColumnInfo
            {
                Index = columnIndex,
                Name = "TestGrouping",
                DataType = "int",
                IsGroupColumn = true,
                IsNullable = false,
                CharacterMaxLength = 50,
            });

            _dataSet.Tables[0].Columns.Add(
                new DataColumn
                {
                    ColumnName = "TestGrouping",
                    DataType = typeof(int),
                    AllowDBNull = false,
                });

            return _obfuscationOperation.Destination.Columns.FirstOrDefault(c => c.Index == columnIndex);
        }
    }
}
