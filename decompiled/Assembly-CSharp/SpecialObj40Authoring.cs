using Unity.Entities;
using UnityEngine;

public class SpecialObj40Authoring : MonoBehaviour
{
	private class Baker : Baker<SpecialObj40Authoring>
	{
		public override void Bake(SpecialObj40Authoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			SpecialObj40_Dots component = new SpecialObj40_Dots
			{
				ett_Anima = GetEntity(authoring.ett_Anima, TransformUsageFlags.Dynamic),
				tipsCountPC = authoring.tipsCountPC,
				tipsCountMobile = authoring.tipsCountMobile
			};
			AddComponent(entity, in component);
		}
	}

	public GameObject ett_Anima;

	public int tipsCountPC;

	public int tipsCountMobile;
}
