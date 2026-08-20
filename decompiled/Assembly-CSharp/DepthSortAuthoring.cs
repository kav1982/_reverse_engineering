using Unity.Entities;
using Unity.Rendering;
using UnityEngine;

internal class DepthSortAuthoring : MonoBehaviour
{
	private class DepthSortAuthoringBaker : Baker<DepthSortAuthoring>
	{
		public override void Bake(DepthSortAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			AddComponent<DepthSorted_Tag>(entity);
		}
	}
}
