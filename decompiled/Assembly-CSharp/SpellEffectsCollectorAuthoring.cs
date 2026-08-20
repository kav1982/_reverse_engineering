using Unity.Entities;
using UnityEngine;

internal class SpellEffectsCollectorAuthoring : MonoBehaviour
{
	private class SpellEffectsCollectorAuthoringBaker : Baker<SpellEffectsCollectorAuthoring>
	{
		public override void Bake(SpellEffectsCollectorAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			EffectsCollectorData component = new EffectsCollectorData
			{
				Effect1 = GetEntity(authoring.Effect1, TransformUsageFlags.Dynamic),
				Effect2 = GetEntity(authoring.Effect2, TransformUsageFlags.Dynamic),
				Effect3 = GetEntity(authoring.Effect3, TransformUsageFlags.Dynamic),
				Effect4 = GetEntity(authoring.Effect4, TransformUsageFlags.Dynamic),
				Effect5 = GetEntity(authoring.Effect5, TransformUsageFlags.Dynamic)
			};
			AddComponent(entity, in component);
		}
	}

	public GameObject Effect1;

	public GameObject Effect2;

	public GameObject Effect3;

	public GameObject Effect4;

	public GameObject Effect5;
}
