using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace RxRepeatSample.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "Rx Repeat";
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
            _count = 0;
        }

        int _count = 0;
        private void Start()
        {
            _myDisposer = Observable.Repeat(DateTime.Now.ToString("yyyyMMddHHmmssfff"), 
                                            ThreadPoolScheduler.Instance)
                                            .Do(s=> { Task.Delay(1000); })
                                            .Subscribe(l => {
                                                _count++;
                                                Title = l.ToString() + $"_{_count}";
                                                Console.WriteLine(Title);
                                                if (_count >= 120000) Stop();
                                            },
                                            ex => Title = ex.Message, () => Title = "Complete");
        }

        private IDisposable _myDisposer = null;

        public DelegateCommand StartCommand { get; set; }

        public DelegateCommand StopCommand { get; set; }
    }
}
