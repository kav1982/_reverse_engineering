using UnityEngine;

public class ShootSpellSpatialInfo
{
	public Vector3 Start { get; set; }

	public Vector3 Direction { get; set; }

	public Vector3? Target { get; set; }

	private ShootSpellSpatialInfo()
	{
	}

	public ShootSpellSpatialInfo Copy()
	{
		return new ShootSpellSpatialInfo
		{
			Start = Start,
			Target = Target,
			Direction = Direction
		};
	}

	public static ShootSpellSpatialInfo ToPoint(Vector3 start, Vector3 target)
	{
		return new ShootSpellSpatialInfo
		{
			Start = start,
			Target = target,
			Direction = (target - start).normalized
		};
	}

	public static ShootSpellSpatialInfo ToPoint(Vector3 start, Vector3 target, Vector3 direction)
	{
		return new ShootSpellSpatialInfo
		{
			Start = start,
			Target = target,
			Direction = direction.normalized
		};
	}

	public static ShootSpellSpatialInfo ToStartPointButWithDirection(Vector3 start, Vector3 direction)
	{
		return new ShootSpellSpatialInfo
		{
			Start = start,
			Target = start,
			Direction = direction
		};
	}

	public static ShootSpellSpatialInfo ByDirection(Vector3 start, Vector3 direction)
	{
		return new ShootSpellSpatialInfo
		{
			Start = start,
			Direction = direction.normalized
		};
	}

	public override string ToString()
	{
		if (Target.HasValue)
		{
			return $"<ShootSpellSpatialInfo From {Start} To {Target.Value}, Dir = {Direction}>";
		}
		return $"<ShootSpellSpatialInfo From {Start}, Dir = {Direction}>";
	}
}
