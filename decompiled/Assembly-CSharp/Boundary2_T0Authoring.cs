using Unity.Entities;
using UnityEngine;

public class Boundary2_T0Authoring : MonoBehaviour
{
	private class Baker : Baker<Boundary2_T0Authoring>
	{
		public override void Bake(Boundary2_T0Authoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Boundary2_T0 component = default(Boundary2_T0);
			component.iconChainChance = authoring.iconChainChance;
			component.ironChainPerMeter = authoring.ironChainPerMeter;
			component.offset = authoring.offset;
			component.ettIronChain = GetEntity(authoring.ettIronChain, TransformUsageFlags.Dynamic);
			AddComponent(entity, in component);
		}
	}

	[Range(0f, 1f)]
	public float iconChainChance;

	public int ironChainPerMeter;

	public float offset;

	public GameObject ettIronChain;
}
