using Unity.Entities;
using UnityEngine;

internal class SpellSpeedRatioValueAuthoring : MonoBehaviour
{
	private class SpellSpeedRatioValueAuthoringBaker : Baker<SpellSpeedRatioValueAuthoring>
	{
		public override void Bake(SpellSpeedRatioValueAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			AddComponent<SpellSpeedRatioValueData>(entity);
		}
	}
}
