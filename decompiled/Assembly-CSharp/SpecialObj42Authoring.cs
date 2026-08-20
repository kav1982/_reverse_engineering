using Unity.Entities;
using UnityEngine;

public class SpecialObj42Authoring : MonoBehaviour
{
	private class Baker : Baker<SpecialObj42Authoring>
	{
		public override void Bake(SpecialObj42Authoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			SpecialObj42 component = new SpecialObj42
			{
				ett_WallLeft = GetEntity(authoring.ett_WallLeft, TransformUsageFlags.Dynamic),
				ett_AccessLeft = GetEntity(authoring.ett_AccessLeft, TransformUsageFlags.Dynamic),
				ett_ColliderLeftMiddle = GetEntity(authoring.ett_ColliderLeftMiddle, TransformUsageFlags.Dynamic)
			};
			AddComponent(entity, in component);
		}
	}

	public GameObject ett_WallLeft;

	public GameObject ett_AccessLeft;

	public GameObject ett_ColliderLeftMiddle;
}
