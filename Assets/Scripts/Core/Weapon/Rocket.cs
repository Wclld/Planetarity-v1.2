using System;
using UnityEngine;

public class Rocket : MonoBehaviour
{
	public WeaponInfo Info => _info;

	[SerializeField] WeaponInfo _info = default;
	private bool _launchTriggerLeaved = false;

	private void OnTriggerEnter ( Collider other )
	{
		if ( _launchTriggerLeaved )
		{
			TryDealDamage( other );

			DestroyFlow( );
		}
	}

	private void OnTriggerExit ( Collider other )
	{
		_launchTriggerLeaved = true;
	}

	private void TryDealDamage ( Collider other )
	{
		var damagable = other.GetComponentInParent<IDamagable>();
		if ( damagable != null )
		{
			damagable.TakeDamage( _info.Damage );
		}
	}

	private void DestroyFlow ( )
	{
		WeaponManager.Instance.RemoveRocket( this );
		Destroy( gameObject );
	}
}