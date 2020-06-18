using System;
using UnityEngine;

[Serializable]
internal class MovementInfo
{
	internal Vector3 CenterPos => _center? _center.position : Vector3.zero;
	internal float StartProgress=> _startCycleProgress;
	internal float CycleTime => _cycleTime;
	internal float XRange => _xRange;
	internal float YRange => _yRange;

	[SerializeField] Transform _center = default;
	[Range(0,1)]
	[SerializeField] float _startCycleProgress = 0;
	[SerializeField] float _cycleTime = 5;

	[SerializeField] float _xRange = 1;
	[SerializeField] float _yRange = 1;

}