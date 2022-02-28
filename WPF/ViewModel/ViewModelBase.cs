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





        private string _loadingText;
        public string LoadingText
        {
            get { return _loadingText; }
            set
            {
                _loadingText = value;
                OnPropertyChanged(nameof(LoadingText));
            }
        }

        private string _statusText;
        public string StatusText
        {
            get { return _statusText; }
            set
            {
                _statusText = value;
                OnPropertyChanged(nameof(StatusText));
            }
        }

        private bool _loading;
        public bool Loading
        {
            get { return _loading; }
            set
            {
                _loading = value;
                OnPropertyChanged(nameof(Loading));
            }
        }

        private bool _isSuccessfull;
        public bool IsSuccessfull
        {
            get { return _isSuccessfull; }
            set
            {
                _isSuccessfull = value;
                OnPropertyChanged(nameof(IsSuccessfull));
            }
        }

        private bool _isUnSuccessfull;
        public bool IsUnSuccessfull
        {
            get { return _isUnSuccessfull; }
            set
            {
                _isUnSuccessfull = value;
                OnPropertyChanged(nameof(IsUnSuccessfull));
            }
        }

        private bool _isGenerating;
        public bool IsGenerating
        {
            get { return _isGenerating; }
            set
            {
                _isGenerating = value;
                OnPropertyChanged(nameof(IsGenerating));
            }
        }

        private bool _isGenerated;
        public bool IsGenerated
        {
            get { return _isGenerated; }
            set
            {
                _isGenerated = value;
                OnPropertyChanged(nameof(IsGenerated));
            }
        }



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



        private RelayCommand _generateSqlCommand;
        public RelayCommand GenerateSqlCommand
        {
            get
            {
                if (this._generateSqlCommand == null)
                {
                    this._generateSqlCommand = new RelayCommand(GenerateSql);
                }
                return _generateSqlCommand;
            }
        }




        public async virtual void BrowseFile(object param)
        {

        }


        public async virtual void GenerateSql(object param)
        {

        }

        public void StartLoadingProcess()
        {
            Loading = true;
            IsSuccessfull = false;
            IsUnSuccessfull = false;
        }

        public void SetSuccessRequest()
        {
            Loading = false;
            IsSuccessfull = true;
            IsUnSuccessfull = false;
        }

        public void SetUnSuccessRequest()
        {
            Loading = false;
            IsSuccessfull = false;
            IsUnSuccessfull = true;
        }

        public void SetGenerationStarted()
        {
            IsGenerating = true;
            IsGenerated = false;
        }

        public void SetGenerationFinished()
        {
            IsGenerating = false;
            IsGenerated = true;
        }


        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
