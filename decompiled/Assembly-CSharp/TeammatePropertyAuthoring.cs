using Unity.Entities;
using UnityEngine;

internal class TeammatePropertyAuthoring : MonoBehaviour
{
	private class TeammatePropertyAuthoringBaker : Baker<TeammatePropertyAuthoring>
	{
		public override void Bake(TeammatePropertyAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			TeammateData component = new TeammateData
			{
				TeammateSpeedRatio = 1f,
				AdvanceSkillLevel = 0,
				TeammateHpRecoverAmountPerSecond = 0f,
				TeammateHpDropAmountPerSecond = 0f,
				TeammateHpEffectCalculateTimer = 0f,
				SeparateCalculateHpEffect = false,
				OnDeathSpawnWormCount = 0,
				TeammateCurrentFuseLevel = 0,
				TeammateMaxFuseLevel = 0,
				TeammateDelayDeathTime = 0f,
				TeammateDelayDeathEffectActive = false,
				IsFuseMaterial = false,
				SummonFollowOwnerThroughMapChance = 0f,
				TeammateSuddenDeathHPThreshold = 0f
			};
			AddComponent(entity, in component);
			AddComponent<TeammateDisableSpellMovementTag>(entity);
			SetComponentEnabled<TeammateDisableSpellMovementTag>(entity, enabled: true);
			AddComponent<TeammateDeadTag>(entity);
			SetComponentEnabled<TeammateDeadTag>(entity, enabled: false);
			AddComponent<SpellSpeedRatioValueData>(entity);
		}
	}

	public bool CreateSummonEffect;
}
