using Unity.Entities;
using UnityEngine;

public class Tile_T3_Tile0Authoring : MonoBehaviour
{
	private class Baker : Baker<Tile_T3_Tile0Authoring>
	{
		public override void Bake(Tile_T3_Tile0Authoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Tile_T3_Tile0_Dots component = default(Tile_T3_Tile0_Dots);
			AddComponent(entity, in component);
		}
	}
}
