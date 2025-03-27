using System;
using Features.Tower.Projectile.Views;
using UnityEngine;

namespace Features.Tower.Projectile.Data
{
    [Serializable]
    public class ProjectileRegistryItem
    {
        [SerializeField] private ProjectileType _type;
        [SerializeField] private ProjectileView _view;
        
        public ProjectileType Type => _type;
        public ProjectileView View => _view;
    }
}