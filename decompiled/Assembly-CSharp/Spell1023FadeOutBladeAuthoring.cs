using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

internal class Spell1023FadeOutBladeAuthoring : MonoBehaviour
{
	private class Spell1023FadeOutBladeAuthoringBaker : Baker<Spell1023FadeOutBladeAuthoring>
	{
		public override void Bake(Spell1023FadeOutBladeAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Spell1023FadeOutBladeData component = new Spell1023FadeOutBladeData
			{
				Direction = default(float3),
				Timer = 0f,
				MoveSpeed = 0f
			};
			AddComponent(entity, in component);
		}
	}
}
