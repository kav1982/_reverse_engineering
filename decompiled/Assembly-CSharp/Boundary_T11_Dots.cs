using Unity.Entities;
using UnityEngine;

public struct Boundary_T11_Dots : IComponentData, IQueryTypeParameter, IEnableableComponent
{
	public Entity ett_LayerTile;

	[Header("AO")]
	public Entity ett_LayerAO;

	public Entity ett_AO_LUR;

	public Entity ett_AO_RD;

	public Entity ett_AO_RDL;

	public Entity ett_AO_UR;

	public Entity ett_AO_URD;

	[Header("Wall")]
	public Entity ett_LayerWall;

	public Entity ett_Wall_Corner_RD;

	public Entity ett_Wall_Corner_UR;

	public Entity ett_Wall_LUR;

	public Entity ett_Wall_RD;

	public Entity ett_Wall_RDL;

	public Entity ett_Wall_UR;

	public Entity ett_Wall_URD;

	[Header("ColliderAndShadow")]
	public Entity ett_Collider_Full;

	public Entity ett_Collider_Big;

	public Entity ett_Collider_Small;

	[Header("Detail")]
	[Range(0f, 1f)]
	public float detailChance;

	public Entity ett_Detail_LUR;

	public Entity ett_Detail_RD;

	public Entity ett_Detail_RDL;

	public Entity ett_Detail_UR;

	public Entity ett_Detail_URD;
}
