using System;
using UnityEngine;

public class Rocket : MonoBehaviour
{
	public WeaponInfo Info => _info;

	[SerializeField] WeaponInfo _info = default;
	private bool _launchTriggerLeaved = false;

	private int _prefabIndex;


	public void DestroyFlow ( )
	{
		WeaponManager.Instance.RemoveRocket( this );
		Destroy( gameObject );
	}

	public void SetWeaponPrefabIndex ( int index )
	{
		_prefabIndex = index;
	}

	internal RocketInfo GetInfo ( )
	{
		var info = new RocketInfo()
		{
			Position = transform.position,
			Rotation = transform.rotation.eulerAngles,
			Velocity =  GetComponent<Rigidbody>().velocity,
			WeaponInfoIndex = _prefabIndex
		};
		return info;
	}

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

}