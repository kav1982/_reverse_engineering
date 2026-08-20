using Unity.Entities;
using UnityEngine;

public class SpecialObj44Authoring : MonoBehaviour
{
	private class Baker : Baker<SpecialObj44Authoring>
	{
		public override void Bake(SpecialObj44Authoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			SpecialObj44 component = new SpecialObj44
			{
				ett_LeftWall = GetEntity(authoring.ett_LeftWall, TransformUsageFlags.Dynamic),
				ett_LeftAccess = GetEntity(authoring.ett_LeftAccess, TransformUsageFlags.Dynamic),
				ett_LeftColliderMiddle = GetEntity(authoring.ett_LeftColliderMiddle, TransformUsageFlags.Dynamic),
				ett_RightWall = GetEntity(authoring.ett_RightWall, TransformUsageFlags.Dynamic),
				ett_RightAccess = GetEntity(authoring.ett_RightAccess, TransformUsageFlags.Dynamic),
				ett_RightColliderMiddle = GetEntity(authoring.ett_RightColliderMiddle, TransformUsageFlags.Dynamic)
			};
			AddComponent(entity, in component);
		}
	}

	public GameObject ett_LeftWall;

	public GameObject ett_LeftAccess;

	public GameObject ett_LeftColliderMiddle;

	public GameObject ett_RightWall;

	public GameObject ett_RightAccess;

	public GameObject ett_RightColliderMiddle;
}
