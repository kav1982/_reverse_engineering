using Unity.Entities;
using UnityEngine;

internal class Spell1003ButterflyAuthoring : MonoBehaviour
{
	private class Spell1003ButterflyAuthoringBaker : Baker<Spell1003ButterflyAuthoring>
	{
		public override void Bake(Spell1003ButterflyAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Spell1003ButterflyData component = new Spell1003ButterflyData
			{
				InitialSpeed = 0f,
				IsInitialize = false,
				StartTraceTargets = false
			};
			AddComponent(entity, in component);
			Spell1003ButterflyBeAttackedTag component2 = default(Spell1003ButterflyBeAttackedTag);
			AddComponent(entity, in component2);
			SetComponentEnabled<Spell1003ButterflyBeAttackedTag>(entity, enabled: false);
		}
	}
}
