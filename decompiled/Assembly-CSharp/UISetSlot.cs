using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;

public class UISetSlot : MonoBehaviour
{
	public SkeletonGraphic sGraphic;

	public Text text_Name;

	public Text text_Des;

	public Color color_Unselected;

	public Image WandImage;

	public GameObject Wand5_Orbit;

	public GameObject pfb_RelicDefaultInSet;

	private UISet uiSet;

	private int id;

	private bool isFake;

	private GameObject go_RelicDefaultInSet;

	private Image[] images_RelicDefault;

	public SetConfig Cfg => SetConfig.dic[id];

	public void Initialize(UISet uiSet, int id, bool isFake, bool GiftSet = false)
	{
		this.uiSet = uiSet;
		this.id = id;
		this.isFake = isFake;
		HideDes();
		if (Cfg.relicID != 0)
		{
			if (RelicConfig.dic[Cfg.relicID].skinName != "")
			{
				PlayerSkinMgr.Inst.SetSkin(sGraphic.Skeleton, DataMgr.selectedWorldData.playerLook, new List<RelicConfig> { RelicConfig.dic[Cfg.relicID] }, ignoreDisableRelicSkin: true);
			}
			else
			{
				PlayerSkinMgr.Inst.SetSkin(sGraphic.Skeleton, DataMgr.selectedWorldData.playerLook, new List<RelicConfig>(), ignoreDisableRelicSkin: true);
			}
		}
		else
		{
			PlayerSkinMgr.Inst.SetSkin(sGraphic.Skeleton, DataMgr.selectedWorldData.playerLook, new List<RelicConfig>(), ignoreDisableRelicSkin: true);
		}
		switch (id)
		{
		case 5:
			sGraphic.Skeleton.FindSlot("tui_l").A = 0f;
			sGraphic.Skeleton.FindSlot("tui_r").A = 0f;
			sGraphic.Skeleton.FindSlot("xiaotui_l").A = 0f;
			sGraphic.Skeleton.FindSlot("xiaotui_r").A = 0f;
			sGraphic.Skeleton.FindSlot("xie_l").A = 0f;
			sGraphic.Skeleton.FindSlot("xie_r").A = 0f;
			sGraphic.Skeleton.FindSlot("bilibili_xie_l").A = 0f;
			sGraphic.Skeleton.FindSlot("bilibili_xie_r").A = 0f;
			sGraphic.Skeleton.FindSlot("Hand_L").A = 0f;
			sGraphic.Skeleton.FindSlot("Hand_R").A = 0f;
			sGraphic.Skeleton.FindSlot("dave_tui_l").A = 0f;
			sGraphic.Skeleton.FindSlot("dave_tui_r").A = 0f;
			sGraphic.Skeleton.FindSlot("dave_xiaotui_l").A = 0f;
			sGraphic.Skeleton.FindSlot("dave_xiaotui_r").A = 0f;
			sGraphic.Skeleton.FindSlot("dave_xie_l").A = 0f;
			sGraphic.Skeleton.FindSlot("dave_xie_r").A = 0f;
			break;
		case 7:
		case 8:
		case 11:
			PlayerSkinMgr.Inst.SetSkin(sGraphic.Skeleton, DataMgr.selectedWorldData.playerLook, new List<RelicConfig> { RelicConfig.dic[Cfg.relicID] }, ignoreDisableRelicSkin: true);
			break;
		case 9:
			sGraphic.gameObject.SetActive(value: false);
			go_RelicDefaultInSet = Object.Instantiate(pfb_RelicDefaultInSet, base.transform);
			images_RelicDefault = go_RelicDefaultInSet.GetComponentsInChildren<Image>();
			break;
		}
		CheckSelect(GiftSet);
	}

	public void CheckSelect(bool GiftSet = false)
	{
		WandImage.sprite = ABResources.LoadAsset<Sprite>(WandConfig.dic[Cfg.WandIDs[0]].GetIconPath());
		if (DataMgr.selectedWorldData.IsSetUnlocked(id) || GiftSet)
		{
			text_Name.text = Cfg.GetName();
			text_Name.color = Color.white;
			if (id == 9)
			{
				for (int i = 0; i < images_RelicDefault.Length; i++)
				{
					images_RelicDefault[i].color = Color.white;
				}
			}
			else
			{
				sGraphic.color = Color.white;
				WandImage.color = Color.white;
			}
			return;
		}
		text_Name.text = "???";
		text_Name.color = Color.black;
		HideDes();
		if (id == 9)
		{
			for (int j = 0; j < images_RelicDefault.Length; j++)
			{
				images_RelicDefault[j].color = Color.black;
			}
		}
		else
		{
			sGraphic.color = Color.black;
			WandImage.color = Color.black;
		}
	}

	public void Resetname(bool GiftSet = false)
	{
		if (DataMgr.selectedWorldData.setUnlockedSets.ContainsKey(id) || GiftSet)
		{
			text_Name.text = Cfg.GetName();
			return;
		}
		text_Name.text = "???";
		text_Name.color = Color.black;
		sGraphic.color = Color.black;
	}

	public void zoomin(float yoffset)
	{
		ShowDes();
		base.transform.localScale = new Vector3(1.4f, 1.4f, 1f);
		base.transform.localPosition = new Vector3(base.transform.localPosition.x, base.transform.localPosition.y + yoffset, base.transform.localPosition.z);
	}

	public void zoomout(float yoffset)
	{
		HideDes();
		base.transform.localScale = new Vector3(1f, 1f, 1f);
		base.transform.localPosition = new Vector3(base.transform.localPosition.x, base.transform.localPosition.y - yoffset, base.transform.localPosition.z);
	}

	public void ShowDes()
	{
		text_Des.text = Cfg.GetDes();
	}

	public void HideDes()
	{
		text_Des.text = "";
	}
}
