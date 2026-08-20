using Unity.Entities;
using UnityEngine;

internal class MatOverrideFuseShineProgressAuthoring : MonoBehaviour
{
	private class MatOverrideFuseShineProgressAuthoringBaker : Baker<MatOverrideFuseShineProgressAuthoring>
	{
		public override void Bake(MatOverrideFuseShineProgressAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			MatOverrideFuseProgress component = default(MatOverrideFuseProgress);
			AddComponent(entity, in component);
			SetComponentEnabled<MatOverrideFuseProgress>(entity, enabled: false);
		}
	}
}
