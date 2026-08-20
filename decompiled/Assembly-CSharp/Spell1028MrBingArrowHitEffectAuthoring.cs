using Unity.Entities;
using UnityEngine;

internal class Spell1028MrBingArrowHitEffectAuthoring : MonoBehaviour
{
	private class Spell1028MrBingArrowHitEffectAuthoringBaker : Baker<Spell1028MrBingArrowHitEffectAuthoring>
	{
		public override void Bake(Spell1028MrBingArrowHitEffectAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Spell1028MrBingArrowHitEffectData component = new Spell1028MrBingArrowHitEffectData
			{
				initialized = false
			};
			AddComponent(entity, in component);
		}
	}
}
