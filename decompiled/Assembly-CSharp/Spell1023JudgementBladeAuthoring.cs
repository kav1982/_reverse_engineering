using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

internal class Spell1023JudgementBladeAuthoring : MonoBehaviour
{
	private class Spell1023JudgementBladeAuthoringBaker : Baker<Spell1023JudgementBladeAuthoring>
	{
		public override void Bake(Spell1023JudgementBladeAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Spell1023JudgementBladeData component = new Spell1023JudgementBladeData
			{
				IsInitialized = false,
				State = JudgementBladeState.Spawn,
				Target = default(Entity),
				TargetLastFramePosition = default(float3),
				OwnerLastFramePosition = default(float3),
				IsBladeInQuery = false,
				LockingTargetTimer = 0f,
				FadeInTimer = 0f,
				FadeOutTimer = 0f,
				LockRotateInClockWise = false,
				BladeLockRotateLerpSpeed = 0f,
				BladeRecheckTargetTimer = 0f,
				LockTargetLookingDirection = new float3(0f, -1f, 0f)
			};
			AddComponent(entity, in component);
		}
	}
}
