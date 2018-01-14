using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using Prism.Commands;
using Prism.Mvvm;

namespace RxIntervalSample.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "Rx Interval";
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
            _myDisposer = Observable.Interval(TimeSpan.FromSeconds(10), ThreadPoolScheduler.Instance)
                //.Do(s =>
                //{
                //    Console.WriteLine(s);
                //    Thread.Sleep(5000);
                //})
                .Subscribe(s => Title = s.ToString(),
                    err => Title = err.Message,
                    () => Title = "completed.");
        }

        private IDisposable _myDisposer = null;

        public DelegateCommand StartCommand { get; set; }

        public DelegateCommand StopCommand { get; set; }
    }
}
