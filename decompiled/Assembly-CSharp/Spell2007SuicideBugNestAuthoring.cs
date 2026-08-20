using Unity.Entities;
using UnityEngine;

public class Spell2007SuicideBugNestAuthoring : MonoBehaviour
{
	private class Spell2007Baker : Baker<Spell2007SuicideBugNestAuthoring>
	{
		public override void Bake(Spell2007SuicideBugNestAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			AddComponent<Spell2007SuicideBugNestData>(entity);
			AddBuffer<Spell2007FuseBuffer>(entity);
			AddComponent<Spell2007SuicideBugNestInitializedTag>(entity);
		}
	}
}
