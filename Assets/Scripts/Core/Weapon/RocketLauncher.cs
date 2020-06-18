using System;
using UnityEngine;

internal sealed class RocketLauncher : IWeapon
{
	public event Action OnCooldown = default;

	[SerializeField] GameObject _rocketPrefab;

	private Transform _homePlanet = default;
	private Vector3 _aimDirection = default;
	private float _cooldown = 0;

	public void Init ( Transform homePlanet, float remainingCooldown = 0 )
	{
		_homePlanet = homePlanet;

		OnCooldown += CooldownAction;

	}

	public void SetPrefab ( GameObject prefab )
	{
		_rocketPrefab = prefab;
	}

	public void Fire ( )
	{
		if ( _cooldown > 0 )
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
		WeaponManager.Instance.InitWeapon( _rocketPrefab, _homePlanet, _aimDirection );
	}

	public void SetDirection ( Vector2 targetPosition )
	{
		_aimDirection = ( targetPosition - ( Vector2 )_homePlanet.position ).normalized;
		Debug.DrawRay( _homePlanet.position, _aimDirection, Color.cyan, 5 );
	}


	private void CooldownAction ( )
	{
		throw new NotImplementedException( );
	}
}