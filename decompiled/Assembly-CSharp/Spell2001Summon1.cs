using System.Collections.Generic;
using UnityEngine;

public class Spell2001Summon1 : SpellBase, IOnLaunchFromUnitNotPlayer
{
	private UnitProperty summonPpt;

	private Spell3110LifeLine lineScript;

	private static float lastPlayerSummonSoundTime;

	private Vector3 lastFramePosition;

	public override void InitializeCallback()
	{
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
		lineScript = null;
		Vector3 position = base.transform.position;
		if (base.currentSpellMovement == SpellSpecialMovementType.Rotation)
		{
			position += Tool2D.GetDir(Random.Range(0f, 360f)) * base.spellAroundOwnerRadius;
		}
		Vector3 navMeshPointIngoreZ = Tool2D.GetNavMeshPointIngoreZ(position + Random.insideUnitSphere * 0.1f);
		summonPpt = PlayerMgr.Inst.MiniPool.GetGO("Prefabs/Units/" + base.spellCfg.summonID, navMeshPointIngoreZ).GetComponent<UnitProperty>();
		if (base.spellCfg.abilityType != SpellAbilityType.Summon3 && base.spellCfg.abilityType != SpellAbilityType.Summon4 && base.spellCfg.abilityType != SpellAbilityType.Summon7)
		{
			summonPpt.gameObject.layer = LayerMask.NameToLayer("Player");
		}
		UnitBase component = summonPpt.GetComponent<UnitBase>();
		component.SummonsInitial(this);
		if (component is Teammate teammate)
		{
			(int, float) fuseData = base.SIP.fuseData;
			teammate.FusionData.InitFuseData(fuseData.Item1, fuseData.Item2);
			int maxFusionLevel = teammate.FusionData.MaxFusionLevel;
			if (ownerPpt != null && maxFusionLevel > 0)
			{
				if (ownerPpt.unitCfg.unitType == UnitType.Player)
				{
					UnitProperty unitProperty = FindAndRemoveValidFuseTarget(PlayerMgr.Inst.summonsPpts, maxFusionLevel);
					if (unitProperty == null)
					{
						unitProperty = FindAndRemoveValidFuseTarget(PlayerMgr.Inst.summonsNotAttackPpts, maxFusionLevel);
					}
					if (unitProperty != null)
					{
						component.Update();
						unitProperty.UnitBas.Frame1Initial();
						StartFuseProgress(unitProperty.GetComponent<Teammate>(), teammate);
					}
				}
				else
				{
					UnitProperty unitProperty2 = FindAndRemoveValidFuseTarget(ownerPpt.UnitBas.mateSummonsPpts, maxFusionLevel);
					if (unitProperty2 == null)
					{
						unitProperty2 = FindAndRemoveValidFuseTarget(ownerPpt.UnitBas.mateSummonsNotAttackPpts, maxFusionLevel);
					}
					if (unitProperty2 != null)
					{
						component.Update();
						unitProperty2.UnitBas.Frame1Initial();
						StartFuseProgress(unitProperty2.GetComponent<Teammate>(), teammate);
					}
				}
			}
		}
		if (component is Teammate4 teammate2)
		{
			float num = teammate2.width / (float)base.InitialParameter.spellShootCount;
			float num2 = (float)base.InitialParameter.inSpellShootIndex * num + num * 0.5f;
			Vector3 dir = Tool2D.GetDir(base.InitialParameter.shootDirection, -90f);
			summonPpt.transform.position += dir * num2;
		}
		if (base.currentSpellMovement == SpellSpecialMovementType.Rotation)
		{
			if (summonPpt.gameObject.layer != LayerMask.NameToLayer("Teammate_Fly"))
			{
				summonPpt.gameObject.layer = LayerMask.NameToLayer("Teammate_Fly");
			}
			component.GetComponent<UnitProperty>().FlyRegister();
		}
		CreateLifeLine();
		lastFramePosition = base.transform.position;
		SpellSummon5ManaRegenSpeed(base.shooterWand);
		PlaySummonSoundEffect();
		component.Frame1Initial();
		if (voidExplosionInfo != null)
		{
			Spell3129VoidExplosion.VoidExplosionData voidExplosionData = voidExplosionInfo.Copy();
			voidExplosionData.ConstVoidEffect = true;
			component.myPpt.SetVoid(voidExplosionData);
		}
		if (PlayerMgr.Inst.ItemCtrller.curseCfg_Pestilence != null)
		{
			component.myPpt.unitCfg.currentHP = PlayerMgr.Inst.ItemCtrller.curseCfg_Pestilence.int1.result;
		}
	}

	private void PlaySummonSoundEffect()
	{
		if (!(Time.time - lastPlayerSummonSoundTime < 0.3f))
		{
			SEMgr.Inst.PlaySE("SE_TeammateSummon");
			lastPlayerSummonSoundTime = Time.time;
		}
	}

	private void StartFuseProgress(Teammate target1, Teammate target2)
	{
		Spell3115FusionSummon component = PlayerMgr.Inst.MiniPool.GetGO("Prefabs/Spell/" + 31151, target2.transform.position).GetComponent<Spell3115FusionSummon>();
		component.CreateFromMiniPool = true;
		component.StartFusionProgress(target1, target2);
	}

	public override void FixedUpdate()
	{
		base.FixedUpdate();
		if (base.currentSpellMovement != SpellSpecialMovementType.Rotation)
		{
			float num = Tool2D.IgnoreZDistance(base.transform.position, lastFramePosition);
			if (num >= 5f)
			{
				num *= 0.1f;
			}
			base.TriggerCtrl?.UpdateMoveTrigger(num, base.transform.position, base.Direction, out var _);
			lastFramePosition = base.transform.position;
		}
	}

	private UnitProperty FindAndRemoveValidFuseTarget(List<UnitProperty> targets, int targetMaxFusionLevel)
	{
		foreach (UnitProperty target in targets)
		{
			Teammate component = target.GetComponent<Teammate>();
			if (target.UnitBas.SummonerSpellBase.spellCfg.abilityType == base.spellCfg.abilityType && !target.isUnitDead && component.FusionData.CurrentFusionLevel < Mathf.Max(component.FusionData.MaxFusionLevel, targetMaxFusionLevel))
			{
				targets.Remove(target);
				return target;
			}
		}
		return null;
	}

	private void CreateLifeLine()
	{
		if (lifelineDamage > 0f && !(ownerPpt == null))
		{
			lineScript = PlayerMgr.Inst.MiniPool.GetGO("Prefabs/Spell/31101").GetComponent<Spell3110LifeLine>();
			lineScript.Iniatialize(ownerPpt, summonPpt.GetComponent<UnitProperty>(), (int)lifelineDamage, lifelineDamageInterval, this);
			if (base.spellCfg.abilityType == SpellAbilityType.Summon2)
			{
				lineScript.transform.parent = LevelMgr.Inst.CurrentRoomT.parent;
				summonPpt.GetComponent<Teammate2>().targetline = lineScript;
			}
			else
			{
				summonPpt.GetComponent<Teammate>().summonLifeLine = ((base.SIP.SummonFollowOwnerThroughMapChance > 0f) ? lineScript : null);
			}
		}
	}

	public override void OnFirstFrame()
	{
		base.OnFirstFrame();
		if (lineScript != null)
		{
			lineScript.casterWand = base.shooterWand;
		}
	}

	public override void Update()
	{
		base.Update();
		if (!summonPpt.gameObject.activeSelf || summonPpt.unitCfg.currentHP <= 0f)
		{
			PoolRecycle();
		}
	}

	public override void PoolRecycle()
	{
		base.spellSplitCount = 0;
		base.endThunderHitPercent = 0f;
		base.PoolRecycle();
	}

	public void SpellSummon5ManaRegenSpeed(Wand wand, float MpGenSpeed = 0f)
	{
		if (summonPpt != null && base.spellCfg.abilityType == SpellAbilityType.Summon5)
		{
			float mPRegenSpeed = ((wand != null && wand.WandCfg != null) ? (wand.GetWandMpRecoverSpeed() * base.spellCfg.float1 / 100f) : MpGenSpeed);
			summonPpt.GetComponent<Teammate5>().SetMPRegenSpeed(mPRegenSpeed);
		}
	}

	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		Gizmos.color = new Color(0.37f, 1f, 0.27f, 0.49f);
		Gizmos.DrawSphere(base.transform.position, 0.4f);
	}

	public void OnLaunchFromUnitNotPlayer(UnitBase unit)
	{
		if (unit is Teammate5 teammate)
		{
			SpellSummon5ManaRegenSpeed(teammate.SummonerSpellBase.shooterWand, teammate.manaGenerateSpeedPerSecond);
			base.shooterWand = teammate.SummonerSpellBase.shooterWand;
		}
		else if (unit is Teammate5FuseController teammate5FuseController)
		{
			SpellSummon5ManaRegenSpeed(teammate5FuseController.SummonerSpellBase.shooterWand, teammate5FuseController.manaGenerateSpeedPerSecond);
			base.shooterWand = teammate5FuseController.SummonerSpellBase.shooterWand;
		}
	}

	public override void ApplyEnhanceSpell(int spellId)
	{
		base.ApplyEnhanceSpell(spellId);
		if (!SpellConfig.dic.ContainsKey(spellId))
		{
			Debug.LogError($"\ufffd\ufffd\ufffd\ufffd\ufffdڵķ\ufffd\ufffd\ufffdID {base.spellCfg.id}");
			return;
		}
		SpellConfig spellConfig = SpellConfig.dic[spellId];
		if (spellConfig.abilityType == SpellAbilityType.FusionSummon)
		{
			ApplyFuseData(spellConfig.int1, spellConfig.float1);
		}
	}

	private void ApplyFuseData(int maxLevel, float ratio)
	{
		Teammate component = summonPpt.GetComponent<Teammate>();
		if (!component)
		{
			Debug.LogError("û\ufffd\ufffd\ufffdٻ\ufffd\ufffd\uf8ec\ufffd\ufffd\ufffd\ufffdӦ\ufffd\ufffd\ufffdں\ufffdǿ\ufffd\ufffd");
			return;
		}
		component.FusionData.MaxFusionLevel += maxLevel;
		component.FusionData.FuseBuffRatio = ratio;
	}

	protected override void InitSpeedAndPosition()
	{
		float fallExplosionRadius = base.SIP.fallExplosionRadius;
		base.SIP.fallExplosionRadius = 0f;
		base.InitSpeedAndPosition();
		base.SIP.fallExplosionRadius = fallExplosionRadius;
	}
}
