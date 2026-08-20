using System;
using UnityEngine;

public class Story_FirstPickPostWand : MonoBehaviour
{
	public GameObject pfb_UIInfoWand;

	public float camFocusSize;

	public float camFocusTime;

	public void Initialize(WandConfig wandCfg)
	{
		DataMgr.selectedWorldData.storyMixedFirstPickPostSlotWand = true;
		PlayerMgr.Inst.PlayerCtrller.StopMotion();
		PlayerMgr.Inst.PlayerPpt.InvincibleRegister();
		UIPlayerDataMgr.Inst.Hide();
		CamController.Inst.FocusOn(camFocusSize, camFocusTime, PlayerMgr.Inst.PlayerPoint);
		UIMgr.Inst.uiFilmBlackEdge.Show(camFocusTime, delegate
		{
			GameObject _ui = UnityEngine.Object.Instantiate(pfb_UIInfoWand, UIBattleMgr.Inst.rtsf_CanvasThings);
			if (GameMgr.IsMobile_Static)
			{
				_ui.GetComponent<RectTransform>().anchoredPosition = new Vector2(UIMgr.Inst.canvas_1Scaler.referenceResolution.x / 2f, -2f * UIMgr.Inst.canvas_1Scaler.referenceResolution.y / 3f);
			}
			else
			{
				_ui.GetComponent<RectTransform>().anchoredPosition = new Vector2(UIMgr.Inst.canvas_1Scaler.referenceResolution.x / 2f, (0f - UIMgr.Inst.canvas_1Scaler.referenceResolution.y) / 2f);
			}
			_ui.GetComponent<UIInfoWand>().UpdateInfo(wandCfg);
			GameUISingletonMono<UIDialogueMgr>.Inst.HDShow(106, (Action)delegate
			{
				UnityEngine.Object.Destroy(_ui);
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
}
