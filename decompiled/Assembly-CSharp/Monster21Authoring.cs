using Unity.Entities;
using UnityEngine;

public class Monster21Authoring : MonoBehaviour
{
	public class Baker : Baker<Monster21Authoring>
	{
		public override void Bake(Monster21Authoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Monster21_Dots component = new Monster21_Dots
			{
				pattern = authoring.pattern,
				maxAngleDuration = authoring.maxAngleDuration,
				moveAngleOffset = authoring.moveAngleOffset,
				moveAngleOffsetSpeed = authoring.moveAngleOffsetSpeed,
				blinkInterval = authoring.blinkInterval,
				blinkToPlayerBackAngle = authoring.blinkToPlayerBackAngle,
				state = Monster21State.BornIdle
			};
			AddComponent(entity, in component);
		}
	}

	public AIPattern pattern;

	public RandomFloat maxAngleDuration;

	public float moveAngleOffset;

	public float moveAngleOffsetSpeed;

	private Vector3 randomMoveTrackPoint;

	[Header("Pattern2 瞬移")]
	public RandomFloat blinkInterval;

	public float blinkToPlayerBackAngle;

	public Transform tsf_BlinkEF;
}
