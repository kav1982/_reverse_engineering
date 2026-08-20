using Unity.Entities;
using UnityEngine;

public class Spell1017DeathAdderEffectAuthoring : MonoBehaviour
{
	private class Baker : Baker<Spell1017DeathAdderEffectAuthoring>
	{
		public override void Bake(Spell1017DeathAdderEffectAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Spell1017DeathAdderEffectData component = default(Spell1017DeathAdderEffectData);
			AddComponent(entity, in component);
		}
	}
}
