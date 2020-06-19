using System;
using UnityEngine;

internal class MouseInput : MonoBehaviour, IInput
{
	public event Action<Vector2> OnPositionChanged = default;
	public event Action OnFirePressed = default;

	[SerializeField] Transform _target = default;

	private Camera _camera = default;

	private void Awake ( )
	{
		_camera = Camera.main;
	}

	private void Update ( )
	{
		DetectPositionChange( );
		DetectFireInput( );
	}

	public void SetTargetTransform ( Transform target )
	{
		_target = target;
	}

	private void DetectPositionChange ( )
	{
		var mousePos = Input.mousePosition;
		mousePos.z = _camera.nearClipPlane;

		var worldPosition = _camera.ScreenToWorldPoint( mousePos );
		worldPosition.z = _target.position.z;

		OnPositionChanged?.Invoke( worldPosition );
	}

	private void DetectFireInput ( )
	{
		if ( Input.GetMouseButtonDown( 0 ) )
		{
			OnFirePressed?.Invoke( );
		}
	}

	public void SubscribeWeapon ( IWeapon weapon )
	{
		OnFirePressed += weapon.Fire;
		OnPositionChanged += weapon.SetDirection;
	}
	public void UnsubscribeWeapon ( IWeapon weapon )
	{
		OnFirePressed -= weapon.Fire;
		OnPositionChanged -= weapon.SetDirection;
	}
}