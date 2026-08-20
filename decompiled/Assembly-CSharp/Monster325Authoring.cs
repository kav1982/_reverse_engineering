using UnityEngine;

public class Monster325Authoring : MonoBehaviour
{
	[Header("Idle")]
	public RandomFloat idleTime;

	[Header("Random Walk")]
	public RandomFloat randomWalkDistance;

	public RandomFloat randomWalkTime;

	public float randomWalkToTargetAngle = 60f;

	[Header("Close Random Walk")]
	public float closeRandomWalkTriggerDistance = 5f;

	public RandomFloat closeRandomWalkDistance = new RandomFloat(0.8f, 1.8f);

	public float closeRandomWalkToTargetAngle = 25f;

	[Header("移动速度")]
	public float speedRatio = 1f;
}
