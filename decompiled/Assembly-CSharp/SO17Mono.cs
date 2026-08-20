using Spine.Unity;
using UnityEngine;

public class SO17Mono : LayerCorrect
{
	[Space(50f)]
	public SkeletonAnimation sAnima;

	public SkeletonAnimation sAnima_Outline;

	public Material mat_Original;

	public Material mat_Outline;

	[Header("Base")]
	public Transform tsf_LayerBase;

	public SpriteRenderer sr_Base;

	public Sprite[] sr_ThemeBases;

	public Sprite T11_H;

	public override void OnEnable()
	{
		base.OnEnable();
		sAnima_Outline.gameObject.SetActive(value: true);
		sAnima.AnimationState.SetAnimation(0, "Idle", loop: true);
		sAnima_Outline.AnimationState.SetAnimation(0, "Idle", loop: true);
		tsf_LayerBase.transform.position = Tool2D.GetLayerPoint(base.transform, LayerCorrectType.Tile9_AboveAO);
		sr_Base.sprite = sr_ThemeBases[(int)LevelMgr.Inst.CurrentRoomCfg.themeType];
		if (GameMgr.IsHarmony_Static && LevelMgr.Inst.CurrentRoomCfg.themeType == RoomThemeType.Theme11_Chapter5)
		{
			sr_Base.sprite = T11_H;
		}
		sr_Base.transform.position = Tool2D.GetLayerPoint(base.transform, LayerCorrectType.Tile9_AboveAO);
	}

	public void Select()
	{
		sAnima_Outline.CustomMaterialOverride.Add(mat_Original, mat_Outline);
	}

	public void Unselect()
	{
		sAnima_Outline.CustomMaterialOverride.Remove(mat_Original);
	}

	public void AnimaClose()
	{
		sAnima.AnimationState.SetAnimation(0, "Closing", loop: false);
		sAnima_Outline.gameObject.SetActive(value: false);
	}
}
