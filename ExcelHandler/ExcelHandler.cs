using ExcelDataReader;
using Models.Model;
using System;
using System.Data;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using WPF.Utils;

namespace ExcelHandler
{
    public class ExcelHandler
    {
        public async Task<ExcelData> ConvertExcelToDataTable(string filePath)
        {
            ExcelData excelData = new ExcelData();
            return await Task.Run(() =>
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

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

                        excelData.TableName = dataTable.TableName;
                        foreach (DataColumn Column in dataTable.Columns)
                        {
                            excelData.Columns.Add(new Columns()
                            {
                                ColumnName = Column.ColumnName,
                                ColumnType = DataTypes.String.ToString()                                
                            });
                        }
                        excelData.Rows = dataTable.Rows;

                        return excelData;
                    }
                }
            });
        }
    }
}
