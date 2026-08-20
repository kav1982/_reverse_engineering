using Unity.Entities;
using UnityEngine;

public class SpecialObj19Authoring : MonoBehaviour
{
	private class Baker : Baker<SpecialObj19Authoring>
	{
		public override void Bake(SpecialObj19Authoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			SpecialObj19_Dots component = new SpecialObj19_Dots
			{
				type = authoring.type,
				ett_Layer = GetEntity(authoring.ett_Layer, TransformUsageFlags.Dynamic),
				offset = authoring.offset
			};
			AddComponent(entity, in component);
		}
	}

	public LayerCorrectType type;

	public GameObject ett_Layer;

	public float offset;
}
