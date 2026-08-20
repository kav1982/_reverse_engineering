using Unity.Entities;
using UnityEngine;

public class SpecialObj45BloodRoomAuthoring : MonoBehaviour
{
	private class Baker : Baker<SpecialObj45BloodRoomAuthoring>
	{
		public override void Bake(SpecialObj45BloodRoomAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			SpecialObj45BloodRoom component = new SpecialObj45BloodRoom
			{
				ett_Wall = GetEntity(authoring.ett_Wall, TransformUsageFlags.Dynamic)
			};
			AddComponent(entity, in component);
		}
	}

	public GameObject ett_Wall;
}
