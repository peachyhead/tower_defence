using System;

namespace Features.Utility.Area.Data
{
    public interface IReactiveArea
    {
        public void SetRadius(float radius);
        public IObservable<T> OnTriggerEnterObservable<T>();
        public IObservable<T> OnTriggerExitObservable<T>();
    }
}