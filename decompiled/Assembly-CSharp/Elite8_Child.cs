using UnityEngine;

public class Elite8_Child : UnitBase
{
	public Elite8_ChildChain group;

	public Elite8_CoreChain core;

	public Elite8 Master;

	[Header("和谐模式")]
	public SpriteRenderer sr;

	public Sprite sprite_H;

	private Vector3 roomCenterPoint;

	private float roomWidth;

	private float roomHeight;

	public override void SingleInitialCallback()
	{
		if (LevelMgr.Inst.CurrentRoomCtrller.roomCfg.themeType == RoomThemeType.Theme6_Chapter3 || LevelMgr.Inst.CurrentRoomCtrller.roomCfg.themeType == RoomThemeType.Theme22_Chapter3_Shortcut1)
		{
			roomCenterPoint = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint;
			roomWidth = LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme6Width;
			roomHeight = LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme6Height;
		}
		if (GameMgr.IsHarmony_Static)
		{
			sr.sprite = sprite_H;
		}
	}

	public override void EveryInitialCallback()
	{
		UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
		componentData.CanTouch = false;
		SetComponentData(componentData);
	}

	public override void Update()
	{
		SyncDotsPosition();
		base.Update();
		_ = base.IsLocked;
	}

	public void DieWithinBorder()
	{
		TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(myPpt.myEntity);
		info.dontPlayDeadSE = true;
		DotsAnnouncedDeath(info);
	}

	public override void AfterDead(ref TakeDamageInfo_Dots info)
	{
		base.AfterDead(ref info);
		if (group != null)
		{
			group.ChildReportDeath(this);
		}
		if (core != null)
		{
			core.ChildReportDeath();
		}
	}

	public override void AnimaAction(string animaName)
	{
		if (animaName == "IdleDone")
		{
			base.Anima.Play("Elite_8_Child_Aim");
		}
	}
}
