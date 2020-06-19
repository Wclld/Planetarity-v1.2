using System.Collections.Generic;
using UnityEngine;

internal sealed class GameManager : MonoBehaviour
{
	public List<Planet> Planets => _planets;

	[SerializeField] InputElements _inputElements = default;
	[SerializeField] PlanetFabric _planetFabric = default;
	[Range(1,4)]
	[SerializeField] int _enemyCount = default;

	private List<Planet> _planets = new List<Planet>( );

	private void Start ( )
	{
		var playerIndex = Random.Range(0, _enemyCount - 1 );
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

	private void RemovePlanet ( Planet planet )
	{
		if ( _planets.Contains( planet ) )
		{
			_planets.Remove( planet );
		}
	}
}
