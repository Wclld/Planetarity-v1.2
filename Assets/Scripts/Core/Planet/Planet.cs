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

	private int _currentHealth;

	private Vector3 _nextFramePosition;


	private void Awake ( )
	{
		OnDeath += RemoveInput;

		ChangeWeapon( new RocketLauncher( ) );
	}

	private void Start ( )
	{
		_currentHealth = _maxHealth;
		_healthBar.Init( _maxHealth );
		OnHealthChange += CheckDeath;
		OnHealthChange += _healthBar.ChangeHealth;

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
