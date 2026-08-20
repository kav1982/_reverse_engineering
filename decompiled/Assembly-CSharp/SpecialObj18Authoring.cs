using Unity.Entities;
using UnityEngine;

public class SpecialObj18Authoring : MonoBehaviour
{
	private class Baker : Baker<SpecialObj18Authoring>
	{
		public override void Bake(SpecialObj18Authoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			SpecialObj18 component = new SpecialObj18
			{
				ett_Normal = GetEntity(authoring.ett_Normal, TransformUsageFlags.Dynamic),
				ett_Used = GetEntity(authoring.ett_Used, TransformUsageFlags.Dynamic)
			};
			AddComponent(entity, in component);
		}
	}

	public GameObject ett_Normal;

	public GameObject ett_Used;
}
