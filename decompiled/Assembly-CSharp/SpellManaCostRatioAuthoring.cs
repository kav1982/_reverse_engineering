using Unity.Entities;
using UnityEngine;

public class SpellManaCostRatioAuthoring : MonoBehaviour
{
	private class SpellManaCostRatioAuthoringBaker : Baker<SpellManaCostRatioAuthoring>
	{
		public override void Bake(SpellManaCostRatioAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			ManaCostRatio component = new ManaCostRatio
			{
				ratio = 0f
			};
			AddComponent(entity, in component);
		}
	}
}
