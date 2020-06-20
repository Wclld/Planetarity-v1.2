using System;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

internal sealed class GameManager : MonoBehaviour
{
	public event Action OnWin = default;
	public event Action OnLoose = default;
	public List<Planet> Planets => _planets;

	[SerializeField] PlayerHUD _playerHUD = default;
	[SerializeField] InputElements _inputElements = default;
	[SerializeField] PlanetFabric _planetFabric = default;
	[Range(1,4)]
	[SerializeField] int _maxEnemyCount = default;
	[SerializeField] Saver _saver = default;

	private List<Planet> _planets = new List<Planet>( );
	private Planet _playerPlanet = default;
	private bool _playerLost = false;

	private void Awake ( )
	{
		OnWin += ( ) => Debug.Log( "You Win!" );
		OnLoose += ( ) => Debug.Log( "Good luck next time!" );

		OnWin += _planetFabric.ResetMovementInfo;
		OnLoose += _planetFabric.ResetMovementInfo;
	}

	public void InitGame ( )
	{
		_playerLost = false;
		var randomedEnemyCount = Random.Range( 1, _maxEnemyCount );
		var playerIndex = Random.Range( 0, randomedEnemyCount - 1 );
		CreatePlanets( randomedEnemyCount + 1, playerIndex );
	}

	public void LoadFromSave ( )
	{
		if ( _saver != null )
		{
			_playerLost = false;

			var saveInfo = _saver.Load( );
			SetPlanetsFromSave( saveInfo.Planets );
			WeaponManager.Instance.SetRocketsFromSave( saveInfo.Rockets );
		}
	}

	public List<PlanetInfo> GetPlanetsInfo ( )
	{
		var result = new List<PlanetInfo>( );
		for ( int i = 0; i < _planets.Count; i++ )
		{
			result.Add( _planets[i].GetPlanetInfo( ) );
		}
		return result;
	}


	private void SetPlanetsFromSave ( List<PlanetInfo> planets )
	{
		_planets.Clear( );

		for ( int i = 0; i < planets.Count; i++ )
		{
			var currentplanetInfo = planets[i];

			var planet = _planetFabric.CreatePlanet( currentplanetInfo.MovementInfo, currentplanetInfo.PrefabIndex );

			planet.ChangeWeapon( new RocketLauncher( ), currentplanetInfo.WeaponIndex );

			var input = _inputElements.GetNewInput( currentplanetInfo.InputType );
			if ( currentplanetInfo.InputType != InputType.AIEnemy )
			{
				InitPlayerPlanet( planet, input );
			}
			else
			{
				planet.SetInput( input );
			}

			planet.OnDeath += RemovePlanet;
			planet.InitHealth( currentplanetInfo.HealthPoints );

			_planets.Add( planet );
		}
	}

	private void CreatePlanets ( int count, int playerIndex )
	{
		_planets.Clear( );

		for ( int i = 0; i < count; i++ )
		{
			var newPlanet = _planetFabric.CreatePlanet( );
			newPlanet.ChangeWeapon( new RocketLauncher( ) );
			if ( i == playerIndex )
			{
				var input = _inputElements.GetNewInput( InputType.Mouse );

				InitPlayerPlanet( newPlanet, input );
			}
			else
			{
				var input = _inputElements.GetNewInput( InputType.AIEnemy );
				newPlanet.SetInput( input );
			}

			newPlanet.OnDeath += RemovePlanet;

			_planets.Add( newPlanet );
		}
	}

	private void InitPlayerPlanet ( Planet loadedPlanet, IInput input )
	{
		( input as MouseInput ).SetTargetTransform( _planetFabric.transform );
		loadedPlanet.SetInput( input );
		_playerHUD.SubscribePlayer( loadedPlanet );
		_playerPlanet = loadedPlanet;
	}

	private void ClearPlanets ( )
	{
		while ( _planets.Count > 0 )
		{
			_planets[0].Die( );
		}
	}

	private void RemovePlanet ( Planet planet )
	{
		if ( _planets.Contains( planet ) )
		{
			_planets.Remove( planet );
		}

		CheckConditions( );
	}

	private void CheckConditions ( )
	{
		if ( !_playerLost )
		{
			if ( IsWinCondition( ) )
			{
				ClearPlanets( );

				WeaponManager.Instance.Clear( );

				OnWin?.Invoke( );
			}
			else if ( IsLooseCondition( ) )
			{
				_playerLost = true;
				ClearPlanets( );

				WeaponManager.Instance.Clear( );
				OnLoose?.Invoke( );
			}
		}
	}

	private bool IsWinCondition ( )
	{
		if ( _planets.Count == 1 )
		{
			if ( _planets[0] == _playerPlanet )
			{
				return true;
			}
		}
		return false;
	}

	private bool IsLooseCondition ( )
	{
		if ( _planets.Count > 0 )
		{
			if ( !_planets.Contains( _playerPlanet ) )
			{
				return true;
			}
		}
		return false;
	}
}
