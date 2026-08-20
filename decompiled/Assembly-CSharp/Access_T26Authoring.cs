using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class Access_T26Authoring : MonoBehaviour
{
	private class Baker : Baker<Access_T26Authoring>
	{
		public override void Bake(Access_T26Authoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Access_T26 component = new Access_T26
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
