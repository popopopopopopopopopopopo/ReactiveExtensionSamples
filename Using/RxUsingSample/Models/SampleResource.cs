using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RxUsingSample.Models
{
    public class SampleResource : IDisposable
    {
        // データを取得する 
        public IObservable<string> GetData()
        {
            return Observable.Create<string>(o =>
            {
                o.OnNext("one");
                o.OnNext("two");
                o.OnNext("three");
                o.OnCompleted();
                return Disposable.Empty;
            });
        }

        // 解放処理 
        public void Dispose()
        {
            Console.WriteLine("Resource.Dispose called");
        }
    }
}
