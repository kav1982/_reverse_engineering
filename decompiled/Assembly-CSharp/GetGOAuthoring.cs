using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class GetGOAuthoring : MonoBehaviour
{
	private class Baker : Baker<GetGOAuthoring>
	{
		public override void Bake(GetGOAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			GetGO component = new GetGO
			{
				path = authoring.path,
				offset = authoring.offset
			};
			AddComponent(entity, in component);
		}
	}

	public string path;

	public float3 offset;
}
