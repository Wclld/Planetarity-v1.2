using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
	public static WeaponManager Instance
	{
		get; private set;
	}


	private List<Rigidbody> _weaponsRigs = new List<Rigidbody>();
	private List<Rocket> _rockets = new List<Rocket>();

	private void Awake ( )
	{
		if ( Instance == null )
		{
			Instance = this;
		}
	}

	private void Start ( )
	{
		//TODO:
		//Subscribe to Attractor Initialisation
	}

	private void Update ( )
	{
		for ( int i = 0; i < _weaponsRigs.Count; i++ )
		{
			var forceVector = CalculateAcceleration( _weaponsRigs[i], _rockets[i].Info );
			try
			{
				_weaponsRigs[i].AddForce( forceVector, ForceMode.Acceleration );
			}
			catch
			{
				Debug.LogError( "Here" );
			}
		}
	}

	public void InitWeapon ( GameObject prefab, Transform planetOwner, Vector3 direction )
	{
		var weapon = Instantiate( prefab, planetOwner.position, Quaternion.LookRotation( direction ), transform );
		_weaponsRigs.Add( weapon.GetComponent<Rigidbody>( ) );

		var weaponInfo = weapon.GetComponent<Rocket>();
		_rockets.Add( weaponInfo );
		StartCoroutine( WaitForStartAccelerationFinished( weaponInfo.Info ) );
	}

	public void RemoveRocket ( Rocket rocket )
	{
		if ( _rockets.Contains( rocket ) )
		{
			_rockets.Remove( rocket );
			var rocketRig = rocket.GetComponent<Rigidbody>();
			if ( _weaponsRigs.Contains( rocketRig ) )
			{
				_weaponsRigs.Remove( rocketRig );
			}
		}
	}


	private IEnumerator WaitForStartAccelerationFinished ( WeaponInfo info )
	{
		yield return new WaitForSeconds( info.StartAccelerationDuration );
		info.TurnOffStartAcceleration( );
	}

	private Vector3 CalculateAcceleration ( Rigidbody rigidbody, WeaponInfo info )
	{
		var localForward = rigidbody.transform.forward;

		var baseAcceleration = localForward * info.Acceleration * Time.deltaTime;
		if ( !info.IsStartAccelerationFinished )
			return baseAcceleration;

		var position = rigidbody.transform.position;
		var attractionVector = AttractorsManager.CalculateForceVectorForTarget(position, info.Weight);
		//Debug.DrawRay( transform.position,)

		var vectorsSum = baseAcceleration + attractionVector;

		Debug.DrawRay( rigidbody.transform.position, vectorsSum.normalized, Color.red, 5 );



		rigidbody.transform.LookAt( rigidbody.transform.position + vectorsSum );

		return baseAcceleration + attractionVector;
	}
}
