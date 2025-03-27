using Features.Tower.Core.Models;
using Features.Tower.Module;
using Features.Utility.Area.Views;
using UniRx;
using UnityEngine;
using Zenject;

namespace Features.Tower.Core.Views
{
    public class TowerView : MonoBehaviour
    {
        [SerializeField] private SphereArea _sphereArea;
        [SerializeField] private TowerAttackModule _attackModule;
        
        private TowerModel _towerModel;

        [Inject]
        public void Construct(TowerModel towerModel)
        {
            _towerModel = towerModel;
        }
        
        private void Start()
        {
            _towerModel.SetArea(_sphereArea);

            _towerModel
                .TargetObservable()
                .Subscribe(target => _attackModule.Track(target))
                .AddTo(this);
            
            _towerModel
                .ShotObservable()
                .Subscribe(target => _attackModule.Shoot(target))
                .AddTo(this);
        }
    }
}