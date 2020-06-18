using System;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
	public static WeaponManager Instance { get; private set;}


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
			_weaponsRigs[i].AddForce( forceVector, ForceMode.Acceleration );
		}
	}

	public void InitWeapon ( GameObject prefab, Transform planetOwner, Vector3 direction )
	{
		var weapon = Instantiate( prefab, planetOwner.position, Quaternion.LookRotation( direction ), transform );
		_weaponsRigs.Add( weapon.GetComponent<Rigidbody>( ) );

		var weaponInfo = weapon.GetComponent<Rocket>();
		_rockets.Add( weaponInfo );
	}

	private Vector3 CalculateAcceleration ( Rigidbody rigidbody, WeaponInfo info )
	{
		var localForward = rigidbody.transform.forward;

		var baseAcceleration = localForward * info.Acceleration * Time.deltaTime;

		var position = rigidbody.transform.position;
		var attractionVector = AttractorsManager.CalculateForceVectorForTarget(position, info.Weight);
		//Debug.DrawRay( transform.position,)

		var vectorsSum = baseAcceleration + attractionVector;
		Debug.DrawRay( rigidbody.transform.position, vectorsSum.normalized,Color.red,5 );



		rigidbody.transform.LookAt( rigidbody.transform.position + vectorsSum );

		return baseAcceleration + attractionVector;
	}
}
