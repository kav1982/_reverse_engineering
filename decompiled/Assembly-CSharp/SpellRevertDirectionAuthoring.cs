using Unity.Entities;
using UnityEngine;

public class SpellRevertDirectionAuthoring : MonoBehaviour
{
	private class Baker : Baker<SpellRevertDirectionAuthoring>
	{
		public override void Bake(SpellRevertDirectionAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			SpellRevertDirection component = default(SpellRevertDirection);
			AddComponent(entity, in component);
		}
	}
}
