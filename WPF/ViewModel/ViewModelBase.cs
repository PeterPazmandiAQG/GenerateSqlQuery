using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF.Commands;

namespace WPF.ViewModel
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;





        private RelayCommand _browseFileCommand;
        public RelayCommand BrowseFileCommand
        {
            get
            {
                if (this._browseFileCommand == null)
                {
                    this._browseFileCommand = new RelayCommand(BrowseFile);
                }
                return _browseFileCommand;
            }
        }


        public virtual void BrowseFile(object param)
        {

        }



        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
