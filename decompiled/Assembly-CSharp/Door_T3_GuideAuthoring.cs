using Unity.Entities;
using UnityEngine;

public class Door_T3_GuideAuthoring : MonoBehaviour
{
	private class Baker : Baker<Door_T3_GuideAuthoring>
	{
		public override void Bake(Door_T3_GuideAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Door_T3_Guide component = default(Door_T3_Guide);
			AddComponent(entity, in component);
		}
	}
}
