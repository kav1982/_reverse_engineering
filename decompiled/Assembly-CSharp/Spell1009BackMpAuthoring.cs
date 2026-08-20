using Unity.Entities;
using UnityEngine;

public class Spell1009BackMpAuthoring : MonoBehaviour
{
	private class Spell1009BackMpAuthoringBaker : Baker<Spell1009BackMpAuthoring>
	{
		public override void Bake(Spell1009BackMpAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Spell1009BackMpData component = default(Spell1009BackMpData);
			AddComponent(entity, in component);
			Spell1009BackMpInitializeTag component2 = default(Spell1009BackMpInitializeTag);
			AddComponent(entity, in component2);
		}
	}
}
