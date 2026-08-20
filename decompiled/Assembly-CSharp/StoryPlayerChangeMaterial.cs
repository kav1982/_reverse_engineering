using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;

public class StoryPlayerChangeMaterial : MonoBehaviour
{
	public SkeletonAnimation sAnima;

	public Material mat_Original;

	public Material mat_Replacement;

	[Header("Relic_Huang")]
	public LayerCorrect lc_Player;

	private Relic_Huang relic_Huang;

	private void Start()
	{
		sAnima.CustomMaterialOverride.Add(mat_Original, mat_Replacement);
		PlayerSkinMgr.Inst.SetSkin(sAnima.skeleton, DataMgr.selectedWorldData.playerLook, PlayerMgr.Inst.BaData.relicCfgs);
		if (PlayerMgr.Inst.ItemCtrller.relic_MirrorOfSoul != null)
		{
			sAnima.Skeleton.FindSlot("tui_l").A = 0f;
			sAnima.Skeleton.FindSlot("tui_r").A = 0f;
			sAnima.Skeleton.FindSlot("xiaotui_l").A = 0f;
			sAnima.Skeleton.FindSlot("xiaotui_r").A = 0f;
			sAnima.Skeleton.FindSlot("xie_l").A = 0f;
			sAnima.Skeleton.FindSlot("xie_r").A = 0f;
			sAnima.Skeleton.FindSlot("bilibili_xie_l").A = 0f;
			sAnima.Skeleton.FindSlot("bilibili_xie_r").A = 0f;
			sAnima.Skeleton.FindSlot("Hand_L").A = 0f;
			sAnima.Skeleton.FindSlot("Hand_R").A = 0f;
		}
		else if (PlayerMgr.Inst.ItemCtrller.relic_Huang != null)
		{
			sAnima.gameObject.SetActive(value: false);
			relic_Huang = Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Item/Relic_Huang"), base.transform.position, Quaternion.identity, lc_Player.tsf_Layer).GetComponent<Relic_Huang>();
			relic_Huang.Initialize(PlayerMgr.Inst.ItemCtrller.relic_Huang.RelicCfg, inPlot: true);
			relic_Huang.transform.localPosition = Vector3.zero;
		}
		foreach (KeyValuePair<Wand, UnitProperty> autoWand in PlayerMgr.Inst.autoWandList)
		{
			autoWand.Value.transform.HideAllChild();
		}
	}

	private void OnDestroy()
	{
		foreach (KeyValuePair<Wand, UnitProperty> autoWand in PlayerMgr.Inst.autoWandList)
		{
			autoWand.Value.transform.ShowAllChild();
		}
	}

	public void RelicHuangAmaze()
	{
		if (relic_Huang != null)
		{
			relic_Huang.PlotAmaze();
		}
	}

	public void RelicHuangIdle()
	{
		if (relic_Huang != null)
		{
			relic_Huang.PlotIdle();
		}
	}

	public void RelicHuangFaceRight()
	{
		if (relic_Huang != null)
		{
			relic_Huang.PlotFaceRight();
		}
	}

	public void RelicHuangFaceLeft()
	{
		if (relic_Huang != null)
		{
			relic_Huang.PlotFaceLeft();
		}
	}

	public void RelicHuangLie()
	{
		if (relic_Huang != null)
		{
			relic_Huang.PlotLie();
			relic_Huang.PlotFaceLeft();
			relic_Huang.transform.localScale = Vector3.one;
		}
	}

	public void RelicHuangLieUp()
	{
		if (relic_Huang != null)
		{
			relic_Huang.PlotLieUp();
		}
	}
}
