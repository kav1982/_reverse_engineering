using System;
using Unity.Entities;
using UnityEngine;

public class SpecialObj313EndlessStoreLocker : InteractiveObj
{
	public CapsuleCollider CC;

	public SpriteRenderer outline;

	public GameObject Model;

	public GameObject BaseObject;

	private Entity interactiveEntity;

	public Sprite lockSprite;

	public Sprite unlockSprite;

	public SpriteRenderer sr_lock;

	public bool locked;

	public static SpecialObj313EndlessStoreLocker Inst;

	private void Start()
	{
		outline.enabled = false;
		interactiveEntity = RegisterDotsInteractiveObj(CC, InteractiveObjType.SpecialObj313EndlessStoreLocker);
		Hide();
		Inst = this;
	}

	private void Update()
	{
	}

	private void Show()
	{
		if (DataMgr.selectedWorldData.endless_LevelOfLcokMachine <= 0)
		{
			Hide();
			return;
		}
		BaseObject.SetActive(value: true);
		Model.SetActive(value: true);
		SetDotsObjLayer(interactiveEntity, isOpen: true);
	}

	private void Hide()
	{
		BaseObject.SetActive(value: false);
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

	public void SetLock(bool locked)
	{
		if (locked != this.locked)
		{
			this.locked = locked;
			if (locked)
			{
				SEMgr.Inst.endlessStoreLock.PlaySE().pitch = 1.2f;
				SpecialObj301EndlessMonsterSpawner.Inst.LockStoreItems();
				sr_lock.sprite = unlockSprite;
				outline.sprite = unlockSprite;
			}
			else
			{
				SEMgr.Inst.endlessStoreLock.PlaySE().pitch = 1f;
				SpecialObj301EndlessMonsterSpawner.Inst.UnlockStoreItems();
				sr_lock.sprite = lockSprite;
				outline.sprite = lockSprite;
			}
			EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
			InteractiveObj_Dots componentData = entityManager.GetComponentData<InteractiveObj_Dots>(interactiveEntity);
			componentData.type = (locked ? InteractiveObjType.SpecialObj313EndlessStoreLockerUnlock : InteractiveObjType.SpecialObj313EndlessStoreLocker);
			entityManager.SetComponentData(interactiveEntity, componentData);
		}
	}

	public override void Interact()
	{
		SetLock(!locked);
	}
}
