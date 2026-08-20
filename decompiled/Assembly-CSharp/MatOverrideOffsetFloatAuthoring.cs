using Unity.Entities;
using UnityEngine;

internal class MatOverrideOffsetFloatAuthoring : MonoBehaviour
{
	private class MatOverrideOffsetFloatAuthoringBaker : Baker<MatOverrideOffsetFloatAuthoring>
	{
		public override void Bake(MatOverrideOffsetFloatAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			MatOverrideOffsetFloat component = new MatOverrideOffsetFloat
			{
				offset = authoring.startValue
			};
			AddComponent(entity, in component);
		}
	}

	public float startValue;
}
