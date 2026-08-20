using Unity.Physics;
using UnityEngine;

public class Elite3 : UnitBase
{
	private enum MonsterState
	{
		BornIdle,
		Mirror,
		MirrorPattern2
	}

	public VariableFloat shootBulletInterval;

	public float shootTimePerInterval;

	[Range(0f, 1f)]
	public float percentHPEnterInvisible;

	public float attackRotateSpeed;

	public AIPattern pattern;

	[Header("Pattern2")]
	public float perBulletAngleOffset;

	public float attackSpeed;

	[Range(0f, 1f)]
	public float doubleShootHpPercent;

	[Header("Spell")]
	public float spellHight;

	public float spellSpeed;

	public float spellDuration;

	public Elite3_Tentacle tentacleControler;

	private MonsterState state;

	private SpriteRenderer sr_Shadow;

	private float shootBulletIntervalTimer;

	private bool isAttacking;

	private int shootTime;

	private int shootTimer;

	private bool enterInvisible;

	private bool invisibling;

	private bool direction;

	private SpellSpawnParams ssp;

	private bool reversedShoot;

	private float nowReverseTime;

	private void Start()
	{
		if (pattern == AIPattern.Pattern2)
		{
			tentacleControler.rotateSpeed = 1080f;
		}
		Shadow component = GetComponent<Shadow>();
		component.CreateShadow();
		sr_Shadow = component.ShadowGO.GetComponentInChildren<SpriteRenderer>();
		if (GameMgr.IsMobile_Static && pattern == AIPattern.Pattern1)
		{
			shootBulletInterval.value1 /= 2f;
			shootBulletInterval.value2 /= 2f;
		}
		shootBulletInterval.RandomResult();
		ssp = UnitDotsSyncSystem.GetSpellPrototype(10011);
		UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
		sSPModifier.Speed = spellSpeed;
		sSPModifier.Duration = spellDuration;
		sSPModifier.Shooter = myPpt.myEntity;
		sSPModifier.ApplyToSSP(ref ssp);
	}

	public override void Update()
	{
		base.Update();
		if (base.IsLocked)
		{
			return;
		}
		switch (state)
		{
		case MonsterState.BornIdle:
			SetMove(Vector3.zero, isFlip: false);
			bornIdleTimer += Time.deltaTime;
			if (bornIdleTimer >= 0.5f)
			{
				if (pattern == AIPattern.Pattern1)
				{
					state = MonsterState.Mirror;
					break;
				}
				state = MonsterState.MirrorPattern2;
				base.Anima.SetFloat("AttackSpeed", attackSpeed);
			}
			break;
		case MonsterState.Mirror:
		{
			Vector3 targetPoint2 = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint + LevelMgr.Inst.CurrentRoomCtrller.CenterPoint - PlayerMgr.Inst.PlayerPoint;
			targetPoint2.y = PlayerMgr.Inst.PlayerPoint.y;
			GetNavInfo(targetPoint2);
			if (navInfo.allCornerArrived)
			{
				SetMove(Vector3.zero);
			}
			else
			{
				SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed);
			}
			if (isAttacking || invisibling)
			{
				break;
			}
			shootBulletIntervalTimer += Time.deltaTime;
			if (shootBulletIntervalTimer >= shootBulletInterval.result)
			{
				shootBulletIntervalTimer = 0f;
				shootTime = (int)(shootBulletInterval.result * shootTimePerInterval);
				shootBulletInterval.RandomResult();
				isAttacking = true;
				if (pattern == AIPattern.Pattern1)
				{
					base.Anima.SetTrigger("Attack");
				}
				else
				{
					base.Anima.SetTrigger("Attack2");
				}
				base.Anima.Play("Elite3_RotateAttack", 1);
				tentacleControler.attacking = true;
				base.Anima.SetFloat("RotateSpeed", attackRotateSpeed);
			}
			break;
		}
		case MonsterState.MirrorPattern2:
		{
			Vector3 targetPoint = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint + LevelMgr.Inst.CurrentRoomCtrller.CenterPoint - PlayerMgr.Inst.PlayerPoint;
			GetNavInfo(targetPoint);
			if (navInfo.allCornerArrived)
			{
				SetMove(Vector3.zero);
			}
			else
			{
				SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed);
			}
			if (!isAttacking && !invisibling)
			{
				shootBulletIntervalTimer += Time.deltaTime;
				if (shootBulletIntervalTimer >= shootBulletInterval.result)
				{
					shootBulletIntervalTimer = 0f;
					shootTime = (int)(shootBulletInterval.result * shootTimePerInterval);
					shootBulletInterval.RandomResult();
					isAttacking = true;
					base.Anima.SetTrigger("Attack");
					tentacleControler.attacking = true;
					base.Anima.Play("Elite3_RotateAttack", 1);
					base.Anima.SetFloat("RotateSpeed", attackRotateSpeed);
				}
			}
			break;
		}
		default:
			Debug.LogError(state);
			break;
		}
	}

	private void LateUpdate()
	{
		if (base.deadStayed || !enterInvisible || invisibling)
		{
			return;
		}
		for (int i = 0; i < myPpt.SR_Models.Length; i++)
		{
			if (myPpt.SR_Models[i].color.g == 1f)
			{
				myPpt.SR_Models[i].color = new Color(1f, 1f, 1f, 0f);
				myPpt.SR_Models[i].material.SetColor(GameConstManaged.shaderColorIndex, myPpt.SR_Models[i].color);
			}
		}
	}

	public override void AnimaAction(string animaName)
	{
		if (base.deadStayed)
		{
			return;
		}
		switch (animaName)
		{
		case "Attack":
		{
			UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
			switch (pattern)
			{
			case AIPattern.Pattern1:
			{
				Vector3 dir = Tool2D.GetDir(90f);
				if (PlayerMgr.Inst.PlayerPoint.x > base.transform.position.x)
				{
					dir = Tool2D.GetDir(270f);
				}
				sSPModifier.Direction = dir;
				sSPModifier.SpawnPosition = base.transform.position + new Vector3(0f, 0f, 0f - spellHight);
				sSPModifier.ApplyToSSP(ref ssp);
				ShootSpell(ssp);
				break;
			}
			case AIPattern.Pattern2:
			{
				float num = ((!direction) ? ((float)(shootTime - 1) * perBulletAngleOffset * 0.67f - (float)shootTimer * perBulletAngleOffset) : ((float)(-(shootTime - 1)) * perBulletAngleOffset * 0.67f + (float)shootTimer * perBulletAngleOffset));
				if (enterInvisible)
				{
					num = ((!direction) ? ((float)(shootTime - 1) * perBulletAngleOffset * (GameMgr.IsMobile_Static ? 0.9f : 0.9f) - (float)shootTimer * perBulletAngleOffset) : ((float)(-(shootTime - 1)) * perBulletAngleOffset * (GameMgr.IsMobile_Static ? 0.9f : 0.9f) + (float)shootTimer * perBulletAngleOffset));
					if (reversedShoot)
					{
						num = 0f - num;
					}
				}
				sSPModifier.Direction = ToPointDir(PlayerMgr.Inst.PlayerPoint, num);
				sSPModifier.SpawnPosition = base.transform.position + new Vector3(0f, 0f, 0f - spellHight);
				sSPModifier.ApplyToSSP(ref ssp);
				ShootSpell(ssp);
				break;
			}
			default:
				Debug.LogError(pattern);
				break;
			}
			shootTimer++;
			if (shootTimer >= shootTime)
			{
				if (enterInvisible && pattern == AIPattern.Pattern2 && GeneralTool.ChanceResult(0.8f) && nowReverseTime < 5f)
				{
					nowReverseTime += 1f;
					shootTimer = 0;
					reversedShoot = !reversedShoot;
					shootTime = (int)(shootBulletInterval.result * shootTimePerInterval * Random.Range(0.8f, 1.6f));
				}
				else
				{
					nowReverseTime = 0f;
					shootTimer = 0;
					base.Anima.SetTrigger("Idle");
					base.Anima.Play("Elite3_Rotate", 1);
					base.Anima.SetFloat("RotateSpeed", 1f);
					tentacleControler.attacking = false;
					isAttacking = false;
				}
			}
			break;
		}
		case "Invisible":
			invisibling = false;
			ObjPoolMgr.Inst.GetUIGO("Prefabs/UI/UITextFloat").GetComponent<UITextFloat>().Initialize(1002015.GetText(), UITextFloatType.Normal, base.transform.position);
			break;
		default:
			Debug.LogError(animaName);
			break;
		case "ShadowHide":
			break;
		case "ShadowShow":
			break;
		}
	}

	public override void AfterTakeDamage_Dots(ref TakeDamageInfo_Dots info)
	{
		if (!enterInvisible && base.CurrentHPRatio <= percentHPEnterInvisible)
		{
			enterInvisible = true;
			tentacleControler.attacking = false;
			base.Anima.SetTrigger("Invisible");
			shootTimer = 0;
			isAttacking = false;
			invisibling = true;
		}
	}

	protected override void BossDeadStay()
	{
		base.Anima.SetTrigger("Die");
		base.enabled = false;
		base.Rigid.isKinematic = true;
		SyncDotsRigidKindmatic();
		PhysicsVelocity componentData = GetComponentData<PhysicsVelocity>();
		componentData.Linear = Vector3.zero;
		SetComponentData(componentData);
		base.CC_Self.enabled = false;
		SetDotsCCEnable(isOpen: false);
		myPpt.enabled = false;
		UnitProperty_Dots componentData2 = GetComponentData<UnitProperty_Dots>();
		componentData2.BossDeadStay();
		SetComponentData(componentData2);
		myPpt.ResetBodyColor();
		GetComponent<BossDeadCreateEffect>().CreateEffect();
		tentacleControler.stopRotate = true;
	}
}
