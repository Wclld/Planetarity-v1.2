using System;
using UnityEngine;

[Serializable]
internal class MovementInfo
{
	public Vector3 CenterPos => _center? _center.position : Vector3.zero;
	public float StartProgress=> _startCycleProgress;
	public float CycleTime => _cycleTime;
	public float XRange => _xRange;
	public float YRange => _yRange;

	[SerializeField] Transform _center = default;
	[Range(0,1)]
	[SerializeField] float _startCycleProgress = 0;
	[SerializeField] float _cycleTime = 5;

	[SerializeField] float _xRange = 1;
	[SerializeField] float _yRange = 1;

}