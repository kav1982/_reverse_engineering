using Unity.Entities;
using UnityEngine;

internal class UnitBehitLayerAuthoring : MonoBehaviour
{
	private class UnitBehitLayerAuthoringBaker : Baker<UnitBehitLayerAuthoring>
	{
		public override void Bake(UnitBehitLayerAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			UnitBehitLayerData component = new UnitBehitLayerData
			{
				BehitLayerEntity = GetEntity(authoring.BehitLayerEntity, TransformUsageFlags.Dynamic)
			};
			AddComponent(entity, in component);
		}
	}

	public GameObject BehitLayerEntity;
}
