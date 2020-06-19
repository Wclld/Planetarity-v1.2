using System;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

internal class EnemyAIInput : MonoBehaviour, IInput
{
	private const float RANDOM_ANGLE_RANGE = 5f;
	public event Action<Vector2> OnPositionChanged = default;
	public event Action OnFirePressed = default;

	private List<Planet> _planets = new List<Planet>( );
	private Transform _planetOwner = default;

	private void Awake ( )
	{
		var gameManager = FindObjectOfType<GameManager>( );
		if ( gameManager == null )
		{
			Debug.LogError( "[EnemyAIInput.Awake]: Can't find GameManager! Self-destroying!" );
			Destroy( gameObject );
		}
		_planets = gameManager.Planets;
	}


	public void SubscribeWeapon ( IWeapon weapon )
	{
		OnFirePressed += weapon.Fire;
		OnPositionChanged += weapon.SetDirection;

		weapon.OnCooldownFinished += Aim;
		weapon.OnCooldownFinished += Shoot;

		_planetOwner = weapon.Owner;
	}

	private void Aim ( )
	{

		//TODO: Add prediction
		var closestPlanetPosition = GetClosestPlanet( );

		var newAimDirection = CalculateRandomedPosition( closestPlanetPosition );
		var newAimPosition = _planetOwner.position + newAimDirection;

		OnPositionChanged?.Invoke( newAimPosition );
	}

	private void Shoot ( )
	{
		OnFirePressed?.Invoke( );
	}

	private Vector3 GetClosestPlanet ( )
	{
		var planetPositions = GetPlanetPositions( );

		var closestPosition = Vector2.zero;
		var closestDistance = float.MaxValue;

		for ( int i = 0; i < planetPositions.Count; i++ )
		{
			var currntDistance = Vector2.Distance(planetPositions[i], _planetOwner.position);

			if ( currntDistance < closestDistance )
			{
				closestPosition = planetPositions[i];
				closestDistance = currntDistance;
			}
		}

		return closestPosition;
	}

	private Vector3 CalculateRandomedPosition ( Vector3 targetPosition )
	{
		var direction = targetPosition - _planetOwner.position;

		var distance = direction.magnitude;

		direction = direction.normalized;
		var angle = Vector2.Angle( Vector2.right, direction );

		var randomedAngle = Random.Range(-RANDOM_ANGLE_RANGE, RANDOM_ANGLE_RANGE);

		var resultAngle = (angle + randomedAngle) * Mathf.Deg2Rad;

		var resultDirection = new Vector2(Mathf.Sin(resultAngle), Mathf.Cos(resultAngle));

		return resultDirection;
	}

	private List<Vector2> GetPlanetPositions ( )
	{
		var planetPositions = new List<Vector2>( );
		for ( int i = 0; i < _planets.Count; i++ )
		{
			var planetTransform = _planets[i].transform;
			if ( planetTransform != _planetOwner )
			{
				planetPositions.Add( planetTransform.position );
			}
		}

		return planetPositions;
	}
}
