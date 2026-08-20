using Unity.Entities;
using UnityEngine;

public class MatOverrideColorAuthoring : MonoBehaviour
{
	private class Baker : Baker<MatOverrideColorAuthoring>
	{
		public override void Bake(MatOverrideColorAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			MatOverrideColor component = new MatOverrideColor
			{
				color = Color.white
			};
			AddComponent(entity, in component);
		}
	}
}
