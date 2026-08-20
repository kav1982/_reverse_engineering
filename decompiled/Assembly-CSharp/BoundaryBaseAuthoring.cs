using Unity.Entities;
using UnityEngine;

public class BoundaryBaseAuthoring : MonoBehaviour
{
	private class Baker : Baker<BoundaryBaseAuthoring>
	{
		public override void Bake(BoundaryBaseAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			BoundaryBase_Dots component = default(BoundaryBase_Dots);
			AddComponent(entity, in component);
		}
	}
}
