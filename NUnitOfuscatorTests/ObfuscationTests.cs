using FakeItEasy;
using NUnit.Framework;
using Obfuscator.Domain;
using Obfuscator.Entities;
using Obfuscator.Interfaces;
using System;
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
        public void UsingScramblingGeneratorObfuscation_ShufflesExistingValuesWithinGroup()
        {
            // ARRANGE
            var valueColumnIndex = 0;
            var testGroupingColumn = AddGroupColumn_ToObfuscationOperation_AndDataSet("TestingGroup");
            var originalRows = AddValues_AndGroup_ToDataSet();

            var fakeDataPersistence = A.Fake<IDataPersistence>();
            A.CallTo(() => fakeDataPersistence.GetTableData(_obfuscationOperation)).Returns(_dataSet);

            _obfuscationOperation.Origin.DataSourceType = DataSourceType.Scramble;

            // ACT
            var obfuscation = new Obfuscation { DataPersistence = fakeDataPersistence };
            obfuscation.RunOperations(new List<ObfuscationInfo> { _obfuscationOperation });

            // ASSERT
            var currentValues = _dataSet.Tables[0].Rows.Cast<DataRow>().ToList();
            var distinctGroups = originalRows.Select(dr => (int)dr[testGroupingColumn.Index]).Distinct();

            foreach (var groupIndex in distinctGroups)
            {
                var originalValuesInGroup = originalRows.Where(dr => (int)dr[testGroupingColumn.Index] == groupIndex).ToList();
                var currentValuesInGroup = currentValues.Where(dr => ((string)dr[valueColumnIndex]).EndsWith($"-{groupIndex}")).Select(x => x.ItemArray).ToList();

                var allOk = true;
                if (originalValuesInGroup.Count() > 1)
                    foreach (var originalValueRow in originalValuesInGroup)
                    {
                        var rowWithSameValueThanOriginal = currentValuesInGroup.FirstOrDefault(cv => cv[valueColumnIndex].Equals(originalValueRow[valueColumnIndex]));
                        var indexOnCurrentValues = currentValuesInGroup.IndexOf(rowWithSameValueThanOriginal);

                        allOk =  indexOnCurrentValues >= 0;
                        allOk &= originalValuesInGroup.IndexOf(originalValueRow) != indexOnCurrentValues;

                        if (!allOk) break;
                    }

                Assert.AreEqual(originalValuesInGroup.Count(), currentValuesInGroup.Count());
                Assert.IsTrue(allOk, $"Original sequence: {string.Join(",", originalValuesInGroup.Select(ov => ov[0]))}  Scrambled sequence: {string.Join(",", currentValuesInGroup.Select(cv => cv[0]))}");
            }
        }

        [Test]
        public void UsingScramblingGeneratorObfuscation_ShufflesExistingValuesWithinGroups()
        {
            // ARRANGE
            var valueColumnIndex = 0;
            var testGroupingColumn1 = AddGroupColumn_ToObfuscationOperation_AndDataSet("TestingGroup1");
            var testGroupingColumn2 = AddGroupColumn_ToObfuscationOperation_AndDataSet("TestingGroup2");
            var originalRows = AddValues_AndGroup_ToDataSet();

            var fakeDataPersistence = A.Fake<IDataPersistence>();
            A.CallTo(() => fakeDataPersistence.GetTableData(_obfuscationOperation)).Returns(_dataSet);

            _obfuscationOperation.Origin.DataSourceType = DataSourceType.Scramble;

            // ACT
            var obfuscation = new Obfuscation { DataPersistence = fakeDataPersistence };
            obfuscation.RunOperations(new List<ObfuscationInfo> { _obfuscationOperation });

            // ASSERT
            var currentValues = _dataSet.Tables[0].Rows.Cast<DataRow>().ToList();
            var distinctGroups = originalRows.Select(dr => new object[] { (int)dr[testGroupingColumn1.Index], (int)dr[testGroupingColumn2.Index] }).Distinct();

            foreach (var groupIndex in distinctGroups)
            {
                var originalValuesInGroup = originalRows.Where(dr => (int)dr[testGroupingColumn1.Index] == (int)groupIndex[0] && (int)dr[testGroupingColumn2.Index] == (int)groupIndex[1]).ToList();
                var currentValuesInGroup = currentValues.Where(dr => ((string)dr[valueColumnIndex]).EndsWith($"-{(int)groupIndex[0]}-{(int)groupIndex[1]}")).Select(x => x.ItemArray).ToList();

                var allOk = true;
                if (originalValuesInGroup.Count() > 1)
                    foreach (var originalValueRow in originalValuesInGroup)
                    {
                        var rowWithSameValueThanOriginal = currentValuesInGroup.FirstOrDefault(cv => cv[valueColumnIndex].Equals(originalValueRow[valueColumnIndex]));
                        var indexOnCurrentValues = currentValuesInGroup.IndexOf(rowWithSameValueThanOriginal);

                        allOk = indexOnCurrentValues >= 0;
                        allOk &= originalValuesInGroup.IndexOf(originalValueRow) != indexOnCurrentValues;

                        if (!allOk) break;
                    }

                Assert.AreEqual(originalValuesInGroup.Count(), currentValuesInGroup.Count());
                Assert.IsTrue(allOk, $"Original sequence: {string.Join(",", originalValuesInGroup.Select(ov => ov[valueColumnIndex]))}" +
                    $"  Scrambled sequence: {string.Join(",", currentValuesInGroup.Select(cv => cv[valueColumnIndex]))}");
            }
        }

        private List<object[]> AddValues_AndGroup_ToDataSet()
        {
            var valuesOnGroup = new List<string> { "uno", "dos", "tres" };
            var rowsAdded = new List<object[]>();
            var totalGroups = _obfuscationOperation.Destination.Columns.Where(c => c.IsGroupColumn).Count();
            var totalElementsToAdd = 9;

            for (int i = 0; i <= totalElementsToAdd ; i++)
            {
                var randomGroups = new List<object>();
                for (int j = 0; j < totalGroups; j++)
                {
                    randomGroups.Add((object)new Random().Next(totalGroups));
                    System.Threading.Thread.Sleep(3); // delay needed in order to get different groups
                }

                var originalValue = valuesOnGroup[new Random().Next(valuesOnGroup.Count())];
                var row = new List<object> { 
                    $"{originalValue}-{DateTime.Now.Ticks}-{string.Join("-", randomGroups)}" // time ticks makes the value unique for testing purposes
                };
                row.AddRange(randomGroups);

                _dataSet.Tables[0].Rows.Add(row.ToArray());
                rowsAdded.Add(row.ToArray());
            }

            return rowsAdded;
        }

        private DbColumnInfo AddGroupColumn_ToObfuscationOperation_AndDataSet(string columnName)
        {
            var columnIndex = _obfuscationOperation.Destination.Columns.Last().Index + 1;

            _obfuscationOperation.Destination.Columns.Add(new DbColumnInfo
            {
                Index = columnIndex,
                Name = columnName,
                DataType = "int",
                IsGroupColumn = true,
                IsNullable = false,
                CharacterMaxLength = 50,
            });

            _dataSet.Tables[0].Columns.Add(
                new DataColumn
                {
                    ColumnName = columnName,
                    DataType = typeof(int),
                    AllowDBNull = false,
                });

            return _obfuscationOperation.Destination.Columns.FirstOrDefault(c => c.Index == columnIndex);
        }
    }
}
 