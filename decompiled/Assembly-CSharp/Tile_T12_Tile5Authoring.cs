using Unity.Entities;
using UnityEngine;

public class Tile_T12_Tile5Authoring : MonoBehaviour
{
	private class Baker : Baker<Tile_T12_Tile5Authoring>
	{
		public override void Bake(Tile_T12_Tile5Authoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Tile_T12_Tile5_Dots component = new Tile_T12_Tile5_Dots
			{
				ett_DL = GetEntity(authoring.ett_DL, TransformUsageFlags.Dynamic),
				ett_DLU = GetEntity(authoring.ett_DLU, TransformUsageFlags.Dynamic),
				ett_LU = GetEntity(authoring.ett_LU, TransformUsageFlags.Dynamic),
				ett_LUR = GetEntity(authoring.ett_LUR, TransformUsageFlags.Dynamic),
				ett_RD = GetEntity(authoring.ett_RD, TransformUsageFlags.Dynamic),
				ett_RDL = GetEntity(authoring.ett_RDL, TransformUsageFlags.Dynamic),
				ett_UR = GetEntity(authoring.ett_UR, TransformUsageFlags.Dynamic),
				ett_URD = GetEntity(authoring.ett_URD, TransformUsageFlags.Dynamic),
				ett_CornerDL = GetEntity(authoring.ett_CornerDL, TransformUsageFlags.Dynamic),
				ett_CornerLU = GetEntity(authoring.ett_CornerLU, TransformUsageFlags.Dynamic),
				ett_CornerRD = GetEntity(authoring.ett_CornerRD, TransformUsageFlags.Dynamic),
				ett_CornerUR = GetEntity(authoring.ett_CornerUR, TransformUsageFlags.Dynamic)
			};
			AddComponent(entity, in component);
		}
	}

	public GameObject ett_DL;

	public GameObject ett_DLU;

	public GameObject ett_LU;

	public GameObject ett_LUR;

	public GameObject ett_RD;

	public GameObject ett_RDL;

	public GameObject ett_UR;

	public GameObject ett_URD;

	[Header("Corner")]
	public GameObject ett_CornerUR;

	public GameObject ett_CornerRD;

	public GameObject ett_CornerDL;

	public GameObject ett_CornerLU;
}
