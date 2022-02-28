using Models.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF.Commands;
using WPF.Utils;

namespace WPF.ViewModel
{
    public class InsertSqlViewModel: ViewModelBase
    {
        private ExcelData _excelData;
        public ExcelData ExcelData
        {
            get { return _excelData; }
            set
            {
                _excelData = value;
                OnPropertyChanged(nameof(ExcelData));
            }
        }

        public ExcelHandler.ExcelHandler ExcelHandler { get; set; }

        private string _sqlQueries;

        public string SqlQueries
        {
            get { return _sqlQueries; }
            set 
            { 
                _sqlQueries = value;
                OnPropertyChanged(nameof(SqlQueries));
            }
        }

        public bool SqlGenerated
        {
            get { return SqlQueries.Length > 0; }
        }




        public InsertSqlViewModel()
        {
            LoadingText = "Loading....";
            Loading = false;

            ExcelHandler = new ExcelHandler.ExcelHandler();
        }




        public async override void BrowseFile(object param)
        {
            var dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".xls|.xlsx";
            dlg.Filter = "Excel documents (*.xls, *.xlsx)|*.xls;*.xlsx";

            if (dlg.ShowDialog() == true)
            {
                StartLoadingProcess();

                ExcelData = await ExcelHandler.ConvertExcelToDataTable(dlg.FileName);

                // Update SqlGenerated bool
                OnPropertyChanged(nameof(SqlGenerated));

                SetSuccessRequest();
            }
        }

        public async override void GenerateSql(object param)
        {
            await Task.Run(() =>
            {
                SqlQueries = "";

                SetGenerationStarted();

                StatusText = "0%";
                for (int j = 0; j < ExcelData.Rows.Count; j++)
                {
                    DataRow row = ExcelData.Rows[j];

                    SqlQueries += $"INSERT INTO {ExcelData.TableName} (";
                    // Columns
                    for (int i = 0; i < ExcelData.Columns.Count; i++)
                    {
                        SqlQueries += ExcelData.Columns[i].ColumnName;
                        if (i != ExcelData.Columns.Count - 1)
                        {
                            SqlQueries += ", ";
                        }
                    }

                    SqlQueries += ") VALUES (";
                    //Rows
                    for (int i = 0; i < row.ItemArray.Length; i++)
                    {
                        SqlQueries += row.ItemArray[i];
                        if (i != ExcelData.Columns.Count - 1)
                        {
                            SqlQueries += ", ";
                        }
                    }
                    SqlQueries += ")\n";

                    StatusText = (Convert.ToDouble(j) / Convert.ToDouble(ExcelData.Rows.Count)).ToString("#0.##%");
                }

                SetGenerationFinished();
            });
        }
    }
}
