using Unity.Entities;
using UnityEngine;

internal class Spell2006Authoring : MonoBehaviour
{
	private class Spell2006AuthoringBaker : Baker<Spell2006Authoring>
	{
		public override void Bake(Spell2006Authoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Spell2006Data component = new Spell2006Data
			{
				IsInitialized = false,
				CurrentState = Teammate6State.Idle,
				IdleTimer = 0f,
				IdleInterval = Random.Range(1f, 2f),
				IdleWalkDuration = Random.Range(2f, 3f),
				HookDetectRange = 20f,
				SBDecreaseRadiusToDamageRatio = 1f
			};
			AddComponent(entity, in component);
			AddComponent<Spell2006GhostTag>(entity);
			SetComponentEnabled<Spell2006GhostTag>(entity, enabled: false);
			AddComponent<Spell2006FuseTag>(entity);
			SetComponentEnabled<Spell2006FuseTag>(entity, enabled: false);
		}
	}
}
