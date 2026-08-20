using Unity.Entities;
using UnityEngine;

public class Spell4013RuneHammerAuthoring : MonoBehaviour
{
	private class Spell4013RuneHammerAuthoringBaker : Baker<Spell4013RuneHammerAuthoring>
	{
		public override void Bake(Spell4013RuneHammerAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Spell4013RuneHammerInitTag component = default(Spell4013RuneHammerInitTag);
			AddComponent(entity, in component);
			Spell4013RuneHammerData component2 = default(Spell4013RuneHammerData);
			AddComponent(entity, in component2);
		}
	}
}
