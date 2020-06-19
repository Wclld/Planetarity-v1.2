using System;
using UnityEngine;

internal sealed class Planet : MonoBehaviour, IDamagable
{

	public event Action<Planet> OnDeath = default;
	public event Action<int> OnHealthChange = default;
	public event Action<float> OnUpdateCooldown = default;
	public int MaxHealth => _maxHealth;

	[SerializeField] int _maxHealth = 10;
	[SerializeField] HealthBar _healthBar = default;
	[SerializeField] Cooldown _cooldownHUD = default;

	private IMovement _movement;
	private IWeapon _weapon;
	private IInput _input;

	private MovementInfo _movementInfo = default;

	private int _currentHealth = 0;
	private int _weaponIndex;

	private Vector3 _nextFramePosition;


	private void Awake ( )
	{
		OnDeath += RemoveInput;

	}

	private void Start ( )
	{
		if ( _currentHealth == 0 )
		{
			InitHealth( _maxHealth );
		}

		OnUpdateCooldown += _cooldownHUD.UpdateCooldown;
	}

	private void Update ( )
	{
		transform.position = _nextFramePosition;
		var cooldownLeft = _weapon.UpdateCooldown( Time.deltaTime );

		OnUpdateCooldown?.Invoke( cooldownLeft );
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

	public void Die ( )
	{
		OnDeath?.Invoke( this );
		Destroy( gameObject );
	}

	internal void SetInput ( IInput input )
	{
		_input = input;
		_input.SubscribeWeapon( _weapon );
	}

	internal void InitMovement ( MovementInfo info )
	{
		_movement = new ElipticalMovement( );
		_movement.Init( info );
		_movementInfo = info;
	}

	internal void InitHealth ( int health )
	{
		_currentHealth = health;
		_healthBar.Init( _maxHealth, health );
		OnHealthChange += CheckDeath;
		OnHealthChange += _healthBar.ChangeHealth;
	}

	internal PlanetInfo GetPlanetInfo ( )
	{
		var inputType = _input is MouseInput ? InputType.Mouse : InputType.AIEnemy;

		_movementInfo.SetProgress(_movement.CurrentProgress);
		var info = new PlanetInfo( )
		{
			HealthPoints = _currentHealth,
			MovementInfo = _movementInfo,
			WeaponIndex = _weaponIndex,
			InputType = inputType,
			MovementType = MovementType.Eliptical

		};
		return info;
	}

	internal void ChangeWeapon ( IWeapon weapon, int weaponIndex = -1 )
	{
		_weapon = weapon;
		_weapon.Init( transform );

		_weaponIndex = weaponIndex < 0 ? WeaponManager.Instance.GetRandomWeaponIndex( ) : weaponIndex;

		_weapon.SetWeaponIndex( _weaponIndex );

	}

	private void CheckDeath ( int health )
	{
		if ( health <= 0 )
		{
			Die( );
		}
	}

	private void RemoveInput ( Planet planet )
	{
		if ( _input != null )
		{
			_input.UnsubscribeWeapon( _weapon );
			_input.SelfDestroy( );
		}
	}
}
