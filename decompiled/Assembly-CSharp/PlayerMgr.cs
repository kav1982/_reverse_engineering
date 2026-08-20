using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Extensions;
using Unity.Physics.GraphicsIntegration;
using Unity.Transforms;
using UnityEngine;

public class PlayerMgr : MonoBehaviour
{
	public List<Wand> Wands = new List<Wand>();

	public PlayerItemController ItemCtrller;

	[HideInInspector]
	public bool inDashSpell;

	[HideInInspector]
	public bool inDashSpellAccessT6;

	[HideInInspector]
	public List<UnitProperty> summonsPpts = new List<UnitProperty>();

	[HideInInspector]
	public List<UnitProperty> summonsNotAttackPpts = new List<UnitProperty>();

	[HideInInspector]
	public Dictionary<Wand, UnitProperty> autoWandList = new Dictionary<Wand, UnitProperty>();

	private EntityManager ettMgr;

	public static PlayerMgr Inst { get; private set; }

	public Transform PlayerT => PlayerGO.transform;

	public Vector3 PlayerPoint
	{
		get
		{
			if (!PlayerGO)
			{
				return Vector3.zero;
			}
			return PlayerT.position;
		}
	}

	public Vector3 PlayerPointIgnoreZ => Tool2D.IgnoreZPoint(PlayerT.position);

	public UnitProperty PlayerPpt { get; private set; }

	public PlayerController PlayerCtrller { get; private set; }

	public Vector3 PlayerDir => PlayerCtrller.CurrentDir;

	public Wand SelectedWand
	{
		get
		{
			if (SelectedWandIndex == -1)
			{
				return null;
			}
			if (SelectedWandIndex >= Wands.Count)
			{
				return null;
			}
			return Wands[SelectedWandIndex];
		}
	}

	public BattleData BaData
	{
		get
		{
			return DataMgr.selectedWorldData.battleData9;
		}
		set
		{
			DataMgr.selectedWorldData.battleData9 = value;
		}
	}

	public WandConfig SelectedWandCfg
	{
		get
		{
			if (BaData?.wandCfgs == null)
			{
				return null;
			}
			if (!BaData.wandCfgs.IndexOutRange(SelectedWandIndex))
			{
				return BaData.wandCfgs[SelectedWandIndex];
			}
			return null;
		}
	}

	public Vector3 ShootPoint
	{
		get
		{
			if (!SelectedWand)
			{
				return PlayerPoint;
			}
			return SelectedWand.ShootPosition;
		}
	}

	public bool IsAffectedTimeScale { get; set; } = true;


	public float PlayerDeltaTime
	{
		get
		{
			if (!IsAffectedTimeScale)
			{
				return Time.unscaledDeltaTime;
			}
			return Time.deltaTime;
		}
	}

	public int CoinCount => BaData.coinCount;

	public int KeyCount => BaData.keyCount;

	public float PlayerHPRatio
	{
		get
		{
			if (TryGetPlayerPpt(out var playerPpt))
			{
				return playerPpt.unitCfg.currentHP / playerPpt.unitCfg.maxHP;
			}
			return 0f;
		}
	}

	public bool IsFullHP
	{
		get
		{
			if (TryGetPlayerPpt(out var playerPpt))
			{
				return playerPpt.unitCfg.currentHP >= playerPpt.unitCfg.maxHP;
			}
			return false;
		}
	}

	public float ExtraDamageRatio
	{
		get
		{
			float num = BaData.extraDamageRatio;
			if (ItemCtrller.relicCfg_CurseWarrior != null)
			{
				for (int i = 0; i < BaData.curseLevels.Count; i++)
				{
					num += (float)ItemCtrller.relicCfg_CurseWarrior.int1.result / 100f * (float)BaData.curseLevels[i];
				}
			}
			if (ItemCtrller.relicCfg_MoneyIsPower != null)
			{
				num += (float)CoinCount / (float)ItemCtrller.relicCfg_MoneyIsPower.int1.result * (float)ItemCtrller.relicCfg_MoneyIsPower.int2.result / 100f;
			}
			if (ItemCtrller.relicCfg_KeyIsPower != null)
			{
				num += (float)KeyCount / (float)ItemCtrller.relicCfg_KeyIsPower.int1.result * (float)ItemCtrller.relicCfg_KeyIsPower.int2.result / 100f;
			}
			if (ItemCtrller.relicCfg_AddDamage != null)
			{
				num += (float)ItemCtrller.relicCfg_AddDamage.int1.result / 100f;
			}
			if (ItemCtrller.relicCfg_PowerfulMan != null)
			{
				num += (float)ItemCtrller.relicCfg_PowerfulMan.int1.result / 100f;
			}
			if (ItemCtrller.relicCfg_MadEye != null)
			{
				num += ItemCtrller.relicCfg_MadEye.floatTimer;
			}
			if (ItemCtrller.relic_MadWarrior != null)
			{
				num += ItemCtrller.relic_MadWarrior.ExtraDamageRatio;
			}
			if (ItemCtrller.relic_EndlessExtraDamage != null)
			{
				num += (float)ItemCtrller.relic_EndlessExtraDamage.int1.result / 100f;
			}
			if (ItemCtrller.curseCfg_ReduceSpeedDamage != null)
			{
				num -= (float)ItemCtrller.curseCfg_ReduceSpeedDamage.int1.result / 100f;
			}
			if (ItemCtrller.curseCfg_FullHPAddDamage != null)
			{
				num += ItemCtrller.curseCfg_FullHPAddDamage.floatTimer;
			}
			return num;
		}
	}

	public float ExtraFinalDamageRatio => 1f;

	public float ExtraCriticalRatio
	{
		get
		{
			float num = BaData.extraCriticalChance;
			if (ItemCtrller.relicCfg_AddCriticalChance != null)
			{
				num += (float)ItemCtrller.relicCfg_AddCriticalChance.int1.result / 100f;
			}
			if (ItemCtrller.curseCfg_ReduceCriticalRatio != null)
			{
				num += (float)ItemCtrller.curseCfg_ReduceCriticalRatio.int1.result / 100f;
			}
			if (ItemCtrller.uiRelic_RuneWizard != null)
			{
				num += ItemCtrller.uiRelic_RuneWizard.GetRuneWizardSetBonusCriticalChance() / 100f;
			}
			return num;
		}
	}

	public float SummonCountRatio
	{
		get
		{
			float num = 1f;
			if (ItemCtrller.relicCfg_SummonLimit != null)
			{
				num *= (float)ItemCtrller.relicCfg_SummonLimit.int1.result;
			}
			if (ItemCtrller.curseCfg_SummonsReduce != null)
			{
				num /= (float)ItemCtrller.curseCfg_SummonsReduce.int1.result;
			}
			return num;
		}
	}

	public int CanSelectWandCount => Wands.Select((Wand _, int i) => i).Count(CanSelectWand);

	public bool IsBag100Full
	{
		get
		{
			if (DataMgr.selectedWorldData.battleData9 != null)
			{
				return DataMgr.selectedWorldData.battleData9.bagSpellDatas.All((SlotData x) => x != null && (x.id != 0 || x.isSealSlot));
			}
			return false;
		}
	}

	public GameObject PlayerGO { get; private set; }

	public int SelectedWandIndex { get; private set; } = -1;


	public MiniObjPool MiniPool { get; set; }

	public List<Spell1020CoinOnGround> manaCoinList { get; set; } = new List<Spell1020CoinOnGround>();


	public bool IsHide { get; private set; }

	public Entity PlayerEtt { get; private set; }

	public float ExtraRadiusOfInfluence(bool isSpell)
	{
		float num = 0f;
		if (ItemCtrller.relic_RemoteShoot != null && isSpell)
		{
			num += (float)ItemCtrller.relic_RemoteShoot.RelicCfg.int1.result / 100f;
		}
		if (ItemCtrller.relicCfg_AddRadiusOfInfluence != null)
		{
			num += (float)ItemCtrller.relicCfg_AddRadiusOfInfluence.int1.result / 100f;
		}
		if (ItemCtrller.curseCfg_ReduceSpellRadius != null)
		{
			num -= (float)ItemCtrller.curseCfg_ReduceSpellRadius.int1.result / 100f;
		}
		return num;
	}

	public void Initialize()
	{
		Inst = this;
	}

	private void Awake()
	{
		ettMgr = World.DefaultGameObjectInjectionWorld.EntityManager;
	}

	public void CreatePlayer()
	{
		if (ItemCtrller != null)
		{
			UnityEngine.Object.Destroy(ItemCtrller);
		}
		ItemCtrller = base.transform.AddComponent<PlayerItemController>();
		if (ItemCtrller.potion_HoverEFGO != null)
		{
			UnityEngine.Object.Destroy(ItemCtrller.potion_HoverEFGO);
		}
		UIPlayerDataMgr.Inst.ShowResource();
		UIPlayerDataMgr.Inst.BagCheckRelicPandorasBoxImage();
		UIPlayerDataMgr.Inst.rtsf_ActiveRelicUIRoot.DestroyAllChild();
		inDashSpell = false;
		summonsPpts.Clear();
		summonsNotAttackPpts.Clear();
		foreach (KeyValuePair<Wand, UnitProperty> autoWand in autoWandList)
		{
			CancelAutoControlWand(autoWand.Key);
		}
		autoWandList.Clear();
		if (PlayerGO != null)
		{
			UnityEngine.Object.Destroy(PlayerGO);
		}
		SelectedWandIndex = -1;
		if (MiniPool != null)
		{
			UnityEngine.Object.Destroy(MiniPool.gameObject);
		}
		MiniPool = UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Mixed/MiniObjPool")).GetComponent<MiniObjPool>();
		IsHide = false;
		BaData = new BattleData();
		BaData.Initialize();
		if (PlayerEtt != Entity.Null)
		{
			ettMgr.DestroyEntity(PlayerEtt);
		}
		using EntityQuery entityQuery = ettMgr.CreateEntityQuery(typeof(AllUnitEtt));
		AllUnitEtt singleton = entityQuery.GetSingleton<AllUnitEtt>();
		PlayerEtt = ettMgr.Instantiate(singleton.map[800001]);
		PhysicsCollider collider = ettMgr.GetComponentData<PhysicsCollider>(PlayerEtt);
		Entity entity = PlayerEtt;
		collider.MakeUnique(in entity, ettMgr);
		ettMgr.SetComponentData(PlayerEtt, collider);
		LocalTransform componentData = ettMgr.GetComponentData<LocalTransform>(PlayerEtt);
		componentData.Position = PlayerPoint;
		ettMgr.SetComponentData(PlayerEtt, componentData);
		ettMgr.SetName(PlayerEtt, "Player");
		UnitProperty_Dots componentData2 = ettMgr.GetComponentData<UnitProperty_Dots>(PlayerEtt);
		componentData2.unitCfg = BaData.playerCfg;
		componentData2.Initialize(UnitConfig.map);
		ettMgr.SetComponentData(PlayerEtt, componentData2);
		PlayerGO = UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Units/" + 800001));
		PlayerGO.SetActive(value: true);
		PlayerCtrller = PlayerGO.GetComponent<PlayerController>();
		PlayerCtrller.myPpt.myEntity = PlayerEtt;
		PlayerPpt = PlayerCtrller.myPpt;
		UnitDotsSyncSystem.existingUnit.Add(PlayerPpt);
		PlayerController_Dots componentData3 = ettMgr.GetComponentData<PlayerController_Dots>(PlayerEtt);
		componentData3.playerCtrllerMono = PlayerCtrller;
		ettMgr.SetComponentData(PlayerEtt, componentData3);
		UpdateSkin();
	}

	public void SetPlayerPoint(Vector3 pos)
	{
		using EntityQuery entityQuery = ettMgr.CreateEntityQuery(typeof(PlayerController_Dots));
		if (entityQuery.CalculateEntityCount() > 0)
		{
			PlayerCtrller.ps_FootStepSmoke.Stop();
			Entity singletonEntity = entityQuery.GetSingletonEntity();
			LocalTransform componentData = ettMgr.GetComponentData<LocalTransform>(singletonEntity);
			componentData.Position = pos;
			ettMgr.SetComponentData(singletonEntity, componentData);
			PlayerT.position = pos;
			PhysicsGraphicalSmoothing componentData2 = ettMgr.GetComponentData<PhysicsGraphicalSmoothing>(singletonEntity);
			componentData2.ApplySmoothing = 0;
			componentData2.CurrentVelocity.Linear = ettMgr.GetComponentData<PhysicsVelocity>(singletonEntity).Linear;
			ettMgr.SetComponentData(singletonEntity, componentData2);
			LocalToWorld componentData3 = ettMgr.GetComponentData<LocalToWorld>(singletonEntity);
			componentData3.Value.c3 = new float4(PlayerT.position, componentData3.Value.c3.w);
			ettMgr.SetComponentData(singletonEntity, componentData3);
			PlayerCtrller.ps_FootStepSmoke.Play();
		}
	}

	public void RefreshPlayer(bool cancelAutoWand = true)
	{
		if (ItemCtrller.relicCfg_NoAttackStealth != null)
		{
			PlayerCtrller.SetVisiable();
		}
		if (cancelAutoWand)
		{
			Wands?.Where((Wand e) => (object)e != null && e.WandCfg != null && e.passiveAutoWand).Action(CancelAutoControlWand);
		}
		foreach (Wand wand in Wands)
		{
			wand.ClearAutoSpell(typeof(Spell4019BiAnBladeData));
		}
		BaData = new BattleData();
		BaData.Initialize();
		UnitProperty_Dots componentData = ettMgr.GetComponentData<UnitProperty_Dots>(PlayerEtt);
		componentData.unitCfg = BaData.playerCfg;
		ettMgr.SetComponentData(PlayerEtt, componentData);
		UIPlayerDataMgr.Inst.WandReset();
		UIPlayerDataMgr.Inst.UpdateBag();
		UIPlayerDataMgr.Inst.UpdateHP();
		UIPlayerDataMgr.Inst.UpdateCoin();
		WandRecreate(rebuildUI: false, cancelAutoWand);
		WandSelect(0);
		UpdateSkin();
		AllWandFullMP();
		if (DataMgr.selectedWorldData.GetSelectedSetCfg().relicID != 0)
		{
			RelicAddFromSet();
		}
	}

	public void RelicAddFromSet()
	{
		for (int i = 0; i < DataMgr.selectedWorldData.setUnlockedSets[DataMgr.selectedWorldData.selectedSetID]; i++)
		{
			ItemCtrller.RelicAdd(DataMgr.selectedWorldData.GetSelectedSetCfg().relicID, addGallery: false);
		}
	}

	public void SpawnAutoControlWand(Wand wand)
	{
		if (wand == null || autoWandList.ContainsKey(wand) || !wand.passiveAutoWand)
		{
			return;
		}
		Spell4005SummonAutoWand component = MiniPool.GetGO("Prefabs/Spell/" + 40051, Tool2D.GetNavMeshPointIngoreZ(ShootPoint)).GetComponent<Spell4005SummonAutoWand>();
		component.CreateFromMiniPool = true;
		component.targetWand = wand;
		component.Initialize(PlayerPpt, Vector3.zero, 40051, 0f, new List<int>());
		if (SelectedWand == wand)
		{
			wand.Display_Hide();
		}
		EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
		using EntityQuery entityQuery = entityManager.CreateEntityQuery(ComponentType.ReadOnly<SpellSingleton>());
		Entity singletonEntity = entityQuery.GetSingletonEntity();
		SpellSingleton componentData = entityManager.GetComponentData<SpellSingleton>(singletonEntity);
		FixedString64Bytes fs = "Spell_4005";
		Entity entity = entityManager.Instantiate(componentData.Prefabs[fs]);
		entityManager.SetComponentData(entity, new LocalTransform
		{
			Position = wand.transform.position,
			Scale = 1f,
			Rotation = quaternion.identity
		});
		entityManager.SetComponentData(entity, new Spell4005WandSpiritData
		{
			Wand = wand
		});
		wand.PassiveWandSpiritEntity = entity;
	}

	public Teammate52 GetAutoWandScript(Wand wand)
	{
		if (autoWandList.ContainsKey(wand))
		{
			return autoWandList[wand].GetComponent<Teammate52>();
		}
		return null;
	}

	public void CancelAutoControlWand(Wand wand)
	{
		if (wand == null)
		{
			return;
		}
		CancelWandAutoSpell(wand);
		if (autoWandList.ContainsKey(wand) && !(autoWandList[wand] == null) && autoWandList[wand].gameObject.activeInHierarchy)
		{
			if (wand.IsCharging)
			{
				wand.CancelCharge();
			}
			SpellTools.KillAllChildTeammates(World.DefaultGameObjectInjectionWorld.EntityManager, in wand.PassiveWandSpiritEntity);
			wand.passiveAutoWandShooterData = null;
			UnitProperty unitProperty = autoWandList[wand];
			SpellBase summonerSpellBase = unitProperty.UnitBas.SummonerSpellBase;
			((Teammate52)unitProperty.UnitBas).targetWand = null;
			unitProperty.AnnouncedDeath();
			summonerSpellBase.PoolRecycle();
			ettMgr.World.GetOrCreateSystemManaged<EndSpellSimulationEntityCommandBufferSystem>().CreateCommandBuffer().DestroyEntity(wand.PassiveWandSpiritEntity);
			MiniPool.RecycleGO(autoWandList[wand].gameObject);
			wand.PassiveWandSpiritEntity = Entity.Null;
			autoWandList.Remove(wand);
			if (wand == SelectedWand)
			{
				wand.Display_Show();
			}
		}
	}

	public void CancelWandAutoSpell(Wand wand)
	{
		if (wand != null)
		{
			if (wand.passiveRuneHammerEnable)
			{
				wand.ClearAutoSpell(typeof(Spell4013RuneHammerData));
			}
			if (wand.passiveLaserCrystalEnable)
			{
				wand.ClearAutoSpell(typeof(Spell4014LaserCrystalData));
			}
			if (wand.passiveBiAnBladeEnable)
			{
				wand.ClearAutoSpell(typeof(Spell4019BiAnBladeData));
			}
		}
	}

	public void AddExtraWand(WandConfig wandCfg, bool fullMp = false)
	{
		Inst.BaData.wandCfgs.Add(wandCfg);
		Inst.BaData.wandMaxCount++;
		Wand component = UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Mixed/Wand"), PlayerCtrller.tsf_WandPoint.position, Quaternion.identity, PlayerCtrller.tsf_WandPoint).GetComponent<Wand>();
		component.transform.localRotation = Quaternion.identity;
		Wands.Add(component);
		component.Initialize(Wands.Count - 1);
		UIPlayerDataMgr.Inst.WandReset();
		if (component.WandCfg != null)
		{
			component.ResetAndRecheck();
		}
		if (component.passiveAutoWand)
		{
			component.Display_Hide();
		}
		if (fullMp)
		{
			component.CurrentMP = component.MaxMP;
		}
	}

	public void WandLimitChange(int value)
	{
		if (value == 0)
		{
			Debug.LogError("为什么改变法杖上限为0");
			return;
		}
		if (value > 0)
		{
			for (int i = 0; i < value; i++)
			{
				AddExtraWand(null);
			}
		}
		else
		{
			for (int j = 0; j < -value; j++)
			{
				if (BaData.wandMaxCount <= 1)
				{
					break;
				}
				int num = BaData.wandMaxCount - 1;
				if (num < 0 || num >= BaData.wandCfgs.Count || num >= Wands.Count)
				{
					break;
				}
				if (BaData.wandCfgs[num] != null)
				{
					DropWand(num, spawnOnGround: true);
				}
				BaData.wandMaxCount--;
				BaData.wandCfgs.RemoveAt(num);
				Wand wand = Wands[num];
				Wands.RemoveAt(num);
				if (wand != null)
				{
					UnityEngine.Object.Destroy(wand.gameObject);
				}
			}
		}
		if (SelectedWandIndex >= Wands.Count || SelectedWand == null)
		{
			SelectedWandIndex = -1;
			for (int k = 0; k < Wands.Count; k++)
			{
				if (CanSelectWand(k))
				{
					WandSelect(k);
					break;
				}
			}
		}
		UIPlayerDataMgr.Inst.WandReset();
	}

	private void LateUpdate()
	{
		if (!DataMgr.selectedWorldData.FindSet5 && !(UICampMgr.Inst != null) && autoWandList.Count >= 3)
		{
			DataMgr.selectedWorldData.SetFindSet5();
		}
	}

	public bool TryGetPlayerPpt(out UnitProperty_Dots playerPpt)
	{
		if (PlayerEtt == Entity.Null || PlayerCtrller == null)
		{
			playerPpt = default(UnitProperty_Dots);
			return false;
		}
		if (ettMgr.HasComponent<UnitProperty_Dots>(PlayerEtt))
		{
			playerPpt = ettMgr.GetComponentData<UnitProperty_Dots>(PlayerEtt);
			if (playerPpt.isInitialed)
			{
				return true;
			}
			return false;
		}
		playerPpt = default(UnitProperty_Dots);
		return false;
	}

	public bool TryGetPlayerCtrller(out PlayerController_Dots playerCtrller)
	{
		if (PlayerEtt == Entity.Null)
		{
			playerCtrller = default(PlayerController_Dots);
			return false;
		}
		if (ettMgr.HasComponent<PlayerController_Dots>(PlayerEtt))
		{
			playerCtrller = ettMgr.GetComponentData<PlayerController_Dots>(PlayerEtt);
			return true;
		}
		playerCtrller = default(PlayerController_Dots);
		return false;
	}

	public void WandRecreate(bool rebuildUI = false, bool cancelAutoWand = true)
	{
		if (cancelAutoWand)
		{
			Wands.Where((Wand e) => (object)e != null && e.WandCfg != null && e.passiveAutoWand).Action(CancelAutoControlWand);
		}
		Wands.Clear();
		PlayerCtrller.tsf_WandPoint.DestroyAllChild();
		for (int i = 0; i < BaData.wandMaxCount; i++)
		{
			Wand component = UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Mixed/Wand"), PlayerCtrller.tsf_WandPoint.position, Quaternion.identity, PlayerCtrller.tsf_WandPoint).GetComponent<Wand>();
			component.transform.localRotation = Quaternion.identity;
			Wands.Add(component);
		}
		for (int j = 0; j < Wands.Count; j++)
		{
			Wands[j].Initialize(j);
		}
		if (rebuildUI)
		{
			UIPlayerDataMgr.Inst.WandReset();
		}
		foreach (Wand wand in Wands)
		{
			if (wand.WandCfg != null)
			{
				wand.ResetAndRecheck();
			}
			if (wand.passiveAutoWand)
			{
				wand.Display_Hide();
			}
		}
		PlayerCtrller.myPpt.RegetSR();
	}

	public void WandRemoveSpellTypePassive()
	{
		foreach (Wand wand in Wands)
		{
			CancelWandAutoSpell(wand);
		}
	}

	public void WandReset(int index, WandConfig cfg)
	{
		if (index < 0 || index >= BaData.wandMaxCount)
		{
			Debug.LogError("!");
			return;
		}
		Wand wand = Wands[index];
		if (((object)wand != null && wand.WandCfg != null) || Wands[index].WandCfg != null)
		{
			CancelWandAutoSpell(Wands[index]);
		}
		wand = Wands[index];
		if ((object)wand != null && wand.passiveAutoWand && wand.WandCfg != null)
		{
			CancelAutoControlWand(Wands[index]);
		}
		BaData.wandCfgs[index] = cfg;
		UIPlayerDataMgr.Inst.WandUpdate(index);
		Wands[index].Display_UpdateShowOrHide();
		Wands[index].ResetAndRecheck();
		if (Wands[index].passiveAutoWand)
		{
			Wands[index].Display_Hide();
		}
		if (SelectedWandIndex == -1)
		{
			for (int i = 0; i < Wands.Count; i++)
			{
				if (Wands[i].WandCfg != null && !Wands[i].passiveAutoWand)
				{
					WandSelect(i);
					break;
				}
			}
		}
		else
		{
			if (SelectedWandCfg != null)
			{
				return;
			}
			for (int j = 0; j < Wands.Count; j++)
			{
				if (Wands[j].WandCfg != null && !Wands[j].passiveAutoWand)
				{
					WandSelect(j);
					return;
				}
			}
			SelectedWandIndex = -1;
		}
	}

	public void WandRefreshDataByIndex(int index)
	{
		if (index < 0 || index >= BaData.wandMaxCount)
		{
			Debug.LogError("!");
			return;
		}
		Wand wand = Wands[index];
		if (((object)wand != null && wand.WandCfg != null) || Wands[index].WandCfg != null)
		{
			CancelWandAutoSpell(Wands[index]);
		}
		wand = Wands[index];
		if ((object)wand != null && wand.passiveAutoWand && wand.WandCfg != null)
		{
			CancelAutoControlWand(Wands[index]);
		}
		UIPlayerDataMgr.Inst.WandUpdate(index);
		Wands[index].Display_UpdateShowOrHide();
		Wands[index].ResetAndRecheck(refreshAutoSpells: false);
		if (Wands[index].passiveAutoWand)
		{
			Wands[index].Display_Hide();
		}
		if (SelectedWandIndex == -1)
		{
			for (int i = 0; i < Wands.Count; i++)
			{
				if (Wands[i].WandCfg != null && !Wands[i].passiveAutoWand)
				{
					WandSelect(i);
					break;
				}
			}
		}
		else
		{
			if (SelectedWandCfg != null)
			{
				return;
			}
			for (int j = 0; j < Wands.Count; j++)
			{
				if (Wands[j].WandCfg != null && !Wands[j].passiveAutoWand)
				{
					WandSelect(j);
					return;
				}
			}
			SelectedWandIndex = -1;
		}
	}

	public void WandSetConfigWithoutRefresh(int index, WandConfig cfg)
	{
		if (index < 0 || index >= BaData.wandMaxCount)
		{
			Debug.LogError("!");
		}
		else
		{
			BaData.wandCfgs[index] = cfg;
		}
	}

	public bool CanSelectWand(int wandIndex)
	{
		if (Wands.IndexOutRange(wandIndex))
		{
			return false;
		}
		Wand wand = Wands[wandIndex];
		if ((object)wand == null || wand.WandCfg == null)
		{
			return false;
		}
		return true;
	}

	public bool WandSelect(int index)
	{
		if (!CanSelectWand(index))
		{
			Debug.LogWarning($"{index} 号法杖不能手持");
			return false;
		}
		if (SelectedWandIndex == index)
		{
			return false;
		}
		if (SelectedWandIndex != -1)
		{
			SEMgr.Inst.wandChange.PlaySE();
		}
		if (SelectedWand != null)
		{
			if (SelectedWand.passiveChargeEnable && SelectedWand.IsCharging)
			{
				SelectedWand.ReleaseCharge();
				PlayerCtrller.WandChargeEffect(chargeStart: false);
			}
			SelectedWand.Display_Hide();
		}
		SelectedWandIndex = index;
		if (!SelectedWand.passiveAutoWand)
		{
			SelectedWand.Display_Show();
		}
		PlayerCtrller.CastLockUnRegister();
		UIPlayerDataMgr.Inst.UpdateMP();
		return true;
	}

	public void WandSelectOffset(int offset)
	{
		if (CanSelectWandCount == 0)
		{
			SelectedWandIndex = -1;
			return;
		}
		if (CanSelectWandCount == 1)
		{
			for (int i = 0; i < Wands.Count; i++)
			{
				if (CanSelectWand(i))
				{
					WandSelect(i);
					break;
				}
			}
			return;
		}
		int num = SelectedWandIndex;
		do
		{
			num += offset;
			if (num < 0)
			{
				num = Wands.Count - 1;
			}
			if (num >= Wands.Count)
			{
				num = 0;
			}
		}
		while (!CanSelectWand(num));
		WandSelect(num);
	}

	public void WandPickUp(WandConfig cfg, bool fullMp = false)
	{
		if (UIPlayerDataMgr.Inst.uiWand_Drag != null)
		{
			UIPlayerDataMgr.Inst.UIWandEventDragEnd();
		}
		if (!ScriptableObjMgr.Inst.testCtrller.SkipAllStoryMixed && !DataMgr.selectedWorldData.storyMixedFirstPickPostSlotWand && UIBattleMgr.Inst != null && cfg.postSlots.Length != 0)
		{
			UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Storys/Story_FirstPickPostWand")).GetComponent<Story_FirstPickPostWand>().Initialize(cfg);
		}
		int pickWandIndex = GetPickWandIndex();
		if (Wands[pickWandIndex] == null || Wands[pickWandIndex].WandCfg == null)
		{
			SetWand(pickWandIndex, cfg);
		}
		else
		{
			ReplaceWand(pickWandIndex, cfg);
		}
		if (fullMp)
		{
			Wands[pickWandIndex].CurrentMP = Wands[pickWandIndex].MaxMP;
		}
		if (SelectedWand == null || SelectedWand.WandCfg == null)
		{
			WandSelect(pickWandIndex);
		}
		UIPlayerDataMgr.Inst.WandUpdate(pickWandIndex);
		TopUI.inst.wandImage.sprite = ABResources.LoadAsset<Sprite>(WandConfig.dic[Inst.SelectedWandCfg.id].GetIconPath());
	}

	public int GetPickWandIndex()
	{
		for (int i = 0; i < Wands.Count; i++)
		{
			if (Wands[i].WandCfg == null)
			{
				return i;
			}
		}
		if (SelectedWandIndex >= 0)
		{
			return SelectedWandIndex;
		}
		return 0;
	}

	public void ReplaceWand(int wandIndex, WandConfig cfg)
	{
		if (Wands.IndexOutRange(wandIndex).LogIf($"{wandIndex} 号法杖超出索引"))
		{
			return;
		}
		if (Wands[wandIndex].WandCfg == null)
		{
			Debug.LogWarning("不能替换空位置的法杖，对于空的法杖位置可以直接 SetWand");
			return;
		}
		SlotData[] array = PopWandSpells(wandIndex);
		DropWand(wandIndex, spawnOnGround: true);
		SetWand(wandIndex, cfg);
		SlotData[] array2 = array;
		foreach (SlotData slotData in array2)
		{
			if (!PushSlotDataIntoWand(slotData, wandIndex, WandSlotType.Normal) && !PushSlotDataIntoWand(slotData, wandIndex, WandSlotType.Post))
			{
				SpellPick(slotData);
			}
		}
		if (array.Length != 0)
		{
			Wands[wandIndex].ResetAndRecheck();
		}
	}

	public SlotData[] PopWandSpells(int wandIndex)
	{
		List<SlotData> list = new List<SlotData>();
		if (Wands.IndexOutRange(wandIndex).LogIf($"{wandIndex} 号法杖超出索引"))
		{
			return list.ToArray();
		}
		WandConfig wandCfg = Wands[wandIndex].WandCfg;
		if (wandCfg == null)
		{
			Debug.LogWarning($"{wandIndex} 号位置没有法杖");
			return list.ToArray();
		}
		for (int i = 0; i < wandCfg.normalSlots.Length; i++)
		{
			if (!wandCfg.IsSlotLock(WandSlotType.Normal, i))
			{
				SlotData slotData = wandCfg.normalSlots[i];
				if (slotData != null && !slotData.isSealSlot)
				{
					list.Add(wandCfg.normalSlots[i]);
					wandCfg.normalSlots.Bag_RemoveSlot(i);
				}
			}
		}
		for (int j = 0; j < wandCfg.postSlots.Length; j++)
		{
			if (!wandCfg.IsSlotLock(WandSlotType.Post, j))
			{
				SlotData slotData = wandCfg.postSlots[j];
				if (slotData != null && !slotData.isSealSlot)
				{
					list.Add(wandCfg.postSlots[j]);
					wandCfg.postSlots.Bag_RemoveSlot(j);
				}
			}
		}
		return list.ToArray();
	}

	public void DropWand(int wandIndex, bool spawnOnGround)
	{
		if (Wands.IndexOutRange(wandIndex).LogIf($"{wandIndex} 号法杖超出索引，不能丢弃"))
		{
			return;
		}
		Wand wand = Wands[wandIndex];
		WandConfig wandCfg = wand.WandCfg;
		if (wandCfg == null)
		{
			Debug.LogWarning($"{wandIndex} 号位置没有法杖，不能丢弃");
			return;
		}
		wand.Initialize(wandIndex);
		wand.ResetAndRecheck();
		wand.Display_UpdateShowOrHide();
		WandCheckSlotCount(wandIndex);
		if (wand.passiveAutoWand)
		{
			CancelAutoControlWand(wand);
		}
		CancelWandAutoSpell(wand);
		wand.ResetWandSlotState();
		BaData.wandCfgs[wandIndex] = null;
		UIPlayerDataMgr.Inst.WandUpdate(wandIndex);
		if (spawnOnGround)
		{
			QuickCreateSystem.Inst.CreateItem(LevelMgr.Inst.CurrentRoomMapPos, wandCfg, Tool2D.IgnoreZPoint(PlayerPoint));
		}
	}

	public void SetWand(int wandIndex, WandConfig cfg)
	{
		if (!(cfg == null).LogIf("SetWand 不能传入空 WandConfig，如果想要删除法杖应该调用 DropWand") && !Wands.IndexOutRange(wandIndex).LogIf($"{wandIndex} 超出法杖索引，不能替换法杖"))
		{
			Wand wand = Wands[wandIndex];
			BaData.wandCfgs[wandIndex] = cfg;
			wand.Initialize(wandIndex);
			wand.Display_UpdateShowOrHide();
			WandCheckSlotCount(wandIndex);
			wand.ResetAndRecheck();
			UIPlayerDataMgr.Inst.WandUpdate(wandIndex);
		}
	}

	public void AllWandFullMP(float currentMPRatio = 1f)
	{
		if (currentMPRatio > 1f)
		{
			Debug.LogWarning("回魔系数不应该大于1");
			currentMPRatio = 1f;
		}
		else if (currentMPRatio < 0f)
		{
			Debug.LogWarning("回魔系数不应该小于0");
			currentMPRatio = 0f;
		}
		foreach (Wand item in Wands.Where((Wand t) => t.WandCfg != null))
		{
			item.CurrentMP = item.MaxMP * currentMPRatio;
		}
	}

	public List<SlotData> GetAllWandPassiveAllFieldEnhanceSharedSpell()
	{
		List<SlotData> list = new List<SlotData>();
		foreach (Wand item in Wands.Where((Wand wand) => wand != null && wand.WandCfg != null))
		{
			list.AddRange(item.GetWandAllFieldEnhanceSpell());
		}
		return list;
	}

	public bool SpellHasSplitEffect(SlotData[] data)
	{
		foreach (SlotData slotData in data)
		{
			if (slotData != null && slotData.GetFinalConfig().abilityType == SpellAbilityType.SpellSplit)
			{
				return true;
			}
		}
		return false;
	}

	public bool WandCheckSlotCount(int index)
	{
		if (BaData.wandCfgs == null || BaData.wandCfgs[index] == null)
		{
			return false;
		}
		Wand wand = Wands[index];
		bool flag = false;
		bool flag2 = false;
		int startIndex = 0;
		if (wand.IsTransIntoPostDataChanged())
		{
			flag2 = true;
			flag = true;
			wand.PassiveTransPostBackToNormalSlot();
			wand.PassiveRemoveTransSlotFromPostSlot();
		}
		int num = WandConfig.dic[BaData.wandCfgs[index].id].normalSlots.Length;
		if (ItemCtrller.relicCfg_WandAddSlot != null)
		{
			num += ItemCtrller.relicCfg_WandAddSlot.int1.result;
		}
		if (ItemCtrller.relicCfg_LessWandMoreSlot != null)
		{
			num += ItemCtrller.relicCfg_LessWandMoreSlot.int2.result;
		}
		if (index >= 0 && Wands.Count > 0 && Wands[index].ExtraNormalSlot > 0)
		{
			num += Wands[index].ExtraNormalSlot;
		}
		if (index >= 0 && Wands.Count > 0 && Wands[index].WandCfg.transIntoPostslotData.Length != 0 && !flag2)
		{
			num -= Wands[index].WandCfg.transIntoPostslotData.Length;
		}
		num += GetAllWandPassiveAllFieldEnhanceSharedSpell().Count;
		if (BaData.wandCfgs[index].normalSlots.Length < num)
		{
			SlotData[] normalSlots = BaData.wandCfgs[index].normalSlots;
			BaData.wandCfgs[index].normalSlots = new SlotData[num];
			for (int i = 0; i < BaData.wandCfgs[index].normalSlots.Length; i++)
			{
				if (i < normalSlots.Length)
				{
					BaData.wandCfgs[index].normalSlots[i] = normalSlots[i];
				}
			}
			bool[] normalSlotIsLock = BaData.wandCfgs[index].normalSlotIsLock;
			BaData.wandCfgs[index].normalSlotIsLock = new bool[num];
			for (int j = 0; j < BaData.wandCfgs[index].normalSlotIsLock.Length; j++)
			{
				if (j < normalSlotIsLock.Length)
				{
					BaData.wandCfgs[index].normalSlotIsLock[j] = normalSlotIsLock[j];
				}
				else
				{
					BaData.wandCfgs[index].normalSlotIsLock[j] = false;
				}
			}
			flag = true;
		}
		else if (BaData.wandCfgs[index].normalSlots.Length > num)
		{
			foreach (SlotData item in Wands[index].ResizeSlots(WandSlotType.Normal, num))
			{
				GetValidBagSlotIndex(item, ref startIndex);
				if (startIndex >= BaData.bagSpellDatas.Count || startIndex == -1)
				{
					SpawnSpellToGround(item);
				}
				else
				{
					SpellPick(item);
				}
			}
			flag = true;
		}
		num = WandConfig.dic[BaData.wandCfgs[index].id].postSlots.Length;
		if (index >= 0 && Wands.Count > 0 && Wands[index].ExtraPostSlot > 0)
		{
			num += Wands[index].ExtraPostSlot;
		}
		if (index >= 0 && Wands.Count > 0 && Wands[index].WandCfg.transIntoPostslotData.Length != 0 && !flag2)
		{
			num += Wands[index].WandCfg.transIntoPostslotData.Length;
		}
		if (BaData.wandCfgs[index].postSlots.Length < num)
		{
			SlotData[] postSlots = BaData.wandCfgs[index].postSlots;
			BaData.wandCfgs[index].postSlots = new SlotData[num];
			for (int k = 0; k < BaData.wandCfgs[index].postSlots.Length; k++)
			{
				if (k < postSlots.Length)
				{
					BaData.wandCfgs[index].postSlots[k] = postSlots[k];
				}
			}
			bool[] postSlotIsLock = BaData.wandCfgs[index].postSlotIsLock;
			BaData.wandCfgs[index].postSlotIsLock = new bool[num];
			for (int l = 0; l < BaData.wandCfgs[index].postSlotIsLock.Length; l++)
			{
				if (l < postSlotIsLock.Length)
				{
					BaData.wandCfgs[index].postSlotIsLock[l] = postSlotIsLock[l];
				}
				else
				{
					BaData.wandCfgs[index].postSlotIsLock[l] = false;
				}
			}
			flag = true;
		}
		else if (BaData.wandCfgs[index].postSlots.Length > num)
		{
			foreach (SlotData item2 in Wands[index].ResizeSlots(WandSlotType.Post, num))
			{
				GetValidBagSlotIndex(item2, ref startIndex);
				if (startIndex >= BaData.bagSpellDatas.Count || startIndex == -1)
				{
					SpawnSpellToGround(item2);
				}
				else
				{
					SpellPick(item2);
				}
			}
			flag = true;
		}
		if (flag2)
		{
			wand.UpdatePassiveTransIntoPostSlotData();
			wand.PassiveRemoveTransSlotFromNormalSlot();
			wand.PassiveAddTransSlotIntoPostSlot();
		}
		if (flag)
		{
			wand.CheckSpellListForManaTendrilEffect(WandSlotType.Normal);
			wand.CheckSpellListForManaTendrilEffect(WandSlotType.Post);
			wand.CheckWandManaRelatePassiveEffect();
			UIPlayerDataMgr.Inst.WandUpdate(index);
		}
		return flag;
	}

	public float GetPostSlotChargeEfficiency(WandConfig targetConfig)
	{
		float num = 1f;
		if (ItemCtrller.relicCfg_PostSlotMoreEfficiency != null)
		{
			num += (float)ItemCtrller.relicCfg_PostSlotMoreEfficiency.int1.result / 100f;
		}
		num += Inst.PostSlotChargeRatioFromWandAbility() - 1f;
		foreach (Wand wand in Wands)
		{
			if (wand.WandCfg != null && wand.WandCfg == targetConfig)
			{
				return num + wand.GetManaToPostChargeEffect();
			}
		}
		return num;
	}

	public bool CanSpellPick(SlotData spell)
	{
		int slotCost = SpellConfig.dic[spell.id].slotCost;
		return BaData.bagSpellDatas.ToArray().Bag_SpaceCount() >= slotCost;
	}

	public void SpawnSpellToGround(SlotData item)
	{
		QuickCreateSystem.Inst.CreateItem(LevelMgr.Inst.CurrentRoomMapPos, new ItemInfo(item), Tool2D.GetNavMeshPointIngoreZ(PlayerPoint) + Tool2D.GetDir() * 0.1f);
	}

	public void SpellPick(SlotData spellData)
	{
		SlotData[] array = BaData.bagSpellDatas.ToArray();
		while (!CanSpellPick(spellData))
		{
			SlotData slotData = array.Bag_PopLast(new bool[array.Length]);
			BaData.bagSpellDatas = array.ToList();
			if (slotData == null)
			{
				break;
			}
			SpawnSpellToGround(slotData);
			int num = array.Bag_GetNextSpell(-1);
			if (num >= 0)
			{
				array.Bag_PushToRight(new bool[array.Length], num);
			}
		}
		BaData.bagSpellDatas = array.ToList();
		PushSlotDataIntoBag(spellData);
	}

	public bool CanBagSpellChange(int index, SlotData data)
	{
		return BaData.bagSpellDatas.ToArray().Bag_CanSetSlotWithPush(new bool[BaData.bagSpellDatas.Count], data, index);
	}

	public void BagSpellChange(int spellIndex, [CanBeNull] SlotData newData)
	{
		if (newData == null)
		{
			Slot_RemoveBagSlot(spellIndex);
		}
		else
		{
			SlotData[] array = BaData.bagSpellDatas.ToArray();
			if (array[spellIndex] != null)
			{
				array.Bag_RemoveSlot(spellIndex);
			}
			array.Bag_SetSlotWithPush(new bool[array.Length], newData, spellIndex);
			BaData.bagSpellDatas = array.ToList();
		}
		UIPlayerDataMgr.Inst.UpdateBag();
	}

	public bool CanChangeInWandSpell(int wandIndex, WandSlotType slotType, int spellIndex, SlotData data)
	{
		SlotData[] slotsData = Wands[wandIndex].WandCfg.GetSlotsData(slotType);
		bool[] slotsLockState = Wands[wandIndex].WandCfg.GetSlotsLockState(slotType);
		return slotsData.Bag_CanSetSlotWithPush(slotsLockState, data, spellIndex);
	}

	public void ChangeWandSpell(int wandIndex, WandSlotType slotType, int spellIndex, SlotData newData)
	{
		if (newData == null)
		{
			Slot_RemoveWandSlot(wandIndex, slotType, spellIndex);
		}
		else
		{
			Wand wand = Wands[wandIndex];
			SlotData[] slotsData = wand.WandCfg.GetSlotsData(slotType);
			if (slotsData[spellIndex] != null)
			{
				slotsData.Bag_RemoveSlot(spellIndex);
			}
			bool[] slotsLockState = wand.WandCfg.GetSlotsLockState(slotType);
			_Slot_Set(slotsData, slotsLockState, spellIndex, newData);
			wand.ResetAndRecheck();
		}
		UIPlayerDataMgr.Inst.WandUpdate(wandIndex);
	}

	public void ChangeWandSpell(int wandIndex, int allIndex, SlotData newData)
	{
		if (allIndex < BaData.wandCfgs[wandIndex].normalSlots.Length)
		{
			ChangeWandSpell(wandIndex, WandSlotType.Normal, allIndex, newData);
		}
		else if (allIndex < BaData.wandCfgs[wandIndex].normalSlots.Length + BaData.wandCfgs[wandIndex].postSlots.Length)
		{
			ChangeWandSpell(wandIndex, WandSlotType.Post, allIndex - BaData.wandCfgs[wandIndex].normalSlots.Length, newData);
		}
		else
		{
			Debug.LogError("不在范围内！！！");
		}
	}

	public (WandSlotType type, int indexInTheType) WandSlotIndex2SlotType(int wandIndex, int allIndex)
	{
		if (allIndex < BaData.wandCfgs[wandIndex].normalSlots.Length)
		{
			return (WandSlotType.Normal, allIndex);
		}
		if (allIndex < BaData.wandCfgs[wandIndex].normalSlots.Length + BaData.wandCfgs[wandIndex].postSlots.Length)
		{
			return (WandSlotType.Post, allIndex - BaData.wandCfgs[wandIndex].normalSlots.Length);
		}
		throw new Exception("不在范围内！！！");
	}

	public void GetValidBagSlotIndex(SlotData checkSlot, ref int startIndex)
	{
		startIndex = -1;
		if (checkSlot == null)
		{
			for (int i = 0; i < BaData.bagSpellDatas.Count; i++)
			{
				if (BaData.bagSpellDatas[i] == null)
				{
					startIndex = i;
					break;
				}
			}
			return;
		}
		int slotCost = SpellConfig.dic[checkSlot.id].slotCost;
		int num = 0;
		for (int j = 0; j < BaData.bagSpellDatas.Count; j++)
		{
			if (startIndex == -1)
			{
				startIndex = j;
			}
			if (BaData.bagSpellDatas[j] == null)
			{
				num++;
				if (num >= slotCost)
				{
					break;
				}
			}
			else
			{
				startIndex = -1;
				num = 0;
			}
		}
	}

	public bool CheckIfSpellOverSizeToPutInWand(WandConfig targetcfg, WandSlotType type, int id)
	{
		if (targetcfg == null)
		{
			Debug.Log("目标法杖cfg为空 这不对");
			return false;
		}
		int slotCost = SpellConfig.dic[id].slotCost;
		SlotData[] array = null;
		bool[] array2 = null;
		int num = 0;
		switch (type)
		{
		case WandSlotType.Normal:
			array = targetcfg.normalSlots;
			array2 = targetcfg.normalSlotIsLock;
			break;
		case WandSlotType.Post:
			array = targetcfg.postSlots;
			array2 = targetcfg.postSlotIsLock;
			break;
		}
		if (array == null || array.Length < slotCost)
		{
			return false;
		}
		for (int i = 0; i < array.Length; i++)
		{
			num = ((array[i] == null || !array2[i]) ? (num + 1) : 0);
			if (num >= slotCost)
			{
				return true;
			}
		}
		return false;
	}

	public void PushSlotDataIntoBag(SlotData data, int index)
	{
		if (data == null)
		{
			SlotData[] array = BaData.bagSpellDatas.ToArray();
			array.Bag_RemoveSlot(index);
			BaData.bagSpellDatas = array.ToList();
			return;
		}
		if (BaData.bagSpellDatas.Contains(data))
		{
			int num = BaData.bagSpellDatas.IndexOf(data);
			SlotData slotData = BaData.bagSpellDatas[index];
			int b = -1;
			if (slotData != null)
			{
				if (slotData.isSealSlot)
				{
					slotData = slotData.sealSlotOwner;
					b = BaData.bagSpellDatas.IndexOf(slotData);
				}
				else
				{
					slotData = BaData.bagSpellDatas[index];
					b = index;
				}
			}
			SlotData[] array2 = BaData.bagSpellDatas.ToArray();
			if (slotData != null)
			{
				array2.Bag_SwapSlot(new bool[array2.Length], num, b);
			}
			else
			{
				array2.Bag_RemoveSlot(num);
				array2.Bag_SetSlotWithPush(new bool[array2.Length], data, index);
			}
			BaData.bagSpellDatas = array2.ToList();
		}
		else
		{
			SlotData[] array3 = BaData.bagSpellDatas.ToArray();
			array3.Bag_SetSlotWithPush(new bool[array3.Length], data, index);
			BaData.bagSpellDatas = array3.ToList();
		}
		UIPlayerDataMgr.Inst.UpdateBag();
	}

	public void PushSlotDataIntoBag(SlotData data)
	{
		data.mimicSpellID = 0;
		int firstCanPushSlotDataIntoBagIndex = GetFirstCanPushSlotDataIntoBagIndex(data);
		if (firstCanPushSlotDataIntoBagIndex >= 0)
		{
			PushSlotDataIntoBag(data, firstCanPushSlotDataIntoBagIndex);
		}
	}

	public bool PushSlotDataIntoWand(SlotData data, int wandIndex, WandSlotType slotType)
	{
		SlotData[] slotsData = Wands[wandIndex].WandCfg.GetSlotsData(slotType);
		bool[] slotsLockState = Wands[wandIndex].WandCfg.GetSlotsLockState(slotType);
		int num = slotsData.Bag_GetFirstNullSlotIndex();
		if (num < 0)
		{
			return false;
		}
		if (!slotsData.Bag_CanSetSlotWithPush(slotsLockState, data, num))
		{
			return false;
		}
		slotsData.Bag_SetSlotWithPush(slotsLockState, data, num);
		return true;
	}

	public int GetFirstCanPushSlotDataIntoBagIndex(SlotData data)
	{
		SlotData[] array = BaData.bagSpellDatas.ToArray();
		for (int i = 0; i < BaData.bagSpellDatas.Count; i++)
		{
			if (BaData.bagSpellDatas[i] == null && array.Bag_CanSetSpell(new bool[array.Length], data, i))
			{
				return i;
			}
		}
		for (int j = 0; j < BaData.bagSpellDatas.Count; j++)
		{
			if (BaData.bagSpellDatas[j] == null && array.Bag_CanSetSlotWithPush(new bool[array.Length], data, j))
			{
				return j;
			}
		}
		return -1;
	}

	public SlotData CreateSealedSlotData(SlotData owner)
	{
		if (owner == null)
		{
			return null;
		}
		return new SlotData
		{
			sealSlotOwner = owner
		};
	}

	private void CorrectMoveAnimatorSpeed()
	{
	}

	public int ClampValue(int current, int changed)
	{
		long num = (long)current + (long)changed;
		if (num > int.MaxValue)
		{
			num = 2147483647L;
		}
		if (num < 0)
		{
			Debug.LogError("为什么会是负数？");
			num = 0L;
		}
		return (int)num;
	}

	public void ChangeCoin(int value)
	{
		BaData.coinCount = ClampValue(BaData.coinCount, value);
		UIPlayerDataMgr.Inst.UpdateCoin();
		if (value > 0 && ItemCtrller.relicCfg_CoinHeal != null)
		{
			UnitDotsSyncSystem.UnitRecoveryHP(PlayerEtt, ItemCtrller.relicCfg_CoinHeal.int1.result * value, ettMgr);
		}
		if (BaData.coinCount >= 1000)
		{
			SteamAchievementMgr.UnlockAndUpload(SteamAchievementType.Coin1000);
		}
		EventMgr.coinCountChange?.Invoke();
	}

	public void ChangeKey(int value, TextFloatQueueType queueType = TextFloatQueueType.None)
	{
		if (value == 0)
		{
			return;
		}
		BaData.keyCount = ClampValue(BaData.keyCount, value);
		switch (queueType)
		{
		case TextFloatQueueType.DirectFloat:
			if (GameMgr.IsSupportVFX)
			{
				if (value > 0)
				{
					QuickCreateSystem.Inst.CreateTextFloatVFX(value, UITextFloatType.GetKey, PlayerPoint);
				}
				else
				{
					QuickCreateSystem.Inst.CreateTextFloatVFX(value, UITextFloatType.DropKey, PlayerPoint);
				}
			}
			else if (value > 0)
			{
				ObjPoolMgr.Inst.GetUIGO("Prefabs/UI/UITextFloat").GetComponent<UITextFloat>().Initialize("+" + value, UITextFloatType.GetKey, PlayerPoint);
			}
			else
			{
				ObjPoolMgr.Inst.GetUIGO("Prefabs/UI/UITextFloat").GetComponent<UITextFloat>().Initialize(value.ToString(), UITextFloatType.DropKey, PlayerPoint);
			}
			break;
		case TextFloatQueueType.QueueFloat:
			if (value > 0)
			{
				PlayerCtrller.TextFloatQueueAdd("+" + value, UITextFloatType.GetKey);
			}
			else
			{
				PlayerCtrller.TextFloatQueueAdd(value.ToString(), UITextFloatType.DropKey);
			}
			break;
		default:
			Debug.LogError(queueType);
			break;
		case TextFloatQueueType.None:
			break;
		}
		UIPlayerDataMgr.Inst.UpdateKey();
	}

	public void ChangeMagicCrystal(int value)
	{
		DataMgr.selectedWorldData.magicCrystalCount = ClampValue(DataMgr.selectedWorldData.magicCrystalCount, value);
		UIPlayerDataMgr.Inst.UpdateMagicCrystal();
		if (value != 0)
		{
			EventMgr.MagicCrystalChange?.Invoke();
		}
	}

	public void ChangeAncientBlood(int value)
	{
		DataMgr.selectedWorldData.ancientBloodCount = ClampValue(DataMgr.selectedWorldData.ancientBloodCount, value);
		if (value > 0)
		{
			DataMgr.selectedWorldData.hadBlood = true;
		}
		UIPlayerDataMgr.Inst.UpdateAncientBlood();
		if (value != 0)
		{
			EventMgr.AncienBloodChange?.Invoke();
		}
	}

	public void ChangeChaosCore(int value)
	{
		DataMgr.selectedWorldData.chaosCoreCount = ClampValue(DataMgr.selectedWorldData.chaosCoreCount, value);
		if (value > 0)
		{
			DataMgr.selectedWorldData.hadCore = true;
		}
		UIPlayerDataMgr.Inst.UpdateChaosCore();
		if (value != 0)
		{
			EventMgr.ChaosCoreChange?.Invoke();
		}
	}

	public void ChangeGear(int value)
	{
		if ((bool)BattleMgr.Inst)
		{
			BattleMgr.Inst.EndlessCurrentGear += value;
		}
		DataMgr.selectedWorldData.GearCount = ClampValue(DataMgr.selectedWorldData.GearCount, value);
		UIPlayerDataMgr.Inst.UpdateGear();
		if (value != 0)
		{
			EventMgr.GearChange?.Invoke();
		}
	}

	public void ChangeHPMax(float value, TextFloatQueueType queueType = TextFloatQueueType.QueueFloat)
	{
		if (TryGetPlayerPpt(out var playerPpt))
		{
			playerPpt.unitCfg.maxHP += value;
			if (playerPpt.unitCfg.maxHP < 1f)
			{
				playerPpt.unitCfg.maxHP = 1f;
			}
			if (playerPpt.unitCfg.currentHP > playerPpt.unitCfg.maxHP)
			{
				playerPpt.unitCfg.currentHP = playerPpt.unitCfg.maxHP;
			}
			ettMgr.SetComponentData(PlayerEtt, playerPpt);
		}
		UIPlayerDataMgr.Inst.UpdateHP();
		EventMgr.coinCountChange?.Invoke();
		switch (queueType)
		{
		case TextFloatQueueType.DirectFloat:
		{
			string text2 = ((!(value >= 0f)) ? value.ToString("F0") : ("+" + value));
			ObjPoolMgr.Inst.GetUIGO("Prefabs/UI/UITextFloat").GetComponent<UITextFloat>().Initialize(1000505.GetText() + text2, UITextFloatType.Normal, base.transform.position);
			break;
		}
		case TextFloatQueueType.QueueFloat:
		{
			string text = ((!(value >= 0f)) ? value.ToString("F0") : ("+" + value));
			PlayerCtrller.TextFloatQueueAdd(1000505.GetText() + text, UITextFloatType.Normal);
			break;
		}
		default:
			Debug.LogError(queueType);
			break;
		case TextFloatQueueType.None:
			break;
		}
	}

	public void ChangeHPCurrent(float value)
	{
		if (TryGetPlayerPpt(out var playerPpt))
		{
			playerPpt.unitCfg.currentHP += value;
			ettMgr.SetComponentData(PlayerEtt, playerPpt);
		}
		UIPlayerDataMgr.Inst.UpdateHP();
	}

	public void ChangeMPCurrent(int value)
	{
		if (SelectedWandCfg != null)
		{
			SelectedWand.CurrentMP += value;
			if (SelectedWand.CurrentMP > SelectedWand.MaxMP)
			{
				SelectedWand.CurrentMP = SelectedWand.MaxMP;
			}
			if (GameMgr.IsSupportVFX)
			{
				QuickCreateSystem.Inst.CreateTextFloatVFX(value, UITextFloatType.RecoverMP, PlayerPoint);
			}
			else
			{
				ObjPoolMgr.Inst.GetUIGO("Prefabs/UI/UITextFloat").GetComponent<UITextFloat>().Initialize("+" + value, UITextFloatType.RecoverMP, PlayerPoint);
			}
		}
	}

	public void ChangeMPMax(int value, TextFloatQueueType queueType = TextFloatQueueType.QueueFloat)
	{
		BaData.mpMax += value;
		switch (queueType)
		{
		case TextFloatQueueType.DirectFloat:
		{
			string text2 = ((value < 0) ? value.ToString("F0") : ("+" + value));
			ObjPoolMgr.Inst.GetUIGO("Prefabs/UI/UITextFloat").GetComponent<UITextFloat>().Initialize(1000510.GetText() + text2, UITextFloatType.Normal, base.transform.position);
			break;
		}
		case TextFloatQueueType.QueueFloat:
		{
			string text = ((value < 0) ? value.ToString("F0") : ("+" + value));
			PlayerCtrller.TextFloatQueueAdd(1000510.GetText() + text, UITextFloatType.Normal);
			break;
		}
		default:
			Debug.LogError(queueType);
			break;
		case TextFloatQueueType.None:
			break;
		}
	}

	public void ChangeMPRecovery(int value)
	{
		BaData.mpRecovery += value;
	}

	public void ChangeShield(float value, TextFloatQueueType queueType = TextFloatQueueType.QueueFloat)
	{
		if (!TryGetPlayerPpt(out var playerPpt))
		{
			return;
		}
		playerPpt.unitCfg.shield += value;
		if (playerPpt.unitCfg.shield < 0f)
		{
			playerPpt.unitCfg.shield = 0f;
		}
		ettMgr.SetComponentData(PlayerEtt, playerPpt);
		UIPlayerDataMgr.Inst.UpdateShield();
		switch (queueType)
		{
		case TextFloatQueueType.DirectFloat:
			if (GameMgr.IsSupportVFX)
			{
				if (value >= 0f)
				{
					QuickCreateSystem.Inst.CreateTextFloatVFX(value, UITextFloatType.GetShield, PlayerPoint);
				}
				else
				{
					QuickCreateSystem.Inst.CreateTextFloatVFX(value, UITextFloatType.PlayerLostShield, PlayerPoint);
				}
			}
			else if (value > 0f)
			{
				ObjPoolMgr.Inst.GetUIGO("Prefabs/UI/UITextFloat").GetComponent<UITextFloat>().Initialize("+" + value, UITextFloatType.GetShield, PlayerPoint);
			}
			else
			{
				ObjPoolMgr.Inst.GetUIGO("Prefabs/UI/UITextFloat").GetComponent<UITextFloat>().Initialize(value.ToString(), UITextFloatType.PlayerLostShield, PlayerPoint);
			}
			break;
		case TextFloatQueueType.QueueFloat:
			if (value >= 0f)
			{
				PlayerCtrller.TextFloatQueueAdd("+" + value, UITextFloatType.GetShield);
			}
			else
			{
				PlayerCtrller.TextFloatQueueAdd(value.ToString("F0"), UITextFloatType.PlayerLostShield);
			}
			break;
		default:
			Debug.LogError(queueType);
			break;
		case TextFloatQueueType.None:
			break;
		}
		EventMgr.PlayerHPOrShiledChange?.Invoke();
	}

	public void ChangeShieldTemp(float value, TextFloatQueueType queueType = TextFloatQueueType.QueueFloat)
	{
		if (!TryGetPlayerPpt(out var playerPpt))
		{
			return;
		}
		playerPpt.unitCfg.shieldTemp += value;
		if (playerPpt.unitCfg.shieldTemp < 0f)
		{
			playerPpt.unitCfg.shieldTemp = 0f;
		}
		ettMgr.SetComponentData(PlayerEtt, playerPpt);
		UIPlayerDataMgr.Inst.UpdateShieldTemp();
		switch (queueType)
		{
		case TextFloatQueueType.DirectFloat:
			if (value >= 0f)
			{
				ObjPoolMgr.Inst.GetUIGO("Prefabs/UI/UITextFloat").GetComponent<UITextFloat>().Initialize("+" + value, UITextFloatType.GetTempShield, PlayerPoint);
			}
			else
			{
				ObjPoolMgr.Inst.GetUIGO("Prefabs/UI/UITextFloat").GetComponent<UITextFloat>().Initialize(value.ToString("F0"), UITextFloatType.PlayerLostTempShield, PlayerPoint);
			}
			break;
		case TextFloatQueueType.QueueFloat:
			if (value >= 0f)
			{
				PlayerCtrller.TextFloatQueueAdd("+" + value, UITextFloatType.GetTempShield);
			}
			else
			{
				PlayerCtrller.TextFloatQueueAdd(value.ToString("F0"), UITextFloatType.PlayerLostTempShield);
			}
			break;
		default:
			Debug.LogError(queueType);
			break;
		case TextFloatQueueType.None:
			break;
		}
		EventMgr.PlayerHPOrShiledChange?.Invoke();
	}

	public void ChangeBodySize(float value = 1f)
	{
		BaData.bodySize *= value;
		if (BaData.bodySize < 0.0001f)
		{
			BaData.bodySize = 0.0001f;
		}
		PlayerPpt.transform.localScale = Vector3.one * BaData.bodySize;
		CorrectMoveAnimatorSpeed();
		if (ItemCtrller.relic_BlockSpellMono != null)
		{
			ItemCtrller.relic_BlockSpellMono.CorrectDistance();
		}
	}

	public void ChangeKnockbackRatio(float value)
	{
		if (Inst.TryGetPlayerPpt(out var playerPpt))
		{
			playerPpt.unitCfg.knockbackRatio += value;
			ettMgr.SetComponentData(PlayerEtt, playerPpt);
		}
	}

	public void ClearAllRegister()
	{
		if (TryGetPlayerPpt(out var playerPpt))
		{
			playerPpt.ClearAllRegister();
			ettMgr.SetComponentData(PlayerEtt, playerPpt);
		}
		else
		{
			Debug.LogError("没有PlayerPpt");
		}
	}

	public void InvincibleRegister()
	{
		if (TryGetPlayerPpt(out var playerPpt))
		{
			playerPpt.InvincibleRegister();
			ettMgr.SetComponentData(PlayerEtt, playerPpt);
		}
		else
		{
			Debug.LogError("没有PlayerPpt");
		}
	}

	public void InvincibleUnregister()
	{
		if (TryGetPlayerPpt(out var playerPpt))
		{
			playerPpt.InvincibleUnregister();
			ettMgr.SetComponentData(PlayerEtt, playerPpt);
		}
		else
		{
			Debug.LogError("没有PlayerPpt");
		}
	}

	public void FlyRegister()
	{
		if (TryGetPlayerPpt(out var playerPpt))
		{
			playerPpt.FlyRegister();
			ettMgr.SetComponentData(PlayerEtt, playerPpt);
		}
		else
		{
			Debug.LogError("没有PlayerPpt");
		}
	}

	public void FlyUnregister()
	{
		if (TryGetPlayerPpt(out var playerPpt))
		{
			playerPpt.FlyUnregister();
			ettMgr.SetComponentData(PlayerEtt, playerPpt);
		}
		else
		{
			Debug.LogError("没有PlayerPpt");
		}
	}

	public void FlyRegisterWithAllMate()
	{
		FlyRegister();
		for (int i = 0; i < summonsPpts.Count; i++)
		{
			summonsPpts[i].FlyRegisterWithMate();
		}
	}

	public void FlyUnregisterWithAllMate()
	{
		FlyUnregister();
		for (int i = 0; i < summonsPpts.Count; i++)
		{
			summonsPpts[i].FlyUnregisterWithMate();
		}
	}

	public void ImmuneKnockbackRegister()
	{
		if (TryGetPlayerPpt(out var playerPpt))
		{
			playerPpt.ImmuneKnockbackRegister();
			ettMgr.SetComponentData(PlayerEtt, playerPpt);
		}
		else
		{
			Debug.LogError("没有PlayerPpt");
		}
	}

	public void ImmuneKnockbackUnregister()
	{
		if (TryGetPlayerPpt(out var playerPpt))
		{
			playerPpt.ImmuneKnockbackUnregister();
			ettMgr.SetComponentData(PlayerEtt, playerPpt);
		}
		else
		{
			Debug.LogError("没有PlayerPpt");
		}
	}

	public void ImmuneMucusRegister()
	{
		if (TryGetPlayerPpt(out var playerPpt))
		{
			playerPpt.ImmuneMucusRegister();
			ettMgr.SetComponentData(PlayerEtt, playerPpt);
		}
		else
		{
			Debug.LogError("没有PlayerPpt");
		}
	}

	public void ImmuneMucusUnregister()
	{
		if (TryGetPlayerPpt(out var playerPpt))
		{
			playerPpt.ImmuneMucusUnregister();
			ettMgr.SetComponentData(PlayerEtt, playerPpt);
		}
		else
		{
			Debug.LogError("没有PlayerPpt");
		}
	}

	public void ImmuneVenomRegister()
	{
		if (TryGetPlayerPpt(out var playerPpt))
		{
			playerPpt.ImmuneVenomRegister();
			ettMgr.SetComponentData(PlayerEtt, playerPpt);
		}
		else
		{
			Debug.LogError("没有PlayerPpt");
		}
	}

	public void ImmuneVenomUnregister()
	{
		if (TryGetPlayerPpt(out var playerPpt))
		{
			playerPpt.ImmuneVenomUnregister();
			ettMgr.SetComponentData(PlayerEtt, playerPpt);
		}
		else
		{
			Debug.LogError("没有PlayerPpt");
		}
	}

	public void HideAndDisableControl()
	{
		if (!IsHide)
		{
			IsHide = true;
			PlayerT.HideAllChild();
			PlayerCtrller.StopMotion();
			Shadow_Dots componentData = ettMgr.GetComponentData<Shadow_Dots>(PlayerEtt);
			componentData.onHide = true;
			ettMgr.SetComponentData(PlayerEtt, componentData);
			if (ItemCtrller.uiRelic_WarmSnow != null)
			{
				Spell4019BiAnBladeSystem.HideBlade = true;
			}
		}
	}

	public void ShowAndEnableControl()
	{
		if (IsHide)
		{
			IsHide = false;
			PlayerT.ShowAllChild();
			PlayerCtrller.StartMotion();
			Shadow_Dots componentData = ettMgr.GetComponentData<Shadow_Dots>(PlayerEtt);
			componentData.onShow = true;
			ettMgr.SetComponentData(PlayerEtt, componentData);
			if (ItemCtrller.uiRelic_WarmSnow != null)
			{
				Spell4019BiAnBladeSystem.HideBlade = false;
			}
		}
	}

	public (int RedRune, int GreenRune, int BlueRune) GetPlayerRuneCount()
	{
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		foreach (Wand wand in Wands)
		{
			if (!(wand == null) && !wand.IsDestroyed())
			{
				(int, int, int) wandRuneCount = wand.GetWandRuneCount();
				num += wandRuneCount.Item1;
				num2 += wandRuneCount.Item2;
				num3 += wandRuneCount.Item3;
			}
		}
		if (ItemCtrller.uiRelic_RuneWizard != null)
		{
			foreach (RelicConfig relicCfg in BaData.relicCfgs)
			{
				if (relicCfg != null)
				{
					num += relicCfg.RedRunePoint * relicCfg.level;
					num2 += relicCfg.GreenRunePoint * relicCfg.level;
					num3 += relicCfg.BlueRunePoint * relicCfg.level;
				}
			}
		}
		return (num, num2, num3);
	}

	public int GetRuneEffectLevel(int targetRuneCount)
	{
		int num = 0;
		foreach (int item in GameConstManaged.LostCastleRuneLevelThreshold)
		{
			if (item <= targetRuneCount)
			{
				num++;
				continue;
			}
			return num;
		}
		return num;
	}

	public void ForceTriggerGreenRune(float3 targetPos)
	{
		foreach (Wand wand in Wands)
		{
			wand.TrySpawnGreenRuneBall(isForceSpawn: true, targetPos);
		}
	}

	public void SummonsRegister(UnitProperty ppt)
	{
		if (ppt.unitCfg.unitType != UnitType.Teammate && ppt.unitCfg.unitType != UnitType.TeammateNotAttack)
		{
			Debug.LogError(ppt.unitCfg.unitType);
		}
		else
		{
			if (summonsPpts.Contains(ppt) || summonsNotAttackPpts.Contains(ppt))
			{
				return;
			}
			int num = 1000;
			if (ppt.UnitBas.SummonerSpellBase != null && ppt.UnitBas.SummonerSpellBase.spellCfg.summonLimit != 0)
			{
				num = ppt.UnitBas.SummonerSpellBase.spellCfg.summonLimit;
			}
			if (ItemCtrller.relicCfg_SummonLimit != null)
			{
				num *= ItemCtrller.relicCfg_SummonLimit.int1.result;
			}
			if (ItemCtrller.curseCfg_SummonsReduce != null)
			{
				num = Mathf.CeilToInt((float)num / (float)ItemCtrller.curseCfg_SummonsReduce.int1.result);
			}
			if (num > 0)
			{
				List<UnitProperty> list = ((ppt.unitCfg.unitType == UnitType.Teammate) ? summonsPpts : summonsNotAttackPpts);
				UnitProperty unitProperty = null;
				int num2 = 0;
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].UnitBas.SummonerSpellBase.spellCfg.abilityType == ppt.UnitBas.SummonerSpellBase.spellCfg.abilityType && !list[i].isUnitDead)
					{
						if (unitProperty == null)
						{
							unitProperty = list[i];
						}
						num2++;
						if (num2 >= num)
						{
							unitProperty.TeammateAnnounceDeath(new TeammateAnnounceDeathInfo
							{
								isInstanceDeath = false
							});
							break;
						}
					}
				}
			}
			if (ppt.unitCfg.unitType == UnitType.Teammate)
			{
				summonsPpts.Add(ppt);
			}
			else
			{
				summonsNotAttackPpts.Add(ppt);
			}
		}
	}

	public void SummonsUnregister(UnitProperty ppt)
	{
		if (ppt.unitCfg.unitType == UnitType.Teammate)
		{
			summonsPpts.Remove(ppt);
		}
		else if (ppt.unitCfg.unitType == UnitType.TeammateNotAttack)
		{
			summonsNotAttackPpts.Remove(ppt);
		}
		else
		{
			Debug.LogError(ppt.unitCfg.unitType);
		}
	}

	public void SummonsThrough()
	{
		for (int num = summonsPpts.Count - 1; num >= 0; num--)
		{
			summonsPpts[num].UnitBas.SummonsThrough();
		}
		for (int num2 = summonsNotAttackPpts.Count - 1; num2 >= 0; num2--)
		{
			summonsNotAttackPpts[num2].UnitBas.SummonsThrough();
		}
		foreach (KeyValuePair<Wand, UnitProperty> autoWand in autoWandList)
		{
			autoWand.Value.UnitBas.SummonsThrough();
		}
		foreach (Wand item in Wands.Where((Wand e) => e))
		{
			item.ResetAndRecheck();
		}
	}

	public void SummonsAllDead(bool instanceDeath = false, bool clearAllAutoWand = true)
	{
		using EntityQuery entityQuery = ettMgr.CreateEntityQuery(ComponentType.ReadWrite<Spell2007SuicideBugNestData>());
		using NativeArray<Spell2007SuicideBugNestData> nativeArray = entityQuery.ToComponentDataArray<Spell2007SuicideBugNestData>(Allocator.TempJob);
		using NativeArray<Entity> nativeArray2 = entityQuery.ToEntityArray(Allocator.TempJob);
		for (int i = 0; i < nativeArray.Length; i++)
		{
			Spell2007SuicideBugNestData componentData = nativeArray[i];
			componentData.ForbiddenSpawnSuicideBugWhenDestroy = true;
			ettMgr.SetComponentData(nativeArray2[i], componentData);
		}
		using EntityQuery entityQuery2 = ettMgr.CreateEntityQuery(typeof(TeammateData));
		NativeArray<Entity> nativeArray3 = entityQuery2.ToEntityArray(Allocator.Temp);
		foreach (Entity item in nativeArray3)
		{
			ettMgr.SetComponentEnabled<TeammateDeadTag>(item, value: true);
		}
		nativeArray3.Dispose();
	}

	public void SummonsLoseTarget()
	{
		for (int num = summonsPpts.Count - 1; num >= 0; num--)
		{
			summonsPpts[num].UnitBas.LoseTarget();
		}
		for (int num2 = summonsNotAttackPpts.Count - 1; num2 >= 0; num2--)
		{
			summonsNotAttackPpts[num2].UnitBas.LoseTarget();
		}
		foreach (KeyValuePair<Wand, UnitProperty> autoWand in autoWandList)
		{
			autoWand.Value.UnitBas.LoseTarget();
		}
	}

	public UnitProperty GetNearestPpt(Vector3 checkPoint, bool checkWall = false)
	{
		UnitProperty result = null;
		float num = 100000000f;
		if (PlayerCtrller.IsVisible && PlayerPpt.CanBeTarget)
		{
			if (checkWall)
			{
				float num2 = Tool2D.IgnoreZDistanceSqr(PlayerPoint, checkPoint);
				UnityEngine.Ray ray = new UnityEngine.Ray(checkPoint, Tool2D.IgnoreZV2ToV1Normal(PlayerPoint, checkPoint));
				if (Physics.Raycast(ray, out var hitInfo, 100000000f, LayerMask.GetMask("Wall")))
				{
					if (num2 < (ray.origin - hitInfo.point).sqrMagnitude)
					{
						result = PlayerPpt;
						num = num2;
					}
				}
				else
				{
					result = PlayerPpt;
					num = num2;
				}
			}
			else
			{
				result = PlayerPpt;
				num = Tool2D.IgnoreZDistanceSqr(PlayerPoint, checkPoint);
			}
		}
		for (int i = 0; i < summonsPpts.Count; i++)
		{
			if (!summonsPpts[i].CanBeTarget)
			{
				continue;
			}
			float num3 = Tool2D.IgnoreZDistanceSqr(checkPoint, summonsPpts[i].transform.position);
			if (!(num3 < num))
			{
				continue;
			}
			if (checkWall)
			{
				UnityEngine.Ray ray2 = new UnityEngine.Ray(checkPoint, Tool2D.IgnoreZV2ToV1Normal(summonsPpts[i].transform.position, checkPoint));
				if (Physics.Raycast(ray2, out var hitInfo2, num3, LayerMask.GetMask("Wall")))
				{
					if (num3 < (ray2.origin - hitInfo2.point).sqrMagnitude)
					{
						result = summonsPpts[i];
						num = num3;
					}
				}
				else
				{
					result = summonsPpts[i];
					num = num3;
				}
			}
			else
			{
				result = summonsPpts[i];
				num = num3;
			}
		}
		return result;
	}

	public UnitProperty GetNearestPptPlayerFirst(Vector3 checkPoint, bool checkWall = false)
	{
		UnitProperty unitProperty = null;
		float num = 100000000f;
		if (PlayerCtrller.IsVisible && PlayerPpt.CanBeTarget)
		{
			if (checkWall)
			{
				float num2 = Tool2D.IgnoreZDistanceSqr(PlayerPoint, checkPoint);
				UnityEngine.Ray ray = new UnityEngine.Ray(checkPoint, Tool2D.IgnoreZV2ToV1Normal(PlayerPoint, checkPoint));
				if (Physics.Raycast(ray, out var hitInfo, 100000000f, LayerMask.GetMask("Wall")))
				{
					if (num2 < (ray.origin - hitInfo.point).sqrMagnitude)
					{
						unitProperty = PlayerPpt;
						num = num2;
					}
				}
				else
				{
					unitProperty = PlayerPpt;
					num = num2;
				}
			}
			else
			{
				unitProperty = PlayerPpt;
				num = Tool2D.IgnoreZDistanceSqr(PlayerPoint, checkPoint);
			}
		}
		if (unitProperty != null)
		{
			return unitProperty;
		}
		for (int i = 0; i < summonsPpts.Count; i++)
		{
			if (!summonsPpts[i].CanBeTarget)
			{
				continue;
			}
			float num3 = Tool2D.IgnoreZDistanceSqr(checkPoint, summonsPpts[i].transform.position);
			if (!(num3 < num))
			{
				continue;
			}
			if (checkWall)
			{
				UnityEngine.Ray ray2 = new UnityEngine.Ray(checkPoint, Tool2D.IgnoreZV2ToV1Normal(summonsPpts[i].transform.position, checkPoint));
				if (Physics.Raycast(ray2, out var hitInfo2, num3, LayerMask.GetMask("Wall")))
				{
					if (num3 < (ray2.origin - hitInfo2.point).sqrMagnitude)
					{
						unitProperty = summonsPpts[i];
						num = num3;
					}
				}
				else
				{
					unitProperty = summonsPpts[i];
					num = num3;
				}
			}
			else
			{
				unitProperty = summonsPpts[i];
				num = num3;
			}
		}
		return unitProperty;
	}

	public UnitProperty GetRandomPpt()
	{
		if (summonsPpts.Count == 0)
		{
			if (PlayerCtrller.IsVisible && PlayerPpt.CanBeTarget)
			{
				return PlayerPpt;
			}
			return null;
		}
		List<UnitProperty> list = new List<UnitProperty>();
		if (PlayerCtrller.IsVisible && PlayerPpt.CanBeTarget)
		{
			list.Add(PlayerPpt);
		}
		for (int i = 0; i < summonsPpts.Count; i++)
		{
			if (summonsPpts[i].CanBeTarget)
			{
				list.Add(summonsPpts[i]);
			}
		}
		if (list.Count == 0)
		{
			return null;
		}
		return list[UnityEngine.Random.Range(0, list.Count)];
	}

	public UnitProperty GetMinimalAngleTargetablePpt(Vector3 checkPoint, Vector3 fromDir, bool checkWall = false)
	{
		UnitProperty result = null;
		float f = 100000000f;
		if (PlayerCtrller.IsVisible && PlayerPpt.CanBeTarget)
		{
			if (checkWall)
			{
				float num = Tool2D.IgnoreZAngle(fromDir, PlayerPoint - checkPoint);
				if (!Physics.Raycast(new UnityEngine.Ray(checkPoint, Tool2D.IgnoreZV2ToV1Normal(PlayerPoint, checkPoint)), num, LayerMask.GetMask("Wall")))
				{
					result = PlayerPpt;
					f = num;
				}
			}
			else
			{
				result = PlayerPpt;
				f = Tool2D.IgnoreZAngle(fromDir, PlayerPoint - checkPoint);
			}
		}
		for (int i = 0; i < summonsPpts.Count; i++)
		{
			if (!summonsPpts[i].CanBeTarget)
			{
				continue;
			}
			float num2 = Tool2D.IgnoreZAngle(fromDir, summonsPpts[i].transform.position - checkPoint);
			if (!(Mathf.Abs(num2) < Mathf.Abs(f)))
			{
				continue;
			}
			if (checkWall)
			{
				if (!Physics.Raycast(new UnityEngine.Ray(checkPoint, Tool2D.IgnoreZV2ToV1Normal(summonsPpts[i].transform.position, checkPoint)), num2, LayerMask.GetMask("Wall")))
				{
					result = summonsPpts[i];
					f = num2;
				}
			}
			else
			{
				result = summonsPpts[i];
				f = num2;
			}
		}
		return result;
	}

	public void UpdateSkin()
	{
		if (BaData != null)
		{
			PlayerSkinMgr.Inst.SetSkin(PlayerCtrller.SAnima.skeleton, DataMgr.selectedWorldData.playerLook, BaData.relicCfgs, ignoreDisableRelicSkin: false, DataMgr.selectedWorldData.inBattle9);
		}
	}

	public Vector3 GetMousePoint(float z = 0f)
	{
		Vector3 result = Vector3.zero;
		if (GameMgr.IsMobile_Static)
		{
			result = PlayerCtrller.ShootWorldPoint;
		}
		else
		{
			switch (UIMgr.Inst.InputType)
			{
			case PlayerInputType.Keyboard:
				result = Tool2D.IgnoreZPoint(CamController.Inst.cam_Main.ScreenToWorldPoint(Input.mousePosition), z);
				break;
			case PlayerInputType.Gamepad:
				result = ((ControlMgr.Inst.GetControllerType() != ControlMgr.controllertype.SteamDeckKeyBoard) ? PlayerCtrller.ShootWorldPoint : Tool2D.IgnoreZPoint(CamController.Inst.cam_Main.ScreenToWorldPoint(Input.mousePosition), z));
				break;
			}
		}
		return result;
	}

	public int NeedKeyCount()
	{
		if (ItemCtrller.relic_FollowObj_SilverKey != null)
		{
			return 0;
		}
		if (ItemCtrller.curse_IsDoubleLock)
		{
			return 2;
		}
		return 1;
	}

	public bool IsKeyEnough()
	{
		if (KeyCount >= NeedKeyCount())
		{
			return true;
		}
		return false;
	}

	public int GetPotionNum()
	{
		int num = 0;
		if (BaData == null)
		{
			return 0;
		}
		foreach (int potionID in BaData.potionIDs)
		{
			if (potionID != 0)
			{
				num++;
			}
		}
		return num;
	}

	private void _Slot_Move(SlotData[] slots, bool[] locks, int index, int offset)
	{
		while (offset > 0)
		{
			if (!slots.Bag_CanPushToRight(locks, index))
			{
				return;
			}
			slots.Bag_PushToRight(locks, index);
			offset--;
			index++;
		}
		while (offset < 0 && slots.Bag_CanPushToLeft(locks, index))
		{
			slots.Bag_PushToLeft(locks, index);
			offset++;
			index--;
		}
	}

	private void _Slot_SwapSlot(SlotData[] s1, bool[] locks1, SlotData[] s2, bool[] locks2, int i1, int i2)
	{
		int num = i1;
		int num2 = i2;
		i1 = s1.Bag_GetOwnerSlotIndex(i1);
		i2 = s2.Bag_GetOwnerSlotIndex(i2);
		SlotData slotData = s1[i1];
		SlotData slotData2 = s2[i2];
		if (s1 == s2)
		{
			if (num == num2)
			{
				return;
			}
			if (slotData == slotData2)
			{
				int num3 = 0;
				num3 = ((num != i1) ? (num - num2) : (num2 - num));
				_Slot_Move(s1, locks1, i1, num3);
				return;
			}
		}
		if (slotData == null && slotData2 == null)
		{
			return;
		}
		if (slotData != null)
		{
			slotData.mimicSpellID = 0;
		}
		if (slotData2 != null)
		{
			slotData2.mimicSpellID = 0;
		}
		if (slotData == null)
		{
			s2.Bag_RemoveSlot(i2);
			s1.Bag_SetSlotWithPush(locks1, slotData2, i1);
			return;
		}
		if (slotData2 == null)
		{
			s1.Bag_RemoveSlot(i1);
			s2.Bag_SetSlotWithPush(locks2, slotData, i2);
			return;
		}
		s1.Bag_RemoveSlot(i1);
		s2.Bag_RemoveSlot(i2);
		s1.Bag_SetSlotWithPush(locks1, slotData2, num);
		int num4 = num2;
		do
		{
			if (s2.Bag_CanSetSlotWithPush(locks2, slotData, num4))
			{
				s2.Bag_SetSlotWithPush(locks2, slotData, num4);
				return;
			}
			num4++;
			if (num4 >= s2.Length)
			{
				num4 = 0;
			}
		}
		while (num4 != num2);
		throw new Exception("放不下这个法术");
	}

	private void _Slot_Set(SlotData[] s, bool[] locks, int i, SlotData data)
	{
		data.mimicSpellID = 0;
		s.Bag_SetSlotWithPush(locks, data, i);
	}

	public void Slot_RemoveBagSlot(int index)
	{
		SlotData[] array = BaData.bagSpellDatas.ToArray();
		array.Bag_RemoveSlot(index);
		BaData.bagSpellDatas = array.ToList();
		UIPlayerDataMgr.Inst.UpdateBag();
	}

	public void Slot_RemoveWandSlot(int wandIndex, WandSlotType slotType, int index)
	{
		Wands[wandIndex].WandCfg.GetSlotsData(slotType).Bag_RemoveSlot(index);
		Wands[wandIndex].ResetAndRecheck();
		UIPlayerDataMgr.Inst.WandUpdate(wandIndex);
	}

	public void Slot_SwapSlotBetweenBagAndBag(int aIndex, int bIndex)
	{
		SlotData[] array = BaData.bagSpellDatas.ToArray();
		bool[] array2 = new bool[array.Length];
		Array.Fill(array2, value: false);
		_Slot_SwapSlot(array, array2, array, array2, aIndex, bIndex);
		BaData.bagSpellDatas = array.ToList();
		UIPlayerDataMgr.Inst.UpdateBag();
	}

	public bool Slot_CanSwapSlotBetweenBagAndWand(int bagSlotIndex, int wandIndex, WandSlotType slotType, int wandSlotIndex)
	{
		SlotData[] array = BaData.bagSpellDatas.ToArray().Bag_DeepCopy();
		bool[] array2 = new bool[array.Length];
		Array.Fill(array2, value: false);
		SlotData[] s = Wands[wandIndex].WandCfg.GetSlotsData(slotType).Bag_DeepCopy();
		bool[] slotsLockState = Wands[wandIndex].WandCfg.GetSlotsLockState(slotType);
		try
		{
			_Slot_SwapSlot(array, array2, s, slotsLockState, bagSlotIndex, wandSlotIndex);
			return true;
		}
		catch
		{
			return false;
		}
	}

	public void Slot_SwapSlotBetweenBagAndWand(int bagSlotIndex, int wandIndex, WandSlotType slotType, int wandSlotIndex)
	{
		SlotData[] array = BaData.bagSpellDatas.ToArray();
		bool[] array2 = new bool[array.Length];
		Array.Fill(array2, value: false);
		Wands[wandIndex].ReleaseCharge();
		SlotData[] slotsData = Wands[wandIndex].WandCfg.GetSlotsData(slotType);
		bool[] slotsLockState = Wands[wandIndex].WandCfg.GetSlotsLockState(slotType);
		_Slot_SwapSlot(array, array2, slotsData, slotsLockState, bagSlotIndex, wandSlotIndex);
		BaData.bagSpellDatas = array.ToList();
		Wands[wandIndex].ResetAndRecheck();
		UIPlayerDataMgr.Inst.UpdateBag();
		UIPlayerDataMgr.Inst.WandUpdate(wandIndex);
	}

	public bool Slot_CanSwapSlotBetweenWandAndWand(int wandIndex1, WandSlotType slotType1, int wandSlotIndex1, int wandIndex2, WandSlotType slotType2, int wandSlotIndex2)
	{
		SlotData[] slotsData = Wands[wandIndex1].WandCfg.GetSlotsData(slotType1);
		bool[] slotsLockState = Wands[wandIndex1].WandCfg.GetSlotsLockState(slotType1);
		SlotData[] slotsData2 = Wands[wandIndex2].WandCfg.GetSlotsData(slotType2);
		bool[] slotsLockState2 = Wands[wandIndex2].WandCfg.GetSlotsLockState(slotType2);
		try
		{
			SlotData[] array = slotsData.Bag_DeepCopy();
			SlotData[] s = ((slotsData != slotsData2) ? slotsData2.Bag_DeepCopy() : array);
			_Slot_SwapSlot(array, slotsLockState, s, slotsLockState2, wandSlotIndex1, wandSlotIndex2);
			return true;
		}
		catch
		{
			return false;
		}
	}

	public void Slot_SwapSlotBetweenWandAndWand(int wandIndex1, WandSlotType slotType1, int wandSlotIndex1, int wandIndex2, WandSlotType slotType2, int wandSlotIndex2)
	{
		Wands[wandIndex1].ReleaseCharge();
		Wands[wandIndex2].ReleaseCharge();
		_Slot_SwapSlot(Wands[wandIndex1].WandCfg.GetSlotsData(slotType1), Wands[wandIndex1].WandCfg.GetSlotsLockState(slotType1), Wands[wandIndex2].WandCfg.GetSlotsData(slotType2), Wands[wandIndex2].WandCfg.GetSlotsLockState(slotType2), wandSlotIndex1, wandSlotIndex2);
		Wands[wandIndex1].ResetAndRecheck();
		Wands[wandIndex2].ResetAndRecheck();
		UIPlayerDataMgr.Inst.WandUpdate(wandIndex1);
		UIPlayerDataMgr.Inst.WandUpdate(wandIndex2);
	}
}
