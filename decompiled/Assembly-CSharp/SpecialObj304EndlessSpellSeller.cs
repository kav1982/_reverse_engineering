using System;
using Unity.Entities;
using UnityEngine;

public class SpecialObj304EndlessSpellSeller : InteractiveObj
{
	public CapsuleCollider CC;

	public SpriteRenderer outline;

	public GameObject Model;

	private Entity interactiveEntity;

	private void Start()
	{
		outline.enabled = false;
		interactiveEntity = RegisterDotsInteractiveObj(CC, InteractiveObjType.SpecialObj304EndlessSpellSeller);
		Hide();
	}

	private void Update()
	{
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
		if ((bool)UIBattleMgr.Inst)
		{
			GameUISingletonMono<UISell>.ShowInit(interactiveEntity);
		}
	}
}
