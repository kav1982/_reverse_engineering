using Unity.Entities;
using UnityEngine;

public class Spell9008SinWaveSpeedAuthoring : MonoBehaviour
{
	private class Spell9008SinWaveSpeedAuthoringBaker : Baker<Spell9008SinWaveSpeedAuthoring>
	{
		public override void Bake(Spell9008SinWaveSpeedAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Spell9008SinWaveSpeedData component = new Spell9008SinWaveSpeedData
			{
				Initialized = false
			};
			AddComponent(entity, in component);
		}
	}
}
