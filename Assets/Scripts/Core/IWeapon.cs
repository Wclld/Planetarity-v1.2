
using System;
using UnityEngine;

internal interface IWeapon
{
	event Action OnCooldown;
	void Init ( Transform homePlanet, float remainingCooldown = 0 );
	void SetDirection ( Vector2 targetPosition );
	void Fire ( );

	//Temp!
	void SetPrefab ( GameObject prefab );
}