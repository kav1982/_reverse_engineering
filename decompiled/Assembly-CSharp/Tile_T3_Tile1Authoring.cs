using Unity.Entities;
using UnityEngine;

public class Tile_T3_Tile1Authoring : MonoBehaviour
{
	private class Baker : Baker<Tile_T3_Tile1Authoring>
	{
		public override void Bake(Tile_T3_Tile1Authoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Tile_T3_Tile1_Dots component = new Tile_T3_Tile1_Dots
			{
				ett_D = GetEntity(authoring.ett_D, TransformUsageFlags.Dynamic),
				ett_DL = GetEntity(authoring.ett_DL, TransformUsageFlags.Dynamic),
				ett_DLU = GetEntity(authoring.ett_DLU, TransformUsageFlags.Dynamic),
				ett_L = GetEntity(authoring.ett_L, TransformUsageFlags.Dynamic),
				ett_LR = GetEntity(authoring.ett_LR, TransformUsageFlags.Dynamic),
				ett_LU = GetEntity(authoring.ett_LU, TransformUsageFlags.Dynamic),
				ett_LUR = GetEntity(authoring.ett_LUR, TransformUsageFlags.Dynamic),
				ett_Null = GetEntity(authoring.ett_Null, TransformUsageFlags.Dynamic),
				ett_R = GetEntity(authoring.ett_R, TransformUsageFlags.Dynamic),
				ett_RD = GetEntity(authoring.ett_RD, TransformUsageFlags.Dynamic),
				ett_RDL = GetEntity(authoring.ett_RDL, TransformUsageFlags.Dynamic),
				ett_U = GetEntity(authoring.ett_U, TransformUsageFlags.Dynamic),
				ett_UD = GetEntity(authoring.ett_UD, TransformUsageFlags.Dynamic),
				ett_UR = GetEntity(authoring.ett_UR, TransformUsageFlags.Dynamic),
				ett_URD = GetEntity(authoring.ett_URD, TransformUsageFlags.Dynamic),
				ett_CornerDL = GetEntity(authoring.ett_CornerDL, TransformUsageFlags.Dynamic),
				ett_CornerLU = GetEntity(authoring.ett_CornerLU, TransformUsageFlags.Dynamic),
				ett_CornerRD = GetEntity(authoring.ett_CornerRD, TransformUsageFlags.Dynamic),
				ett_CornerUR = GetEntity(authoring.ett_CornerUR, TransformUsageFlags.Dynamic),
				ett_NoCornerUR = GetEntity(authoring.ett_NoCornerUR, TransformUsageFlags.Dynamic),
				ett_NoCornerRD = GetEntity(authoring.ett_NoCornerRD, TransformUsageFlags.Dynamic),
				ett_NoCornerDL = GetEntity(authoring.ett_NoCornerDL, TransformUsageFlags.Dynamic),
				ett_NoCornerLU = GetEntity(authoring.ett_NoCornerLU, TransformUsageFlags.Dynamic)
			};
			AddComponent(entity, in component);
		}
	}

	public GameObject ett_D;

	public GameObject ett_DL;

	public GameObject ett_DLU;

	public GameObject ett_L;

	public GameObject ett_LR;

	public GameObject ett_LU;

	public GameObject ett_LUR;

	public GameObject ett_Null;

	public GameObject ett_R;

	public GameObject ett_RD;

	public GameObject ett_RDL;

	public GameObject ett_U;

	public GameObject ett_UD;

	public GameObject ett_UR;

	public GameObject ett_URD;

	[Header("Corner")]
	public GameObject ett_CornerUR;

	public GameObject ett_CornerRD;

	public GameObject ett_CornerDL;

	public GameObject ett_CornerLU;

	public GameObject ett_NoCornerUR;

	public GameObject ett_NoCornerRD;

	public GameObject ett_NoCornerDL;

	public GameObject ett_NoCornerLU;
}
