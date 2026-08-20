using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

public static class SpellAutoFullTool
{
	private delegate bool SpellFilter([NotNull] SlotData slot);

	private static readonly HashSet<SpellAbilityType> SelfRepelSpells = new HashSet<SpellAbilityType>
	{
		SpellAbilityType.FireCrystal,
		SpellAbilityType.Frozen,
		SpellAbilityType.MucusCrystal,
		SpellAbilityType.ThunderCrystal,
		SpellAbilityType.VenomCrystal
	};

	private static readonly HashSet<SpellAbilityType>[] RepelSpellGroup = new HashSet<SpellAbilityType>[1]
	{
		new HashSet<SpellAbilityType>
		{
			SpellAbilityType.FollowOwner,
			SpellAbilityType.FollowTarget,
			SpellAbilityType.AroundOwner,
			SpellAbilityType.AroundMouse
		}
	};

	private static readonly Dictionary<SpellAbilityType, HashSet<SpellAbilityType>> SpellAvoidEnhances = new Dictionary<SpellAbilityType, HashSet<SpellAbilityType>>
	{
		{
			SpellAbilityType.ArcaneExplosion,
			new HashSet<SpellAbilityType> { SpellAbilityType.EnhanceSpeedValue }
		},
		{
			SpellAbilityType.Meteor,
			new HashSet<SpellAbilityType>
			{
				SpellAbilityType.EnhanceRadiusRatio,
				SpellAbilityType.FollowOwner
			}
		},
		{
			SpellAbilityType.FireBall,
			new HashSet<SpellAbilityType>
			{
				SpellAbilityType.EnhanceRadiusRatio,
				SpellAbilityType.FollowOwner
			}
		},
		{
			SpellAbilityType.DeathAdder,
			new HashSet<SpellAbilityType>
			{
				SpellAbilityType.EnhanceRadiusRatio,
				SpellAbilityType.FollowOwner
			}
		}
	};

	private static readonly HashSet<SpellAbilityType> IgnoreSpells = new HashSet<SpellAbilityType>
	{
		SpellAbilityType.OnHitTrigger,
		SpellAbilityType.OnOverTrigger,
		SpellAbilityType.OnOverSplitTrigger,
		SpellAbilityType.OnStartRotationTrigger,
		SpellAbilityType.OnMoveTrigger,
		SpellAbilityType.PostSlotExtenderMove,
		SpellAbilityType.PostSlotExtenderStand,
		SpellAbilityType.PostSlotExtenderCastSpell,
		SpellAbilityType.PostSlotExtenderTime,
		SpellAbilityType.Volley,
		SpellAbilityType.TotalScattering,
		SpellAbilityType.RadiuRatioDown,
		SpellAbilityType.Mimic,
		SpellAbilityType.Summon5,
		SpellAbilityType.PreFirework,
		SpellAbilityType.ArcaneNova,
		SpellAbilityType.AllFieldEnhance,
		SpellAbilityType.ManaTendril
	};

	private static readonly HashSet<SpellAbilityType> RecommendedCastFromTriggerSpells = new HashSet<SpellAbilityType>
	{
		SpellAbilityType.ArcaneExplosion,
		SpellAbilityType.BlackHole,
		SpellAbilityType.DisintegrationRay,
		SpellAbilityType.DragonBreath
	};

	public static void AutoFull()
	{
		List<SlotData> list = RemoveAllSpell();
		foreach (int item in GetWandsOrderByRating())
		{
			TryFullToWand(list, item);
			UIPlayerDataMgr.Inst.WandUpdate(item);
		}
		foreach (SlotData item2 in list)
		{
			PlayerMgr.Inst.SpellPick(item2);
		}
	}

	private static List<SlotData> RemoveAllSpell()
	{
		List<SlotData> list = RemoveBagSpell();
		for (int i = 0; i < PlayerMgr.Inst.Wands.Count; i++)
		{
			Wand wand = PlayerMgr.Inst.Wands[i];
			if ((object)wand != null && wand.WandCfg != null)
			{
				list.AddRange(RemoveWandSpell(i));
			}
		}
		list.Sort((SlotData x, SlotData y) => x.id - y.id);
		return list;
	}

	private static List<SlotData> RemoveBagSpell()
	{
		List<SlotData> list = new List<SlotData>();
		SlotData[] array = PlayerMgr.Inst.BaData.bagSpellDatas.ToArray();
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i] != null)
			{
				list.Add(array[i]);
				array.Bag_RemoveSlot(i);
			}
		}
		PlayerMgr.Inst.BaData.bagSpellDatas = array.ToList();
		UIPlayerDataMgr.Inst.UpdateBag();
		return list;
	}

	private static List<SlotData> RemoveWandSpell(int wandIndex)
	{
		List<SlotData> res = new List<SlotData>();
		WandConfig wandCfg = PlayerMgr.Inst.Wands[wandIndex].WandCfg;
		PopSpells(wandCfg.normalSlots, wandCfg.normalSlotIsLock);
		PopSpells(wandCfg.postSlots, wandCfg.postSlotIsLock);
		PlayerMgr.Inst.Wands[wandIndex].ResetAndRecheck();
		PlayerMgr.Inst.WandCheckSlotCount(wandIndex);
		UIPlayerDataMgr.Inst.WandUpdate(wandIndex);
		return res;
		void PopSpells(SlotData[] spells, bool[] locks)
		{
			for (int i = 0; i < spells.Length; i++)
			{
				if (!locks[i] && spells[i] != null)
				{
					spells[i].mimicSpellID = 0;
					res.Add(spells[i]);
					spells.Bag_RemoveSlot(i);
				}
			}
		}
	}

	private static List<int> GetWandsOrderByRating()
	{
		List<int> list = new List<int>();
		for (int i = 0; i < PlayerMgr.Inst.Wands.Count; i++)
		{
			Wand wand = PlayerMgr.Inst.Wands[i];
			if ((object)wand != null && wand.WandCfg != null)
			{
				list.Add(i);
			}
		}
		list.Sort((int x, int y) => (int)(WandRating(PlayerMgr.Inst.Wands[y].WandCfg) - WandRating(PlayerMgr.Inst.Wands[x].WandCfg)));
		return list;
	}

	private static void TryFullToWand(List<SlotData> source, int wandIndex)
	{
		Wand wand = PlayerMgr.Inst.Wands[wandIndex];
		AppendPassiveSpells(source, wand, WandSlotType.Normal);
		AppendPassiveSpells(source, wand, WandSlotType.Post);
		AppendShootableSpell(source, wand, WandSlotType.Normal);
		AppendShootableSpell(source, wand, WandSlotType.Post);
		AppendEnhanceSpell(source, wand, WandSlotType.Normal);
		AppendEnhanceSpell(source, wand, WandSlotType.Post);
	}

	private static void AppendPassiveSpells(List<SlotData> source, Wand wand, WandSlotType slotType)
	{
		GetSlotsInfo(wand, slotType, out var slots, out var locks);
		while ((float)slots.Bag_SpaceCount() > (float)slots.Length * 0.7f)
		{
			SlotData randomSpell = GetRandomSpell(source, delegate(SlotData data)
			{
				SpellConfig spellConfig = SpellConfig.dic[data.id];
				return spellConfig.useType == SpellType.Passive && !IgnoreSpells.Contains(spellConfig.abilityType);
			});
			if (randomSpell == null)
			{
				break;
			}
			int num = slots.Bag_GetFirstCanSetWithPushSlotIndex(locks, randomSpell);
			if (num == -1)
			{
				break;
			}
			slots.Bag_SetSlotWithPush(locks, randomSpell, num);
			wand.ResetAndRecheck();
			PlayerMgr.Inst.WandCheckSlotCount(PlayerMgr.Inst.Wands.IndexOf(wand));
			GetSlotsInfo(wand, slotType, out slots, out locks);
			source.Remove(randomSpell);
		}
	}

	private static void AppendShootableSpell(List<SlotData> source, Wand wand, WandSlotType slotType)
	{
		GetSlotsInfo(wand, slotType, out var slots, out var locks);
		if (GetShootableSpell(slots) != null)
		{
			return;
		}
		SlotData randomSpell = GetRandomSpell(source, delegate(SlotData slot)
		{
			SpellConfig spellConfig = SpellConfig.dic[slot.id];
			SpellType useType = spellConfig.useType;
			return (useType == SpellType.Missile || useType == SpellType.Summon) && !IgnoreSpells.Contains(spellConfig.abilityType);
		});
		if (randomSpell == null)
		{
			return;
		}
		for (int num = slots.Length - 1; num >= 0; num--)
		{
			if (slots.Bag_CanSetSlotWithPush(locks, randomSpell, num))
			{
				slots.Bag_SetSlotWithPush(locks, randomSpell, num);
				wand.ResetAndRecheck();
				PlayerMgr.Inst.WandCheckSlotCount(PlayerMgr.Inst.Wands.IndexOf(wand));
				GetSlotsInfo(wand, slotType, out slots, out locks);
				source.Remove(randomSpell);
				break;
			}
		}
		AppendShootableSpellPostProcess(source, wand, slotType);
	}

	private static void AppendShootableSpellPostProcess(List<SlotData> source, Wand wand, WandSlotType slotType)
	{
		GetSlotsInfo(wand, slotType, out var slots, out var locks);
		SlotData shootableSpell = GetShootableSpell(slots);
		if (shootableSpell == null || !RecommendedCastFromTriggerSpells.Contains(shootableSpell.GetConfigIgnoreMimic().abilityType))
		{
			return;
		}
		SlotData randomSpell = GetRandomSpell(source, delegate(SlotData slot)
		{
			SpellAbilityType abilityType = slot.GetConfigIgnoreMimic().abilityType;
			return abilityType == SpellAbilityType.PreFirework || abilityType == SpellAbilityType.Summon5 || abilityType == SpellAbilityType.ArcaneNova;
		});
		if (randomSpell == null)
		{
			return;
		}
		for (int num = slots.Length - 1; num >= 0; num--)
		{
			if (slots.Bag_CanSetSpell(locks, randomSpell, num))
			{
				slots.Bag_SetSlotWithPush(locks, randomSpell, num);
				wand.ResetAndRecheck();
				PlayerMgr.Inst.WandCheckSlotCount(PlayerMgr.Inst.Wands.IndexOf(wand));
				source.Remove(randomSpell);
				break;
			}
		}
	}

	private static void AppendEnhanceSpell(List<SlotData> source, Wand wand, WandSlotType slotType)
	{
		GetSlotsInfo(wand, slotType, out var slots, out var locks);
		SlotData shootableSpell = GetShootableSpell(slots);
		SpellConfig mainCfg = shootableSpell?.GetConfigIgnoreMimic();
		for (int i = 0; i < slots.Length; i++)
		{
			SlotData randomSpell = GetRandomSpell(source, delegate(SlotData slot)
			{
				SpellConfig spellConfig = SpellConfig.dic[slot.id];
				bool flag = true;
				if (shootableSpell != null)
				{
					flag = Filter_SelfRepel(spellConfig) && Filter_EffectRangeEnhance(spellConfig, mainCfg) && Filter_SummonOnlyEnhance(spellConfig, mainCfg) && Filter_AvoidEnhance(spellConfig, mainCfg) && Filter_GroupRepel(spellConfig);
				}
				return flag && spellConfig.useType == SpellType.Enhance && !IgnoreSpells.Contains(spellConfig.abilityType);
			});
			if (randomSpell == null)
			{
				break;
			}
			if (!slots.Bag_CanSetSlotWithPush(locks, randomSpell, i))
			{
				continue;
			}
			slots.Bag_SetSlotWithPush(locks, randomSpell, i);
			wand.ResetAndRecheck();
			PlayerMgr.Inst.WandCheckSlotCount(PlayerMgr.Inst.Wands.IndexOf(wand));
			GetSlotsInfo(wand, slotType, out slots, out locks);
			int slotNumModifyValue = randomSpell.GetConfigIgnoreMimic().slotNumModifyValue;
			if (slotNumModifyValue > 0)
			{
				for (int j = 0; j < slotNumModifyValue; j++)
				{
					PushShootableSpell();
				}
				wand.ResetAndRecheck();
				PlayerMgr.Inst.WandCheckSlotCount(PlayerMgr.Inst.Wands.IndexOf(wand));
				GetSlotsInfo(wand, slotType, out slots, out locks);
			}
			source.Remove(randomSpell);
		}
		static bool Filter_AvoidEnhance(SpellConfig target, SpellConfig mainSpell)
		{
			if (SpellAvoidEnhances.TryGetValue(mainSpell.abilityType, out var value))
			{
				return !value.Contains(target.abilityType);
			}
			return true;
		}
		static bool Filter_EffectRangeEnhance(SpellConfig target, SpellConfig mainSpell)
		{
			if (target.abilityType != SpellAbilityType.EnhanceRadiusRatio)
			{
				return true;
			}
			return mainSpell.radius > 0f;
		}
		bool Filter_GroupRepel(SpellConfig target)
		{
			HashSet<SpellAbilityType>[] repelSpellGroup = RepelSpellGroup;
			foreach (HashSet<SpellAbilityType> hashSet in repelSpellGroup)
			{
				if (hashSet.Contains(target.abilityType) && (from e in slots
					where e != null && e.id > 0
					select e.GetConfigIgnoreMimic().abilityType).Intersect(hashSet).Any())
				{
					return false;
				}
			}
			return true;
		}
		bool Filter_SelfRepel(SpellConfig target)
		{
			if (!SelfRepelSpells.Contains(target.abilityType))
			{
				return true;
			}
			return !slots.Any((SlotData e) => e != null && e.id > 0 && e.GetConfigIgnoreMimic().abilityType == target.abilityType);
		}
		static bool Filter_SummonOnlyEnhance(SpellConfig target, SpellConfig mainSpell)
		{
			if (target.haveEffecforMissileSpell)
			{
				return true;
			}
			return mainSpell.useType == SpellType.Summon;
		}
		void PushShootableSpell()
		{
			for (int num = slots.Length - 1; num >= 0; num--)
			{
				SlotData slotData = slots[num];
				if (slotData != null && slotData.id > 0)
				{
					SpellType useType = slots[num].GetConfigIgnoreMimic().useType;
					if ((useType == SpellType.Missile || useType == SpellType.Summon) && slots.Bag_CanPushToRight(locks, num))
					{
						slots.Bag_PushToRight(locks, num);
					}
				}
			}
		}
	}

	[CanBeNull]
	private static SlotData GetShootableSpell(SlotData[] slots)
	{
		for (int num = slots.Length - 1; num >= 0; num--)
		{
			if (slots[num] != null && !slots[num].isSealSlot)
			{
				SpellType useType = SpellConfig.dic[slots[num].id].useType;
				if (useType == SpellType.Summon || useType == SpellType.Missile)
				{
					return slots[num];
				}
			}
		}
		return null;
	}

	private static void GetSlotsInfo(Wand wand, WandSlotType slotType, out SlotData[] slots, out bool[] locks)
	{
		if (slotType == WandSlotType.Normal)
		{
			slots = wand.WandCfg.normalSlots;
			locks = wand.WandCfg.normalSlotIsLock;
		}
		else
		{
			slots = wand.WandCfg.postSlots;
			locks = wand.WandCfg.postSlotIsLock;
		}
	}

	[CanBeNull]
	private static SlotData GetRandomSpell(List<SlotData> source, SpellFilter filter = null)
	{
		List<SlotData> list = source.Where((SlotData t) => filter == null || filter(t)).ToList();
		if (list.Count == 0)
		{
			return null;
		}
		return list[Random.Range(0, list.Count)];
	}

	public static float WandRating(WandConfig wand)
	{
		if (wand.specialAbility == WandAbility.Battery)
		{
			return 0f;
		}
		return (0f + (float)wand.maxMP + (float)(wand.mpRecovery * 10) + (float)(wand.normalSlots.Length * 10) + (float)(wand.postSlots.Length * 6) - (wand.shootInterval + wand.coolDown) * 50f) * (wand.damageCorrection * 1f) * ((1f + wand.criticalChance * 0.01f) * 1f) / (float)wand.costCorrection;
	}
}
