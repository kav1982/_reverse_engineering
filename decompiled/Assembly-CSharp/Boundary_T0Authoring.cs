using Unity.Entities;
using UnityEngine;

public class Boundary_T0Authoring : MonoBehaviour
{
	private class Baker : Baker<Boundary_T0Authoring>
	{
		public override void Bake(Boundary_T0Authoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Boundary_T0_Dots component = default(Boundary_T0_Dots);
			component.ett_LayerAO = GetEntity(authoring.ett_LayerAO, TransformUsageFlags.Dynamic);
			component.ett_AO_LUR = GetEntity(authoring.ett_AO_LUR, TransformUsageFlags.Dynamic);
			component.ett_AO_RD = GetEntity(authoring.ett_AO_RD, TransformUsageFlags.Dynamic);
			component.ett_AO_RDL = GetEntity(authoring.ett_AO_RDL, TransformUsageFlags.Dynamic);
			component.ett_AO_UR = GetEntity(authoring.ett_AO_UR, TransformUsageFlags.Dynamic);
			component.ett_AO_URD = GetEntity(authoring.ett_AO_URD, TransformUsageFlags.Dynamic);
			component.ett_Wall_Corner_RD = GetEntity(authoring.ett_Wall_Corner_RD, TransformUsageFlags.Dynamic);
			component.ett_Wall_Corner_RD_Short = GetEntity(authoring.ett_Wall_Corner_RD_Short, TransformUsageFlags.Dynamic);
			component.ett_Wall_Corner_UR = GetEntity(authoring.ett_Wall_Corner_UR, TransformUsageFlags.Dynamic);
			component.ett_Wall_LUR = GetEntity(authoring.ett_Wall_LUR, TransformUsageFlags.Dynamic);
			component.ett_Wall_LUR_Short = GetEntity(authoring.ett_Wall_LUR_Short, TransformUsageFlags.Dynamic);
			component.ett_Wall_RD = GetEntity(authoring.ett_Wall_RD, TransformUsageFlags.Dynamic);
			component.ett_Wall_RDL = GetEntity(authoring.ett_Wall_RDL, TransformUsageFlags.Dynamic);
			component.ett_Wall_UR = GetEntity(authoring.ett_Wall_UR, TransformUsageFlags.Dynamic);
			component.ett_Wall_UR_Short = GetEntity(authoring.ett_Wall_UR_Short, TransformUsageFlags.Dynamic);
			component.ett_Wall_URAndCorner = GetEntity(authoring.ett_Wall_URAndCorner, TransformUsageFlags.Dynamic);
			component.ett_Wall_URD = GetEntity(authoring.ett_Wall_URD, TransformUsageFlags.Dynamic);
			component.detailChance = authoring.detailChance;
			component.detailInterval = authoring.detailInterval;
			component.ett_Detail_LUR = GetEntity(authoring.ett_Detail_LUR, TransformUsageFlags.Dynamic);
			component.ett_Detail_URD = GetEntity(authoring.ett_Detail_URD, TransformUsageFlags.Dynamic);
			component.ett_Detail_RDL = GetEntity(authoring.ett_Detail_RDL, TransformUsageFlags.Dynamic);
			component.detailChance_Corner = authoring.detailChance_Corner;
			component.ett_Detail_Corner_RD = GetEntity(authoring.ett_Detail_Corner_RD, TransformUsageFlags.Dynamic);
			component.ett_Detail_Corner_UR = GetEntity(authoring.ett_Detail_Corner_UR, TransformUsageFlags.Dynamic);
			AddComponent(entity, in component);
		}
	}

	[Header("AO")]
	public GameObject ett_LayerAO;

	public GameObject ett_AO_LUR;

	public GameObject ett_AO_RD;

	public GameObject ett_AO_RDL;

	public GameObject ett_AO_UR;

	public GameObject ett_AO_URD;

	[Header("Wall")]
	public GameObject ett_Wall_Corner_RD;

	public GameObject ett_Wall_Corner_RD_Short;

	public GameObject ett_Wall_Corner_UR;

	public GameObject ett_Wall_LUR;

	public GameObject ett_Wall_LUR_Short;

	public GameObject ett_Wall_RD;

	public GameObject ett_Wall_RDL;

	public GameObject ett_Wall_UR;

	public GameObject ett_Wall_UR_Short;

	public GameObject ett_Wall_URAndCorner;

	public GameObject ett_Wall_URD;

	[Header("Detail")]
	public int detailInterval;

	[Range(0f, 1f)]
	public float detailChance_Corner;

	public GameObject ett_Detail_Corner_RD;

	public GameObject ett_Detail_Corner_UR;

	[Range(0f, 1f)]
	public float detailChance;

	public GameObject ett_Detail_LUR;

	public GameObject ett_Detail_RDL;

	public GameObject ett_Detail_URD;
}
