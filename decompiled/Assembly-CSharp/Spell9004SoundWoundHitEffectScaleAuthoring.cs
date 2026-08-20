using Unity.Entities;
using UnityEngine;

public class Spell9004SoundWoundHitEffectScaleAuthoring : MonoBehaviour
{
	private class Spell9004SoundWoundHitEffectScaleAuthoringBaker : Baker<Spell9004SoundWoundHitEffectScaleAuthoring>
	{
		public override void Bake(Spell9004SoundWoundHitEffectScaleAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Spell9004SoundWoundHitEffectScaleData component = default(Spell9004SoundWoundHitEffectScaleData);
			AddComponent(entity, in component);
		}
	}
}
