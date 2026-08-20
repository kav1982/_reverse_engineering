using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

internal class Spell1003BodyTagAuthoring : MonoBehaviour
{
	private class Spell1003BodyTagAuthoringBaker : Baker<Spell1003BodyTagAuthoring>
	{
		public override void Bake(Spell1003BodyTagAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Spell1003BodyTag component = new Spell1003BodyTag
			{
				lastFramePos = float3.zero
			};
			AddComponent(entity, in component);
		}
	}
}
