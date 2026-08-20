using Unity.Entities;
using UnityEngine;

public class Spell1002RollBallAuthoring : MonoBehaviour
{
	private class Spell1002RollBallBaker : Baker<Spell1002RollBallAuthoring>
	{
		public override void Bake(Spell1002RollBallAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Spell1002RollBallBeHitTimer component = default(Spell1002RollBallBeHitTimer);
			AddComponent(entity, in component);
			Spell1002CreateLiquid component2 = default(Spell1002CreateLiquid);
			AddComponent(entity, in component2);
			AddComponent<Spell1002InitializeTag>(entity);
			SetComponentEnabled<Spell1002InitializeTag>(entity, enabled: true);
			AddComponent<Spell1002RollBallFallToAbyssTag>(entity);
			SetComponentEnabled<Spell1002RollBallFallToAbyssTag>(entity, enabled: false);
		}
	}
}
