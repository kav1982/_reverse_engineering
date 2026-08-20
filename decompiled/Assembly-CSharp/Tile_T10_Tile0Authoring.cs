using Unity.Entities;
using UnityEngine;

public class Tile_T10_Tile0Authoring : MonoBehaviour
{
	private class Baker : Baker<Tile_T10_Tile0Authoring>
	{
		public override void Bake(Tile_T10_Tile0Authoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Tile_T10_Tile0 component = default(Tile_T10_Tile0);
			AddComponent(entity, in component);
		}
	}
}
