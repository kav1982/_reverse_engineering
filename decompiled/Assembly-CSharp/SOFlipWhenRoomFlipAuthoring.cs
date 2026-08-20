using Unity.Entities;
using UnityEngine;

public class SOFlipWhenRoomFlipAuthoring : MonoBehaviour
{
	private class Baker : Baker<SOFlipWhenRoomFlipAuthoring>
	{
		public override void Bake(SOFlipWhenRoomFlipAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			SOFlipWhenRoomFlip component = default(SOFlipWhenRoomFlip);
			AddComponent(entity, in component);
		}
	}
}
