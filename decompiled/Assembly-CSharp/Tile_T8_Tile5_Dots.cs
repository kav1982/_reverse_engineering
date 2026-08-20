using Unity.Entities;
using UnityEngine;

public struct Tile_T8_Tile5_Dots : IComponentData, IQueryTypeParameter, IEnableableComponent
{
	public Entity ett_DL;

	public Entity ett_DLU;

	public Entity ett_Full;

	public Entity ett_LU;

	public Entity ett_LUR;

	public Entity ett_RD;

	public Entity ett_RDL;

	public Entity ett_UR;

	public Entity ett_URD;

	[Header("Corner")]
	public Entity ett_CornerUR;

	public Entity ett_CornerRD;

	public Entity ett_CornerDL;

	public Entity ett_CornerLU;
}
