using ExcelDataReader;
using System;
using System.IO;

namespace ExcelHandler
{
    public class ExcelHandler
    {
        public void ReadFile(string filePath, FileMode fileMode, FileAccess fileAccess)
        {
            using (var stream = File.Open(filePath, fileMode, fileAccess))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    do
                    {
                        while (reader.Read())
                        {

                        }
                    } while (reader.NextResult());

                    var result = reader.AsDataSet();
                }
            }
        }
    }
}
