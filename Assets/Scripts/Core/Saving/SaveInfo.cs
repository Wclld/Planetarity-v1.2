using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public sealed class SaveInfo
{
	public List<PlanetInfo> Planets = default;
	public List<RocketInfo> Rockets = default;

	public string Serialize ( )
	{
		var result = JsonUtility.ToJson( this );
		return result;
	}

	public void AddPlanetesInfo ( )
	{
		Planets = GameObject.FindObjectOfType<GameManager>( ).GetPlanetsInfo( );
	}
	public void AddRocketsInfo ( )
	{
		Rockets = WeaponManager.Instance.GetRocketsInfo( );
	}

}

