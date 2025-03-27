using System;
using Features.Tower.Projectile.Data;
using Features.Tower.Projectile.Views;
using UnityEngine;
using Zenject;

namespace Features.Tower.Projectile.Factories
{
    public class ProjectileFactory : IFactory<ProjectileType, float, ProjectileView>
    {
        private readonly DiContainer _container;
        private readonly ProjectileRegistry _registry;

        public ProjectileFactory(DiContainer container, ProjectileRegistry registry)
        {
            _container = container;
            _registry = registry;
        }

        public ProjectileView Create(ProjectileType type, float damage)
        {
            var item = _registry.GetView(type);
            var view = _container.InstantiatePrefabForComponent<ProjectileView>(item, 
                new object[] {damage});
            return view;
        }
    }
}