using Unity.Entities;
using UnityEngine;

public class Boundary_T10Authoring : MonoBehaviour
{
	private class Baker : Baker<Boundary_T10Authoring>
	{
		public override void Bake(Boundary_T10Authoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Boundary_T10_Dots component = default(Boundary_T10_Dots);
			component.ett_LayerAO = GetEntity(authoring.ett_LayerAO, TransformUsageFlags.Dynamic);
			component.ett_AO_LUR = GetEntity(authoring.ett_AO_LUR, TransformUsageFlags.Dynamic);
			component.ett_AO_RD = GetEntity(authoring.ett_AO_RD, TransformUsageFlags.Dynamic);
			component.ett_AO_RDL = GetEntity(authoring.ett_AO_RDL, TransformUsageFlags.Dynamic);
			component.ett_AO_UR = GetEntity(authoring.ett_AO_UR, TransformUsageFlags.Dynamic);
			component.ett_AO_URD = GetEntity(authoring.ett_AO_URD, TransformUsageFlags.Dynamic);
			component.ett_Wall_Corner_RD = GetEntity(authoring.ett_Wall_Corner_RD, TransformUsageFlags.Dynamic);
			component.ett_Wall_Corner_UR = GetEntity(authoring.ett_Wall_Corner_UR, TransformUsageFlags.Dynamic);
			component.ett_Wall_LUR = GetEntity(authoring.ett_Wall_LUR, TransformUsageFlags.Dynamic);
			component.ett_Wall_RD = GetEntity(authoring.ett_Wall_RD, TransformUsageFlags.Dynamic);
			component.ett_Wall_RDL = GetEntity(authoring.ett_Wall_RDL, TransformUsageFlags.Dynamic);
			component.ett_Wall_UR = GetEntity(authoring.ett_Wall_UR, TransformUsageFlags.Dynamic);
			component.ett_Wall_URD = GetEntity(authoring.ett_Wall_URD, TransformUsageFlags.Dynamic);
			component.detailChance = authoring.detailChance;
			component.ett_Detail_LUR = GetEntity(authoring.ett_Detail_LUR, TransformUsageFlags.Dynamic);
			component.ett_Detail_RDL = GetEntity(authoring.ett_Detail_RDL, TransformUsageFlags.Dynamic);
			component.ett_Detail_URD = GetEntity(authoring.ett_Detail_URD, TransformUsageFlags.Dynamic);
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

	public GameObject ett_Wall_Corner_UR;

	public GameObject ett_Wall_LUR;

	public GameObject ett_Wall_RD;

	public GameObject ett_Wall_RDL;

	public GameObject ett_Wall_UR;

	public GameObject ett_Wall_URD;

	[Range(0f, 1f)]
	[Header("Detail")]
	public float detailChance;

	public GameObject ett_Detail_LUR;

	public GameObject ett_Detail_RDL;

	public GameObject ett_Detail_URD;
}
