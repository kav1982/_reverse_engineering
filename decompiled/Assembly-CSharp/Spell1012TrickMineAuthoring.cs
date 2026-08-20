using Unity.Entities;
using UnityEngine;

internal class Spell1012TrickMineAuthoring : MonoBehaviour
{
	private class Spell1012TrickMineAuthoringBaker : Baker<Spell1012TrickMineAuthoring>
	{
		public override void Bake(Spell1012TrickMineAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Spell1012TrickMineData component = new Spell1012TrickMineData
			{
				ChainExplosionImmuteTimer = 0.2f,
				IsInitialize = false,
				EndingFlashEnable = false,
				IsDenoteByOtherTrickMine = false
			};
			AddComponent(entity, in component);
			AddComponent<Spell1012TrickmineFallToAbyssTag>(entity);
			SetComponentEnabled<Spell1012TrickmineFallToAbyssTag>(entity, enabled: false);
		}
	}
}
