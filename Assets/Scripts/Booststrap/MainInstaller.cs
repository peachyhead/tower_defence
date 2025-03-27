using Features.Tower.Projectile.Factories;
using Zenject;

namespace Booststrap
{
    public class MainInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<ProjectileFactory>().AsSingle();
        }
    }
}