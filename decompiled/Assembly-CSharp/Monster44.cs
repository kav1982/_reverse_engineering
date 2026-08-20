using Unity.Entities;
using UnityEngine;

public class Monster44 : UnitBase
{
	public enum MonsterState
	{
		BornIdle,
		Idle,
		RandomMove,
		Move,
		Attack,
		Amaze,
		AfterAttack,
		ChargePrepare,
		Charge,
		AfterCharge,
		MegaAttackPrepare,
		MegaAttack,
		SpreadAttack
	}

	public MonsterState _state;

	private bool stateQuit;

	private bool changedState;

	private float stateExistTime;

	[Header("关于虚化")]
	public float hideDistance;

	public float hideCD;

	private float hideCDTimer;

	public float hideAlpha;

	public bool isHide;

	public float checkHideInterval;

	private float checkHideTime;

	public SpriteRenderer mainSprite;

	public SpriteRenderer borderSprite;

	[Header("其他动作")]
	public float idleTime;

	private float idleTimer;

	public VariableFloat randomMoveRadius;

	public float randomMoveLimitTime;

	public float sight;

	public float warningRadius;

	private float warningTimer;

	public float warningTime;

	public ParticleSystem appearParticle;

	public float reappearTime;

	private float appearTimer;

	[Header("攻击通用")]
	public float safeDistance;

	public float keepDistanceAngle;

	public VariableFloat randomMoveDistance;

	[Header("冲刺")]
	public float chargeChance;

	public float chargeTime;

	public float afterChargeTime;

	public float chargeSpeedFix;

	public float chargeAngleRotateSpeed;

	public float chargeRange;

	private Vector3 chargeDir;

	[Header("远程攻击")]
	public float attackChance;

	public float AttackRange;

	public float afterAttackTime;

	public AIPattern pattern;

	private SpellInitialParameter sipBullet = new SpellInitialParameter();

	public float spellSpeed;

	public float spellDuration;

	public int spellDamage;

	public float spellHeight;

	public float spellCount;

	public float spellAmplitude;

	public float spellFrequency;

	private Vector3 attackDir;

	[Header("三模式连射枪")]
	public float megaAttackChance;

	public VariableInt attackTimes;

	private int attackCount;

	[Header("三模式散射枪")]
	public float spreadAngle;

	public int spreadCount;

	public float spreadFrequency;

	private bool reversed;

	private float originalFrozenTimeRatio;

	private SpellSpawnParams ssp;

	public MonsterState state
	{
		get
		{
			return _state;
		}
		set
		{
			stateExistTime = 0f;
			stateQuit = true;
			_state = value;
		}
	}

	public override void SingleInitialCallback()
	{
		originalFrozenTimeRatio = myPpt.unitCfg.frozenTimeRatio;
		ssp = UnitDotsSyncSystem.GetSpellPrototype(90081);
		UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
		sSPModifier.Shooter = myPpt.myEntity;
		sSPModifier.Duration = spellDuration;
		sSPModifier.Damage = spellDamage;
		sSPModifier.Speed = spellSpeed;
		sSPModifier.ApplyToSSP(ref ssp);
	}

	public override void EveryInitialCallback()
	{
		state = MonsterState.BornIdle;
		hideCDTimer = 0f;
		borderSprite.enabled = false;
	}

	public void Warning(Entity target)
	{
		if (state == MonsterState.Idle || state == MonsterState.BornIdle || state == MonsterState.RandomMove)
		{
			targetEntity = target;
			state = MonsterState.Move;
		}
	}

	public bool CheckTargetNearby()
	{
		if (Tool2D.IgnoreZDistanceSqr(base.transform.position, PlayerMgr.Inst.PlayerCtrller.transform.position) < hideDistance * hideDistance)
		{
			return true;
		}
		if (UnitDotsSyncSystem.HaveCollider(base.transform.position, hideDistance, GameConst.Filter_Friendly))
		{
			return true;
		}
		return false;
	}

	public override void Update()
	{
		checkHideTime += Time.deltaTime;
		if (checkHideTime > checkHideInterval)
		{
			if (!CheckTargetNearby())
			{
				hideCDTimer += Time.deltaTime;
			}
			else
			{
				hideCDTimer = 0f;
				isHide = false;
			}
		}
		if (hideCDTimer > hideCD)
		{
			isHide = true;
			hideCDTimer = 0f;
		}
		base.Update();
		if (base.IsLocked)
		{
			return;
		}
		if (isHide)
		{
			if (base.gameObject.tag != "Untagged")
			{
				base.gameObject.tag = "Untagged";
				borderSprite.enabled = true;
				UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
				componentData.unitCfg.frozenTimeRatio = 0f;
				SetComponentData(componentData);
				SetDotsLayer(262144u);
				mainSprite.material.color = new Color(mainSprite.color.r, mainSprite.color.g, mainSprite.color.b, hideAlpha);
			}
			mainSprite.material.color = new Color(mainSprite.color.r, mainSprite.color.g, mainSprite.color.b, hideAlpha);
			if (mainSprite.material.color.a != hideAlpha)
			{
				appearTimer = 0f;
			}
			appearTimer += Time.deltaTime;
		}
		else
		{
			if (base.gameObject.tag != "Monster")
			{
				base.gameObject.tag = "Monster";
				UnitProperty_Dots componentData2 = GetComponentData<UnitProperty_Dots>();
				componentData2.unitCfg.frozenTimeRatio = originalFrozenTimeRatio;
				SetComponentData(componentData2);
				base.gameObject.tag = "Monster";
				borderSprite.enabled = false;
				SetDotsLayer(2048u);
				if (appearTimer > reappearTime)
				{
					appearParticle.Play();
					SEMgr.Inst.PlaySE(SEMgr.Inst.monster44Appear, SEPlayMode.Unique);
				}
			}
			if (mainSprite.material.color.a != 1f)
			{
				mainSprite.material.color = new Color(mainSprite.color.r, mainSprite.color.g, mainSprite.color.b, 1f);
			}
		}
		if (stateQuit)
		{
			stateQuit = false;
			changedState = true;
		}
		else
		{
			changedState = false;
		}
		stateExistTime += Time.deltaTime;
		switch (state)
		{
		case MonsterState.BornIdle:
			if (changedState)
			{
				bornIdleTimer = 0f;
				base.Anima.Play("Monster44_Idle");
			}
			bornIdleTimer += Time.deltaTime;
			if (bornIdleTimer > 0.5f)
			{
				state = MonsterState.Idle;
			}
			else
			{
				SetMove(Vector3.zero, isFlip: false);
			}
			break;
		case MonsterState.Idle:
			if (changedState)
			{
				idleTimer = 0f;
				base.Anima.Play("Monster44_Idle");
			}
			SetMove(Vector3.zero, isFlip: false);
			idleTimer += Time.deltaTime;
			if (idleTimer > idleTime)
			{
				state = MonsterState.RandomMove;
				break;
			}
			checkTargetIntervalTimer += Time.deltaTime;
			if (checkTargetIntervalTimer > 1f)
			{
				GetNearestTarget();
				if (base.HaveTarget && ToTargetDistanceSqr() < sight * sight)
				{
					state = MonsterState.Move;
				}
			}
			break;
		case MonsterState.RandomMove:
			if (changedState)
			{
				base.Anima.Play("Monster44_Move");
				randomMoveRadius.RandomResult();
				GetNavInfo(base.transform.position + Tool2D.GetDir() * randomMoveRadius.result);
			}
			if (stateExistTime > randomMoveLimitTime)
			{
				state = MonsterState.Idle;
				break;
			}
			if (navInfo.allCornerArrived)
			{
				state = MonsterState.Idle;
				break;
			}
			SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed);
			CheckNavInfo();
			checkTargetIntervalTimer += Time.deltaTime;
			if (checkTargetIntervalTimer > 1f)
			{
				GetNearestTarget();
				if (base.HaveTarget && ToTargetDistanceSqr() < sight * sight)
				{
					state = MonsterState.Move;
				}
			}
			break;
		case MonsterState.Amaze:
			if (changedState)
			{
				base.Anima.Play("Monster44_Amaze");
			}
			SetMove(Vector3.zero, isFlip: false);
			break;
		case MonsterState.Move:
			if (changedState)
			{
				base.Anima.Play("Monster44_Move");
				if (!base.HaveTarget)
				{
					GetNearestTarget();
				}
				if (!base.HaveTarget)
				{
					state = MonsterState.Idle;
					break;
				}
				GetNavInfo(base.TargetPoint);
			}
			if (!base.HaveTarget)
			{
				GetNearestTarget();
			}
			if (!base.HaveTarget)
			{
				state = MonsterState.Idle;
				break;
			}
			GetNavInfo(base.TargetPoint);
			if (pattern == AIPattern.Pattern2)
			{
				if (ToTargetDistanceSqr() < AttackRange * AttackRange)
				{
					if (Random.Range(0f, 1f) < attackChance)
					{
						state = MonsterState.Attack;
					}
					else
					{
						state = MonsterState.AfterAttack;
					}
					break;
				}
			}
			else if (pattern == AIPattern.Pattern3)
			{
				if (ToTargetDistanceSqr() < AttackRange * AttackRange)
				{
					if (Random.Range(0f, 1f) < attackChance)
					{
						if (Random.Range(0f, 1f) < megaAttackChance)
						{
							state = MonsterState.MegaAttackPrepare;
						}
						else
						{
							state = MonsterState.SpreadAttack;
						}
					}
					else
					{
						state = MonsterState.AfterAttack;
					}
					break;
				}
			}
			else if (ToTargetDistanceSqr() < chargeRange * chargeRange)
			{
				if (Random.Range(0f, 1f) < chargeChance)
				{
					state = MonsterState.ChargePrepare;
				}
				else
				{
					state = MonsterState.AfterCharge;
				}
				break;
			}
			SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed);
			warningTimer += Time.deltaTime;
			if (warningTimer > warningTime)
			{
				warningTimer = 0f;
				GetNearestTarget();
			}
			break;
		case MonsterState.ChargePrepare:
			if (changedState)
			{
				base.Anima.Play("Monster44_ChargePrepare");
			}
			SetMove(Vector3.zero, isFlip: false);
			if (base.HaveTarget)
			{
				SetFlip(ToTargetDir().x);
			}
			break;
		case MonsterState.Charge:
			if (changedState)
			{
				base.Anima.Play("Monster44_Charge");
				GetNearestTarget();
				if (base.HaveTarget)
				{
					chargeDir = ToTargetDir();
				}
				else
				{
					chargeDir = Tool2D.GetDir();
				}
			}
			if (base.HaveTarget)
			{
				chargeDir = Tool2D.RotateTowardsAroundZAxis(chargeDir, ToTargetDir(), Time.deltaTime * chargeAngleRotateSpeed);
			}
			GetNavInfo(chargeDir.normalized * base.MoveSpeed * chargeSpeedFix * Time.deltaTime + base.transform.position);
			SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed * chargeSpeedFix);
			if (stateExistTime > chargeTime)
			{
				state = MonsterState.AfterCharge;
			}
			break;
		case MonsterState.AfterCharge:
			if (changedState)
			{
				base.Anima.Play("Monster44_Move");
				GetNearestTarget();
				if (!base.HaveTarget)
				{
					GetNearestTarget();
				}
				if (!base.HaveTarget)
				{
					state = MonsterState.Idle;
					break;
				}
				if (ToTargetDistanceSqr() < safeDistance * safeDistance)
				{
					GetNavInfo(Tool2D.GetNavMeshPointIngoreZ(base.transform.position, randomMoveDistance, -ToTargetDir(), keepDistanceAngle, navAreaMask));
				}
				else
				{
					GetNavInfo(Tool2D.GetNavMeshPointIngoreZ(base.transform.position, randomMoveDistance, navAreaMask));
				}
			}
			if (!base.HaveTarget)
			{
				GetNearestTarget();
			}
			if (!base.HaveTarget)
			{
				state = MonsterState.Idle;
				break;
			}
			CheckNavInfo();
			if (!navInfo.allCornerArrived)
			{
				SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed);
			}
			else if (ToTargetDistanceSqr() < safeDistance * safeDistance)
			{
				GetNavInfo(Tool2D.GetNavMeshPointIngoreZ(base.transform.position, randomMoveDistance, -ToTargetDir(), keepDistanceAngle, navAreaMask));
			}
			else
			{
				GetNavInfo(Tool2D.GetNavMeshPointIngoreZ(base.transform.position, randomMoveDistance, navAreaMask));
			}
			if (stateExistTime > afterChargeTime)
			{
				state = MonsterState.Move;
			}
			break;
		case MonsterState.Attack:
			if (changedState)
			{
				base.Anima.Play("Monster44_Attack");
			}
			if (base.HaveTarget)
			{
				SetFlip(ToTargetDir().x);
			}
			SetMove(Vector3.zero, isFlip: false);
			break;
		case MonsterState.SpreadAttack:
			if (changedState)
			{
				base.Anima.Play("Monster44_SpreadAttack");
			}
			if (base.HaveTarget)
			{
				SetFlip(ToTargetDir().x);
			}
			SetMove(Vector3.zero, isFlip: false);
			break;
		case MonsterState.AfterAttack:
			if (changedState)
			{
				base.Anima.Play("Monster44_Move");
				if (!base.HaveTarget)
				{
					GetNearestTarget();
				}
				if (!base.HaveTarget)
				{
					state = MonsterState.Idle;
					break;
				}
				if (ToTargetDistanceSqr() < safeDistance * safeDistance)
				{
					GetNavInfo(Tool2D.GetNavMeshPointIngoreZ(base.transform.position, randomMoveDistance, -ToTargetDir(), keepDistanceAngle, navAreaMask));
				}
				else
				{
					GetNavInfo(Tool2D.GetNavMeshPointIngoreZ(base.transform.position, randomMoveDistance, navAreaMask));
				}
			}
			if (!base.HaveTarget)
			{
				GetNearestTarget();
			}
			if (!base.HaveTarget)
			{
				state = MonsterState.Idle;
				break;
			}
			CheckNavInfo();
			if (!navInfo.allCornerArrived)
			{
				SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed);
			}
			else if (ToTargetDistanceSqr() < safeDistance * safeDistance)
			{
				GetNavInfo(Tool2D.GetNavMeshPointIngoreZ(base.transform.position, randomMoveDistance, -ToTargetDir(), keepDistanceAngle, navAreaMask));
			}
			else
			{
				GetNavInfo(Tool2D.GetNavMeshPointIngoreZ(base.transform.position, randomMoveDistance, navAreaMask));
			}
			if (stateExistTime > afterAttackTime)
			{
				state = MonsterState.Move;
			}
			break;
		case MonsterState.MegaAttackPrepare:
			if (changedState)
			{
				attackDir = Tool2D.GetDir();
				base.Anima.Play("Monster44_MegaAttackPrepare");
				attackTimes.RandomResult();
				attackCount = 0;
			}
			SetMove(Vector3.zero, isFlip: false);
			if (base.HaveTarget)
			{
				SetFlip(ToTargetDir().x);
			}
			break;
		case MonsterState.MegaAttack:
			if (changedState)
			{
				attackCount++;
				base.Anima.Play("Monster44_MegaAttack", 0, 0f);
			}
			SetFlip(attackDir.x);
			break;
		}
	}

	public override void AfterTakeDamage_Dots(ref TakeDamageInfo_Dots info)
	{
		if (EntityIsValid(info.attackerEntity) && (state == MonsterState.Idle || state == MonsterState.BornIdle || state == MonsterState.RandomMove))
		{
			UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>(info.attackerEntity);
			if (componentData.unitCfg.IsSameCamp(UnitType.Player) && componentData.unitCfg.unitType == UnitType.Player && PlayerMgr.Inst.PlayerCtrller.IsVisible)
			{
				targetEntity = PlayerMgr.Inst.PlayerEtt;
				state = MonsterState.Move;
			}
		}
	}

	public override void BeforeTakeDamage_Dots(ref TakeDamageInfo_Dots info)
	{
		if (isHide)
		{
			info.immuneDamage = true;
		}
	}

	public override void AnimaAction(string animaName)
	{
		switch (animaName)
		{
		case "FirstAttack":
		{
			attackDir = Tool2D.GetDir();
			GetNearestTarget();
			if (base.HaveTarget)
			{
				attackDir = ToTargetDir();
			}
			UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
			sSPModifier.Direction = attackDir;
			sSPModifier.SpawnPosition = base.transform.position + new Vector3(0f, 0f, 0f - spellHeight);
			sSPModifier.Float1 = spellAmplitude;
			sSPModifier.Float2 = spellFrequency;
			sSPModifier.Float3 = -1f;
			sSPModifier.ApplyToSSP(ref ssp);
			ShootSpell(ssp);
			sSPModifier.Float3 = 1f;
			sSPModifier.ApplyToSSP(ref ssp);
			ShootSpell(ssp);
			sipBullet.shootDirection = attackDir;
			break;
		}
		case "Attack":
		{
			UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
			sSPModifier.SpawnPosition = base.transform.position + new Vector3(0f, 0f, 0f - spellHeight);
			sSPModifier.Float1 = spellAmplitude;
			sSPModifier.Float2 = spellFrequency;
			sSPModifier.Float3 = -1f;
			sSPModifier.ApplyToSSP(ref ssp);
			ShootSpell(ssp);
			sSPModifier.Float3 = 1f;
			sSPModifier.ApplyToSSP(ref ssp);
			ShootSpell(ssp);
			break;
		}
		case "AttackFinish":
			state = MonsterState.AfterAttack;
			break;
		case "AmazeFinish":
			state = MonsterState.Move;
			break;
		case "ChargePrepareFinish":
			state = MonsterState.Charge;
			break;
		case "MegaAttackPrepareFinish":
			state = MonsterState.MegaAttack;
			break;
		case "MegaAttack":
		{
			GetNearestTarget();
			if (base.HaveTarget)
			{
				attackDir = ToTargetDir();
			}
			sipBullet.shootDirection = attackDir;
			UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
			sSPModifier.Direction = attackDir;
			sSPModifier.SpawnPosition = base.transform.position + new Vector3(0f, 0f, 0f - spellHeight);
			sSPModifier.Float1 = spellAmplitude;
			sSPModifier.Float2 = spellFrequency;
			sSPModifier.Float3 = -1f;
			sSPModifier.ApplyToSSP(ref ssp);
			ShootSpell(ssp);
			sSPModifier.Float3 = 1f;
			sSPModifier.ApplyToSSP(ref ssp);
			ShootSpell(ssp);
			break;
		}
		case "MegaAttackCancel":
			if (attackCount <= attackTimes.result)
			{
				state = MonsterState.MegaAttack;
			}
			break;
		case "MegaAttackFinish":
			state = MonsterState.AfterAttack;
			break;
		case "SpreadAttackFirst":
		{
			attackDir = Tool2D.GetDir();
			GetNearestTarget();
			if (base.HaveTarget)
			{
				attackDir = ToTargetDir();
			}
			reversed = Random.Range(0f, 1f) < 0.5f;
			sipBullet.shootDirection = attackDir;
			UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
			sSPModifier.SpawnPosition = base.transform.position + new Vector3(0f, 0f, 0f - spellHeight);
			sSPModifier.Float1 = spellAmplitude;
			sSPModifier.Float2 = spellFrequency;
			sSPModifier.Float3 = ((!reversed) ? 1 : (-1));
			sipBullet.shootDirection = attackDir;
			for (int j = 0; j < spreadCount; j++)
			{
				sSPModifier.Direction = (sipBullet.shootDirection = Tool2D.GetDir(attackDir, (float)j * spreadAngle - (float)spreadCount / 2f * spreadAngle));
				sSPModifier.ApplyToSSP(ref ssp);
				ShootSpell(ssp);
			}
			break;
		}
		case "SpreadAttack":
		{
			UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
			sSPModifier.SpawnPosition = base.transform.position + new Vector3(0f, 0f, 0f - spellHeight);
			sSPModifier.Float1 = spellAmplitude;
			sSPModifier.Float2 = spellFrequency;
			sSPModifier.Float3 = ((!reversed) ? 1 : (-1));
			sipBullet.shootDirection = attackDir;
			for (int i = 0; i < spreadCount; i++)
			{
				sSPModifier.Direction = (sipBullet.shootDirection = Tool2D.GetDir(attackDir, (float)i * spreadAngle - (float)spreadCount / 2f * spreadAngle));
				sSPModifier.ApplyToSSP(ref ssp);
				ShootSpell(ssp);
			}
			break;
		}
		}
	}
}
