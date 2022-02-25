using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF.Commands;

namespace WPF.ViewModel
{
    public class InsertSqlViewModel: ViewModelBase
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

        public InsertSqlViewModel()
        {
            this.SomeText = "Insert SQL queries";
            this.ExcelHandler = new ExcelHandler.ExcelHandler();
        }

        public override void BrowseFile(object param)
        {
            var dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".xls|.xlsx";
            dlg.Filter = "Excel documents (*.xls, *.xlsx)|*.xls;*.xlsx";

            if (dlg.ShowDialog() == true)
            {
                this.ExcelHandler.ConvertExcelToDataTable(dlg.FileName);
            }
        }
    }
}
