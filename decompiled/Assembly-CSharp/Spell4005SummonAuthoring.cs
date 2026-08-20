using Unity.Entities;
using UnityEngine;

internal class Spell4005SummonAuthoring : MonoBehaviour
{
	private class Spell4005SummonAuthoringBaker : Baker<Spell4005SummonAuthoring>
	{
		public override void Bake(Spell4005SummonAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Spell4005WandSpiritData component = new Spell4005WandSpiritData
			{
				IsInitialized = false,
				State = WandSpiritState.Idle
			};
			AddComponent(entity, in component);
			AddBuffer<UnitKeepCastingSpells>(entity);
			AddComponent<TeammateDisableSpellMovementTag>(entity);
			SetComponentEnabled<TeammateDisableSpellMovementTag>(entity, enabled: true);
		}
	}
}
