using System;
using Features.Tower.Projectile.Data;
using UnityEngine;

namespace Features.Tower.Module.Data
{
    [Serializable]
    public class ShootingModuleData
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _damage;
        [SerializeField] private float _rotationSpeed;
        [SerializeField] private ProjectileType _projectileType;
        
        public float Speed => _speed;
        public float Damage => _damage;
        public float RotationSpeed => _rotationSpeed;
        public ProjectileType ProjectileType => _projectileType;
    }
}