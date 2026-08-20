using Spine;
using Unity.Transforms;
using UnityEngine;

public class Monster51 : UnitBase
{
	public enum MonsterState
	{
		BornIdle,
		Idle,
		RandomMove,
		Aim,
		Attack,
		MoveToTarget,
		KeepDistance,
		Teleport
	}

	public MonsterState _state;

	private bool stateQuit;

	private bool changedState;

	private float stateExistTime;

	[Header("随机移动")]
	public VariableFloat randomMoveTime;

	public VariableFloat randomMoveRadius;

	[Header("站立")]
	public VariableFloat idleTime;

	[Header("瞄准和效果版攻击")]
	public float attackCD;

	private float attackCDTimer;

	public float attackRadius;

	public float keepDistance;

	public float aimTime;

	public float aimHeight;

	[Header("最大预测时间")]
	public float maxPreAimTime;

	[Header("子弹版攻击属性")]
	public float spellHeight;

	public float spellSpeed;

	public float spellDuration;

	public int spellDamage;

	private SpellInitialParameter sipBullet = new SpellInitialParameter();

	public AIPattern pattern;

	[Header("二模式传送门！")]
	public float portalCD;

	public float teleportChance;

	private float portalCDTimer;

	public Animator portalAnima1;

	public Animator portalAnima2;

	public SpriteRenderer portalSprite1;

	public SpriteRenderer portalSprite2;

	public float portalSwitchInterval;

	private Vector3 teleportPoint;

	private Vector3 beforeTeleportPoint;

	public VariableFloat teleportRadius;

	public float teleportMinDistance;

	public ParticleSystem portalParticle;

	public Shadow thisShadow;

	public float groundSpearHeight;

	public Monster51_Tongue tongue;

	public SpriteRenderer aimRenderer;

	public bool aimTracking;

	private float originalFrozenTimeRatio;

	[Header("二模式分裂三叉")]
	public float trapleSpearAngle;

	[Header("和谐模式")]
	public Sprite aimSprite_H;

	private Vector3 aimDir;

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

	public float GetPreAimTime(Vector3 delta, Vector3 targetSpeed)
	{
		float num = targetSpeed.x * targetSpeed.x + targetSpeed.y * targetSpeed.y - spellSpeed * spellSpeed;
		float num2 = -2f * (targetSpeed.x * delta.x + targetSpeed.y * delta.y);
		float num3 = delta.x * delta.x + delta.y * delta.y;
		float num4 = num2 * num2 - 4f * num * num3;
		if (num4 < 0f)
		{
			return -1f;
		}
		float num5 = (0f - num2 + Mathf.Pow(num4, 0.5f)) / (2f * num);
		float num6 = (0f - num2 - Mathf.Pow(num4, 0.5f)) / (2f * num);
		if (num5 < 0f && num6 < 0f)
		{
			return -1f;
		}
		if (num5 * num6 < 0f)
		{
			return Mathf.Min(maxPreAimTime, Mathf.Max(num5, num6));
		}
		return Mathf.Min(maxPreAimTime, Mathf.Min(num5, num6));
	}

	public bool WallBlocked(bool usePosition = false, Vector3 startPosition = default(Vector3))
	{
		Vector3 vector = base.transform.position;
		if (usePosition)
		{
			vector = startPosition;
		}
		Ray ray = new Ray(vector, Tool2D.IgnoreZV2ToV1Normal(base.TargetPoint, vector));
		if (UnitDotsSyncSystem.Raycast(ray, 999f, GameConst.Filter_Wall, out var result))
		{
			if (base.HaveTarget && ToTargetDistanceSqr() < (ray.origin - result.point).sqrMagnitude)
			{
				return false;
			}
			return true;
		}
		return false;
	}

	public override void SingleInitialCallback()
	{
		originalFrozenTimeRatio = myPpt.unitCfg.frozenTimeRatio;
		ssp = UnitDotsSyncSystem.GetSpellPrototype(90141);
		UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
		sSPModifier.Shooter = myPpt.myEntity;
		sSPModifier.Damage = spellDamage;
		sSPModifier.Duration = spellDuration;
		sSPModifier.Speed = spellSpeed;
		sSPModifier.ApplyToSSP(ref ssp);
		myPpt.RemoveSRFromArray(portalSprite1);
		myPpt.RemoveSRFromArray(portalSprite2);
		if (GameMgr.IsHarmony_Static)
		{
			base.SAnima.initialSkinName += "_HX";
			base.SAnima.Initialize(overwrite: true);
		}
		if (GameMgr.IsMobile_Static)
		{
			spellSpeed *= 0.8f;
			attackRadius *= 0.7f;
		}
	}

	public override void EveryInitialCallback()
	{
		attackCDTimer = Random.Range(0f, attackCD);
		if (pattern == AIPattern.Pattern2)
		{
			portalAnima1.Play("Monster51_PortalInvisible");
			portalAnima2.Play("Monster51_PortalInvisible");
			aimRenderer.enabled = false;
		}
		base.SAnima.timeScale = 1f;
		base.SAnima.AnimationState.Data.DefaultMix = 0f;
		base.SAnima.AnimationState.SetAnimation(0, "idle", loop: true);
		base.SAnima.Update(1f);
		base.SAnima.skeleton.UpdateWorldTransform(Skeleton.Physics.None);
		base.SAnima.LateUpdate();
		tongue.Allmove();
		state = MonsterState.BornIdle;
		if (GameMgr.IsChAge14_Static && pattern == AIPattern.Pattern2)
		{
			aimRenderer.sprite = aimSprite_H;
		}
	}

	public override void Frame1InitialCallback()
	{
		if (pattern == AIPattern.Pattern2)
		{
			thisShadow.Show();
		}
	}

	protected override void SetFlip(float motionX)
	{
		for (int i = 0; i < myPpt.SR_Models.Length; i++)
		{
			myPpt.SR_Models[i].flipX = motionX < 0f;
			if (myPpt.SR_Models[i] != tongue.mainRenderer)
			{
				myPpt.SR_Models[i].material.SetFloat(GameConstManaged.shaderFlipXIndex, (!myPpt.SR_Models[i].flipX) ? 1 : (-1));
			}
		}
		base.SAnima.transform.localScale = new Vector3(Mathf.Abs(base.SAnima.transform.localScale.x) * (float)((!(motionX <= 0f)) ? 1 : (-1)), base.SAnima.transform.localScale.y, base.SAnima.transform.localScale.z);
	}

	public override void Update()
	{
		base.Update();
		if (base.IsLocked)
		{
			return;
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
		attackCDTimer += Time.deltaTime;
		portalCDTimer += Time.deltaTime;
		switch (state)
		{
		default:
			return;
		case MonsterState.BornIdle:
			if (changedState)
			{
				base.SAnima.AnimationState.Data.DefaultMix = 0f;
				base.SAnima.AnimationState.SetAnimation(0, "idle", loop: true);
				if (pattern == AIPattern.Pattern1)
				{
					base.Anima.Play("Monster51_Idle");
				}
				else
				{
					base.Anima.Play("Monster51_Idle 1");
				}
			}
			if (stateExistTime > 0.5f)
			{
				base.SAnima.AnimationState.Data.DefaultMix = 0.2f;
				base.SAnima.AnimationState.SetAnimation(0, "idle", loop: true);
				state = MonsterState.MoveToTarget;
			}
			break;
		case MonsterState.RandomMove:
			if (changedState)
			{
				base.SAnima.AnimationState.SetAnimation(0, "move", loop: true);
				if (pattern == AIPattern.Pattern1)
				{
					base.Anima.Play("Monster51_Move");
				}
				else
				{
					base.Anima.Play("Monster51_Move 1");
				}
				randomMoveTime.RandomResult();
				randomMoveRadius.RandomResult();
				GetNavInfo(base.transform.position + Tool2D.GetDir() * randomMoveRadius.result);
			}
			if (stateExistTime > randomMoveTime.result)
			{
				state = MonsterState.Idle;
				break;
			}
			if (navInfo.allCornerArrived)
			{
				GetNavInfo(base.transform.position + Tool2D.GetDir() * randomMoveRadius.result);
				break;
			}
			SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed);
			CheckNavInfo();
			break;
		case MonsterState.MoveToTarget:
			if (changedState)
			{
				base.SAnima.AnimationState.SetAnimation(0, "move", loop: true);
				if (pattern == AIPattern.Pattern1)
				{
					base.Anima.Play("Monster51_Move");
				}
				else
				{
					base.Anima.Play("Monster51_Move 1");
				}
				randomMoveTime.RandomResult();
				randomMoveRadius.RandomResult();
			}
			if (!base.HaveTarget)
			{
				GetNearestTarget();
			}
			if (!base.HaveTarget)
			{
				state = MonsterState.Idle;
			}
			else if (pattern == AIPattern.Pattern2 && portalCDTimer > portalCD)
			{
				attackCDTimer = 0f;
				portalCDTimer = 0f;
				state = MonsterState.Teleport;
			}
			else if ((base.HaveTarget && WallBlocked()) || ToTargetDistanceSqr() > attackRadius * attackRadius)
			{
				GetNavInfo(base.TargetPoint);
				SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed);
			}
			else
			{
				state = MonsterState.Idle;
			}
			break;
		case MonsterState.KeepDistance:
			if (changedState)
			{
				base.SAnima.AnimationState.SetAnimation(0, "move", loop: true);
				if (pattern == AIPattern.Pattern1)
				{
					base.Anima.Play("Monster51_Move");
				}
				else
				{
					base.Anima.Play("Monster51_Move 1");
				}
			}
			if (!base.HaveTarget)
			{
				state = MonsterState.Idle;
				break;
			}
			if (pattern == AIPattern.Pattern2)
			{
				if (base.HaveTarget && portalCDTimer > portalCD)
				{
					attackCDTimer = 0f;
					portalCDTimer = 0f;
					if (teleportChance < Random.Range(0f, 1f))
					{
						state = MonsterState.Teleport;
					}
					else
					{
						state = MonsterState.Aim;
					}
					break;
				}
				if (base.HaveTarget && ToTargetDistanceSqr() < attackRadius * attackRadius && attackCDTimer > attackCD)
				{
					attackCDTimer = 0f;
					state = MonsterState.Aim;
					break;
				}
			}
			else if (base.HaveTarget && ToTargetDistanceSqr() < attackRadius * attackRadius && attackCDTimer > attackCD)
			{
				portalCDTimer = 0f;
				attackCDTimer = 0f;
				state = MonsterState.Aim;
				break;
			}
			if (base.HaveTarget && !WallBlocked() && ToTargetDistanceSqr() < keepDistance * keepDistance)
			{
				GetNavInfo(base.TargetPoint + -ToTargetDir() * keepDistance);
				CheckNavInfo();
				SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed);
			}
			else
			{
				state = MonsterState.MoveToTarget;
			}
			break;
		case MonsterState.Idle:
			if (changedState)
			{
				base.SAnima.AnimationState.SetAnimation(0, "idle", loop: true);
				if (pattern == AIPattern.Pattern1)
				{
					base.Anima.Play("Monster51_Idle");
				}
				else
				{
					base.Anima.Play("Monster51_Idle 1");
				}
				idleTime.RandomResult();
			}
			if (!base.HaveTarget)
			{
				GetNearestTarget(checkWall: true);
			}
			if (stateExistTime > idleTime.result)
			{
				state = MonsterState.RandomMove;
				break;
			}
			if (base.HaveTarget && (WallBlocked() || ToTargetDistanceSqr() > attackRadius * attackRadius))
			{
				state = MonsterState.MoveToTarget;
				break;
			}
			if (base.HaveTarget && !WallBlocked() && ToTargetDistanceSqr() < keepDistance * keepDistance)
			{
				state = MonsterState.KeepDistance;
				break;
			}
			if (pattern == AIPattern.Pattern2)
			{
				if (base.HaveTarget && portalCDTimer > portalCD)
				{
					attackCDTimer = 0f;
					portalCDTimer = 0f;
					if (teleportChance < Random.Range(0f, 1f))
					{
						state = MonsterState.Teleport;
					}
					else
					{
						state = MonsterState.Aim;
					}
					break;
				}
				if (base.HaveTarget && ToTargetDistanceSqr() < attackRadius * attackRadius && attackCDTimer > attackCD)
				{
					attackCDTimer = 0f;
					state = MonsterState.Aim;
					break;
				}
			}
			else if (base.HaveTarget && ToTargetDistanceSqr() < attackRadius * attackRadius && attackCDTimer > attackCD)
			{
				portalCDTimer = 0f;
				attackCDTimer = 0f;
				state = MonsterState.Aim;
				break;
			}
			SetMove(Vector3.zero, isFlip: false);
			break;
		case MonsterState.Aim:
			if (changedState)
			{
				base.SAnima.AnimationState.SetAnimation(0, "aim&attack", loop: false);
				if (pattern == AIPattern.Pattern1)
				{
					base.Anima.Play("Monster51_Aim");
				}
				else
				{
					base.Anima.Play("Monster51_Aim 1");
				}
			}
			if (stateExistTime > aimTime)
			{
				state = MonsterState.Attack;
				break;
			}
			SetMove(Vector3.zero);
			if (base.HaveTarget)
			{
				SetFlip(ToTargetDir().x);
			}
			break;
		case MonsterState.Attack:
			if (changedState)
			{
				if (pattern == AIPattern.Pattern1)
				{
					base.Anima.Play("Monster51_Attack");
				}
				else
				{
					base.Anima.Play("Monster51_Attack 1");
				}
			}
			break;
		case MonsterState.Teleport:
			if (changedState)
			{
				GetTeleportPoint();
				base.SAnima.AnimationState.SetAnimation(0, "teleport", loop: false);
				base.Anima.Play("Monster51_Teleport");
				beforeTeleportPoint = base.transform.position;
			}
			SetMove(Vector3.zero, isFlip: false);
			if (base.HaveTarget)
			{
				SetFlip(ToTargetDir().x);
			}
			portalAnima1.transform.position = Tool2D.GetLayerPoint(beforeTeleportPoint, LayerCorrectType.GroundEffect);
			portalAnima2.transform.position = Tool2D.GetLayerPoint(teleportPoint, LayerCorrectType.GroundEffect);
			if (!base.HaveTarget)
			{
				aimRenderer.enabled = false;
			}
			if (base.HaveTarget && aimTracking)
			{
				aimRenderer.enabled = true;
				aimRenderer.transform.position = Tool2D.GetLayerPoint(base.TargetPointIgnoreZ, LayerCorrectType.GroundEffect);
			}
			break;
		}
		if (pattern == AIPattern.Pattern2)
		{
			SetSingleFlip(portalSprite1, flipX: false);
			SetSingleFlip(portalSprite2, flipX: false);
		}
	}

	public void GetTeleportPoint()
	{
		if (base.HaveTarget)
		{
			for (int i = 0; i < 30; i++)
			{
				teleportPoint = Tool2D.GetNavMeshPoint(base.TargetPoint, teleportRadius);
				if (Vector3.SqrMagnitude(base.transform.position - teleportPoint) > teleportMinDistance * teleportMinDistance)
				{
					break;
				}
			}
		}
		else
		{
			teleportPoint = Tool2D.GetNavMeshPoint(base.transform.position, teleportRadius);
		}
		teleportPoint = Tool2D.IgnoreZPoint(teleportPoint);
	}

	public override void Theme6Reposition(Vector3 changeValue)
	{
		tongue.LockMotion();
		base.Theme6Reposition(changeValue);
		tongue.UnlockMotion();
	}

	public override void AnimaAction(string animaName)
	{
		switch (animaName)
		{
		case "Attack":
			if (base.HaveTarget)
			{
				Vector3 vector = ((!(targetEntity == PlayerMgr.Inst.PlayerEtt)) ? ((Vector3)GetComponentData<UnitBase_Dots>(targetEntity).currentMotion) : PlayerMgr.Inst.PlayerCtrller.CurrentMotion);
				float preAimTime = GetPreAimTime(Tool2D.IgnoreZPoint(base.TargetPoint - base.transform.position), vector);
				if (preAimTime < 0f)
				{
					aimDir = ToTargetDir();
				}
				else
				{
					aimDir = (Tool2D.IgnoreZPoint(base.TargetPoint - base.transform.position) + preAimTime * vector).normalized;
				}
			}
			else
			{
				aimDir = Tool2D.GetDir();
			}
			if (pattern == AIPattern.Pattern1)
			{
				UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
				sSPModifier.Direction = aimDir;
				sSPModifier.SpawnPosition = base.transform.position + new Vector3(0f, 0f, 0f - spellHeight);
				sSPModifier.ApplyToSSP(ref ssp);
				ShootSpell(ssp);
			}
			else
			{
				string text2 = "EF_Monster51_Spear";
				if (GameMgr.IsChAge14_Static)
				{
					text2 = "EF_Monster51_Spear_H";
				}
				for (int i = 0; i < 3; i++)
				{
					Vector3 normalized = Tool2D.GetDir(aimDir, (float)i * trapleSpearAngle - trapleSpearAngle).normalized;
					ObjPoolMgr.Inst.GetGO("Prefabs/EF/" + text2, base.transform.position).GetComponent<Monster51_Spear>().Initialize(normalized, spellSpeed, this);
				}
			}
			SEMgr.Inst.monster51_Attack.PlaySE();
			break;
		case "AttackFinish":
			state = MonsterState.Idle;
			break;
		case "PortalShow":
		{
			portalAnima1.Play("Monster51_PortalShow");
			portalAnima2.Play("Monster51_PortalShow");
			SEMgr.Inst.monster51_Teleport.PlaySE();
			UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
			componentData.unitCfg.frozenTimeRatio = 0f;
			SetComponentData(componentData);
			break;
		}
		case "PortalHide":
		{
			portalAnima1.Play("Monster51_PortalHide");
			portalAnima2.Play("Monster51_PortalHide");
			UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
			componentData.unitCfg.frozenTimeRatio = originalFrozenTimeRatio;
			SetComponentData(componentData);
			break;
		}
		case "PortalIn":
			thisShadow.Hide();
			tongue.LockMotion();
			break;
		case "PortalOut":
			aimTracking = true;
			base.SAnima.AnimationState.SetAnimation(0, "teleport_attack", loop: false);
			tongue.UnlockMotion();
			break;
		case "CloseCollider":
		{
			base.CC_Self.enabled = false;
			SetDotsCCEnable(isOpen: false);
			UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
			componentData.CanTouch = false;
			SetComponentData(componentData);
			break;
		}
		case "OpenCollider":
		{
			base.CC_Self.enabled = true;
			SetDotsCCEnable(isOpen: true);
			UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
			componentData.CanTouch = true;
			SetComponentData(componentData);
			break;
		}
		case "Teleport":
		{
			base.transform.position = teleportPoint;
			portalAnima1.transform.position = Tool2D.GetLayerPoint(beforeTeleportPoint, LayerCorrectType.GroundEffect);
			portalAnima2.transform.position = Tool2D.GetLayerPoint(teleportPoint, LayerCorrectType.GroundEffect);
			LocalTransform componentData2 = GetComponentData<LocalTransform>();
			componentData2.Position = teleportPoint;
			SetComponentData(componentData2);
			break;
		}
		case "TeleportAttack":
		{
			Vector3 targetPoint = Tool2D.IgnoreZPoint(Tool2D.GetNavMeshPoint(base.transform.position, teleportRadius));
			aimTracking = false;
			if (base.HaveTarget)
			{
				targetPoint = base.TargetPointIgnoreZ;
			}
			SEMgr.Inst.monster51_Attack.PlaySE();
			string text = "EF_Monster51_GroundSpear";
			if (GameMgr.IsChAge14_Static)
			{
				text = "EF_Monster51_GroundSpear_H";
			}
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/" + text, base.transform.position + new Vector3(0f, 0f, 0f - groundSpearHeight), 5f).GetComponent<Monster51_GroundSpear>().Initialize(targetPoint, this);
			break;
		}
		case "ShadowShow":
			thisShadow.Show();
			break;
		case "TeleportFinish":
			state = MonsterState.MoveToTarget;
			break;
		}
	}
}
