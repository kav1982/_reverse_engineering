using Unity.Entities;
using UnityEngine;

public class Tile_T9_Tile9Authoring : MonoBehaviour
{
	private class Baker : Baker<Tile_T9_Tile9Authoring>
	{
		public override void Bake(Tile_T9_Tile9Authoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Tile_T9_Tile9_Dots component = new Tile_T9_Tile9_Dots
			{
				ett_LayerBase = GetEntity(authoring.ett_LayerBase, TransformUsageFlags.Dynamic),
				ett_LayerWall = GetEntity(authoring.ett_LayerWall, TransformUsageFlags.Dynamic),
				ett_Base_D = GetEntity(authoring.ett_Base_D, TransformUsageFlags.Dynamic),
				ett_Base_LR = GetEntity(authoring.ett_Base_LR, TransformUsageFlags.Dynamic),
				ett_Base_LUR = GetEntity(authoring.ett_Base_LUR, TransformUsageFlags.Dynamic),
				ett_Base_R = GetEntity(authoring.ett_Base_R, TransformUsageFlags.Dynamic),
				ett_Base_RD = GetEntity(authoring.ett_Base_RD, TransformUsageFlags.Dynamic),
				ett_Base_RDL = GetEntity(authoring.ett_Base_RDL, TransformUsageFlags.Dynamic),
				ett_Base_U = GetEntity(authoring.ett_Base_U, TransformUsageFlags.Dynamic),
				ett_Base_UD = GetEntity(authoring.ett_Base_UD, TransformUsageFlags.Dynamic),
				ett_Base_UR = GetEntity(authoring.ett_Base_UR, TransformUsageFlags.Dynamic),
				ett_Base_URD = GetEntity(authoring.ett_Base_URD, TransformUsageFlags.Dynamic),
				ett_Wall_Corner_UR = GetEntity(authoring.ett_Wall_Corner_UR, TransformUsageFlags.Dynamic),
				ett_Wall_Corner_RD = GetEntity(authoring.ett_Wall_Corner_RD, TransformUsageFlags.Dynamic),
				ett_Wall_Corner_DL = GetEntity(authoring.ett_Wall_Corner_DL, TransformUsageFlags.Dynamic),
				ett_Wall_Corner_LU = GetEntity(authoring.ett_Wall_Corner_LU, TransformUsageFlags.Dynamic),
				ett_Wall_D = GetEntity(authoring.ett_Wall_D, TransformUsageFlags.Dynamic),
				ett_Wall_Full = GetEntity(authoring.ett_Wall_Full, TransformUsageFlags.Dynamic),
				ett_Wall_FullFog = GetEntity(authoring.ett_Wall_FullFog, TransformUsageFlags.Dynamic),
				ett_Wall_LR = GetEntity(authoring.ett_Wall_LR, TransformUsageFlags.Dynamic),
				ett_Wall_LUR = GetEntity(authoring.ett_Wall_LUR, TransformUsageFlags.Dynamic),
				ett_Wall_Null = GetEntity(authoring.ett_Wall_Null, TransformUsageFlags.Dynamic),
				ett_Wall_R = GetEntity(authoring.ett_Wall_R, TransformUsageFlags.Dynamic),
				ett_Wall_RD = GetEntity(authoring.ett_Wall_RD, TransformUsageFlags.Dynamic),
				ett_Wall_RDL = GetEntity(authoring.ett_Wall_RDL, TransformUsageFlags.Dynamic),
				ett_Wall_U = GetEntity(authoring.ett_Wall_U, TransformUsageFlags.Dynamic),
				ett_Wall_UD = GetEntity(authoring.ett_Wall_UD, TransformUsageFlags.Dynamic),
				ett_Wall_UR = GetEntity(authoring.ett_Wall_UR, TransformUsageFlags.Dynamic),
				ett_Wall_URD = GetEntity(authoring.ett_Wall_URD, TransformUsageFlags.Dynamic),
				ett_Collider_Full = GetEntity(authoring.ett_Collider_Full, TransformUsageFlags.Dynamic),
				ett_Collider_Big = GetEntity(authoring.ett_Collider_Big, TransformUsageFlags.Dynamic),
				ett_Collider_Small = GetEntity(authoring.ett_Collider_Small, TransformUsageFlags.Dynamic)
			};
			AddComponent(entity, in component);
			AddComponentObject(entity, new BoundaryT2RoomCtrller());
		}
	}

	public GameObject ett_LayerBase;

	public GameObject ett_LayerWall;

	[Header("Base")]
	public GameObject ett_Base_D;

	public GameObject ett_Base_LR;

	public GameObject ett_Base_LUR;

	public GameObject ett_Base_R;

	public GameObject ett_Base_RD;

	public GameObject ett_Base_RDL;

	public GameObject ett_Base_U;

	public GameObject ett_Base_UD;

	public GameObject ett_Base_UR;

	public GameObject ett_Base_URD;

	[Header("Wall")]
	public GameObject ett_Wall_Corner_UR;

	public GameObject ett_Wall_Corner_RD;

	public GameObject ett_Wall_Corner_DL;

	public GameObject ett_Wall_Corner_LU;

	public GameObject ett_Wall_D;

	public GameObject ett_Wall_Full;

	public GameObject ett_Wall_FullFog;

	public GameObject ett_Wall_LR;

	public GameObject ett_Wall_LUR;

	public GameObject ett_Wall_Null;

	public GameObject ett_Wall_R;

	public GameObject ett_Wall_RD;

	public GameObject ett_Wall_RDL;

	public GameObject ett_Wall_U;

	public GameObject ett_Wall_UD;

	public GameObject ett_Wall_UR;

	public GameObject ett_Wall_URD;

	[Header("ColliderAndShadow")]
	public GameObject ett_Collider_Full;

	public GameObject ett_Collider_Big;

	public GameObject ett_Collider_Small;
}
