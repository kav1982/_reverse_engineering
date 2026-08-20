using Unity.Entities;
using UnityEngine;

internal class SpecialObj400Authoring : MonoBehaviour
{
	private class SpecialObj400Baker : Baker<SpecialObj400Authoring>
	{
		public override void Bake(SpecialObj400Authoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			SpecialObj400Data component = new SpecialObj400Data
			{
				Type = authoring.Type
			};
			AddComponent(entity, in component);
		}
	}

	public EndlessCampTeleporterType Type;
}
