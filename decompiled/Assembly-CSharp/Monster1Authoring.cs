using Unity.Entities;
using UnityEngine;

public class Monster1Authoring : MonoBehaviour
{
	private class Baker : Baker<Monster1Authoring>
	{
		public override void Bake(Monster1Authoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Monster1_Dots component = new Monster1_Dots
			{
				idleTime = authoring.idleTime,
				randomWalkDistance = authoring.randomWalkDistance,
				randomWalkTime = authoring.randomWalkTime,
				followTargetChance = authoring.followTargetChance,
				followTargetDistance = authoring.followTargetDistance,
				followTargetTime = authoring.followTargetTime
			};
			AddComponent(entity, in component);
		}
	}

	public RandomFloat idleTime;

	public RandomFloat randomWalkDistance;

	public RandomFloat randomWalkTime;

	[Range(0f, 1f)]
	public float followTargetChance;

	public float followTargetDistance;

	public float followTargetTime;
}
