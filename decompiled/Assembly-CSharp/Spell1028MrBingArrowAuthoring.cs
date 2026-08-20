using Unity.Entities;
using UnityEngine;

internal class Spell1028MrBingArrowAuthoring : MonoBehaviour
{
	private class Spell1028MrBingArrowAuthoringBaker : Baker<Spell1028MrBingArrowAuthoring>
	{
		public override void Bake(Spell1028MrBingArrowAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Spell1028MrBingArrowNeedInitTag component = default(Spell1028MrBingArrowNeedInitTag);
			AddComponent(entity, in component);
			SetComponentEnabled<Spell1028MrBingArrowNeedInitTag>(entity, enabled: true);
		}
	}
}
