
using System;
using UnityEngine;

internal interface IWeapon
{
	event Action OnCooldown;
	event Action OnCooldownFinished;
	Transform Owner{ get; }
	void Init ( Transform homePlanet, float remainingCooldown = 0 );
	void SetDirection ( Vector2 targetPosition );
	void UpdateCooldown ( float time );
	void Fire ( );

	//Temp!
	void SetPrefab ( GameObject prefab );
}