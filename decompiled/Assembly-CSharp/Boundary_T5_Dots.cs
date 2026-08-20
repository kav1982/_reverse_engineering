using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct Boundary_T5_Dots : IComponentData, IQueryTypeParameter, IEnableableComponent
{
	public Entity ett_LayerCliff;

	public Entity ett_LayerWater;

	public Entity ett_LayerWater2;

	public Entity ett_LayerStone;

	public Entity ett_LayerTile;

	[Header("Cliff")]
	public Entity ett_Cliff_D;

	public Entity ett_Cliff_DCorner;

	[Header("Lava")]
	public Entity ett_Lava_Corner_RD;

	public Entity ett_Lava_Corner_UR;

	public Entity ett_Lava_Corner_UR2;

	public Entity ett_Lava_RD;

	public Entity ett_Lava_RDL;

	public Entity ett_Lava_UR;

	public Entity ett_Lava_URD;

	public float3 waterOffset;

	[Header("Stone")]
	public Entity ett_Stone_Corner_RD;

	public Entity ett_Stone_Corner_UR;

	public Entity ett_Stone_LUR;

	public Entity ett_Stone_RD;

	public Entity ett_Stone_UR;

	public Entity ett_Stone_URD;

	public float stoneOffset;

	[Header("Tile")]
	public Entity ett_Tile_Corner_RD;

	public Entity ett_Tile_Corner_UR;

	public Entity ett_Tile_LUR;

	public Entity ett_Tile_RD;

	public Entity ett_Tile_RDL;

	public Entity ett_Tile_UR;

	public Entity ett_Tile_URD;
}
