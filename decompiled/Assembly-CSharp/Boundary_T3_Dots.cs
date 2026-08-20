using Unity.Entities;
using UnityEngine;

public struct Boundary_T3_Dots : IComponentData, IQueryTypeParameter, IEnableableComponent
{
	public Entity ett_LayerBoundary;

	public Entity ett_LayerAO;

	[Header("AO")]
	public Entity ett_AO_LUR;

	public Entity ett_AO_RD;

	public Entity ett_AO_RDL;

	public Entity ett_AO_UR;

	public Entity ett_AO_URD;

	[Header("Wall")]
	public Entity ett_Wall_Corner_RD;

	public Entity ett_Wall_Corner_RD_Short;

	public Entity ett_Wall_Corner_UR;

	public Entity ett_Wall_LUR;

	public Entity ett_Wall_LUR_Short;

	public Entity ett_Wall_RD;

	public Entity ett_Wall_RDL;

	public Entity ett_Wall_UR;

	public Entity ett_Wall_UR_Short;

	public Entity ett_Wall_URD;

	[Header("Detail")]
	public float detailChance;

	public Entity ett_Detail_Corner_RD;

	public Entity ett_Detail_LUR;

	public Entity ett_Detail_RDL;

	public Entity ett_Detail_URD;
}
