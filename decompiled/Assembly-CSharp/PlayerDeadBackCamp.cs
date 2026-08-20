using System.Collections.Generic;
using UnityEngine;

public class PlayerDeadBackCamp : MonoBehaviour
{
	public Animator Anima;

	public SpriteRenderer SR;

	public List<Sprite> sprites;

	public List<Material> mats;

	private void Start()
	{
		PlayerMgr.Inst.HideAndDisableControl();
		if (!GameMgr.IsMobile_Static)
		{
			SR.sprite = sprites[(int)GameMgr.CampSkinType];
			SR.material = mats[(int)GameMgr.CampSkinType];
		}
	}

	private void _CreateEF()
	{
		PlayerMgr.Inst.ShowAndEnableControl();
		PlayerMgr.Inst.PlayerCtrller.StopMotion();
		PlayerMgr.Inst.PlayerCtrller.StopFace(isFlip: false);
		PlayerMgr.Inst.PlayerCtrller.SetEmoji(PlayerEmojiType.Amaze);
		ObjPoolMgr.Inst.GetGO("Prefabs/Item/Curse_InjuredRandomPoint", PlayerMgr.Inst.PlayerPointIgnoreZ, 2f);
		SEMgr.Inst.curseInjuredRandomPoint.PlaySE();
	}

	private void _Finish()
	{
		if (((DataMgr.selectedWorldData.deadCount >= 3 && !DataMgr.selectedWorldData.isReachChatper2) || (DataMgr.selectedWorldData.deadCount >= 5 && !DataMgr.selectedWorldData.isReachChatper3)) && !DataMgr.selectedWorldData.showTourHint && !DataMgr.settingData.isTouristMode)
		{
			UICampMgr.Inst.TourHint = Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/UI/UITourHint"), UIMgr.Inst.rtsf_Canvas10.transform);
			DataMgr.selectedWorldData.showTourHint = true;
		}
		else
		{
			PlayerMgr.Inst.PlayerCtrller.SetEmoji(PlayerEmojiType.Normal);
			PlayerMgr.Inst.PlayerCtrller.StartMotion();
		}
		UIPlaceNameMgr.Inst.Show(PlaceNameType.Camp);
		Object.Destroy(base.gameObject);
	}
}
