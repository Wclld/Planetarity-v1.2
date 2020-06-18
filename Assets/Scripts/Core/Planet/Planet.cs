using System;
using UnityEngine;

internal sealed class Planet : MonoBehaviour, IDamagable
{

	public event Action<Planet> OnDeath = default;
	public event Action<int> OnHealthChange = default;

	[SerializeField] int _maxHealth = 10;
	[SerializeField] MovementInfo _movementInfo = default;
	//TEMP!
	[SerializeField] GameObject _rocketPrefab = default;

	private IMovement _movement;
	private IWeapon _weapon;

	private int _currentHealth;

	private Vector3 _nextFramePosition;
	//TEMP!
	[SerializeField] bool _canShoot = default;


	private void Awake ( )
	{
		OnHealthChange += x => Debug.Log( $"hp left:{x}", gameObject );
		OnDeath += x =>
		{
			Debug.Log( $"hp left:{x}", gameObject );
			Destroy( gameObject );
		};
		InitMovement( MovementType.Eliptical );
		ChangeWeapon( new RocketLauncher( ) );
	}

	private void Start ( )
	{
		//TEMP!
		_currentHealth = _maxHealth;
		OnHealthChange += CheckDeath;
		if ( _canShoot )
		{
			
			InputManager.SubscribeToInput( _weapon.Fire, _weapon.SetDirection );
		}
		//tempEnd
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

	public void Init ( MovementInfo info )
	{
		//_currentHealth = _maxHealth;
		//OnHealthChange += CheckDeath;
		_movementInfo = info;
	}

	public void DealDamage ( int damage )
	{
		if ( damage != 0 )
		{
			_currentHealth -= damage;
			_currentHealth = Mathf.Clamp( _currentHealth, 0, _maxHealth );
			OnHealthChange?.Invoke( _currentHealth );
		}
	}


	private void InitMovement ( MovementType type )
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

	private void ChangeWeapon ( IWeapon weapon )
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

	private void CheckDeath ( int health )
	{
		if ( health <= 0 )
		{
			OnDeath?.Invoke( this );
		}
	}
}
