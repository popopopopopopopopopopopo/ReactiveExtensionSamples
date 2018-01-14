using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Mvvm;

namespace RxDeferSample.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "Rx Defer";
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
            _myDisposer = Observable.Defer(() =>
                                {
                                    Title = "processing defer...........";
                                    return Observable.Range(1, 10, ThreadPoolScheduler.Instance);
                                })
                                .Do(s => Thread.Sleep(3000))
                                .Subscribe(i =>
                                    {
                                        Console.WriteLine(i);
                                        Title = i.ToString();
                                    },
                                ex => Title = ex.Message,
                                () => Title = "completed.");
        }

        private IDisposable _myDisposer = null;

        public DelegateCommand StartCommand { get; set; }

        public DelegateCommand StopCommand { get; set; }
    }
}
