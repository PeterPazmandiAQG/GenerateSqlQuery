using Models.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WPF.Utils;

namespace WPF.ViewModel
{
    public class UpdateSqlViewModel: ViewModelBase
    {
        private string _someText;

        public string SomeText
        {
            get { return _someText; }
            set
            {
                _someText = value;
                OnPropertyChanged(nameof(SomeText));
            }
        }

        public ExcelHandler.ExcelHandler ExcelHandler { get; set; }

        public UpdateSqlViewModel()
        {
            this.SomeText = "Update SQL queries";
            ExcelHandler = new ExcelHandler.ExcelHandler();
        }

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



        public async override void BrowseFile(object param)
        {
            try
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
            catch (System.IO.IOException ex)
            {
                if (ex.Message.Contains("it is being used by another process"))
                {
                    System.Windows.MessageBox.Show("The excel file it is being used by another process", "Error", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                    SetUnSuccessRequest();
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("it is being used by another process"))
                {
                    System.Windows.MessageBox.Show(ex.Message, "Error", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                    SetUnSuccessRequest();
                }
            }
        }

        public async override void GenerateSql(object param)
        {
            ClearQueryTexts();

            await Task.Run(() =>
            {
                InsertIntoPart += $"UPDATE {ExcelData.TableName} ";

                SetGenerationStarted();

                StatusText = "0%";
                for (int j = 0; j < ExcelData.Rows.Count; j++)
                {
                    DataRow row = ExcelData.Rows[j];

                    SqlQueries += InsertIntoPart;
                    //Rows
                    SqlQueries += $"SET ";
                    for (int i = 0; i < row.ItemArray.Length-2; i++)
                    {
                        if (string.IsNullOrEmpty(row.ItemArray[i].ToString()))
                        {
                            SqlQueries += "NULL";
                        }
                        else
                        {
                            DataTypes dataType = DataTypes.String;
                            Enum.TryParse<DataTypes>(ExcelData.Columns[i].ColumnType, out dataType);


                            switch (dataType)
                            {
                                case DataTypes.String:
                                    {
                                        SqlQueries += $"{ExcelData.Columns[i].ColumnName} = '{row.ItemArray[i].ToString()}'";
                                        break;
                                    }
                                case DataTypes.Number:
                                    {
                                        SqlQueries += $"{ExcelData.Columns[i].ColumnName} =  {row.ItemArray[i].ToString()}";
                                        break;
                                    }
                                case DataTypes.Date:
                                    {
                                        DateTime dateTime = DateTime.Now;
                                        if (DateTime.TryParse(row.ItemArray[i].ToString(), out dateTime))
                                        {
                                            SqlQueries += $"{ExcelData.Columns[i].ColumnName} = '{dateTime.Year.ToString("0000")}-{dateTime.Month.ToString("00")}-{dateTime.Day.ToString("00")}'";
                                        }
                                        break;
                                    }
                                default:
                                    break;
                            }
                        }

                        if (i == row.ItemArray.Length - 3)
                        {
                            if (row.ItemArray[row.ItemArray.Length - 2].ToString() == "="
                                || row.ItemArray[row.ItemArray.Length - 2].ToString() == ">" || row.ItemArray[row.ItemArray.Length - 2].ToString() == "<"
                                || row.ItemArray[row.ItemArray.Length - 2].ToString() == ">=" || row.ItemArray[row.ItemArray.Length - 2].ToString() == "<=")
                            {
                                SqlQueries += $" WHERE {ExcelData.Columns[row.ItemArray.Length - 1].ColumnName} {row.ItemArray[row.ItemArray.Length - 2].ToString()} {row.ItemArray[row.ItemArray.Length - 1].ToString()}";
                            }
                            else if (row.ItemArray[row.ItemArray.Length - 2].ToString() == "IN" || row.ItemArray[row.ItemArray.Length - 2].ToString() == "in")
                            {
                                SqlQueries += $" WHERE {ExcelData.Columns[row.ItemArray.Length - 1].ColumnName} IN ";
                                String[] splits = row.ItemArray[row.ItemArray.Length - 1].ToString().Split(',');
                                
                                SqlQueries += $"('{splits[0]}'";

                                for (int k = 1; k < splits.Length; k++)
                                {
                                    SqlQueries += $", '{splits[k]}'";
                                }

                                SqlQueries += $") ";
                            }
                            else if (row.ItemArray[row.ItemArray.Length - 2].ToString() == "LIKE" || row.ItemArray[row.ItemArray.Length - 2].ToString() == "like")
                            {
                                SqlQueries += $" WHERE {ExcelData.Columns[row.ItemArray.Length - 1].ColumnName} LIKE '%{row.ItemArray[row.ItemArray.Length - 1].ToString()}%'";
                            }
                            else if (row.ItemArray[row.ItemArray.Length - 2].ToString() == "=>")
                            {
                                SqlQueries += $" WHERE {ExcelData.Columns[row.ItemArray.Length - 1].ColumnName} >= {row.ItemArray[row.ItemArray.Length - 1].ToString()}";
                            }
                            else if (row.ItemArray[row.ItemArray.Length - 2].ToString() == "=<")
                            {
                                SqlQueries += $" WHERE {ExcelData.Columns[row.ItemArray.Length - 1].ColumnName} <= {row.ItemArray[row.ItemArray.Length - 1].ToString()}";
                            }

                            SqlQueries += "\nGO\n";
                        }
                        else
                        {
                            SqlQueries += ", ";
                        }

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
