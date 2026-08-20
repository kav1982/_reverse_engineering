using Unity.Entities;
using Unity.Physics;
using UnityEngine;

public class PlayerControllerAuthoring : MonoBehaviour
{
	private class Baker : Baker<PlayerControllerAuthoring>
	{
		public override void Bake(PlayerControllerAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			PlayerController_Dots component = new PlayerController_Dots
			{
				interactiveRadius = authoring.interactiveRadius
			};
			AddComponent(entity, in component);
			AddBuffer<UnitKeepCastingSpells>(entity);
			AddComponent<PhysicsMassOverride>(entity);
		}
	}

	public float interactiveRadius;
}
