using Unity.Entities;
using UnityEngine;

internal class MatOverrideAddGaintArrowColorAuthoring : MonoBehaviour
{
	private class MatOverrideAddGaintArrowColorBaker : Baker<MatOverrideAddGaintArrowColorAuthoring>
	{
		public override void Bake(MatOverrideAddGaintArrowColorAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			MatOverrideAddGaintArrowColor component = new MatOverrideAddGaintArrowColor
			{
				addGaintArrowColor = 0f
			};
			AddComponent(entity, in component);
		}
	}
}
