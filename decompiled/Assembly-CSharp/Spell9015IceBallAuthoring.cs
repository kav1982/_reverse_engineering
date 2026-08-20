using Unity.Entities;
using UnityEngine;

public class Spell9015IceBallAuthoring : MonoBehaviour
{
	private class Spell9015IceBallAuthoringBaker : Baker<Spell9015IceBallAuthoring>
	{
		public override void Bake(Spell9015IceBallAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Spell9015IceBallData component = default(Spell9015IceBallData);
			AddComponent(entity, in component);
		}
	}
}
