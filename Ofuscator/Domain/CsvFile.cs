using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ofuscator.Domain
{
    public class CsvFile
    {
        public IEnumerable<string> CsvLines { get; set; }
        public bool HasHeaders { get; set; } = true;

        public IEnumerable<string> GetHeaders() => HasHeaders ? CsvLines?.FirstOrDefault()?.Split(new char[] { ',' }) : null;

        public IEnumerable<string> GetContent(int columnIndex)
        {
            var startingLine = HasHeaders ? 1 : 0;

            var allCsvLines = CsvLines.ToList();
            var contentLines = allCsvLines.GetRange(startingLine, allCsvLines.Count - startingLine);
            IEnumerable<string> columnContent = contentLines.Select( line => line.Split(new char[] { ',' }).ElementAtOrDefault(columnIndex) );

            return columnContent;
        }
        
        /// <summary>
        /// Read file content
        /// </summary>
        /// <param name="fileName">Name of the file to read</param>
        /// <param name="maxLines">If greather than Zero, reads only the specified number of lines. If Zero, reads all lines.</param>
        public void ReadFile(string fileName, long maxLines = 0)
        {
            using (FileStream fs = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (BufferedStream bs = new BufferedStream(fs))
            using (StreamReader sr = new StreamReader(bs))
            {
                var lines = new List<string>();
                string line;

                maxLines = (maxLines > 0 ? maxLines : maxLines-1);

                while ((line = sr.ReadLine()) != null && (maxLines != 0))
                {
                    lines.Add(line);
                    maxLines--;
                }
                CsvLines = lines;
            }
        }
    }
}
