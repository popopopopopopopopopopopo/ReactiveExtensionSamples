using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Mvvm;

namespace RxReturnSample.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private static MainWindowViewModel _myInstance = null;

        public static MainWindowViewModel Instance
        {
            get => _myInstance ?? GetInstance();
            set => _myInstance = value;
        }

        private static MainWindowViewModel GetInstance()
        {
            return _myInstance ?? (_myInstance = new MainWindowViewModel());
        }

        public MainWindowViewModel()
        {
            StartCommand = new DelegateCommand(Start);
            StopCommand = new DelegateCommand(Stop);
        }

        private void Stop()
        {
            if (_myDisposer != null)
            {
                _myDisposer.Dispose();
                _myDisposer = null;
                Status = "stopd.";
            }
        }

        private void Start()
        {
            _myDisposer = Observable.Return(10).Subscribe(l => Status = l.ToString(),
                ex => Status = ex.Message, () => Status = "Complete");
        }

        private IDisposable _myDisposer = null;

        private string _myStatus = "";

        public string Status
        {
            get => _myStatus;
            set => SetProperty(ref _myStatus, value);
        }

        public DelegateCommand StartCommand { get; set; }

        public DelegateCommand StopCommand { get; set; }
    }
}
