using System;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;
using Prism.Commands;
using Prism.Mvvm;

namespace RxFromEventPatternSample.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "Rx FromEventPattern";
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
            FireButtonClickCommand = new DelegateCommand(FireButtonClick);
            DisposeButtonCommand = new DelegateCommand(DisposeButton);
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

        private void FireButtonClick()
        {
            _myButton?.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        }

        public void DisposeButton()
        {
            if (_myButton != null) _myButton = null;
        }

        private void Start()
        {
            var formatDate = "yyyyMMddHHmmssfff";

            _myButton = new Button();

            //Funcを渡さないバージョンのFromEvent
            //UI系のイベントはFromEventPatternでキャプチャする
            var source = Observable.FromEventPattern<RoutedEventHandler, RoutedEventArgs>(
                h =>
                {
                    Console.WriteLine("add");
                    _myButton.Click += h;
                },
                h =>
                {
                    Console.WriteLine("Remove");
                    _myButton.Click -= h;
                });

            //_myButton.Click += (sender, args) => Console.WriteLine(args.ToString());

            //Hotな
            _myDisposer1 = source.Subscribe(
                e => Status1 = $"[{DateTime.Now.ToString(formatDate)}] 1##OnNext([{e.ToString()}]",
                ex => Status1 = "1##OnError({ ex.Message})",
                () => Status1 = "1##Completed()");

            Status1 = "SubScribed.";

            //Hotな
            _myDisposer2 = source.Subscribe(
                e => Status2 = $"[{DateTime.Now.ToString(formatDate)}] 2##OnNext([{e.ToString()}]",
                ex => Status2 = "2##OnError({ ex.Message})",
                () => Status2 = "2##Completed()");

            Status2 = "SubScribed.";
        }

        private Button _myButton = null;

        private IDisposable _myDisposer1 = null;

        private IDisposable _myDisposer2 = null;

        public DelegateCommand StartCommand { get; set; }

        public DelegateCommand StopOneCommand { get; set; }

        public DelegateCommand StopTwoCommand { get; set; }

        public DelegateCommand FireButtonClickCommand { get; set; }

        public DelegateCommand DisposeButtonCommand { get; set; }

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
