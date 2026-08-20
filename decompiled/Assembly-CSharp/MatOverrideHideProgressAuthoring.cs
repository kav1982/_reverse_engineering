using Unity.Entities;
using UnityEngine;

internal class MatOverrideHideProgressAuthoring : MonoBehaviour
{
	private class MatOverrideHideProgressAuthoringBaker : Baker<MatOverrideHideProgressAuthoring>
	{
		public override void Bake(MatOverrideHideProgressAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			MatOverrideHideProgressEffect component = default(MatOverrideHideProgressEffect);
			AddComponent(entity, in component);
		}
	}
}
