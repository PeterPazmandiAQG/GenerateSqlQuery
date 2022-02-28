using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF.Utils;

namespace Models.Model
{
    public class ExcelData
    {
        public string TableName { get; set; }

        public ObservableCollection<Columns> Columns { get; set; } = new ObservableCollection<Columns>();

        public DataRowCollection Rows { get; set; }
    }
}
