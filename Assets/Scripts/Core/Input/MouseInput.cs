using System;
using UnityEngine;

internal class MouseInput : MonoBehaviour, IInput
{
	public event Action<Vector2> OnPositionChanged = default;
	public event Action OnFirePressed = default;

	[SerializeField] Transform _planetes = default;

	private Camera _camera = default;

	private Vector3 _previousPosition = default;

	private void Awake ( )
	{
		_camera = Camera.main;
		InputManager.RegisterInput( this );
	}

	private void Update ( )
	{
		DetectPositionChange( );
		DetectFireInput( );
	}

	private void DetectPositionChange ( )
	{
		var mousePos = Input.mousePosition;
		mousePos.z = _camera.nearClipPlane;

		var worldPosition = _camera.ScreenToWorldPoint( mousePos );
		worldPosition.z = _planetes.position.z;

		if ( _previousPosition != worldPosition )
		{
			_previousPosition = worldPosition;
			OnPositionChanged?.Invoke( worldPosition );
		}
	}

	private void DetectFireInput ( )
	{
		if ( Input.GetMouseButtonDown( 0 ) )
		{
			OnFirePressed?.Invoke( );
		}
	}

	private void OnDrawGizmos ( )
	{
		Gizmos.color = Color.green;
		Gizmos.DrawSphere( _previousPosition, .2f );
	}
}