using Unity.Entities;
using UnityEngine;

public class Boundary_T2Authoring : MonoBehaviour
{
	private class Baker : Baker<Boundary_T2Authoring>
	{
		public override void Bake(Boundary_T2Authoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Boundary_T2_Dots component = default(Boundary_T2_Dots);
			component.ett_LayerTile = GetEntity(authoring.ett_LayerTile, TransformUsageFlags.Dynamic);
			component.ett_LayerAO = GetEntity(authoring.ett_LayerAO, TransformUsageFlags.Dynamic);
			component.ett_AO_D = GetEntity(authoring.ett_AO_D, TransformUsageFlags.Dynamic);
			component.ett_AO_LUR = GetEntity(authoring.ett_AO_LUR, TransformUsageFlags.Dynamic);
			component.ett_AO_R = GetEntity(authoring.ett_AO_R, TransformUsageFlags.Dynamic);
			component.ett_AO_RD = GetEntity(authoring.ett_AO_RD, TransformUsageFlags.Dynamic);
			component.ett_AO_RDL = GetEntity(authoring.ett_AO_RDL, TransformUsageFlags.Dynamic);
			component.ett_AO_U = GetEntity(authoring.ett_AO_U, TransformUsageFlags.Dynamic);
			component.ett_AO_UR = GetEntity(authoring.ett_AO_UR, TransformUsageFlags.Dynamic);
			component.ett_AO_URD = GetEntity(authoring.ett_AO_URD, TransformUsageFlags.Dynamic);
			component.ett_LayerWall = GetEntity(authoring.ett_LayerWall, TransformUsageFlags.Dynamic);
			component.ett_Wall_Corner_RD = GetEntity(authoring.ett_Wall_Corner_RD, TransformUsageFlags.Dynamic);
			component.ett_Wall_Corner_UR = GetEntity(authoring.ett_Wall_Corner_UR, TransformUsageFlags.Dynamic);
			component.ett_Wall_D = GetEntity(authoring.ett_Wall_D, TransformUsageFlags.Dynamic);
			component.ett_Wall_LUR = GetEntity(authoring.ett_Wall_LUR, TransformUsageFlags.Dynamic);
			component.ett_Wall_RD = GetEntity(authoring.ett_Wall_RD, TransformUsageFlags.Dynamic);
			component.ett_Wall_RDL = GetEntity(authoring.ett_Wall_RDL, TransformUsageFlags.Dynamic);
			component.ett_Wall_U = GetEntity(authoring.ett_Wall_U, TransformUsageFlags.Dynamic);
			component.ett_Wall_UR = GetEntity(authoring.ett_Wall_UR, TransformUsageFlags.Dynamic);
			component.ett_Wall_URD = GetEntity(authoring.ett_Wall_URD, TransformUsageFlags.Dynamic);
			component.ett_Collider_Full = GetEntity(authoring.ett_Collider_Full, TransformUsageFlags.Dynamic);
			component.ett_Collider_Big = GetEntity(authoring.ett_Collider_Big, TransformUsageFlags.Dynamic);
			component.ett_Collider_Small = GetEntity(authoring.ett_Collider_Small, TransformUsageFlags.Dynamic);
			component.detailChance = authoring.detailChance;
			component.ett_Detail_LUR = GetEntity(authoring.ett_Detail_LUR, TransformUsageFlags.Dynamic);
			component.ett_Detail_RD = GetEntity(authoring.ett_Detail_RD, TransformUsageFlags.Dynamic);
			component.ett_Detail_RDL = GetEntity(authoring.ett_Detail_RDL, TransformUsageFlags.Dynamic);
			component.ett_Detail_UR = GetEntity(authoring.ett_Detail_UR, TransformUsageFlags.Dynamic);
			component.ett_Detail_URD = GetEntity(authoring.ett_Detail_URD, TransformUsageFlags.Dynamic);
			AddComponent(entity, in component);
			AddComponentObject(entity, new BoundaryT2RoomCtrller());
		}
	}

	public GameObject ett_LayerTile;

	[Header("AO")]
	public GameObject ett_LayerAO;

	public GameObject ett_AO_D;

	public GameObject ett_AO_LUR;

	public GameObject ett_AO_R;

	public GameObject ett_AO_RD;

	public GameObject ett_AO_RDL;

	public GameObject ett_AO_U;

	public GameObject ett_AO_UR;

	public GameObject ett_AO_URD;

	[Header("Wall")]
	public GameObject ett_LayerWall;

	public GameObject ett_Wall_Corner_RD;

	public GameObject ett_Wall_Corner_UR;

	public GameObject ett_Wall_D;

	public GameObject ett_Wall_LUR;

	public GameObject ett_Wall_RD;

	public GameObject ett_Wall_RDL;

	public GameObject ett_Wall_U;

	public GameObject ett_Wall_UR;

	public GameObject ett_Wall_URD;

	[Header("ColliderAndShadow")]
	public GameObject ett_Collider_Full;

	public GameObject ett_Collider_Big;

	public GameObject ett_Collider_Small;

	[Header("Detail")]
	[Range(0f, 1f)]
	public float detailChance;

	public GameObject ett_Detail_LUR;

	public GameObject ett_Detail_RD;

	public GameObject ett_Detail_RDL;

	public GameObject ett_Detail_UR;

	public GameObject ett_Detail_URD;
}
