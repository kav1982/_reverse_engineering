using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class Access_T0Authoring : MonoBehaviour
{
	private class Baker : Baker<Access_T0Authoring>
	{
		public override void Bake(Access_T0Authoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Access_T0_Dots component = new Access_T0_Dots
			{
				ett_Access = GetEntity(authoring.ett_Access, TransformUsageFlags.Dynamic),
				ett_AccessNotNeedKey = GetEntity(authoring.ett_AccessNotNeedKey, TransformUsageFlags.Dynamic),
				torch1Offst = authoring.torch1Offst,
				torch2Offst = authoring.torch2Offst
			};
			AddComponent(entity, in component);
		}
	}

	public GameObject ett_Access;

	public GameObject ett_AccessNotNeedKey;

	public float3 torch1Offst;

	public float3 torch2Offst;
}
