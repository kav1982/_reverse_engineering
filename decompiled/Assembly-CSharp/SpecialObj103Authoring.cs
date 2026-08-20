using Unity.Entities;
using UnityEngine;

public class SpecialObj103Authoring : MonoBehaviour
{
	private class Baker : Baker<SpecialObj103Authoring>
	{
		public override void Bake(SpecialObj103Authoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			SpecialObj103 component = new SpecialObj103
			{
				type = authoring.type,
				spaceX = authoring.spaceX,
				spaceY = authoring.spaceY
			};
			AddComponent(entity, in component);
		}
	}

	public SO103Type type;

	public float spaceX;

	public float spaceY;
}
