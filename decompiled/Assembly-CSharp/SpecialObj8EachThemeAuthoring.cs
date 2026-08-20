using Unity.Entities;
using UnityEngine;

public class SpecialObj8EachThemeAuthoring : MonoBehaviour
{
	private class Baker : Baker<SpecialObj8EachThemeAuthoring>
	{
		public override void Bake(SpecialObj8EachThemeAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			SpecialObj8EachTheme component = new SpecialObj8EachTheme
			{
				pfb_801BoxCollider = GetEntity(authoring.pfb_801BoxCollider, TransformUsageFlags.Dynamic),
				ett_Layer = GetEntity(authoring.ett_Layer, TransformUsageFlags.Dynamic),
				ett_CornerURLD = GetEntity(authoring.ett_CornerURLD, TransformUsageFlags.Dynamic),
				ett_CornerRD = GetEntity(authoring.ett_CornerRD, TransformUsageFlags.Dynamic),
				ett_CornerUR = GetEntity(authoring.ett_CornerUR, TransformUsageFlags.Dynamic),
				ett_Full = GetEntity(authoring.ett_Full, TransformUsageFlags.Dynamic),
				ett_LUR = GetEntity(authoring.ett_LUR, TransformUsageFlags.Dynamic),
				ett_RD = GetEntity(authoring.ett_RD, TransformUsageFlags.Dynamic),
				ett_RDL = GetEntity(authoring.ett_RDL, TransformUsageFlags.Dynamic),
				ett_UR = GetEntity(authoring.ett_UR, TransformUsageFlags.Dynamic),
				ett_URD = GetEntity(authoring.ett_URD, TransformUsageFlags.Dynamic)
			};
			AddComponent(entity, in component);
		}
	}

	public GameObject pfb_801BoxCollider;

	public GameObject ett_Layer;

	public GameObject ett_CornerURLD;

	public GameObject ett_CornerRD;

	public GameObject ett_CornerUR;

	public GameObject ett_Full;

	public GameObject ett_LUR;

	public GameObject ett_RD;

	public GameObject ett_RDL;

	public GameObject ett_UR;

	public GameObject ett_URD;
}
