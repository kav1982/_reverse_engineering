using UnityEngine;

public class Monster999_Mother : UnitBase
{
	public SpriteRenderer sr_Gizmo;

	private bool createdPortal;

	public override void SingleInitialCallback()
	{
		myPpt.InvincibleRegister();
		sr_Gizmo.gameObject.SetActive(value: false);
	}

	public override void EveryInitialCallback()
	{
		createdPortal = false;
		myPpt.InvincibleRegister();
		myPpt.CanTouch = false;
		myPpt.CanBeTarget = false;
	}

	public override void Frame1InitialCallback()
	{
		Debug.Log("这个地方应该将额外掉落物清空，现在掉落物走的是Dots，所以这个怪物有了Dots在DOts里写");
		float num = 0f;
		for (int i = 0; i < LevelMgr.Inst.CurrentRoomCtrller.TargetablePpts.Count; i++)
		{
			if (LevelMgr.Inst.CurrentRoomCtrller.TargetablePpts[i] != myPpt)
			{
				num += LevelMgr.Inst.CurrentRoomCtrller.TargetablePpts[i].unitCfg.maxHP;
				LevelMgr.Inst.CurrentRoomCtrller.TargetablePpts[i].GetComponent<Monster999>()?.SetMother(this);
			}
		}
		myPpt.unitCfg.maxHP = num;
		myPpt.unitCfg.currentHP = num;
		GameUISingletonMono<UIBossHP>.HideIfInited();
	}

	public override void Update()
	{
		float num = 0f;
		for (int i = 0; i < LevelMgr.Inst.CurrentRoomCtrller.TargetablePpts.Count; i++)
		{
			if (LevelMgr.Inst.CurrentRoomCtrller.TargetablePpts[i] != myPpt)
			{
				num += LevelMgr.Inst.CurrentRoomCtrller.TargetablePpts[i].unitCfg.currentHP;
			}
		}
		myPpt.unitCfg.currentHP = num;
		if (LevelMgr.Inst.CurrentRoomCtrller.TargetablePpts.Count == 1)
		{
			if (!createdPortal)
			{
				createdPortal = true;
				QuickCreateSystem.Inst.CreateMixedEtt("BackCampPortal", Tool2D.GetNavMeshPointIngoreZ(base.transform.position));
			}
			myPpt.AnnouncedDeath();
		}
		base.Update();
	}

	public override void AfterDead(ref TakeDamageInfo_Dots info)
	{
		base.AfterDead(ref info);
		if (!createdPortal)
		{
			createdPortal = true;
			QuickCreateSystem.Inst.CreateMixedEtt("BackCampPortal", Tool2D.GetNavMeshPointIngoreZ(base.transform.position));
		}
	}

	public void ShowHP()
	{
		GameUISingletonMono<UIBossHP>.ShowInit(myPpt);
		LevelMgr.Inst.CurrentRewardType = LevelRewardType.Store;
	}
}
