using System;
using UnityEngine;

internal sealed class Planet : MonoBehaviour, IDamagable
{

	public event Action<Planet> OnDeath = default;
	public event Action<int> OnHealthChange = default;

	[SerializeField] int _maxHealth = 10;

	private IMovement _movement;
	private IWeapon _weapon;
	private IInput _input;

	private int _currentHealth;

	private Vector3 _nextFramePosition;


	private void Awake ( )
	{
		OnHealthChange += x => Debug.Log( $"hp left:{x}", gameObject );
		OnDeath += x =>
		{
			Debug.Log( $"hp left:{x}", gameObject );
			Destroy( gameObject );
		};
		ChangeWeapon( new RocketLauncher( ) );
	}


	private void Start ( )
	{
		_currentHealth = _maxHealth;
		OnHealthChange += CheckDeath;
	}

	private void Update ( )
	{
		transform.position = _nextFramePosition;
		_weapon.UpdateCooldown( Time.deltaTime );
	}

	private void FixedUpdate ( )
	{
		_nextFramePosition = _movement.UpdatePosition( Time.fixedDeltaTime );
	}

	public void TakeDamage ( int damage )
	{
		if ( damage != 0 )
		{
			_currentHealth -= damage;
			_currentHealth = Mathf.Clamp( _currentHealth, 0, _maxHealth );
			OnHealthChange?.Invoke( _currentHealth );
		}
	}


	internal void SetInput ( IInput input )
	{
		_input = input;
		_input.SubscribeWeapon( _weapon );
		OnDeath += x => _input.UnsubscribeWeapon( _weapon );
		
		
	}


	internal void InitMovement ( MovementInfo info )
	{
		_movement = new ElipticalMovement( );
		_movement.Init( info );
	}

	private void ChangeWeapon ( IWeapon weapon )
	{
		_weapon = weapon;
		_weapon.Init( transform );

		var prefab = WeaponManager.Instance.GetRandomWeapon( );
		_weapon.SetPrefab( prefab );
	}

	private void CheckDeath ( int health )
	{
		if ( health <= 0 )
		{
			OnDeath?.Invoke( this );
		}
	}
}
