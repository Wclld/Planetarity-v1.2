using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
	public static WeaponManager Instance
	{
		get; private set;
	}

	[SerializeField] List<GameObject> _rocketVariants = new List<GameObject>( );

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
	}

	private void Update ( )
	{
		for ( int i = 0; i < _weaponsRigs.Count; i++ )
		{
			var forceVector = CalculateAcceleration( _weaponsRigs[i], _rockets[i].Info );
			
			_weaponsRigs[i].AddForce( forceVector, ForceMode.Acceleration );
		}
	}

	public void Clear ( )
	{
		while ( _rockets.Count > 0 )
		{
			_rockets[0].DestroyFlow( );
		}
	}

	public void SetRocketsFromSave ( List<RocketInfo> info )
	{
		for ( int i = 0; i < info.Count; i++ )
		{
			var rocket = InitWeapon(info[i].WeaponInfoIndex, info[i].Position, Vector3.one, false);
			rocket.transform.rotation = Quaternion.Euler( info[i].Rotation);
			rocket.GetComponent<Rigidbody>( ).velocity = info[i].Velocity;
		}
	}

	public WeaponInfo GetPrefabWeaponInfoByIndex ( int index )
	{
		return _rocketVariants[index].GetComponent<Rocket>( ).Info;
	}

	public Rocket InitWeapon ( int prefabIndex, Vector3 StartPosition, Vector3 direction, bool withAcceleration = true )
	{
		var prefab = _rocketVariants[prefabIndex];

		var weapon = Instantiate( prefab, StartPosition, Quaternion.LookRotation( direction ), transform );
		_weaponsRigs.Add( weapon.GetComponent<Rigidbody>( ) );

		var weaponInfo = weapon.GetComponent<Rocket>();
		weaponInfo.SetWeaponPrefabIndex( prefabIndex );

		_rockets.Add( weaponInfo );

		if ( withAcceleration )
		{
			StartCoroutine( WaitForStartAccelerationFinished( weaponInfo.Info ) );
		}
		return weaponInfo;
	}

	public int GetRandomWeaponIndex ( )
	{
		var index = Random.Range(0, _rocketVariants.Count);
		return index;
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

	internal List<RocketInfo> GetRocketsInfo ( )
	{
		var result = new List<RocketInfo>();
		for ( int i = 0; i < _rockets.Count; i++ )
		{
			var info = _rockets[i].GetInfo( );
			result.Add( info );
		}

		return result;
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

		var vectorsSum = baseAcceleration + attractionVector;

		rigidbody.transform.LookAt( rigidbody.transform.position + vectorsSum );

		return baseAcceleration + attractionVector;
	}
}
