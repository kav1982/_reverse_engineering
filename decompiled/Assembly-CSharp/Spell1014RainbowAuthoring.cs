using Unity.Entities;
using UnityEngine;

internal class Spell1014RainbowAuthoring : MonoBehaviour
{
	private class Spell1014RainbowAuthoringBaker : Baker<Spell1014RainbowAuthoring>
	{
		public override void Bake(Spell1014RainbowAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			AddComponent<Spell1014Data>(entity);
		}
	}
}
