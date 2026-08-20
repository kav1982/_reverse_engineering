using Unity.Entities;
using UnityEngine;

public class Spell1029DimensionTravellerAuthoring : MonoBehaviour
{
	public class Baker : Baker<Spell1029DimensionTravellerAuthoring>
	{
		public override void Bake(Spell1029DimensionTravellerAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Spell1029DimensionTravellerData component = default(Spell1029DimensionTravellerData);
			AddComponent(entity, in component);
			Spell1029InitializedTag component2 = default(Spell1029InitializedTag);
			AddComponent(entity, in component2);
		}
	}
}
