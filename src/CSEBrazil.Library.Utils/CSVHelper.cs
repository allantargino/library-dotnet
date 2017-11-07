using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;

namespace CSEBrazil.Library.Utils
{
    public class CSVHelper
    {
        private string[] _lines;

        public CSVHelper(string pathCSV, bool ignoreHeader = false)
        {
            if (string.IsNullOrEmpty(pathCSV)) throw new ArgumentNullException(nameof(pathCSV));
            if (!File.Exists(pathCSV)) throw new FileNotFoundException();

            _lines = File.ReadAllLines(pathCSV);
            if (ignoreHeader)
                _lines = _lines.Skip(1).ToArray();
        }

        /// <summary>
        /// Return the CSV contents by proving a generic function reponsible to process each line
        /// </summary>
        /// <param name="func"></param>
        /// <returns>IEnumerable object with the CSV content</returns>
        public IEnumerable<Object> GetCsvContents(Func<string, Object> func)
            => _lines.Select(line => func(line)).ToArray().ToList();

    }
}
