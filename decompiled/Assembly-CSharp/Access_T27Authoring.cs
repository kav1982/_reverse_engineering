using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class Access_T27Authoring : MonoBehaviour
{
	private class Baker : Baker<Access_T27Authoring>
	{
		public override void Bake(Access_T27Authoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Access_T27 component = new Access_T27
			{
				torchPos = authoring.torchPos,
				torch2Pos = authoring.torchPos2
			};
			AddComponent(entity, in component);
		}
	}

	public float3 torchPos;

	public float3 torchPos2;
}
