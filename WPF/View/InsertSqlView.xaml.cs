using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPF.ViewModel;

namespace WPF.View
{
    /// <summary>
    /// Interaction logic for InsertSql.xaml
    /// </summary>
    public partial class InsertSqlView : UserControl
    {
        public InsertSqlView()
        {
            InitializeComponent();
            this.DataContext = new InsertSqlViewModel();
        }
    }
}
