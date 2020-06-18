using System;
using UnityEngine;

[Serializable]
public class WeaponInfo
{
	public float Cooldown => _cooldown;
	public float Weight => _weight;
	public float Damage => _damage;
	public float StartAccelerationDuration => _startAccelerationDuration;
	public float Acceleration => _acceleration;
	public bool IsStartAccelerationFinished => _isStartAccelerationFinished;
	public void TurnOffStartAcceleration ( ) => _isStartAccelerationFinished = true;
	

	[SerializeField] float _acceleration;
	[SerializeField] float _startAccelerationDuration;
	[SerializeField] float _weight;
	[SerializeField] float _damage;
	[SerializeField] float _cooldown;

	private bool _isStartAccelerationFinished = false;
}
