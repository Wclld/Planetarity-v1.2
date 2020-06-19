using System;
using UnityEngine;

[Serializable]
public class MovementInfo
{
	internal Vector3 CenterPos => _center? _center.position : Vector3.zero;
	internal float StartProgress => _startProgress;
	internal float CurrentProgress => _currentProgress;
	internal float CycleTime => _cycleTime;
	internal float XRange => _xRange;
	internal float YRange => _yRange;

	[SerializeField] Transform _center = default;
	[Range(0,1)]
	[SerializeField] float _startProgress = 0;
	[SerializeField] float _cycleTime = 5;
	[SerializeField] float _xRange = 1;
	[SerializeField] float _yRange = 1;
	[SerializeField] float _currentProgress = 0;

	public MovementInfo ( Transform center, Vector2 circulationRange, float cycleTime, float startProgress )
	{
		_center = center;
		_xRange = circulationRange.x;
		_yRange = circulationRange.y;
		_cycleTime = cycleTime;
		_startProgress = startProgress;
		_currentProgress = startProgress;
	}

	public void SetProgress ( float newProgress )
	{
		_currentProgress = newProgress;
	}
}