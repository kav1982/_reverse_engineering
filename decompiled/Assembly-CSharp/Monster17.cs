using System.Collections.Generic;
using Unity.Transforms;
using UnityEngine;

public class Monster17 : UnitBase
{
	private enum MonsterState
	{
		BornIdle,
		MoveRandom,
		MoveToTarget,
		WaitActionBefore,
		Hiding
	}

	[Range(0f, 1f)]
	[Space(50f)]
	public float moveToTargetChance;

	public VariableFloat moveRandomRadius;

	public VariableFloat moveTime;

	public VariableFloat hideTime;

	public float hideKnockbackRatio;

	[Header("Pattern")]
	public AIPattern pattern;

	public int hatID;

	public Monster17_Chain pfb_Monster17Chain;

	public GameObject chainRoot;

	private float originalKnockbackRatio;

	private MonsterState state;

	private float moveTimer;

	private float hideTimer;

	private Monster17_Chain monster17Chain;

	private Vector3 originChainRoot;

	private List<SpellAbilityType> allowRebounceType = new List<SpellAbilityType>
	{
		SpellAbilityType.Bullet,
		SpellAbilityType.Rollball,
		SpellAbilityType.Butterfly,
		SpellAbilityType.Laser,
		SpellAbilityType.PreFirework,
		SpellAbilityType.HoverTorch,
		SpellAbilityType.BackMP,
		SpellAbilityType.SnakeWalk,
		SpellAbilityType.Rollball,
		SpellAbilityType.ArcaneNova,
		SpellAbilityType.Dash,
		SpellAbilityType.ManaCoin,
		SpellAbilityType.Boomerang,
		SpellAbilityType.ShiningStar,
		SpellAbilityType.MrBingArrow,
		SpellAbilityType.DimensionTraveller,
		SpellAbilityType.ShotGun,
		SpellAbilityType.BulletParabola
	};

	public override void SingleInitialCallback()
	{
		originalKnockbackRatio = myPpt.unitCfg.knockbackRatio;
	}

	public override void EveryInitialCallback()
	{
		state = MonsterState.BornIdle;
		moveTimer = 0f;
		hideTimer = 0f;
		moveTime.RandomResult();
		hideTime.RandomResult();
		base.Anima.Play("Monster17_Idle");
		if (pattern == AIPattern.Pattern3)
		{
			monster17Chain = Object.Instantiate(pfb_Monster17Chain, chainRoot.transform.position, base.transform.rotation, LevelMgr.Inst.CurrentRoomT);
			monster17Chain.mainTail = chainRoot;
			originChainRoot = chainRoot.transform.localPosition;
		}
	}

	public override void Update()
	{
		if (pattern == AIPattern.Pattern3 && myPpt.BaseColor != monster17Chain.chainColor)
		{
			monster17Chain.chainColor = myPpt.BaseColor;
		}
		base.Update();
		if (base.IsLocked)
		{
			return;
		}
		switch (state)
		{
		case MonsterState.BornIdle:
			SetMove(Vector3.zero, isFlip: false);
			fakeMoveFlip(0f);
			bornIdleTimer += Time.deltaTime;
			if (!(bornIdleTimer >= 0.5f))
			{
				break;
			}
			base.Anima.SetTrigger("Walk");
			if (Random.value <= moveToTargetChance)
			{
				GetNearestTarget();
				if (base.HaveTarget)
				{
					state = MonsterState.MoveToTarget;
					break;
				}
				state = MonsterState.MoveRandom;
				GetNavInfo(Tool2D.GetNavMeshPoint(base.transform.position, moveRandomRadius));
			}
			else
			{
				state = MonsterState.MoveRandom;
				GetNavInfo(Tool2D.GetNavMeshPoint(base.transform.position, moveRandomRadius));
			}
			break;
		case MonsterState.MoveRandom:
			if (navInfo.allCornerArrived)
			{
				GetNavInfo(Tool2D.GetNavMeshPoint(base.transform.position, moveRandomRadius));
			}
			else
			{
				SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed);
				fakeMoveFlip(ToPointDir(navInfo.ToGoPoint).x);
				CheckNavInfo();
			}
			moveTimer += Time.deltaTime;
			if (moveTimer >= moveTime.result)
			{
				moveTimer = 0f;
				moveTime.RandomResult();
				base.Anima.SetTrigger("Hide");
				UnitProperty_Dots componentData3 = GetComponentData<UnitProperty_Dots>();
				componentData3.unitCfg.knockbackRatio = hideKnockbackRatio;
				SetComponentData(componentData3);
			}
			break;
		case MonsterState.MoveToTarget:
			if (!base.HaveTarget)
			{
				GetNearestTarget();
			}
			if (!base.HaveTarget)
			{
				state = MonsterState.MoveRandom;
				GetNavInfo(Tool2D.GetNavMeshPoint(base.transform.position, moveRandomRadius));
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
			fakeMoveFlip(ToPointDir(navInfo.ToGoPoint).x);
			moveTimer += Time.deltaTime;
			if (moveTimer >= moveTime.result)
			{
				moveTimer = 0f;
				moveTime.RandomResult();
				base.Anima.SetTrigger("Hide");
				UnitProperty_Dots componentData2 = GetComponentData<UnitProperty_Dots>();
				componentData2.unitCfg.knockbackRatio = hideKnockbackRatio;
				SetComponentData(componentData2);
			}
			break;
		case MonsterState.WaitActionBefore:
			SetMove(Vector3.zero);
			break;
		case MonsterState.Hiding:
			SetMove(Vector3.zero);
			hideTimer += Time.deltaTime;
			if (hideTimer >= hideTime.result)
			{
				hideTimer = 0f;
				hideTime.RandomResult();
				base.Anima.SetTrigger("Show");
				state = MonsterState.WaitActionBefore;
				UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
				componentData.unitCfg.knockbackRatio = originalKnockbackRatio;
				componentData.unitCfg.immuneSpike = false;
				SetComponentData(componentData);
			}
			break;
		default:
			Debug.LogError(state);
			break;
		}
	}

	private void fakeMoveFlip(float x)
	{
		if (pattern == AIPattern.Pattern3 && chainRoot != null)
		{
			if (x >= 0f)
			{
				chainRoot.transform.localPosition = originChainRoot;
			}
			else
			{
				chainRoot.transform.localPosition = new Vector3(0f - originChainRoot.x, originChainRoot.y, originChainRoot.z);
			}
		}
	}

	public override void AnimaAction(string animaName)
	{
		if (!(animaName == "HideFinish"))
		{
			if (animaName == "ShowFinish")
			{
				if (Random.value <= moveToTargetChance)
				{
					GetNearestTarget();
					if (base.HaveTarget)
					{
						state = MonsterState.MoveToTarget;
						return;
					}
					state = MonsterState.MoveRandom;
					GetNavInfo(Tool2D.GetNavMeshPoint(base.transform.position, moveRandomRadius));
				}
				else
				{
					state = MonsterState.MoveRandom;
					GetNavInfo(Tool2D.GetNavMeshPoint(base.transform.position, moveRandomRadius));
				}
			}
			else
			{
				Debug.LogError(animaName);
			}
		}
		else
		{
			state = MonsterState.Hiding;
			UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
			componentData.unitCfg.immuneSpike = true;
			SetComponentData(componentData);
		}
	}

	public override void BeforeTakeDamage_Dots(ref TakeDamageInfo_Dots info)
	{
		if (state == MonsterState.Hiding)
		{
			info.immuneDamage = true;
			if (pattern != 0 && UnitDotsSyncSystem.EntityIsValid(info.spell.Entity) && !info.spell.Movement.IsFallSpell && (info.spell.Config.ShooterType == UnitType.Teammate || info.spell.Config.ShooterType == UnitType.Player) && allowRebounceType.Contains(info.spell.Config.AbilityType))
			{
				UnitDotsSyncSystem.entityMgr.SetComponentEnabled<SpellDestroyTag>(info.spell.Entity, value: true);
				SpellSpawnParams ssp = Monster17_Hat.CreateRebounceSpell(info.spell, GetComponentData<LocalTransform>(info.spell.Entity).Position, this);
				ShootSpell(ssp);
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Rebound", Tool2D.GetLayerPoint(ssp.SpawnPosition), Vector3.one * 0.2f, 1f);
			}
		}
	}

	public override void AfterDead(ref TakeDamageInfo_Dots info)
	{
		if (pattern == AIPattern.Pattern3)
		{
			_ = monster17Chain.chainColor;
			monster17Chain.chainColor = new Color(1f, 1f, 1f, 1f);
			monster17Chain.mainTail = null;
			Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Units/" + hatID), base.transform.position, Quaternion.identity, LevelMgr.Inst.CurrentRoomT).SetActive(value: true);
		}
		base.AfterDead(ref info);
	}
}
