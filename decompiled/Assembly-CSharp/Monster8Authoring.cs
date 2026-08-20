using Unity.Entities;
using UnityEngine;

public class Monster8Authoring : MonoBehaviour
{
	private class Baker : Baker<Monster8Authoring>
	{
		public override void Bake(Monster8Authoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Monster8_Dots component = new Monster8_Dots
			{
				moveThreshold = authoring.moveThreshold,
				checkRadius = authoring.checkRadius,
				amazeRadius = authoring.amazeRadius,
				idleTime = authoring.idleTime,
				randomWalkTime = authoring.randomWalkTime,
				randomWalkRadius = authoring.randomWalkRadius,
				followTargetChance = authoring.followTargetChance,
				followTargetTime = authoring.followTargetTime,
				state = Monster8State.BornIdle
			};
			AddComponent(entity, in component);
			Monster8_Dots_Amaze component2 = default(Monster8_Dots_Amaze);
			AddComponent(entity, in component2);
		}
	}

	public float moveThreshold;

	public float checkRadius;

	public float amazeRadius;

	public RandomFloat idleTime;

	public RandomFloat randomWalkTime;

	public RandomFloat randomWalkRadius;

	[Range(0f, 1f)]
	public float followTargetChance;

	public float followTargetTime;
}
