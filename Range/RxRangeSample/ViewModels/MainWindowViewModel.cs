using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RxRangeSample.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "Rx Range";
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
            _myDisposer = Observable.Range(1, 10,ThreadPoolScheduler.Instance)
                                            .Subscribe(l =>
                                            {
                                                Thread.Sleep(5000);
                                                Title = l.ToString();
                                                Console.WriteLine(Title);
                                            },
                                            ex => Title = ex.Message,
                                            () => Title = "Complete");
        }

        private IDisposable _myDisposer = null;

        public DelegateCommand StartCommand { get; set; }

        public DelegateCommand StopCommand { get; set; }
    }
}
