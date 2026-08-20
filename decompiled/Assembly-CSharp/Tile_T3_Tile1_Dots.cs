using Unity.Entities;
using UnityEngine;

public struct Tile_T3_Tile1_Dots : IComponentData, IQueryTypeParameter, IEnableableComponent
{
	public Entity ett_D;

	public Entity ett_DL;

	public Entity ett_DLU;

	public Entity ett_L;

	public Entity ett_LR;

	public Entity ett_LU;

	public Entity ett_LUR;

	public Entity ett_Null;

	public Entity ett_R;

	public Entity ett_RD;

	public Entity ett_RDL;

	public Entity ett_U;

	public Entity ett_UD;

	public Entity ett_UR;

	public Entity ett_URD;

	[Header("Corner")]
	public Entity ett_CornerUR;

	public Entity ett_CornerRD;

	public Entity ett_CornerDL;

	public Entity ett_CornerLU;

	public Entity ett_NoCornerUR;

	public Entity ett_NoCornerRD;

	public Entity ett_NoCornerDL;

	public Entity ett_NoCornerLU;
}
