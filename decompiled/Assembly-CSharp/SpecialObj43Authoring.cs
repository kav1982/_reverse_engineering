using Unity.Entities;
using UnityEngine;

public class SpecialObj43Authoring : MonoBehaviour
{
	private class Baker : Baker<SpecialObj43Authoring>
	{
		public override void Bake(SpecialObj43Authoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			SpecialObj43 component = new SpecialObj43
			{
				ett_WallLeft = GetEntity(authoring.ett_WallLeft, TransformUsageFlags.Dynamic),
				ett_AccessLeft = GetEntity(authoring.ett_AccessLeft, TransformUsageFlags.Dynamic),
				ett_ColliderMiddleLeft = GetEntity(authoring.ett_ColliderMiddleLeft, TransformUsageFlags.Dynamic),
				ett_WallRight = GetEntity(authoring.ett_WallRight, TransformUsageFlags.Dynamic),
				ett_AccessRight = GetEntity(authoring.ett_AccessRight, TransformUsageFlags.Dynamic),
				ett_ColliderMiddleRight = GetEntity(authoring.ett_ColliderMiddleRight, TransformUsageFlags.Dynamic)
			};
			AddComponent(entity, in component);
		}
	}

	public GameObject ett_WallLeft;

	public GameObject ett_AccessLeft;

	public GameObject ett_ColliderMiddleLeft;

	public GameObject ett_WallRight;

	public GameObject ett_AccessRight;

	public GameObject ett_ColliderMiddleRight;
}
