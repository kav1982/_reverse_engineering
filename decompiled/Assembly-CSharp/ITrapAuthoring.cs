using Unity.Entities;
using UnityEngine;

public class ITrapAuthoring : MonoBehaviour
{
	private class Baker : Baker<ITrapAuthoring>
	{
		public override void Bake(ITrapAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			ITrap_Dots component = default(ITrap_Dots);
			AddComponent(entity, in component);
		}
	}
}
