using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

public class Elite56GiantBomb : MonoBehaviour
{
	private static readonly int GroundHiddenHeight = Shader.PropertyToID("_GroundHiddenHeight");

	public Transform RotateTransform;

	public SpriteRenderer BombSprite;

	public Text TimerText;

	public RectTransform BombRemainTimerTransform;

	public Transform BombTextTrasnform;

	public ParticleSystem TrailParticle;

	public SpriteRenderer StoneSprite;

	public float BombKnockBack;

	private Vector3 targetPosition;

	private Vector3 FallDirection;

	private Vector3 InitialPosition;

	private float fallDuration;

	private float fallTimer;

	public float BombInitHeight;

	private bool isFalling;

	private Vector3 startPosition;

	private float toTargetDistance;

	public Vector2 BombSpawnToTargetDistance;

	private bool isLand;

	private float fallRadius;

	private float fallDamage;

	private float explosionRadius;

	private float explosionDamage;

	private float explosionWaitTime;

	private bool shootFromRight;

	public ShockParam LandShock;

	public ShockParam ExplosionShock;

	private void OnEnable()
	{
		isFalling = false;
		TrailParticle.Stop();
		BombSprite.enabled = false;
		StoneSprite.enabled = false;
		TrailParticle.Stop();
	}

	public void InitialBomb(Vector3 targetPos, float fallDuration, Vector3 shooterPosition, float fallRadius, float fallDamage, float explosionRadius, float explosionDamage, float explosionWaitTime)
	{
		targetPosition = targetPos.IgnoreZ();
		toTargetDistance = Random.Range(BombSpawnToTargetDistance.x, BombSpawnToTargetDistance.y);
		shootFromRight = shooterPosition.x >= targetPos.x;
		Vector3 vector = (InitialPosition = (shootFromRight ? (targetPosition - new Vector3(0f - toTargetDistance, 0f, 0f)) : (targetPosition - new Vector3(toTargetDistance, 0f, 0f))));
		fallTimer = 0f;
		isFalling = true;
		BombSprite.material.SetFloat(GroundHiddenHeight, targetPos.y);
		this.fallDuration = fallDuration;
		FallDirection = (targetPosition - InitialPosition - new Vector3(0f, BombInitHeight, 0f)).normalized;
		RotateTransform.right = FallDirection;
		isLand = false;
		this.fallRadius = fallRadius;
		this.fallDamage = fallDamage;
		this.explosionWaitTime = explosionWaitTime;
		this.explosionDamage = explosionDamage;
		this.explosionRadius = explosionRadius;
		base.transform.position = InitialPosition + new Vector3(0f, 0f, 0f - BombInitHeight);
		BombSprite.enabled = true;
	}

	public void Update()
	{
		if (!isFalling)
		{
			return;
		}
		float num = Mathf.Min(fallTimer / fallDuration, 1f);
		base.transform.position = InitialPosition + (targetPosition - InitialPosition).normalized * num * toTargetDistance + new Vector3(0f, 0f, (0f - BombInitHeight) * (1f - num));
		fallTimer += Time.deltaTime;
		if (!TrailParticle.isPlaying && !isLand)
		{
			TrailParticle.Play();
		}
		string text = Mathf.FloorToInt(explosionWaitTime - fallTimer).ToString("D2");
		if (text != TimerText.text && fallTimer >= fallDuration && fallTimer <= explosionWaitTime)
		{
			SEMgr.Inst.elite56CounterDown.PlaySE();
		}
		TimerText.text = text;
		if (shootFromRight)
		{
			BombRemainTimerTransform.localScale = new Vector3(0f - Mathf.Abs(BombRemainTimerTransform.localScale.x), 0f - Mathf.Abs(BombRemainTimerTransform.localScale.y), BombRemainTimerTransform.localScale.z);
		}
		else
		{
			BombRemainTimerTransform.localScale = new Vector3(Mathf.Abs(BombRemainTimerTransform.localScale.x), Mathf.Abs(BombRemainTimerTransform.localScale.y), BombRemainTimerTransform.localScale.z);
		}
		if (fallTimer >= fallDuration && !isLand)
		{
			isLand = true;
			SEMgr.Inst.elite56BombLand.PlaySE();
			TrailParticle.Stop(withChildren: true, ParticleSystemStopBehavior.StopEmitting);
			StoneSprite.enabled = true;
			StoneSprite.flipX = !shootFromRight;
			ObjPoolMgr.Inst.GetGO("Prefabs/Mixed/WarningArea_Circle", Tool2D.IgnoreZPoint(targetPosition)).GetComponent<WarningArea>().Initialize(explosionRadius, explosionWaitTime - fallTimer);
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Elite56_BombLand", Tool2D.IgnoreZPoint(targetPosition), 3.2f);
			CamController.Inst.SetShock(LandShock, new Vector3(0f, 1f, 0f));
		}
		if (!(fallTimer >= explosionWaitTime))
		{
			return;
		}
		ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Elite56_BombExplosion", base.transform.position, Quaternion.identity, Vector3.one * explosionRadius / 2f, 5f);
		SEMgr.Inst.elite56BombExplosion.PlaySE();
		List<UnitDotsSyncSystem.DistanceHitResult> list = new List<UnitDotsSyncSystem.DistanceHitResult>();
		UnitDotsSyncSystem.GetCollidersInRange(base.transform.position, explosionRadius, GameConst.Filter_MonsterAoe, list);
		for (int i = 0; i < list.Count; i++)
		{
			UnitDotsSyncSystem.DistanceHitResult distanceHitResult = list[i];
			Entity entity = distanceHitResult.entity;
			switch (UnitDotsSyncSystem.GetLayer(entity))
			{
			case 16777216u:
			{
				UnitDotsSyncSystem.ProcessHitSpell(entity, explosionDamage, out var _);
				break;
			}
			case 512u:
			case 32768u:
			case 131072u:
			case 2097152u:
				if (UnitDotsSyncSystem.HasComponent<UnitProperty_Dots>(distanceHitResult.entity))
				{
					TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(AttackerType.NothingSpecial);
					info.damage = explosionDamage;
					float num2 = 1f - Tool2D.IgnoreZDistance(base.transform.position, distanceHitResult.point) / explosionRadius;
					info.knockbackForce = Tool2D.IgnoreZV2ToV1Normal(distanceHitResult.point, base.transform.position) * BombKnockBack * num2;
					UnitDotsSyncSystem.AddTakeDamageRequest(distanceHitResult.entity, info);
				}
				break;
			}
		}
		isFalling = false;
		CamController.Inst.SetShock(ExplosionShock, new Vector3(0f, -1f, 0f));
		ObjPoolMgr.Inst.RecycleGO(base.gameObject);
	}
}
