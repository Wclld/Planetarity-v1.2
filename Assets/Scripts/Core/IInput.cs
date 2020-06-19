using System;
using UnityEngine;

internal interface IInput
{
	event Action<Vector2> OnPositionChanged;
	event Action OnFirePressed;
	void SubscribeWeapon ( IWeapon weapon );
}