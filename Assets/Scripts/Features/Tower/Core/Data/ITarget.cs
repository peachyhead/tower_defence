using System;
using UniRx;
using UnityEngine;

namespace Features.Tower.Core.Data
{
    public interface ITarget
    {
        public Vector3 Position { get; }
        public Vector3 Velocity { get; }

        public void Damage(float damage);
        
        public IObservable<Unit> OnDestroyAsObservable();
    }
}