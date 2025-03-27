using System;
using System.Linq;
using Features.Tower.Core.Data;
using Features.Utility.Area.Data;
using UniRx;
using Zenject;

namespace Features.Tower.Core.Models
{
    public class TowerModel : IInitializable, IDisposable
    {
        private readonly float _range;
        private readonly float _shootInterval;

        private readonly Subject<Unit> _disposeSubject = new();
        private readonly Subject<ITarget> _shotSubject = new();
        private readonly ReactiveCollection<ITarget> _targets = new();
        private readonly ReactiveProperty<ITarget> _currentTarget = new();
        
        private readonly CompositeDisposable _disposable = new();
        
        public TowerModel(float range, float shootInterval)
        {
            _range = range;
            _shootInterval = shootInterval;
        }
        
        public void Initialize()
        {
            Observable
                .Interval(TimeSpan.FromSeconds(_shootInterval))
                .Select(_ => _currentTarget.Value)
                .Where(target => target != null)
                .Subscribe(_shotSubject.OnNext)
                .AddTo(_disposable);

            _targets.ObserveAdd().AsUnitObservable()
                .Merge(_targets.ObserveRemove().AsUnitObservable())
                .Select(_ => _targets.FirstOrDefault())
                .Subscribe(target => _currentTarget.Value = target)
                .AddTo(_disposable);
        }
        
        public void SetArea(IReactiveArea reactiveArea)
        {
            reactiveArea.SetRadius(_range);
            reactiveArea
                .OnTriggerEnterObservable<ITarget>()
                .Subscribe(target =>
                {
                    _targets.Add(target);
                    target.OnDestroyAsObservable()
                        .Take(1)
                        .Subscribe(_ => _targets.Remove(target));
                })
                .AddTo(_disposable);
            
            reactiveArea
                .OnTriggerExitObservable<ITarget>()
                .Subscribe(target => _targets.Remove(target))
                .AddTo(_disposable);
        }

        public IObservable<ITarget> TargetObservable()
        {
            return _currentTarget.AsObservable(); 
        }

        public IObservable<ITarget> ShotObservable()
        {
            return _shotSubject.AsObservable();
        }

        public void Dispose()
        {
            _targets.Clear();
            _disposable.Dispose();
            _shotSubject.Dispose();
            _disposeSubject.OnNext(new Unit());
            _disposeSubject.Dispose();
        }
    }
}