using UnityEngine;

internal interface IMovement
{
	float CurrentProgress
	{
		get;
	}
	Vector3 UpdatePosition ( float timePassed );
	void Init ( MovementInfo info );
}