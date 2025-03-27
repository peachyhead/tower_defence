using System;
using Features.Tower.Core.Data;
using Features.Tower.Module.Data;
using Features.Tower.Projectile.Factories;
using UniRx;
using UnityEngine;
using Zenject;

namespace Features.Tower.Module
{
    public abstract class TowerAttackModule : MonoBehaviour
    {
        [SerializeField] protected Transform MuzzlePoint;
        [SerializeField] protected ShootingModuleData Data;
        [SerializeField] private Transform _root;
        
        protected abstract Vector3 GetAimPoint(ITarget target);
        protected abstract Vector3 GetLaunchVelocity(ITarget target);
        private ProjectileFactory _projectileFactory;

        private IDisposable _targetTrackStream;
        
        [Inject]
        public void Construct(ProjectileFactory projectileFactory)
        {
            _projectileFactory = projectileFactory;
        }
        
        public void Track(ITarget target)
        {
            _targetTrackStream?.Dispose();
            
            _targetTrackStream = Observable
                .EveryUpdate()
                .Where(_ => target != null)
                .Subscribe(_ =>
                {
                    var aimPoint = GetAimPoint(target);
                    var direction = (aimPoint - _root.position).normalized;
                    var targetRotation = Quaternion.LookRotation(direction);
                    _root.rotation = Quaternion.Slerp(_root.rotation, targetRotation,
                        Data.RotationSpeed * Time.deltaTime);
                });
        }

        public void Shoot(ITarget target)
        {
            var projectile = _projectileFactory.Create(Data.ProjectileType, Data.Damage);
            projectile.transform.position = MuzzlePoint.position;
            projectile.Launch(GetLaunchVelocity(target));
        }
    }
}