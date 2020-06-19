using System;
using UnityEngine;

[Serializable]
public class WeaponInfo
{
	public float Cooldown => _cooldown;
	public float Weight => _weight;
	public int Damage => _damage;
	public float StartAccelerationDuration => _startAccelerationDuration;
	public float Acceleration => _acceleration;
	public bool IsStartAccelerationFinished => _isStartAccelerationFinished;
	public void TurnOffStartAcceleration ( ) => _isStartAccelerationFinished = true;
	

	[SerializeField] float _acceleration = default;
	[SerializeField] float _startAccelerationDuration = default;
	[SerializeField] float _weight = default;
	[SerializeField] int _damage = default;
	[SerializeField] float _cooldown = default;

	private bool _isStartAccelerationFinished = false;
}
