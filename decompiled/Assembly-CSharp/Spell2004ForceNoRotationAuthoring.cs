using Unity.Entities;
using UnityEngine;

public class Spell2004ForceNoRotationAuthoring : MonoBehaviour
{
	private class Spell2004ForceNoRotationBaker : Baker<Spell2004ForceNoRotationAuthoring>
	{
		public override void Bake(Spell2004ForceNoRotationAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Spell2004NoRotationTag component = default(Spell2004NoRotationTag);
			AddComponent(entity, in component);
		}
	}
}
