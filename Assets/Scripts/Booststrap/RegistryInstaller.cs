using Features.Tower.Projectile.Data;
using UnityEngine;
using Zenject;

namespace Booststrap
{
    [CreateAssetMenu(fileName = "RegistryInstaller", menuName = "Booststrap/Registry")]
    public class RegistryInstaller : ScriptableObjectInstaller<RegistryInstaller>
    {
        [SerializeField] private ProjectileRegistry _projectileRegistry;
        
        public override void InstallBindings()
        {
            Container.Bind<ProjectileRegistry>().FromInstance(_projectileRegistry);
        }
    }
}