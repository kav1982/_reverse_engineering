using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

internal class Spell1018ChainAuthoring : MonoBehaviour
{
	private class Spell1018ChainAuthoringBaker : Baker<Spell1018ChainAuthoring>
	{
		public override void Bake(Spell1018ChainAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Spell1018ChainData component = new Spell1018ChainData
			{
				IsInitialized = false,
				Position1 = default(float3),
				Position2 = default(float3),
				duration = 0.3f
			};
			AddComponent(entity, in component);
		}
	}
}
