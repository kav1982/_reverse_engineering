using UnityEngine;

public class Door_T8 : DoorBase
{
	[Space(50f)]
	public GameObject go_Outline;

	public Animator anima;

	public Collider collide;

	public SpriteRenderer sr_Base;

	public SpriteRenderer sr_Base2;

	public SpriteRenderer sr_Reward;

	public Sprite sprite_BaseRuined;

	[Header("Harmonious")]
	public Sprite sprite_BaseH;

	public Sprite sprite_Base2H;

	public Sprite sprite_BaseRuinedH;

	private void Start()
	{
		sr_Base.transform.position = Tool2D.GetLayerPoint(base.transform, LayerCorrectType.AccessOpen);
	}

	public override void UpdateDisplay()
	{
		string text = "Textures/LevelReward/" + (int)base.RewardType;
		string nameH = "Textures/LevelReward/" + (int)base.RewardType + "H";
		if (base.RewardType == LevelRewardType.Ruined)
		{
			if (GameMgr.IsHarmony_Static)
			{
				sr_Base.sprite = sprite_BaseRuinedH;
			}
			else
			{
				sr_Base.sprite = sprite_BaseRuined;
			}
			Object.Destroy(anima);
			return;
		}
		if (GameMgr.IsHarmony_Static)
		{
			sr_Base.sprite = sprite_BaseH;
			sr_Base2.sprite = sprite_Base2H;
		}
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
			anima.Play("Open");
			SEMgr.Inst.openDoor_T8.PlaySE(base.transform.position);
		}
	}

	public override void OpenDirect()
	{
		if (base.RewardType != LevelRewardType.Ruined && belongRoom.AllLevelRewardPicked)
		{
			anima.Play("OpenDirect");
			collide.enabled = true;
		}
	}

	public override void Select()
	{
		go_Outline.SetActive(value: true);
	}

	public override void Unselect()
	{
		go_Outline.SetActive(value: false);
	}

	public override void Interact()
	{
		Debug.LogError("已替换为Dots");
	}

	private void _OpenFinish()
	{
		collide.enabled = true;
	}
}
