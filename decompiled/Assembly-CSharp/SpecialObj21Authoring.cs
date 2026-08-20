using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class SpecialObj21Authoring : MonoBehaviour
{
	private class Baker : Baker<SpecialObj21Authoring>
	{
		public override void Bake(SpecialObj21Authoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			SpecialObj21_Dots component = new SpecialObj21_Dots
			{
				ett_Normal = GetEntity(authoring.ett_Normal, TransformUsageFlags.Dynamic),
				ett_Used = GetEntity(authoring.ett_Used, TransformUsageFlags.Dynamic),
				ett_Anima = GetEntity(authoring.ett_Anima, TransformUsageFlags.Dynamic),
				fixedUsage = authoring.fixedUsage,
				brokenChance = DTool.ArrayToBlobArray(authoring.brokenChance),
				brokenEFCenter = authoring.brokenEFCenter,
				brokenEFOffset = authoring.brokenEFOffset
			};
			AddComponent(entity, in component);
		}
	}

	public GameObject ett_Normal;

	public GameObject ett_Used;

	public GameObject ett_Anima;

	public int fixedUsage;

	public float[] brokenChance;

	public float3 brokenEFCenter;

	public float3 brokenEFOffset;
}
