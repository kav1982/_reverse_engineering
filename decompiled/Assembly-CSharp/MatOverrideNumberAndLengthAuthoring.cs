using Unity.Entities;
using UnityEngine;

public class MatOverrideNumberAndLengthAuthoring : MonoBehaviour
{
	private class Baker : Baker<MatOverrideNumberAndLengthAuthoring>
	{
		public override void Bake(MatOverrideNumberAndLengthAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			MatOverrideNumberAndLength component = default(MatOverrideNumberAndLength);
			AddComponent(entity, in component);
		}
	}
}
