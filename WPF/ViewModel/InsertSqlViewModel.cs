using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public InsertSqlViewModel()
        {
            this.SomeText = "Insert SQL queries";
        }
    }
}
