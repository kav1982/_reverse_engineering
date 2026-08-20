using Unity.Entities;
using UnityEngine;

internal class MatOverrideTwirlProgressAuthoring : MonoBehaviour
{
	private class MatOverrideTwirlProgressAuthoringBaker : Baker<MatOverrideTwirlProgressAuthoring>
	{
		public override void Bake(MatOverrideTwirlProgressAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			MatOverrideTwirlProgressData component = default(MatOverrideTwirlProgressData);
			AddComponent(entity, in component);
		}
	}
}
