using System.Collections.Generic;
using UnityEngine;

internal sealed class PlanetFabric : MonoBehaviour
{
	private const float CYCLE_MULTIPLIER = 5f;

	[SerializeField] List<GameObject> _planetPrefabs = default;
	[SerializeField] Vector2 _minRadiusChange = default;
	[SerializeField] Vector2 _maxRadiusChange = default;

	private MovementInfo _lastInfo = default;


	internal Planet CreatePlanet ( )
	{
		MovementInfo newInfo = CalculateNewMovement( );
		_lastInfo = newInfo;

		var randomIndex = Random.Range( 0, _planetPrefabs.Count );

		var planet = Instantiate( _planetPrefabs[randomIndex], transform ).GetComponent<Planet>( );
		planet.InitMovement( newInfo );
		planet.SetPrefabInex( randomIndex );

		return planet;
	}

	internal Planet CreatePlanet ( MovementInfo info, int index )
	{
		var planet = Instantiate( _planetPrefabs[index], transform ).GetComponent<Planet>( );
		planet.InitMovement( info );
		planet.SetPrefabInex( index );
		return planet;
	}

	internal void ResetMovementInfo ( )
	{
		_lastInfo = null;
	}



	private MovementInfo CalculateNewMovement ( )
	{
		var newRadius = RandomRadius( );
		var newCycleTime = CalculateCycle( newRadius );
		var newProgress = Random.Range( 0f, 1f );

		var newInfo = new MovementInfo(transform, newRadius, newCycleTime, newProgress);
		return newInfo;
	}

	private float CalculateCycle ( Vector2 newRadius )
	{
		return newRadius.x + newRadius.y * CYCLE_MULTIPLIER;
	}

	private Vector2 RandomRadius ( )
	{
		var xChange = Random.Range(_minRadiusChange.x, _maxRadiusChange.x);
		var yChange = Random.Range(_minRadiusChange.y, _maxRadiusChange.y);

		var radiusChange = new Vector2(xChange,yChange);

		var oldRadius = _lastInfo != null ?
			new Vector2(_lastInfo.XRange, _lastInfo.YRange) :
			Vector2.zero;

		return radiusChange + oldRadius;
	}
}
