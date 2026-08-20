using System;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

public class SpecialObj308EndlessReward : InteractiveObj, IRoomObjExtraData
{
	public LevelRewardType sellType;

	public CapsuleCollider CC;

	public SpriteRenderer outline;

	public SpriteRenderer main;

	public List<Sprite> rewardSprite;

	public Sprite waitSprite;

	public Text text_Cost;

	public GameObject Model;

	private Entity interactiveEntity;

	private Entity sellEntity;

	public int initialCost;

	public int eachStageCost;

	private bool sellEntityExists;

	public int finalCost => BattleMgr.Inst.CurrentLevel * eachStageCost + initialCost;

	public bool coinEnough => PlayerMgr.Inst.CoinCount >= finalCost;

	public void SetExtraData(float data1, float data2, float data3)
	{
		sellType = (LevelRewardType)Mathf.Clamp((int)data1, 1, 3);
		main.sprite = rewardSprite[(int)(sellType - 1)];
		outline.sprite = main.sprite;
	}

	private void Start()
	{
		outline.enabled = false;
		interactiveEntity = RegisterDotsInteractiveObj(CC, InteractiveObjType.SpecialObj301EndlessMonsterSpawner);
		Hide();
	}

	public override void OnEnable()
	{
		base.OnEnable();
		EventMgr.EndlessStageStart = (Action)Delegate.Combine(EventMgr.EndlessStageStart, new Action(Hide));
		EventMgr.EndlessStageClear = (Action)Delegate.Combine(EventMgr.EndlessStageClear, new Action(Show));
	}

	private void OnDisable()
	{
		EventMgr.EndlessStageStart = (Action)Delegate.Remove(EventMgr.EndlessStageStart, new Action(Hide));
		EventMgr.EndlessStageClear = (Action)Delegate.Remove(EventMgr.EndlessStageClear, new Action(Show));
	}

	private void Show()
	{
		Model.SetActive(value: true);
		SetDotsObjLayer(interactiveEntity, isOpen: true);
	}

	private void Hide()
	{
		Model.SetActive(value: false);
		SetDotsObjLayer(interactiveEntity, isOpen: false);
	}

	private void Update()
	{
		if ((bool)SpecialObj301EndlessMonsterSpawner.Inst)
		{
			text_Cost.text = finalCost.ToString();
			if (!coinEnough)
			{
				text_Cost.color = Color.red;
			}
			else
			{
				text_Cost.color = Color.green;
			}
			if (!UnitDotsSyncSystem.EntityIsValid(sellEntity) && sellEntityExists)
			{
				SetDotsObjLayer(interactiveEntity, isOpen: true);
				main.sprite = rewardSprite[(int)(sellType - 1)];
				sellEntityExists = false;
				Model.SetActive(value: true);
			}
		}
	}

	public override void Select()
	{
		outline.enabled = true;
	}

	public override void Unselect()
	{
		outline.enabled = false;
	}

	public override void Interact()
	{
		if (coinEnough && !sellEntityExists)
		{
			Vector3 doorToWalkablePoint = LevelMgr.Inst.CurrentRoomCtrller.GetDoorToWalkablePoint(base.transform.position);
			sellEntity = QuickCreateSystem.Inst.CreateLevelReward(sellType, OutputMgr_Dots.GetLevelReward(sellType), doorToWalkablePoint);
			LevelMgr.Inst.CurrentRoomCtrller.LevelRewardRegister(sellEntity);
			sellEntityExists = true;
			PlayerMgr.Inst.ChangeCoin(-finalCost);
			main.sprite = waitSprite;
			Model.SetActive(value: false);
			SetDotsObjLayer(interactiveEntity, isOpen: false);
		}
	}
}
