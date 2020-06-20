using System;

[Serializable]
public class PlanetInfo
{
	public MovementInfo MovementInfo = default;
	public MovementType MovementType = default;
	public InputType InputType = default;
	public int WeaponIndex = default;
	public int HealthPoints = default;
	public int PrefabIndex = default;
}
