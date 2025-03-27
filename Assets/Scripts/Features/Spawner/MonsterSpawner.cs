using System;
using UniRx;
using UnityEngine;

namespace Features.Spawner
{
	public class MonsterSpawner : MonoBehaviour
	{
		[SerializeField] private float _interval = 3;
		[SerializeField] private Transform _moveTarget;
		[SerializeField] private Monster.Views.Monster _prefab;
		
		private void Start ()
		{
			CreateMonster();
			
			Observable
				.Interval(TimeSpan.FromSeconds(_interval))
				.Subscribe(_ => CreateMonster())
				.AddTo(this);
		}

		private void CreateMonster()
		{
			var monster = Instantiate(_prefab);
			monster.transform.position = transform.position;
			monster.SetTarget(_moveTarget);
		}
	}
}
