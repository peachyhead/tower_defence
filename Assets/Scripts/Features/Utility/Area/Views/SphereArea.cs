using System;
using Features.Utility.Area.Data;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Features.Utility.Area.Views
{
    public class SphereArea : MonoBehaviour, IReactiveArea
    {
        [SerializeField] private SphereCollider _collider;

        public void SetRadius(float radius)
        {
            _collider.radius = radius;
        }

        public IObservable<T> OnTriggerEnterObservable<T>()
        {
            return _collider
                .OnTriggerEnterAsObservable()
                .Select(other => other.GetComponent<T>())
                .Where(component => component != null);
        }

        public IObservable<T> OnTriggerExitObservable<T>()
        {
            return _collider
                .OnTriggerExitAsObservable()
                .Select(other => other.GetComponent<T>())
                .Where(component => component != null);
        }
    }
}