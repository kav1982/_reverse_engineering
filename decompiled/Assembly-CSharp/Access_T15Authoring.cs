using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class Access_T15Authoring : MonoBehaviour
{
	private class Baker : Baker<Access_T15Authoring>
	{
		public override void Bake(Access_T15Authoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Access_T15_Dots component = new Access_T15_Dots
			{
				ett_Layer = GetEntity(authoring.ett_Layer, TransformUsageFlags.Dynamic),
				ett_Offset = GetEntity(authoring.ett_Offset, TransformUsageFlags.Dynamic),
				ett_OffsetH = GetEntity(authoring.ett_OffsetH, TransformUsageFlags.Dynamic),
				torch1Offset = authoring.torch1Offset,
				torch2Offset = authoring.torch2Offset,
				openFinalYOffset = authoring.openFinalYOffset,
				openYOffsetSpeed = authoring.openYOffsetSpeed
			};
			AddComponent(entity, in component);
		}
	}

	public GameObject ett_Layer;

	public GameObject ett_Offset;

	public GameObject ett_OffsetH;

	public float3 torch1Offset;

	public float3 torch2Offset;

	public float openFinalYOffset;

	public float openYOffsetSpeed;
}
