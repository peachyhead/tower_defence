using System;
using Features.Tower.Core.Data;
using UniRx;
using UnityEngine;

namespace Features.Monster.Views
{
	public class Monster : MonoBehaviour, ITarget
	{
		public Vector3 Position => transform.position;
		public Vector3 Velocity => _velocity;
		
		[SerializeField] private float _reachDistance = 0.3f;
		[SerializeField] private float _speed = 0.1f;
		[SerializeField] private int _maxHP = 30;

		private readonly ReactiveProperty<float> _healthProperty = new();
		private Vector3 _velocity;
		private Transform _target;
		
		private void Start() 
		{
			_healthProperty.Value = _maxHP;

			_healthProperty
				.Where(value => value <= 0)
				.Subscribe(_ => Destroy(gameObject))
				.AddTo(this);
		}

		public void Damage(float damage)
		{
			_healthProperty.Value -= damage;
		}

		public IObservable<Unit> OnDestroyAsObservable()
		{
			return _healthProperty
				.Where(value => value <= 0)
				.AsUnitObservable();
		}

		public void SetTarget(Transform target)
		{
			_target = target;
		}
	
		private void Update () {
			if (_target ==null)
				return;
		
			if (Vector3.Distance(transform.position, _target.transform.position) <= _reachDistance) {
				Destroy(gameObject);
				return;
			}

			_velocity = _target.transform.position - transform.position;
			if (_velocity.magnitude > _speed)
			{
				_velocity = _velocity.normalized * _speed;
			}
			transform.Translate(_velocity);
		}
	}
}
