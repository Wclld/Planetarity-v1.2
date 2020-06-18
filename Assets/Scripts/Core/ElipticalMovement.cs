using UnityEngine;

internal sealed class ElipticalMovement : IMovement
{
	private MovementInfo _info;
	private float _currentProgress;
	private float _rotationSpeed;
	private Vector2 _defaultOffset;

	public void Init ( MovementInfo info )
	{
		_info = info;

		_currentProgress = info.StartProgress;
		_rotationSpeed = 1 / info.CycleTime;
		_defaultOffset = info.CenterPos;
	}

	public Vector3 UpdatePosition ( float timePassed )
	{
		_currentProgress += timePassed * _rotationSpeed;

		var angle = _currentProgress * 360 * Mathf.Deg2Rad;

		var xPos = Mathf.Sin(angle) * _info.XRange;
		var yPos = Mathf.Cos(angle) * _info.YRange;

		return new Vector2(xPos,yPos) + _defaultOffset;
	}
}