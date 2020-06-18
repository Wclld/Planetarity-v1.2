using System;
using UnityEngine;

internal sealed class Planet : MonoBehaviour, IDamagable
{
	[SerializeField] MovementInfo _movementInfo = default;
	//TEMP!
	[SerializeField] GameObject _rocketPrefab = default;

	private IMovement _movement;
	private IWeapon _weapon;

	private Vector3 _nextFramePosition;
	//TEMP!
	[SerializeField] bool _canShoot = default;


	private void Awake ( )
	{
		InitMovement( MovementType.Eliptical );
		ChangeWeapon( new RocketLauncher( ) );
	}

	private void Start ( )
	{
		//TEMP!
		if( _canShoot )
			InputManager.SubscribeToInput( _weapon.Fire, _weapon.SetDirection );
	}

	private void OnValidate ( )
	{
		InitMovement( MovementType.Eliptical );
		transform.position = _movement.UpdatePosition( Time.fixedDeltaTime );
	}

	private void Update ( )
	{
		transform.position = _nextFramePosition;
	}

	private void FixedUpdate ( )
	{
		_nextFramePosition = _movement.UpdatePosition( Time.fixedDeltaTime );
	}

	public void DealDamage ( float damage )
	{
		Debug.Log( $"Damage dealed: {damage}" );
	}


	internal void InitMovement ( MovementType type )
	{
		switch ( type )
		{
			case MovementType.Circular:
				throw new NotImplementedException( );
				//break;
			case MovementType.Eliptical:
				_movement = new ElipticalMovement( );
				_movement.Init( _movementInfo );
				break;
			default:
				throw new NotImplementedException( );
				//break;
		}
	}

	internal void ChangeWeapon ( IWeapon weapon )
	{
		//TEMP
		if ( _canShoot )
		{
			_weapon = weapon;
			_weapon.Init( transform );
			//TEMP!
			_weapon.SetPrefab( _rocketPrefab );
		}
	}

}
