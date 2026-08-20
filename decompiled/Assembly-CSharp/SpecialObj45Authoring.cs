using Unity.Entities;
using UnityEngine;

public class SpecialObj45Authoring : MonoBehaviour
{
	private class Baker : Baker<SpecialObj45Authoring>
	{
		public override void Bake(SpecialObj45Authoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			SpecialObj45 component = new SpecialObj45
			{
				ett_WallRight = GetEntity(authoring.ett_WallRight, TransformUsageFlags.Dynamic),
				ett_AccessRight = GetEntity(authoring.ett_AccessRight, TransformUsageFlags.Dynamic)
			};
			AddComponent(entity, in component);
		}
	}

	public GameObject ett_WallRight;

	public GameObject ett_AccessRight;
}
