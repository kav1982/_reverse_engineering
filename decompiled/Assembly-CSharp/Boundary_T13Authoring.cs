using Unity.Entities;
using UnityEngine;

public class Boundary_T13Authoring : MonoBehaviour
{
	private class Baker : Baker<Boundary_T13Authoring>
	{
		public override void Bake(Boundary_T13Authoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Boundary_T13_Dots component = default(Boundary_T13_Dots);
			component.ett_LayerTile = GetEntity(authoring.ett_LayerTile, TransformUsageFlags.Dynamic);
			component.ett_LayerAO = GetEntity(authoring.ett_LayerAO, TransformUsageFlags.Dynamic);
			component.ett_LayerWall = GetEntity(authoring.ett_LayerWall, TransformUsageFlags.Dynamic);
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
			component.ett_Grass_UR = GetEntity(authoring.ett_Grass_UR, TransformUsageFlags.Dynamic);
			component.ett_Grass_URD = GetEntity(authoring.ett_Grass_URD, TransformUsageFlags.Dynamic);
			component.ett_WallLow_LUR = GetEntity(authoring.ett_WallLow_LUR, TransformUsageFlags.Dynamic);
			component.ett_WallLow_UR = GetEntity(authoring.ett_WallLow_UR, TransformUsageFlags.Dynamic);
			AddComponent(entity, in component);
		}
	}

	public GameObject ett_LayerTile;

	public GameObject ett_LayerAO;

	public GameObject ett_LayerWall;

	[Header("AO")]
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

	[Header("Grass")]
	public GameObject ett_Grass_UR;

	public GameObject ett_Grass_URD;

	[Header("WallLow")]
	public GameObject ett_WallLow_LUR;

	public GameObject ett_WallLow_UR;
}
