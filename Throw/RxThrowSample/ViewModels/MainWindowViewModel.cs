using System;
using System.IO;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using Prism.Commands;
using Prism.Mvvm;

namespace RxThrowSample.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "Rx Throw";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
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
                Title = "stopd.";
            }
        }

        private void Start()
        {

            var source = Observable.Throw<Exception>(new Exception("hoge Exception"));

            _myDisposer = source
                            .Subscribe(i =>
                            {
                                Console.WriteLine(i);
                                Title = i.ToString();
                            },
                                ex =>
                                {
                                    Thread.Sleep(5000);
                                    Title = ex.Message;
                                },
                                () => Title = "completed.");
        }

        private IDisposable _myDisposer = null;

        public DelegateCommand StartCommand { get; set; }

        public DelegateCommand StopCommand { get; set; }
    }
}
