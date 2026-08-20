using Unity.Entities;
using UnityEngine;

public class Monster5Authoring : MonoBehaviour
{
	private enum MonsterState
	{
		BornIdle,
		AroundSimilar,
		AroundObj,
		NoAround
	}

	private class Baker : Baker<Monster5Authoring>
	{
		public override void Bake(Monster5Authoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Monster5_Dots component = new Monster5_Dots
			{
				state = Monster5State.BornIdle,
				aroundAdjustmentDistance = authoring.aroundAdjustmentDistance,
				aroundPlayerRadius = authoring.aroundPlayerRadius,
				aroundPointDistance = authoring.aroundPointDistance,
				isAroundPlayer = authoring.isAroundPlayer,
				sprintInterval = authoring.sprintInterval,
				sprintTime = authoring.sprintTime,
				sprintSpeedRatio = authoring.sprintSpeedRatio,
				noAroundRotateSpeed = authoring.noAroundRotateSpeed
			};
			AddComponent(entity, in component);
		}
	}

	public float aroundCheckInterval;

	public float aroundAdjustmentDistance;

	public float aroundPointDistance;

	public float noAroundRotateSpeed;

	[Header("Sprint")]
	public float sprintInterval;

	public float sprintTime;

	public float sprintSpeedRatio;

	public AIPattern pattern;

	[Header("Pattern2 Pattern3")]
	public VariableFloat attackInterval;

	public float attackDistance;

	[Header("Spell")]
	public float spellHieght;

	public float spellSpeed;

	public float spellDuration;

	[Header("AroundPlayer")]
	public bool isAroundPlayer;

	public float aroundPlayerRadius;
}
