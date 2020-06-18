using UnityEngine;

internal interface IMovement
{
	Vector3 UpdatePosition ( float timePassed );
	void Init ( MovementInfo info );
}