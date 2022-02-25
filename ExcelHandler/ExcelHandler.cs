using ExcelDataReader;
using System;
using System.Data;
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
        public DataTable ConvertExcelToDataTable(string filePath)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            //open file and returns as Stream
            using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {

                    var conf = new ExcelDataSetConfiguration
                    {
                        ConfigureDataTable = _ => new ExcelDataTableConfiguration
                        {
                            UseHeaderRow = true
                        }
                    };

                    var dataSet = reader.AsDataSet(conf);

                    // Now you can get data from each sheet by its index or its "name"
                    var dataTable = dataSet.Tables[0];

                    Console.WriteLine("Total no of rows  " + dataTable.Rows.Count);
                    Console.WriteLine("Total no of Columns  " + dataTable.Columns.Count);

                    return dataTable;

                }

            }

        }
    }
}
