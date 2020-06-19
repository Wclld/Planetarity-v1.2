using System;
using UnityEngine;

internal sealed class RocketLauncher : IWeapon
{
	public event Action OnCooldown = default;
	public event Action OnCooldownFinished = default;

	private int _rocketPrefabIndex;

	private Transform _homePlanet = default;
	private Vector3 _aimDirection = default;
	private float _maxCooldown = 0;
	private float _currentCooldown = 0;


	public Transform Owner => _homePlanet;

	public void Init ( Transform homePlanet, float remainingCooldown = 0 )
	{
		_homePlanet = homePlanet;
	}

	public void SetWeaponIndex ( int index )
	{
		_rocketPrefabIndex = index;

		_maxCooldown = WeaponManager.Instance.GetPrefabWeaponInfoByIndex( index ).Cooldown;
		_currentCooldown = _maxCooldown;
	}

	public void Fire ( )
	{
		if ( _currentCooldown > 0 )
		{
			OnCooldown?.Invoke( );
		}
		else
		{
			LaunchRocket( );
		}
	}

	private void LaunchRocket ( )
	{
		WeaponManager.Instance.InitWeapon( _rocketPrefabIndex, _homePlanet.position, _aimDirection );
		_currentCooldown = _maxCooldown;

	}

	public void SetDirection ( Vector2 targetPosition )
	{
		_aimDirection = ( targetPosition - ( Vector2 )_homePlanet.position ).normalized;
	}

	public float UpdateCooldown ( float time )
	{
		_currentCooldown -= time;
		if ( _currentCooldown <= 0 )
		{
			OnCooldownFinished?.Invoke( );
		}
		_currentCooldown = Mathf.Clamp( _currentCooldown, 0, _maxCooldown );

		return _currentCooldown / _maxCooldown;
	}
}
