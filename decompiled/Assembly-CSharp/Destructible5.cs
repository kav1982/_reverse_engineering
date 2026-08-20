using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;

public class Destructible5 : UnitBase, IRoomCtrller
{
	[Space(50f)]
	public float calculateInterval;

	public float recordTime;

	public Text text_DPS;

	public Shadow shadow;

	public int mobileFontSize = 28;

	private List<float> times = new List<float>();

	private List<float> damages = new List<float>();

	private float damageCounter;

	private float calculateIntervalTimer;

	private bool isAdvancedScarecrow;

	private BigInteger highestDPS;

	private float highestOneHit;

	public float AbnormalStatePurifyInterval;

	private float abnormalStatePurifyTimer;

	[Header("各种皮肤")]
	public SpriteRenderer sr_Self;

	public Sprite normalSprite;

	public Sprite frogSprite;

	public Sprite HalloweenSprite;

	public Sprite SummerSprite;

	public Sprite ChristmasSprite;

	public Sprite SpringSprite;

	private RoomController belongRoomCtrller;

	public override void SingleInitialCallback()
	{
		isAdvancedScarecrow = DataMgr.selectedWorldData.GetResearchValueConsiderActive(ResearchAbilityType.AdvancedScarecrow) != 0;
		if (GameMgr.IsMobile_Static)
		{
			text_DPS.fontSize = mobileFontSize;
			text_DPS.resizeTextMaxSize = mobileFontSize;
		}
	}

	public void SetRoomCtrlller(RoomController roomCtrller)
	{
		belongRoomCtrller = roomCtrller;
		if (belongRoomCtrller != LevelMgr.Inst.CurrentRoomCtrller)
		{
			LevelMgr.Inst.CurrentRoomCtrller.UnitUnregister(myPpt.myEntity);
			belongRoomCtrller.UnitRegister(myPpt.myEntity);
		}
	}

	public override void EveryInitialCallback()
	{
		base.Anima.Play("Idle");
		times.Clear();
		damages.Clear();
		UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
		componentData.CanTouch = false;
		SetComponentData(componentData);
		calculateIntervalTimer = 0f;
		abnormalStatePurifyTimer = 0f;
		highestDPS = 0;
		highestOneHit = 0f;
		sr_Self.sprite = normalSprite;
		CheckSprite();
		belongRoomCtrller = LevelMgr.Inst.CurrentRoomCtrller;
		if (!DataMgr.selectedWorldData.isScarecrowOpen)
		{
			belongRoomCtrller.UnitUnregister(myPpt.myEntity);
			myPpt.tsf_Layer.gameObject.SetActive(value: false);
			myPpt.CC_Self.enabled = false;
			SetDotsCCEnable(isOpen: false);
			shadow.ShadowGO.SetActive(value: false);
		}
	}

	private void OnEnable()
	{
		EventMgr.ScarecrowChange = (Action)Delegate.Combine(EventMgr.ScarecrowChange, new Action(ScarecrowChange));
	}

	private void OnDisable()
	{
		EventMgr.ScarecrowChange = (Action)Delegate.Remove(EventMgr.ScarecrowChange, new Action(ScarecrowChange));
	}

	private void CheckSprite()
	{
		Sprite sprite = normalSprite;
		if (GameMgr.CampSkinType == CampSkinType.Halloween)
		{
			sprite = HalloweenSprite;
		}
		else if (GameMgr.CampSkinType == CampSkinType.Spring)
		{
			sprite = SpringSprite;
		}
		else if (GameMgr.CampSkinType == CampSkinType.Christmas)
		{
			sprite = ChristmasSprite;
		}
		else if (GameMgr.CampSkinType == CampSkinType.Summer)
		{
			sprite = SummerSprite;
		}
		else if (DataMgr.selectedWorldData.playerLook == PlayerLook.Frog && PlayerMgr.Inst.ItemCtrller.uiRelic_WarmSnow == null && PlayerMgr.Inst.ItemCtrller.relic_Reaper == null && PlayerMgr.Inst.ItemCtrller.relic_Huang == null)
		{
			sprite = frogSprite;
		}
		if (sr_Self.sprite != sprite && sprite != null)
		{
			sr_Self.sprite = sprite;
		}
	}

	private void ScarecrowChange()
	{
		ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_ScarecrowChange", base.transform.position, 2f);
		if (DataMgr.selectedWorldData.isScarecrowOpen)
		{
			belongRoomCtrller.UnitRegister(myPpt.myEntity);
			myPpt.tsf_Layer.gameObject.SetActive(value: true);
			myPpt.CC_Self.enabled = true;
			SetDotsCCEnable(isOpen: true);
			shadow.ShadowGO.SetActive(value: true);
			times.Clear();
			damages.Clear();
			calculateIntervalTimer = 0f;
			highestDPS = 0;
			highestOneHit = 0f;
			abnormalStatePurifyTimer = 0f;
			text_DPS.text = 1002409.GetText(forceApplyAlogia: true) + ":" + highestOneHit.ToString("F0") + "\n" + 1002408.GetText(forceApplyAlogia: true) + ":" + highestDPS.ToString("F0") + "\n" + 14000302.GetText(forceApplyAlogia: true) + ":0";
			UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
			componentData.CanBeTarget = true;
			componentData.disabled = false;
			SetComponentData(componentData);
		}
		else
		{
			belongRoomCtrller.UnitUnregister(myPpt.myEntity);
			myPpt.tsf_Layer.gameObject.SetActive(value: false);
			myPpt.CC_Self.enabled = false;
			SetDotsCCEnable(isOpen: false);
			myPpt.PurifyAbnormalState();
			shadow.ShadowGO.SetActive(value: false);
			UnitProperty_Dots componentData2 = GetComponentData<UnitProperty_Dots>();
			componentData2.CanBeTarget = false;
			componentData2.disabled = true;
			componentData2.PurifyAbnormalState();
			SetComponentData(componentData2);
		}
	}

	public override void Update()
	{
		CheckSprite();
		base.Update();
		if (!EntityIsValid(myPpt.myEntity))
		{
			return;
		}
		UpdateAbnormalStateTimer();
		UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
		componentData.unitCfg.currentHP = componentData.unitCfg.maxHP;
		SetComponentData(componentData);
		calculateIntervalTimer += Time.deltaTime;
		if (!(calculateIntervalTimer >= calculateInterval))
		{
			return;
		}
		calculateIntervalTimer -= calculateInterval;
		if (damageCounter > 0f)
		{
			times.Add(Time.timeSinceLevelLoad);
			damages.Add(damageCounter);
			damageCounter = 0f;
		}
		float num = 0f;
		for (int num2 = times.Count - 1; num2 >= 0; num2--)
		{
			if (Time.timeSinceLevelLoad - times[num2] > recordTime)
			{
				times.RemoveAt(num2);
				damages.RemoveAt(num2);
			}
			else
			{
				num += damages[num2];
			}
		}
		float num3 = num / recordTime;
		if (num3 >= 100000f && !CampMgr.Inst)
		{
			SteamAchievementMgr.UnlockAndUpload(SteamAchievementType.DPS100K);
		}
		if (isAdvancedScarecrow)
		{
			if ((BigInteger)num3 > highestDPS)
			{
				highestDPS = (BigInteger)num3;
			}
			text_DPS.text = 1002409.GetText() + ":" + highestOneHit.FormatWithUnit() + "\n" + 1002408.GetText() + ":" + highestDPS.FormatWithUnit() + "\n" + 14000302.GetText() + ":" + num3.FormatWithUnit();
		}
		else
		{
			text_DPS.text = 14000302.GetText() + ":" + num3.FormatWithUnit();
		}
	}

	private void UpdateAbnormalStateTimer()
	{
		UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
		if (componentData.affect_VenomCurrentStack > 0f)
		{
			abnormalStatePurifyTimer += Time.deltaTime;
		}
		else
		{
			abnormalStatePurifyTimer = 0f;
		}
		if (abnormalStatePurifyTimer >= AbnormalStatePurifyInterval)
		{
			abnormalStatePurifyTimer = 0f;
			componentData.ClearVenomState();
			componentData.UpdateBodyColor();
			SetComponentData(componentData);
			SpawnAbnormalStatePurifyText();
		}
	}

	private void SpawnAbnormalStatePurifyText()
	{
		UITextFloat component = ObjPoolMgr.Inst.GetUIGO("Prefabs/UI/UITextFloat").GetComponent<UITextFloat>();
		string str = 1002034.GetText().Replace("{float}", AbnormalStatePurifyInterval.ToString());
		component.Initialize(str, UITextFloatType.Normal, base.transform.position + new UnityEngine.Vector3(0f, 1f, 0f));
	}

	public override void AnimaAction(string animaName)
	{
		if (animaName == "BeHitFinish")
		{
			base.Anima.SetTrigger("Idle");
		}
		else
		{
			Debug.LogError(animaName);
		}
	}

	public override void BeforeAnnouncedDeath_Dots(ref TakeDamageInfo_Dots info)
	{
		base.BeforeAnnouncedDeath_Dots(ref info);
		info.stopAnnouncedDeath = true;
		UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
		componentData.unitCfg.currentHP = componentData.unitCfg.maxHP;
		SetComponentData(componentData);
		damageCounter += info.damage;
		if (isAdvancedScarecrow && info.damage > highestOneHit)
		{
			highestOneHit = info.damage;
		}
	}

	public override void AfterTakeDamage_Dots(ref TakeDamageInfo_Dots info)
	{
		base.Anima.SetTrigger("BeHit");
		damageCounter += info.damage;
		if (isAdvancedScarecrow && info.damage > highestOneHit)
		{
			highestOneHit = info.damage;
		}
	}
}
