using Unity.Entities;
using UnityEngine;

public class Spell4013HitTriggerAuthoring : MonoBehaviour
{
	private class Spell4013HitTriggerAuthoringBaker : Baker<Spell4013HitTriggerAuthoring>
	{
		public override void Bake(Spell4013HitTriggerAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Spell4013HitTriggerData component = default(Spell4013HitTriggerData);
			AddComponent(entity, in component);
			IgnoreSpellHitTag component2 = default(IgnoreSpellHitTag);
			AddComponent(entity, in component2);
		}
	}
}
