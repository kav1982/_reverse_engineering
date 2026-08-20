using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

internal class Spell2001SummonBoBoAuthoring : MonoBehaviour
{
	private class Spell2001SummonBoBoAuthoringBaker : Baker<Spell2001SummonBoBoAuthoring>
	{
		public override void Bake(Spell2001SummonBoBoAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Spell2001BoBoData component = new Spell2001BoBoData
			{
				AttackIntervalTimer = 0f,
				AttackCoolDownTimer = 0f,
				NormalBulletLeft = 0,
				State = Spell2001State.Initialize,
				AttackRange = 0f,
				TargetEntityLastFramePosition = default(float3),
				AttackMouseOpenAnimeTimer = 0f,
				BodyAnimaTimer = 0f,
				AfterAttackCoolDownTimer = 0f,
				BoBoBombReady = false,
				fakeMoveTimer = 0f
			};
			AddComponent(entity, in component);
		}
	}
}
