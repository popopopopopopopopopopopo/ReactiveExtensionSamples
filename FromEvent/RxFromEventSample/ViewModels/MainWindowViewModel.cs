using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using System.Timers;
using Prism.Commands;
using Prism.Mvvm;

namespace RxFromEventSample.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "Rx FromEvent";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public MainWindowViewModel()
        {
            StartCommand = new DelegateCommand(Start);
            StopOneCommand = new DelegateCommand(StopOne);
            StopTwoCommand = new DelegateCommand(StopTwo);
            StopTimerCommand = new DelegateCommand(StopTimer);
        }

        private void StopOne()
        {
            if (_myDisposer1 != null)
            {
                _myDisposer1.Dispose();
                _myDisposer1 = null;
                Status1 = "stopped one.";
            }
        }

        private void StopTwo()
        {
            if (_myDisposer2 != null)
            {
                _myDisposer2.Dispose();
                _myDisposer2 = null;
                Status2 = "stopped two.";
            }
        }

        private void StopTimer()
        {
            if (_myTimer != null)
            {
                _myTimer.Dispose();
                _myTimer = null;
                StatusTimer = "stopped timer.";
            }
        }

        private void Start()
        {
            var formatDate = "yyyyMMddHHmmssfff";

            //もともとタイマー自体が非同期で動作するので、後述するFromEvent時にスケジューラを登録しなくてもよい
            _myTimer = new System.Timers.Timer(5000);

            //分解バージョンのソース
            var source0 = Observable.FromEvent<ElapsedEventHandler, ElapsedEventArgs>(
                //アクションを返すFuncを登録
                h =>
                {

                    Console.WriteLine("hogemoge");
                    return (s, e) =>
                    {
                        //イベントからFireされた際、OnNextアクションが実行される
                        //この辺に何らかの処理を記述することで、Coldな処理もできそう

                        Console.WriteLine("hogehoge");
                        //Actionの呼び出し
                        h(e);
                    };
                },
                h => _myTimer.Elapsed += h,
                h => _myTimer.Elapsed -= h);

            //分解しないバージョン
            var source = Observable.FromEvent<ElapsedEventHandler, ElapsedEventArgs>(
                                    h => (s, e) => h(e),
                                    h => _myTimer.Elapsed += h,
                                    h => _myTimer.Elapsed -= h);

            //テストコード
            source = source0;

            //Hotな
            _myDisposer1 = source.Subscribe(
                e => Status1 = $"[{DateTime.Now.ToString(formatDate)}] 1##OnNext([{e.SignalTime.ToString(formatDate)}]",
                ex => Status1 = "1##OnError({ ex.Message})",
                () => Status1 = "1##Completed()");

            Status1 = "SubScribed.";

            //Hotな
            _myDisposer2 = source.Subscribe(
                e => Status2 = $"[{DateTime.Now.ToString(formatDate)}] 2##OnNext([{e.SignalTime.ToString(formatDate)}]",
                ex => Status2 = "2##OnError({ ex.Message})",
                () => Status2 = "2##Completed()");

            Status2 = "SubScribed.";

            _myTimer.Start();
            StatusTimer = "Timer Started.";
        }

        private System.Timers.Timer _myTimer = null;

        private IDisposable _myDisposer1 = null;

        private IDisposable _myDisposer2 = null;

        public DelegateCommand StartCommand { get; set; }

        public DelegateCommand StopOneCommand { get; set; }

        public DelegateCommand StopTwoCommand { get; set; }

        public DelegateCommand StopTimerCommand { get; set; }

        private string _myStatus1 = "Sub1 Status.";
        public string Status1
        {
            get { return _myStatus1; }
            set { SetProperty(ref _myStatus1, value); }
        }

        private string _myStatus2 = "Sub2 Status.";
        public string Status2
        {
            get { return _myStatus2; }
            set { SetProperty(ref _myStatus2, value); }
        }

        private string _myStatusTimer = "Timer Status.";
        public string StatusTimer
        {
            get { return _myStatusTimer; }
            set { SetProperty(ref _myStatusTimer, value); }
        }
    }
}
