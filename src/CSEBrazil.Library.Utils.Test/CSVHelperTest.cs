using System;
using System.IO;
using Xunit;
using System.Linq;
using System.Collections.Generic;

namespace CSEBrazil.Library.Utils.Test
{
    public class CSVHelperTest
    {
        [Fact]
        public void ReadCSVContent()
        {
            string directory = Directory.GetCurrentDirectory() + "\\Data";
            String[] files = Directory.GetFiles(directory);
            if (files.Length > 0)
            {
                CSVHelper helper = new CSVHelper(files[0], true);
                IEnumerable<Object> list =  helper.GetCsvContents(func);

                Console.WriteLine($"we process the file with success - {list.Count()} registros encontrados!");
            }
            else
            {
                Console.WriteLine($"No file was found in {directory}");
            }
        }

        private object func(string arg)
        {
            ModelTest model = null;
            try
            {
                model = new ModelTest()
                {
                    ID = arg.Split(";")[0],
                    Name = arg.Split(";")[1]
                };
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return model;
        }
    }

    class ModelTest
    {
        public string ID { get; set; }
        public string Name { get; set; }
    }
}
