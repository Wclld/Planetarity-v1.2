using UnityEngine;

public class Rocket: MonoBehaviour
{
	public WeaponInfo Info => _info;

	[SerializeField] WeaponInfo _info;

	private void OnTriggerEnter ( Collider other )
	{
		
	}
}