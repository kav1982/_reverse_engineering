using Unity.Entities;
using UnityEngine;

public struct Boundary_T13_Dots : IComponentData, IQueryTypeParameter, IEnableableComponent
{
	public Entity ett_LayerTile;

	public Entity ett_LayerAO;

	public Entity ett_LayerWall;

	[Header("AO")]
	public Entity ett_AO_LUR;

	public Entity ett_AO_RD;

	public Entity ett_AO_RDL;

	public Entity ett_AO_UR;

	public Entity ett_AO_URD;

	[Header("Wall")]
	public Entity ett_Wall_Corner_RD;

	public Entity ett_Wall_Corner_UR;

	public Entity ett_Wall_LUR;

	public Entity ett_Wall_RD;

	public Entity ett_Wall_RDL;

	public Entity ett_Wall_UR;

	public Entity ett_Wall_URD;

	[Header("Grass")]
	public Entity ett_Grass_UR;

	public Entity ett_Grass_URD;

	[Header("WallLow")]
	public Entity ett_WallLow_LUR;

	public Entity ett_WallLow_UR;
}
