using System;
using UnityEngine;

internal sealed class Planet : MonoBehaviour
{
	[SerializeField] MovementInfo _movementInfo;
	private IMovement _movement;
	private Vector3 _nextFramePosition;

	internal void InitMovement ( MovementType type )
	{
		switch ( type )
		{
			case MovementType.Circular:
				throw new NotImplementedException( );
				break;
			case MovementType.Eliptical:
				_movement = new ElipticalMovement( );
				_movement.Init( _movementInfo );
				break;
			default:
				throw new NotImplementedException( );
				break;
		}
	}

	private void Awake ( )
	{
		InitMovement( MovementType.Eliptical );
	}

	private void OnValidate ( )
	{
		InitMovement( MovementType.Eliptical );
		transform.position = _movement.UpdatePosition( Time.fixedDeltaTime );
	}

	private void Update ( )
	{
		transform.position = _nextFramePosition;
	}

	private void FixedUpdate ( )
	{
		_nextFramePosition = _movement.UpdatePosition( Time.fixedDeltaTime );
	}
}
