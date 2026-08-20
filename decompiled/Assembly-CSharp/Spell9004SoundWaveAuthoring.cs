using Unity.Entities;
using UnityEngine;

public class Spell9004SoundWaveAuthoring : MonoBehaviour
{
	private class Spell9004SoundWaveAuthoringBaker : Baker<Spell9004SoundWaveAuthoring>
	{
		public override void Bake(Spell9004SoundWaveAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Spell9004SoundWaveData component = new Spell9004SoundWaveData
			{
				InitOver = false
			};
			AddComponent(entity, in component);
		}
	}
}
