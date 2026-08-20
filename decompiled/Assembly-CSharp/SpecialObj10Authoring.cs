using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class SpecialObj10Authoring : MonoBehaviour
{
	private class Baker : Baker<SpecialObj10Authoring>
	{
		public override void Bake(SpecialObj10Authoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			SpecialObj10_Dots component = new SpecialObj10_Dots
			{
				ett_Normal = GetEntity(authoring.ett_Normal, TransformUsageFlags.Dynamic),
				ett_Used = GetEntity(authoring.ett_Used, TransformUsageFlags.Dynamic),
				maxUseTime = authoring.maxUseTime,
				costs = DTool.ArrayToBlobArray(authoring.costs),
				brokenEFCount = authoring.brokenEFCount,
				brokenEFOffset = authoring.brokenEFOffset,
				brokenEFRadius = authoring.brokenEFRadius,
				discountRatio = 1f
			};
			AddComponent(entity, in component);
		}
	}

	public GameObject ett_Normal;

	public GameObject ett_Used;

	public int maxUseTime;

	public int[] costs;

	public int brokenEFCount;

	public float3 brokenEFOffset;

	public float brokenEFRadius;
}
