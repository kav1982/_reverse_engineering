using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class Access_T28Authoring : MonoBehaviour
{
	private class Baker : Baker<Access_T28Authoring>
	{
		public override void Bake(Access_T28Authoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Access_T28 component = new Access_T28
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
