using Unity.Entities;
using UnityEngine;

public class Spell9003LongTrailAuthoring : MonoBehaviour
{
	private class Baker : Baker<Spell9003LongTrailAuthoring>
	{
		public override void Bake(Spell9003LongTrailAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Spell9003LongTrailData component = new Spell9003LongTrailData
			{
				InitOver = false
			};
			AddComponent(entity, in component);
		}
	}
}
