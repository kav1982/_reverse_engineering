using Unity.Entities;
using UnityEngine;

public class Tile_T0_Tile6Authoring : MonoBehaviour
{
	private class Baker : Baker<Tile_T0_Tile6Authoring>
	{
		public override void Bake(Tile_T0_Tile6Authoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Tile_T0_Tile6_Dots component = new Tile_T0_Tile6_Dots
			{
				ett_Corner_UR = GetEntity(authoring.ett_Corner_UR, TransformUsageFlags.Dynamic),
				ett_Full = GetEntity(authoring.ett_Full, TransformUsageFlags.Dynamic),
				ett_LUR = GetEntity(authoring.ett_LUR, TransformUsageFlags.Dynamic),
				ett_UR = GetEntity(authoring.ett_UR, TransformUsageFlags.Dynamic)
			};
			AddComponent(entity, in component);
		}
	}

	public GameObject ett_Corner_UR;

	public GameObject ett_Full;

	public GameObject ett_LUR;

	public GameObject ett_UR;
}
