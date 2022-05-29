using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLNet_BERT_Uncased.Helpers
{
    class FileReader
    {
        public static List<string> ReadFile(string fileName)
        {
            var result = new List<string>();

            using (var reader = new StreamReader(fileName))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (!string.IsNullOrWhiteSpace(line))
                        result.Add(line);
                }
            }

            return result;
        }
    }
}
