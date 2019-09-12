using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Tests
{
    public class CsvFileTests
    {
        private string _csvHeadersLine;
        private List<string> _csvContent;

        [SetUp]
        public void Setup()
        {
            _csvHeadersLine = "nombre,frec,edad_media";
            _csvContent = new List<string>
            {
                _csvHeadersLine,
                "MARIA CARMEN,681108,51.1",
                "MARIA,668112,49.3",
                "CARMEN,447130,59.5",
                "JOSEFA,325874,64.1",
                "DOLORES,236302,63.4"
            };
        }

        [Test]
        public void ReadingTheHeaderOfTheCSV_ReturnsTheDataFromTheFirstLine()
        {
            var expectedHeaders = _csvHeadersLine.Split(new char[] { ',' }).ToList();

            var csvReader = new Ofuscator.Domain.CsvFile();
            csvReader.CsvLines = _csvContent;
            csvReader.HasHeaders = true;
            var headers = csvReader.GetHeaders();

            Assert.IsTrue(expectedHeaders.SequenceEqual(headers));
        }

        [Test]
        public void ReadingTheHeaderOfTheCSV_IfNoContent_ReturnsNull()
        {
            var csvReader = new Ofuscator.Domain.CsvFile();
            csvReader.CsvLines = null;

            var headers = csvReader.GetHeaders();

            Assert.IsNull(headers);
        }

        [Test]
        public void ReadingTheHeaderOfTheCSV_IfNoHeader_ReturnsNull()
        {
            var csvReader = new Ofuscator.Domain.CsvFile();
            csvReader.CsvLines = null;
            csvReader.HasHeaders = false;
            var headers = csvReader.GetHeaders();

            Assert.IsNull(headers);
        }

        [Test]
        public void ReadingTheContentOfTheSecondColumn_ReturnsDataFromTheSecondColumn()
        {
            var columnIndex = 1;
            var expectedResult = _csvContent.GetRange(1, _csvContent.Count - 1).Select(line => line.Split(new char[] { ',' })[columnIndex]);

            var csvReader = new Ofuscator.Domain.CsvFile();
            csvReader.CsvLines = _csvContent;
            csvReader.HasHeaders = true;
            var columnContent = csvReader.GetContent(columnIndex);

            Assert.IsTrue(expectedResult.SequenceEqual(columnContent));
        }
    }
}