using System;
using System.Collections;
using System.Collections.Generic;
using PlayerLogger;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

public class SpecialObj217 : LayerCorrect, IRoomCtrller
{
	public class RussianDrop
	{
		public rewardType rewardtype;

		public int id;

		public Vector3 Position;

		public GameObject rootObj;

		public GameObject dropItem;

		public GameObject bg;

		public GameObject star1;

		public GameObject star2;

		public SpriteRenderer Sprite;

		public RussianDrop(rewardType _rewardType)
		{
			rewardtype = _rewardType;
		}

		public RussianDrop(rewardType _rewardType, int _id)
		{
			rewardtype = _rewardType;
			id = _id;
		}
	}

	public enum rewardType
	{
		SpellCommonlv1,
		SpellCommonlv2,
		SpellCommonlv3,
		SpellRarelv1,
		SpellRarelv2,
		SpellRarelv3,
		SpellEpic,
		SpellSpecial,
		RelicCommon,
		RelicSpecial,
		RelicRare,
		RelicEpic,
		Curse,
		Chest,
		Coin,
		Dimond,
		Potion,
		Wand
	}

	public float audio1;

	public float audio2;

	private RoomController belongCtrller;

	public GameObject goRoulette;

	public GameObject goRoulette1;

	public GameObject ParticleBroke;

	public AnimationCurve SpinCurve;

	public Coroutine coroutineGlow;

	public Coroutine coroutineSpin;

	public float radius = 5f;

	public int Speed = 10;

	public float SpinTime = 5f;

	private Dictionary<int, RussianDrop> rewardsDrop = new Dictionary<int, RussianDrop>();

	public static SpecialObj217 Inst;

	[Header("具体掉落")]
	public int rewardSpellnumCommonlv2;

	public int rewardSpellnumCommonlv3;

	public int rewardSpellnumRarelv1;

	public int rewardSpellnumRarelv0;

	public int rewardSpellEpic;

	public int rewardRelicnumCommon;

	public int rewardRelicnumRare;

	public int rewardCoin;

	public int rewardDimond;

	public int rewardCurse;

	public int rewardChest;

	public List<int> OverrideSpellIDs = new List<int>();

	private List<int> _OverrideSpellIDsCurrent = new List<int>();

	public List<int> OverrideRelicIds = new List<int>();

	private List<int> _OverrideRelicIds = new List<int>();

	[Range(0f, 100f)]
	public int reward_UpgradeRare;

	[Range(0f, 100f)]
	public int reward_UpgradeLevel;

	[Space(50f)]
	[Header("Handle")]
	public Vector3 handleOffsetFlipped;

	public Transform tsfHandle;

	public Animator animatorHandle;

	public SpecialObj217_Handle handle;

	public Collider handleCollider;

	[SerializeField]
	private int Damage = 25;

	[HideInInspector]
	public int _damageCounted;

	public int _damageCountedDiscount;

	private float discount = 1f;

	public int maxInteractTime = 3;

	private int _interactLeft;

	[Space(50f)]
	[Header("UI")]
	public SpriteRenderer Highlight;

	public Sprite spriteBroken;

	public SpriteRenderer SpriteHandle;

	public Sprite spritePlatBroken1;

	public Sprite spritePlatBroken2;

	public SpriteRenderer spriteRenderPlat1Broken;

	public SpriteRenderer spriteRenderPlat2Broken;

	public GameObject SpellIcon;

	public GameObject SpellIconStar;

	public GameObject SpellIconBackground;

	public float SpriteSize = 10f;

	public Text text_Cost;

	public Text text_CostAfterDiscount;

	public GameObject goDiscountUI;

	public Vector3 SpriteOffset = new Vector3(0f, -0.2f, 0f);

	public GameObject CanvasBloodCost;

	public Sprite Chest;

	public Sprite Coin;

	public Sprite Dimond;

	public Vector2 StarPositionOffset;

	public Vector2 SecondStarPositionOffset;

	[ColorUsage(true, true)]
	public Color ColorNormal;

	[ColorUsage(true, true)]
	public Color ColorLight;

	[Header("Color")]
	public Color ColorAll;

	public Color Commonlv1;

	public Color Commonlv2;

	public Color Commonlv3;

	public Color RareLv1;

	public Color Rarelv2;

	public Color Rarelv3;

	public Color RelicCommon;

	public Color RelicRare;

	public Color colorCurse;

	public Color colorChest;

	[Header("AudioSource")]
	public AudioSource as_Interact;

	public AudioSource as_Rotate;

	public AudioSource as_Bell;

	public float as_RotateVolume;

	private Vector3 nextDropPoint = Vector3.zero;

	public bool OverrideDrop
	{
		get
		{
			if (OverrideRelicIds.Count == 0 && _OverrideRelicIds.Count == 0 && OverrideSpellIDs.Count == 0)
			{
				return _OverrideSpellIDsCurrent.Count != 0;
			}
			return true;
		}
	}

	public bool HPAndShiledEnough
	{
		get
		{
			if (PlayerMgr.Inst.TryGetPlayerPpt(out var playerPpt))
			{
				return playerPpt.unitCfg.currentHP + playerPpt.unitCfg.shieldTemp + playerPpt.unitCfg.shield > (float)_damageCounted;
			}
			Debug.LogError("为什么没有playerPpt");
			return false;
		}
	}

	public bool HPAndShiledEnoughDiscount
	{
		get
		{
			if (PlayerMgr.Inst.TryGetPlayerPpt(out var playerPpt))
			{
				return playerPpt.unitCfg.currentHP + playerPpt.unitCfg.shieldTemp + playerPpt.unitCfg.shield > (float)_damageCountedDiscount;
			}
			Debug.LogError("为什么没有playerPpt");
			return false;
		}
	}

	public int numberOfObjects => rewardSpellnumCommonlv2 + rewardSpellnumCommonlv3 + rewardSpellnumRarelv1 + rewardSpellnumRarelv0 + rewardRelicnumCommon + rewardRelicnumRare + rewardCurse + rewardChest + rewardCoin + rewardDimond + rewardSpellEpic;

	public void SetRoomCtrlller(RoomController levelCtrller)
	{
		belongCtrller = levelCtrller;
	}

	public override void OnEnable()
	{
		base.OnEnable();
		EventMgr.PotionUse_Discount = (Action<float>)Delegate.Combine(EventMgr.PotionUse_Discount, new Action<float>(PotionUse_Discount));
	}

	private void OnDisable()
	{
		EventMgr.PotionUse_Discount = (Action<float>)Delegate.Remove(EventMgr.PotionUse_Discount, new Action<float>(PotionUse_Discount));
	}

	private void PotionUse_Discount(float discountRatio)
	{
		if (!(belongCtrller != LevelMgr.Inst.CurrentRoomCtrller))
		{
			discount = discountRatio;
		}
	}

	public int GetCost()
	{
		return Damage * (maxInteractTime - _interactLeft);
	}

	public int GetCostDiscount()
	{
		return Mathf.CeilToInt((float)(Damage * (maxInteractTime - _interactLeft)) * discount);
	}

	private void Start()
	{
		_damageCounted = Damage;
		_damageCountedDiscount = Damage;
		StartCoroutine(GenerateObjectsInCircle());
		_interactLeft = maxInteractTime;
		EventMgr.SoundVolumeChange = (Action)Delegate.Combine(EventMgr.SoundVolumeChange, new Action(SoundVolumeChange));
		SoundVolumeChange();
		if (!belongCtrller.roomCfg.isFlipped)
		{
			tsfHandle.transform.position -= handleOffsetFlipped;
		}
		Inst = this;
	}

	private void Update()
	{
		_damageCounted = GetCost();
		_damageCountedDiscount = GetCostDiscount();
		text_Cost.text = _damageCounted.ToString();
		if (!HPAndShiledEnough)
		{
			text_Cost.color = Color.red;
		}
		else
		{
			text_Cost.color = Color.green;
		}
		if (discount != 1f)
		{
			_damageCountedDiscount = GetCostDiscount();
			goDiscountUI.SetActive(value: true);
			text_CostAfterDiscount.text = GetCostDiscount().ToString();
			if (!HPAndShiledEnoughDiscount)
			{
				text_CostAfterDiscount.color = Color.red;
			}
			else
			{
				text_CostAfterDiscount.color = Color.green;
			}
		}
		CanvasBloodCost.SetActive(value: true);
		if (_interactLeft == 0)
		{
			CanvasBloodCost.SetActive(value: false);
		}
	}

	private void SoundVolumeChange()
	{
		as_Interact.volume = DataMgr.settingData.GetFinalSound();
		as_RotateVolume = DataMgr.settingData.GetFinalSound();
		as_Rotate.volume = as_RotateVolume;
		as_Bell.volume = DataMgr.settingData.GetFinalSound();
	}

	public void InteractHandle()
	{
		if (HPAndShiledEnoughDiscount)
		{
			as_Interact.PlayOneShot(as_Interact.clip);
			animatorHandle.Play("Trigger");
			if (_damageCounted != 0)
			{
				TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(AttackerType.NothingSpecial);
				info.ignorePlayerInvincibleFrame = true;
				info.ignoreUmbrella = true;
				info.ignoreRelicDodge = true;
				info.ignoreRelicOrCurseDamageRatioChange = true;
				info.damage = GetCostDiscount();
				UnitDotsSyncSystem.AddTakeDamageRequest(PlayerMgr.Inst.PlayerEtt, info);
			}
			_interactLeft--;
			if (_interactLeft == 0)
			{
				handle.SetDotsObjLayer(handle.thisEntity, isOpen: false);
				ParticleBroke.SetActive(value: true);
				CanvasBloodCost.SetActive(value: false);
				SpriteHandle.sprite = spriteBroken;
				spriteRenderPlat1Broken.sprite = spritePlatBroken1;
				spriteRenderPlat2Broken.sprite = spritePlatBroken2;
			}
			as_Bell.Stop();
			GetNextDropPoint();
			coroutineSpin = StartCoroutine(Spin(SpinTime, Speed));
		}
	}

	public bool haveEPicSpellNotDrop()
	{
		foreach (int item in _OverrideSpellIDsCurrent)
		{
			if (RollRewardFly.DropType2So217rewardType(RollRewardFly.DropType.Spell, item) == rewardType.SpellEpic)
			{
				return true;
			}
		}
		return false;
	}

	public void GetNextDropPoint()
	{
		nextDropPoint = Tool2D.GetNavMeshPointIngoreZ(base.transform.position + new Vector3(-4f, 2f, 0f), 1f);
		if (belongCtrller.roomCfg.isFlipped)
		{
			nextDropPoint = Tool2D.GetNavMeshPointIngoreZ(base.transform.position + new Vector3(4f, 2f, 0f), 1f);
		}
	}

	public void DropReward(RussianDrop drop)
	{
		switch (drop.rewardtype)
		{
		case rewardType.SpellCommonlv1:
		case rewardType.SpellCommonlv2:
		case rewardType.SpellCommonlv3:
		case rewardType.SpellEpic:
		case rewardType.SpellSpecial:
			DropSpell(drop.id);
			LevelMgr.Inst.RoomFinishLogger?.AddCurrentSideRoomReward(PlayerLogger.Item.CreateSpell(drop.id));
			break;
		case rewardType.SpellRarelv1:
		case rewardType.SpellRarelv2:
		case rewardType.SpellRarelv3:
			DropSpell(drop.id);
			LevelMgr.Inst.RoomFinishLogger?.AddCurrentSideRoomReward(PlayerLogger.Item.CreateSpell(drop.id));
			break;
		case rewardType.RelicCommon:
		case rewardType.RelicRare:
		case rewardType.RelicEpic:
			DropRelic(drop.id);
			LevelMgr.Inst.RoomFinishLogger?.AddCurrentSideRoomReward(new PlayerLogger.Item
			{
				type = PlayerLogger.Item.Type.Relic,
				number = 1,
				id = drop.id
			});
			break;
		case rewardType.Curse:
			GetCurse(drop.id);
			LevelMgr.Inst.RoomFinishLogger?.AddCurrentSideRoomReward(new PlayerLogger.Item
			{
				type = PlayerLogger.Item.Type.Curse,
				number = 1,
				id = drop.id
			});
			break;
		case rewardType.Chest:
			GetChest();
			break;
		case rewardType.Coin:
			LevelMgr.Inst.RoomFinishLogger?.AddCurrentSideRoomReward(new PlayerLogger.Item
			{
				type = PlayerLogger.Item.Type.Coin,
				number = 1,
				id = 0
			});
			DropCoin();
			break;
		case rewardType.Dimond:
			LevelMgr.Inst.RoomFinishLogger?.AddCurrentSideRoomReward(new PlayerLogger.Item
			{
				type = PlayerLogger.Item.Type.Coin,
				number = 5,
				id = 0
			});
			DropDimond();
			break;
		case rewardType.RelicSpecial:
			break;
		}
		void DropCoin()
		{
			PlayerItemController itemCtrller2 = PlayerMgr.Inst.ItemCtrller;
			rewardType rewardtype = drop.rewardtype;
			Vector3 position2 = drop.Position;
			Vector3 moveToPoint2 = nextDropPoint;
			RoomController roomController2 = belongCtrller;
			itemCtrller2.RewardDropFly(11, rewardtype, position2, moveToPoint2, null, useParticleColor: true, null, isUI: false, dropItem: true, roomController2);
		}
		void DropDimond()
		{
			PlayerItemController itemCtrller4 = PlayerMgr.Inst.ItemCtrller;
			rewardType rewardtype2 = drop.rewardtype;
			Vector3 position4 = drop.Position;
			Vector3 moveToPoint4 = nextDropPoint;
			RoomController roomController4 = belongCtrller;
			itemCtrller4.RewardDropFly(12, rewardtype2, position4, moveToPoint4, null, useParticleColor: true, null, isUI: false, dropItem: true, roomController4);
		}
		void DropRelic(int id)
		{
			if (OverrideDrop)
			{
				_OverrideRelicIds.Remove(id);
			}
			PlayerItemController itemCtrller = PlayerMgr.Inst.ItemCtrller;
			Vector3 position = drop.Position;
			Vector3 moveToPoint = nextDropPoint;
			RoomController roomController = belongCtrller;
			itemCtrller.RewardDropFly(id, RollRewardFly.DropType.Relic, position, moveToPoint, null, useParticleColor: true, null, isUI: false, dropItem: true, roomController);
		}
		void DropSpell(int id)
		{
			if (OverrideDrop)
			{
				_OverrideSpellIDsCurrent.Remove(id);
				OverrideSpellIDs.Add(id);
			}
			PlayerItemController itemCtrller3 = PlayerMgr.Inst.ItemCtrller;
			Vector3 position3 = drop.Position;
			Vector3 moveToPoint3 = nextDropPoint;
			RoomController roomController3 = belongCtrller;
			itemCtrller3.RewardDropFly(id, RollRewardFly.DropType.Spell, position3, moveToPoint3, null, useParticleColor: true, null, isUI: false, dropItem: true, roomController3);
		}
		void GetChest()
		{
			ChestType chestType = (ChestType)UnityEngine.Random.Range(0, 4);
			int id2 = 401;
			switch (chestType)
			{
			case ChestType.NoLock:
				id2 = 404;
				break;
			case ChestType.Lock:
				id2 = 401;
				break;
			case ChestType.Spike:
				id2 = 402;
				break;
			case ChestType.Curse:
				id2 = 403;
				break;
			default:
				Debug.LogError(chestType);
				break;
			}
			Entity entity = QuickCreateSystem.Inst.CreateSpecialObj(id2, drop.Position);
			Vector3 vector = nextDropPoint;
			if (chestType == ChestType.NoLock)
			{
				SpecialObj4NoLock componentData = UnitDotsSyncSystem.GetComponentData<SpecialObj4NoLock>(entity);
				componentData.SetFly(vector);
				UnitDotsSyncSystem.SetComponentData(componentData, entity);
			}
			else
			{
				SpecialObj4_Dots componentData2 = UnitDotsSyncSystem.GetComponentData<SpecialObj4_Dots>(entity);
				componentData2.SetFly(vector);
				UnitDotsSyncSystem.SetComponentData(componentData2, entity);
			}
			IRoomCtrller_Dots componentData3 = UnitDotsSyncSystem.GetComponentData<IRoomCtrller_Dots>(entity);
			componentData3.belongRoom.Value = LevelMgr.Inst.CurrentRoomCtrller;
			componentData3.onRoomEnter = true;
			UnitDotsSyncSystem.SetComponentData(componentData3, entity);
		}
		void GetCurse(int id)
		{
			PlayerMgr.Inst.ItemCtrller.CurseAdd(id, drop.Position);
		}
	}

	public IEnumerator Spin(float MaxTime, float Speed)
	{
		coroutineGlow = StartCoroutine(Glow());
		System.Random random = new System.Random();
		System.Random random2 = new System.Random();
		float num = (float)random.NextDouble() * 0.6f - 0.3f;
		float num2 = (float)random2.NextDouble() * 0.5f - 0.25f;
		float randomTime = SpinTime * num;
		float randomSpeed = Speed * num2;
		handle.SetDotsObjLayer(handle.thisEntity, isOpen: false);
		float time = 0f;
		as_Rotate.pitch = as_Rotate.clip.length / MaxTime;
		as_Rotate.Play();
		do
		{
			yield return new WaitForEndOfFrame();
			time += Time.deltaTime;
			float num3 = SpinCurve.Evaluate(time / (MaxTime + randomTime));
			goRoulette.transform.Rotate(new Vector3(0f, 0f, num3 * (Speed + randomSpeed)) * Time.timeScale, Space.Self);
			goRoulette1.transform.Rotate(new Vector3(0f, 0f, num3 * (Speed + randomSpeed)) * Time.timeScale, Space.Self);
			float num4 = num3 * 6f + 0.7f;
			as_Rotate.outputAudioMixerGroup.audioMixer.SetFloat("MASTER_Pitch", num4);
			as_Rotate.outputAudioMixerGroup.audioMixer.SetFloat("PITCHSHIFTER_Pitch", 1f / num4 + 0.7f);
			as_Rotate.volume = (num3 * 1f + 0.3f) * 0.3f * as_RotateVolume;
		}
		while (!(time >= MaxTime + randomTime));
		int dropid = GenerateID(goRoulette.transform.eulerAngles.z);
		DropReward(rewardsDrop[dropid]);
		coroutineSpin = null;
		as_Rotate.Stop();
		as_Bell.PlayOneShot(as_Bell.clip);
		yield return new WaitForSeconds(0.5f);
		int eular = 0;
		while (eular < 180)
		{
			yield return new WaitForFixedUpdate();
			eular += 5;
			if (eular == 90)
			{
				ReturnPointedRegenerate(dropid);
				rewardsDrop[dropid].rootObj.gameObject.transform.eulerAngles = new Vector3(0f, -90f, 0f);
			}
			else if (eular >= 180)
			{
				eular = 180;
				rewardsDrop[dropid].rootObj.gameObject.transform.rotation = Quaternion.identity;
			}
			else
			{
				rewardsDrop[dropid].rootObj.gameObject.transform.Rotate(new Vector3(0f, 5f, 0f));
			}
		}
		animatorHandle.Play("Recover");
		if (_interactLeft > 0)
		{
			handle.SetDotsObjLayer(handle.thisEntity, isOpen: true);
		}
	}

	public float smooth(float min, float max, float minfrom, float maxfrom, float current)
	{
		return (current - minfrom) / (maxfrom - minfrom) * (max - min) + min;
	}

	private IEnumerator GenerateObjectsInCircle()
	{
		yield return new WaitForEndOfFrame();
		float angleIncrement = 360f / (float)numberOfObjects;
		float Stargangle = angleIncrement / 2f;
		int _spellr1lv1 = rewardSpellnumCommonlv2;
		int _spellr1lv2 = rewardSpellnumCommonlv3;
		int _spellr2lv1 = rewardSpellnumRarelv1;
		int _spellr2lv0 = rewardSpellnumRarelv0;
		int _spellepic = rewardSpellEpic;
		int _coin = rewardCoin;
		int _dimond = rewardDimond;
		int _relicr1 = rewardRelicnumCommon;
		int _relicr2 = rewardRelicnumRare;
		int _curse = rewardCurse;
		int _chest = rewardChest;
		List<int> ids = new List<int>();
		for (int num = 0; num < numberOfObjects; num++)
		{
			ids.Add(num);
		}
		while (_spellr1lv1 > 0)
		{
			int j = UnityEngine.Random.Range(0, ids.Count);
			yield return StartCoroutine(GetARussionDropItem(ids[j], rewardType.SpellCommonlv2));
			ids.Remove(ids[j]);
			_spellr1lv1--;
		}
		while (_spellr1lv2 > 0)
		{
			int j = UnityEngine.Random.Range(0, ids.Count);
			yield return StartCoroutine(GetARussionDropItem(ids[j], rewardType.SpellCommonlv3));
			ids.Remove(ids[j]);
			_spellr1lv2--;
		}
		while (_spellr2lv1 > 0)
		{
			int j = UnityEngine.Random.Range(0, ids.Count);
			yield return StartCoroutine(GetARussionDropItem(ids[j], rewardType.SpellRarelv2));
			ids.Remove(ids[j]);
			_spellr2lv1--;
		}
		while (_spellr2lv0 > 0)
		{
			int j = UnityEngine.Random.Range(0, ids.Count);
			yield return StartCoroutine(GetARussionDropItem(ids[j], rewardType.SpellRarelv1));
			ids.Remove(ids[j]);
			_spellr2lv0--;
		}
		while (_relicr1 > 0)
		{
			int j = UnityEngine.Random.Range(0, ids.Count);
			yield return StartCoroutine(GetARussionDropItem(ids[j], rewardType.RelicCommon));
			ids.Remove(ids[j]);
			_relicr1--;
		}
		while (_relicr2 > 0)
		{
			int j = UnityEngine.Random.Range(0, ids.Count);
			yield return StartCoroutine(GetARussionDropItem(ids[j], rewardType.RelicRare));
			ids.Remove(ids[j]);
			_relicr2--;
		}
		while (_chest > 0)
		{
			int j = UnityEngine.Random.Range(0, ids.Count);
			yield return StartCoroutine(GetARussionDropItem(ids[j], rewardType.Chest));
			ids.Remove(ids[j]);
			_chest--;
		}
		while (_curse > 0)
		{
			int j = UnityEngine.Random.Range(0, ids.Count);
			yield return StartCoroutine(GetARussionDropItem(ids[j], rewardType.Curse));
			ids.Remove(ids[j]);
			_curse--;
		}
		while (_dimond > 0)
		{
			int j = UnityEngine.Random.Range(0, ids.Count);
			yield return StartCoroutine(GetARussionDropItem(ids[j], rewardType.Dimond));
			ids.Remove(ids[j]);
			_dimond--;
		}
		while (_coin > 0)
		{
			int j = UnityEngine.Random.Range(0, ids.Count);
			yield return StartCoroutine(GetARussionDropItem(ids[j], rewardType.Coin));
			ids.Remove(ids[j]);
			_coin--;
		}
		while (_spellepic > 0)
		{
			int j = UnityEngine.Random.Range(0, ids.Count);
			yield return StartCoroutine(GetARussionDropItem(ids[j], rewardType.SpellEpic));
			ids.Remove(ids[j]);
			_spellepic--;
		}
		IEnumerator GetARussionDropItem(int i, rewardType rewardType)
		{
			yield return new WaitForEndOfFrame();
			float num2 = (float)i * angleIncrement + Stargangle;
			float x = base.transform.position.x + radius * Mathf.Cos(num2 * (MathF.PI / 180f));
			float y = base.transform.position.y + radius * Mathf.Sin(num2 * (MathF.PI / 180f)) * Mathf.Cos(goRoulette.transform.rotation.eulerAngles.x * (MathF.PI / 180f));
			Vector3 position = new Vector3(x, y, base.transform.position.z) + SpriteOffset;
			RussianDrop russianDrop = new RussianDrop(rewardType);
			russianDrop.id = GetDropID(rewardType);
			russianDrop.rootObj = GenerateReward(russianDrop, position, i, AddToDic: true);
			russianDrop.Position = position;
		}
	}

	private RussianDrop GenerateIDAngle(float angle)
	{
		float num = 360f / (float)numberOfObjects;
		int key = (int)((angle - num / 2f) / num + 0.5f);
		if (!rewardsDrop.ContainsKey(key))
		{
			return rewardsDrop[0];
		}
		return rewardsDrop[key];
	}

	private int GenerateID(float angle)
	{
		float num = 360f / (float)numberOfObjects;
		int num2 = (int)((angle - num / 2f) / num + 0.5f);
		if (!rewardsDrop.ContainsKey(num2))
		{
			Debug.LogError("出错了");
			return 0;
		}
		return num2;
	}

	public int GetDropID(rewardType rewardType, RussianDrop russianDrop = null)
	{
		if (!OverrideDrop)
		{
			return rewardType switch
			{
				rewardType.SpellCommonlv1 => PlayerMgr.Inst.BaData.GetSpellFromPool(1, ItemDropType.Common), 
				rewardType.SpellCommonlv2 => PlayerMgr.Inst.BaData.GetSpellFromPool(2, ItemDropType.Common), 
				rewardType.SpellCommonlv3 => PlayerMgr.Inst.BaData.GetSpellFromPool(3, ItemDropType.Common), 
				rewardType.SpellRarelv1 => PlayerMgr.Inst.BaData.GetSpellFromPool(1, ItemDropType.Rare), 
				rewardType.SpellRarelv2 => PlayerMgr.Inst.BaData.GetSpellFromPool(2, ItemDropType.Rare), 
				rewardType.SpellRarelv3 => PlayerMgr.Inst.BaData.GetSpellFromPool(3, ItemDropType.Rare), 
				rewardType.SpellEpic => PlayerMgr.Inst.BaData.GetSpellFromPool(1, ItemDropType.Epic), 
				rewardType.SpellSpecial => PlayerMgr.Inst.BaData.GetSpellFromPool(1, ItemDropType.Special), 
				rewardType.RelicCommon => PlayerMgr.Inst.BaData.GetRelicFromPool(ItemDropType.Common), 
				rewardType.RelicRare => PlayerMgr.Inst.BaData.GetRelicFromPool(ItemDropType.Rare), 
				rewardType.Curse => PlayerMgr.Inst.BaData.GetCurseFromPool(ItemDropType.Common), 
				rewardType.Chest => 1, 
				rewardType.Coin => 1, 
				rewardType.Dimond => 1, 
				_ => PlayerMgr.Inst.BaData.GetSpellFromPool(1, ItemDropType.Common), 
			};
		}
		switch (rewardType)
		{
		case rewardType.SpellCommonlv1:
		case rewardType.SpellCommonlv2:
		case rewardType.SpellCommonlv3:
		case rewardType.SpellRarelv1:
		case rewardType.SpellRarelv2:
		case rewardType.SpellRarelv3:
		case rewardType.SpellEpic:
		case rewardType.SpellSpecial:
		{
			int num4 = OverrideSpellIDs[UnityEngine.Random.Range(0, OverrideSpellIDs.Count)];
			bool flag = haveEPicSpellNotDrop();
			int num5 = 0;
			if (russianDrop != null)
			{
				while (num4 == russianDrop.id || (flag && rewardType.SpellEpic == RollRewardFly.DropType2So217rewardType(RollRewardFly.DropType.Spell, num4)))
				{
					num5++;
					num4 = OverrideSpellIDs[UnityEngine.Random.Range(0, OverrideSpellIDs.Count)];
					if (num5 > 100)
					{
						Debug.Log("取不出来合适的法术");
						break;
					}
				}
			}
			OverrideSpellIDs.Remove(num4);
			_OverrideSpellIDsCurrent.Add(num4);
			return num4;
		}
		case rewardType.RelicCommon:
		case rewardType.RelicRare:
		{
			if (OverrideRelicIds.Count > 0)
			{
				int num = OverrideRelicIds[UnityEngine.Random.Range(0, OverrideRelicIds.Count)];
				int num2 = 0;
				if (russianDrop != null)
				{
					while (num == russianDrop.id || _OverrideRelicIds.Contains(num))
					{
						num2++;
						num = OverrideRelicIds[UnityEngine.Random.Range(0, OverrideRelicIds.Count)];
						if (num2 > 100)
						{
							Debug.Log("取不出来合适的元素");
							russianDrop.rewardtype = rewardType.SpellRarelv2;
							num = OverrideSpellIDs[UnityEngine.Random.Range(0, OverrideSpellIDs.Count)];
							OverrideSpellIDs.Remove(num);
							_OverrideSpellIDsCurrent.Add(num);
							return num;
						}
					}
				}
				else
				{
					while (_OverrideRelicIds.Contains(num))
					{
						num2++;
						num = OverrideRelicIds[UnityEngine.Random.Range(0, OverrideRelicIds.Count)];
					}
				}
				OverrideRelicIds.Remove(num);
				if (PlayerMgr.Inst.BaData.poolOfRelic.ContainsKey(num))
				{
					PlayerMgr.Inst.BaData.RemoveRelicFromPool(num);
				}
				_OverrideRelicIds.Add(num);
				return num;
			}
			russianDrop.rewardtype = rewardType.SpellRarelv2;
			int num3 = OverrideSpellIDs[UnityEngine.Random.Range(0, OverrideSpellIDs.Count)];
			OverrideSpellIDs.Remove(num3);
			_OverrideSpellIDsCurrent.Add(num3);
			return num3;
		}
		case rewardType.Curse:
			return PlayerMgr.Inst.BaData.GetCurseFromPool(ItemDropType.Common);
		case rewardType.Chest:
			return 1;
		case rewardType.Coin:
			return 1;
		case rewardType.Dimond:
			return 1;
		default:
			Debug.LogError("错误");
			return 1;
		}
	}

	public void ReturnPointedRegenerate(int Dropid)
	{
		rewardsDrop[Dropid].id = GetDropID(rewardsDrop[Dropid].rewardtype, rewardsDrop[Dropid]);
		float num = 360f / (float)numberOfObjects;
		float num2 = num / 2f;
		float num3 = (float)Dropid * num + num2;
		float x = base.transform.position.x + radius * Mathf.Cos(num3 * (MathF.PI / 180f));
		float y = base.transform.position.y + radius * Mathf.Sin(num3 * (MathF.PI / 180f)) * Mathf.Cos(goRoulette.transform.rotation.eulerAngles.x * (MathF.PI / 180f));
		Vector3 position = new Vector3(x, y, base.transform.position.z) + SpriteOffset;
		rewardsDrop[Dropid].rootObj = GenerateReward(rewardsDrop[Dropid], position, Dropid, AddToDic: false);
		rewardsDrop[Dropid].Position = position;
	}

	private GameObject GenerateReward(RussianDrop dropItem, Vector3 position, int i, bool AddToDic)
	{
		if (dropItem.bg != null)
		{
			UnityEngine.Object.Destroy(dropItem.bg);
		}
		if (dropItem.rootObj != null)
		{
			UnityEngine.Object.Destroy(dropItem.rootObj);
		}
		GameObject gameObject = UnityEngine.Object.Instantiate(SpellIconBackground, new Vector3(position.x, position.y, tsf_Layer.transform.position.z - 2E-05f), Quaternion.identity, tsf_Layer.transform);
		gameObject.GetComponent<SpriteRenderer>().color = Color.black;
		dropItem.bg = gameObject;
		GameObject BackGround = UnityEngine.Object.Instantiate(SpellIconBackground, new Vector3(position.x, position.y, tsf_Layer.transform.position.z - 5E-05f), Quaternion.identity, tsf_Layer.transform);
		BackGround.GetComponent<SpriteRenderer>().color = GetBackgoundColor(dropItem);
		rewardType rewardType = dropItem.rewardtype;
		if (OverrideDrop)
		{
			rewardType = RollRewardFly.DropType2So217rewardType(RollRewardFly.Convert217rewardType2DromType(dropItem.rewardtype), dropItem.id);
		}
		switch (rewardType)
		{
		case rewardType.SpellCommonlv1:
		{
			Sprite sprite = ABResources.LoadAsset<Sprite>(SpellConfig.dic[dropItem.id].GetIconPath());
			SpellIcon.GetComponent<SpriteRenderer>().sprite = sprite;
			SpellIcon.name = dropItem.id.ToString();
			GameObject gameObject2 = UnityEngine.Object.Instantiate(SpellIcon, new Vector3(position.x, position.y - 0.25f, BackGround.transform.position.z - 0.0001f), Quaternion.identity, BackGround.transform);
			gameObject2.transform.localScale = new Vector3(SpriteSize * sprite.pixelsPerUnit / sprite.rect.width, SpriteSize * sprite.pixelsPerUnit / sprite.rect.width, 1f);
			break;
		}
		case rewardType.SpellCommonlv2:
		{
			Sprite sprite = ABResources.LoadAsset<Sprite>(SpellConfig.dic[dropItem.id].GetIconPath());
			SpellIcon.GetComponent<SpriteRenderer>().sprite = sprite;
			SpellIcon.name = dropItem.id.ToString();
			GameObject gameObject2 = UnityEngine.Object.Instantiate(SpellIcon, new Vector3(position.x, position.y - 0.25f, BackGround.transform.position.z - 0.0001f), Quaternion.identity, BackGround.transform);
			gameObject2.transform.localScale = new Vector3(SpriteSize * sprite.pixelsPerUnit / sprite.rect.width, SpriteSize * sprite.pixelsPerUnit / sprite.rect.width, 1f);
			InitOneStar();
			dropItem.dropItem = gameObject2;
			break;
		}
		case rewardType.SpellCommonlv3:
		{
			Sprite sprite = ABResources.LoadAsset<Sprite>(SpellConfig.dic[dropItem.id].GetIconPath());
			SpellIcon.GetComponent<SpriteRenderer>().sprite = sprite;
			SpellIcon.name = dropItem.id.ToString();
			GameObject gameObject2 = UnityEngine.Object.Instantiate(SpellIcon, new Vector3(position.x, position.y - 0.25f, BackGround.transform.position.z - 0.0001f), Quaternion.identity, BackGround.transform);
			gameObject2.transform.localScale = new Vector3(SpriteSize * sprite.pixelsPerUnit / sprite.rect.width, SpriteSize * sprite.pixelsPerUnit / sprite.rect.width, 1f);
			InitTwoStar();
			dropItem.dropItem = gameObject2;
			break;
		}
		case rewardType.SpellEpic:
		case rewardType.SpellSpecial:
		{
			Sprite sprite = ABResources.LoadAsset<Sprite>(SpellConfig.dic[dropItem.id].GetIconPath());
			SpellIcon.GetComponent<SpriteRenderer>().sprite = sprite;
			SpellIcon.name = dropItem.id.ToString();
			GameObject gameObject2 = UnityEngine.Object.Instantiate(SpellIcon, new Vector3(position.x - 0.02f, position.y - 0.25f, BackGround.transform.position.z - 0.0003f), Quaternion.identity, BackGround.transform);
			gameObject2.transform.localScale = new Vector3(SpriteSize * sprite.pixelsPerUnit / sprite.rect.width, SpriteSize * sprite.pixelsPerUnit / sprite.rect.width, 1f);
			dropItem.dropItem = gameObject2;
			break;
		}
		case rewardType.SpellRarelv1:
		{
			Sprite sprite = ABResources.LoadAsset<Sprite>(SpellConfig.dic[dropItem.id].GetIconPath());
			SpellIcon.GetComponent<SpriteRenderer>().sprite = sprite;
			SpellIcon.name = dropItem.id.ToString();
			GameObject gameObject2 = UnityEngine.Object.Instantiate(SpellIcon, new Vector3(position.x, position.y - 0.25f, BackGround.transform.position.z - 0.0001f), Quaternion.identity, BackGround.transform);
			gameObject2.transform.localScale = new Vector3(SpriteSize * sprite.pixelsPerUnit / sprite.rect.width, SpriteSize * sprite.pixelsPerUnit / sprite.rect.width, 1f);
			dropItem.dropItem = gameObject2;
			break;
		}
		case rewardType.SpellRarelv2:
		{
			Sprite sprite = ABResources.LoadAsset<Sprite>(SpellConfig.dic[dropItem.id].GetIconPath());
			SpellIcon.GetComponent<SpriteRenderer>().sprite = sprite;
			SpellIcon.name = dropItem.id.ToString();
			GameObject gameObject2 = UnityEngine.Object.Instantiate(SpellIcon, new Vector3(position.x, position.y - 0.25f, BackGround.transform.position.z - 0.0001f), Quaternion.identity, BackGround.transform);
			gameObject2.transform.localScale = new Vector3(SpriteSize * sprite.pixelsPerUnit / sprite.rect.width, SpriteSize * sprite.pixelsPerUnit / sprite.rect.width, 1f);
			InitOneStar();
			dropItem.dropItem = gameObject2;
			break;
		}
		case rewardType.SpellRarelv3:
		{
			Sprite sprite = ABResources.LoadAsset<Sprite>(SpellConfig.dic[dropItem.id].GetIconPath());
			SpellIcon.GetComponent<SpriteRenderer>().sprite = sprite;
			SpellIcon.name = dropItem.id.ToString();
			GameObject gameObject2 = UnityEngine.Object.Instantiate(SpellIcon, new Vector3(position.x, position.y - 0.25f, BackGround.transform.position.z - 0.0001f), Quaternion.identity, BackGround.transform);
			gameObject2.transform.localScale = new Vector3(SpriteSize * sprite.pixelsPerUnit / sprite.rect.width, SpriteSize * sprite.pixelsPerUnit / sprite.rect.width, 1f);
			InitTwoStar();
			dropItem.dropItem = gameObject2;
			break;
		}
		case rewardType.RelicCommon:
		case rewardType.RelicRare:
		case rewardType.RelicEpic:
		{
			Sprite sprite = ABResources.LoadAsset<Sprite>(RelicConfig.dic[dropItem.id].GetIconPath());
			SpellIcon.GetComponent<SpriteRenderer>().sprite = sprite;
			SpellIcon.name = dropItem.id.ToString();
			GameObject gameObject2 = UnityEngine.Object.Instantiate(SpellIcon, new Vector3(position.x, position.y - 0.2f, BackGround.transform.position.z - 0.0001f), Quaternion.identity, BackGround.transform);
			gameObject2.transform.localScale = new Vector3(SpriteSize * sprite.pixelsPerUnit / sprite.rect.width, SpriteSize * sprite.pixelsPerUnit / sprite.rect.width, 1f);
			dropItem.dropItem = gameObject2;
			break;
		}
		case rewardType.Curse:
		{
			Sprite sprite = ABResources.LoadAsset<Sprite>(CurseConfig.dic[dropItem.id].GetIconPath());
			SpellIcon.GetComponent<SpriteRenderer>().sprite = sprite;
			SpellIcon.name = dropItem.id.ToString();
			GameObject gameObject2 = UnityEngine.Object.Instantiate(SpellIcon, new Vector3(position.x, position.y, BackGround.transform.position.z - 0.0001f), Quaternion.identity, BackGround.transform);
			gameObject2.transform.localScale = new Vector3(SpriteSize * sprite.pixelsPerUnit / sprite.rect.width, SpriteSize * sprite.pixelsPerUnit / sprite.rect.width, 1f);
			dropItem.dropItem = gameObject2;
			break;
		}
		case rewardType.Dimond:
		{
			SpellIcon.GetComponent<SpriteRenderer>().sprite = Dimond;
			SpellIcon.name = dropItem.id.ToString();
			GameObject gameObject2 = UnityEngine.Object.Instantiate(SpellIcon, new Vector3(position.x, position.y, BackGround.transform.position.z - 0.0001f), Quaternion.identity, BackGround.transform);
			gameObject2.transform.localScale = new Vector3(SpriteSize * Dimond.pixelsPerUnit / Dimond.rect.width * 0.7f, SpriteSize * Dimond.pixelsPerUnit / Dimond.rect.width * 0.7f, 1f);
			dropItem.dropItem = gameObject2;
			break;
		}
		case rewardType.Coin:
		{
			SpellIcon.GetComponent<SpriteRenderer>().sprite = Coin;
			SpellIcon.name = dropItem.id.ToString();
			GameObject gameObject2 = UnityEngine.Object.Instantiate(SpellIcon, new Vector3(position.x, position.y, BackGround.transform.position.z - 0.0001f), Quaternion.identity, BackGround.transform);
			gameObject2.transform.localScale = new Vector3(SpriteSize * Coin.pixelsPerUnit / Coin.rect.width * 0.7f, SpriteSize * Coin.pixelsPerUnit / Coin.rect.width * 0.7f, 1f);
			dropItem.dropItem = gameObject2;
			break;
		}
		case rewardType.Chest:
		{
			SpellIcon.GetComponent<SpriteRenderer>().sprite = Chest;
			SpellIcon.name = dropItem.id.ToString();
			GameObject gameObject2 = UnityEngine.Object.Instantiate(SpellIcon, new Vector3(position.x, position.y, BackGround.transform.position.z - 0.0001f), Quaternion.identity, BackGround.transform);
			gameObject2.transform.localScale = new Vector3(SpriteSize * Chest.pixelsPerUnit / Chest.rect.width, SpriteSize * Chest.pixelsPerUnit / Chest.rect.width, 1f);
			dropItem.dropItem = gameObject2;
			break;
		}
		}
		if (AddToDic)
		{
			rewardsDrop.Add(i, dropItem);
		}
		return BackGround;
		void InitOneStar()
		{
			dropItem.star1 = UnityEngine.Object.Instantiate(SpellIconStar, new Vector3(position.x + StarPositionOffset.x, position.y + StarPositionOffset.y, tsf_Layer.transform.position.z - 0.0005f), Quaternion.identity, BackGround.transform);
		}
		void InitTwoStar()
		{
			dropItem.star1 = UnityEngine.Object.Instantiate(SpellIconStar, new Vector3(position.x + StarPositionOffset.x, position.y + StarPositionOffset.y, tsf_Layer.transform.position.z - 0.0005f), Quaternion.identity, BackGround.transform);
			dropItem.star2 = UnityEngine.Object.Instantiate(SpellIconStar, new Vector3(position.x + SecondStarPositionOffset.x, position.y + SecondStarPositionOffset.y, tsf_Layer.transform.position.z - 0.05f), Quaternion.identity, BackGround.transform);
		}
	}

	public Color GetBackgoundColor(RussianDrop drop)
	{
		return ColorAll;
	}

	private IEnumerator Glow()
	{
		yield return new WaitForFixedUpdate();
		float _Time = 1.5f;
		while (coroutineSpin != null)
		{
			float time2 = 0f;
			while (time2 < _Time)
			{
				time2 += Time.deltaTime;
				if (time2 > _Time)
				{
					time2 = _Time;
				}
				Color value = Color.Lerp(ColorNormal, ColorLight, time2 / _Time);
				Highlight.material.SetColor("_MainColor", value);
				yield return new WaitForFixedUpdate();
			}
			time2 = 0f;
			while (time2 < _Time)
			{
				time2 += Time.deltaTime;
				if (time2 > _Time)
				{
					time2 = _Time;
				}
				Color value2 = Color.Lerp(ColorLight, ColorNormal, time2 / _Time);
				Highlight.material.SetColor("_MainColor", value2);
				yield return new WaitForFixedUpdate();
			}
		}
		coroutineGlow = null;
	}

	public void OnDestroy()
	{
		StopAllCoroutines();
		EventMgr.SoundVolumeChange = (Action)Delegate.Remove(EventMgr.SoundVolumeChange, new Action(SoundVolumeChange));
		foreach (RussianDrop value in rewardsDrop.Values)
		{
			switch (value.rewardtype)
			{
			case rewardType.RelicCommon:
			case rewardType.RelicRare:
			case rewardType.RelicEpic:
				if (PlayerMgr.Inst.BaData != null)
				{
					PlayerMgr.Inst.BaData.BackRelicToPool(value.id, 1);
				}
				break;
			case rewardType.Curse:
				if (PlayerMgr.Inst.BaData != null)
				{
					PlayerMgr.Inst.BaData.BackCurseToPool(value.id, 1);
				}
				break;
			}
		}
	}
}
