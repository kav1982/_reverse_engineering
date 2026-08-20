using Spine;
using Spine.Unity;
using UnityEngine;

public class Door_T11 : DoorBase
{
	[Space(50f)]
	public GameObject go_Highlight;

	public GameObject go_Ruined;

	public GameObject go_Portal;

	public SkeletonAnimation sAnima;

	public Collider collide;

	public SpriteRenderer sr_Reward;

	private void SAnimaEvent(TrackEntry trackEntry, Spine.Event e)
	{
		if (e.String == "OpenFinish")
		{
			collide.enabled = true;
		}
		else
		{
			Debug.LogError(e.String);
		}
	}

	public override void Initialize(RoomController roomCtrller, LevelRewardType rewardType)
	{
		base.Initialize(roomCtrller, rewardType);
		sAnima.AnimationState.Event += SAnimaEvent;
	}

	public override void UpdateDisplay()
	{
		string text = "Textures/LevelReward/" + (int)base.RewardType;
		string nameH = "Textures/LevelReward/" + (int)base.RewardType + "H";
		if (base.RewardType == LevelRewardType.Ruined)
		{
			go_Ruined.SetActive(value: true);
			go_Portal.SetActive(value: false);
			sAnima.gameObject.SetActive(value: false);
			return;
		}
		go_Portal.SetActive(value: true);
		if (PlayerMgr.Inst.ItemCtrller.curse_IsInvisibleDoor)
		{
			sr_Reward.sprite = ABResources.LoadAsset<Sprite>("Textures/LevelReward/" + 200);
		}
		else
		{
			sr_Reward.sprite = ABResources.LoadHarmonizableAsset<Sprite>(text, nameH);
		}
	}

	public override void ResetType(LevelRewardType rewardType)
	{
		if (base.RewardType != LevelRewardType.Ruined && base.RewardType != LevelRewardType.Chapter)
		{
			base.RewardType = rewardType;
			UpdateDisplay();
		}
	}

	public override void Open()
	{
		if (base.RewardType != LevelRewardType.Ruined && belongRoom.AllLevelRewardPicked)
		{
			sAnima.AnimationState.SetAnimation(0, "Open", loop: false);
			DoorOpenSE();
		}
	}

	public override void OpenDirect()
	{
		if (base.RewardType != LevelRewardType.Ruined && belongRoom.AllLevelRewardPicked)
		{
			sAnima.AnimationState.SetAnimation(0, "OpenDirect", loop: false);
			collide.enabled = true;
		}
	}

	public override void Select()
	{
		go_Highlight.SetActive(value: true);
	}

	public override void Unselect()
	{
		go_Highlight.SetActive(value: false);
	}

	public override void Interact()
	{
		Debug.LogError("已替换为Dots");
	}
}
