using Unity.Entities;
using UnityEngine;

public class SpecialObj310EndlessEntrance : InteractiveObj
{
	public CapsuleCollider CC;

	public SpriteRenderer outline;

	public SpriteRenderer main;

	public GameObject Model;

	public static SpecialObj310EndlessEntrance Inst;

	private Entity interactiveEntity;

	private void Start()
	{
		outline.enabled = false;
		Inst = this;
		interactiveEntity = RegisterDotsInteractiveObj(CC, InteractiveObjType.SpecialObj301EndlessMonsterSpawner);
		SetDotsObjLayer(interactiveEntity, isOpen: true);
	}

	private void OnDestroy()
	{
		if (interactiveEntity != Entity.Null)
		{
			SetDotsObjLayer(interactiveEntity, isOpen: false);
		}
	}

	private void Update()
	{
		if (BattleMgr.Inst != null && (BattleMgr.Inst.CurrentLevel != 0 || BattleMgr.Inst.CurrentStage != 1))
		{
			Object.Destroy(base.gameObject);
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
		DoorBase_Dots doorBase = default(DoorBase_Dots);
		doorBase.rewardType = LevelRewardType.EndlessChapter;
		BattleMgr.Inst.PlayerEnterDoor(doorBase);
	}
}
