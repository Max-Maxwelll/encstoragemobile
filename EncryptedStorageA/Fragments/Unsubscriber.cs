using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace EncryptedStorageA.Fragments
{
    class Unsubscriber : IDisposable
    {
        private List<IObserver<CustomFragment>> observers;
        private IObserver<CustomFragment> observer;

        public Unsubscriber(List<IObserver<CustomFragment>> observers, IObserver<CustomFragment> observer)
        {
            this.observers = observers;
            this.observer = observer;
        }

        public void Dispose()
        {
            if (observer != null && observers.Contains(observer))
                observers.Remove(observer);
        }
    }
}