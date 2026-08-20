using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class PlayerItemController : MonoBehaviour
{
	[HideInInspector]
	public RelicConfig relicCfg_ImmuneSurface;

	[HideInInspector]
	public RelicConfig relicCfg_BottomGuard;

	[HideInInspector]
	public Relic_StayAndFocus relic_StayAndFocus;

	[HideInInspector]
	public RelicConfig relicCfg_SpellCopy;

	[HideInInspector]
	public RelicConfig relicCfg_SuckBlood;

	[HideInInspector]
	public RelicConfig relicCfg_FollowGhost;

	[HideInInspector]
	public Relic_BlockSpellMono relic_BlockSpellMono;

	[HideInInspector]
	public RelicConfig relicCfg_InjuredAttackAll;

	[HideInInspector]
	public Relic_GluttonousSnake relic_GluttonousSnake;

	[HideInInspector]
	public Relic_SpellDestroyer relic_SpellDestroyer;

	[HideInInspector]
	public Relic_Fly relic_Fly;

	[HideInInspector]
	public RelicConfig relicCfg_Alchemy;

	[HideInInspector]
	public RelicConfig relicCfg_CoinHeal;

	[HideInInspector]
	public RelicConfig relicCfg_KillShield;

	[HideInInspector]
	public RelicConfig relicCfg_FloorShield;

	[HideInInspector]
	public Relic_FollowObj relic_FollowObj_FloorInterest;

	[HideInInspector]
	public RelicConfig relicCfg_FloorRecovery;

	[HideInInspector]
	public RelicConfig relicCfg_Seckill;

	[HideInInspector]
	public Relic_Variator relic_Variator;

	[HideInInspector]
	public RelicConfig relicCfg_MaxMP;

	[HideInInspector]
	public RelicConfig relicCfg_MPRecovery;

	[HideInInspector]
	public RelicConfig relicCfg_AddRelicOption;

	[HideInInspector]
	public RelicConfig relicCfg_SummonLimit;

	[HideInInspector]
	public RelicConfig relicCfg_PotionAddiction;

	[HideInInspector]
	public RelicConfig relicCfg_EndlessBottle;

	[HideInInspector]
	public bool relic_SpecialStore;

	[HideInInspector]
	public RelicConfig relicCfg_KillBackMP;

	[HideInInspector]
	public RelicConfig relicCfg_TempShield;

	[HideInInspector]
	public RelicConfig relicCfg_ReduceDamage;

	[HideInInspector]
	public Relic_DecelerateShield relic_DecelerateShield;

	[HideInInspector]
	public RelicConfig relicCfg_KillTempShiled;

	[HideInInspector]
	public RelicConfig relicCfg_CurseWarrior;

	[HideInInspector]
	public RelicConfig relicCfg_RerollRelic;

	[HideInInspector]
	public bool relic_ShootNotSlowdown;

	[HideInInspector]
	public Relic_RemoteShoot relic_RemoteShoot;

	[HideInInspector]
	public RelicConfig relicCfg_Dodge;

	[HideInInspector]
	public RelicConfig relicCfg_MoneyIsPower;

	[HideInInspector]
	public RelicConfig relicCfg_WandAddSlot;

	[HideInInspector]
	public RelicConfig relicCfg_NoAttackStealth;

	[HideInInspector]
	public Relic_Resurgence relic_Resurgence;

	[HideInInspector]
	public Relic_SaintSword relic_SaintSword;

	[HideInInspector]
	public RelicConfig relicCfg_AddCriticalChance;

	[HideInInspector]
	public Relic_GreedSeed relic_GreedSeed;

	[HideInInspector]
	public RelicConfig relicCfg_LongNeck;

	[HideInInspector]
	public RelicConfig relicCfg_AddDamage;

	[HideInInspector]
	public bool relic_SpellThroughWall;

	[HideInInspector]
	public RelicConfig relicCfg_SpellKnockback;

	[HideInInspector]
	public RelicConfig relicCfg_MaxUndifferDamage;

	[HideInInspector]
	public RelicConfig relicCfg_AddCriticalDamage;

	[HideInInspector]
	public RelicConfig relicCfg_AddRadiusOfInfluence;

	[HideInInspector]
	public Relic_AddMoveSpeed relic_AddMoveSpeed;

	[HideInInspector]
	public RelicConfig relicCfg_PowerfulMan;

	[HideInInspector]
	public Relic_FollowObj relic_FollowObj_SilverKey;

	[HideInInspector]
	public RelicConfig relicCfg_MadEye;

	[HideInInspector]
	public RelicConfig relicCfg_ShowUnitHPUI;

	[HideInInspector]
	public Relic_FollowObj relic_FollowObj_BloodKey;

	[HideInInspector]
	public Relic_FiniteGlove relic_FiniteGlove;

	[HideInInspector]
	public bool relic_ExtraDoor;

	[HideInInspector]
	public RelicConfig relicCfg_PostSlotMoreEfficiency;

	[HideInInspector]
	public RelicConfig relicCfg_FreeGods;

	[HideInInspector]
	public bool relic_CertainlyHaveRRO;

	[HideInInspector]
	public RelicConfig relicCfg_EndlessChest;

	[HideInInspector]
	public RelicConfig relicCfg_PandorasBox;

	[HideInInspector]
	public Relic_InjuredAddMoveSpeed relic_InjuredAddMoveSpeed;

	[HideInInspector]
	public Relic_RainbowRibbon relic_RainbowRibbon;

	[HideInInspector]
	public RelicConfig relicCfg_EnterDoorRemoveCurse;

	[HideInInspector]
	public Relic_InvisibleWing relic_InvisibleWing;

	[HideInInspector]
	public Relic_MadWarrior relic_MadWarrior;

	[HideInInspector]
	public RelicConfig relicCfg_ReduceSkillCD;

	[HideInInspector]
	public RelicConfig relicCfg_PickMoreRelic;

	[HideInInspector]
	public RelicConfig relicCfg_MoreMaxHPOutput;

	[HideInInspector]
	public RelicConfig relicCfg_MoreCoinOutput;

	[HideInInspector]
	public RelicConfig relicCfg_KeyIsPower;

	[HideInInspector]
	public RelicConfig relicCfg_ExtraPotionStorage;

	[HideInInspector]
	public RelicConfig relicCfg_LessWandMoreSlot;

	[HideInInspector]
	public RelicConfig relicCfg_ExtraSkillUsage;

	[HideInInspector]
	public RelicConfig relicCfg_AddSpellOption;

	[HideInInspector]
	public RelicConfig relicCfg_RerollSpell;

	[HideInInspector]
	public RelicConfig relicCfg_PickMoreSpell;

	[HideInInspector]
	public RelicConfig relic_EndlessExtraDamage;

	[HideInInspector]
	public RelicConfig relic_EndlessExtraMaxHP;

	[HideInInspector]
	public Relic_MirrorOfSoul relic_MirrorOfSoul;

	[HideInInspector]
	public UIRelic_LightArmor uiRelic_LightArmor;

	[HideInInspector]
	public UIRelic_GrassCloth uiRelic_WarmSnow;

	[HideInInspector]
	public Relic_Reaper relic_Reaper;

	[HideInInspector]
	public Relic_Huang relic_Huang;

	[HideInInspector]
	public Relic_MedicineKit relic_MedicineKit;

	[HideInInspector]
	public Relic_DruidRing relic_DruidRing;

	[HideInInspector]
	public UIRelic_DaveHarpoons uiRelic_DaveHarpoons;

	[HideInInspector]
	public UiRelic_RuneWizard uiRelic_RuneWizard;

	[HideInInspector]
	public RelicConfig relic_DivingSuit;

	[HideInInspector]
	public RelicConfig relic_PowerfulHarpoonHead;

	[HideInInspector]
	public RelicConfig relic_PoisonousHarpoonHead;

	[HideInInspector]
	public RelicConfig relic_LightningHarpoonHead;

	[HideInInspector]
	public RelicConfig relic_FrozenHarpoonHead;

	[HideInInspector]
	public RelicConfig relic_FlameHarpoonHead;

	[HideInInspector]
	public RelicConfig relic_HarpoonsHeadExtend;

	[HideInInspector]
	public readonly Dictionary<int, RelicGroupConfig> relicGroupConfigs = new Dictionary<int, RelicGroupConfig>();

	[HideInInspector]
	public UIPotion_Psychedelic uiPotionPsychedelic;

	[HideInInspector]
	public GameObject potion_HoverEFGO;

	[HideInInspector]
	public Potion_Invincible potion_Invincible;

	[HideInInspector]
	public Potion_Petrifaction potion_Petrifaction;

	[HideInInspector]
	public Potion_Stomachache potion_Stomachache;

	[HideInInspector]
	public Potion_Invisible potion_Invisible;

	[HideInInspector]
	public CurseConfig curseCfg_PastDueResource;

	[HideInInspector]
	public bool curse_IsInvisibleDoor;

	[HideInInspector]
	public CurseConfig curseCfg_TargetedTrap;

	[HideInInspector]
	public CurseConfig curseCfg_EnemyAddMove;

	[HideInInspector]
	public CurseConfig curseCfg_RevengeGhost;

	[HideInInspector]
	public CurseConfig curseCfg_InjuredLoseCoin;

	[HideInInspector]
	public bool curse_IsInjuredRandomPoint;

	[HideInInspector]
	public CurseConfig curseCfg_InjuredLoseMaxHP;

	[HideInInspector]
	public Curse_InjuredCantShoot curse_InjuredCantShoot;

	[HideInInspector]
	public bool curse_IsDiamondToCion;

	[HideInInspector]
	public CurseConfig curseCfg_ReduceMoveSpeed;

	[HideInInspector]
	public Curse_CantShootEnterRoom curse_CantShootEnterRoom;

	[HideInInspector]
	public CurseConfig curseCfg_ReduceSpellSpeed;

	[HideInInspector]
	public CurseConfig curseCfg_ReduceSpeedDamage;

	[HideInInspector]
	public CurseConfig curseCfg_SlowWand;

	[HideInInspector]
	public CurseConfig curseCfg_Bled;

	[HideInInspector]
	public bool curse_IsDoubleLock;

	[HideInInspector]
	public CurseConfig curseCfg_EnterDoorLoseCoin;

	[HideInInspector]
	public CurseConfig curseCfg_DoubleEnemy;

	[HideInInspector]
	public CurseConfig curseCfg_GetCoinLoseHP;

	[HideInInspector]
	public bool curse_IsReverseKnockback;

	[HideInInspector]
	public Curse_DarkView curse_DarkView;

	[HideInInspector]
	public CurseConfig curseCfg_LoseMPRecovery;

	[HideInInspector]
	public CurseConfig curseCfg_LoseMPLimit;

	[HideInInspector]
	public CurseConfig curseCfg_CostCorrection;

	[HideInInspector]
	public CurseConfig curseCfg_AddRecoil;

	[HideInInspector]
	public CurseConfig curseCfg_RelicReduce;

	[HideInInspector]
	public CurseConfig curseCfg_MonsterRecover;

	[HideInInspector]
	public Curse_RandomCurse curse_RandomCurseCommon;

	[HideInInspector]
	public Curse_RandomCurse curse_RandomCurseRare;

	[HideInInspector]
	public bool curse_IsIlliteracy;

	[HideInInspector]
	public bool curse_IsReverseMove;

	[HideInInspector]
	public Curse_Shackle curse_Shackle;

	[HideInInspector]
	public CurseConfig curseCfg_PotionReduceHP;

	[HideInInspector]
	public CurseConfig curseCfg_RandomBomb;

	[HideInInspector]
	public CurseConfig curseCfg_DeathBet;

	[HideInInspector]
	public Curse_Recall curse_Recall;

	[HideInInspector]
	public CurseConfig curseCfg_EnterDoorNoMP;

	[HideInInspector]
	public CurseConfig curseCfg_ReduceCriticalRatio;

	[HideInInspector]
	public CurseConfig curseCfg_NoCargo;

	[HideInInspector]
	public CurseConfig curseCfg_ScatterAdd;

	[HideInInspector]
	public CurseConfig curseCfg_SummonsReduce;

	[HideInInspector]
	public Curse_SnailHunt curse_SnailHunt;

	[HideInInspector]
	public CurseConfig curseCfg_OldWound;

	[HideInInspector]
	public bool curse_IsReverseRecoil;

	[HideInInspector]
	public CurseConfig curseCfg_Vulnerability;

	[HideInInspector]
	public CurseConfig curseCfg_ReduceSpellRadius;

	[HideInInspector]
	public CurseConfig curseCfg_MoreMoneyMoreInjured;

	[HideInInspector]
	public bool curse_IsReverseShoot;

	[HideInInspector]
	public CurseConfig curseCfg_EnemyReduceDamage;

	[HideInInspector]
	public CurseConfig curseCfg_ShootSlow;

	[HideInInspector]
	public CurseConfig curseCfg_ZeroFriction;

	[HideInInspector]
	public bool curse_IsIsaacVirus;

	[HideInInspector]
	public Curse_Stealthy curse_Stealthy;

	[HideInInspector]
	public CurseConfig curseCfg_ChestMonster;

	[HideInInspector]
	public CurseConfig curseCfg_FullHPAddDamage;

	[HideInInspector]
	public CurseConfig curseCfg_Pestilence;

	[HideInInspector]
	public CurseConfig curseCfg_LostSpellOption;

	[HideInInspector]
	public CurseConfig curseCfg_InvalidCurse;

	private EntityManager ettMgr;

	public int SelectedPotionID
	{
		get
		{
			return PlayerMgr.Inst.BaData.potionIDs[SelectedPotionIndex];
		}
		set
		{
			PlayerMgr.Inst.BaData.potionIDs[SelectedPotionIndex] = value;
		}
	}

	public int SelectedPotionIndex { get; set; }

	private void Awake()
	{
		ettMgr = World.DefaultGameObjectInjectionWorld.EntityManager;
	}

	private void Update()
	{
		ItemImplement();
	}

	private void ItemImplement()
	{
		if (PlayerMgr.Inst.PlayerCtrller == null || !PlayerMgr.Inst.TryGetPlayerPpt(out var playerPpt))
		{
			return;
		}
		if (relicCfg_BottomGuard != null)
		{
			if (playerPpt.unitCfg.currentHP < (float)PlayerMgr.Inst.ItemCtrller.relicCfg_BottomGuard.int1.result && playerPpt.unitCfg.currentHP < playerPpt.unitCfg.maxHP)
			{
				relicCfg_BottomGuard.floatTimer += PlayerMgr.Inst.PlayerDeltaTime;
				if (relicCfg_BottomGuard.floatTimer >= relicCfg_BottomGuard.float1.result)
				{
					relicCfg_BottomGuard.floatTimer = 0f;
					UnitDotsSyncSystem.UnitRecoveryHP(PlayerMgr.Inst.PlayerEtt, 1f, ettMgr);
				}
			}
			else
			{
				relicCfg_BottomGuard.floatTimer = 0f;
			}
		}
		if (relicCfg_NoAttackStealth != null && PlayerMgr.Inst.PlayerCtrller.IsVisible)
		{
			relicCfg_NoAttackStealth.floatTimer += PlayerMgr.Inst.PlayerDeltaTime;
			if (relicCfg_NoAttackStealth.floatTimer >= relicCfg_NoAttackStealth.float1.result)
			{
				relicCfg_NoAttackStealth.floatTimer = 0f;
				PlayerMgr.Inst.PlayerCtrller.SetInvisiable();
			}
		}
		if (relicCfg_MadEye != null)
		{
			relicCfg_MadEye.floatTimer = (playerPpt.unitCfg.maxHP - playerPpt.unitCfg.currentHP) * relicCfg_MadEye.float1.result / 100f / (float)relicCfg_MadEye.int1.result;
		}
		if (curseCfg_RandomBomb != null && PlayerMgr.Inst.PlayerCtrller.CanMotion && LevelMgr.Inst.CurrentRoomCfg.id != 201 && LevelMgr.Inst.CurrentRoomCfg.id != 203 && LevelMgr.Inst.CurrentRoomCfg.id != 205 && LevelMgr.Inst.CurrentRoomCfg.id != 207 && LevelMgr.Inst.CurrentRoomCfg.id != 202 && LevelMgr.Inst.CurrentRoomCfg.id != 204 && LevelMgr.Inst.CurrentRoomCfg.id != 206 && LevelMgr.Inst.CurrentRoomCfg.id != 208 && LevelMgr.Inst.CurrentRoomCfg.id != 211 && LevelMgr.Inst.CurrentRoomCfg.id != 213 && LevelMgr.Inst.CurrentRoomCfg.id != 215 && LevelMgr.Inst.CurrentRoomCfg.id != 217 && LevelMgr.Inst.CurrentRoomCfg.id != 212 && LevelMgr.Inst.CurrentRoomCfg.id != 214 && LevelMgr.Inst.CurrentRoomCfg.id != 216 && LevelMgr.Inst.CurrentRoomCfg.id != 218 && LevelMgr.Inst.CurrentRoomCfg.id != 231 && LevelMgr.Inst.CurrentRoomCfg.id != 232 && LevelMgr.Inst.CurrentRoomCfg.id != 233 && LevelMgr.Inst.CurrentRoomCfg.id != 234 && LevelMgr.Inst.CurrentRoomCfg.id != 235)
		{
			curseCfg_RandomBomb.floatTimer += Time.deltaTime;
			if (curseCfg_RandomBomb.floatTimer >= curseCfg_RandomBomb.float1.result)
			{
				curseCfg_RandomBomb.floatTimer = 0f;
				curseCfg_RandomBomb.float1.result = UnityEngine.Random.Range(curseCfg_RandomBomb.float1.value, curseCfg_RandomBomb.float1.valueUpgrade);
				QuickCreateSystem.Inst.CreateMixedEtt("Curse_RandomBomb", PlayerMgr.Inst.PlayerPointIgnoreZ);
			}
		}
		if (curseCfg_MoreMoneyMoreInjured != null)
		{
			curseCfg_MoreMoneyMoreInjured.floatTimer = (float)PlayerMgr.Inst.CoinCount / (float)curseCfg_MoreMoneyMoreInjured.int1.result * (float)curseCfg_MoreMoneyMoreInjured.int2.result / 100f;
			if (curseCfg_MoreMoneyMoreInjured.floatTimer > curseCfg_MoreMoneyMoreInjured.float1.result / 100f)
			{
				curseCfg_MoreMoneyMoreInjured.floatTimer = curseCfg_MoreMoneyMoreInjured.float1.result / 100f;
			}
		}
		if (curseCfg_FullHPAddDamage != null)
		{
			if (playerPpt.unitCfg.currentHP == playerPpt.unitCfg.maxHP)
			{
				curseCfg_FullHPAddDamage.floatTimer = (float)curseCfg_FullHPAddDamage.int1.result / 100f;
			}
			else
			{
				curseCfg_FullHPAddDamage.floatTimer = (float)curseCfg_FullHPAddDamage.int2.result / 100f;
			}
		}
	}

	public RelicConfig GetRelicConfig(int id)
	{
		for (int i = 0; i < PlayerMgr.Inst.BaData.relicCfgs.Count; i++)
		{
			if (PlayerMgr.Inst.BaData.relicCfgs[i].id == id)
			{
				return PlayerMgr.Inst.BaData.relicCfgs[i];
			}
		}
		return null;
	}

	public int GetRelicIndex(int id)
	{
		for (int i = 0; i < PlayerMgr.Inst.BaData.relicCfgs.Count; i++)
		{
			if (PlayerMgr.Inst.BaData.relicCfgs[i].id == id)
			{
				return i;
			}
		}
		return -1;
	}

	public void RelicAdd(int id, bool addGallery = true, bool fromLoadSave = false)
	{
		if (!RelicConfig.dic.ContainsKey(id))
		{
			return;
		}
		if (RelicConfig.dic[id].abilityType == RelicAbilityType.RandomLevelUp)
		{
			List<int> list = new List<int>();
			for (int i = 0; i < PlayerMgr.Inst.BaData.relicCfgs.Count; i++)
			{
				if (PlayerMgr.Inst.BaData.relicCfgs[i].dropType != ItemDropType.Special && PlayerMgr.Inst.BaData.relicCfgs[i].level < PlayerMgr.Inst.BaData.relicCfgs[i].maxCount)
				{
					list.Add(PlayerMgr.Inst.BaData.relicCfgs[i].id);
				}
			}
			if (list.Count == 0)
			{
				int relicFromPool = PlayerMgr.Inst.BaData.GetRelicFromPool(ItemDropType.Common);
				RelicAdd(relicFromPool, addGallery);
				PlayerMgr.Inst.PlayerCtrller.TextFloatQueueAdd(1002202.GetText() + ": " + RelicConfig.dic[relicFromPool].GetName(), UITextFloatType.Normal);
			}
			else
			{
				int num = list[UnityEngine.Random.Range(0, list.Count)];
				PlayerMgr.Inst.BaData.RemoveRelicFromPool(num);
				RelicAdd(num, addGallery);
				PlayerMgr.Inst.PlayerCtrller.TextFloatQueueAdd(1002004.GetText() + ": " + RelicConfig.dic[num].GetName(), UITextFloatType.Normal);
			}
			DataMgr.selectedWorldData.GalleryRelicGet(id);
			return;
		}
		if (id == 69)
		{
			UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/UI/UIRelic_RestartKey"), UIMgr.Inst.canvas_10Scaler.transform);
			if (addGallery)
			{
				DataMgr.selectedWorldData.GalleryRelicGet(id);
			}
			return;
		}
		RelicConfig relicConfig = GetRelicConfig(id);
		bool flag = relicConfig != null;
		if (flag)
		{
			relicConfig.level++;
		}
		else
		{
			relicConfig = RelicConfig.GetConfig(id);
			PlayerMgr.Inst.BaData.relicCfgs.Add(relicConfig);
		}
		relicConfig.CalculateAbility();
		int? relicGroupIdByRelicId = RelicGroupConfig.GetRelicGroupIdByRelicId(relicConfig.id);
		if (relicGroupIdByRelicId.HasValue)
		{
			RelicGroupConfig relicGroupConfig = RelicGroupConfig.dic[relicGroupIdByRelicId.Value].Copy();
			relicGroupConfig.CalculateAbility();
			if (relicGroupConfig.CheckRelicGroupIsActive(PlayerMgr.Inst.BaData.relicCfgs.Select((RelicConfig e) => e.id)) && relicGroupConfigs.TryAdd(relicGroupConfig.id, relicGroupConfig))
			{
				if (!fromLoadSave)
				{
					UIMgr.Inst.ShowActiveRelicGroupUI(relicGroupConfig);
				}
				if (relicGroupConfig.abilityType == RelicGroupAbilityType.AddMaxHp)
				{
					PlayerMgr.Inst.ChangeHPMax(relicGroupConfig.int1.result);
				}
			}
		}
		UIPlayerDataMgr.Inst.RelicUpdate();
		PlayerMgr.Inst.TryGetPlayerPpt(out var playerPpt);
		switch (relicConfig.abilityType)
		{
		case RelicAbilityType.Heavyweights:
			if (flag)
			{
				PlayerMgr.Inst.ChangeHPMax(relicConfig.int1.valueUpgrade);
				UnitDotsSyncSystem.UnitRecoveryHP(PlayerMgr.Inst.PlayerEtt, relicConfig.int1.valueUpgrade, ettMgr, needTextFloat: false, needCreateEF: false);
				PlayerMgr.Inst.ChangeBodySize(1f + (float)relicConfig.int2.valueUpgrade / 100f);
				PlayerMgr.Inst.ChangeKnockbackRatio((float)relicConfig.int3.valueUpgrade / 100f);
			}
			else
			{
				PlayerMgr.Inst.ChangeHPMax(relicConfig.int1.value);
				UnitDotsSyncSystem.UnitRecoveryHP(PlayerMgr.Inst.PlayerEtt, relicConfig.int1.value, ettMgr, needTextFloat: false, needCreateEF: false);
				PlayerMgr.Inst.ChangeBodySize((float)relicConfig.int2.value / 100f);
				PlayerMgr.Inst.ChangeKnockbackRatio((float)relicConfig.int3.value / 100f);
			}
			break;
		case RelicAbilityType.ImmuneSurface:
			playerPpt.ImmuneMucusRegister();
			ettMgr.SetComponentData(PlayerMgr.Inst.PlayerEtt, playerPpt);
			relicCfg_ImmuneSurface = relicConfig;
			break;
		case RelicAbilityType.BottomGuard:
			relicCfg_BottomGuard = relicConfig;
			break;
		case RelicAbilityType.StayAndFocus:
			if (relic_StayAndFocus == null)
			{
				relic_StayAndFocus = UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Item/Relic_StayAndFocus"), LevelMgr.Inst.tsf_PlayerThings).GetComponent<Relic_StayAndFocus>();
			}
			relic_StayAndFocus.Initialize(relicConfig);
			break;
		case RelicAbilityType.SpellCopy:
			relicCfg_SpellCopy = relicConfig;
			break;
		case RelicAbilityType.SuckBlood:
			relicCfg_SuckBlood = relicConfig;
			break;
		case RelicAbilityType.FollowGhost:
			relicCfg_FollowGhost = relicConfig;
			break;
		case RelicAbilityType.BlockSpell:
			if (relic_BlockSpellMono == null)
			{
				relic_BlockSpellMono = UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Item/Relic_BlockSpell"), LevelMgr.Inst.tsf_PlayerThings).GetComponent<Relic_BlockSpellMono>();
			}
			relic_BlockSpellMono.Initialize(relicConfig);
			break;
		case RelicAbilityType.InjuredAttackAll:
			relicCfg_InjuredAttackAll = relicConfig;
			break;
		case RelicAbilityType.GluttonousSnake:
			if (relic_GluttonousSnake == null)
			{
				relic_GluttonousSnake = UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Item/Relic_GluttonousSnake"), LevelMgr.Inst.tsf_PlayerThings).GetComponent<Relic_GluttonousSnake>();
			}
			relic_GluttonousSnake.Initialize(relicConfig);
			break;
		case RelicAbilityType.ImmuneGround:
			playerPpt.ImmuneVenomRegister();
			playerPpt.ImmuneMucusRegister();
			playerPpt.unitCfg.immuneSpike = true;
			ettMgr.SetComponentData(PlayerMgr.Inst.PlayerEtt, playerPpt);
			break;
		case RelicAbilityType.SpellDestroyer:
			if (relic_SpellDestroyer == null)
			{
				relic_SpellDestroyer = UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Item/Relic_SpellDestroyer"), LevelMgr.Inst.tsf_PlayerThings).GetComponent<Relic_SpellDestroyer>();
			}
			relic_SpellDestroyer.Initialize(relicConfig);
			break;
		case RelicAbilityType.Fly:
			if (relic_Fly == null)
			{
				relic_Fly = UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Item/Relic_Fly"), LevelMgr.Inst.tsf_PlayerThings).GetComponent<Relic_Fly>();
				playerPpt.FlyRegister();
				ettMgr.SetComponentData(PlayerMgr.Inst.PlayerEtt, playerPpt);
			}
			break;
		case RelicAbilityType.Alchemy:
			relicCfg_Alchemy = relicConfig;
			break;
		case RelicAbilityType.CoinHeal:
			relicCfg_CoinHeal = relicConfig;
			break;
		case RelicAbilityType.KillShield:
			PlayerMgr.Inst.ChangeShield(relicConfig.int1.result);
			relicCfg_KillShield = relicConfig;
			break;
		case RelicAbilityType.FloorShield:
			relicCfg_FloorShield = relicConfig;
			break;
		case RelicAbilityType.FloorInterest:
			if (relic_FollowObj_FloorInterest == null)
			{
				relic_FollowObj_FloorInterest = UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Item/Relic_FloorInterest"), LevelMgr.Inst.tsf_PlayerThings).GetComponent<Relic_FollowObj>();
			}
			relic_FollowObj_FloorInterest.Initialize(relicConfig);
			break;
		case RelicAbilityType.FloorRecovery:
			relicCfg_FloorRecovery = relicConfig;
			break;
		case RelicAbilityType.Seckill:
			relicCfg_Seckill = relicConfig;
			break;
		case RelicAbilityType.Variator:
			if (relic_Variator == null)
			{
				relic_Variator = UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Item/Relic_Variator"), LevelMgr.Inst.tsf_PlayerThings).GetComponent<Relic_Variator>();
			}
			relic_Variator.Initialize(relicConfig);
			break;
		case RelicAbilityType.MPMax:
			relicCfg_MaxMP = relicConfig;
			break;
		case RelicAbilityType.MPRecovery:
			relicCfg_MPRecovery = relicConfig;
			break;
		case RelicAbilityType.AddRelicOption:
			relicCfg_AddRelicOption = relicConfig;
			break;
		case RelicAbilityType.SummonLimit:
			relicCfg_SummonLimit = relicConfig;
			break;
		case RelicAbilityType.PotionAddiction:
			relicCfg_PotionAddiction = relicConfig;
			break;
		case RelicAbilityType.EndlessBottle:
			relicCfg_EndlessBottle = relicConfig;
			break;
		case RelicAbilityType.SpecialStore:
			relic_SpecialStore = true;
			break;
		case RelicAbilityType.KillBackMP:
			relicCfg_KillBackMP = relicConfig;
			break;
		case RelicAbilityType.TempShield:
			relicCfg_TempShield = relicConfig;
			break;
		case RelicAbilityType.ReduceDamage:
			relicCfg_ReduceDamage = relicConfig;
			break;
		case RelicAbilityType.DecelerateShiled:
			if (relic_DecelerateShield == null)
			{
				relic_DecelerateShield = UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Item/Relic_DecelerateShield"), LevelMgr.Inst.tsf_PlayerThings).GetComponent<Relic_DecelerateShield>();
			}
			relic_DecelerateShield.Initialize(relicConfig);
			break;
		case RelicAbilityType.KillTempShiled:
			relicCfg_KillTempShiled = relicConfig;
			break;
		case RelicAbilityType.CurseWarrior:
			relicCfg_CurseWarrior = relicConfig;
			break;
		case RelicAbilityType.RerollBlessing:
			relicCfg_RerollRelic = relicConfig;
			break;
		case RelicAbilityType.ShootNoSlowdown:
			relic_ShootNotSlowdown = true;
			break;
		case RelicAbilityType.RandomLevelUp:
			Debug.LogError("这个祝福在加入字典前就应该处理，不会走到这一步");
			break;
		case RelicAbilityType.RemoteShoot:
			if (relic_RemoteShoot == null)
			{
				relic_RemoteShoot = UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Item/Relic_RemoteShoot"), LevelMgr.Inst.tsf_PlayerThings).GetComponent<Relic_RemoteShoot>();
			}
			relic_RemoteShoot.Initialize(relicConfig);
			break;
		case RelicAbilityType.Dodge:
			relicCfg_Dodge = relicConfig;
			break;
		case RelicAbilityType.MoneyIsPower:
			relicCfg_MoneyIsPower = relicConfig;
			break;
		case RelicAbilityType.WandAddSlot:
		{
			relicCfg_WandAddSlot = relicConfig;
			for (int j = 0; j < PlayerMgr.Inst.BaData.wandCfgs.Count; j++)
			{
				PlayerMgr.Inst.WandCheckSlotCount(j);
			}
			break;
		}
		case RelicAbilityType.NoAttackStealth:
			relicCfg_NoAttackStealth = relicConfig;
			break;
		case RelicAbilityType.Resurgence:
			if (relic_Resurgence == null)
			{
				relic_Resurgence = UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Item/Relic_Resurgence"), LevelMgr.Inst.tsf_PlayerThings).GetComponent<Relic_Resurgence>();
			}
			relic_Resurgence.Initialize(relicConfig);
			break;
		case RelicAbilityType.SaintSword:
			if (relic_SaintSword == null)
			{
				relic_SaintSword = UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Item/Relic_SaintSword"), LevelMgr.Inst.tsf_PlayerThings).GetComponent<Relic_SaintSword>();
			}
			relic_SaintSword.Initialize(relicConfig);
			break;
		case RelicAbilityType.AddCriticalChance:
			relicCfg_AddCriticalChance = relicConfig;
			break;
		case RelicAbilityType.GreedSeed:
			if (relic_GreedSeed == null)
			{
				relic_GreedSeed = UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Item/Relic_GreedSeed"), LevelMgr.Inst.tsf_PlayerThings).GetComponent<Relic_GreedSeed>();
			}
			relic_GreedSeed.Initialize(relicConfig);
			break;
		case RelicAbilityType.LongNeck:
			relicCfg_LongNeck = relicConfig;
			PlayerMgr.Inst.UpdateSkin();
			break;
		case RelicAbilityType.AddDamage:
			relicCfg_AddDamage = relicConfig;
			break;
		case RelicAbilityType.SpellThroughWall:
			relic_SpellThroughWall = true;
			break;
		case RelicAbilityType.SpellKnockback:
		{
			relicCfg_SpellKnockback = relicConfig;
			for (int n = 0; n < PlayerMgr.Inst.Wands.Count; n++)
			{
				if (PlayerMgr.Inst.Wands[n] != null && PlayerMgr.Inst.Wands[n].tsf_Layer.Find(RelicAbilityType.SpellKnockback.ToString()) == null)
				{
					GameObject obj2 = UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Item/Relic_SpellKnockback"), PlayerMgr.Inst.Wands[n].tsf_Layer);
					obj2.transform.localPosition = Vector3.zero;
					obj2.transform.localRotation = Quaternion.identity;
					obj2.name = RelicAbilityType.SpellKnockback.ToString();
				}
			}
			break;
		}
		case RelicAbilityType.LowerUndifferDamageRatio:
			relicCfg_MaxUndifferDamage = relicConfig;
			break;
		case RelicAbilityType.AddCriticalDamage:
			relicCfg_AddCriticalDamage = relicConfig;
			break;
		case RelicAbilityType.AddRadiusOfInfluence:
			relicCfg_AddRadiusOfInfluence = relicConfig;
			RelicUpdateRadius();
			break;
		case RelicAbilityType.AddMoveSpeed:
			if (relic_AddMoveSpeed == null)
			{
				relic_AddMoveSpeed = UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Item/Relic_AddMoveSpeed"), LevelMgr.Inst.tsf_PlayerThings).GetComponent<Relic_AddMoveSpeed>();
			}
			relic_AddMoveSpeed.Initialize(relicConfig);
			break;
		case RelicAbilityType.PowerfulMan:
			relicCfg_PowerfulMan = relicConfig;
			PlayerMgr.Inst.UpdateSkin();
			break;
		case RelicAbilityType.SilverKey:
			if (relic_FollowObj_SilverKey == null)
			{
				relic_FollowObj_SilverKey = UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Item/Relic_SilverKey"), LevelMgr.Inst.tsf_PlayerThings).GetComponent<Relic_FollowObj>();
			}
			relic_FollowObj_SilverKey.Initialize(relicConfig);
			break;
		case RelicAbilityType.MadEye:
			relicCfg_MadEye = relicConfig;
			break;
		case RelicAbilityType.ShowUnitHP:
		{
			relicCfg_ShowUnitHPUI = relicConfig;
			PlayerMgr.Inst.UpdateSkin();
			using (EntityQuery entityQuery = ettMgr.CreateEntityQuery(typeof(AllMixedEtt)))
			{
				AllMixedEtt singleton = entityQuery.GetSingleton<AllMixedEtt>();
				using EntityQuery entityQuery2 = ettMgr.CreateEntityQuery(typeof(UnitProperty_Dots));
				NativeArray<Entity> nativeArray = entityQuery2.ToEntityArray(Allocator.Temp);
				for (int m = 0; m < nativeArray.Length; m++)
				{
					UnitProperty_Dots componentData = ettMgr.GetComponentData<UnitProperty_Dots>(nativeArray[m]);
					if (componentData.unitCfg.relicShowHPUIHight != 0f && componentData.unitCfg.unitType != UnitType.Elite)
					{
						if (!ettMgr.HasComponent<Relic_ShowUnitHP>(componentData.ett_RelicShowUnitHP))
						{
							componentData.isRelicShowUnitHPCreate = true;
							componentData.ett_RelicShowUnitHP = ettMgr.Instantiate(singleton.map["Relic_ShowUnitHP"]);
						}
						Relic_ShowUnitHP componentData2 = ettMgr.GetComponentData<Relic_ShowUnitHP>(componentData.ett_RelicShowUnitHP);
						componentData2.Initialized(nativeArray[m], relicCfg_ShowUnitHPUI.level);
						ettMgr.SetComponentData(componentData.ett_RelicShowUnitHP, componentData2);
						ettMgr.SetComponentData(nativeArray[m], componentData);
					}
				}
			}
			break;
		}
		case RelicAbilityType.BloodKey:
			if (relic_FollowObj_BloodKey == null)
			{
				relic_FollowObj_BloodKey = UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Item/Relic_BloodKey"), LevelMgr.Inst.tsf_PlayerThings).GetComponent<Relic_FollowObj>();
			}
			relic_FollowObj_BloodKey.Initialize(relicConfig);
			break;
		case RelicAbilityType.FiniteGlove:
			if (relic_FiniteGlove == null)
			{
				relic_FiniteGlove = UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Item/Relic_FiniteGlove"), LevelMgr.Inst.tsf_PlayerThings).GetComponent<Relic_FiniteGlove>();
			}
			relic_FiniteGlove.Initialize(relicConfig);
			break;
		case RelicAbilityType.ExtraDoor:
			relic_ExtraDoor = true;
			break;
		case RelicAbilityType.PostSlotMoreEfficiency:
		{
			relicCfg_PostSlotMoreEfficiency = relicConfig;
			for (int l = 0; l < PlayerMgr.Inst.Wands.Count; l++)
			{
				if (PlayerMgr.Inst.Wands[l] != null && PlayerMgr.Inst.Wands[l].tsf_Layer.Find(RelicAbilityType.PostSlotMoreEfficiency.ToString()) == null)
				{
					GameObject obj = UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Item/Relic_PostSlotMoreEfficiency"), PlayerMgr.Inst.Wands[l].tsf_Layer);
					obj.transform.localPosition = Vector3.zero;
					obj.transform.localRotation = Quaternion.identity;
					obj.name = RelicAbilityType.PostSlotMoreEfficiency.ToString();
				}
			}
			break;
		}
		case RelicAbilityType.FreeGods:
			relicCfg_FreeGods = relicConfig;
			break;
		case RelicAbilityType.MoreChanceEncounterChest:
			relic_CertainlyHaveRRO = true;
			break;
		case RelicAbilityType.EndlessChest:
			relicCfg_EndlessChest = relicConfig;
			break;
		case RelicAbilityType.RestartKey:
			Debug.LogError("在方法开头就处理了");
			break;
		case RelicAbilityType.PandorasBox:
			relicCfg_PandorasBox = relicConfig;
			UIPlayerDataMgr.Inst.BagCheckRelicPandorasBoxImage();
			break;
		case RelicAbilityType.InjuredAddMoveSpeed:
			if (relic_InjuredAddMoveSpeed == null)
			{
				relic_InjuredAddMoveSpeed = UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Item/Relic_InjuredAddMoveSpeed"), LevelMgr.Inst.tsf_PlayerThings).GetComponent<Relic_InjuredAddMoveSpeed>();
			}
			relic_InjuredAddMoveSpeed.Initialize(relicConfig);
			break;
		case RelicAbilityType.RainbowRibbon:
			if (relic_RainbowRibbon == null)
			{
				relic_RainbowRibbon = UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Item/Relic_RainbowRibbon"), LevelMgr.Inst.tsf_PlayerThings).GetComponent<Relic_RainbowRibbon>();
			}
			break;
		case RelicAbilityType.EnterDoorRemoveCurse:
			relicCfg_EnterDoorRemoveCurse = relicConfig;
			break;
		case RelicAbilityType.InvisibleWing:
			if (relic_InvisibleWing == null)
			{
				relic_InvisibleWing = UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Item/Relic_InvisibleWing"), LevelMgr.Inst.tsf_PlayerThings).GetComponent<Relic_InvisibleWing>();
			}
			relic_InvisibleWing.Intialize(relicConfig);
			break;
		case RelicAbilityType.MadWarrior:
			if (relic_MadWarrior == null)
			{
				relic_MadWarrior = UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Item/Relic_MadWarrior"), LevelMgr.Inst.tsf_PlayerThings).GetComponent<Relic_MadWarrior>();
			}
			relic_MadWarrior.Intialize(relicConfig);
			break;
		case RelicAbilityType.ReduceSkillCD:
			relicCfg_ReduceSkillCD = relicConfig;
			break;
		case RelicAbilityType.PickMoreRelic:
			relicCfg_PickMoreRelic = relicConfig;
			PlayerMgr.Inst.UpdateSkin();
			break;
		case RelicAbilityType.MoreMaxHPOutput:
			relicCfg_MoreMaxHPOutput = relicConfig;
			break;
		case RelicAbilityType.MoreCoinOutput:
			relicCfg_MoreCoinOutput = relicConfig;
			break;
		case RelicAbilityType.KeyIsPower:
			relicCfg_KeyIsPower = relicConfig;
			break;
		case RelicAbilityType.ExtraPotionStorage:
			relicCfg_ExtraPotionStorage = relicConfig;
			PlayerMgr.Inst.BaData.potionMaxCount = PlayerMgr.Inst.ItemCtrller.CaculatePotionStorage();
			PlayerMgr.Inst.ItemCtrller.PotionChangeSlotDelay();
			break;
		case RelicAbilityType.LessWandMoreSlot:
		{
			relicCfg_LessWandMoreSlot = relicConfig;
			PlayerMgr.Inst.WandLimitChange(relicConfig.int1.result);
			for (int k = 0; k < PlayerMgr.Inst.BaData.wandCfgs.Count; k++)
			{
				PlayerMgr.Inst.WandCheckSlotCount(k);
			}
			break;
		}
		case RelicAbilityType.AddSpellOption:
			relicCfg_AddSpellOption = relicConfig;
			break;
		case RelicAbilityType.SpellReroll:
			relicCfg_RerollSpell = relicConfig;
			break;
		case RelicAbilityType.PickMoreSpell:
			relicCfg_PickMoreSpell = relicConfig;
			break;
		case RelicAbilityType.EndlessExtraDamage:
			relic_EndlessExtraDamage = relicConfig;
			break;
		case RelicAbilityType.EndlessExtraHP:
			PlayerMgr.Inst.ChangeHPMax(relicConfig.int1.valueUpgrade, TextFloatQueueType.None);
			UnitDotsSyncSystem.UnitRecoveryHP(PlayerMgr.Inst.PlayerEtt, relicConfig.int1.valueUpgrade, ettMgr, needTextFloat: false, needCreateEF: false);
			relic_EndlessExtraMaxHP = relicConfig;
			break;
		case RelicAbilityType.MirrorOfSoul:
			if (relic_MirrorOfSoul == null)
			{
				relic_MirrorOfSoul = UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Item/Relic_MirrorOfSoul"), LevelMgr.Inst.tsf_PlayerThings).GetComponent<Relic_MirrorOfSoul>();
			}
			relic_MirrorOfSoul.Initialize(relicConfig);
			break;
		case RelicAbilityType.LightArmor:
			if (uiRelic_LightArmor == null)
			{
				uiRelic_LightArmor = UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/UI/UIRelic_LightArmor"), UIPlayerDataMgr.Inst.rtsf_ActiveRelicUIRoot).GetComponent<UIRelic_LightArmor>();
			}
			uiRelic_LightArmor.transform.localPosition = Vector3.zero;
			if (GameMgr.IsMobile_Static)
			{
				RectTransform component3 = uiRelic_LightArmor.GetComponent<RectTransform>();
				component3.anchoredPosition = new Vector2(UIPlayerDataMgr.Inst.skillUIOffsetMobile[1], component3.anchoredPosition.y);
			}
			uiRelic_LightArmor.Initialize(relicConfig);
			break;
		case RelicAbilityType.WarmSnow:
			if (uiRelic_WarmSnow == null)
			{
				uiRelic_WarmSnow = UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/UI/UIRelic_GrassCloth"), UIPlayerDataMgr.Inst.rtsf_ActiveRelicUIRoot).GetComponent<UIRelic_GrassCloth>();
			}
			uiRelic_WarmSnow.transform.localPosition = Vector3.zero;
			if (GameMgr.IsMobile_Static)
			{
				RectTransform component2 = uiRelic_WarmSnow.GetComponent<RectTransform>();
				component2.anchoredPosition = new Vector2(UIPlayerDataMgr.Inst.skillUIOffsetMobile[2], component2.anchoredPosition.y);
			}
			uiRelic_WarmSnow.Initialize(relicConfig);
			PlayerMgr.Inst.UpdateSkin();
			break;
		case RelicAbilityType.Reaper:
			if (relic_Reaper == null)
			{
				relic_Reaper = UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Item/Relic_Reaper"), LevelMgr.Inst.tsf_PlayerThings).GetComponent<Relic_Reaper>();
			}
			relic_Reaper.Initialize(relicConfig);
			PlayerMgr.Inst.UpdateSkin();
			break;
		case RelicAbilityType.Hunag:
			if (relic_Huang == null)
			{
				relic_Huang = UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Item/Relic_Huang"), LevelMgr.Inst.tsf_PlayerThings).GetComponent<Relic_Huang>();
			}
			relic_Huang.Initialize(relicConfig, inPlot: false);
			PlayerMgr.Inst.UpdateSkin();
			break;
		case RelicAbilityType.DruidRing:
			if (relic_DruidRing == null)
			{
				GameObject original = ABResources.LoadAsset<GameObject>("Prefabs/Item/Relic_DruidRing");
				relic_DruidRing = UnityEngine.Object.Instantiate(original, LevelMgr.Inst.tsf_PlayerThings).GetComponent<Relic_DruidRing>();
			}
			relic_DruidRing.Initialize(relicConfig);
			break;
		case RelicAbilityType.MedicineKit:
			if (relic_MedicineKit == null)
			{
				relic_MedicineKit = UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Item/Relic_MedicineKit"), LevelMgr.Inst.tsf_PlayerThings).GetComponent<Relic_MedicineKit>();
			}
			relic_MedicineKit.Initialize(relicConfig);
			break;
		case RelicAbilityType.DivingSuit:
			relic_DivingSuit = relicConfig;
			PlayerMgr.Inst.UpdateSkin();
			if (uiRelic_DaveHarpoons == null)
			{
				uiRelic_DaveHarpoons = UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/UI/UIRelic_DaveHarpoons"), UIPlayerDataMgr.Inst.rtsf_ActiveRelicUIRoot).GetComponent<UIRelic_DaveHarpoons>();
			}
			uiRelic_DaveHarpoons.transform.localPosition = Vector3.zero;
			if (GameMgr.IsMobile_Static)
			{
				RectTransform component = uiRelic_DaveHarpoons.GetComponent<RectTransform>();
				component.anchoredPosition = new Vector2(UIPlayerDataMgr.Inst.skillUIOffsetMobile[2], component.anchoredPosition.y);
			}
			uiRelic_DaveHarpoons.Initialize(relicConfig);
			break;
		case RelicAbilityType.RuneWizard:
			if (uiRelic_RuneWizard == null)
			{
				uiRelic_RuneWizard = UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/UI/UIRelic_RuneWizard"), UIPlayerDataMgr.Inst.rtsf_ActiveRelicUIRoot).GetComponent<UiRelic_RuneWizard>();
			}
			uiRelic_RuneWizard.UpdateRuneCounter();
			break;
		case RelicAbilityType.PowerfulHarpoonHead:
			relic_PowerfulHarpoonHead = relicConfig;
			break;
		case RelicAbilityType.PoisonousHarpoonHead:
			relic_PoisonousHarpoonHead = relicConfig;
			break;
		case RelicAbilityType.LightningHarpoonHead:
			relic_LightningHarpoonHead = relicConfig;
			break;
		case RelicAbilityType.FrozenHarpoonHead:
			relic_FrozenHarpoonHead = relicConfig;
			break;
		case RelicAbilityType.FlameHarpoonHead:
			relic_FlameHarpoonHead = relicConfig;
			break;
		case RelicAbilityType.ExpandHarpoonHead:
			relic_HarpoonsHeadExtend = relicConfig;
			break;
		case RelicAbilityType.MagicThing:
			if (flag)
			{
				PlayerMgr.Inst.ChangeHPMax(relicConfig.int1.valueUpgrade);
			}
			else
			{
				PlayerMgr.Inst.ChangeHPMax(relicConfig.int1.value);
			}
			break;
		default:
			Debug.LogError(relicConfig.abilityType);
			break;
		}
		if (relicConfig.skinName != "")
		{
			PlayerMgr.Inst.UpdateSkin();
		}
		if (addGallery)
		{
			DataMgr.selectedWorldData.GalleryRelicGet(id);
		}
	}

	public static int? GetRelicFlyPreProcess(int id, bool removeFromPool = true)
	{
		if (RelicConfig.dic[id].abilityType == RelicAbilityType.RandomLevelUp)
		{
			List<int> list = new List<int>();
			for (int i = 0; i < PlayerMgr.Inst.BaData.relicCfgs.Count; i++)
			{
				if (PlayerMgr.Inst.BaData.relicCfgs[i].dropType != ItemDropType.Special && UIPlayerDataMgr.Inst.CurrentRelicLevel(PlayerMgr.Inst.BaData.relicCfgs[i].id) < PlayerMgr.Inst.BaData.relicCfgs[i].maxCount)
				{
					list.Add(PlayerMgr.Inst.BaData.relicCfgs[i].id);
				}
			}
			DataMgr.selectedWorldData.GalleryRelicGet(id);
			if (list.Count == 0)
			{
				int relicFromPool = PlayerMgr.Inst.BaData.GetRelicFromPool(ItemDropType.Common);
				PlayerMgr.Inst.PlayerCtrller.TextFloatQueueAdd(1002202.GetText() + ": " + RelicConfig.dic[relicFromPool].GetName(), UITextFloatType.Normal);
				return relicFromPool;
			}
			int num = list[UnityEngine.Random.Range(0, list.Count)];
			PlayerMgr.Inst.BaData.RemoveRelicFromPool(num);
			PlayerMgr.Inst.PlayerCtrller.TextFloatQueueAdd(1002004.GetText() + ": " + RelicConfig.dic[num].GetName(), UITextFloatType.Normal);
			return num;
		}
		if (id == 69)
		{
			PlayerMgr.Inst.ItemCtrller.RelicAdd(id);
			return null;
		}
		return id;
	}

	public void RelicRemove(int id, int reduceLevel = 99999999)
	{
		RelicConfig relicConfig = GetRelicConfig(id);
		int relicIndex = GetRelicIndex(id);
		if (relicIndex == -1)
		{
			Debug.LogError("移除遗物，但没有这个遗物ID:" + id);
			return;
		}
		if (relicConfig.level > reduceLevel)
		{
			relicConfig.level -= reduceLevel;
			relicConfig.CalculateAbility();
		}
		else
		{
			PlayerMgr.Inst.BaData.relicCfgs.RemoveAt(relicIndex);
			relicConfig.CalculateAbility();
			relicConfig.level = 0;
			int? relicGroupIdByRelicId = RelicGroupConfig.GetRelicGroupIdByRelicId(relicConfig.id);
			if (relicGroupIdByRelicId.HasValue && relicGroupConfigs.Remove(relicGroupIdByRelicId.Value, out var value) && value.abilityType == RelicGroupAbilityType.AddMaxHp)
			{
				PlayerMgr.Inst.ChangeHPMax(-value.int1.result);
			}
		}
		UIPlayerDataMgr.Inst.RelicUpdate();
		ReduceRelicProcess(relicConfig, reduceLevel);
		if (relicConfig.skinName != "")
		{
			PlayerMgr.Inst.UpdateSkin();
		}
	}

	private void ReduceRelicProcess(RelicConfig _relicCfg, int reduceLevel)
	{
		bool flag = _relicCfg.level > 0;
		PlayerMgr.Inst.TryGetPlayerPpt(out var playerPpt);
		switch (_relicCfg.abilityType)
		{
		case RelicAbilityType.Heavyweights:
			if (flag)
			{
				PlayerMgr.Inst.ChangeHPMax(-_relicCfg.int1.value * reduceLevel);
				PlayerMgr.Inst.ChangeBodySize(1f / Mathf.Pow((float)_relicCfg.int2.value / 100f, reduceLevel));
				PlayerMgr.Inst.ChangeKnockbackRatio((float)(-_relicCfg.int3.value) / 100f * (float)reduceLevel);
			}
			else
			{
				PlayerMgr.Inst.ChangeHPMax(-_relicCfg.int1.result);
				PlayerMgr.Inst.ChangeBodySize(1f / ((float)_relicCfg.int2.result / 100f));
				PlayerMgr.Inst.ChangeKnockbackRatio((float)(-_relicCfg.int3.result) / 100f);
			}
			break;
		case RelicAbilityType.ImmuneSurface:
			playerPpt.ImmuneMucusUnregister();
			ettMgr.SetComponentData(PlayerMgr.Inst.PlayerEtt, playerPpt);
			if (flag)
			{
				relicCfg_ImmuneSurface = _relicCfg;
			}
			else
			{
				relicCfg_ImmuneSurface = null;
			}
			break;
		case RelicAbilityType.BottomGuard:
			if (flag)
			{
				relicCfg_BottomGuard = _relicCfg;
			}
			else
			{
				relicCfg_BottomGuard = null;
			}
			break;
		case RelicAbilityType.StayAndFocus:
			if (flag)
			{
				relic_StayAndFocus.Initialize(_relicCfg);
				break;
			}
			relic_StayAndFocus.DestroySelf();
			relic_StayAndFocus = null;
			break;
		case RelicAbilityType.SpellCopy:
			if (flag)
			{
				relicCfg_SpellCopy = _relicCfg;
			}
			else
			{
				relicCfg_SpellCopy = null;
			}
			break;
		case RelicAbilityType.SuckBlood:
			if (flag)
			{
				relicCfg_SuckBlood = _relicCfg;
			}
			else
			{
				relicCfg_SuckBlood = null;
			}
			break;
		case RelicAbilityType.FollowGhost:
			if (flag)
			{
				relicCfg_FollowGhost = _relicCfg;
			}
			else
			{
				relicCfg_FollowGhost = null;
			}
			break;
		case RelicAbilityType.BlockSpell:
			if (flag)
			{
				relic_BlockSpellMono.Initialize(_relicCfg);
				break;
			}
			relic_BlockSpellMono.DestroySelf();
			relic_BlockSpellMono = null;
			break;
		case RelicAbilityType.InjuredAttackAll:
			if (flag)
			{
				relicCfg_InjuredAttackAll = _relicCfg;
			}
			else
			{
				relicCfg_InjuredAttackAll = null;
			}
			break;
		case RelicAbilityType.GluttonousSnake:
			if (flag)
			{
				relic_GluttonousSnake.Initialize(_relicCfg);
				break;
			}
			relic_GluttonousSnake.DestroySelf();
			relic_GluttonousSnake = null;
			break;
		case RelicAbilityType.ImmuneGround:
			playerPpt.ImmuneVenomUnregister();
			playerPpt.ImmuneMucusUnregister();
			playerPpt.unitCfg.immuneSpike = false;
			ettMgr.SetComponentData(PlayerMgr.Inst.PlayerEtt, playerPpt);
			break;
		case RelicAbilityType.SpellDestroyer:
			if (flag)
			{
				relic_SpellDestroyer.Initialize(_relicCfg);
				break;
			}
			relic_SpellDestroyer.DestroySelf();
			relic_SpellDestroyer = null;
			break;
		case RelicAbilityType.Fly:
			if (flag)
			{
				Debug.LogError("理论上这个道具只有一级");
				break;
			}
			relic_Fly.DestroySelf();
			relic_Fly = null;
			break;
		case RelicAbilityType.Alchemy:
			if (flag)
			{
				relicCfg_Alchemy = _relicCfg;
			}
			else
			{
				relicCfg_Alchemy = null;
			}
			break;
		case RelicAbilityType.CoinHeal:
			if (flag)
			{
				relicCfg_CoinHeal = _relicCfg;
			}
			else
			{
				relicCfg_CoinHeal = null;
			}
			break;
		case RelicAbilityType.KillShield:
			if (flag)
			{
				relicCfg_KillShield = _relicCfg;
			}
			else
			{
				relicCfg_KillShield = null;
			}
			break;
		case RelicAbilityType.FloorShield:
			if (flag)
			{
				relicCfg_FloorShield = _relicCfg;
			}
			else
			{
				relicCfg_FloorShield = null;
			}
			break;
		case RelicAbilityType.FloorInterest:
			if (flag)
			{
				relic_FollowObj_FloorInterest.Initialize(_relicCfg);
				break;
			}
			relic_FollowObj_FloorInterest.DestroySelf();
			relic_FollowObj_FloorInterest = null;
			break;
		case RelicAbilityType.FloorRecovery:
			if (flag)
			{
				relicCfg_FloorRecovery = _relicCfg;
			}
			else
			{
				relicCfg_FloorRecovery = null;
			}
			break;
		case RelicAbilityType.Seckill:
			if (flag)
			{
				relicCfg_Seckill = _relicCfg;
			}
			else
			{
				relicCfg_Seckill = null;
			}
			break;
		case RelicAbilityType.Variator:
			if (flag)
			{
				relic_Variator.Initialize(_relicCfg);
				break;
			}
			relic_Variator.DestroySelf();
			relic_Variator = null;
			break;
		case RelicAbilityType.MPMax:
			if (flag)
			{
				relicCfg_MaxMP = _relicCfg;
			}
			else
			{
				relicCfg_MaxMP = null;
			}
			break;
		case RelicAbilityType.MPRecovery:
			if (flag)
			{
				relicCfg_MPRecovery = _relicCfg;
			}
			else
			{
				relicCfg_MPRecovery = null;
			}
			break;
		case RelicAbilityType.AddRelicOption:
			if (flag)
			{
				relicCfg_AddRelicOption = _relicCfg;
			}
			else
			{
				relicCfg_AddRelicOption = null;
			}
			break;
		case RelicAbilityType.SummonLimit:
			if (flag)
			{
				relicCfg_SummonLimit = _relicCfg;
			}
			else
			{
				relicCfg_SummonLimit = null;
			}
			break;
		case RelicAbilityType.PotionAddiction:
			if (flag)
			{
				relicCfg_PotionAddiction = _relicCfg;
			}
			else
			{
				relicCfg_PotionAddiction = null;
			}
			break;
		case RelicAbilityType.EndlessBottle:
			if (flag)
			{
				relicCfg_EndlessBottle = _relicCfg;
			}
			else
			{
				relicCfg_EndlessBottle = null;
			}
			break;
		case RelicAbilityType.SpecialStore:
			relic_SpecialStore = false;
			break;
		case RelicAbilityType.KillBackMP:
			if (flag)
			{
				relicCfg_KillBackMP = _relicCfg;
			}
			else
			{
				relicCfg_KillBackMP = null;
			}
			break;
		case RelicAbilityType.TempShield:
			if (flag)
			{
				relicCfg_TempShield = _relicCfg;
			}
			else
			{
				relicCfg_TempShield = null;
			}
			break;
		case RelicAbilityType.ReduceDamage:
			if (flag)
			{
				relicCfg_ReduceDamage = _relicCfg;
			}
			else
			{
				relicCfg_ReduceDamage = null;
			}
			break;
		case RelicAbilityType.DecelerateShiled:
			if (flag)
			{
				relic_DecelerateShield.Initialize(_relicCfg);
				break;
			}
			relic_DecelerateShield.DestroySelf();
			relic_DecelerateShield = null;
			break;
		case RelicAbilityType.KillTempShiled:
			if (flag)
			{
				relicCfg_KillTempShiled = _relicCfg;
			}
			else
			{
				relicCfg_KillTempShiled = null;
			}
			break;
		case RelicAbilityType.CurseWarrior:
			if (flag)
			{
				relicCfg_CurseWarrior = _relicCfg;
			}
			else
			{
				relicCfg_CurseWarrior = null;
			}
			break;
		case RelicAbilityType.RerollBlessing:
			if (flag)
			{
				relicCfg_RerollRelic = _relicCfg;
			}
			else
			{
				relicCfg_RerollRelic = null;
			}
			break;
		case RelicAbilityType.ShootNoSlowdown:
			relic_ShootNotSlowdown = false;
			break;
		case RelicAbilityType.RandomLevelUp:
			Debug.LogError("这个遗物在加入字典前就应该处理，不会走到这一步");
			break;
		case RelicAbilityType.RemoteShoot:
			if (flag)
			{
				relic_RemoteShoot.Initialize(_relicCfg);
				break;
			}
			relic_RemoteShoot.DestroySelf();
			relic_RemoteShoot = null;
			break;
		case RelicAbilityType.Dodge:
			if (flag)
			{
				relicCfg_Dodge = _relicCfg;
			}
			else
			{
				relicCfg_Dodge = null;
			}
			break;
		case RelicAbilityType.MoneyIsPower:
			if (flag)
			{
				relicCfg_MoneyIsPower = _relicCfg;
			}
			else
			{
				relicCfg_MoneyIsPower = null;
			}
			break;
		case RelicAbilityType.WandAddSlot:
		{
			if (flag)
			{
				relicCfg_WandAddSlot = _relicCfg;
			}
			else
			{
				relicCfg_WandAddSlot = null;
			}
			for (int k = 0; k < PlayerMgr.Inst.BaData.wandCfgs.Count; k++)
			{
				PlayerMgr.Inst.WandCheckSlotCount(k);
			}
			{
				foreach (Wand wand in PlayerMgr.Inst.Wands)
				{
					wand.ResetAndRecheck();
				}
				break;
			}
		}
		case RelicAbilityType.NoAttackStealth:
			if (flag)
			{
				relicCfg_NoAttackStealth = _relicCfg;
				break;
			}
			relicCfg_NoAttackStealth = null;
			PlayerMgr.Inst.PlayerCtrller.SetVisiable();
			break;
		case RelicAbilityType.Resurgence:
			if (flag)
			{
				relic_Resurgence.Initialize(_relicCfg);
				break;
			}
			relic_Resurgence.DestroySelf();
			relic_Resurgence = null;
			break;
		case RelicAbilityType.SaintSword:
			if (flag)
			{
				relic_SaintSword.Initialize(_relicCfg);
				break;
			}
			relic_SaintSword.DestroySelf();
			relic_SaintSword = null;
			break;
		case RelicAbilityType.AddCriticalChance:
			if (flag)
			{
				relicCfg_AddCriticalChance = _relicCfg;
			}
			else
			{
				relicCfg_AddCriticalChance = null;
			}
			break;
		case RelicAbilityType.GreedSeed:
			if (flag)
			{
				Debug.LogError("理论上这个道具只有一级");
				break;
			}
			relic_GreedSeed.DestroySelf();
			relic_GreedSeed = null;
			break;
		case RelicAbilityType.LongNeck:
			if (flag)
			{
				relicCfg_LongNeck = _relicCfg;
			}
			else
			{
				relicCfg_LongNeck = null;
			}
			PlayerMgr.Inst.UpdateSkin();
			break;
		case RelicAbilityType.AddDamage:
			if (flag)
			{
				relicCfg_AddDamage = _relicCfg;
			}
			else
			{
				relicCfg_AddDamage = null;
			}
			break;
		case RelicAbilityType.SpellThroughWall:
			if (flag)
			{
				relic_SpellThroughWall = true;
			}
			else
			{
				relic_SpellThroughWall = false;
			}
			break;
		case RelicAbilityType.SpellKnockback:
		{
			if (flag)
			{
				relicCfg_SpellKnockback = _relicCfg;
				break;
			}
			relicCfg_SpellKnockback = null;
			for (int m = 0; m < PlayerMgr.Inst.Wands.Count; m++)
			{
				if (PlayerMgr.Inst.Wands[m] != null)
				{
					Transform transform2 = PlayerMgr.Inst.Wands[m].tsf_Layer.Find(RelicAbilityType.SpellKnockback.ToString());
					if (transform2 != null)
					{
						UnityEngine.Object.Destroy(transform2.gameObject);
					}
				}
			}
			break;
		}
		case RelicAbilityType.LowerUndifferDamageRatio:
			if (flag)
			{
				relicCfg_MaxUndifferDamage = _relicCfg;
			}
			else
			{
				relicCfg_MaxUndifferDamage = null;
			}
			break;
		case RelicAbilityType.AddCriticalDamage:
			if (flag)
			{
				relicCfg_AddCriticalDamage = _relicCfg;
			}
			else
			{
				relicCfg_AddCriticalDamage = null;
			}
			break;
		case RelicAbilityType.AddRadiusOfInfluence:
			if (flag)
			{
				relicCfg_AddRadiusOfInfluence = _relicCfg;
			}
			else
			{
				relicCfg_AddRadiusOfInfluence = null;
			}
			RelicUpdateRadius();
			break;
		case RelicAbilityType.AddMoveSpeed:
			if (flag)
			{
				relic_AddMoveSpeed.Initialize(_relicCfg);
				break;
			}
			relic_AddMoveSpeed.DestroySelf();
			relic_AddMoveSpeed = null;
			break;
		case RelicAbilityType.PowerfulMan:
			if (flag)
			{
				relicCfg_PowerfulMan = _relicCfg;
			}
			else
			{
				relicCfg_PowerfulMan = null;
			}
			PlayerMgr.Inst.UpdateSkin();
			break;
		case RelicAbilityType.SilverKey:
			if (flag)
			{
				relic_FollowObj_SilverKey.Initialize(_relicCfg);
				break;
			}
			relic_FollowObj_SilverKey.DestroySelf();
			relic_FollowObj_SilverKey = null;
			break;
		case RelicAbilityType.MadEye:
			if (flag)
			{
				relicCfg_MadEye = _relicCfg;
			}
			else
			{
				relicCfg_MadEye = null;
			}
			break;
		case RelicAbilityType.ShowUnitHP:
			if (flag)
			{
				relicCfg_ShowUnitHPUI = _relicCfg;
				using EntityQuery entityQuery = ettMgr.CreateEntityQuery(typeof(AllMixedEtt));
				AllMixedEtt singleton = entityQuery.GetSingleton<AllMixedEtt>();
				using EntityQuery entityQuery2 = ettMgr.CreateEntityQuery(typeof(UnitProperty_Dots));
				NativeArray<Entity> nativeArray = entityQuery2.ToEntityArray(Allocator.Temp);
				for (int j = 0; j < nativeArray.Length; j++)
				{
					UnitProperty_Dots componentData = ettMgr.GetComponentData<UnitProperty_Dots>(nativeArray[j]);
					if (componentData.unitCfg.unitType == UnitType.Monster || componentData.unitCfg.unitType == UnitType.Teammate || componentData.unitCfg.unitType == UnitType.TeammateNotAttack)
					{
						if (!ettMgr.HasComponent<Relic_ShowUnitHP>(componentData.ett_RelicShowUnitHP))
						{
							componentData.ett_RelicShowUnitHP = ettMgr.Instantiate(singleton.map["Relic_ShowUnitHP"]);
						}
						Relic_ShowUnitHP componentData2 = ettMgr.GetComponentData<Relic_ShowUnitHP>(componentData.ett_RelicShowUnitHP);
						componentData2.Initialized(nativeArray[j], relicCfg_ShowUnitHPUI.level);
						ettMgr.SetComponentData(componentData.ett_RelicShowUnitHP, componentData2);
						ettMgr.SetComponentData(nativeArray[j], componentData);
					}
				}
			}
			else
			{
				relicCfg_ShowUnitHPUI = null;
				using EntityQuery entityQuery3 = ettMgr.CreateEntityQuery(typeof(Relic_ShowUnitHP));
				NativeArray<Entity> entities = entityQuery3.ToEntityArray(Allocator.Temp);
				ettMgr.DestroyEntity(entities);
				entities.Dispose();
			}
			PlayerMgr.Inst.UpdateSkin();
			break;
		case RelicAbilityType.BloodKey:
			if (flag)
			{
				relic_FollowObj_BloodKey.Initialize(_relicCfg);
				break;
			}
			relic_FollowObj_BloodKey.DestroySelf();
			relic_FollowObj_BloodKey = null;
			break;
		case RelicAbilityType.FiniteGlove:
			if (flag)
			{
				relic_FiniteGlove.Initialize(_relicCfg);
				break;
			}
			relic_FiniteGlove.DestroySelf();
			relic_FiniteGlove = null;
			break;
		case RelicAbilityType.ExtraDoor:
			if (flag)
			{
				relic_ExtraDoor = true;
			}
			else
			{
				relic_ExtraDoor = false;
			}
			break;
		case RelicAbilityType.PostSlotMoreEfficiency:
		{
			if (flag)
			{
				relicCfg_PostSlotMoreEfficiency = _relicCfg;
				break;
			}
			relicCfg_PostSlotMoreEfficiency = null;
			for (int i = 0; i < PlayerMgr.Inst.Wands.Count; i++)
			{
				if (PlayerMgr.Inst.Wands[i] != null)
				{
					Transform transform = PlayerMgr.Inst.Wands[i].tsf_Layer.Find(RelicAbilityType.PostSlotMoreEfficiency.ToString());
					if (transform != null)
					{
						UnityEngine.Object.Destroy(transform.gameObject);
					}
				}
			}
			break;
		}
		case RelicAbilityType.FreeGods:
			if (flag)
			{
				relicCfg_FreeGods = _relicCfg;
			}
			else
			{
				relicCfg_FreeGods = null;
			}
			break;
		case RelicAbilityType.MoreChanceEncounterChest:
			if (flag)
			{
				relic_CertainlyHaveRRO = true;
			}
			else
			{
				relic_CertainlyHaveRRO = false;
			}
			break;
		case RelicAbilityType.EndlessChest:
			if (flag)
			{
				relicCfg_EndlessChest = _relicCfg;
			}
			else
			{
				relicCfg_EndlessChest = null;
			}
			break;
		case RelicAbilityType.RestartKey:
			Debug.LogError("不应该可以移除该诅咒");
			break;
		case RelicAbilityType.PandorasBox:
			if (flag)
			{
				relicCfg_PandorasBox = _relicCfg;
			}
			else
			{
				relicCfg_PandorasBox = null;
			}
			UIPlayerDataMgr.Inst.BagCheckRelicPandorasBoxImage();
			break;
		case RelicAbilityType.InjuredAddMoveSpeed:
			if (flag)
			{
				relic_InjuredAddMoveSpeed.Initialize(_relicCfg);
				break;
			}
			relic_InjuredAddMoveSpeed.DestroySelf();
			relic_InjuredAddMoveSpeed = null;
			break;
		case RelicAbilityType.RainbowRibbon:
			if (!flag)
			{
				relic_RainbowRibbon.DestroySelf();
				relic_RainbowRibbon = null;
			}
			break;
		case RelicAbilityType.EnterDoorRemoveCurse:
			if (flag)
			{
				relicCfg_EnterDoorRemoveCurse = _relicCfg;
			}
			else
			{
				relicCfg_EnterDoorRemoveCurse = null;
			}
			break;
		case RelicAbilityType.InvisibleWing:
			if (!flag)
			{
				relic_InvisibleWing.DestroySelf();
				relic_InvisibleWing = null;
			}
			break;
		case RelicAbilityType.MadWarrior:
			if (flag)
			{
				relic_MadWarrior.Intialize(_relicCfg);
				break;
			}
			relic_MadWarrior.DestroySelf();
			relic_MadWarrior = null;
			break;
		case RelicAbilityType.ReduceSkillCD:
			if (flag)
			{
				relicCfg_ReduceSkillCD = _relicCfg;
			}
			else
			{
				relicCfg_ReduceSkillCD = null;
			}
			break;
		case RelicAbilityType.PickMoreRelic:
			relicCfg_PickMoreRelic = (flag ? _relicCfg : null);
			PlayerMgr.Inst.UpdateSkin();
			break;
		case RelicAbilityType.MoreMaxHPOutput:
			relicCfg_MoreMaxHPOutput = (flag ? _relicCfg : null);
			break;
		case RelicAbilityType.MoreCoinOutput:
			relicCfg_MoreCoinOutput = (flag ? _relicCfg : null);
			break;
		case RelicAbilityType.KeyIsPower:
			if (flag)
			{
				relicCfg_KeyIsPower = _relicCfg;
			}
			else
			{
				relicCfg_KeyIsPower = null;
			}
			break;
		case RelicAbilityType.ExtraPotionStorage:
			relicCfg_ExtraPotionStorage = (flag ? _relicCfg : null);
			PlayerMgr.Inst.BaData.potionMaxCount = PlayerMgr.Inst.ItemCtrller.CaculatePotionStorage();
			PlayerMgr.Inst.ItemCtrller.PotionChangeSlotDelay();
			break;
		case RelicAbilityType.LessWandMoreSlot:
		{
			if (flag)
			{
				relicCfg_LessWandMoreSlot = _relicCfg;
			}
			else
			{
				relicCfg_LessWandMoreSlot = null;
				PlayerMgr.Inst.WandLimitChange(-_relicCfg.int1.result);
			}
			for (int l = 0; l < PlayerMgr.Inst.BaData.wandCfgs.Count; l++)
			{
				PlayerMgr.Inst.WandCheckSlotCount(l);
			}
			break;
		}
		case RelicAbilityType.AddSpellOption:
			if (flag)
			{
				relicCfg_AddSpellOption = _relicCfg;
			}
			else
			{
				relicCfg_AddSpellOption = null;
			}
			break;
		case RelicAbilityType.SpellReroll:
			relicCfg_RerollSpell = (flag ? _relicCfg : null);
			break;
		case RelicAbilityType.PickMoreSpell:
			relicCfg_PickMoreSpell = (flag ? _relicCfg : null);
			break;
		case RelicAbilityType.EndlessExtraDamage:
			relicCfg_PickMoreSpell = (flag ? _relicCfg : null);
			break;
		case RelicAbilityType.EndlessExtraHP:
			PlayerMgr.Inst.ChangeHPMax(-_relicCfg.int1.value * reduceLevel);
			relicCfg_PickMoreSpell = (flag ? _relicCfg : null);
			break;
		case RelicAbilityType.MirrorOfSoul:
			if (!flag)
			{
				relic_MirrorOfSoul.DestroySelf();
				relic_MirrorOfSoul = null;
			}
			else
			{
				relic_MirrorOfSoul.Initialize(_relicCfg);
			}
			break;
		case RelicAbilityType.LightArmor:
			if (!flag)
			{
				uiRelic_LightArmor.DestroySelf();
				uiRelic_LightArmor = null;
			}
			else
			{
				uiRelic_LightArmor.Initialize(_relicCfg);
			}
			break;
		case RelicAbilityType.WarmSnow:
			if (!flag)
			{
				uiRelic_WarmSnow.DestroySelf();
				uiRelic_WarmSnow = null;
				PlayerMgr.Inst.UpdateSkin();
			}
			else
			{
				uiRelic_WarmSnow.Initialize(_relicCfg);
			}
			break;
		case RelicAbilityType.Reaper:
			if (!flag)
			{
				relic_Reaper.DestroySelf();
				relic_Reaper = null;
				PlayerMgr.Inst.UpdateSkin();
			}
			else
			{
				relic_Reaper.Initialize(_relicCfg);
			}
			break;
		case RelicAbilityType.Hunag:
			if (!flag)
			{
				relic_Huang.DestroySelf();
				relic_Huang = null;
				PlayerMgr.Inst.UpdateSkin();
			}
			else
			{
				relic_Huang.Initialize(_relicCfg, inPlot: false);
			}
			break;
		case RelicAbilityType.DruidRing:
			if (flag)
			{
				relic_DruidRing.Initialize(_relicCfg);
				break;
			}
			relic_DruidRing.DestroySelf();
			relic_DruidRing = null;
			break;
		case RelicAbilityType.MedicineKit:
			if (flag)
			{
				relic_MedicineKit.Initialize(_relicCfg);
				break;
			}
			relic_MedicineKit.DestroySelf();
			relic_MedicineKit = null;
			break;
		case RelicAbilityType.DivingSuit:
			relic_DivingSuit = (flag ? _relicCfg : null);
			if (!flag)
			{
				uiRelic_DaveHarpoons.DestroySelf();
				uiRelic_DaveHarpoons = null;
				PlayerMgr.Inst.UpdateSkin();
			}
			else
			{
				uiRelic_DaveHarpoons.Initialize(_relicCfg);
			}
			break;
		case RelicAbilityType.RuneWizard:
			if (!flag)
			{
				uiRelic_RuneWizard.DestroySelf();
				uiRelic_RuneWizard = null;
			}
			else
			{
				uiRelic_RuneWizard.UpdateRuneCounter();
			}
			break;
		case RelicAbilityType.PowerfulHarpoonHead:
			relic_PowerfulHarpoonHead = (flag ? _relicCfg : null);
			break;
		case RelicAbilityType.PoisonousHarpoonHead:
			relic_PoisonousHarpoonHead = (flag ? _relicCfg : null);
			break;
		case RelicAbilityType.LightningHarpoonHead:
			relic_LightningHarpoonHead = (flag ? _relicCfg : null);
			break;
		case RelicAbilityType.FrozenHarpoonHead:
			relic_FrozenHarpoonHead = (flag ? _relicCfg : null);
			break;
		case RelicAbilityType.FlameHarpoonHead:
			relic_FlameHarpoonHead = (flag ? _relicCfg : null);
			break;
		case RelicAbilityType.ExpandHarpoonHead:
			relic_HarpoonsHeadExtend = (flag ? _relicCfg : null);
			break;
		case RelicAbilityType.MagicThing:
			if (flag)
			{
				PlayerMgr.Inst.ChangeHPMax(-_relicCfg.int1.value * reduceLevel);
			}
			else
			{
				PlayerMgr.Inst.ChangeHPMax(-_relicCfg.int1.result);
			}
			break;
		default:
			Debug.LogError(_relicCfg.abilityType);
			break;
		}
	}

	public void RelicUpdateRadius()
	{
		if (relic_DecelerateShield != null)
		{
			relic_DecelerateShield.UpdateRadius();
		}
		if (relic_SaintSword != null)
		{
			relic_SaintSword.UpdateRadius();
		}
	}

	public void PotionPickup(int id)
	{
		for (int i = 0; i < PlayerMgr.Inst.BaData.potionIDs.Count; i++)
		{
			if (PlayerMgr.Inst.BaData.potionIDs[i] == 0)
			{
				PlayerMgr.Inst.BaData.potionIDs[i] = id;
				UIPlayerDataMgr.Inst.uiPotionsCtrller.UpdateAllUI();
				UIPlayerDataMgr.Inst.UISlotPotionInfoUpdate();
				if (GameMgr.IsMobile_Static)
				{
					MobileMgr.inst.UpdateDrinkButton();
				}
				if (GameMgr.IsMobile_Static && !DataMgr.selectedWorldData.mobilePotionDragTutorialShown && DataMgr.selectedWorldData.battleData9.potionIDs.Count((int x) => x != 0) >= 2)
				{
					TopUI.inst.mobilePotionDragTutorial.gameObject.SetActive(value: true);
				}
				return;
			}
		}
		int selectedPotionID = SelectedPotionID;
		SelectedPotionID = id;
		Vector3 vector = new Vector3(UnityEngine.Random.Range(-0.02f, 0.02f), UnityEngine.Random.Range(-0.02f, 0.02f), 0f);
		QuickCreateSystem.Inst.CreateItem(LevelMgr.Inst.CurrentRoomMapPos, new ItemInfo(ItemType.Potion, selectedPotionID), PlayerMgr.Inst.PlayerPointIgnoreZ + vector);
		UIPlayerDataMgr.Inst.uiPotionsCtrller.UpdateAllUI();
		UIPlayerDataMgr.Inst.UISlotPotionInfoUpdate();
	}

	public void PotionUse()
	{
		if (SelectedPotionID == 0)
		{
			return;
		}
		UnitProperty_Dots componentData = ettMgr.GetComponentData<UnitProperty_Dots>(PlayerMgr.Inst.PlayerEtt);
		PotionConfig potionCfg = PotionConfig.dic[SelectedPotionID];
		switch (potionCfg.abilityType)
		{
		case PotionAbilityType.RecoverRandomHP:
		{
			int num17 = UnityEngine.Random.Range(potionCfg.int1, potionCfg.int2);
			UnitDotsSyncSystem.UnitRecoveryHP(PlayerMgr.Inst.PlayerEtt, num17, ettMgr, needTextFloat: false);
			PlayerMgr.Inst.PlayerCtrller.TextFloatQueueAdd("+" + num17, UITextFloatType.Recover);
			break;
		}
		case PotionAbilityType.LoseHPAddShield:
		{
			float num8 = componentData.unitCfg.maxHP - componentData.unitCfg.currentHP;
			if (num8 > (float)potionCfg.int1)
			{
				num8 = potionCfg.int1;
			}
			if (num8 > 0f)
			{
				PlayerMgr.Inst.ChangeShield((int)num8);
			}
			else
			{
				PlayerMgr.Inst.PlayerCtrller.TextFloatQueueAdd(1002003.GetText(), UITextFloatType.Normal);
			}
			break;
		}
		case PotionAbilityType.RemoveCurse:
			if (PlayerMgr.Inst.BaData.curseIDs.Count > 0)
			{
				CurseRemoveByIndex(UnityEngine.Random.Range(0, PlayerMgr.Inst.BaData.curseIDs.Count));
			}
			else
			{
				PlayerMgr.Inst.PlayerCtrller.TextFloatQueueAdd(1002003.GetText(), UITextFloatType.Normal);
			}
			break;
		case PotionAbilityType.RerollSpell:
		{
			bool flag = false;
			for (int j = 0; j < PlayerMgr.Inst.BaData.bagSpellDatas.Count; j++)
			{
				SlotData slotData = PlayerMgr.Inst.BaData.bagSpellDatas[j];
				if (slotData == null || slotData.isSealSlot)
				{
					continue;
				}
				SpellConfig spellConfig = SpellConfig.dic[slotData.id];
				if (spellConfig.dropType != ItemDropType.Special)
				{
					int num3 = 0;
					num3 = ((spellConfig.abilityType != SpellAbilityType.DeathAdder) ? PlayerMgr.Inst.BaData.GetSpellFromPool(spellConfig.level, spellConfig.dropType, spellConfig.id) : PlayerMgr.Inst.BaData.GetSpellFromPool(1, spellConfig.dropType, 10171));
					SlotData slotData2 = new SlotData(num3);
					PlayerMgr.Inst.BagSpellChange(j, null);
					if (PlayerMgr.Inst.CanBagSpellChange(j, slotData2))
					{
						PlayerMgr.Inst.BagSpellChange(j, slotData2);
					}
					else
					{
						PlayerMgr.Inst.SpawnSpellToGround(slotData2);
					}
					flag = true;
					if (UIPlayerDataMgr.Inst.IsBagOpen)
					{
						Vector3 position = UIPlayerDataMgr.Inst.rtsf_BagSpell.GetChild(j).transform.position;
						ObjPoolMgr.Inst.GetUIGO("Prefabs/Item/Potion_WhiteSmoke_UI", position, 2f);
					}
					DataMgr.selectedWorldData.GalleryUnlock(GalleryCategory.Spell, slotData2.id);
				}
			}
			UIPlayerDataMgr.Inst.UpdateBag();
			if (!flag)
			{
				PlayerMgr.Inst.PlayerCtrller.TextFloatQueueAdd(1002003.GetText(), UITextFloatType.Normal);
			}
			break;
		}
		case PotionAbilityType.Psychedelic:
			if (uiPotionPsychedelic == null)
			{
				uiPotionPsychedelic = UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/UI/UIPotion_Psychedelic"), UIBattleMgr.Inst.rtsf_CanvasThings).GetComponent<UIPotion_Psychedelic>();
			}
			uiPotionPsychedelic.Initialize(potionCfg);
			break;
		case PotionAbilityType.ManaRouse:
			UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Item/Potion_ManaRouse"), PlayerMgr.Inst.PlayerPoint, Quaternion.identity, PlayerMgr.Inst.PlayerT).GetComponent<Potion_ManaRouse>().Initialize(potionCfg);
			break;
		case PotionAbilityType.Refresh:
			foreach (KeyValuePair<Vector2Int, RoomController> roomCtrller in LevelMgr.Inst.RoomCtrllers)
			{
				if (BattleMgr.Inst.CurrentStage == 9 || BattleMgr.Inst.CurrentStage == 10)
				{
					PlayerMgr.Inst.PlayerCtrller.TextFloatQueueAdd(1002039.GetText().Replace("str1", 1001706.GetText()), UITextFloatType.Normal);
				}
				else if (roomCtrller.Value.doorEttList.Count == 1 && ettMgr.GetComponentData<DoorBase_Dots>(roomCtrller.Value.doorEttList[0]).rewardType == LevelRewardType.Chapter)
				{
					PlayerMgr.Inst.PlayerCtrller.TextFloatQueueAdd(1002003.GetText(), UITextFloatType.Normal);
				}
				else
				{
					List<LevelRewardType> list2 = new List<LevelRewardType>
					{
						LevelRewardType.Spell,
						LevelRewardType.Relic,
						LevelRewardType.Coin,
						LevelRewardType.MaxHP,
						LevelRewardType.Store,
						LevelRewardType.Process
					};
					if (DataMgr.selectedWorldData.GetResearchValueConsiderActive(ResearchAbilityType.Spring) != 0)
					{
						list2.Add(LevelRewardType.Spring);
					}
					for (int n = 0; n < roomCtrller.Value.doorEttList.Count; n++)
					{
						DoorBase_Dots componentData7 = ettMgr.GetComponentData<DoorBase_Dots>(roomCtrller.Value.doorEttList[n]);
						if (list2.Contains(componentData7.rewardType))
						{
							list2.Remove(componentData7.rewardType);
						}
					}
					list2.Upset();
					for (int num11 = 0; num11 < roomCtrller.Value.doorEttList.Count; num11++)
					{
						DoorBase_Dots componentData8 = ettMgr.GetComponentData<DoorBase_Dots>(roomCtrller.Value.doorEttList[num11]);
						componentData8.RefreshType(list2[num11]);
						ettMgr.SetComponentData(roomCtrller.Value.doorEttList[num11], componentData8);
						if (num11 < DataMgr.selectedWorldData.battleData9.nextRewardTypes.Count)
						{
							DataMgr.selectedWorldData.battleData9.nextRewardTypes[num11] = list2[num11];
						}
						else
						{
							DataMgr.selectedWorldData.battleData9.nextExtraDoorRewardType = list2[num11];
						}
						LevelMgr.Inst.RoomFinishLogger.next_room_options.Clear();
						LevelMgr.Inst.RoomFinishLogger.next_room_options.Add(list2[num11]);
					}
					if (BattleMgr.Inst.CurrentLevel == BattleMgr.Inst.stageLevelsCount[BattleMgr.Inst.CurrentStage - 1] - 1)
					{
						DataMgr.selectedWorldData.battleData9.specialRoomLevels.Remove(BattleMgr.Inst.CurrentLevel);
						DataMgr.selectedWorldData.battleData9.currentLevel--;
					}
				}
				using EntityQuery entityQuery4 = ettMgr.CreateEntityQuery(typeof(Item));
				NativeArray<Entity> nativeArray4 = entityQuery4.ToEntityArray(Allocator.Temp);
				for (int num12 = 0; num12 < nativeArray4.Length; num12++)
				{
					Item componentData9 = ettMgr.GetComponentData<Item>(nativeArray4[num12]);
					componentData9.onRefresh = true;
					ettMgr.SetComponentData(nativeArray4[num12], componentData9);
				}
				nativeArray4.Dispose();
			}
			break;
		case PotionAbilityType.UnlockThings:
		{
			bool flag5 = false;
			using (EntityQuery entityQuery5 = ettMgr.CreateEntityQuery(typeof(SpecialObj4_Dots)))
			{
				NativeArray<Entity> nativeArray5 = entityQuery5.ToEntityArray(Allocator.Temp);
				for (int num18 = 0; num18 < nativeArray5.Length; num18++)
				{
					if (ettMgr.GetComponentData<IRoomCtrller_Dots>(nativeArray5[num18]).belongRoom.Value.MapPos == LevelMgr.Inst.CurrentRoomMapPos)
					{
						SpecialObj4_Dots componentData14 = ettMgr.GetComponentData<SpecialObj4_Dots>(nativeArray5[num18]);
						if (!componentData14.alreadyOpen)
						{
							componentData14.isOpenByPotion = true;
							ettMgr.SetComponentData(nativeArray5[num18], componentData14);
							InteractiveObj_Dots componentData15 = ettMgr.GetComponentData<InteractiveObj_Dots>(nativeArray5[num18]);
							componentData15.onInteract = true;
							ettMgr.SetComponentData(nativeArray5[num18], componentData15);
							flag5 = true;
						}
					}
				}
				nativeArray5.Dispose();
				using EntityQuery entityQuery6 = ettMgr.CreateEntityQuery(typeof(SpecialObj4NoLock));
				NativeArray<Entity> nativeArray6 = entityQuery6.ToEntityArray(Allocator.Temp);
				for (int num19 = 0; num19 < nativeArray6.Length; num19++)
				{
					if (ettMgr.GetComponentData<IRoomCtrller_Dots>(nativeArray6[num19]).belongRoom.Value.MapPos == LevelMgr.Inst.CurrentRoomMapPos)
					{
						SpecialObj4NoLock componentData16 = ettMgr.GetComponentData<SpecialObj4NoLock>(nativeArray6[num19]);
						if (!componentData16.alreadyOpen)
						{
							componentData16.isOpenByPotion = true;
							ettMgr.SetComponentData(nativeArray6[num19], componentData16);
							flag5 = true;
						}
					}
				}
				nativeArray6.Dispose();
				for (int num20 = 0; num20 < LevelMgr.Inst.CurrentRoomCtrller.accessEttList.Count; num20++)
				{
					AccessBase_Dots componentData17 = ettMgr.GetComponentData<AccessBase_Dots>(LevelMgr.Inst.CurrentRoomCtrller.accessEttList[num20]);
					if (componentData17.needKey && !componentData17.alreadyUseKey)
					{
						componentData17.alreadyUseKey = true;
						if (LevelMgr.Inst.CurrentRoomCtrller.IsFinish)
						{
							componentData17.onOpen = true;
						}
						ettMgr.SetComponentData(LevelMgr.Inst.CurrentRoomCtrller.accessEttList[num20], componentData17);
						flag5 = true;
					}
				}
				if (!flag5)
				{
					PlayerMgr.Inst.PlayerCtrller.TextFloatQueueAdd(1002003.GetText(), UITextFloatType.Normal);
				}
			}
			break;
		}
		case PotionAbilityType.Midas:
		{
			bool flag3 = false;
			List<Entity> monsterEttList = LevelMgr.Inst.CurrentRoomCtrller.monsterEttList;
			for (int num14 = monsterEttList.Count - 1; num14 >= 0; num14--)
			{
				UnitProperty_Dots componentData10 = ettMgr.GetComponentData<UnitProperty_Dots>(monsterEttList[num14]);
				if (componentData10.unitCfg.unitType != UnitType.Elite && componentData10.unitCfg.unitType != UnitType.Boss && componentData10.unitCfg.id != 199901 && componentData10.unitCfg.triggerDeadEvent)
				{
					LocalTransform componentData11 = ettMgr.GetComponentData<LocalTransform>(monsterEttList[num14]);
					ObjPoolMgr.Inst.GetGO("Prefabs/Item/Potion_WhiteSmoke", componentData11.Position + new float3(0f, 0.4f, 0f), 2f);
					QuickCreateSystem.Inst.CreateItem(LevelMgr.Inst.CurrentRoomMapPos, new ItemInfo(ItemType.Resource, 11), Tool2D.GetNavMeshPointIngoreZ(componentData11.Position));
					componentData10.AnnouncedDeath(monsterEttList[num14]);
					ettMgr.SetComponentData(monsterEttList[num14], componentData10);
					flag3 = true;
				}
			}
			List<Entity> noAttackTriggerDeadEttList = LevelMgr.Inst.CurrentRoomCtrller.noAttackTriggerDeadEttList;
			for (int num15 = noAttackTriggerDeadEttList.Count - 1; num15 >= 0; num15--)
			{
				UnitProperty_Dots componentData12 = ettMgr.GetComponentData<UnitProperty_Dots>(noAttackTriggerDeadEttList[num15]);
				if (componentData12.unitCfg.unitType != UnitType.Elite && componentData12.unitCfg.unitType != UnitType.Boss && componentData12.unitCfg.id != 199901 && componentData12.unitCfg.triggerDeadEvent)
				{
					LocalTransform componentData13 = ettMgr.GetComponentData<LocalTransform>(noAttackTriggerDeadEttList[num15]);
					ObjPoolMgr.Inst.GetGO("Prefabs/Item/Potion_WhiteSmoke", componentData13.Position + new float3(0f, 0.4f, 0f), 2f);
					QuickCreateSystem.Inst.CreateItem(LevelMgr.Inst.CurrentRoomMapPos, new ItemInfo(ItemType.Resource, 11), Tool2D.GetNavMeshPointIngoreZ(componentData13.Position));
					componentData12.AnnouncedDeath(noAttackTriggerDeadEttList[num15]);
					ettMgr.SetComponentData(noAttackTriggerDeadEttList[num15], componentData12);
					flag3 = true;
				}
			}
			if (!flag3)
			{
				PlayerMgr.Inst.PlayerCtrller.TextFloatQueueAdd(1002003.GetText(), UITextFloatType.Normal);
			}
			break;
		}
		case PotionAbilityType.Discount:
		{
			EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
			entityQueryBuilder = entityQueryBuilder.WithAll<Item>();
			EntityQuery entityQuery = entityQueryBuilder.Build(ettMgr);
			NativeArray<Item> componentDataArray = entityQuery.ToComponentDataArray<Item>(Allocator.Temp);
			for (int k = 0; k < componentDataArray.Length; k++)
			{
				Item value = componentDataArray[k];
				if (value.belongRoomMapPos == LevelMgr.Inst.CurrentRoomMapPos)
				{
					value.SetPriceFactor(0.7f);
					componentDataArray[k] = value;
				}
			}
			entityQuery.CopyFromComponentDataArray(componentDataArray);
			EventMgr.PotionUse_Discount?.Invoke(0.7f);
			break;
		}
		case PotionAbilityType.Hover:
			if (potion_HoverEFGO == null)
			{
				potion_HoverEFGO = UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Item/Potion_Hover"), PlayerMgr.Inst.PlayerPoint, Quaternion.identity, PlayerMgr.Inst.PlayerT);
				PlayerMgr.Inst.FlyRegister();
			}
			break;
		case PotionAbilityType.Invisible:
			if (potion_Invisible == null)
			{
				potion_Invisible = UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Item/Potion_Invisible"), PlayerMgr.Inst.PlayerPoint, Quaternion.identity, PlayerMgr.Inst.PlayerT).GetComponent<Potion_Invisible>();
			}
			potion_Invisible.Initialize(potionCfg);
			break;
		case PotionAbilityType.Invincible:
			if (potion_Invincible == null)
			{
				potion_Invincible = UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Item/Potion_Invincible"), PlayerMgr.Inst.PlayerPoint, Quaternion.identity, PlayerMgr.Inst.PlayerT).GetComponent<Potion_Invincible>();
			}
			potion_Invincible.Initialize(potionCfg.float1);
			break;
		case PotionAbilityType.Petrifaction:
			if (potion_Petrifaction == null)
			{
				potion_Petrifaction = UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Item/Potion_Petrifaction"), PlayerMgr.Inst.PlayerPoint, Quaternion.identity, PlayerMgr.Inst.PlayerT).GetComponent<Potion_Petrifaction>();
				if (GameMgr.IsMobile_Static)
				{
					MobileMgr.inst.isUsingPetrifaction = true;
				}
				PlayerMgr.Inst.PlayerCtrller.StopMotion();
				PlayerMgr.Inst.PlayerCtrller.NonInteractiveRegister();
				PlayerMgr.Inst.InvincibleRegister();
				PlayerMgr.Inst.ImmuneKnockbackRegister();
			}
			potion_Petrifaction.Initialize(potionCfg);
			break;
		case PotionAbilityType.MoveSpeed:
			PlayerMgr.Inst.BaData.extraMoveSpeedRatio += (float)potionCfg.int1 / 100f;
			PlayerMgr.Inst.PlayerCtrller.TextFloatQueueAdd(1001901.GetText() + "+" + potionCfg.int1 + "%", UITextFloatType.Normal);
			break;
		case PotionAbilityType.BodySize:
			PlayerMgr.Inst.ChangeBodySize((UnityEngine.Random.Range(0, 2) == 0) ? potionCfg.float1 : (1f / potionCfg.float1));
			break;
		case PotionAbilityType.MPRecovery:
			PlayerMgr.Inst.ChangeMPRecovery(potionCfg.int1);
			PlayerMgr.Inst.PlayerCtrller.TextFloatQueueAdd(1001902.GetText() + "+" + potionCfg.int1, UITextFloatType.Normal);
			break;
		case PotionAbilityType.UnstableRedPotion:
		{
			int num2 = ((UnityEngine.Random.Range(0, 2) == 0) ? potionCfg.int1 : potionCfg.int2);
			PlayerMgr.Inst.ChangeHPMax(num2);
			if (num2 > 0)
			{
				UnitDotsSyncSystem.UnitRecoveryHP(PlayerMgr.Inst.PlayerEtt, num2, ettMgr, needTextFloat: false);
			}
			break;
		}
		case PotionAbilityType.UnstableBluePotion:
		{
			int value2 = ((UnityEngine.Random.Range(0, 2) == 0) ? potionCfg.int1 : potionCfg.int2);
			PlayerMgr.Inst.ChangeMPMax(value2);
			break;
		}
		case PotionAbilityType.Purification:
		{
			for (int num9 = PlayerMgr.Inst.BaData.curseIDs.Count - 1; num9 >= 0; num9--)
			{
				CurseRemoveByIndex(num9, 0, textFloat: true, ignoreCurseDamage: true);
			}
			if (componentData.unitCfg.shieldTemp > 0f)
			{
				PlayerMgr.Inst.ChangeShieldTemp(0f - componentData.unitCfg.shieldTemp);
			}
			if (componentData.unitCfg.shield > 0f)
			{
				PlayerMgr.Inst.ChangeShield(0f - componentData.unitCfg.shield);
			}
			StartCoroutine(PotionPurificationSetHPTo1IE());
			break;
		}
		case PotionAbilityType.Fortune:
		{
			int num10 = Mathf.CeilToInt((float)PlayerMgr.Inst.CoinCount * potionCfg.float1 / 100f);
			if (num10 > 0)
			{
				ObjPoolMgr.Inst.GetGO("Prefabs/Item/Potion_Fortune", PlayerMgr.Inst.PlayerPoint, Quaternion.identity, 0f, PlayerMgr.Inst.PlayerT).GetComponent<Potion_Fortune>().Initialize(num10);
			}
			else
			{
				PlayerMgr.Inst.PlayerCtrller.TextFloatQueueAdd(1002003.GetText(), UITextFloatType.Normal);
			}
			break;
		}
		case PotionAbilityType.TearOfTheGoddess:
			if (PlayerMgr.Inst.BaData.curseIDs.Count == potionCfg.int1)
			{
				for (int num6 = PlayerMgr.Inst.BaData.curseIDs.Count - 1; num6 >= 0; num6--)
				{
					CurseRemoveByIndex(num6);
				}
			}
			else
			{
				PlayerMgr.Inst.PlayerCtrller.TextFloatQueueAdd(1002003.GetText(), UITextFloatType.Normal);
			}
			break;
		case PotionAbilityType.RelicUpgrade:
		{
			List<int> list3 = new List<int>();
			for (int num21 = 0; num21 < PlayerMgr.Inst.BaData.relicCfgs.Count; num21++)
			{
				if (PlayerMgr.Inst.BaData.relicCfgs[num21].dropType != ItemDropType.Special && PlayerMgr.Inst.BaData.relicCfgs[num21].level < PlayerMgr.Inst.BaData.relicCfgs[num21].maxCount)
				{
					list3.Add(PlayerMgr.Inst.BaData.relicCfgs[num21].id);
				}
			}
			if (list3.Count > 0)
			{
				int num22 = list3[UnityEngine.Random.Range(0, list3.Count)];
				PlayerMgr.Inst.BaData.RemoveRelicFromPool(num22);
				RelicAdd(num22);
				PlayerMgr.Inst.PlayerCtrller.TextFloatQueueAdd(1002004.GetText() + ": " + RelicConfig.dic[num22].GetName(haveLevel: false), UITextFloatType.Normal);
			}
			else
			{
				int relicFromPool = PlayerMgr.Inst.BaData.GetRelicFromPool(ItemDropType.Common);
				RelicAdd(relicFromPool);
				PlayerMgr.Inst.PlayerCtrller.TextFloatQueueAdd(1002202.GetText() + ": " + RelicConfig.dic[relicFromPool].GetName(), UITextFloatType.Normal);
			}
			break;
		}
		case PotionAbilityType.ResetRelic:
		{
			bool flag4 = false;
			for (int num16 = 0; num16 < PlayerMgr.Inst.BaData.relicCfgs.Count; num16++)
			{
				if (PlayerMgr.Inst.BaData.relicCfgs[num16].dropType != 0 && PlayerMgr.Inst.BaData.relicCfgs[num16].dropType != ItemDropType.Special)
				{
					flag4 = true;
					break;
				}
			}
			if (flag4)
			{
				GameUISingletonMono<UIRerollRelic>.ShowInit();
			}
			else
			{
				PlayerMgr.Inst.PlayerCtrller.TextFloatQueueAdd(1002010.GetText(), UITextFloatType.Normal);
			}
			break;
		}
		case PotionAbilityType.Sacrifice:
		{
			int num13 = Mathf.FloorToInt(componentData.unitCfg.currentHP * (float)potionCfg.int1 / 100f);
			if (num13 > 0)
			{
				componentData.unitCfg.currentHP -= num13;
				componentData.PlayerIntoInvisibleFrame();
				ettMgr.SetComponentData(PlayerMgr.Inst.PlayerEtt, componentData);
				UIPlayerDataMgr.Inst.UpdateHP();
				ObjPoolMgr.Inst.GetUIGO("Prefabs/UI/UITextFloat").GetComponent<UITextFloat>().Initialize("-" + num13, UITextFloatType.PlayerTakeDamage, PlayerMgr.Inst.PlayerPoint);
				ObjPoolMgr.Inst.GetGO("Prefabs/Item/Potion_Fortune", PlayerMgr.Inst.PlayerPoint, Quaternion.identity, 0f, PlayerMgr.Inst.PlayerT).GetComponent<Potion_Fortune>().Initialize(num13);
			}
			else
			{
				PlayerMgr.Inst.PlayerCtrller.TextFloatQueueAdd(1002003.GetText(), UITextFloatType.Normal);
			}
			break;
		}
		case PotionAbilityType.DoubleKey:
			if (PlayerMgr.Inst.KeyCount == 0)
			{
				PlayerMgr.Inst.ChangeKey(potionCfg.int1, TextFloatQueueType.QueueFloat);
			}
			else
			{
				PlayerMgr.Inst.ChangeKey(PlayerMgr.Inst.KeyCount, TextFloatQueueType.QueueFloat);
			}
			break;
		case PotionAbilityType.Stomachache:
			if (potion_Stomachache == null)
			{
				potion_Stomachache = UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Item/Potion_Stomachache"), PlayerMgr.Inst.PlayerPoint, Quaternion.identity, PlayerMgr.Inst.PlayerT).GetComponent<Potion_Stomachache>();
			}
			potion_Stomachache.Initialize(potionCfg);
			break;
		case PotionAbilityType.CopyResource:
		{
			bool flag2 = false;
			using (EntityQuery entityQuery2 = ettMgr.CreateEntityQuery(typeof(Item)))
			{
				NativeArray<Entity> nativeArray = entityQuery2.ToEntityArray(Allocator.Temp);
				if (nativeArray.Length > 0)
				{
					flag2 = true;
					for (int l = 0; l < nativeArray.Length; l++)
					{
						Item componentData5 = ettMgr.GetComponentData<Item>(nativeArray[l]);
						if (componentData5.belongRoomMapPos == LevelMgr.Inst.CurrentRoomMapPos && componentData5.info.type == ItemType.Resource)
						{
							switch (ResourceConfig.dic[componentData5.info.id].abilityType)
							{
							case ResourceAbilityType.Coin:
							case ResourceAbilityType.Key:
							case ResourceAbilityType.HP:
							case ResourceAbilityType.Shield:
								QuickCreateSystem.Inst.CreateItem(componentData5.belongRoomMapPos, componentData5.info, ettMgr.GetComponentData<LocalTransform>(nativeArray[l]).Position);
								break;
							default:
								Debug.LogError(ResourceConfig.dic[componentData5.info.id].abilityType);
								break;
							case ResourceAbilityType.MagicCrystal:
							case ResourceAbilityType.AcientBlood:
							case ResourceAbilityType.ChaosCore:
							case ResourceAbilityType.Gear:
								break;
							}
						}
					}
				}
				using EntityQuery entityQuery3 = ettMgr.CreateEntityQuery(typeof(Spell10201Coin));
				NativeArray<Spell10201Coin> nativeArray2 = entityQuery3.ToComponentDataArray<Spell10201Coin>(Allocator.Temp);
				NativeArray<Entity> nativeArray3 = entityQuery3.ToEntityArray(Allocator.Temp);
				if (nativeArray3.Length > 0)
				{
					flag2 = true;
					for (int m = 0; m < nativeArray3.Length; m++)
					{
						if (nativeArray2[m].belongRoomMapPos == LevelMgr.Inst.CurrentRoomMapPos)
						{
							Vector3 navMeshPointIngoreZ3 = Tool2D.GetNavMeshPointIngoreZ(ettMgr.GetComponentData<LocalTransform>(nativeArray3[m]).Position + (float3)Tool2D.GetDir() * 0.5f);
							Entity entity2 = QuickCreateSystem.Inst.CreateMixedEtt("Spell10201Coin", navMeshPointIngoreZ3);
							Spell10201Coin componentData6 = ettMgr.GetComponentData<Spell10201Coin>(entity2);
							componentData6.belongRoomMapPos = LevelMgr.Inst.CurrentRoomMapPos;
							componentData6.coinCount = nativeArray2[m].coinCount;
							ettMgr.SetComponentData(entity2, componentData6);
						}
					}
				}
				nativeArray2.Dispose();
				nativeArray3.Dispose();
				if (!flag2)
				{
					PlayerMgr.Inst.PlayerCtrller.TextFloatQueueAdd(1002003.GetText(), UITextFloatType.Normal);
				}
			}
			break;
		}
		case PotionAbilityType.AddDamage:
			PlayerMgr.Inst.BaData.extraDamageRatio += (float)potionCfg.int1 / 100f;
			PlayerMgr.Inst.PlayerCtrller.TextFloatQueueAdd(1001903.GetText() + "+" + potionCfg.int1 + "%", UITextFloatType.Normal);
			break;
		case PotionAbilityType.RandomChest:
		{
			ChestType chestType = (ChestType)UnityEngine.Random.Range(0, 4);
			int id = 401;
			switch (chestType)
			{
			case ChestType.NoLock:
				id = 404;
				break;
			case ChestType.Lock:
				id = 401;
				break;
			case ChestType.Spike:
				id = 402;
				break;
			case ChestType.Curse:
				id = 403;
				break;
			default:
				Debug.LogError(chestType);
				break;
			}
			Vector3 navMeshPointIngoreZ = Tool2D.GetNavMeshPointIngoreZ(PlayerMgr.Inst.PlayerPoint, 1.5f);
			Entity entity = QuickCreateSystem.Inst.CreateSpecialObj(id, navMeshPointIngoreZ);
			Vector3 navMeshPointIngoreZ2 = Tool2D.GetNavMeshPointIngoreZ(navMeshPointIngoreZ, 1.5f);
			if (ettMgr.HasComponent<SpecialObj4_Dots>(entity))
			{
				SpecialObj4_Dots componentData2 = ettMgr.GetComponentData<SpecialObj4_Dots>(entity);
				componentData2.SetFly(navMeshPointIngoreZ2);
				ettMgr.SetComponentData(entity, componentData2);
			}
			else if (ettMgr.HasComponent<SpecialObj4NoLock>(entity))
			{
				SpecialObj4NoLock componentData3 = ettMgr.GetComponentData<SpecialObj4NoLock>(entity);
				componentData3.SetFly(navMeshPointIngoreZ2);
				ettMgr.SetComponentData(entity, componentData3);
			}
			IRoomCtrller_Dots componentData4 = ettMgr.GetComponentData<IRoomCtrller_Dots>(entity);
			componentData4.belongRoom.Value = LevelMgr.Inst.CurrentRoomCtrller;
			componentData4.onRoomEnter = true;
			ettMgr.SetComponentData(entity, componentData4);
			break;
		}
		case PotionAbilityType.LoseHPAddTempShield:
			PlayerMgr.Inst.ChangeShieldTemp((int)(componentData.unitCfg.maxHP - componentData.unitCfg.currentHP));
			break;
		case PotionAbilityType.HPToShield:
		{
			int num7 = Mathf.FloorToInt(componentData.unitCfg.currentHP * (float)potionCfg.int1 / 100f);
			if (num7 > 0)
			{
				componentData.unitCfg.currentHP -= num7;
				componentData.SetBeHitColor();
				ettMgr.SetComponentData(PlayerMgr.Inst.PlayerEtt, componentData);
				UIPlayerDataMgr.Inst.UpdateHP();
				PlayerMgr.Inst.PlayerCtrller.TextFloatQueueAdd("-" + num7, UITextFloatType.PlayerTakeDamage);
				PlayerMgr.Inst.ChangeShield(num7);
			}
			else
			{
				PlayerMgr.Inst.PlayerCtrller.TextFloatQueueAdd(1002003.GetText(), UITextFloatType.Normal);
			}
			break;
		}
		case PotionAbilityType.AddCritical:
			PlayerMgr.Inst.BaData.extraCriticalChance += (float)potionCfg.int1 / 100f;
			PlayerMgr.Inst.PlayerCtrller.TextFloatQueueAdd(1001904.GetText() + "+" + potionCfg.int1 + "%", UITextFloatType.Normal);
			break;
		case PotionAbilityType.AddCurseAddRelic:
			UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Item/Potion_AddCurseAddRelic")).GetComponent<Potion_AddCurseAddRelic>().Initialize(potionCfg);
			break;
		case PotionAbilityType.HP1AddMaxHP:
		{
			float num4 = componentData.unitCfg.currentHP - 1f;
			if (num4 > 0f)
			{
				componentData.unitCfg.currentHP = 1f;
				componentData.PlayerIntoInvisibleFrame();
				ettMgr.SetComponentData(PlayerMgr.Inst.PlayerEtt, componentData);
				UIPlayerDataMgr.Inst.UpdateHP();
				ObjPoolMgr.Inst.GetUIGO("Prefabs/UI/UITextFloat").GetComponent<UITextFloat>().Initialize("-" + num4.ToStringDamage(), UITextFloatType.PlayerTakeDamage, PlayerMgr.Inst.PlayerPoint);
				int num5 = Mathf.CeilToInt(num4 * (float)potionCfg.int1 / 100f);
				PlayerMgr.Inst.ChangeHPMax(num5);
			}
			else
			{
				PlayerMgr.Inst.PlayerCtrller.TextFloatQueueAdd(1002003.GetText(), UITextFloatType.Normal);
			}
			break;
		}
		case PotionAbilityType.GetTwoPotions:
		{
			List<ItemInfo> list = new List<ItemInfo>();
			for (int i = 0; i < potionCfg.int1; i++)
			{
				list.Add(new ItemInfo(ItemType.Potion, DataMgr.selectedWorldData.battleData9.GetPotionFromPool()));
			}
			BlobAssetReference<BlobArray<ItemInfo>> infos = DTool.ListToBlobArray(list);
			QuickCreateSystem.Inst.CreateItemDrop(LevelMgr.Inst.CurrentRoomMapPos, infos, PlayerMgr.Inst.PlayerPointIgnoreZ, 0.1f);
			break;
		}
		case PotionAbilityType.LostMaxHpRecoverAllMp:
		{
			PlayerMgr.Inst.ChangeHPMax((float)potionCfg.int1 / 100f * (0f - PlayerMgr.Inst.PlayerPpt.unitCfg.maxHP));
			int num = Mathf.CeilToInt(PlayerMgr.Inst.PlayerPpt.unitCfg.maxHP - PlayerMgr.Inst.PlayerPpt.unitCfg.currentHP);
			if (num > 0)
			{
				PlayerMgr.Inst.PlayerPpt.HPRecovery(num, textFloat: false);
				PlayerMgr.Inst.PlayerCtrller.TextFloatQueueAdd($"+{num}", UITextFloatType.Recover);
			}
			break;
		}
		default:
			Debug.LogError(potionCfg.abilityType);
			break;
		}
		if (curseCfg_PotionReduceHP != null)
		{
			TakeDamageInfo_Dots damageInfo = TakeDamageInfo_Dots.NewInfo(PlayerMgr.Inst.PlayerEtt);
			damageInfo.damage = curseCfg_PotionReduceHP.int1.result;
			damageInfo.ignorePlayerInvincibleFrame = true;
			damageInfo.ignoreRelicDodge = true;
			damageInfo.ignoreRelicOrCurseDamageRatioChange = true;
			damageInfo.ignoreUmbrella = true;
			Entity targetEtt = PlayerMgr.Inst.PlayerEtt;
			UnitDotsSyncSystem.TryAttackEntity(in targetEtt, in damageInfo, ettMgr, checkCamp: false);
		}
		DataMgr.selectedWorldData.GalleryPotionUse(SelectedPotionID);
		DataMgr.selectedWorldData.SetFindSet10();
		SelectedPotionID = 0;
		if (relicCfg_EndlessBottle != null && UnityEngine.Random.value <= (float)relicCfg_EndlessBottle.int1.result / 100f)
		{
			int potionFromPool = PlayerMgr.Inst.BaData.GetPotionFromPool();
			QuickCreateSystem.Inst.CreateItem(LevelMgr.Inst.CurrentRoomMapPos, new ItemInfo(ItemType.Potion, potionFromPool), PlayerMgr.Inst.PlayerPoint + Tool2D.GetDir() * 0.1f);
			PlayerMgr.Inst.PlayerCtrller.TextFloatQueueAdd(1002009.GetText() + ": " + PotionConfig.dic[potionFromPool].GetName(), UITextFloatType.Normal);
		}
		if (relic_MedicineKit != null)
		{
			int result = relic_MedicineKit.cfg.int3.result;
			UnitDotsSyncSystem.UnitRecoveryHP(PlayerMgr.Inst.PlayerEtt, result, ettMgr, needTextFloat: false);
			PlayerMgr.Inst.PlayerCtrller.TextFloatQueueAdd($"+{result}", UITextFloatType.Recover);
		}
		bool flag6 = false;
		for (int num23 = SelectedPotionIndex + 1; num23 < PlayerMgr.Inst.BaData.potionMaxCount; num23++)
		{
			if (PlayerMgr.Inst.BaData.potionIDs[num23] != 0)
			{
				flag6 = true;
				PotionSelect(num23);
				break;
			}
		}
		if (!flag6)
		{
			for (int num24 = SelectedPotionIndex - 1; num24 >= 0; num24--)
			{
				if (PlayerMgr.Inst.BaData.potionIDs[num24] != 0)
				{
					flag6 = true;
					PotionSelect(num24);
					break;
				}
			}
		}
		if (!flag6)
		{
			PotionSelect(0);
		}
		if (relicCfg_PotionAddiction != null)
		{
			PlayerMgr.Inst.ChangeHPMax(relicCfg_PotionAddiction.int1.result);
			UnitDotsSyncSystem.UnitRecoveryHP(PlayerMgr.Inst.PlayerEtt, relicCfg_PotionAddiction.int1.result, ettMgr, needTextFloat: false, needCreateEF: false);
			PlayerMgr.Inst.ChangeMPMax(relicCfg_PotionAddiction.int2.result);
		}
		if (GameMgr.IsMobile_Static)
		{
			MobileMgr.inst.UpdateDrinkButton();
		}
	}

	public int CaculatePotionStorage()
	{
		int num = 1 + DataMgr.selectedWorldData.GetResearchValueConsiderActive(ResearchAbilityType.PotionLimit);
		if (relic_MedicineKit != null)
		{
			num += relic_MedicineKit.cfg.int1.result;
		}
		if (relicCfg_ExtraPotionStorage != null)
		{
			num += relicCfg_ExtraPotionStorage.int1.value * relicCfg_ExtraPotionStorage.level;
		}
		return num;
	}

	public void PotionRemove(int index)
	{
		if (PlayerMgr.Inst.BaData.potionIDs.Count == 0)
		{
			Debug.LogError("没有药水");
			return;
		}
		if (index > PlayerMgr.Inst.BaData.potionIDs.Count)
		{
			Debug.LogError("超出下标");
			return;
		}
		if (PlayerMgr.Inst.BaData.potionIDs[index] == 0)
		{
			Debug.LogError("该下标没有药水: " + index);
			return;
		}
		PlayerMgr.Inst.BaData.potionIDs[index] = 0;
		if (GameMgr.IsMobile_Static)
		{
			MobileMgr.inst.UpdateDrinkButton();
		}
		if (SelectedPotionIndex == index)
		{
			bool flag = false;
			for (int i = SelectedPotionIndex + 1; i < PlayerMgr.Inst.BaData.potionMaxCount; i++)
			{
				if (PlayerMgr.Inst.BaData.potionIDs[i] != 0)
				{
					flag = true;
					PotionSelect(i);
					break;
				}
			}
			if (!flag)
			{
				for (int num = SelectedPotionIndex - 1; num >= 0; num--)
				{
					if (PlayerMgr.Inst.BaData.potionIDs[num] != 0)
					{
						flag = true;
						PotionSelect(num);
						break;
					}
				}
			}
			if (!flag)
			{
				PotionSelect(0);
			}
		}
		UIPlayerDataMgr.Inst.uiPotionsCtrller.CheckCountAndUpdateAllUI();
	}

	public void PotionChangeSlotDelay()
	{
		StartCoroutine(PotionChangeSlotDelayIE());
	}

	private IEnumerator PotionChangeSlotDelayIE()
	{
		yield return null;
		PotionChangeSlot(0);
	}

	public void PotionChangeSlot(int count)
	{
		PlayerMgr.Inst.BaData.potionMaxCount += count;
		if (PlayerMgr.Inst.BaData.potionMaxCount < 0)
		{
			PlayerMgr.Inst.BaData.potionMaxCount = 0;
			Debug.LogError("药水栏位数量小于0");
		}
		if (PlayerMgr.Inst.BaData.potionIDs.Count < PlayerMgr.Inst.BaData.potionMaxCount)
		{
			for (int i = PlayerMgr.Inst.BaData.potionIDs.Count; i < PlayerMgr.Inst.BaData.potionMaxCount; i++)
			{
				PlayerMgr.Inst.BaData.potionIDs.Add(0);
			}
		}
		else if (PlayerMgr.Inst.BaData.potionIDs.Count > PlayerMgr.Inst.BaData.potionMaxCount)
		{
			for (int num = PlayerMgr.Inst.BaData.potionIDs.Count - 1; num >= PlayerMgr.Inst.BaData.potionMaxCount; num--)
			{
				if (PlayerMgr.Inst.BaData.potionIDs[num] != 0)
				{
					Vector3 vector = PlayerMgr.Inst.PlayerPoint + Tool2D.GetDir() * 0.02f;
					QuickCreateSystem.Inst.CreateItem(LevelMgr.Inst.CurrentRoomMapPos, new ItemInfo(ItemType.Potion, PlayerMgr.Inst.BaData.potionIDs[num]), vector);
				}
				PlayerMgr.Inst.BaData.potionIDs.RemoveAt(num);
			}
		}
		UIPlayerDataMgr.Inst.uiPotionsCtrller.CheckCountAndUpdateAllUI();
		if (GameMgr.IsMobile_Static)
		{
			MobileMgr.inst.UpdateDrinkButton();
		}
	}

	public void PotionChange(int potionIndex, int newID)
	{
		PlayerMgr.Inst.BaData.potionIDs[potionIndex] = newID;
		if (potionIndex == SelectedPotionIndex && newID == 0)
		{
			bool flag = false;
			for (int i = SelectedPotionIndex + 1; i < PlayerMgr.Inst.BaData.potionMaxCount; i++)
			{
				if (PlayerMgr.Inst.BaData.potionIDs[i] != 0)
				{
					flag = true;
					PotionSelect(i);
					break;
				}
			}
			if (!flag)
			{
				for (int num = SelectedPotionIndex - 1; num >= 0; num--)
				{
					if (PlayerMgr.Inst.BaData.potionIDs[num] != 0)
					{
						flag = true;
						PotionSelect(num);
						break;
					}
				}
			}
			if (!flag)
			{
				PotionSelect(0);
			}
		}
		UIPlayerDataMgr.Inst.uiPotionsCtrller.UpdateAllUI();
		if (GameMgr.IsMobile_Static)
		{
			MobileMgr.inst.UpdateDrinkButton();
		}
	}

	public void PotionChangeSelect(bool nextOne)
	{
		if (PlayerMgr.Inst.BaData.potionMaxCount == 0)
		{
			return;
		}
		if (nextOne)
		{
			if (SelectedPotionIndex == PlayerMgr.Inst.BaData.potionMaxCount - 1)
			{
				for (int i = 0; i < PlayerMgr.Inst.BaData.potionMaxCount; i++)
				{
					if (PlayerMgr.Inst.BaData.potionIDs[i] != 0)
					{
						PotionSelect(i);
						break;
					}
				}
				return;
			}
			for (int j = SelectedPotionIndex + 1; j < PlayerMgr.Inst.BaData.potionMaxCount; j++)
			{
				if (PlayerMgr.Inst.BaData.potionIDs[j] != 0)
				{
					PotionSelect(j);
					return;
				}
			}
			for (int k = 0; k < PlayerMgr.Inst.BaData.potionMaxCount; k++)
			{
				if (PlayerMgr.Inst.BaData.potionIDs[k] != 0)
				{
					PotionSelect(k);
					break;
				}
			}
			return;
		}
		if (SelectedPotionIndex == 0)
		{
			for (int num = PlayerMgr.Inst.BaData.potionMaxCount - 1; num >= 0; num--)
			{
				if (PlayerMgr.Inst.BaData.potionIDs[num] != 0)
				{
					PotionSelect(num);
					break;
				}
			}
			return;
		}
		for (int num2 = SelectedPotionIndex - 1; num2 >= 0; num2--)
		{
			if (PlayerMgr.Inst.BaData.potionIDs[num2] != 0)
			{
				PotionSelect(num2);
				return;
			}
		}
		for (int num3 = PlayerMgr.Inst.BaData.potionMaxCount - 1; num3 > SelectedPotionIndex - 1; num3--)
		{
			if (PlayerMgr.Inst.BaData.potionIDs[num3] != 0)
			{
				PotionSelect(num3);
				break;
			}
		}
	}

	public void PotionSelect(int index, bool showInfoPanel = true)
	{
		SelectedPotionIndex = index;
		UIPlayerDataMgr.Inst.uiPotionsCtrller.UpdateAllUI();
	}

	public void PotionSelectFirst()
	{
		for (int i = 0; i < PlayerMgr.Inst.BaData.potionMaxCount; i++)
		{
			if (i < PlayerMgr.Inst.BaData.potionIDs.Count && PlayerMgr.Inst.BaData.potionIDs[i] != 0)
			{
				PotionSelect(i);
				return;
			}
		}
		PotionSelect(0);
	}

	private IEnumerator PotionPurificationSetHPTo1IE()
	{
		yield return null;
		if (PlayerMgr.Inst.TryGetPlayerPpt(out var playerPpt) && playerPpt.unitCfg.currentHP > 1f)
		{
			playerPpt = ettMgr.GetComponentData<UnitProperty_Dots>(PlayerMgr.Inst.PlayerEtt);
			playerPpt.unitCfg.currentHP = 1f;
			ettMgr.SetComponentData(PlayerMgr.Inst.PlayerEtt, playerPpt);
			UIPlayerDataMgr.Inst.UpdateHP();
		}
	}

	public void CurseAdd(int id, bool textFloat, bool addGallery = true)
	{
		bool flag = PlayerMgr.Inst.BaData.curseIDs.Contains(id);
		if (flag)
		{
			PlayerMgr.Inst.BaData.curseLevels[PlayerMgr.Inst.BaData.curseIDs.IndexOf(id)]++;
		}
		else
		{
			PlayerMgr.Inst.BaData.curseIDs.Add(id);
			PlayerMgr.Inst.BaData.curseLevels.Add(1);
		}
		CurseConfig config = CurseConfig.GetConfig(id);
		config.level = PlayerMgr.Inst.BaData.curseLevels[PlayerMgr.Inst.BaData.curseIDs.IndexOf(id)];
		config.CalculateAbility();
		switch (config.abilityType)
		{
		case CurseAbilityType.ResourceDisappear:
			curseCfg_PastDueResource = config;
			break;
		case CurseAbilityType.InvisibleDoor:
			curse_IsInvisibleDoor = true;
			LevelMgr.Inst.AllRoomAllDoorUpdateDisplay();
			break;
		case CurseAbilityType.TargetedTrap:
			curseCfg_TargetedTrap = config;
			break;
		case CurseAbilityType.EnemyAddMove:
			curseCfg_EnemyAddMove = config;
			break;
		case CurseAbilityType.RevengeGhost:
			curseCfg_RevengeGhost = config;
			break;
		case CurseAbilityType.InjuredLoseCoin:
			curseCfg_InjuredLoseCoin = config;
			break;
		case CurseAbilityType.InjuredRandomPoint:
			curse_IsInjuredRandomPoint = true;
			break;
		case CurseAbilityType.InjuredLoseMaxHP:
			curseCfg_InjuredLoseMaxHP = config;
			break;
		case CurseAbilityType.InjuredCantShoot:
			if (curse_InjuredCantShoot == null)
			{
				curse_InjuredCantShoot = UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Item/Curse_InjuredCantShoot"), LevelMgr.Inst.tsf_PlayerThings).GetComponent<Curse_InjuredCantShoot>();
			}
			curse_InjuredCantShoot.Initialize(config);
			break;
		case CurseAbilityType.DiamondToCion:
			curse_IsDiamondToCion = true;
			break;
		case CurseAbilityType.ReduceMoveSpeed:
			curseCfg_ReduceMoveSpeed = config;
			break;
		case CurseAbilityType.CantShootEnterRoom:
			if (curse_CantShootEnterRoom == null)
			{
				curse_CantShootEnterRoom = UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Item/Curse_CantShootEnterRoom"), LevelMgr.Inst.tsf_PlayerThings).GetComponent<Curse_CantShootEnterRoom>();
			}
			curse_CantShootEnterRoom.Initialize(config);
			break;
		case CurseAbilityType.ReduceSpellSpeed:
			curseCfg_ReduceSpellSpeed = config;
			break;
		case CurseAbilityType.ReduceSpeedDamage:
			curseCfg_ReduceSpeedDamage = config;
			break;
		case CurseAbilityType.SlowWand:
			curseCfg_SlowWand = config;
			break;
		case CurseAbilityType.Bled:
			curseCfg_Bled = config;
			break;
		case CurseAbilityType.DoubleLock:
			curse_IsDoubleLock = true;
			break;
		case CurseAbilityType.EnterDoorLoseCoin:
			curseCfg_EnterDoorLoseCoin = config;
			break;
		case CurseAbilityType.DoubleEnemy:
			curseCfg_DoubleEnemy = config;
			break;
		case CurseAbilityType.GetCoinLoseHP:
			curseCfg_GetCoinLoseHP = config;
			break;
		case CurseAbilityType.ReverseKnoackback:
			curse_IsReverseKnockback = true;
			break;
		case CurseAbilityType.DarkView:
			if (curse_DarkView == null)
			{
				curse_DarkView = UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Item/Curse_DarkView"), LevelMgr.Inst.tsf_PlayerThings).GetComponent<Curse_DarkView>();
			}
			break;
		case CurseAbilityType.LoseMPRecovery:
			curseCfg_LoseMPRecovery = config;
			break;
		case CurseAbilityType.LoseMPLimit:
			curseCfg_LoseMPLimit = config;
			break;
		case CurseAbilityType.CostCorrection:
		{
			curseCfg_CostCorrection = config;
			for (int k = 0; k < PlayerMgr.Inst.Wands.Count; k++)
			{
				Wand wand = PlayerMgr.Inst.Wands[k];
				if ((object)wand != null)
				{
					wand.ResetAndRecheck();
					UIPlayerDataMgr.Inst.WandUpdate(k);
				}
			}
			break;
		}
		case CurseAbilityType.AddRecoil:
			curseCfg_AddRecoil = config;
			break;
		case CurseAbilityType.RelicReduce:
			curseCfg_RelicReduce = config;
			break;
		case CurseAbilityType.MonsterRecover:
			curseCfg_MonsterRecover = config;
			break;
		case CurseAbilityType.CantSeeResource:
			UIPlayerDataMgr.Inst.HideResource();
			break;
		case CurseAbilityType.RandomCurseCommon:
			if (curse_RandomCurseCommon == null)
			{
				curse_RandomCurseCommon = UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Item/Curse_RandomCurse"), LevelMgr.Inst.tsf_PlayerThings).GetComponent<Curse_RandomCurse>();
			}
			curse_RandomCurseCommon.Initialize(config);
			break;
		case CurseAbilityType.RandomCurseRare:
			if (curse_RandomCurseRare == null)
			{
				curse_RandomCurseRare = UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Item/Curse_RandomCurse"), LevelMgr.Inst.tsf_PlayerThings).GetComponent<Curse_RandomCurse>();
			}
			curse_RandomCurseRare.Initialize(config);
			break;
		case CurseAbilityType.Illiteracy:
			curse_IsIlliteracy = true;
			EventMgr.LanguageChange?.Invoke();
			break;
		case CurseAbilityType.ReverseMove:
			curse_IsReverseMove = true;
			break;
		case CurseAbilityType.Shackle:
			if (curse_Shackle == null)
			{
				curse_Shackle = UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Item/Curse_Shackle"), LevelMgr.Inst.tsf_PlayerThings).GetComponent<Curse_Shackle>();
			}
			break;
		case CurseAbilityType.BagReduce:
		{
			int num2 = 0;
			num2 = ((!flag) ? config.int1.value : config.int1.valueUpgrade);
			if (PlayerMgr.Inst.BaData.bagCount < num2)
			{
				num2 = PlayerMgr.Inst.BaData.bagCount;
			}
			for (int j = 0; j < num2; j++)
			{
				int index3 = PlayerMgr.Inst.BaData.bagCount - 1 - j;
				SlotData slotData = PlayerMgr.Inst.BaData.bagSpellDatas[index3];
				if (slotData != null)
				{
					if (slotData.isSealSlot)
					{
						index3 = PlayerMgr.Inst.BaData.bagSpellDatas.ToArray().Bag_GetOwnerSlotIndex(index3);
						slotData = PlayerMgr.Inst.BaData.bagSpellDatas[index3];
					}
					PlayerMgr.Inst.Slot_RemoveBagSlot(index3);
					PlayerMgr.Inst.SpawnSpellToGround(slotData);
				}
			}
			PlayerMgr.Inst.BaData.bagSpellDatas.RemoveRange(PlayerMgr.Inst.BaData.bagSpellDatas.Count - num2, num2);
			PlayerMgr.Inst.BaData.bagCount -= num2;
			UIPlayerDataMgr.Inst.UpdateBag();
			UIPlayerDataMgr.Inst.BagCheckRelicPandorasBoxImage();
			break;
		}
		case CurseAbilityType.PotionReduceHP:
			curseCfg_PotionReduceHP = config;
			break;
		case CurseAbilityType.IntervalBomb:
			curseCfg_RandomBomb = config;
			curseCfg_RandomBomb.float1.result = UnityEngine.Random.Range(curseCfg_RandomBomb.float1.value, curseCfg_RandomBomb.float1.valueUpgrade);
			break;
		case CurseAbilityType.RandomRemoveRelic:
		{
			List<int> list = new List<int>();
			for (int i = 0; i < PlayerMgr.Inst.BaData.relicCfgs.Count; i++)
			{
				if (PlayerMgr.Inst.BaData.relicCfgs[i].dropType != 0 && PlayerMgr.Inst.BaData.relicCfgs[i].dropType != ItemDropType.Special)
				{
					list.Add(PlayerMgr.Inst.BaData.relicCfgs[i].id);
				}
			}
			if (list.Count > 0)
			{
				int num = list[UnityEngine.Random.Range(0, list.Count)];
				RelicRemove(num, 1);
				textFloat = false;
				PlayerMgr.Inst.PlayerCtrller.TextFloatQueueAdd(1002042.GetText() + ": " + RelicConfig.dic[num].GetName(), UITextFloatType.Normal);
			}
			int index2 = PlayerMgr.Inst.BaData.curseIDs.IndexOf(id);
			PlayerMgr.Inst.BaData.curseLevels.RemoveAt(index2);
			PlayerMgr.Inst.BaData.curseIDs.RemoveAt(index2);
			break;
		}
		case CurseAbilityType.ReduceMaxHP:
			if (config.level == 1)
			{
				PlayerMgr.Inst.ChangeHPMax(-config.int1.value);
			}
			else
			{
				PlayerMgr.Inst.ChangeHPMax(-config.int1.valueUpgrade);
			}
			break;
		case CurseAbilityType.DeathBet:
			curseCfg_DeathBet = config;
			break;
		case CurseAbilityType.Recall:
			if (curse_Recall == null)
			{
				curse_Recall = UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Item/Curse_Recall"), LevelMgr.Inst.tsf_PlayerThings).GetComponent<Curse_Recall>();
			}
			curse_Recall.Initialize(config);
			break;
		case CurseAbilityType.EnterDoorNoMP:
			curseCfg_EnterDoorNoMP = config;
			break;
		case CurseAbilityType.ReduceCriticalRatio:
			curseCfg_ReduceCriticalRatio = config;
			break;
		case CurseAbilityType.NoCargo:
			curseCfg_NoCargo = config;
			break;
		case CurseAbilityType.ScatterAdd:
			curseCfg_ScatterAdd = config;
			break;
		case CurseAbilityType.SummonsReduce:
			curseCfg_SummonsReduce = config;
			break;
		case CurseAbilityType.SnailHunt:
			if (curse_SnailHunt == null)
			{
				curse_SnailHunt = UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Item/Curse_SnailHunt"), LevelMgr.Inst.tsf_PlayerThings).GetComponent<Curse_SnailHunt>();
			}
			break;
		case CurseAbilityType.OldWound:
			curseCfg_OldWound = config;
			break;
		case CurseAbilityType.ReverseRecoil:
			curse_IsReverseRecoil = true;
			break;
		case CurseAbilityType.Vulnerability:
			curseCfg_Vulnerability = config;
			break;
		case CurseAbilityType.ReduceSpellRadius:
			curseCfg_ReduceSpellRadius = config;
			break;
		case CurseAbilityType.MoreMoneyMoreInjured:
			curseCfg_MoreMoneyMoreInjured = config;
			break;
		case CurseAbilityType.ReverseShoot:
			curse_IsReverseShoot = true;
			break;
		case CurseAbilityType.EnemyReduceDamage:
			curseCfg_EnemyReduceDamage = config;
			break;
		case CurseAbilityType.ShootSlow:
			curseCfg_ShootSlow = config;
			break;
		case CurseAbilityType.ZeroFriction:
			curseCfg_ZeroFriction = config;
			break;
		case CurseAbilityType.IsaacVirus:
			curse_IsIsaacVirus = true;
			break;
		case CurseAbilityType.Stealthy:
			if (curse_Stealthy == null)
			{
				curse_Stealthy = UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Item/Curse_Stealthy"), LevelMgr.Inst.tsf_PlayerThings).GetComponent<Curse_Stealthy>();
			}
			break;
		case CurseAbilityType.ChestMonster:
			curseCfg_ChestMonster = config;
			break;
		case CurseAbilityType.FullHPAddDamage:
			curseCfg_FullHPAddDamage = config;
			break;
		case CurseAbilityType.LoseAllKey:
		{
			PlayerMgr.Inst.ChangeKey(-PlayerMgr.Inst.KeyCount, TextFloatQueueType.DirectFloat);
			int index = PlayerMgr.Inst.BaData.curseIDs.IndexOf(id);
			PlayerMgr.Inst.BaData.curseLevels.RemoveAt(index);
			PlayerMgr.Inst.BaData.curseIDs.RemoveAt(index);
			break;
		}
		case CurseAbilityType.Pestilence:
			curseCfg_Pestilence = config;
			break;
		case CurseAbilityType.LostSpellOption:
			curseCfg_LostSpellOption = config;
			break;
		case CurseAbilityType.InvalidCurse:
			curseCfg_InvalidCurse = config;
			break;
		default:
			Debug.LogError(config.abilityType);
			break;
		}
		UIPlayerDataMgr.Inst.CurseUpdate();
		DataMgr.selectedWorldData.GalleryUnlock(GalleryCategory.Curse, id);
		if (addGallery)
		{
			DataMgr.selectedWorldData.GalleryCurseGet(id);
		}
		if (PlayerMgr.Inst.BaData.curseIDs.Count >= 15)
		{
			SteamAchievementMgr.UnlockAndUpload(SteamAchievementType.Get15Curse);
		}
		if (textFloat)
		{
			ObjPoolMgr.Inst.GetUIGO("Prefabs/UI/UITextFloat").GetComponent<UITextFloat>().Initialize(1002210.GetText() + ": " + CurseConfig.dic[id].GetName(), UITextFloatType.Normal, PlayerMgr.Inst.PlayerPoint);
		}
	}

	public void CurseAdd(int id, Vector3 worldPoint, bool addGallery = true)
	{
		if (GameMgr.IsMobile_Static)
		{
			Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(null, MobileMgr.inst.topui.goMenuButton.GetComponent<RectTransform>().position);
			RectTransformUtility.ScreenPointToLocalPointInRectangle(UIMgr.Inst.canvas_10.GetComponent<RectTransform>(), screenPoint, null, out var localPoint);
			PlayerMgr.Inst.ItemCtrller.UIRewardFly(UIMgr.Inst.canvas_1, id, RollRewardFly.DropType.Curse, worldPoint, localPoint + new Vector2(0f, -10f), null, useParticleColor: true, delegate
			{
				UIPlayerDataMgr.Inst.MenuShakeButton();
				PlayerMgr.Inst.ItemCtrller.CurseAdd(id, addGallery);
				SEMgr.Inst.uiCurseFlyFinish.PlaySE();
			}, isUI: true, dropOnEnd: true);
		}
		else
		{
			Vector3 moveToPoint = UIMgr.Inst.canvas_1Scaler.transform.InverseTransformPoint(UIPlayerDataMgr.Inst.uiLayout_Curse.transform.position);
			moveToPoint += new Vector3((float)PlayerMgr.Inst.BaData.curseIDs.Count * UIPlayerDataMgr.Inst.uiLayout_Curse.childSize.x + UIPlayerDataMgr.Inst.uiLayout_Curse.childSize.x / 2f, (0f - UIPlayerDataMgr.Inst.uiLayout_Curse.childSize.y) / 2f, 0f);
			string path = (GameMgr.IsHarmony_Static ? "Prefabs/UI/UICurseFlyH" : "Prefabs/UI/UICurseFly");
			UICurseFly component = ObjPoolMgr.Inst.GetUIGO(path).GetComponent<UICurseFly>();
			component.rtsf_Self.anchoredPosition = GeneralTool.WorldToCanvasLocalPoint(worldPoint);
			component.Initialize(id, moveToPoint, addGallery);
		}
	}

	public void CurseRemoveByIndex(int curseIndex, int removeLevel = 0, bool textFloat = true, bool ignoreCurseDamage = false)
	{
		CurseConfig config = CurseConfig.GetConfig(PlayerMgr.Inst.BaData.curseIDs[curseIndex]);
		bool flag = true;
		if (removeLevel > PlayerMgr.Inst.BaData.curseLevels[curseIndex])
		{
			Debug.LogError("要移除的诅咒等级不应该比自身已有的诅咒等级高");
			return;
		}
		if (removeLevel == PlayerMgr.Inst.BaData.curseLevels[curseIndex] || removeLevel == 0)
		{
			flag = false;
			removeLevel = PlayerMgr.Inst.BaData.curseLevels[curseIndex];
			Vector3 position = UIMgr.Inst.canvas_10Scaler.transform.InverseTransformPoint(UIPlayerDataMgr.Inst.uiLayout_Curse.transform.position);
			position += new Vector3((float)curseIndex * UIPlayerDataMgr.Inst.uiLayout_Curse.childSize.x + UIPlayerDataMgr.Inst.uiLayout_Curse.childSize.x / 2f, (0f - UIPlayerDataMgr.Inst.uiLayout_Curse.childSize.y) / 2f, 0f);
			position = UIMgr.Inst.canvas_10Scaler.transform.TransformPoint(position);
			ObjPoolMgr.Inst.GetUIGO("Prefabs/UI/UICurseRemove", position);
			if (textFloat)
			{
				PlayerMgr.Inst.PlayerCtrller.TextFloatQueueAdd(1002007.GetText() + ": " + config.GetName(), UITextFloatType.Normal);
			}
			PlayerMgr.Inst.BaData.curseIDs.RemoveAt(curseIndex);
			PlayerMgr.Inst.BaData.curseLevels.RemoveAt(curseIndex);
		}
		else
		{
			flag = true;
			config.level = PlayerMgr.Inst.BaData.curseLevels[curseIndex] - removeLevel;
			config.CalculateAbility();
			PlayerMgr.Inst.BaData.curseLevels[curseIndex] -= removeLevel;
		}
		UIPlayerDataMgr.Inst.CurseUpdate();
		switch (config.abilityType)
		{
		case CurseAbilityType.ResourceDisappear:
			if (flag)
			{
				curseCfg_PastDueResource = config;
			}
			else
			{
				curseCfg_PastDueResource = null;
			}
			break;
		case CurseAbilityType.InvisibleDoor:
			if (!flag)
			{
				curse_IsInvisibleDoor = false;
				LevelMgr.Inst.AllRoomAllDoorUpdateDisplay();
			}
			break;
		case CurseAbilityType.TargetedTrap:
			if (flag)
			{
				curseCfg_TargetedTrap = config;
			}
			else
			{
				curseCfg_TargetedTrap = null;
			}
			break;
		case CurseAbilityType.EnemyAddMove:
			if (flag)
			{
				curseCfg_EnemyAddMove = config;
			}
			else
			{
				curseCfg_EnemyAddMove = null;
			}
			break;
		case CurseAbilityType.RevengeGhost:
			if (flag)
			{
				curseCfg_RevengeGhost = config;
			}
			else
			{
				curseCfg_RevengeGhost = null;
			}
			break;
		case CurseAbilityType.InjuredLoseCoin:
			if (flag)
			{
				curseCfg_InjuredLoseCoin = config;
			}
			else
			{
				curseCfg_InjuredLoseCoin = null;
			}
			break;
		case CurseAbilityType.InjuredRandomPoint:
			if (!flag)
			{
				curse_IsInjuredRandomPoint = false;
			}
			break;
		case CurseAbilityType.InjuredLoseMaxHP:
			if (flag)
			{
				curseCfg_InjuredLoseMaxHP = config;
			}
			else
			{
				curseCfg_InjuredLoseMaxHP = null;
			}
			break;
		case CurseAbilityType.InjuredCantShoot:
			if (flag)
			{
				curse_InjuredCantShoot.Initialize(config);
				break;
			}
			UnityEngine.Object.Destroy(curse_InjuredCantShoot.gameObject);
			curse_InjuredCantShoot = null;
			break;
		case CurseAbilityType.DiamondToCion:
			if (!flag)
			{
				curse_IsDiamondToCion = false;
			}
			break;
		case CurseAbilityType.ReduceMoveSpeed:
			if (flag)
			{
				curseCfg_ReduceMoveSpeed = config;
			}
			else
			{
				curseCfg_ReduceMoveSpeed = null;
			}
			break;
		case CurseAbilityType.CantShootEnterRoom:
			if (flag)
			{
				curse_CantShootEnterRoom.Initialize(config);
				break;
			}
			UnityEngine.Object.Destroy(curse_CantShootEnterRoom.gameObject);
			curse_CantShootEnterRoom = null;
			break;
		case CurseAbilityType.ReduceSpellSpeed:
			if (flag)
			{
				curseCfg_ReduceSpellSpeed = config;
			}
			else
			{
				curseCfg_ReduceSpellSpeed = null;
			}
			break;
		case CurseAbilityType.ReduceSpeedDamage:
			if (flag)
			{
				curseCfg_ReduceSpeedDamage = config;
			}
			else
			{
				curseCfg_ReduceSpeedDamage = null;
			}
			break;
		case CurseAbilityType.SlowWand:
			if (flag)
			{
				curseCfg_SlowWand = config;
			}
			else
			{
				curseCfg_SlowWand = null;
			}
			break;
		case CurseAbilityType.Bled:
			if (flag)
			{
				curseCfg_Bled = config;
			}
			else
			{
				curseCfg_Bled = null;
			}
			break;
		case CurseAbilityType.DoubleLock:
			if (!flag)
			{
				curse_IsDoubleLock = false;
			}
			break;
		case CurseAbilityType.EnterDoorLoseCoin:
			if (flag)
			{
				curseCfg_EnterDoorLoseCoin = config;
			}
			else
			{
				curseCfg_EnterDoorLoseCoin = null;
			}
			break;
		case CurseAbilityType.DoubleEnemy:
			if (flag)
			{
				curseCfg_DoubleEnemy = config;
			}
			else
			{
				curseCfg_DoubleEnemy = null;
			}
			break;
		case CurseAbilityType.GetCoinLoseHP:
			if (flag)
			{
				curseCfg_GetCoinLoseHP = config;
			}
			else
			{
				curseCfg_GetCoinLoseHP = null;
			}
			break;
		case CurseAbilityType.ReverseKnoackback:
			if (!flag)
			{
				curse_IsReverseKnockback = false;
			}
			break;
		case CurseAbilityType.DarkView:
			if (!flag)
			{
				curse_DarkView.DestroySelf();
				curse_DarkView = null;
			}
			break;
		case CurseAbilityType.LoseMPRecovery:
			if (flag)
			{
				curseCfg_LoseMPRecovery = config;
			}
			else
			{
				curseCfg_LoseMPRecovery = null;
			}
			break;
		case CurseAbilityType.LoseMPLimit:
			if (flag)
			{
				curseCfg_LoseMPLimit = config;
			}
			else
			{
				curseCfg_LoseMPLimit = null;
			}
			break;
		case CurseAbilityType.CostCorrection:
		{
			if (flag)
			{
				curseCfg_CostCorrection = config;
			}
			else
			{
				curseCfg_CostCorrection = null;
			}
			for (int i = 0; i < PlayerMgr.Inst.Wands.Count; i++)
			{
				Wand wand = PlayerMgr.Inst.Wands[i];
				if ((object)wand != null)
				{
					wand.ResetAndRecheck();
					UIPlayerDataMgr.Inst.WandUpdate(i);
				}
			}
			break;
		}
		case CurseAbilityType.AddRecoil:
			if (flag)
			{
				curseCfg_AddRecoil = config;
			}
			else
			{
				curseCfg_AddRecoil = null;
			}
			break;
		case CurseAbilityType.RelicReduce:
			if (flag)
			{
				curseCfg_RelicReduce = config;
			}
			else
			{
				curseCfg_RelicReduce = null;
			}
			break;
		case CurseAbilityType.MonsterRecover:
			if (flag)
			{
				curseCfg_MonsterRecover = config;
			}
			else
			{
				curseCfg_MonsterRecover = null;
			}
			break;
		case CurseAbilityType.CantSeeResource:
			if (!flag)
			{
				UIPlayerDataMgr.Inst.ShowResource();
			}
			break;
		case CurseAbilityType.Illiteracy:
			if (!flag)
			{
				curse_IsIlliteracy = false;
				EventMgr.LanguageChange?.Invoke();
			}
			break;
		case CurseAbilityType.ReverseMove:
			if (!flag)
			{
				curse_IsReverseMove = false;
			}
			break;
		case CurseAbilityType.Shackle:
			if (!flag)
			{
				UnityEngine.Object.Destroy(curse_Shackle.gameObject);
				curse_Shackle = null;
			}
			break;
		case CurseAbilityType.BagReduce:
		{
			CurseConfig config3 = CurseConfig.GetConfig(35);
			config3.level = removeLevel;
			config3.CalculateAbility();
			for (int j = 0; j < config3.int1.result; j++)
			{
				PlayerMgr.Inst.BaData.bagCount++;
				PlayerMgr.Inst.BaData.bagSpellDatas.Add(null);
			}
			UIPlayerDataMgr.Inst.UpdateBag();
			UIPlayerDataMgr.Inst.BagCheckRelicPandorasBoxImage();
			break;
		}
		case CurseAbilityType.PotionReduceHP:
			if (flag)
			{
				curseCfg_PotionReduceHP = config;
			}
			else
			{
				curseCfg_PotionReduceHP = null;
			}
			break;
		case CurseAbilityType.IntervalBomb:
			if (flag)
			{
				curseCfg_RandomBomb = config;
			}
			else
			{
				curseCfg_RandomBomb = null;
			}
			break;
		case CurseAbilityType.RandomRemoveRelic:
			Debug.LogError("这个诅咒根本就不会存在");
			break;
		case CurseAbilityType.ReduceMaxHP:
		{
			CurseConfig config2 = CurseConfig.GetConfig(39);
			if (flag)
			{
				PlayerMgr.Inst.ChangeHPMax(config2.int1.valueUpgrade * removeLevel);
				break;
			}
			config2.level = removeLevel;
			config2.CalculateAbility();
			PlayerMgr.Inst.ChangeHPMax(config2.int1.result);
			break;
		}
		case CurseAbilityType.DeathBet:
			if (flag)
			{
				curseCfg_DeathBet = config;
			}
			else
			{
				curseCfg_DeathBet = null;
			}
			break;
		case CurseAbilityType.Recall:
			if (flag)
			{
				curse_Recall.Initialize(config);
				break;
			}
			curse_Recall.DestroySelf();
			curse_Recall = null;
			break;
		case CurseAbilityType.EnterDoorNoMP:
			if (flag)
			{
				curseCfg_EnterDoorNoMP = config;
			}
			else
			{
				curseCfg_EnterDoorNoMP = null;
			}
			break;
		case CurseAbilityType.ReduceCriticalRatio:
			if (flag)
			{
				curseCfg_ReduceCriticalRatio = config;
			}
			else
			{
				curseCfg_ReduceCriticalRatio = null;
			}
			break;
		case CurseAbilityType.NoCargo:
			if (flag)
			{
				curseCfg_NoCargo = config;
			}
			else
			{
				curseCfg_NoCargo = null;
			}
			break;
		case CurseAbilityType.ScatterAdd:
			if (flag)
			{
				curseCfg_ScatterAdd = config;
			}
			else
			{
				curseCfg_ScatterAdd = null;
			}
			break;
		case CurseAbilityType.SummonsReduce:
			if (flag)
			{
				curseCfg_SummonsReduce = config;
			}
			else
			{
				curseCfg_SummonsReduce = null;
			}
			break;
		case CurseAbilityType.SnailHunt:
			if (!flag)
			{
				curse_SnailHunt.DestroySelf();
				curse_SnailHunt = null;
			}
			break;
		case CurseAbilityType.OldWound:
		{
			if (flag)
			{
				curseCfg_OldWound = config;
				if (!ignoreCurseDamage)
				{
					TakeDamageInfo_Dots damageInfo = TakeDamageInfo_Dots.NewInfo(AttackerType.NothingSpecial);
					damageInfo.damage = curseCfg_OldWound.int1.valueUpgrade * removeLevel;
					Entity targetEtt = PlayerMgr.Inst.PlayerEtt;
					UnitDotsSyncSystem.TryAttackEntity(in targetEtt, in damageInfo, ettMgr);
				}
				break;
			}
			curseCfg_OldWound = null;
			CurseConfig config4 = CurseConfig.GetConfig(48);
			config4.level = removeLevel;
			config4.CalculateAbility();
			if (!ignoreCurseDamage)
			{
				TakeDamageInfo_Dots damageInfo2 = TakeDamageInfo_Dots.NewInfo(AttackerType.NothingSpecial);
				damageInfo2.damage = config4.int1.result;
				Entity targetEtt = PlayerMgr.Inst.PlayerEtt;
				UnitDotsSyncSystem.TryAttackEntity(in targetEtt, in damageInfo2, ettMgr);
			}
			break;
		}
		case CurseAbilityType.ReverseRecoil:
			if (!flag)
			{
				curse_IsReverseRecoil = false;
			}
			break;
		case CurseAbilityType.Vulnerability:
			if (flag)
			{
				curseCfg_Vulnerability = config;
			}
			else
			{
				curseCfg_Vulnerability = null;
			}
			break;
		case CurseAbilityType.ReduceSpellRadius:
			if (flag)
			{
				curseCfg_ReduceSpellRadius = config;
			}
			else
			{
				curseCfg_ReduceSpellRadius = null;
			}
			break;
		case CurseAbilityType.MoreMoneyMoreInjured:
			if (flag)
			{
				curseCfg_MoreMoneyMoreInjured = config;
			}
			else
			{
				curseCfg_MoreMoneyMoreInjured = null;
			}
			break;
		case CurseAbilityType.ReverseShoot:
			if (!flag)
			{
				curse_IsReverseShoot = false;
			}
			break;
		case CurseAbilityType.EnemyReduceDamage:
			if (flag)
			{
				curseCfg_EnemyReduceDamage = config;
			}
			else
			{
				curseCfg_EnemyReduceDamage = null;
			}
			break;
		case CurseAbilityType.ShootSlow:
			if (flag)
			{
				curseCfg_ShootSlow = config;
			}
			else
			{
				curseCfg_ShootSlow = null;
			}
			break;
		case CurseAbilityType.ZeroFriction:
			if (flag)
			{
				curseCfg_ZeroFriction = config;
			}
			else
			{
				curseCfg_ZeroFriction = null;
			}
			break;
		case CurseAbilityType.IsaacVirus:
			if (!flag)
			{
				curse_IsIsaacVirus = false;
			}
			break;
		case CurseAbilityType.Stealthy:
			if (!flag)
			{
				curse_Stealthy.DestroySelf();
				curse_Stealthy = null;
			}
			break;
		case CurseAbilityType.ChestMonster:
			if (flag)
			{
				curseCfg_ChestMonster = config;
			}
			else
			{
				curseCfg_ChestMonster = null;
			}
			break;
		case CurseAbilityType.FullHPAddDamage:
			if (flag)
			{
				curseCfg_FullHPAddDamage = config;
			}
			else
			{
				curseCfg_FullHPAddDamage = null;
			}
			break;
		case CurseAbilityType.LoseAllKey:
			Debug.LogError("这个诅咒根本就不会存在");
			break;
		case CurseAbilityType.Pestilence:
			curseCfg_Pestilence = (flag ? config : null);
			break;
		case CurseAbilityType.LostSpellOption:
			curseCfg_LostSpellOption = (flag ? config : null);
			break;
		case CurseAbilityType.InvalidCurse:
			if (flag)
			{
				curseCfg_InvalidCurse = config;
			}
			else
			{
				curseCfg_InvalidCurse = null;
			}
			break;
		default:
			Debug.LogError(config.abilityType);
			break;
		case CurseAbilityType.RandomCurseCommon:
		case CurseAbilityType.RandomCurseRare:
			break;
		}
		if (curse_RandomCurseCommon != null)
		{
			curse_RandomCurseCommon.CheckRemoveCurseIsMyCurse(config.id);
		}
		if (curse_RandomCurseRare != null)
		{
			curse_RandomCurseRare.CheckRemoveCurseIsMyCurse(config.id);
		}
		PlayerMgr.Inst.BaData.BackCurseToPool(config.id, config.level);
	}

	public void CurseRemoveByID(int id, int removeLevel, bool textFloat = true)
	{
		if (!PlayerMgr.Inst.BaData.curseIDs.Contains(id))
		{
			Debug.LogError("为什么要移除的诅咒并不在玩家身上？");
		}
		else
		{
			CurseRemoveByIndex(PlayerMgr.Inst.BaData.curseIDs.IndexOf(id), removeLevel, textFloat);
		}
	}

	public void ItemPointerToPlayer()
	{
		if (relic_BlockSpellMono != null)
		{
			relic_BlockSpellMono.PointerToPlayer();
		}
		if (relic_GluttonousSnake != null)
		{
			relic_GluttonousSnake.PointerToPlayer();
		}
		if (relic_AddMoveSpeed != null)
		{
			relic_AddMoveSpeed.PointerToPlayer();
		}
		if (relic_GreedSeed != null)
		{
			relic_GreedSeed.PointerToPlayer();
		}
		if (relic_Fly != null)
		{
			relic_Fly.PointerToPlayer();
		}
		if (relic_InvisibleWing != null)
		{
			relic_InvisibleWing.PointerToPlayer();
		}
		if (curse_Shackle != null)
		{
			curse_Shackle.PointerToPlayer();
		}
		if (curse_Recall != null)
		{
			curse_Recall.ResetState();
		}
		PlayerMgr.Inst.PlayerCtrller.FollowObjThrough();
	}

	public void RewardDropFly(int id, SpecialObj217.rewardType rewardType, Vector3 worldPoint, Vector3 MoveToPoint, Vector3? MoveToPointAppearance = null, bool useParticleColor = true, Action dropAction = null, bool isUI = false, bool dropItem = true, RoomController roomController = null)
	{
		RollRewardFly component = UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/UI/RollRewardFly")).GetComponent<RollRewardFly>();
		component.transform.position = worldPoint;
		component.Initialize(id, rewardType, MoveToPoint, MoveToPointAppearance, useParticleColor, dropAction, isUI, dropItem, roomController);
	}

	public void RewardDropFly(int id, RollRewardFly.DropType droptype, Vector3 worldPoint, Vector3 MoveToPoint, Vector3? MoveToPointAppearance = null, bool useParticleColor = true, Action dropAction = null, bool isUI = false, bool dropItem = true, RoomController roomController = null)
	{
		RollRewardFly component = UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/UI/RollRewardFly")).GetComponent<RollRewardFly>();
		component.transform.position = worldPoint;
		component.Initialize(id, droptype, MoveToPoint, MoveToPointAppearance, useParticleColor, dropAction, isUI, dropItem, roomController);
	}

	public void AddRewardFly(int id, RollRewardFly.DropType dropType, Vector2 wordPositionFrom, Camera camRef = null, WandConfig wandConfig = null)
	{
		Vector3 worldPoint = default(Vector3);
		Vector2 vector = default(Vector2);
		Vector3 vector2 = default(Vector3);
		Action dropAction = null;
		switch (dropType)
		{
		case RollRewardFly.DropType.Relic:
		{
			int? realGet = GetRelicFlyPreProcess(id);
			if (!realGet.HasValue)
			{
				return;
			}
			RelicConfig relicConfig = PlayerMgr.Inst.ItemCtrller.GetRelicConfig(realGet.Value);
			bool _alreadyHave = relicConfig != null;
			UIPlayerDataMgr.Inst.RelicFlyCountAdd(realGet.Value);
			if (GameMgr.IsMobile_Static)
			{
				vector = new Vector2(-10f, -10f);
				worldPoint = MobileMgr.inst.topui.goMenuButton.GetComponent<RectTransform>().position;
			}
			else
			{
				vector = new Vector2(30f, -40f);
				if (_alreadyHave)
				{
					worldPoint = UIPlayerDataMgr.Inst.GetObtainedRelicPosition(PlayerMgr.Inst.ItemCtrller.GetRelicIndex(realGet.Value));
				}
				else
				{
					UIPlayerDataMgr.Inst.RelicFlyCountNewAdd(realGet.Value);
					worldPoint = UIPlayerDataMgr.Inst.GetNextRelicPosition(realGet.Value);
				}
			}
			dropAction = delegate
			{
				PlayerMgr.Inst.ItemCtrller.RelicAdd(realGet.Value);
				UIPlayerDataMgr.Inst.RelicFlyCountSub(realGet.Value);
				if (GameMgr.IsMobile_Static)
				{
					UIPlayerDataMgr.Inst.MenuShakeButton();
				}
				else if (!_alreadyHave)
				{
					UIPlayerDataMgr.Inst.RelicFlyCountNewSub(realGet.Value);
				}
			};
			break;
		}
		case RollRewardFly.DropType.Curse:
			if (GameMgr.IsMobile_Static)
			{
				worldPoint = MobileMgr.inst.topui.goMenuButton.GetComponent<RectTransform>().position;
				dropAction = delegate
				{
					PlayerMgr.Inst.ItemCtrller.CurseAdd(id, textFloat: true);
					UIPlayerDataMgr.Inst.MenuShakeButton();
				};
				break;
			}
			vector2 = UIMgr.Inst.canvas_1Scaler.transform.InverseTransformPoint(UIPlayerDataMgr.Inst.uiLayout_Curse.transform.position);
			vector2 += new Vector3((float)PlayerMgr.Inst.BaData.curseIDs.Count * UIPlayerDataMgr.Inst.uiLayout_Curse.childSize.x + UIPlayerDataMgr.Inst.uiLayout_Curse.childSize.x / 2f, (0f - UIPlayerDataMgr.Inst.uiLayout_Curse.childSize.y) / 2f, 0f);
			dropAction = delegate
			{
				PlayerMgr.Inst.ItemCtrller.CurseAdd(id, textFloat: true);
				UIPlayerDataMgr.Inst.MenuShakeButton();
			};
			break;
		case RollRewardFly.DropType.Wand:
		{
			worldPoint = UIPlayerDataMgr.Inst.uiWands[PlayerMgr.Inst.GetPickWandIndex()].image_Icon.transform.position;
			Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(null, worldPoint);
			RectTransformUtility.ScreenPointToLocalPointInRectangle(UIMgr.Inst.canvas_10.GetComponent<RectTransform>(), screenPoint, null, out var localPoint);
			vector2 = localPoint + new Vector2(-10f, -10f);
			dropAction = delegate
			{
				PlayerMgr.Inst.WandPickUp(wandConfig);
			};
			break;
		}
		}
		if (vector2 == default(Vector3))
		{
			Vector2 screenPoint2 = RectTransformUtility.WorldToScreenPoint(null, worldPoint);
			RectTransformUtility.ScreenPointToLocalPointInRectangle(UIMgr.Inst.canvas_10.GetComponent<RectTransform>(), screenPoint2, null, out var localPoint2);
			vector2 = localPoint2 + vector;
		}
		PlayerMgr.Inst.ItemCtrller.UIRewardFly(UIMgr.Inst.canvas_3, id, dropType, wordPositionFrom, vector2, null, useParticleColor: true, dropAction, isUI: true, dropOnEnd: false, null, camRef);
	}

	public void UIRewardFly(Canvas canvas, int id, RollRewardFly.DropType droptype, Vector3 worldPositoin, Vector3 MoveToPointUI, Vector3? MoveToPointAppearanceUI = null, bool useParticleColor = false, Action dropAction = null, bool isUI = true, bool dropOnEnd = false, int? overrideLayer = null, Camera cam = null, RoomController roomController = null)
	{
		if (cam == null)
		{
			cam = CamController.Inst.cam_Main;
		}
		Vector3 worldPointUI = GeneralTool.WorldToCanvasLocalPoint(worldPositoin, canvas, cam);
		UIRewardFly_fromUI(canvas, id, droptype, worldPointUI, MoveToPointUI, MoveToPointAppearanceUI, useParticleColor, dropAction, isUI, dropOnEnd, overrideLayer, roomController);
	}

	public void UIRewardFlyToTransform(Canvas canvas, int id, RollRewardFly.DropType droptype, Vector3 worldPoint, Transform MoveToPointUI, Vector3? MoveToPointAppearanceUIOffset = null, bool useParticleColor = false, Action dropAction = null, bool isUI = true, bool dropOnEnd = false, int? overrideLayer = null, Camera cam = null, RoomController roomController = null)
	{
		if (cam == null)
		{
			cam = CamController.Inst.cam_Main;
		}
		Vector3 worldPointUI = GeneralTool.WorldToCanvasLocalPoint(worldPoint, canvas, cam);
		UIRewardFly_fromUIToTransform(canvas, id, droptype, worldPointUI, MoveToPointUI, MoveToPointAppearanceUIOffset, useParticleColor, dropAction, isUI, dropOnEnd, overrideLayer, roomController);
	}

	private void UIRewardFly_fromUI(Canvas canvas, int id, RollRewardFly.DropType droptype, Vector3 worldPointUI, Vector3 MoveToPointUI, Vector3? MoveToPointAppearanceUI = null, bool useParticleColor = false, Action dropAction = null, bool isUI = true, bool dropOnEnd = false, int? overrideLayer = null, RoomController roomController = null)
	{
		RollRewardFly component = UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/UI/uiRollRewardFly"), canvas.transform).GetComponent<RollRewardFly>();
		if (overrideLayer.HasValue)
		{
			Canvas canvas2 = component.AddComponent<Canvas>();
			canvas2.overrideSorting = true;
			canvas2.sortingOrder = overrideLayer.Value;
		}
		component.GetComponent<RectTransform>().anchoredPosition = worldPointUI;
		component.Initialize(id, droptype, MoveToPointUI, MoveToPointAppearanceUI, useParticleColor, dropAction, isUI: true, dropOnEnd: false, roomController);
	}

	private void UIRewardFly_fromUIToTransform(Canvas canvas, int id, RollRewardFly.DropType droptype, Vector3 worldPointUI, Transform MoveToPointUI, Vector3? MoveToPointAppearanceUI = null, bool useParticleColor = false, Action dropAction = null, bool isUI = true, bool dropOnEnd = false, int? overrideLayer = null, RoomController roomController = null)
	{
		RollRewardFly component = UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/UI/uiRollRewardFly"), canvas.transform).GetComponent<RollRewardFly>();
		if (overrideLayer.HasValue)
		{
			Canvas canvas2 = component.AddComponent<Canvas>();
			canvas2.overrideSorting = true;
			canvas2.sortingOrder = overrideLayer.Value;
		}
		component.GetComponent<RectTransform>().anchoredPosition = worldPointUI;
		component.Initialize(id, droptype, MoveToPointUI, MoveToPointAppearanceUI, useParticleColor, dropAction, isUI: true, dropOnEnd: false, roomController);
	}

	public RollRewardFly CustomDropFly(Canvas canvas, SpecialObj217.rewardType droptype, Vector3 worldPointUI, Transform MoveToPointUI, Sprite overrideSprite, Vector3? MoveToPointAppearanceUI = null, bool useParticleColor = false, Action dropAction = null, bool isUI = true, bool dropOnEnd = false, int? overrideLayer = null, Camera cam = null, RoomController roomController = null)
	{
		RollRewardFly component = UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/UI/uiRollRewardFly"), canvas.transform).GetComponent<RollRewardFly>();
		if (overrideLayer.HasValue)
		{
			Canvas canvas2 = component.AddComponent<Canvas>();
			canvas2.overrideSorting = true;
			canvas2.sortingOrder = overrideLayer.Value;
		}
		if (cam == null)
		{
			cam = CamController.Inst.cam_Main;
		}
		Vector3 vector = GeneralTool.WorldToCanvasLocalPoint(worldPointUI, canvas, cam);
		component.GetComponent<RectTransform>().anchoredPosition = vector;
		component.Initialize(-1, droptype, MoveToPointUI, MoveToPointAppearanceUI, useParticleColor, dropAction, isUI: true, dropOnEnd: false, roomController);
		component.OverrideSprite(overrideSprite);
		return component;
	}
}
