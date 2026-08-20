using Spine.Unity;
using UnityEngine;

public class SpecialObj17 : InteractiveObj
{
	[Space(50f)]
	public SkeletonAnimation sAnima;

	public SkeletonAnimation sAnima_Outline;

	public Material mat_Original;

	public Material mat_Outline;

	[Header("Base")]
	public SpriteRenderer sr_Base;

	public Sprite[] sr_ThemeBases;

	public Sprite T11_H;

	private void Start()
	{
		sr_Base.sprite = sr_ThemeBases[(int)LevelMgr.Inst.CurrentRoomCfg.themeType];
		if (GameMgr.IsHarmony_Static && LevelMgr.Inst.CurrentRoomCfg.themeType == RoomThemeType.Theme11_Chapter5)
		{
			sr_Base.sprite = T11_H;
		}
		sr_Base.transform.position = Tool2D.GetLayerPoint(base.transform, LayerCorrectType.Tile9_AboveAO);
	}

	public string GetName()
	{
		string text = (DataMgr.selectedWorldData.IsDave ? 1001322 : 1001313).GetText();
		bool flag = true;
		for (int i = 0; i < DataMgr.selectedWorldData.researchedIDs.Count; i++)
		{
			if (ResearchConfig.dic[DataMgr.selectedWorldData.researchedIDs[i]].abilityType == ResearchAbilityType.Spring)
			{
				if (flag)
				{
					flag = false;
				}
				else
				{
					text += "+";
				}
			}
		}
		return text;
	}

	public string GetDesc()
	{
		return 1001314.GetText().Replace("int1", DataMgr.selectedWorldData.GetResearchValueConsiderActive(ResearchAbilityType.Spring).ToString());
	}

	public override void Select()
	{
		sAnima_Outline.CustomMaterialOverride.Add(mat_Original, mat_Outline);
	}

	public override void Unselect()
	{
		sAnima_Outline.CustomMaterialOverride.Remove(mat_Original);
	}

	public override void Interact()
	{
		float num = (float)DataMgr.selectedWorldData.GetResearchValueConsiderActive(ResearchAbilityType.Spring) / 100f;
		PlayerMgr.Inst.PlayerPpt.HPRecovery((int)(PlayerMgr.Inst.PlayerPpt.unitCfg.maxHP * num));
		base.tag = "Untagged";
		SEMgr.Inst.so17Drink.PlaySE();
		sAnima.AnimationState.SetAnimation(0, "Closing", loop: false);
		sAnima_Outline.gameObject.SetActive(value: false);
	}
}
