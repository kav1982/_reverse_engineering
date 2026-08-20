using System;
using Unity.Entities;
using UnityEngine;

public class SpecialObj305EndlessSpellCombiner : InteractiveObj
{
	public CapsuleCollider CC;

	public SpriteRenderer outline;

	public GameObject Model;

	public GameObject ModelBottom;

	private Entity interactiveEntity;

	private void Start()
	{
		outline.enabled = false;
		interactiveEntity = RegisterDotsInteractiveObj(CC, InteractiveObjType.SO305EndlessCompound);
		Hide();
	}

	private void Update()
	{
	}

	private void Show()
	{
		if (SpecialObj301EndlessMonsterSpawner.Inst.HaveSpellProcessor)
		{
			Model.SetActive(value: true);
			ModelBottom.SetActive(value: true);
			SetDotsObjLayer(interactiveEntity, isOpen: true);
		}
	}

	private void Hide()
	{
		Model.SetActive(value: false);
		ModelBottom.SetActive(value: false);
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
			GameUISingletonMono<UICompound>.ShowInit(interactiveEntity);
		}
	}
}
