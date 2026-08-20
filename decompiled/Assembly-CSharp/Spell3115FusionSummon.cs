using System.Collections;
using UnityEngine;

public class Spell3115FusionSummon : SpellBase
{
	private Teammate target1;

	private Teammate target2;

	private UnitProperty summonPpt;

	private Spell3110LifeLine lineScript;

	private float fuseBookMpGenSpeed;

	public override void OnEnable()
	{
		base.OnEnable();
		target1 = null;
		target2 = null;
		summonPpt = null;
		fuseBookMpGenSpeed = 0f;
	}

	public override void Update()
	{
		base.Update();
		if ((bool)summonPpt && (!summonPpt.gameObject.activeSelf || summonPpt.unitCfg.currentHP <= 0f))
		{
			PoolRecycle();
		}
	}

	private void SpawnFusionTeammate(Vector3 spawnPos)
	{
		Teammate teammate = ((target1.FusionData.CurrentFusionLevel >= target2.FusionData.CurrentFusionLevel) ? target1 : target2);
		int maxFusionLevel = Mathf.Max(target1.FusionData.MaxFusionLevel, target2.FusionData.MaxFusionLevel);
		float num = (float)teammate.FusionData.CurrentFusionLevel + 2f;
		InitializeV2(teammate.SummonerSpellBase.SIP, teammate.SummonerSpellBase.ShootData);
		base.ColorType = teammate.SummonerSpellBase.ColorType;
		if (ownerPpt.tag == null)
		{
			PoolRecycle();
			return;
		}
		if (ownerPpt.unitCfg.unitType != 0 && ownerPpt.unitCfg.unitType != UnitType.Teammate && ownerPpt.unitCfg.unitType != UnitType.TeammateNotAttack)
		{
			Debug.LogError(ownerPpt.unitCfg.unitType);
			PoolRecycle();
			return;
		}
		base.enableAroundPlayer = false;
		enableFollowMouse = false;
		enableFollowTarget = false;
		Vector3 vector = spawnPos;
		if (base.currentSpellMovement == SpellSpecialMovementType.Rotation)
		{
			vector += Tool2D.GetDir(Random.Range(0f, 360f)) * base.spellAroundOwnerRadius;
		}
		string text = "";
		switch (base.spellCfg.abilityType)
		{
		case SpellAbilityType.Summon1:
			text = "700111";
			break;
		case SpellAbilityType.Summon2:
			text = "700211";
			break;
		case SpellAbilityType.Summon3:
			text = "700311";
			break;
		case SpellAbilityType.Summon4:
			text = "700411";
			break;
		case SpellAbilityType.Summon5:
			text = "700511";
			break;
		case SpellAbilityType.Summon6:
			text = "700601";
			break;
		case SpellAbilityType.Summon7:
			text = "700701";
			break;
		default:
			Debug.LogError("融合法术不包含该类法术的融合单位或该类法术不为合法的召唤物法术" + base.spellCfg.abilityType);
			PoolRecycle();
			return;
		}
		Vector3 navMeshPointIngoreZ = Tool2D.GetNavMeshPointIngoreZ(vector + Random.insideUnitSphere * 0.1f);
		summonPpt = PlayerMgr.Inst.MiniPool.GetGO("Prefabs/Units/" + text, navMeshPointIngoreZ).GetComponent<UnitProperty>();
		if (base.spellCfg.abilityType != SpellAbilityType.Summon3 && base.spellCfg.abilityType != SpellAbilityType.Summon4 && base.spellCfg.abilityType != SpellAbilityType.Summon7)
		{
			summonPpt.gameObject.layer = LayerMask.NameToLayer("Player");
		}
		UnitBase component = summonPpt.GetComponent<UnitBase>();
		component.SummonsInitial(this);
		int num2 = 0;
		UnitProperty unitProperty = teammate.SummonerSpellBase.ownerPpt;
		float spellSummonGainOwnerHpRatio = teammate.SummonerSpellBase.SpellSummonGainOwnerHpRatio;
		UnitBase unitBas = unitProperty.UnitBas;
		num2 = ((!(unitBas is Teammate5) && !(unitBas is Teammate5FuseController)) ? Mathf.CeilToInt(PlayerMgr.Inst.PlayerPpt.unitCfg.maxHP * spellSummonGainOwnerHpRatio) : Mathf.CeilToInt(unitProperty.unitCfg.maxHP * spellSummonGainOwnerHpRatio));
		if (component is Teammate teammate2)
		{
			teammate2.FusionData.IsFusedUnit = true;
			teammate2.FusionData.CurrentFusionLevel = teammate.FusionData.CurrentFusionLevel + 1;
			teammate2.FusionData.MaxFusionLevel = maxFusionLevel;
			teammate2.FusionData.SourceTeammateID = teammate.SummonerSpellBase.spellCfg.summonID;
			teammate2.myPpt.unitCfg.maxHP = Mathf.CeilToInt((UnitConfig.map[teammate.SummonerSpellBase.spellCfg.summonID].maxHP + (float)num2) * teammate.SummonerSpellBase.SpellSummonHPRatio * teammate.SummonerSpellBase.SpellSUmmonFinalHpRatio * num);
			teammate2.myPpt.unitCfg.currentHP = teammate2.myPpt.unitCfg.maxHP;
			if (teammate2 is Teammate5FuseController teammate5FuseController)
			{
				float mPRegenSpeed = 0f;
				if (teammate is Teammate5 teammate3)
				{
					mPRegenSpeed = teammate3.GetMPRegenSpeed();
				}
				else if (teammate is Teammate5FuseController)
				{
					mPRegenSpeed = fuseBookMpGenSpeed;
				}
				teammate5FuseController.SetMPRegenSpeed(mPRegenSpeed);
			}
		}
		if (base.currentSpellMovement == SpellSpecialMovementType.Rotation)
		{
			if (summonPpt.gameObject.layer != LayerMask.NameToLayer("Teammate_Fly"))
			{
				summonPpt.gameObject.layer = LayerMask.NameToLayer("Teammate_Fly");
			}
			component.myPpt.FlyRegister();
		}
		if (teammate.SummonerSpellBase.voidExplosionInfo != null)
		{
			Spell3129VoidExplosion.VoidExplosionData voidExplosionData = teammate.SummonerSpellBase.voidExplosionInfo.Copy();
			voidExplosionData.ConstVoidEffect = true;
			component.myPpt.SetVoid(voidExplosionData);
		}
		CreateLifeLine();
		if (PlayerMgr.Inst.ItemCtrller.curseCfg_Pestilence != null)
		{
			summonPpt.unitCfg.currentHP = PlayerMgr.Inst.ItemCtrller.curseCfg_Pestilence.int1.result;
		}
	}

	public override void PoolRecycle()
	{
		base.spellSplitCount = 0;
		base.endThunderHitPercent = 0f;
		base.PoolRecycle();
	}

	private void CreateLifeLine()
	{
		if (lifelineDamage > 0f && !(ownerPpt == null))
		{
			lineScript = ObjPoolMgr.Inst.GetGO("Prefabs/Spell/31101").GetComponent<Spell3110LifeLine>();
			lineScript.Iniatialize(ownerPpt, summonPpt.GetComponent<UnitProperty>(), (int)lifelineDamage, lifelineDamageInterval, this);
			if (base.spellCfg.abilityType == SpellAbilityType.Summon2)
			{
				lineScript.transform.parent = LevelMgr.Inst.CurrentRoomT.parent;
				summonPpt.GetComponent<Teammate2>().targetline = lineScript;
			}
		}
	}

	public void StartFusionProgress(Teammate target1, Teammate target2)
	{
		this.target1 = target1;
		this.target2 = target2;
		lineScript = null;
		effectPrefix = this.target1.SummonerSpellBase.effectPrefix;
		SpecialCaseRecordFuseBookMpGenSpeed();
		Vector3 navMeshPointIngoreZ = Tool2D.GetNavMeshPointIngoreZ(target1.transform.position + (target2.transform.position - target1.transform.position) / 2f + Tool2D.IgnoreZPoint(Random.insideUnitSphere) * Random.Range(0f, Vector3.Distance(target2.transform.position, target1.transform.position) / 5f));
		ObjPoolMgr.Inst.GetGO("Prefabs/Spell/" + 31151 + "/31151_ForceCenter", navMeshPointIngoreZ, 2f);
		ObjPoolMgr.Inst.GetGO("Prefabs/Spell/" + 31151 + "/31151_PullingForce", target2.transform.position).GetComponent<Spell3115ForceController>().Initialize(target1.transform, navMeshPointIngoreZ);
		ObjPoolMgr.Inst.GetGO("Prefabs/Spell/" + 31151 + "/31151_PullingForce", target2.transform.position).GetComponent<Spell3115ForceController>().Initialize(target2.transform, navMeshPointIngoreZ);
		float num = 1f;
		float spellSummonimmuteDeathTime = target1.myPpt.UnitBas.SummonerSpellBase.SIP.SpellSummonimmuteDeathTime;
		float spellSummonimmuteDeathTime2 = target2.myPpt.UnitBas.SummonerSpellBase.SIP.SpellSummonimmuteDeathTime;
		target1.myPpt.UnitBas.SummonerSpellBase.SIP.SpellSummonimmuteDeathTime = 0f;
		target2.myPpt.UnitBas.SummonerSpellBase.SIP.SpellSummonimmuteDeathTime = 0f;
		target1.myPpt.TeammateAnnounceDeath(new TeammateAnnounceDeathInfo
		{
			isInstanceDeath = false,
			setOneHp = false,
			delayDeathTime = num
		});
		target1.myPpt.StartFuseProgress();
		target2.myPpt.TeammateAnnounceDeath(new TeammateAnnounceDeathInfo
		{
			isInstanceDeath = false,
			setOneHp = false,
			delayDeathTime = num
		});
		target2.myPpt.StartFuseProgress();
		target1.myPpt.UnitBas.SummonerSpellBase.SIP.SpellSummonimmuteDeathTime = spellSummonimmuteDeathTime;
		target2.myPpt.UnitBas.SummonerSpellBase.SIP.SpellSummonimmuteDeathTime = spellSummonimmuteDeathTime2;
		StartCoroutine(DelaySpawn(num, navMeshPointIngoreZ));
	}

	private IEnumerator DelaySpawn(float summonDuration, Vector3 spawnPos)
	{
		yield return new WaitForSeconds(summonDuration);
		SpawnFusionTeammate(spawnPos);
		ObjPoolMgr.Inst.GetGO("Prefabs/Spell/" + 31151 + "/31151_SummonDone", spawnPos, 2f);
	}

	private void SpecialCaseRecordFuseBookMpGenSpeed()
	{
		if (target1 is Teammate5FuseController teammate5FuseController)
		{
			fuseBookMpGenSpeed = Mathf.Max(teammate5FuseController.GetMPRegenSpeed(), fuseBookMpGenSpeed);
		}
		if (target1 is Teammate5FuseController teammate5FuseController2)
		{
			fuseBookMpGenSpeed = Mathf.Max(teammate5FuseController2.GetMPRegenSpeed(), fuseBookMpGenSpeed);
		}
	}
}
