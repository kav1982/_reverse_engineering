using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

internal class Spell2003SummonAuthoring : MonoBehaviour
{
	private class Spell2003SummonAuthoringBaker : Baker<Spell2003SummonAuthoring>
	{
		public override void Bake(Spell2003SummonAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Spell2003TentacleData component = new Spell2003TentacleData
			{
				AttackCoolDownTimer = 0f,
				AttackCoolDownTime = 1f,
				State = Spell2003State.Initialize,
				TargetLastFramePosition = default(float3)
			};
			AddComponent(entity, in component);
			AddBuffer<Spell2003TentacleEffectData>(entity);
		}
	}
}
