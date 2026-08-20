using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct Boundary_T1_Dots : IComponentData, IQueryTypeParameter, IEnableableComponent
{
	public Entity ett_LayerCliff;

	public Entity ett_LayerLava;

	public Entity ett_LayerLava2;

	public Entity ett_LayerRail;

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

	public float3 lavaOffset;

	[Header("Rail")]
	public Entity ett_Rail_Corner_RD;

	public Entity ett_Rail_Corner_UR;

	public Entity ett_Rail_LUR;

	public Entity ett_Rail_RD;

	public Entity ett_Rail_UR;

	public Entity ett_Rail_URD;

	public float railOffset;

	[Header("Tile")]
	public Entity ett_Tile_Corner_RD;

	public Entity ett_Tile_Corner_UR;

	public Entity ett_Tile_LUR;

	public Entity ett_Tile_RD;

	public Entity ett_Tile_RDL;

	public Entity ett_Tile_UR;

	public Entity ett_Tile_URD;
}
