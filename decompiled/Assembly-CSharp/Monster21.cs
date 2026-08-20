using Unity.Transforms;
using UnityEngine;

public class Monster21 : UnitBase
{
	private enum MonsterState
	{
		BornIdle,
		Idle,
		MoveToTarget,
		RandomMove,
		Blink,
		SummonFly,
		Avoid
	}

	public AIPattern pattern;

	public VariableFloat maxAngleDuration;

	public float moveAngleOffset;

	public float moveAngleOffsetSpeed;

	private Vector3 randomMoveTrackPoint;

	[Header("Pattern2 瞬移")]
	public VariableFloat blinkInterval;

	public float blinkToPlayerBackAngle;

	public Transform tsf_BlinkEF;

	[Header("Pattern3,4侧闪")]
	public float avoidSpeedFixer;

	public float avoidAngleOffset;

	public VariableFloat avoidCheckInterval;

	public float avoidTime;

	public ParticleSystem mirageParticle;

	public float mirageExtraTime;

	[Header("Invincible")]
	public bool canInvincible;

	public float checkInvincibleInterval;

	public float invincibleDistance;

	public ParticleSystem ps_Vincible;

	public MeshRenderer mr;

	public MeshRenderer mr_Blink;

	public Sprite sprite_Normal;

	public Material mat_Normal;

	public ParticleSystemRenderer psr_Mirage;

	public Sprite sprite_Invincible;

	public Material mat_Invincible;

	[Header("和谐模式")]
	public Sprite sprite_Normal_H;

	public Material mat_Normal_H;

	public Sprite sprite_Invincible_H;

	public Material mat_Invincible_H;

	private MonsterState state;

	private bool angleToLeft = true;

	private float angleCounter;

	private float maxAngleDurationTimer;

	private Vector3 blinkPoint;

	private float blinkIntervalTimer;

	private GameObject blinkEF;

	private ParticleSystem.MainModule mainModule;

	private MonsterState preState;

	private MonsterState tempState;

	private float avoidCheckIntervalTimer;

	private float avoidTimer;

	private float mirageExtraTimer;

	private bool changedState;

	private Vector3 targetPointAvoid;

	private float checkInvincibleIntervalTimer;

	private float originalFrozenTimeRatio;

	private bool nowInvincible;

	public override void SingleInitialCallback()
	{
		if (pattern == AIPattern.Pattern3 || pattern == AIPattern.Pattern4)
		{
			mainModule = mirageParticle.main;
		}
		originalFrozenTimeRatio = myPpt.unitCfg.frozenTimeRatio;
		if (GameMgr.IsHarmony_Static)
		{
			sprite_Normal = sprite_Normal_H;
			sprite_Invincible = sprite_Invincible_H;
			mr.material.SetTexture(GameConstManaged.shaderTextureIndex, sprite_Normal.texture);
			if (pattern > AIPattern.Pattern2)
			{
				mat_Normal = mat_Normal_H;
				mat_Invincible = mat_Invincible_H;
				Object.Destroy(psr_Mirage.material);
				psr_Mirage.material = mat_Normal;
			}
			if (mr_Blink != null)
			{
				mr_Blink.material.SetTexture(GameConstManaged.shaderTextureIndex, sprite_Normal.texture);
			}
		}
	}

	public override void EveryInitialCallback()
	{
		state = MonsterState.BornIdle;
		angleToLeft = true;
		angleCounter = 0f;
		maxAngleDurationTimer = 0f;
		base.Anima.SetTrigger("Idle");
		blinkInterval.RandomResult();
		if (pattern == AIPattern.Pattern3 || pattern == AIPattern.Pattern4)
		{
			avoidCheckIntervalTimer = 0f;
			avoidTimer = 0f;
			mirageExtraTimer = 0f;
			mirageParticle.Stop();
			mirageParticle.Clear();
		}
		if (canInvincible)
		{
			checkInvincibleIntervalTimer = 0f;
			mr.material.SetTexture(GameConstManaged.shaderTextureIndex, sprite_Normal.texture);
			if (mr_Blink != null)
			{
				mr_Blink.material.SetTexture(GameConstManaged.shaderTextureIndex, sprite_Normal.texture);
			}
			base.gameObject.tag = "Monster";
			base.gameObject.layer = LayerMask.NameToLayer("Monster_Ghost");
			SetDotsLayer(8192u);
			nowInvincible = false;
		}
		if (pattern == AIPattern.Pattern2 || pattern == AIPattern.Pattern4)
		{
			tsf_BlinkEF.gameObject.SetActive(value: false);
		}
	}

	public override void Update()
	{
		if (pattern == AIPattern.Pattern3 || pattern == AIPattern.Pattern4)
		{
			if (base.IsFlipped)
			{
				psr_Mirage.flip = new Vector3(1f, 0f, 0f);
			}
			else if (!base.IsFlipped)
			{
				psr_Mirage.flip = new Vector3(0f, 0f, 0f);
			}
		}
		if (canInvincible)
		{
			checkInvincibleIntervalTimer += Time.deltaTime;
			if (checkInvincibleIntervalTimer >= checkInvincibleInterval)
			{
				checkInvincibleIntervalTimer = 0f;
				bool flag = false;
				if (Tool2D.IgnoreZDistanceSqr(base.transform.position, PlayerMgr.Inst.PlayerPoint) < invincibleDistance * invincibleDistance)
				{
					flag = true;
				}
				bool flag2 = false;
				if (!flag && UnitDotsSyncSystem.HaveCollider(base.transform.position, invincibleDistance, GameConst.Filter_Friendly))
				{
					flag2 = true;
				}
				if (flag2 || flag)
				{
					if (nowInvincible)
					{
						nowInvincible = false;
						mr.material.SetTexture(GameConstManaged.shaderTextureIndex, sprite_Normal.texture);
						if (mr_Blink != null)
						{
							mr_Blink.material.SetTexture(GameConstManaged.shaderTextureIndex, sprite_Normal.texture);
						}
						ps_Vincible.Play();
						UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
						componentData.unitCfg.frozenTimeRatio = originalFrozenTimeRatio;
						componentData.InvincibleUnregister();
						SetComponentData(componentData);
						base.gameObject.tag = "Monster";
						base.gameObject.layer = LayerMask.NameToLayer("Monster_Ghost");
						SetDotsLayer(8192u);
					}
				}
				else if (!nowInvincible)
				{
					nowInvincible = true;
					mr.material.SetTexture(GameConstManaged.shaderTextureIndex, sprite_Invincible.texture);
					if (mr_Blink != null)
					{
						mr_Blink.material.SetTexture(GameConstManaged.shaderTextureIndex, sprite_Invincible.texture);
					}
					UnitProperty_Dots componentData2 = GetComponentData<UnitProperty_Dots>();
					componentData2.unitCfg.frozenTimeRatio = 0f;
					componentData2.InvincibleRegister();
					SetComponentData(componentData2);
					base.gameObject.tag = "Untagged";
					base.gameObject.layer = LayerMask.NameToLayer("Invisible");
					SetDotsLayer(128u);
				}
			}
		}
		base.Update();
		if (base.IsLocked)
		{
			return;
		}
		changedState = false;
		preState = tempState;
		tempState = state;
		if (preState != state)
		{
			changedState = true;
		}
		if (pattern == AIPattern.Pattern3 || pattern == AIPattern.Pattern4)
		{
			if (mirageExtraTime < mirageExtraTimer)
			{
				if (mirageParticle.isPlaying && state != MonsterState.Avoid)
				{
					mirageParticle.Stop();
				}
			}
			else
			{
				mirageExtraTimer += Time.deltaTime;
			}
		}
		switch (state)
		{
		case MonsterState.BornIdle:
			SetMove(Vector3.zero);
			bornIdleTimer += Time.deltaTime;
			if (bornIdleTimer >= 0.5f)
			{
				state = MonsterState.MoveToTarget;
			}
			break;
		case MonsterState.Idle:
			if (changedState)
			{
				base.Anima.SetTrigger("Walk");
				checkTargetIntervalTimer = 0f;
			}
			SetMove(Vector3.zero);
			break;
		case MonsterState.RandomMove:
			if (changedState)
			{
				base.Anima.SetTrigger("Walk");
				maxAngleDuration.RandomResult();
				angleToLeft = ((Random.Range(0, 2) == 0) ? true : false);
				GetNearestTarget();
				blinkIntervalTimer = 0f;
				avoidCheckInterval.RandomResult();
				Vector3 centerPoint2 = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint;
				Vector3 vector2 = LevelMgr.Inst.CurrentRoomCtrller.RoomScale * 0.25f;
				randomMoveTrackPoint = new Vector3((float)Random.Range(-1, 1) * vector2.x, (float)Random.Range(-1, 1) * vector2.y, 0f) + centerPoint2;
			}
			checkTargetIntervalTimer += Time.deltaTime;
			if (checkTargetIntervalTimer >= 1f)
			{
				checkTargetIntervalTimer = 0f;
				GetNearestTarget();
				if (base.HaveTarget)
				{
					state = MonsterState.MoveToTarget;
				}
			}
			if ((base.transform.position - randomMoveTrackPoint).sqrMagnitude < 2f)
			{
				Vector3 centerPoint3 = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint;
				Vector3 vector3 = LevelMgr.Inst.CurrentRoomCtrller.RoomScale * 0.25f;
				randomMoveTrackPoint = new Vector3((float)Random.Range(-1, 1) * vector3.x, (float)Random.Range(-1, 1) * vector3.y, 0f) + centerPoint3;
			}
			if (angleToLeft)
			{
				angleCounter -= moveAngleOffsetSpeed * Time.deltaTime;
				if (angleCounter < 0f - moveAngleOffset)
				{
					angleCounter = 0f - moveAngleOffset;
				}
				if (angleCounter == 0f - moveAngleOffset)
				{
					maxAngleDurationTimer += Time.deltaTime;
					if (maxAngleDurationTimer >= maxAngleDuration.result)
					{
						maxAngleDurationTimer = 0f;
						angleToLeft = false;
					}
				}
			}
			else
			{
				angleCounter += moveAngleOffsetSpeed * Time.deltaTime;
				if (angleCounter > moveAngleOffset)
				{
					angleCounter = moveAngleOffset;
				}
				if (angleCounter == moveAngleOffset)
				{
					maxAngleDurationTimer += Time.deltaTime;
					if (maxAngleDurationTimer >= maxAngleDuration.result)
					{
						maxAngleDurationTimer = 0f;
						angleToLeft = true;
					}
				}
			}
			SetMove(ToPointDir(randomMoveTrackPoint, angleCounter) * base.MoveSpeed);
			SetFlip(ToPointDir(randomMoveTrackPoint).x);
			break;
		case MonsterState.MoveToTarget:
			if (changedState)
			{
				base.Anima.SetTrigger("Walk");
				maxAngleDuration.RandomResult();
				angleToLeft = ((Random.Range(0, 2) == 0) ? true : false);
				GetNearestTarget();
				blinkIntervalTimer = 0f;
				avoidCheckInterval.RandomResult();
			}
			if (!base.HaveTarget)
			{
				GetNearestTarget();
			}
			if (!base.HaveTarget)
			{
				state = MonsterState.RandomMove;
				break;
			}
			if (angleToLeft)
			{
				angleCounter -= moveAngleOffsetSpeed * Time.deltaTime;
				if (angleCounter < 0f - moveAngleOffset)
				{
					angleCounter = 0f - moveAngleOffset;
				}
				if (angleCounter == 0f - moveAngleOffset)
				{
					maxAngleDurationTimer += Time.deltaTime;
					if (maxAngleDurationTimer >= maxAngleDuration.result)
					{
						maxAngleDurationTimer = 0f;
						angleToLeft = false;
					}
				}
			}
			else
			{
				angleCounter += moveAngleOffsetSpeed * Time.deltaTime;
				if (angleCounter > moveAngleOffset)
				{
					angleCounter = moveAngleOffset;
				}
				if (angleCounter == moveAngleOffset)
				{
					maxAngleDurationTimer += Time.deltaTime;
					if (maxAngleDurationTimer >= maxAngleDuration.result)
					{
						maxAngleDurationTimer = 0f;
						angleToLeft = true;
					}
				}
			}
			if (ToTargetDistanceSqr() > 0.040000003f)
			{
				SetMove(ToTargetDir(angleCounter) * base.MoveSpeed);
			}
			else
			{
				SetMove(Vector3.zero, isFlip: false);
			}
			SetFlip(ToTargetDir().x);
			if (pattern == AIPattern.Pattern2 || pattern == AIPattern.Pattern4)
			{
				blinkIntervalTimer += Time.deltaTime;
				if (blinkIntervalTimer >= blinkInterval.result)
				{
					blinkInterval.RandomResult();
					state = MonsterState.Blink;
					break;
				}
			}
			if (pattern == AIPattern.Pattern3 || pattern == AIPattern.Pattern4)
			{
				avoidCheckIntervalTimer += Time.deltaTime;
				if (avoidCheckIntervalTimer >= avoidCheckInterval.result)
				{
					mirageExtraTimer = 0f;
					avoidCheckIntervalTimer = 0f;
					avoidCheckInterval.RandomResult();
					state = MonsterState.Avoid;
				}
			}
			break;
		case MonsterState.Avoid:
			if (changedState)
			{
				if (!base.HaveTarget)
				{
					state = MonsterState.MoveToTarget;
					break;
				}
				targetPointAvoid = base.TargetPointIgnoreZ;
				SEMgr.Inst.monster7Trace.PlaySE();
				mirageParticle.Play();
				avoidTimer = 0f;
			}
			avoidTimer += Time.deltaTime;
			SetMove(Tool2D.GetDir(targetPointAvoid - base.transform.position, (float)((!angleToLeft) ? 1 : (-1)) * avoidAngleOffset).normalized * base.MoveSpeed * avoidSpeedFixer);
			if (avoidTimer >= avoidTime)
			{
				avoidTimer = 0f;
				state = MonsterState.MoveToTarget;
			}
			break;
		case MonsterState.Blink:
			if (changedState)
			{
				if (!base.HaveTarget)
				{
					GetNearestTarget();
				}
				if (!base.HaveTarget)
				{
					state = MonsterState.MoveToTarget;
					break;
				}
				if (base.HaveTarget)
				{
					base.Anima.SetTrigger("Blink");
					blinkPoint = base.TargetPoint + ToTargetDir(Random.Range((0f - blinkToPlayerBackAngle) / 2f, blinkToPlayerBackAngle / 2f)) * ToTargetDistance();
					Vector3 centerPoint = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint;
					Vector3 vector = LevelMgr.Inst.CurrentRoomCtrller.RoomScale * 1.2f;
					blinkPoint.x = Mathf.Min(blinkPoint.x, centerPoint.x + vector.x);
					blinkPoint.y = Mathf.Min(blinkPoint.y, centerPoint.y + vector.y);
					blinkPoint.x = Mathf.Max(blinkPoint.x, centerPoint.x - vector.x);
					blinkPoint.y = Mathf.Max(blinkPoint.y, centerPoint.y - vector.y);
					tsf_BlinkEF.gameObject.SetActive(value: true);
				}
			}
			tsf_BlinkEF.position = Tool2D.IgnoreZPoint(blinkPoint);
			SetMove(Vector3.zero);
			break;
		case MonsterState.SummonFly:
			SetMove(Vector3.zero);
			if (base.CurrentMotion.magnitude <= 0.01f)
			{
				state = MonsterState.MoveToTarget;
			}
			break;
		default:
			Debug.LogError(state);
			break;
		}
	}

	public override void AnimaAction(string animaName)
	{
		if (!(animaName == "Blink"))
		{
			if (animaName == "BlinkFinish")
			{
				state = MonsterState.MoveToTarget;
				base.Anima.SetTrigger("Walk");
			}
			else
			{
				Debug.LogError(animaName);
			}
		}
		else
		{
			tsf_BlinkEF.gameObject.SetActive(value: false);
			base.transform.position = blinkPoint;
			LocalTransform componentData = GetComponentData<LocalTransform>();
			componentData.Position = base.transform.position;
			SetComponentData(componentData);
			SEMgr.Inst.monster7Blink.PlaySE();
		}
	}

	public override void AfterDead(ref TakeDamageInfo_Dots info)
	{
		base.AfterDead(ref info);
		if (blinkEF != null && blinkEF.activeSelf)
		{
			blinkEF.SetActive(value: false);
		}
	}

	public void SummonFly(Vector3 force)
	{
		state = MonsterState.SummonFly;
		base.CurrentMotion = force;
	}
}
