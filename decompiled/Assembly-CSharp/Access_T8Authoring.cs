using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class Access_T8Authoring : MonoBehaviour
{
	private class Baker : Baker<Access_T8Authoring>
	{
		public override void Bake(Access_T8Authoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Access_T8_Dots component = new Access_T8_Dots
			{
				ett_Layer = GetEntity(authoring.ett_Layer, TransformUsageFlags.Dynamic),
				ett_Access = GetEntity(authoring.ett_Access, TransformUsageFlags.Dynamic),
				ett_AccessNotNeedKey = GetEntity(authoring.ett_AccessNotNeedKey, TransformUsageFlags.Dynamic),
				ett_Offset = GetEntity(authoring.ett_Offset, TransformUsageFlags.Dynamic),
				ett_OffsetNotNeedKey = GetEntity(authoring.ett_OffsetNotNeedKey, TransformUsageFlags.Dynamic),
				ett_AccessLight = GetEntity(authoring.ett_AccessLight, TransformUsageFlags.Dynamic),
				ett_AccessLightNotNeedKey = GetEntity(authoring.ett_AccessLightNotNeedKey, TransformUsageFlags.Dynamic),
				torch1Offset = authoring.torch1Offset,
				torch2Offset = authoring.torch2Offset,
				openFinalYOffset = authoring.openFinalYOffset,
				openYOffsetSpeed = authoring.openYOffsetSpeed
			};
			AddComponent(entity, in component);
		}
	}

	public GameObject ett_Layer;

	public GameObject ett_Access;

	public GameObject ett_AccessNotNeedKey;

	public GameObject ett_Offset;

	public GameObject ett_OffsetNotNeedKey;

	public GameObject ett_AccessLight;

	public GameObject ett_AccessLightNotNeedKey;

	public float3 torch1Offset;

	public float3 torch2Offset;

	public float openFinalYOffset;

	public float openYOffsetSpeed;
}
