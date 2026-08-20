using Unity.Entities;
using UnityEngine;

internal class MatOverrideTeammateRepeatCounterAuthoring : MonoBehaviour
{
	private class MatOverrideTilingXAuthoringBaker : Baker<MatOverrideTeammateRepeatCounterAuthoring>
	{
		public override void Bake(MatOverrideTeammateRepeatCounterAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			MatOverrideRepeatCounter component = default(MatOverrideRepeatCounter);
			AddComponent(entity, in component);
		}
	}
}
