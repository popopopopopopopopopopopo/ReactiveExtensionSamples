using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Mvvm;

namespace RxCreateSample.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "Rx Create";
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
            //Createは同期的処理に使ったほうがよさげ
            var source = Observable.Create<int>(observer =>
                                            {
                                                observer.OnNext(1);
                                                observer.OnNext(2);
                                                observer.OnNext(3);
                                                observer.OnCompleted();
                                                //リソースの開放などを下記のアクション中で行ったりする
                                                return () => Title = "Created Subscribe Completed";
                                            });

            _myDisposer = source
                            .Subscribe(i =>
                                {
                                    Thread.Sleep(5000);
                                    Console.WriteLine(i);
                                    Title = i.ToString();
                                },
                                ex => Title = ex.Message,
                                () => Title = "completed.");

            _myDisposer = source
                .Subscribe(i =>
                    {
                        Thread.Sleep(5000);
                        Console.WriteLine(i.ToString() + "_2");
                        Title = i.ToString() + "_2";
                    },
                    ex => Title = ex.Message + "_2",
                    () => Title = "completed 2.");
        }

        private IDisposable _myDisposer = null;

        public DelegateCommand StartCommand { get; set; }

        public DelegateCommand StopCommand { get; set; }
    }
}
