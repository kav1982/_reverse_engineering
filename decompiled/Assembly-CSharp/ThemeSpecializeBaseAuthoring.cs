using Unity.Entities;
using UnityEngine;

public class ThemeSpecializeBaseAuthoring : MonoBehaviour
{
	private class Baker : Baker<ThemeSpecializeBaseAuthoring>
	{
		public override void Bake(ThemeSpecializeBaseAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			ThemeSpecializeBase component = default(ThemeSpecializeBase);
			AddComponent(entity, in component);
		}
	}
}
