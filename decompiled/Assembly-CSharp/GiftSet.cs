using UnityEngine;

public class GiftSet : InteractiveObj
{
	public int BloodRewardCount;

	[Space(50f)]
	public GameObject go_Outline;

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
		if (DataMgr.selectedWorldData.IsSetUnlocked(7) && DataMgr.selectedWorldData.IsSetUnlocked(8) && DataMgr.selectedWorldData.IsSetUnlocked(9))
		{
			DataMgr.selectedWorldData.useGift = true;
			PlayerMgr.Inst.ChangeAncientBlood(BloodRewardCount);
			DataMgr.SaveSelectedWorldData();
			Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/UI/UIGetNewSuit"), UIMgr.Inst.rtsf_Canvas1.transform).GetComponent<UI_GetSuitPopOut>().text.text = 1006304.GetText() + 1000011.GetText() + "x" + BloodRewardCount;
			CampMgr.Inst.SetEttEnable(CampMgr.Inst.CurrentCampSkin.ett_GiftSet, enable: false);
		}
		else
		{
			GameUISingletonMono<UIChoseGiftSet>.ShowInit();
		}
	}
}
