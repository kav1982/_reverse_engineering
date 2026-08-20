using Unity.Entities;
using UnityEngine;

internal class MatOverrideGhostEffectAuthoring : MonoBehaviour
{
	private class MatOverrideGhostEffectAuthoringBaker : Baker<MatOverrideGhostEffectAuthoring>
	{
		public override void Bake(MatOverrideGhostEffectAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			MatOverrideGhostEffect component = default(MatOverrideGhostEffect);
			AddComponent(entity, in component);
		}
	}
}
