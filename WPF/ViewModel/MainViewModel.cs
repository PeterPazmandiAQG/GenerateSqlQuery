using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF.Commands;

namespace WPF.ViewModel
{
    public class MainViewModel: ViewModelBase
    {
        private ViewModelBase _currentViewModel;
        public ViewModelBase CurrentViewModel
        {
            get { return _currentViewModel; }
            set
            {
                _currentViewModel = value;
                OnPropertyChanged(nameof(CurrentViewModel));
            }
        }




        private RelayCommand menuItemOpenCommand;
        public RelayCommand MenuItemOpenCommand
        {
            get
            {
                if (this.menuItemOpenCommand == null)
                {
                    this.menuItemOpenCommand = new RelayCommand(OpenMenuItem);
                }
                return menuItemOpenCommand;
            }
        }



        public MainViewModel()
        {

        }





        private void OpenMenuItem(object param)
        {
            if (param.Equals("InsertSqlViewModel"))
            {
                this.CurrentViewModel = new InsertSqlViewModel();
            }
            if (param.Equals("UpdateSqlViewModel"))
            {
                this.CurrentViewModel = new UpdateSqlViewModel();
            }
        }
    }
}
