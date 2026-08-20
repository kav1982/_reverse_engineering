using System;
using System.Collections.Generic;
using System.Linq;
using SpriteEffectSystem;
using UnityEngine;

namespace _Scripts.Units;

public class Teammate7 : Teammate
{
	private float spawnInterval;

	private float spawnTimer;

	public Shadow SelfShadow;

	public GameObject HoleObject;

	public Transform HoleFollowTransform;

	private float leftHoleXPosShift = -0.5f;

	private float rightHoleXPosShift = 0.5f;

	private float leftrightHoleYPosShift = 0.1f;

	private float eachLevelYPosShift = 0.5f;

	private List<Spell2007WormHole> holeList = new List<Spell2007WormHole>();

	private float normalSpawnBugHpCostRatio = 0.06f;

	private bool deathSpawnWorm = true;

	public float EssenceFallSpeed;

	public float EsscenFallStartHeight;

	private new bool isFalling;

	public ShockParam shockParam;

	private bool essenceExplosionDone;

	private bool fallFlyHasApply;

	public SpriteEffectAnima ExplosionAnima;

	public override void EveryInitialCallback()
	{
		base.EveryInitialCallback();
		base.Anima.SetTrigger("Idle");
		base.Anima.speed = 1f;
		HideAllWormHole();
		isFalling = false;
		essenceExplosionDone = false;
		fallFlyHasApply = false;
	}

	public override void Frame1InitialCallback()
	{
		base.Frame1InitialCallback();
		base.SummonerSpellBase.GetAroundTargetBasePoint();
		CalculateSpawnInterval();
		base.SummonerSpellBase.endTHunderHitChance = ((base.SummonerSpellBase.ColorType == SpellColorType.Thunder) ? 1 : 0);
		spawnTimer = spawnInterval - Mathf.Min(spawnInterval, 0.4f);
		base.Anima.speed = base.SummonerSpellBase.GetSummonValueRatio().attackSpeedRatio;
		ShowTeammate();
		base.Anima.SetTrigger("Idle");
		WormHoleDataInitialize();
		deathSpawnWorm = true;
		if (base.SummonerSpellBase.SIP.summonAdvanceSkillType1Level > 0)
		{
			base.transform.position = GetFallEndPos();
			base.transform.position += new Vector3(0f, 0f, 0f - EsscenFallStartHeight);
			isFalling = true;
			fallFlyHasApply = true;
			myPpt.FlyRegister();
			SEMgr.Inst.teammate7EssenceShoot.PlaySE(SEPlayMode.Replay, 3, 0.1f);
		}
	}

	private Vector3 GetFallEndPos()
	{
		if (base.SummonerSpellBase.SIP.finalShootSpatialInfo.Target.HasValue)
		{
			switch (base.SummonerSpellBase.currentSpellMovement)
			{
			case SpellSpecialMovementType.Normal:
				return base.SummonerSpellBase.SIP.finalShootSpatialInfo.Target.Value;
			case SpellSpecialMovementType.ChaseOwner:
				return RecalculateScatter(base.SummonerSpellBase.transform.position);
			default:
				throw new ArgumentOutOfRangeException();
			case SpellSpecialMovementType.ChaseEnemy:
			case SpellSpecialMovementType.ChaseMouse:
			case SpellSpecialMovementType.Rotation:
				break;
			}
		}
		ShootCause shootCause = base.SummonerSpellBase.SIP.ShootCause;
		if (!(shootCause is ShootCause.BySpell))
		{
			if (!(shootCause is ShootCause.ByUnit))
			{
				if (shootCause is ShootCause.ByWand)
				{
					if (base.SummonerSpellBase.shooterWand.passiveAutoWand && LevelMgr.Inst.CurrentRoomCtrller.CurrentRoomHasValidTarget())
					{
						return Tool2D.GetNavMeshPoint(GetTargetMovementFinalPos(LevelMgr.Inst.CurrentRoomCtrller.GetNearestTargetablePpt(base.SummonerSpellBase.ownerPpt.transform.position).transform.position));
					}
					return Tool2D.GetNavMeshPoint(GetTargetMovementFinalPos(PlayerMgr.Inst.GetMousePoint()));
				}
				return base.transform.position;
			}
			if (LevelMgr.Inst.CurrentRoomCtrller.CurrentRoomHasValidTarget())
			{
				return Tool2D.GetNavMeshPoint(GetTargetMovementFinalPos(LevelMgr.Inst.CurrentRoomCtrller.GetNearestTargetablePpt(base.SummonerSpellBase.ownerPpt.transform.position).transform.position));
			}
			return Tool2D.GetNavMeshPoint(GetTargetMovementFinalPos(PlayerMgr.Inst.GetMousePoint()));
		}
		if (base.SummonerSpellBase.SIP.finalShootSpatialInfo.Target.HasValue)
		{
			return Tool2D.GetNavMeshPoint(GetTargetMovementFinalPos(base.SummonerSpellBase.SIP.finalShootSpatialInfo.Target.Value));
		}
		return Tool2D.GetNavMeshPoint(GetTargetMovementFinalPos(base.SummonerSpellBase.OwnerSpell.transform.position));
	}

	private Vector3 GetTargetMovementFinalPos(Vector3 pos)
	{
		bool flag = false;
		switch (base.SummonerSpellBase.currentSpellMovement)
		{
		case SpellSpecialMovementType.Normal:
			flag = true;
			break;
		case SpellSpecialMovementType.ChaseEnemy:
		{
			UnitProperty nearestTargetablePpt = LevelMgr.Inst.CurrentRoomCtrller.GetNearestTargetablePpt(pos);
			if ((bool)nearestTargetablePpt && Tool2D.IgnoreZDistance(nearestTargetablePpt.transform.position, pos) <= base.SummonerSpellBase.spellFollowTargetRotateSpeed * 0.4f)
			{
				pos = nearestTargetablePpt.transform.position;
				flag = true;
				break;
			}
			return pos;
		}
		case SpellSpecialMovementType.ChaseMouse:
			pos = PlayerMgr.Inst.GetMousePoint();
			flag = true;
			break;
		case SpellSpecialMovementType.Rotation:
			pos = base.SummonerSpellBase.GetAroundTargetBasePoint() + Tool2D.GetDir(UnityEngine.Random.Range(0f, 360f)) * base.SummonerSpellBase.spellAroundOwnerRadius;
			break;
		case SpellSpecialMovementType.ChaseOwner:
			pos = base.SummonerSpellBase.GetAroundTargetBasePoint();
			flag = true;
			break;
		}
		if (flag)
		{
			pos += UnityEngine.Random.insideUnitSphere.IgnoreZ() * base.SummonerSpellBase._angle * 0.03f;
		}
		return pos;
	}

	private Vector3 RecalculateScatter(Vector3 pos)
	{
		return pos + UnityEngine.Random.insideUnitSphere.IgnoreZ() * base.SummonerSpellBase._angle * 0.03f;
	}

	private void UpdateEssenceFallingState()
	{
		if (isFalling)
		{
			float num = (EssenceFallSpeed + base.SummonerSpellBase.bonusSpeed) * base.SummonerSpellBase.speedRatio * base.SummonerSpellBase.finalSpeedRatio * (1f + (float)(FusionData.CurrentFusionLevel - 1) * 0.2f);
			float num2 = base.transform.position.z + num * Time.deltaTime;
			if (num2 >= 0f)
			{
				isFalling = false;
				base.transform.position = base.transform.position.IgnoreZ();
				num2 = 0f;
				base.Anima.SetTrigger("Landing");
				EssenceSkillLandingEffect();
			}
			base.transform.position = base.transform.position.IgnoreZ() + new Vector3(0f, 0f, num2);
		}
	}

	private void EssenceSkillLandingEffect()
	{
		if (!essenceExplosionDone)
		{
			essenceExplosionDone = true;
			if (fallFlyHasApply)
			{
				myPpt.FlyUnregister();
				fallFlyHasApply = false;
			}
			float num = 3.5f * base.SummonerSpellBase.radiusRatio * base.SummonerSpellBase.finalRadiusRatio;
			float num2 = myPpt.unitCfg.maxHP * 180f / 100f * (float)base.SummonerSpellBase.SIP.summonAdvanceSkillType1Level * base.SummonerSpellBase.damageRatio * base.SummonerSpellBase.finalDamageRatio + base.SummonerSpellBase.SIP.finalDamageExtra;
			num2 *= 1f + GeneralTool.GetSpellRadiusToDamageRatio(num, base.SummonerSpellBase.SIP.radiuDecreaseRatio, base.SummonerSpellBase.SIP.radiuDcreaseTransIntoDamageRatio);
			EssenceGroundExplosion(num, num2);
		}
	}

	private void EssenceGroundExplosion(float EffectRange, float hitDamage)
	{
		List<Collider> essenceHeavyHitTargetList = GetEssenceHeavyHitTargetList(EffectRange);
		base.SummonerSpellBase.GetEffect("SpawnBomb_" + base.SummonerSpellBase.ColorType, base.transform.position, 1.5f).transform.localScale = Vector3.one * EffectRange;
		SEMgr.Inst.teammate7EssenceExplosion.PlaySE(SEPlayMode.Replay, 3, 0.1f);
		ShockParam shockParam = this.shockParam;
		shockParam.radius *= Mathf.Min(1f + (float)(FusionData.CurrentFusionLevel - 1) / 2f, 3.5f);
		shockParam.speed *= Mathf.Min(1f + (float)(FusionData.CurrentFusionLevel - 1) / 2f, 3.5f);
		shockParam.time *= Mathf.Min(1f + (float)(FusionData.CurrentFusionLevel - 1) * 0.2f, 1f);
		CamController.Inst.SetShock(shockParam, new Vector3(0f, -1f, 0f));
		foreach (Collider item in essenceHeavyHitTargetList.Where((Collider e) => e.gameObject.activeInHierarchy))
		{
			if (item.gameObject.CompareAnyTag("Spell", "RollBall", "Butterfly"))
			{
				SpellBase componentInParent = item.GetComponentInParent<SpellBase>();
				if (!(componentInParent is Spell1002RollBall spell1002RollBall))
				{
					if (componentInParent is Spell1003Butterfly spell1003Butterfly)
					{
						spell1003Butterfly.HitEFAndRecycle();
					}
				}
				else
				{
					spell1002RollBall.TakeDamage(hitDamage);
				}
			}
			else if (item.gameObject.CompareAnyTag("Monster"))
			{
				TakeDamageInfo takeDamageInfo = new TakeDamageInfo
				{
					damage = hitDamage,
					canRebound = false
				};
				UnitProperty component = item.GetComponent<UnitProperty>();
				base.SummonerSpellBase.spellCfg.damage = takeDamageInfo.damage;
				base.SummonerSpellBase.ApplyVoidEffect(component);
				base.SummonerSpellBase.OutputDamage(component, takeDamageInfo, SpellAbilityType.TeammateSprite);
			}
			else
			{
				TakeDamageInfo info = new TakeDamageInfo
				{
					damage = hitDamage,
					canRebound = false
				};
				base.SummonerSpellBase.OutputDamage(item.gameObject, info);
			}
		}
	}

	private List<Collider> GetEssenceHeavyHitTargetList(float EffectRange)
	{
		return GeneralTool.GetCollidersByTag(base.transform.position, EffectRange, "Monster", "Destructible", "SolidObj", "RollBall", "Butterfly", "Brittleness");
	}

	private void HideAllWormHole()
	{
		foreach (Spell2007WormHole hole in holeList)
		{
			hole.gameObject.SetActive(value: false);
		}
	}

	private void WormHoleDataInitialize()
	{
		int num = FusionData.CurrentFusionLevel + 1 - holeList.Count;
		if (num > 0)
		{
			for (int i = 0; i < num; i++)
			{
				GameObject gameObject = SpawnHole();
				holeList.Add(gameObject.GetComponent<Spell2007WormHole>());
			}
		}
		for (int j = 0; j <= FusionData.CurrentFusionLevel; j++)
		{
			holeList[j].gameObject.SetActive(value: true);
			holeList[j].SetColor(base.SummonerSpellBase.ColorType);
			holeList[j].transform.localPosition = new Vector3(0f, eachLevelYPosShift * (float)Mathf.FloorToInt((float)j / 3f), 0.01f * (float)Mathf.FloorToInt((float)j / 3f));
			if (j % 3 == 1)
			{
				holeList[j].transform.localPosition += new Vector3(leftHoleXPosShift, leftrightHoleYPosShift, 0.02f);
			}
			else if (j % 3 == 2)
			{
				holeList[j].transform.localPosition += new Vector3(rightHoleXPosShift, leftrightHoleYPosShift, 0.02f);
			}
		}
		myPpt.SR_Models = GetComponentsInChildren<SpriteRenderer>(includeInactive: true);
	}

	private GameObject SpawnHole()
	{
		return UnityEngine.Object.Instantiate(HoleObject, HoleFollowTransform);
	}

	public override void OnEnterDelayDeathEvent()
	{
		ShowTeammate();
		base.OnEnterDelayDeathEvent();
		if (base.SummonerSpellBase.SIP.SpellSummonimmuteDeathTime <= 0f)
		{
			return;
		}
		foreach (Spell2007WormHole hole in holeList)
		{
			hole.EnterDelayDeathState();
		}
		SummonGhostEffectToggle(state: true);
		ColliderToggle(state: false);
		FreeFromTeammate6();
	}

	public override void OnEnterFuseStateEvent()
	{
		base.OnEnterFuseStateEvent();
		foreach (Spell2007WormHole hole in holeList)
		{
			hole.EnterFuseState();
		}
		SelfShadow.ShadowGO.SetActive(value: false);
	}

	public void ControldByTeammate6()
	{
		base.CanMove = false;
		ColliderToggle(state: false);
		HideTeammate();
		base.beingControlledByTeammate6 = true;
	}

	public void FreeFromTeammate6()
	{
		if (base.beingControlledByTeammate6)
		{
			base.CanMove = true;
			base.transform.eulerAngles = Vector3.zero;
		}
	}

	public override void HideTeammate()
	{
		myPpt.tsf_Layer.gameObject.SetActive(value: false);
		SelfShadow.ShadowGO.SetActive(value: false);
	}

	public override void ShowTeammate()
	{
		myPpt.tsf_Layer.gameObject.SetActive(value: true);
		SelfShadow.ShadowGO.SetActive(value: true);
	}

	public override void Update()
	{
		base.Update();
		UpdateEssenceFallingState();
		SummonsTouchMonster();
		StateUpdate();
	}

	private void CalculateSpawnInterval()
	{
		spawnInterval = base.SummonerSpellBase.spellCfg.float2 / base.SummonerSpellBase.GetSummonValueRatio().attackSpeedRatio;
	}

	private void StateUpdate()
	{
		if (!isFalling)
		{
			spawnTimer += Time.deltaTime;
			if (!(spawnTimer < spawnInterval) || !base.CanMove)
			{
				spawnTimer -= spawnInterval;
				base.Anima.SetTrigger("Attack");
			}
		}
	}

	private void SpawnExplosionWorm(int spawnCount)
	{
		if (spawnCount > 0)
		{
			float num = UnityEngine.Random.Range(0f, 360f);
			spawnCount *= FusionData.CurrentFusionLevel + 1;
			SEMgr.Inst.teammate7Shoot.PlaySE(SEPlayMode.Replay, 3, 0.1f);
			int num2 = spawnCount;
			float spawnCountFinalDamageRatio = 1f;
			if (GeneralTool.IsLowFpsOptimizeActive(40f))
			{
				num2 = Mathf.CeilToInt((float)spawnCount * GameMgr.Inst.GetFps() / 40f);
				spawnCountFinalDamageRatio = (float)spawnCount / (float)num2;
			}
			for (int i = 0; i < num2; i++)
			{
				Vector3 v = base.transform.position + Tool2D.GetDir(num + 360f / (float)num2 * (float)i) * 0.5f;
				ObjPoolMgr.Inst.GetGO("Prefabs/Units/" + 705401, Tool2D.GetNavMeshPointIngoreZ(Tool2D.IgnoreZPoint(v)), Quaternion.identity).GetComponent<SpellExplosionBug>().ApplySpellEffect(base.SummonerSpellBase, myPpt.unitCfg.maxHP, spawnCountFinalDamageRatio);
			}
		}
	}

	public override void BeforeTakeDamage(TakeDamageInfo info)
	{
		base.BeforeTakeDamage(info);
		if (deathSpawnWorm)
		{
			SpawnExplosionWorm(base.SummonerSpellBase.spellCfg.int1 * (FusionData.CurrentFusionLevel + 1));
		}
	}

	public override void AnimaAction(string animaName)
	{
		if (animaName == "SpawnExplosionBug")
		{
			myPpt.TakeDamage(myPpt.unitCfg.maxHP * normalSpawnBugHpCostRatio, myPpt, new TakeDamageInfo
			{
				beHitColor = false,
				beHitShake = false
			});
		}
	}

	protected override void SummonHpRecoverOrTakedamage(bool independentEffect = false)
	{
		base.SummonHpRecoverOrTakedamage(independentEffect: true);
	}

	public override void SummonsThrough()
	{
		if (SummonMayThroughMap())
		{
			SummonFollowOwnerThroughMap();
			return;
		}
		base.SummonerSpellBase.SpellSummonAfterDeadSpawnWormCount = 0;
		base.SummonerSpellBase.SIP.SpellSummonimmuteDeathTime = 0f;
		myPpt.ClearVoidState();
		deathSpawnWorm = false;
		base.SummonsThrough();
		myPpt.AnnouncedDeath(new TakeDamageInfo
		{
			isPlayDeadSE = false,
			isCreateDeadEF = false,
			isTeammateThrough = true
		});
	}

	public override void AfterDead(ref TakeDamageInfo_Dots info)
	{
		if (deathSpawnWorm)
		{
			SpawnExplosionWorm(base.SummonerSpellBase.spellCfg.int2 * (FusionData.CurrentFusionLevel + 1));
		}
		if (base.SummonerSpellBase.SIP.summonAdvanceSkillType1Level > 0 && isFalling)
		{
			isFalling = false;
			base.transform.position = base.transform.position.IgnoreZ();
			EssenceSkillLandingEffect();
		}
		base.AfterDead(ref info);
	}

	private void OnDisable()
	{
		if (fallFlyHasApply)
		{
			myPpt.FlyUnregister();
		}
	}
}
