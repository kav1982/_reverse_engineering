using Unity.Entities;
using UnityEngine;

internal class Spell1018ThunderAuraAuthoring : MonoBehaviour
{
	private class Spell1018ThunderAuraAuthoringBaker : Baker<Spell1018ThunderAuraAuthoring>
	{
		public override void Bake(Spell1018ThunderAuraAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Spell1018ThunderAuraData component = new Spell1018ThunderAuraData
			{
				FallDelayTimer = 0f
			};
			AddComponent(entity, in component);
			Spell1018ThunderAuraInitializeTag component2 = default(Spell1018ThunderAuraInitializeTag);
			AddComponent(entity, in component2);
		}
	}

	public float LightningChainMaxConductRangeRatio;
}
