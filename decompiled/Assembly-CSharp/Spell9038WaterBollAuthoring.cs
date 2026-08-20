using Unity.Entities;
using UnityEngine;

public class Spell9038WaterBollAuthoring : MonoBehaviour
{
	private class Spell9038WaterBollAuthoringBaker : Baker<Spell9038WaterBollAuthoring>
	{
		public override void Bake(Spell9038WaterBollAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Spell9038WaterBollData component = default(Spell9038WaterBollData);
			AddComponent(entity, in component);
		}
	}
}
