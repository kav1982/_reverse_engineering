using Unity.Entities;
using UnityEngine;

[DisallowMultipleComponent]
public class TileBaseAuthoring : MonoBehaviour
{
	private class Baker : Baker<TileBaseAuthoring>
	{
		public override void Bake(TileBaseAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			TileBase_Dots component = default(TileBase_Dots);
			AddComponent(entity, in component);
		}
	}
}
