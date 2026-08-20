using UnityEngine;

public class Door_T0 : DoorBase
{
	[Space(50f)]
	public GameObject go_Highlight;

	public Animator anima;

	public AnimaEvent animaEvent;

	public Collider collide;

	public SpriteRenderer sr;

	public SpriteRenderer sr_Reward;

	public Sprite sprite_Ruined;

	private void DoAction(string animaName)
	{
		if (animaName == "OpenFinish")
		{
			collide.enabled = true;
		}
		else
		{
			Debug.LogError(animaName);
		}
	}

	public override void Initialize(RoomController belongRoom, LevelRewardType rewardType)
	{
		base.Initialize(belongRoom, rewardType);
		animaEvent.DoAction = DoAction;
	}

	public override void UpdateDisplay()
	{
		string text = "Textures/LevelReward/" + (int)base.RewardType;
		string nameH = "Textures/LevelReward/" + (int)base.RewardType + "H";
		if (base.RewardType == LevelRewardType.Ruined)
		{
			sr.sprite = sprite_Ruined;
			Object.Destroy(anima);
		}
		else if (PlayerMgr.Inst.ItemCtrller.curse_IsInvisibleDoor)
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
			anima.SetTrigger("Open");
			DoorOpenSE();
		}
	}

	public override void OpenDirect()
	{
		if (base.RewardType != LevelRewardType.Ruined && belongRoom.AllLevelRewardPicked)
		{
			anima.SetTrigger("OpenDirect");
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
		if (GameMgr.IsMobile_Static)
		{
			collide.enabled = false;
		}
		Debug.LogError("已替换为Dots");
	}
}
