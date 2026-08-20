using Unity.Entities;
using UnityEngine;

public class MatOverrideFrameIndexAuthoring : MonoBehaviour
{
	private class Baker : Baker<MatOverrideFrameIndexAuthoring>
	{
		public override void Bake(MatOverrideFrameIndexAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			MatOverrideFrameIndex component = default(MatOverrideFrameIndex);
			AddComponent(entity, in component);
		}
	}
}
