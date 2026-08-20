using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

public class UIRelic_RestartKey : MonoBehaviour
{
	public RectTransform rtsf_KeyCenter;

	public float keyToCenterSpeed;

	public float finishDelayToDestroy;

	public AudioSource as_Loop;

	private List<int> bagSpellIDs = new List<int>();

	private List<int> wandIDs = new List<int>();

	private List<int> wandSpellIDs = new List<int>();

	private Dictionary<int, int> specialRelics = new Dictionary<int, int>();

	private Dictionary<int, int> relics = new Dictionary<int, int>();

	private Dictionary<int, int> curses = new Dictionary<int, int>();

	private int potionCount;

	private bool isFinish;

	private float finishDelayToDestroyTimer;

	private void OnEnable()
	{
		EventMgr.SoundVolumeChange = (Action)Delegate.Combine(EventMgr.SoundVolumeChange, new Action(SoundVolumeChange));
	}

	private void OnDisable()
	{
		EventMgr.SoundVolumeChange = (Action)Delegate.Remove(EventMgr.SoundVolumeChange, new Action(SoundVolumeChange));
	}

	private void SoundVolumeChange()
	{
		as_Loop.volume = DataMgr.settingData.GetFinalSound();
	}

	private void Start()
	{
		PlayerMgr.Inst.InvincibleRegister();
		PlayerMgr.Inst.PlayerCtrller.StopMotion();
		rtsf_KeyCenter.anchoredPosition = GeneralTool.WorldToCanvasLocalPoint(PlayerMgr.Inst.PlayerPoint);
		MusicMgr.Inst.ForcePlayMusic("");
		SoundVolumeChange();
	}

	private void Update()
	{
		if (rtsf_KeyCenter.anchoredPosition != Vector2.zero)
		{
			rtsf_KeyCenter.anchoredPosition = Vector2.MoveTowards(rtsf_KeyCenter.anchoredPosition, Vector2.zero, keyToCenterSpeed * Time.deltaTime);
		}
		if (isFinish)
		{
			finishDelayToDestroyTimer += Time.deltaTime;
			if (finishDelayToDestroyTimer >= finishDelayToDestroy)
			{
				RefreshAllLaser();
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}
	}

	private void _Through()
	{
		PlayerMgr.Inst.ClearAllRegister();
		PlayerMgr.Inst.FlyRegisterWithAllMate();
		PlayerMgr.Inst.InvincibleRegister();
		GameMgr.Inst.ClearAllPool();
		if (BattleMgr.Inst.CurrentStage % 2 == 0)
		{
			BattleMgr.Inst.CurrentStage--;
		}
		BattleMgr.Inst.CurrentLevel = 0;
		BattleMgr.Inst.CurrentRewardType = LevelRewardType.None;
		BattleMgr.Inst.NextRewardTypes = OutputMgr.GetDoors();
		for (int i = 0; i < PlayerMgr.Inst.Wands.Count; i++)
		{
			PlayerMgr.Inst.Wands[i].ClearAutoSpell(typeof(Spell4019BiAnBladeData));
		}
		if (PlayerMgr.Inst.ItemCtrller.relic_ExtraDoor)
		{
			BattleMgr.Inst.NextExtraDoorRewardType = OutputMgr.GetExtraDoor(BattleMgr.Inst.NextRewardTypes);
		}
		else
		{
			BattleMgr.Inst.NextExtraDoorRewardType = LevelRewardType.None;
		}
		if (PlayerMgr.Inst.ItemCtrller.curse_Stealthy != null)
		{
			PlayerMgr.Inst.ItemCtrller.curse_Stealthy.DestroySelf();
		}
		PlayerMgr.Inst.ItemCtrller.curse_IsInvisibleDoor = false;
		Dictionary<Vector2Int, RoomConfig> dictionary = new Dictionary<Vector2Int, RoomConfig>();
		RoomConfig chapterRoom = OutputMgr.GetChapterRoom0();
		chapterRoom.isFinalRoom = true;
		dictionary.Add(Vector2Int.zero, chapterRoom);
		PlayerMgr.Inst.BaData.currentRoomID = dictionary[Vector2Int.zero].id;
		LevelMgr.Inst.CreateLevel(dictionary, BattleMgr.Inst.CurrentRewardType, BattleMgr.Inst.NextRewardTypes, BattleMgr.Inst.NextExtraDoorRewardType, fadeDisappear: false, CreateLevelFinish);
	}

	public void RefreshAllLaser()
	{
		EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
		EntityCommandBuffer entityCommandBuffer = entityManager.World.GetOrCreateSystemManaged<EndSimulationEntityCommandBufferSystem>().CreateCommandBuffer();
		using EntityQuery entityQuery = entityManager.CreateEntityQuery(new EntityQueryDesc
		{
			All = new ComponentType[3]
			{
				ComponentType.ReadOnly<SpellConfigComponentData>(),
				new ComponentType(typeof(Spell4014LaserCrystalData), ComponentType.AccessMode.ReadOnly),
				ComponentType.ReadOnly<SpellComponentData>()
			}
		});
		using NativeArray<Entity> nativeArray = entityQuery.ToEntityArray(Allocator.TempJob);
		using (entityQuery.ToComponentDataArray<SpellComponentData>(Allocator.TempJob))
		{
			for (int i = 0; i < nativeArray.Length; i++)
			{
				entityCommandBuffer.DestroyEntity(nativeArray[i]);
			}
			if (SEMgr.Inst.loopSEs.TryGetValue("SE_Spell4014Loop", out var value))
			{
				value.mute = true;
			}
			for (int j = 0; j < PlayerMgr.Inst.Wands.Count; j++)
			{
				PlayerMgr.Inst.Wands[j].RefreshLaserBeam();
			}
		}
	}

	private void CreateLevelFinish()
	{
		for (int i = 0; i < PlayerMgr.Inst.BaData.bagSpellDatas.Count; i++)
		{
			if (PlayerMgr.Inst.BaData.bagSpellDatas[i] != null && PlayerMgr.Inst.BaData.bagSpellDatas[i].id != 0)
			{
				bagSpellIDs.Add(PlayerMgr.Inst.BaData.bagSpellDatas[i].id);
			}
		}
		for (int j = 0; j < PlayerMgr.Inst.BaData.wandCfgs.Count; j++)
		{
			if (PlayerMgr.Inst.BaData.wandCfgs[j] == null)
			{
				continue;
			}
			wandIDs.Add(PlayerMgr.Inst.BaData.wandCfgs[j].id);
			if (PlayerMgr.Inst.BaData.wandCfgs[j].normalSlots != null)
			{
				for (int k = 0; k < PlayerMgr.Inst.BaData.wandCfgs[j].normalSlots.Length; k++)
				{
					if (PlayerMgr.Inst.BaData.wandCfgs[j].normalSlots[k] != null && PlayerMgr.Inst.BaData.wandCfgs[j].normalSlots[k].id != 0 && !PlayerMgr.Inst.BaData.wandCfgs[j].normalSlots[k].isSealSlot && !PlayerMgr.Inst.BaData.wandCfgs[j].normalSlotIsLock[k])
					{
						wandSpellIDs.Add(PlayerMgr.Inst.BaData.wandCfgs[j].normalSlots[k].id);
					}
				}
			}
			if (PlayerMgr.Inst.BaData.wandCfgs[j].postSlots == null)
			{
				continue;
			}
			for (int l = 0; l < PlayerMgr.Inst.BaData.wandCfgs[j].postSlots.Length; l++)
			{
				if (PlayerMgr.Inst.BaData.wandCfgs[j].postSlots[l] != null && PlayerMgr.Inst.BaData.wandCfgs[j].postSlots[l].id != 0 && !PlayerMgr.Inst.BaData.wandCfgs[j].postSlots[l].isSealSlot && !PlayerMgr.Inst.BaData.wandCfgs[j].postSlotIsLock[l])
				{
					wandSpellIDs.Add(PlayerMgr.Inst.BaData.wandCfgs[j].postSlots[l].id);
				}
			}
		}
		for (int m = 0; m < PlayerMgr.Inst.BaData.relicCfgs.Count; m++)
		{
			if (PlayerMgr.Inst.BaData.relicCfgs[m].id != 69)
			{
				relics.Add(PlayerMgr.Inst.BaData.relicCfgs[m].id, PlayerMgr.Inst.BaData.relicCfgs[m].level);
			}
		}
		for (int n = 0; n < PlayerMgr.Inst.BaData.curseIDs.Count; n++)
		{
			curses.Add(PlayerMgr.Inst.BaData.curseIDs[n], PlayerMgr.Inst.BaData.curseLevels[n]);
		}
		for (int num = 0; num < PlayerMgr.Inst.BaData.potionIDs.Count; num++)
		{
			if (PlayerMgr.Inst.BaData.potionIDs[num] != 0)
			{
				potionCount++;
			}
		}
		List<int> list = new List<int>();
		for (int num2 = 0; num2 < PlayerMgr.Inst.BaData.poolOfSpellID.Count; num2++)
		{
			list.Add(PlayerMgr.Inst.BaData.poolOfSpellID[num2]);
		}
		PlayerMgr.Inst.BaData.InitialPools();
		PlayerMgr.Inst.BaData.bagSpellDatas.Clear();
		PlayerMgr.Inst.BaData.poolOfRelic.Remove(69);
		UIPlayerDataMgr.Inst.rtsf_ActiveRelicUIRoot.DestroyAllChild();
		PlayerMgr.Inst.PlayerCtrller.followObjClear();
		PlayerMgr.Inst.PlayerCtrller.tsf_WandRoot.localPosition = Vector3.zero;
		PlayerMgr.Inst.PlayerCtrller.SetVisiable();
		UIPlayerDataMgr.Inst.ShowResource();
		List<RelicAbilityType> list2 = new List<RelicAbilityType>
		{
			RelicAbilityType.Heavyweights,
			RelicAbilityType.ExtraPotionStorage,
			RelicAbilityType.LessWandMoreSlot
		};
		foreach (KeyValuePair<int, int> relic in relics)
		{
			for (int num3 = 0; num3 < relic.Value; num3++)
			{
				RelicAbilityType abilityType = RelicConfig.dic[relic.Key].abilityType;
				if (list2.Contains(abilityType))
				{
					PlayerMgr.Inst.ItemCtrller.RelicRemove(relic.Key, 1);
				}
			}
		}
		UnityEngine.Object.Destroy(PlayerMgr.Inst.ItemCtrller);
		PlayerMgr.Inst.ItemCtrller = PlayerMgr.Inst.AddComponent<PlayerItemController>();
		LevelMgr.Inst.tsf_PlayerThings.DestroyAllChild();
		LevelMgr.Inst.globalLight.gameObject.SetActive(value: true);
		UIPlayerDataMgr.Inst.BagCheckRelicPandorasBoxImage();
		for (int num4 = 0; num4 < PlayerMgr.Inst.Wands.Count; num4++)
		{
			PlayerMgr.Inst.DropWand(num4, spawnOnGround: false);
		}
		PlayerMgr.Inst.BaData.wandCfgs.Clear();
		PlayerMgr.Inst.BaData.relicCfgs.Clear();
		PlayerMgr.Inst.BaData.potionIDs.Clear();
		PlayerMgr.Inst.BaData.curseIDs.Clear();
		PlayerMgr.Inst.BaData.curseLevels.Clear();
		PlayerMgr.Inst.BaData.shortcutRoomAppearedStage1 = false;
		PlayerMgr.Inst.BaData.shortcutRoomAppearedStage2 = false;
		PlayerMgr.Inst.BaData.shortcutRoomAppearedStage3 = false;
		PlayerMgr.Inst.BaData.shortcutRoomAppearedStage4 = false;
		PlayerMgr.Inst.BaData.shortcutRoomAppearedStage5 = false;
		PlayerMgr.Inst.BaData.shortcutRoomAppearedStage6 = false;
		PlayerMgr.Inst.BaData.poolOfSpellID = list;
		for (int num5 = 0; num5 < PlayerMgr.Inst.BaData.wandMaxCount; num5++)
		{
			PlayerMgr.Inst.BaData.wandCfgs.Add(null);
		}
		for (int num6 = 0; num6 < PlayerMgr.Inst.BaData.bagCount; num6++)
		{
			PlayerMgr.Inst.BaData.bagSpellDatas.Add(null);
		}
		for (int num7 = 0; num7 < bagSpellIDs.Count; num7++)
		{
			if (SpellConfig.dic[bagSpellIDs[num7]].dropType == ItemDropType.Special || SpellConfig.dic[bagSpellIDs[num7]].abilityType == SpellAbilityType.ShotGun)
			{
				PlayerMgr.Inst.SpellPick(new SlotData(bagSpellIDs[num7]));
				continue;
			}
			int level = SpellConfig.dic[bagSpellIDs[num7]].level;
			ItemDropType dropType = SpellConfig.dic[bagSpellIDs[num7]].dropType;
			if (SpellConfig.dic[bagSpellIDs[num7]].abilityType == SpellAbilityType.DeathAdder)
			{
				level = 1;
			}
			int spellFromPool = PlayerMgr.Inst.BaData.GetSpellFromPool(level, dropType);
			DataMgr.selectedWorldData.GalleryUnlock(GalleryCategory.Spell, spellFromPool);
			PlayerMgr.Inst.SpellPick(new SlotData(spellFromPool));
			DataMgr.selectedWorldData.GalleryUnlock(GalleryCategory.Spell, spellFromPool);
		}
		foreach (KeyValuePair<int, int> relic2 in relics)
		{
			for (int num8 = 0; num8 < relic2.Value; num8++)
			{
				if (RelicConfig.dic[relic2.Key].dropType == ItemDropType.Special || GameConstManaged.SpecialRelicList.Contains(RelicConfig.dic[relic2.Key].id))
				{
					PlayerMgr.Inst.ItemCtrller.RelicAdd(relic2.Key);
					continue;
				}
				int relicFromPool = PlayerMgr.Inst.BaData.GetRelicFromPool(RelicConfig.dic[relic2.Key].dropType);
				PlayerMgr.Inst.ItemCtrller.RelicAdd(relicFromPool);
				DataMgr.selectedWorldData.GalleryUnlock(GalleryCategory.Relic, relicFromPool);
			}
		}
		for (int num9 = 0; num9 < PlayerMgr.Inst.BaData.potionMaxCount; num9++)
		{
			PlayerMgr.Inst.BaData.potionIDs.Add(0);
		}
		UIPotionsController.Inst.CheckCountAndUpdateAllUI();
		for (int num10 = 0; num10 < wandIDs.Count; num10++)
		{
			int dropStage = WandConfig.dic[wandIDs[num10]].dropStage;
			WandConfig wandConfig = null;
			wandConfig = ((dropStage <= 100) ? WandConfig.GetConfig(PlayerMgr.Inst.BaData.GetWandFromPool(dropStage, 2014, 4016)) : WandConfig.GetConfig(wandIDs[num10]));
			for (int num11 = 0; num11 < wandConfig.normalSlots.Length; num11++)
			{
				if (wandConfig.normalSlots[num11] != null && (wandConfig.normalSlots[num11].id != 0 || wandConfig.normalSlots[num11].isSealSlot) && !wandConfig.normalSlotIsLock[num11])
				{
					wandConfig.normalSlots[num11] = null;
				}
			}
			for (int num12 = 0; num12 < wandConfig.postSlots.Length; num12++)
			{
				if (wandConfig.postSlots[num12] != null && (wandConfig.postSlots[num12].id != 0 || wandConfig.postSlots[num12].isSealSlot) && !wandConfig.postSlotIsLock[num12])
				{
					wandConfig.postSlots[num12] = null;
				}
			}
			if (num10 < PlayerMgr.Inst.BaData.wandMaxCount)
			{
				PlayerMgr.Inst.BaData.wandCfgs[num10] = wandConfig;
			}
			else
			{
				QuickCreateSystem.Inst.CreateItem(LevelMgr.Inst.CurrentRoomMapPos, wandConfig, PlayerMgr.Inst.PlayerPointIgnoreZ);
			}
			DataMgr.selectedWorldData.GalleryUnlock(GalleryCategory.Wand, wandConfig.id);
		}
		Wand.wandHoldingFlyEffectApplying = false;
		if (wandSpellIDs.Count > 0)
		{
			List<SlotData> list3 = new List<SlotData>();
			for (int num13 = 0; num13 < wandSpellIDs.Count; num13++)
			{
				if (SpellConfig.dic[wandSpellIDs[num13]].dropType == ItemDropType.Special || SpellConfig.dic[wandSpellIDs[num13]].abilityType == SpellAbilityType.ShotGun)
				{
					PlayerMgr.Inst.SpellPick(new SlotData(wandSpellIDs[num13]));
					continue;
				}
				int level2 = SpellConfig.dic[wandSpellIDs[num13]].level;
				ItemDropType dropType2 = SpellConfig.dic[wandSpellIDs[num13]].dropType;
				if (SpellConfig.dic[wandSpellIDs[num13]].abilityType == SpellAbilityType.DeathAdder)
				{
					level2 = 1;
				}
				int spellFromPool2 = PlayerMgr.Inst.BaData.GetSpellFromPool(level2, dropType2);
				DataMgr.selectedWorldData.GalleryUnlock(GalleryCategory.Spell, spellFromPool2);
				list3.Add(new SlotData(spellFromPool2));
				DataMgr.selectedWorldData.GalleryUnlock(GalleryCategory.Spell, spellFromPool2);
			}
			int num14 = 0;
			if (list3.Count > 0)
			{
				for (int num15 = 0; num15 < PlayerMgr.Inst.BaData.wandCfgs.Count; num15++)
				{
					if (PlayerMgr.Inst.BaData.wandCfgs[num15] == null)
					{
						continue;
					}
					for (int num16 = 0; num16 < PlayerMgr.Inst.Wands[num15].WandCfg.normalSlots.Length; num16++)
					{
						if (PlayerMgr.Inst.Wands[num15].WandCfg.normalSlots[num16] == null && PlayerMgr.Inst.CanChangeInWandSpell(num15, WandSlotType.Normal, num16, list3[num14]))
						{
							PlayerMgr.Inst.ChangeWandSpell(num15, WandSlotType.Normal, num16, list3[num14]);
							num14++;
							if (num14 >= list3.Count)
							{
								break;
							}
						}
					}
					if (num14 >= list3.Count)
					{
						break;
					}
					for (int num17 = 0; num17 < PlayerMgr.Inst.BaData.wandCfgs[num15].postSlots.Length; num17++)
					{
						if (PlayerMgr.Inst.BaData.wandCfgs[num15].postSlots[num17] == null && PlayerMgr.Inst.CanChangeInWandSpell(num15, WandSlotType.Post, num17, list3[num14]))
						{
							PlayerMgr.Inst.ChangeWandSpell(num15, WandSlotType.Post, num17, list3[num14]);
							num14++;
							if (num14 >= list3.Count)
							{
								break;
							}
						}
					}
					if (num14 >= list3.Count)
					{
						break;
					}
				}
			}
			if (num14 < list3.Count)
			{
				for (int num18 = num14; num18 < list3.Count; num18++)
				{
					PlayerMgr.Inst.SpellPick(list3[num18]);
				}
			}
		}
		PlayerMgr.Inst.SpellPick(new SlotData(10011));
		foreach (KeyValuePair<int, int> curse in curses)
		{
			for (int num19 = 0; num19 < curse.Value; num19++)
			{
				int curseFromPool = PlayerMgr.Inst.BaData.GetCurseFromPool(CurseConfig.dic[curse.Key].dropType);
				PlayerMgr.Inst.ItemCtrller.CurseAdd(curseFromPool, textFloat: false);
				DataMgr.selectedWorldData.GalleryUnlock(GalleryCategory.Curse, curseFromPool);
			}
		}
		for (int num20 = 0; num20 < potionCount; num20++)
		{
			int potionFromPool = PlayerMgr.Inst.BaData.GetPotionFromPool();
			PlayerMgr.Inst.ItemCtrller.PotionPickup(potionFromPool);
			DataMgr.selectedWorldData.GalleryUnlock(GalleryCategory.Potion, potionFromPool);
		}
		UIPlayerDataMgr.Inst.UpdateAllInfo();
		for (int num21 = 0; num21 < PlayerMgr.Inst.Wands.Count; num21++)
		{
			PlayerMgr.Inst.Wands[num21].ResetAndRecheck();
			PlayerMgr.Inst.Wands[num21].Display_UpdateShowOrHide();
		}
	}

	private void _THroughFinish()
	{
		isFinish = true;
		PlayerMgr.Inst.PlayerCtrller.StartMotion();
		PlayerMgr.Inst.PlayerCtrller.ClearDashOverheat();
		PlayerMgr.Inst.FlyUnregisterWithAllMate();
		PlayerMgr.Inst.InvincibleUnregister();
	}
}
