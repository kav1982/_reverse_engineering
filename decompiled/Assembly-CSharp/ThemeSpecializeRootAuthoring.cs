using Unity.Entities;
using UnityEngine;

public class ThemeSpecializeRootAuthoring : MonoBehaviour
{
	private class Baker : Baker<ThemeSpecializeRootAuthoring>
	{
		public override void Bake(ThemeSpecializeRootAuthoring rootAuthoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			ThemeSpecialize_Dots component = new ThemeSpecialize_Dots
			{
				ett_RoomSpecializeBase1 = GetEntity(rootAuthoring.ett_RoomSpecializeBase1, TransformUsageFlags.Dynamic),
				ett_RoomSpecializeBase2 = GetEntity(rootAuthoring.ett_RoomSpecializeBase2, TransformUsageFlags.Dynamic),
				ett_RoomSpecializeBase3 = GetEntity(rootAuthoring.ett_RoomSpecializeBase3, TransformUsageFlags.Dynamic)
			};
			AddComponent(entity, in component);
		}
	}

	public GameObject ett_RoomSpecializeBase1;

	public GameObject ett_RoomSpecializeBase2;

	public GameObject ett_RoomSpecializeBase3;
}
