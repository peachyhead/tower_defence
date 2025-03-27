using System;
using Features.Tower.Core.Data;
using UniRx;
using UnityEngine;
using Zenject;

namespace Features.Tower.Projectile.Views
{
    [RequireComponent(typeof(Rigidbody))]
    public class ProjectileView : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;

        private float _damage;
        
        [Inject]
        public void Construct(float damage)
        {
            _damage = damage;
        }
        
        public void Launch(Vector3 velocity)
        {
            _rigidbody.velocity = velocity;

            Observable
                .Timer(TimeSpan.FromSeconds(20))
                .Subscribe(_ => Destroy(gameObject))
                .AddTo(this);
        }
        
        private void OnTriggerEnter(Collider other) {
            var target = other.gameObject.GetComponent<ITarget> ();
            if (target == null)
                return;

            target.Damage(_damage);
            Destroy(gameObject);
        }
    }
}