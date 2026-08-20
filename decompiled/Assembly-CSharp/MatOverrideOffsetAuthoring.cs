using Unity.Entities;
using UnityEngine;

public class MatOverrideOffsetAuthoring : MonoBehaviour
{
	private class Baker : Baker<MatOverrideOffsetAuthoring>
	{
		public override void Bake(MatOverrideOffsetAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			MatOverrideOffset component = default(MatOverrideOffset);
			AddComponent(entity, in component);
		}
	}
}
