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
        private string _insertIntoPart;
        public string InsertIntoPart
        {
            get { return _insertIntoPart; }
            set 
            {
                _insertIntoPart = value;
                OnPropertyChanged(nameof(InsertIntoPart));
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
            ClearQueryTexts();

            await Task.Run(() =>
            {
                InsertIntoPart += $"INSERT INTO {ExcelData.TableName} (";
                // Columns
                for (int i = 0; i < ExcelData.Columns.Count; i++)
                {
                    InsertIntoPart += ExcelData.Columns[i].ColumnName;
                    if (i != ExcelData.Columns.Count - 1)
                    {
                        InsertIntoPart += ", ";
                    }
                }

                SetGenerationStarted();

                StatusText = "0%";
                for (int j = 0; j < ExcelData.Rows.Count; j++)
                {
                    DataRow row = ExcelData.Rows[j];

                    SqlQueries += InsertIntoPart + ") VALUES (";
                    //Rows
                    for (int i = 0; i < row.ItemArray.Length; i++)
                    {
                        if (string.IsNullOrEmpty(row.ItemArray[i].ToString()))
                        {
                            SqlQueries += "''";
                        }
                        else
                        {
                            DataTypes dataType = DataTypes.String;
                            Enum.TryParse<DataTypes>(ExcelData.Columns[i].ColumnType, out dataType);
                            switch (dataType)
                            {
                                case DataTypes.String:
                                    {
                                        SqlQueries += $"'{row.ItemArray[i].ToString()}'";
                                        break;
                                    }
                                case DataTypes.Number:
                                    {
                                        SqlQueries += row.ItemArray[i].ToString();
                                        break;
                                    }
                                case DataTypes.Date:
                                    {
                                        DateTime dateTime = DateTime.Now;
                                        if (DateTime.TryParse(row.ItemArray[i].ToString(), out dateTime))
                                        {
                                            SqlQueries += $"'{dateTime.Year.ToString("0000")}-{dateTime.Month.ToString("00")}-{dateTime.Day.ToString("00")}'";
                                        }
                                        break;
                                    }
                                default:
                                    break;
                            }
                        }

                        if (i == row.ItemArray.Length - 1)
                        {
                            SqlQueries += ")\nGO\n";
                        }
                        else
                        {
                            SqlQueries += ", ";
                        }
                        //SqlQueries += row.ItemArray[i];

                    }

                    StatusText = (Convert.ToDouble(j) / Convert.ToDouble(ExcelData.Rows.Count)).ToString("#0.##%");
                }

                SetGenerationFinished();
            });
        }

        private void ClearQueryTexts()
        {
            InsertIntoPart = "";
            SqlQueries = "";
        }
    }
}
