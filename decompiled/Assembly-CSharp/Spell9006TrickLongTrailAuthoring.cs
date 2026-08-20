using Unity.Entities;
using UnityEngine;

public class Spell9006TrickLongTrailAuthoring : MonoBehaviour
{
	private class Spell9006TrickLongTrailSystemBaker : Baker<Spell9006TrickLongTrailAuthoring>
	{
		public override void Bake(Spell9006TrickLongTrailAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Spell9006TrickLongTrailData component = default(Spell9006TrickLongTrailData);
			AddComponent(entity, in component);
		}
	}
}
