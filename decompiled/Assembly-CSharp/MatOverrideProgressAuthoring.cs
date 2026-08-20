using Unity.Entities;
using UnityEngine;

internal class MatOverrideProgressAuthoring : MonoBehaviour
{
	private class MatOverrideProgressAuthoringBaker : Baker<MatOverrideProgressAuthoring>
	{
		public override void Bake(MatOverrideProgressAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			MatOverrideProgress component = default(MatOverrideProgress);
			AddComponent(entity, in component);
		}
	}
}
