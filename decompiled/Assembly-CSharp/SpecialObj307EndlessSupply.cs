using System;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class SpecialObj307EndlessSupply : InteractiveObj
{
	public CapsuleCollider CC;

	public SpriteRenderer outline;

	public SpriteRenderer main;

	public Sprite loadSprite;

	public Sprite emptySprite;

	private Entity interactiveEntity;

	private void Start()
	{
		outline.enabled = false;
		interactiveEntity = RegisterDotsInteractiveObj(CC, InteractiveObjType.SpecialObj301EndlessMonsterSpawner);
		Hide();
	}

	private void Update()
	{
	}

	private void Show()
	{
		main.sprite = loadSprite;
		SetDotsObjLayer(interactiveEntity, isOpen: true);
	}

	private void Hide()
	{
		main.sprite = emptySprite;
		SetDotsObjLayer(interactiveEntity, isOpen: false);
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
		List<ItemInfo> levelReward = OutputMgr_Dots.GetLevelReward(LevelRewardType.Spell);
		QuickCreateSystem.Inst.CreateItemDrop(LevelMgr.Inst.CurrentRoomMapPos, DTool.ListToBlobArray(levelReward), base.transform.position, 0.5f);
		Hide();
		EventMgr.EndlessStageClear();
	}
}
