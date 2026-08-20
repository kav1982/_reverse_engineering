using Unity.Entities;
using UnityEngine;

public class Spell1021MagicBreakerAuthoring : MonoBehaviour
{
	public class Baker : Baker<Spell1021MagicBreakerAuthoring>
	{
		public override void Bake(Spell1021MagicBreakerAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Spell1021InitEffectTag component = default(Spell1021InitEffectTag);
			AddComponent(entity, in component);
			Spell1021MagicBreakerData component2 = new Spell1021MagicBreakerData
			{
				FlipY = true
			};
			AddComponent(entity, in component2);
		}
	}
}
