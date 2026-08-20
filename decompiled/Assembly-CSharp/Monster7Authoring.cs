using Unity.Entities;
using UnityEngine;

public class Monster7Authoring : MonoBehaviour
{
	private class Baker : Baker<Monster7Authoring>
	{
		public override void Bake(Monster7Authoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Monster7_Dots component = new Monster7_Dots
			{
				blinkInterval = authoring.blinkInterval,
				blinkToPlayerBackAngle = authoring.blinkToPlayerBackAngle,
				idleTime = authoring.idleTime,
				randomMoveDistance = authoring.randomMoveDistance,
				state = Monster7State.BornIdle
			};
			AddComponent(entity, in component);
		}
	}

	[Space(50f)]
	public RandomFloat blinkInterval;

	public float blinkToPlayerBackAngle;

	public Transform tsf_BlinkEF;

	[Header("空闲")]
	public RandomFloat idleTime;

	public RandomFloat randomMoveDistance;

	private float idleTimer;

	private Vector3 randomMovePoint;

	[Header("SpeedRun")]
	public AIPattern pattern;

	public RandomFloat runCheckInterval;

	private float runCheckTimer;

	public float runTime;

	private float runTimer;

	public float runSpeedFixer;

	public ParticleSystem ps_Mirage;

	public float mirageExtraTime;

	private float mirageExtraTimer;

	private ParticleSystem.MainModule mainModule;

	[Header("Invincible")]
	public bool canInvincible;

	public float checkInvincibleInterval;

	public float invincibleDistance;

	public ParticleSystem ps_Vincible;

	public SpriteRenderer sr;

	public SpriteRenderer sr_Blink;

	public Sprite sprite_Normal;

	public Sprite sprite_Invincible;

	public ParticleSystemRenderer psr_Mirage;

	public Transform ModelTransform;

	[Header("和谐模式")]
	public Sprite sprite_Normal_H;

	public Sprite sprite_Invincible_H;
}
