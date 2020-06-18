using UnityEngine;

internal class Attractor: MonoBehaviour
{
	public float Mass => _mass;

	public Vector3 CenterOfMass => transform.position;
	

	[SerializeField] private float _mass = default;


	private void Awake ( )
	{
		AttractorsManager.RegisterAttractor( this );
	}
}