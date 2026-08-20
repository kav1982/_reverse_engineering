using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class Access_T29Authoring : MonoBehaviour
{
	private class Baker : Baker<Access_T29Authoring>
	{
		public override void Bake(Access_T29Authoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Access_T29 component = new Access_T29
			{
				torch1Offset = authoring.torch1Offset,
				torch2Offset = authoring.torch2Offset
			};
			AddComponent(entity, in component);
		}
	}

	public float3 torch1Offset;

	public float3 torch2Offset;
}
