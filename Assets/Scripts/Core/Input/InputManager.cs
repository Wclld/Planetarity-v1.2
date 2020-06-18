using System;
using UnityEngine;

internal static class InputManager
{
	private static IInput _input;
	internal static void RegisterInput ( IInput input )
	{
		if ( _input != null )
		{
			Debug.LogError( "[InputManager.RegisterInput]: Input is allready set!",(MonoBehaviour)input );
			return;
		}
		_input = input;
	}

	internal static void  SubscribeToInput ( Action FireAction, Action<Vector2> PositionChangeAction )
	{
		if ( _input == null )
		{
			Debug.LogError( "[InputManager.SubscribeToInput]: Input is null or not set!" );
			return;
		}
		_input.OnFirePressed += FireAction;
		_input.OnPositionChanged += PositionChangeAction;

	}

	internal static void UnsubscribeFromInput ( Action FireAction, Action<Vector2> PositionChangeAction )
	{
		if ( _input == null )
		{
			Debug.LogError( "[InputManager.UnsubscribeFromInput]: Input is null or not set!" );
			return;
		}
		_input.OnFirePressed -= FireAction;
		_input.OnPositionChanged -= PositionChangeAction;
	}
}
