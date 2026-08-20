using Unity.Entities;
using UnityEngine;

public class Spell4024DaveHarpoonThunderRelicEffectAuthoring : MonoBehaviour
{
	public class Spell4024DaveHarpoonThunderRelicEffectBaker : Baker<Spell4024DaveHarpoonThunderRelicEffectAuthoring>
	{
		public override void Bake(Spell4024DaveHarpoonThunderRelicEffectAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			AddComponent<Spell4024DaveHarpoonThunderRelicEffectData>(entity);
		}
	}
}
