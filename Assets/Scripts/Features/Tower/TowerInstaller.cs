using Features.Tower.Core.Models;
using UnityEngine;
using Zenject;

namespace Features.Tower
{
    public class TowerInstaller : MonoInstaller
    {
        [SerializeField] private float _range;
        [SerializeField] private float _shotInterval;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<TowerModel>()
                .AsCached().WithArguments(_range, _shotInterval);
        }
    }
}