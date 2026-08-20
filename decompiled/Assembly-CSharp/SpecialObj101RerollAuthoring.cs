using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class SpecialObj101RerollAuthoring : MonoBehaviour
{
	private class Baker : Baker<SpecialObj101RerollAuthoring>
	{
		public override void Bake(SpecialObj101RerollAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			SpecialObj101Reroll_Dots component = default(SpecialObj101Reroll_Dots);
			component.ett_CarpetLayer = GetEntity(authoring.ett_LayerCarpet, TransformUsageFlags.Dynamic);
			component.ett_Anima = GetEntity(authoring.ett_Anima, TransformUsageFlags.Dynamic);
			using (BlobBuilder blobBuilder = new BlobBuilder(Allocator.Temp))
			{
				BlobBuilderArray<float> blobBuilderArray = blobBuilder.Allocate(ref blobBuilder.ConstructRoot<BlobArray<float>>(), authoring.brokenChance.Length);
				for (int i = 0; i < authoring.brokenChance.Length; i++)
				{
					blobBuilderArray[i] = authoring.brokenChance[i];
				}
				component.brokenChance = blobBuilder.CreateBlobAssetReference<BlobArray<float>>(Allocator.Persistent);
			}
			component.fixedUsage = authoring.fixedUsage;
			component.brokenEFCenter = authoring.brokenEFCenter;
			component.brokenEFOffset = authoring.brokenEFOffset;
			AddComponent(entity, in component);
		}
	}

	public GameObject ett_LayerCarpet;

	public GameObject ett_Anima;

	public int fixedUsage;

	[Header("Broken")]
	public float[] brokenChance;

	public float3 brokenEFCenter;

	public float3 brokenEFOffset;
}
