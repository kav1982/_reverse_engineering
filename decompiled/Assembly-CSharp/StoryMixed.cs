using System;
using UnityEngine;

public class StoryMixed : MonoBehaviour
{
	public float camFocusSize;

	public float camFocusTime;

	public float triggerDistace;

	private StoryMixedType type;

	private Vector3 lastMonsterDeadPoint;

	private RoomController belongRoom;

	private void Update()
	{
		switch (type)
		{
		case StoryMixedType.FirstFinishLevel:
			if ((PlayerMgr.Inst.PlayerPoint - lastMonsterDeadPoint).sqrMagnitude < triggerDistace * triggerDistace)
			{
				DataMgr.selectedWorldData.storyMixedFirstFinishLevel = true;
				StartHD(101, (PlayerMgr.Inst.PlayerPoint + lastMonsterDeadPoint) / 2f);
				UnityEngine.Object.Destroy(base.gameObject);
			}
			break;
		case StoryMixedType.FirstEncounterElite:
			if (LevelMgr.Inst.CurrentRoomCtrller.IsFinish && LevelMgr.Inst.CurrentRoomCtrller.AllLevelRewardPicked && (PlayerMgr.Inst.PlayerPoint - LevelMgr.Inst.CurrentRoomCtrller.GetAccessCenterPoint(FourDir.Up)).sqrMagnitude < triggerDistace * triggerDistace)
			{
				DataMgr.selectedWorldData.storyMixedFirstEncounterElite = true;
				StartHD(102, (PlayerMgr.Inst.PlayerPoint + LevelMgr.Inst.CurrentRoomCtrller.GetAccessCenterPoint(FourDir.Up)) / 2f);
				UnityEngine.Object.Destroy(base.gameObject);
			}
			break;
		case StoryMixedType.FirstArriveChapter2:
			if ((PlayerMgr.Inst.PlayerPoint - LevelMgr.Inst.CurrentRoomCtrller.GetAccessCenterPoint(FourDir.Up)).sqrMagnitude < triggerDistace * triggerDistace)
			{
				DataMgr.selectedWorldData.storyMixedFirstEnterChapter2 = true;
				StartHD(103, PlayerMgr.Inst.PlayerPoint);
				UnityEngine.Object.Destroy(base.gameObject);
			}
			break;
		case StoryMixedType.FirstArriveChapter3:
			if ((PlayerMgr.Inst.PlayerPoint - LevelMgr.Inst.CurrentRoomCtrller.GetAccessCenterPoint(FourDir.Up)).sqrMagnitude < triggerDistace * triggerDistace)
			{
				DataMgr.selectedWorldData.storyMixedFirstEnterChapter3 = true;
				StartHD(104, PlayerMgr.Inst.PlayerPoint);
				UnityEngine.Object.Destroy(base.gameObject);
			}
			break;
		case StoryMixedType.FirstArriveChapter4:
			if ((PlayerMgr.Inst.PlayerPoint - LevelMgr.Inst.CurrentRoomCtrller.GetAccessCenterPoint(FourDir.Up)).sqrMagnitude < triggerDistace * triggerDistace)
			{
				DataMgr.selectedWorldData.storyMixedFirstEnterChapter4 = true;
				StartHD(108, PlayerMgr.Inst.PlayerPoint);
				UnityEngine.Object.Destroy(base.gameObject);
			}
			break;
		case StoryMixedType.FirstArriveChapter5:
			if ((PlayerMgr.Inst.PlayerPoint - LevelMgr.Inst.CurrentRoomCtrller.GetAccessCenterPoint(FourDir.Up)).sqrMagnitude < triggerDistace * triggerDistace)
			{
				DataMgr.selectedWorldData.storyMixedFirstEnterChapter5 = true;
				StartHD(109, PlayerMgr.Inst.PlayerPoint);
				UnityEngine.Object.Destroy(base.gameObject);
			}
			break;
		case StoryMixedType.FirstEnterBloodRoom:
			if (LevelMgr.Inst.CurrentRoomCtrller == belongRoom && (PlayerMgr.Inst.PlayerPoint - belongRoom.CenterPoint).sqrMagnitude < triggerDistace * triggerDistace)
			{
				DataMgr.selectedWorldData.storyMixedFirstEnterBloodRoom = true;
				StartHD(105, PlayerMgr.Inst.PlayerPoint);
				UnityEngine.Object.Destroy(base.gameObject);
			}
			break;
		case StoryMixedType.SecondEnterBattle:
			if ((PlayerMgr.Inst.PlayerPoint - LevelMgr.Inst.CurrentRoomCtrller.GetAccessCenterPoint(FourDir.Up)).sqrMagnitude < triggerDistace * triggerDistace + 1f)
			{
				DataMgr.selectedWorldData.storyMixedSecondEnterBattle = true;
				StartHD(107, PlayerMgr.Inst.PlayerPoint);
				UnityEngine.Object.Destroy(base.gameObject);
			}
			break;
		default:
			Debug.LogError(type);
			break;
		}
	}

	private void StartHD(int hdID, Vector3 focusPoint)
	{
		DataMgr.selectedWorldData.storyMixedFirstFinishLevel = true;
		PlayerMgr.Inst.PlayerCtrller.StopMotion();
		PlayerMgr.Inst.PlayerPpt.InvincibleRegister();
		UIPlayerDataMgr.Inst.Hide();
		CamController.Inst.FocusOn(camFocusSize, camFocusTime, focusPoint);
		UIMgr.Inst.uiFilmBlackEdge.Show(camFocusTime, delegate
		{
			GameUISingletonMono<UIDialogueMgr>.Inst.HDShow(hdID, (Action)delegate
			{
				CamController.Inst.FocusRecover(camFocusTime);
				UIMgr.Inst.uiFilmBlackEdge.Hide(camFocusTime, delegate
				{
					PlayerMgr.Inst.PlayerCtrller.StartMotion();
					PlayerMgr.Inst.PlayerPpt.InvincibleUnregister();
					UIPlayerDataMgr.Inst.Show();
				});
			});
		});
	}

	public void Initialize(StoryMixedType type)
	{
		this.type = type;
	}

	public void Initialize(StoryMixedType type, Vector3 lastMonsterDeadPoint)
	{
		this.type = type;
		this.lastMonsterDeadPoint = lastMonsterDeadPoint;
	}

	public void Initialize(StoryMixedType type, RoomController belongRoom)
	{
		this.type = type;
		this.belongRoom = belongRoom;
	}
}
