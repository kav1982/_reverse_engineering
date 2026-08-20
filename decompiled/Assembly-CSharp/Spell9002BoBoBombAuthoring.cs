using Unity.Entities;
using UnityEngine;

internal class Spell9002BoBoBombAuthoring : MonoBehaviour
{
	private class Spell9002BoBoBombAuthoringBaker : Baker<Spell9002BoBoBombAuthoring>
	{
		public override void Bake(Spell9002BoBoBombAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Spell9002BoBoBombData component = default(Spell9002BoBoBombData);
			AddComponent(entity, in component);
			IgnoreDynamicOptimizeTag component2 = default(IgnoreDynamicOptimizeTag);
			AddComponent(entity, in component2);
		}
	}
}
