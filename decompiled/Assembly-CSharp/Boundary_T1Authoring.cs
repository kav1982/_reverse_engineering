using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class Boundary_T1Authoring : MonoBehaviour
{
	private class Baker : Baker<Boundary_T1Authoring>
	{
		public override void Bake(Boundary_T1Authoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Boundary_T1_Dots component = default(Boundary_T1_Dots);
			component.ett_LayerCliff = GetEntity(authoring.ett_LayerCliff, TransformUsageFlags.Dynamic);
			component.ett_LayerLava = GetEntity(authoring.ett_LayerLava, TransformUsageFlags.Dynamic);
			component.ett_LayerLava2 = GetEntity(authoring.ett_LayerLava2, TransformUsageFlags.Dynamic);
			component.ett_LayerRail = GetEntity(authoring.ett_LayerRail, TransformUsageFlags.Dynamic);
			component.ett_LayerTile = GetEntity(authoring.ett_LayerTile, TransformUsageFlags.Dynamic);
			component.ett_Cliff_D = GetEntity(authoring.ett_Cliff_D, TransformUsageFlags.Dynamic);
			component.ett_Cliff_DCorner = GetEntity(authoring.ett_Cliff_DCorner, TransformUsageFlags.Dynamic);
			component.ett_Lava_Corner_RD = GetEntity(authoring.ett_Lava_Corner_RD, TransformUsageFlags.Dynamic);
			component.ett_Lava_Corner_UR = GetEntity(authoring.ett_Lava_Corner_UR, TransformUsageFlags.Dynamic);
			component.ett_Lava_Corner_UR2 = GetEntity(authoring.ett_Lava_Corner_UR2, TransformUsageFlags.Dynamic);
			component.ett_Lava_RD = GetEntity(authoring.ett_Lava_RD, TransformUsageFlags.Dynamic);
			component.ett_Lava_RDL = GetEntity(authoring.ett_Lava_RDL, TransformUsageFlags.Dynamic);
			component.ett_Lava_UR = GetEntity(authoring.ett_Lava_UR, TransformUsageFlags.Dynamic);
			component.ett_Lava_URD = GetEntity(authoring.ett_Lava_URD, TransformUsageFlags.Dynamic);
			component.lavaOffset = authoring.lavaOffset;
			component.ett_Rail_Corner_RD = GetEntity(authoring.ett_Rail_Corner_RD, TransformUsageFlags.Dynamic);
			component.ett_Rail_Corner_UR = GetEntity(authoring.ett_Rail_Corner_UR, TransformUsageFlags.Dynamic);
			component.ett_Rail_LUR = GetEntity(authoring.ett_Rail_LUR, TransformUsageFlags.Dynamic);
			component.ett_Rail_RD = GetEntity(authoring.ett_Rail_RD, TransformUsageFlags.Dynamic);
			component.ett_Rail_UR = GetEntity(authoring.ett_Rail_UR, TransformUsageFlags.Dynamic);
			component.ett_Rail_URD = GetEntity(authoring.ett_Rail_URD, TransformUsageFlags.Dynamic);
			component.railOffset = authoring.railOffset;
			component.ett_Tile_Corner_RD = GetEntity(authoring.ett_Tile_Corner_RD, TransformUsageFlags.Dynamic);
			component.ett_Tile_Corner_UR = GetEntity(authoring.ett_Tile_Corner_UR, TransformUsageFlags.Dynamic);
			component.ett_Tile_LUR = GetEntity(authoring.ett_Tile_LUR, TransformUsageFlags.Dynamic);
			component.ett_Tile_RD = GetEntity(authoring.ett_Tile_RD, TransformUsageFlags.Dynamic);
			component.ett_Tile_RDL = GetEntity(authoring.ett_Tile_RDL, TransformUsageFlags.Dynamic);
			component.ett_Tile_UR = GetEntity(authoring.ett_Tile_UR, TransformUsageFlags.Dynamic);
			component.ett_Tile_URD = GetEntity(authoring.ett_Tile_URD, TransformUsageFlags.Dynamic);
			AddComponent(entity, in component);
		}
	}

	public GameObject ett_LayerCliff;

	public GameObject ett_LayerLava;

	public GameObject ett_LayerLava2;

	public GameObject ett_LayerRail;

	public GameObject ett_LayerTile;

	[Header("Cliff")]
	public GameObject ett_Cliff_D;

	public GameObject ett_Cliff_DCorner;

	[Header("Lava")]
	public GameObject ett_Lava_Corner_RD;

	public GameObject ett_Lava_Corner_UR;

	public GameObject ett_Lava_Corner_UR2;

	public GameObject ett_Lava_RD;

	public GameObject ett_Lava_RDL;

	public GameObject ett_Lava_UR;

	public GameObject ett_Lava_URD;

	public float3 lavaOffset;

	[Header("Rail")]
	public GameObject ett_Rail_Corner_RD;

	public GameObject ett_Rail_Corner_UR;

	public GameObject ett_Rail_LUR;

	public GameObject ett_Rail_RD;

	public GameObject ett_Rail_UR;

	public GameObject ett_Rail_URD;

	public float railOffset;

	[Header("Tile")]
	public GameObject ett_Tile_Corner_RD;

	public GameObject ett_Tile_Corner_UR;

	public GameObject ett_Tile_LUR;

	public GameObject ett_Tile_RD;

	public GameObject ett_Tile_RDL;

	public GameObject ett_Tile_UR;

	public GameObject ett_Tile_URD;
}
