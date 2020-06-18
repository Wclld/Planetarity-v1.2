using System.Collections.Generic;
using UnityEngine;

internal static class AttractorsManager
{
	public const float G = 0.000667f;


	public static List<Attractor> _attractors = new List<Attractor>();


	public static void RegisterAttractor ( Attractor attractor )
	{
		_attractors.Add( attractor );
	}
	public static void RemoveAttractor ( Attractor attractor )
	{
		if ( _attractors.Contains( attractor ) )
		{
			_attractors.Remove( attractor );
		}
		else
		{
			Debug.LogError( "[AttractorsManager.RemoveAttractor]: Attractor is not on the list!" );
		}
	}

	public static Vector3 CalculateForceVectorForTarget ( Vector3 position, float mass )
	{
		var attractionSum = Vector3.zero;

		for ( int i = 0; i < _attractors.Count; i++ )
		{
			if ( _attractors[i] != null )
			{
				var direction = (_attractors[i].CenterOfMass - position);
				var sqrDistance = direction.sqrMagnitude;
				var massMulti = mass * _attractors[i].Mass;
				var resNoG =  massMulti / sqrDistance ;
				var attractionForce = G * resNoG;

				attractionSum += direction * attractionForce;
			}
		}

		return attractionSum / _attractors.Count;
	}
}
