using Unity.Entities;
using UnityEngine;

public struct Tile_T8_Tile9_Dots : IComponentData, IQueryTypeParameter, IEnableableComponent
{
	public Entity ett_LayerBase;

	public Entity ett_LayerWall;

	[Header("Base")]
	public Entity ett_Wall_Corner_UR;

	public Entity ett_Wall_Corner_RD;

	public Entity ett_Wall_Corner_DL;

	public Entity ett_Wall_Corner_LU;

	public Entity ett_Base_D;

	public Entity ett_Base_LR;

	public Entity ett_Base_LUR;

	public Entity ett_Base_R;

	public Entity ett_Base_RD;

	public Entity ett_Base_RDL;

	public Entity ett_Base_U;

	public Entity ett_Base_UD;

	public Entity ett_Base_UR;

	public Entity ett_Base_URD;

	[Header("Wall")]
	public Entity ett_Wall_D;

	public Entity ett_Wall_Full;

	public Entity ett_Wall_FullFog;

	public Entity ett_Wall_LR;

	public Entity ett_Wall_LUR;

	public Entity ett_Wall_Null;

	public Entity ett_Wall_R;

	public Entity ett_Wall_RD;

	public Entity ett_Wall_RDL;

	public Entity ett_Wall_U;

	public Entity ett_Wall_UD;

	public Entity ett_Wall_UR;

	public Entity ett_Wall_URD;
}
