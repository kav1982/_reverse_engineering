using Unity.Entities;
using UnityEngine;

public class Spell2007SuicideBugAuthoring : MonoBehaviour
{
	private class Spell2007Baker : Baker<Spell2007SuicideBugAuthoring>
	{
		public override void Bake(Spell2007SuicideBugAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			AddComponent<Spell2007SuicideBugData>(entity);
			AddComponent<SpellDestroyTag>(entity);
			SetComponentEnabled<SpellDestroyTag>(entity, enabled: false);
			AddComponent<Spell2007SuicideBugInitializeTag>(entity);
			SetComponentEnabled<Spell2007SuicideBugInitializeTag>(entity, enabled: true);
		}
	}
}
