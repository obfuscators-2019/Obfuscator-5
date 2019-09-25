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
            var fakeDataPersistence = A.Fake<IDataPersistence>();
            var originalValues = new List<string> { "uno", "dos", "tres" };

            foreach (var value in originalValues)
            {
                var row = _dataSet.Tables[0].NewRow();
                row[0] = value;
                _dataSet.Tables[0].Rows.Add(row);
            }

            A.CallTo(() => fakeDataPersistence.GetTableData(_obfuscationOperation)).Returns(_dataSet);            

            var obfuscation = new Obfuscation { DataPersistence = fakeDataPersistence };
            obfuscation.RunOperations(new List<ObfuscationInfo> { _obfuscationOperation });

            var currentValues = _dataSet.Tables[0].Rows.Cast<DataRow>().Select(dr => dr[0].ToString());

            Assert.IsFalse(originalValues.All(ov => currentValues.Any(cv => cv == ov)));
        }

        [Test]
        public void UsingScramblingGeneratorObfuscation_ShufflesExistingValuesWithinGroups()
        {
            _obfuscationOperation.Destination.Columns.Add(new DbColumnInfo
            {
                Index = 1,
                Name = "TestGrouping",
                DataType = "int",
                IsGroupColumn = true,
                IsNullable = false,
                CharacterMaxLength = 50,
            });

            var fakeDataPersistence = A.Fake<IDataPersistence>();
            var valuesOnGroup = new List<string> { "uno", "dos", "tres" };

            for (int groupIndex = 0; groupIndex < 3; groupIndex++)
                foreach (var originalValue in valuesOnGroup)
                {
                    var row = _dataSet.Tables[0].NewRow();
                    row[0] = $"{groupIndex}-{originalValue}";
                    row[1] = groupIndex;
                    _dataSet.Tables[0].Rows.Add(row);
                }

            var originalRows = _dataSet.Tables[0].Rows.Cast<DataRow>().ToList(); 

            A.CallTo(() => fakeDataPersistence.GetTableData(_obfuscationOperation)).Returns(_dataSet);

            _obfuscationOperation.Origin.DataSourceType = DataSourceType.Scramble;

            var obfuscation = new Obfuscation { DataPersistence = fakeDataPersistence };
            obfuscation.RunOperations(new List<ObfuscationInfo> { _obfuscationOperation });

            var currentValues = _dataSet.Tables[0].Rows.Cast<DataRow>().ToList();

            // for each original row - get rows on the same group
            // they should be scrambled, but all in in the same group


            Assert.Zero(1);
        }
    }
}