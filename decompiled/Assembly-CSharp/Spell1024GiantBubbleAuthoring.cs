using Unity.Entities;
using UnityEngine;

public class Spell1024GiantBubbleAuthoring : MonoBehaviour
{
	private class Spell1024GiantBubbleAuthoringBaker : Baker<Spell1024GiantBubbleAuthoring>
	{
		public override void Bake(Spell1024GiantBubbleAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Spell1024GiantBubbleData component = new Spell1024GiantBubbleData
			{
				IsInit = true,
				ChargeCollisionRange = authoring.scaleRange,
				IsCollapse = false,
				EffectRangeInitScale = authoring.effectRangeInitScale,
				EffectSpellInitScale = authoring.effectSpellInitScale
			};
			AddComponent(entity, in component);
		}
	}

	public float scaleRange;

	public float effectRangeInitScale;

	public float effectSpellInitScale;
}
