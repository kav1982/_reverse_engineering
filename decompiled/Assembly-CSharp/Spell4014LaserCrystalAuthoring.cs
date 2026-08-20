using Unity.Entities;
using UnityEngine;

public class Spell4014LaserCrystalAuthoring : MonoBehaviour
{
	public class Spell4014LaserCrystalBaker : Baker<Spell4014LaserCrystalAuthoring>
	{
		public override void Bake(Spell4014LaserCrystalAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			AddComponent<Spell4014LaserCrystalData>(entity);
			AddBuffer<CrystalLaserPoint>(entity);
			AddBuffer<HittedEntity>(entity);
		}
	}
}
