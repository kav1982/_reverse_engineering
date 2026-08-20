using Unity.Entities;
using UnityEngine;

internal class Spell2005SummonAuthoring : MonoBehaviour
{
	private class Spell2005SummonAuthoringBaker : Baker<Spell2005SummonAuthoring>
	{
		public override void Bake(Spell2005SummonAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Spell2005GrimoireData component = new Spell2005GrimoireData
			{
				ManaRegenPerSecond = 0f,
				MaxMpCapacity = 0f,
				CurrentMp = 0f,
				AttackDuration = 0f,
				AttackTimer = 0f,
				AttackRange = 0f,
				AnimationTimer = 0f,
				BookFloatingTimer = 0f,
				CurrentBaseHeight = 0f,
				IsRotation = false,
				IsLowCostSpell = false,
				ReadyToAttack = false,
				ShootRecoil = 0f,
				CloseBookTimer = 0f,
				UpdateChaseTargetTimer = 0f,
				SpellCastCounter = 0,
				State = Spell2005State.Initialize,
				TeleportCoolDownTimer = 0f,
				TeleportProgressTimer = 0f,
				IsChildTeammateReachLimit = false,
				ReleaseChargeSpell = false,
				ReleaseChargeDuration = 0f,
				ReleaseChargeTimer = 0f
			};
			AddComponent(entity, in component);
		}
	}
}
