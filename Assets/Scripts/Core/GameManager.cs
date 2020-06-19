using System;
using System.Collections.Generic;
using UnityEngine;

internal sealed class GameManager : MonoBehaviour
{
	public event Action OnWin = default;
	public event Action OnLoose = default;
	public List<Planet> Planets => _planets;

	[SerializeField] PlayerHUD _playerHUD = default;
	[SerializeField] InputElements _inputElements = default;
	[SerializeField] PlanetFabric _planetFabric = default;
	[Range(1,4)]
	[SerializeField] int _enemyCount = default;

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
		var playerIndex = UnityEngine.Random.Range(0, _enemyCount - 1 );
		CreatePlanetes( _enemyCount + 1, playerIndex );
	}

	private void CreatePlanetes ( int count, int playerIndex )
	{
		_planets.Clear( );

		for ( int i = 0; i < count; i++ )
		{
			var newPlanet = _planetFabric.CreatePlanet( );

			if ( i == playerIndex )
			{
				var input = _inputElements.GetNewInput( InputType.Mouse );

				( ( MouseInput )input ).SetTargetTransform( _planetFabric.transform );

				newPlanet.SetInput( input );

				_playerHUD.SubscribePlayer( newPlanet );

				_playerPlanet = newPlanet;
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

	private void ClearPlanetes ( )
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
				ClearPlanetes( );

				WeaponManager.Instance.Clear( );

				OnWin?.Invoke( );
			}
			else if ( IsLooseCondition( ) )
			{
				_playerLost = true;
				ClearPlanetes( );

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
