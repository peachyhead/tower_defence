using System.Collections.Generic;
using System.Linq;
using Features.Tower.Projectile.Views;
using UnityEngine;

namespace Features.Tower.Projectile.Data
{
    [CreateAssetMenu(fileName = "ProjectileRegistry.asset", menuName = "Projectile/Projectile Registry")]
    public class ProjectileRegistry : ScriptableObject
    {
        [SerializeField] private List<ProjectileRegistryItem> _registryItems;

        public ProjectileView GetView(ProjectileType projectileType)
        {
            try
            {
                return _registryItems.First(item => item.Type == projectileType).View;
            }
            catch (KeyNotFoundException)
            {
                throw new KeyNotFoundException($"There is no projectile registered for {projectileType}");
            }
        }
    }
}