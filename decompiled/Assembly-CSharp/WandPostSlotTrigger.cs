using System.Linq;
using UnityEngine;

public static class WandPostSlotTrigger
{
	public static WandConfig GetTargetShooterWandConfigDataFromSPellBase(SpellBase targetBase)
	{
		if (targetBase.wandChargeData != null && targetBase.wandChargeData.chargeWandCfg != null)
		{
			return targetBase.wandChargeData.chargeWandCfg;
		}
		if (targetBase.shooterWand != null && targetBase.shooterWand.WandCfg != null)
		{
			return targetBase.shooterWand.WandCfg;
		}
		return null;
	}

	public static WandConfig GetTargetShooterWandConfigDataFromTakeDamageInfo(TakeDamageInfo info)
	{
		WandPostSlotChargeData wandChargeData = info.wandChargeData;
		if (wandChargeData != null && wandChargeData.chargeWandCfg != null)
		{
			return info.wandChargeData.chargeWandCfg;
		}
		if ((bool)info.spellBase)
		{
			wandChargeData = info.spellBase.wandChargeData;
			if (wandChargeData != null && wandChargeData.chargeWandCfg != null)
			{
				return info.spellBase.wandChargeData.chargeWandCfg;
			}
		}
		return null;
	}

	public static void PostSlotTimeTriggerCheck(Wand target)
	{
		if ((!UIBattleMgr.Inst || (!GameUISingletonMono<UIReroll>.StaticIsOpen && !GameUISingletonMono<UICompound>.StaticIsOpen)) && (PlayerMgr.Inst.PlayerCtrller.isFrozen || (bool)PlayerMgr.Inst.ItemCtrller.potion_Petrifaction || PlayerMgr.Inst.PlayerCtrller.CanMotion))
		{
			float num = ((Time.deltaTime > 0.2f) ? 0.2f : Time.deltaTime);
			WandConfig wandCfg = target.WandCfg;
			if (wandCfg != null && wandCfg.postSlots.Length != 0 && wandCfg.PostslotTimeChargeRatio > 0f)
			{
				target.ChargePostSlots(wandCfg.PostslotTimeChargeRatio * num);
			}
		}
	}

	public static void PostSlotMoveDistanceTriggerCheck(Wand target)
	{
		if ((!UIBattleMgr.Inst || (!GameUISingletonMono<UIReroll>.StaticIsOpen && !GameUISingletonMono<UICompound>.StaticIsOpen)) && (PlayerMgr.Inst.PlayerCtrller.isFrozen || (bool)PlayerMgr.Inst.ItemCtrller.potion_Petrifaction || PlayerMgr.Inst.PlayerCtrller.CanMotion))
		{
			WandConfig wandCfg = target.WandCfg;
			if (wandCfg != null && wandCfg.postSlots.Length != 0 && wandCfg.PostslotMoveChargeRatio > 0f)
			{
				target.ChargePostSlots(wandCfg.PostslotMoveChargeRatio * PlayerMgr.Inst.PlayerCtrller.distanceMoveInLastFrame);
			}
		}
	}

	public static void PostSlotStandTriggerCheck(Wand target)
	{
		if ((!UIBattleMgr.Inst || (!GameUISingletonMono<UIReroll>.StaticIsOpen && !GameUISingletonMono<UICompound>.StaticIsOpen)) && (PlayerMgr.Inst.PlayerCtrller.isFrozen || (bool)PlayerMgr.Inst.ItemCtrller.potion_Petrifaction || PlayerMgr.Inst.PlayerCtrller.CanMotion))
		{
			float num = ((PlayerMgr.Inst.PlayerDeltaTime > 0.2f) ? 0.2f : PlayerMgr.Inst.PlayerDeltaTime);
			WandConfig wandCfg = target.WandCfg;
			if (wandCfg != null && wandCfg.postSlots.Length != 0 && wandCfg.PostslotStandChargeRatio > 0f && PlayerMgr.Inst.PlayerCtrller.isStandInLastFrame && !PlayerMgr.Inst.inDashSpell)
			{
				target.ChargePostSlots(wandCfg.PostslotStandChargeRatio * num);
			}
		}
	}

	public static void PostSlotCastSpellTriggerCheck(WandConfig wandCfg)
	{
		if ((bool)UIBattleMgr.Inst && (GameUISingletonMono<UIReroll>.StaticIsOpen || GameUISingletonMono<UICompound>.StaticIsOpen))
		{
			return;
		}
		for (int i = 0; i < PlayerMgr.Inst.Wands.Count; i++)
		{
			Wand wand = PlayerMgr.Inst.Wands[i];
			if (wand.WandCfg != null && wandCfg == wand.WandCfg && wand.WandCfg.PostslotCastSpellChargeRatio > 0f)
			{
				PlayerMgr.Inst.Wands[i].ChargePostSlots(wandCfg.PostslotCastSpellChargeRatio);
				break;
			}
		}
	}

	public static void PostSlotSpellHitTriggerCheck(WandConfig wandCfg)
	{
		if ((bool)UIBattleMgr.Inst && (GameUISingletonMono<UIReroll>.StaticIsOpen || GameUISingletonMono<UICompound>.StaticIsOpen))
		{
			return;
		}
		for (int i = 0; i < PlayerMgr.Inst.Wands.Count; i++)
		{
			Wand wand = PlayerMgr.Inst.Wands[i];
			if (wand.WandCfg != null && wandCfg == wand.WandCfg && wand.WandCfg.PostslotSpellHitChargeRatio > 0f)
			{
				PlayerMgr.Inst.Wands[i].ChargePostSlots(wandCfg.PostslotSpellHitChargeRatio);
				break;
			}
		}
	}

	public static void PostSlotSpellCriticalHitTriggerCheck(WandConfig wandCfg)
	{
		if ((bool)UIBattleMgr.Inst && (GameUISingletonMono<UIReroll>.StaticIsOpen || GameUISingletonMono<UICompound>.StaticIsOpen))
		{
			return;
		}
		for (int i = 0; i < PlayerMgr.Inst.Wands.Count; i++)
		{
			Wand wand = PlayerMgr.Inst.Wands[i];
			if (wand.WandCfg != null && wandCfg == wand.WandCfg && wand.WandCfg.PostslotCriticalHitChargeRatio > 0f)
			{
				PlayerMgr.Inst.Wands[i].ChargePostSlots(wandCfg.PostslotCriticalHitChargeRatio);
				break;
			}
		}
	}

	public static void PostSlotSpellTakeDamageTriggerCheck()
	{
		if ((bool)UIBattleMgr.Inst && (GameUISingletonMono<UIReroll>.StaticIsOpen || GameUISingletonMono<UICompound>.StaticIsOpen))
		{
			return;
		}
		foreach (Wand item in PlayerMgr.Inst.Wands.Where(delegate(Wand e)
		{
			if ((object)e != null)
			{
				WandConfig wandCfg = e.WandCfg;
				if (wandCfg != null)
				{
					return wandCfg.postSlotTriggerType == WandPostSlotTriggerType.TakeDamage;
				}
			}
			return false;
		}))
		{
			item.ChargePostSlots(item.WandCfg.PostslotTakeDamageChargeRatio);
		}
	}

	public static void PostSlotHighDamageTriggerCheck(WandConfig wandCfg)
	{
		if ((bool)UIBattleMgr.Inst && (GameUISingletonMono<UIReroll>.StaticIsOpen || GameUISingletonMono<UICompound>.StaticIsOpen))
		{
			return;
		}
		for (int i = 0; i < PlayerMgr.Inst.Wands.Count; i++)
		{
			Wand wand = PlayerMgr.Inst.Wands[i];
			if (wand.WandCfg != null && wandCfg == wand.WandCfg && wand.WandCfg.PostslotHighDamageChargeRatio > 0f)
			{
				PlayerMgr.Inst.Wands[i].ChargePostSlots(wandCfg.PostslotHighDamageChargeRatio);
				break;
			}
		}
	}

	public static void PostSlotKillEnemyTriggerCheck(TakeDamageInfo info)
	{
		if ((bool)UIBattleMgr.Inst && (GameUISingletonMono<UIReroll>.StaticIsOpen || GameUISingletonMono<UICompound>.StaticIsOpen))
		{
			return;
		}
		WandConfig targetShooterWandConfigDataFromTakeDamageInfo = GetTargetShooterWandConfigDataFromTakeDamageInfo(info);
		for (int i = 0; i < PlayerMgr.Inst.Wands.Count; i++)
		{
			Wand wand = PlayerMgr.Inst.Wands[i];
			if (wand.WandCfg != null && info.beHitPpt.AlreadyDead && targetShooterWandConfigDataFromTakeDamageInfo == wand.WandCfg && wand.WandCfg.PostslotKillEnemyChargeRatio > 0f)
			{
				PlayerMgr.Inst.Wands[i].ChargePostSlots(targetShooterWandConfigDataFromTakeDamageInfo.PostslotKillEnemyChargeRatio);
				break;
			}
		}
	}

	public static void PostSlotKillEnemyTriggerCheck(WandConfig wandCfg)
	{
		if ((bool)UIBattleMgr.Inst && (GameUISingletonMono<UIReroll>.StaticIsOpen || GameUISingletonMono<UICompound>.StaticIsOpen))
		{
			return;
		}
		for (int i = 0; i < PlayerMgr.Inst.Wands.Count; i++)
		{
			Wand wand = PlayerMgr.Inst.Wands[i];
			if (wand.WandCfg != null && wandCfg == wand.WandCfg && wand.WandCfg.PostslotKillEnemyChargeRatio > 0f)
			{
				PlayerMgr.Inst.Wands[i].ChargePostSlots(wandCfg.PostslotKillEnemyChargeRatio);
				break;
			}
		}
	}
}
