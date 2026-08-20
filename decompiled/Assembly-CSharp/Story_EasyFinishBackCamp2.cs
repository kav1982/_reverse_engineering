using UnityEngine;

public class Story_EasyFinishBackCamp2 : MonoBehaviour
{
	public Transform tsf_NPC1;

	public Transform tsf_NPC6;

	public Transform triggerStoryCenter;

	public float triggerStoryRadius;

	public float focusSize;

	public float focusTime;

	private bool haveTriggered;

	private void Update()
	{
		if (haveTriggered || !((PlayerMgr.Inst.PlayerPoint - triggerStoryCenter.position).sqrMagnitude < triggerStoryRadius * triggerStoryRadius))
		{
			return;
		}
		haveTriggered = true;
		PlayerMgr.Inst.PlayerCtrller.StopMotion();
		PlayerMgr.Inst.PlayerCtrller.StopFace(isFlip: true);
		UIPlayerDataMgr.Inst.Hide();
		CamController.Inst.FocusOn(focusSize, focusTime, triggerStoryCenter.position);
		UIMgr.Inst.uiFilmBlackEdge.Show(focusTime, delegate
		{
			GameUISingletonMono<UIDialogueMgr>.Inst.HDShow(40, HDEvent, delegate
			{
				UIMgr.Inst.uiFade.Show(focusTime, delegate
				{
					CampMgr.Inst.npc1Vivian.Show();
					CampMgr.Inst.npc6.Show();
					CamController.Inst.FocusRecover(0f);
					UIMgr.Inst.uiFilmBlackEdge.Hide(0f);
					PlayerMgr.Inst.PlayerCtrller.StartMotion();
					Object.Destroy(base.gameObject);
					UIMgr.Inst.uiFade.Hide(focusTime, delegate
					{
						UIPlayerDataMgr.Inst.Show();
					});
					if (GameMgr.IsMobile_Static)
					{
						GameUISingletonMono<UIUnlockSystem>.ShowInit(UIUnlockSystem.UIUnlockSystemType.ActivateGirl);
					}
				});
			});
		});
	}

	private void HDEvent(string eventStr)
	{
		if (eventStr == "e1")
		{
			Vector3 vector = PlayerMgr.Inst.PlayerPoint - tsf_NPC1.position;
			Vector3 vector2 = PlayerMgr.Inst.PlayerPoint - tsf_NPC6.position;
			tsf_NPC1.localScale = new Vector3((vector.x > 0f) ? 1 : (-1), 1f, 1f);
			tsf_NPC6.localScale = new Vector3((vector2.x > 0f) ? 1 : (-1), 1f, 1f);
		}
		else
		{
			Debug.LogError(eventStr);
		}
	}
}
