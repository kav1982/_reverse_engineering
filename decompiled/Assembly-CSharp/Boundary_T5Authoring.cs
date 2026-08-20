using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class Boundary_T5Authoring : MonoBehaviour
{
	private class Baker : Baker<Boundary_T5Authoring>
	{
		public override void Bake(Boundary_T5Authoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Boundary_T5_Dots component = default(Boundary_T5_Dots);
			component.ett_LayerCliff = GetEntity(authoring.ett_LayerCliff, TransformUsageFlags.Dynamic);
			component.ett_LayerWater = GetEntity(authoring.ett_LayerWater, TransformUsageFlags.Dynamic);
			component.ett_LayerWater2 = GetEntity(authoring.ett_LayerWater2, TransformUsageFlags.Dynamic);
			component.ett_LayerStone = GetEntity(authoring.ett_LayerStone, TransformUsageFlags.Dynamic);
			component.ett_LayerTile = GetEntity(authoring.ett_LayerTile, TransformUsageFlags.Dynamic);
			component.ett_Cliff_D = GetEntity(authoring.ett_Cliff_D, TransformUsageFlags.Dynamic);
			component.ett_Cliff_DCorner = GetEntity(authoring.ett_Cliff_DCorner, TransformUsageFlags.Dynamic);
			component.ett_Lava_Corner_RD = GetEntity(authoring.ett_Lava_Corner_RD, TransformUsageFlags.Dynamic);
			component.ett_Lava_Corner_UR = GetEntity(authoring.ett_Lava_Corner_UR, TransformUsageFlags.Dynamic);
			component.ett_Lava_Corner_UR2 = GetEntity(authoring.ett_Lava_Corner_UR2, TransformUsageFlags.Dynamic);
			component.ett_Lava_RD = GetEntity(authoring.ett_Lava_RD, TransformUsageFlags.Dynamic);
			component.ett_Lava_RDL = GetEntity(authoring.ett_Lava_RDL, TransformUsageFlags.Dynamic);
			component.ett_Lava_UR = GetEntity(authoring.ett_Lava_UR, TransformUsageFlags.Dynamic);
			component.ett_Lava_URD = GetEntity(authoring.ett_Lava_URD, TransformUsageFlags.Dynamic);
			component.waterOffset = authoring.waterOffset;
			component.ett_Stone_Corner_RD = GetEntity(authoring.ett_Stone_Corner_RD, TransformUsageFlags.Dynamic);
			component.ett_Stone_Corner_UR = GetEntity(authoring.ett_Stone_Corner_UR, TransformUsageFlags.Dynamic);
			component.ett_Stone_LUR = GetEntity(authoring.ett_Stone_LUR, TransformUsageFlags.Dynamic);
			component.ett_Stone_RD = GetEntity(authoring.ett_Stone_RD, TransformUsageFlags.Dynamic);
			component.ett_Stone_UR = GetEntity(authoring.ett_Stone_UR, TransformUsageFlags.Dynamic);
			component.ett_Stone_URD = GetEntity(authoring.ett_Stone_URD, TransformUsageFlags.Dynamic);
			component.stoneOffset = authoring.stoneOffset;
			component.ett_Tile_Corner_RD = GetEntity(authoring.ett_Tile_Corner_RD, TransformUsageFlags.Dynamic);
			component.ett_Tile_Corner_UR = GetEntity(authoring.ett_Tile_Corner_UR, TransformUsageFlags.Dynamic);
			component.ett_Tile_LUR = GetEntity(authoring.ett_Tile_LUR, TransformUsageFlags.Dynamic);
			component.ett_Tile_RD = GetEntity(authoring.ett_Tile_RD, TransformUsageFlags.Dynamic);
			component.ett_Tile_RDL = GetEntity(authoring.ett_Tile_RDL, TransformUsageFlags.Dynamic);
			component.ett_Tile_UR = GetEntity(authoring.ett_Tile_UR, TransformUsageFlags.Dynamic);
			component.ett_Tile_URD = GetEntity(authoring.ett_Tile_URD, TransformUsageFlags.Dynamic);
			AddComponent(entity, in component);
		}
	}

	public GameObject ett_LayerCliff;

	public GameObject ett_LayerWater;

	public GameObject ett_LayerWater2;

	public GameObject ett_LayerStone;

	public GameObject ett_LayerTile;

	[Header("Cliff")]
	public GameObject ett_Cliff_D;

	public GameObject ett_Cliff_DCorner;

	[Header("Water")]
	public GameObject ett_Lava_Corner_RD;

	public GameObject ett_Lava_Corner_UR;

	public GameObject ett_Lava_Corner_UR2;

	public GameObject ett_Lava_RD;

	public GameObject ett_Lava_RDL;

	public GameObject ett_Lava_UR;

	public GameObject ett_Lava_URD;

	public float3 waterOffset;

	[Header("Stone")]
	public GameObject ett_Stone_Corner_RD;

	public GameObject ett_Stone_Corner_UR;

	public GameObject ett_Stone_LUR;

	public GameObject ett_Stone_RD;

	public GameObject ett_Stone_UR;

	public GameObject ett_Stone_URD;

	public float stoneOffset;

	[Header("Tile")]
	public GameObject ett_Tile_Corner_RD;

	public GameObject ett_Tile_Corner_UR;

	public GameObject ett_Tile_LUR;

	public GameObject ett_Tile_RD;

	public GameObject ett_Tile_RDL;

	public GameObject ett_Tile_UR;

	public GameObject ett_Tile_URD;
}
