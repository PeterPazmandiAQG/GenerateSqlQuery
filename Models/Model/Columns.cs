using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF.Utils;

namespace Models.Model
{
    public class Columns
    {
        public string ColumnName { get; set; }
        public string ColumnType { get; set; }

        public string[] DataTypes
        {
            get { return Enum.GetNames(typeof(DataTypes)); }
        }
    }
}