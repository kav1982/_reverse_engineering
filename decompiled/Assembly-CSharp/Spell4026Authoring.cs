using Unity.Entities;
using UnityEngine;

internal class Spell4026Authoring : MonoBehaviour
{
	private class Spell4026AuthoringBaker : Baker<Spell4026Authoring>
	{
		public override void Bake(Spell4026Authoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			AddComponent<Spell4026InitializeTag>(entity);
			SetComponentEnabled<Spell4026InitializeTag>(entity, enabled: true);
			Spell4026GreenRuneData component = new Spell4026GreenRuneData
			{
				RuneExplosionDelayDestroyTimer = 0.6f
			};
			AddComponent(entity, in component);
		}
	}
}
