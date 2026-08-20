using Unity.Entities;
using UnityEngine;

public class MatOverrideMixPercentAuthoring : MonoBehaviour
{
	private class Baker : Baker<MatOverrideMixPercentAuthoring>
	{
		public override void Bake(MatOverrideMixPercentAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			MatOverrideMixPercent component = new MatOverrideMixPercent
			{
				mixPercent = 0f
			};
			AddComponent(entity, in component);
		}
	}
}
