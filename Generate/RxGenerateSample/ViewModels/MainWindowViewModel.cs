using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RxGenerateSample.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "Rx Generate";
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
            _myDisposer = Observable.Generate(0, i => i < 10, i => ++i, i => i.ToString() + "hogemoge"
                    ,i=> DateTimeOffset.Now,
                    ThreadPoolScheduler.Instance)
                .Do(s=>Thread.Sleep(3000))
                        .Subscribe(s =>
                            {
                                Console.WriteLine(s);
                                Title = s;
                            },
                            ex => Title = ex.Message,
                            () => Title = "completed.");
        }

        private IDisposable _myDisposer = null;

        public DelegateCommand StartCommand { get; set; }

        public DelegateCommand StopCommand { get; set; }
    }

}
