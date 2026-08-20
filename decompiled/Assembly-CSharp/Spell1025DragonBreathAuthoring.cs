using Unity.Entities;
using UnityEngine;

public class Spell1025DragonBreathAuthoring : MonoBehaviour
{
	public class Spell1025Baker : Baker<Spell1025DragonBreathAuthoring>
	{
		public override void Bake(Spell1025DragonBreathAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Spell1025DragonBreathData component = default(Spell1025DragonBreathData);
			AddComponent(entity, in component);
			Spell1025CreatEffectTag component2 = default(Spell1025CreatEffectTag);
			AddComponent(entity, in component2);
			AddBuffer<Spell1025FireLinePointsBuffer>(entity);
			AddBuffer<Spell1025DragonBreathFireLinePointBuffer>(entity);
			AddBuffer<Spell1025FireGroundEffectBuffer>(entity);
			AddComponent<Spell1025InitializedTag>(entity);
		}
	}
}
