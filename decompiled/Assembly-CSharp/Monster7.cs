using Unity.Transforms;
using UnityEngine;

public class Monster7 : UnitBase
{
	public enum MonsterState
	{
		BornIdle,
		Idle,
		RandomMove,
		RunToTarget,
		SpeedRun,
		Blink
	}

	[Space(50f)]
	public VariableFloat blinkInterval;

	public float blinkToPlayerBackAngle;

	public Transform tsf_BlinkEF;

	[Header("空闲")]
	public VariableFloat idleTime;

	public VariableFloat randomMoveDistance;

	private float idleTimer;

	private Vector3 randomMovePoint;

	[Header("SpeedRun")]
	public AIPattern pattern;

	public VariableFloat runCheckInterval;

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

	public MeshRenderer mr;

	public MeshRenderer mr_Blink;

	public Sprite sprite_Normal;

	public Sprite sprite_Invincible;

	public ParticleSystemRenderer psr_Mirage;

	public Material mat_Normal;

	public Material mat_Invincible;

	public Transform ModelTransform;

	private bool isInvisible;

	[Header("和谐模式")]
	private int shaderPivotIndex = Shader.PropertyToID("_Center");

	public Sprite sprite_Normal_H;

	public Sprite sprite_Invincible_H;

	public Material mat_Normal_H;

	public Material mat_Invincible_H;

	public MonsterState state;

	private Vector3 blinkPoint;

	private float blinkIntervalTimer;

	private float checkInvincibleIntervalTimer;

	private float originalFrozenTimeRatio;

	public override void SingleInitialCallback()
	{
		originalFrozenTimeRatio = myPpt.unitCfg.frozenTimeRatio;
	}

	public override void EveryInitialCallback()
	{
		base.Anima.SetTrigger("Idle");
		state = MonsterState.BornIdle;
		blinkInterval.RandomResult();
		blinkIntervalTimer = Random.Range(0f, blinkInterval.result);
		if (pattern == AIPattern.Pattern2)
		{
			mainModule = ps_Mirage.main;
			ps_Mirage.Clear();
			ps_Mirage.Stop();
			mirageExtraTimer = 0f;
		}
		if (canInvincible)
		{
			checkInvincibleIntervalTimer = 0f;
			mr.material.SetTexture(GameConstManaged.shaderTextureIndex, sprite_Normal.texture);
			mr_Blink.material.SetTexture(GameConstManaged.shaderTextureIndex, sprite_Normal.texture);
			Object.Destroy(psr_Mirage.material);
			psr_Mirage.material = mat_Normal;
			base.gameObject.tag = "Monster";
			base.gameObject.layer = LayerMask.NameToLayer("Monster_Ghost");
		}
		if (GameMgr.IsHarmony_Static)
		{
			if (sprite_Invincible_H != null)
			{
				sprite_Invincible = sprite_Invincible_H;
			}
			if (sprite_Normal_H != null)
			{
				sprite_Normal = sprite_Normal_H;
			}
			if (mat_Invincible_H != null)
			{
				mat_Invincible = mat_Invincible_H;
			}
			if (mat_Normal_H != null)
			{
				mat_Normal = mat_Normal_H;
			}
			mr.material.SetVector(shaderPivotIndex, new Vector2(0.5f, 0.45f));
			mr_Blink.material.SetVector(shaderPivotIndex, new Vector2(0.5f, 0.45f));
		}
		mr.material.SetTexture(GameConstManaged.shaderTextureIndex, sprite_Normal.texture);
		mr_Blink.material.SetTexture(GameConstManaged.shaderTextureIndex, sprite_Normal.texture);
		if (pattern > AIPattern.Pattern1)
		{
			Object.Destroy(psr_Mirage.material);
			psr_Mirage.material = mat_Normal;
		}
		isInvisible = false;
	}

	public override void Update()
	{
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
					if (isInvisible)
					{
						isInvisible = false;
						mr.material.SetTexture(GameConstManaged.shaderTextureIndex, sprite_Normal.texture);
						if (mr_Blink != null)
						{
							mr_Blink.material.SetTexture(GameConstManaged.shaderTextureIndex, sprite_Normal.texture);
						}
						ps_Vincible.Play();
						UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
						componentData.unitCfg.frozenTimeRatio = originalFrozenTimeRatio;
						SetComponentData(componentData);
						base.gameObject.tag = "Monster";
						base.gameObject.layer = LayerMask.NameToLayer("Monster");
						SetDotsLayer(2048u);
						Object.Destroy(psr_Mirage.material);
						psr_Mirage.material = mat_Normal;
					}
				}
				else if (!isInvisible)
				{
					isInvisible = true;
					mr.material.SetTexture(GameConstManaged.shaderTextureIndex, sprite_Invincible.texture);
					if (mr_Blink != null)
					{
						mr_Blink.material.SetTexture(GameConstManaged.shaderTextureIndex, sprite_Invincible.texture);
					}
					UnitProperty_Dots componentData2 = GetComponentData<UnitProperty_Dots>();
					componentData2.unitCfg.frozenTimeRatio = 0f;
					SetComponentData(componentData2);
					base.gameObject.tag = "Untagged";
					base.gameObject.layer = LayerMask.NameToLayer("Item");
					SetDotsLayer(262144u);
					Object.Destroy(psr_Mirage.material);
					psr_Mirage.material = mat_Invincible;
				}
			}
		}
		base.Update();
		if (base.IsLocked)
		{
			return;
		}
		if (pattern == AIPattern.Pattern2)
		{
			mainModule.startSizeXMultiplier = ModelTransform.localScale.x;
			mainModule.startSizeYMultiplier = ModelTransform.localScale.y;
			if (myPpt.isFlipX)
			{
				psr_Mirage.flip = new Vector3(1f, 0f, 0f);
			}
			else if (!myPpt.isFlipX)
			{
				psr_Mirage.flip = new Vector3(0f, 0f, 0f);
			}
			if (mirageExtraTime < mirageExtraTimer)
			{
				if (ps_Mirage.isPlaying && state != MonsterState.SpeedRun)
				{
					ps_Mirage.Stop();
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
			SetMove(Vector3.zero, isFlip: false);
			bornIdleTimer += Time.deltaTime;
			if (bornIdleTimer >= 0.5f)
			{
				GetNearestTarget();
				if (base.HaveTarget)
				{
					base.Anima.SetTrigger("Run");
					state = MonsterState.RunToTarget;
					break;
				}
				base.Anima.SetTrigger("Idle");
				state = MonsterState.Idle;
				idleTimer = 0f;
				idleTime.RandomResult();
			}
			break;
		case MonsterState.Idle:
			SetMove(Vector3.zero, isFlip: false);
			checkTargetIntervalTimer += Time.deltaTime;
			if (checkTargetIntervalTimer >= 1f)
			{
				checkTargetIntervalTimer = 0f;
				GetNearestTarget();
				if (base.HaveTarget)
				{
					base.Anima.SetTrigger("Run");
					state = MonsterState.RunToTarget;
				}
			}
			idleTimer += Time.deltaTime;
			if (idleTimer > idleTime.result)
			{
				base.Anima.SetTrigger("Run");
				state = MonsterState.RandomMove;
				randomMovePoint = Tool2D.GetNavMeshPointIngoreZ(base.transform.position, randomMoveDistance);
				GetNavInfo(randomMovePoint);
			}
			break;
		case MonsterState.RandomMove:
			CheckNavInfo();
			if (navInfo.allCornerArrived)
			{
				base.Anima.SetTrigger("Idle");
				state = MonsterState.Idle;
				idleTimer = 0f;
				idleTime.RandomResult();
			}
			else
			{
				SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed);
			}
			checkTargetIntervalTimer += Time.deltaTime;
			if (checkTargetIntervalTimer >= 1f)
			{
				checkTargetIntervalTimer = 0f;
				GetNearestTarget();
				if (base.HaveTarget)
				{
					state = MonsterState.RunToTarget;
				}
			}
			break;
		case MonsterState.SpeedRun:
			if (!base.HaveTarget)
			{
				GetNearestTarget();
			}
			if (!base.HaveTarget)
			{
				base.Anima.SetTrigger("Idle");
				state = MonsterState.Idle;
				idleTimer = 0f;
				idleTime.RandomResult();
				break;
			}
			runTimer += Time.deltaTime;
			if (runTimer > runTime)
			{
				mirageExtraTimer = 0f;
				state = MonsterState.RunToTarget;
				break;
			}
			GetNavInfo(base.TargetPoint);
			if (ToTargetDistanceSqr() > 0.040000003f)
			{
				SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed * runSpeedFixer);
			}
			else
			{
				SetMove(Vector3.zero, isFlip: false);
			}
			break;
		case MonsterState.RunToTarget:
			if (!base.HaveTarget)
			{
				GetNearestTarget();
			}
			if (!base.HaveTarget)
			{
				base.Anima.SetTrigger("Idle");
				state = MonsterState.Idle;
				idleTimer = 0f;
				idleTime.RandomResult();
				break;
			}
			GetNavInfo(base.TargetPoint);
			if (ToTargetDistanceSqr() > 0.040000003f)
			{
				SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed);
			}
			else
			{
				SetMove(Vector3.zero, isFlip: false);
			}
			if (pattern == AIPattern.Pattern2)
			{
				runCheckTimer += Time.deltaTime;
				if (runCheckTimer > runCheckInterval.result)
				{
					ps_Mirage.Play();
					runCheckTimer = 0f;
					runCheckInterval.RandomResult();
					runTimer = 0f;
					SEMgr.Inst.monster7Trace.PlaySE();
					state = MonsterState.SpeedRun;
					break;
				}
			}
			blinkIntervalTimer += Time.deltaTime;
			if (blinkIntervalTimer > blinkInterval.result)
			{
				blinkIntervalTimer = 0f;
				blinkInterval.RandomResult();
				GetNearestTargetPlayerFirst();
				if (base.HaveTarget)
				{
					state = MonsterState.Blink;
					base.Anima.SetTrigger("Blink");
					blinkPoint = Tool2D.GetNavMeshPointIngoreZ(base.TargetPoint, ToTargetDistance(), ToTargetDir(), blinkToPlayerBackAngle);
					tsf_BlinkEF.position = blinkPoint;
					tsf_BlinkEF.gameObject.SetActive(value: false);
					tsf_BlinkEF.gameObject.SetActive(value: true);
				}
			}
			break;
		case MonsterState.Blink:
			SetMove(Vector3.zero, isFlip: false);
			tsf_BlinkEF.position = blinkPoint;
			break;
		default:
			Debug.LogError(state);
			break;
		}
	}

	public override void BeforeTakeDamage_Dots(ref TakeDamageInfo_Dots info)
	{
		if (isInvisible)
		{
			info.immuneDamage = true;
		}
	}

	public override void AnimaAction(string animaName)
	{
		if (!(animaName == "Blink"))
		{
			if (animaName == "BlinkFinish")
			{
				GetNearestTarget();
				if (base.HaveTarget)
				{
					state = MonsterState.RunToTarget;
					base.Anima.SetTrigger("Run");
					return;
				}
				base.Anima.SetTrigger("Idle");
				state = MonsterState.Idle;
				idleTimer = 0f;
				idleTime.RandomResult();
			}
			else
			{
				Debug.LogError(animaName);
			}
		}
		else
		{
			SEMgr.Inst.monster7Blink.PlaySE();
			base.transform.position = blinkPoint;
			LocalTransform componentData = GetComponentData<LocalTransform>();
			componentData.Position = base.transform.position;
			SetComponentData(componentData);
		}
	}
}
