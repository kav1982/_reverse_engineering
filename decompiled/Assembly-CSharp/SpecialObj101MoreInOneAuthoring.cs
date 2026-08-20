using Unity.Entities;
using UnityEngine;

public class SpecialObj101MoreInOneAuthoring : MonoBehaviour
{
	private class Baker : Baker<SpecialObj101MoreInOneAuthoring>
	{
		public override void Bake(SpecialObj101MoreInOneAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			SpecialObj101MoreInOne_Dots component = new SpecialObj101MoreInOne_Dots
			{
				ett_Effect = GetEntity(authoring.ett_Effect, TransformUsageFlags.Dynamic)
			};
			AddComponent(entity, in component);
		}
	}

	public GameObject ett_Effect;
}
