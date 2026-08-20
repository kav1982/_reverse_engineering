using UnityEngine;

public class Story3 : MonoBehaviour
{
	public Transform tsf_NPC1;

	public Transform triggerStoryCenter;

	public float triggerStoryRadius;

	public float focusSize;

	public float focusTime;

	private bool haveTriggered;

	private void Start()
	{
		CampMgr.Inst.npc1Vivian.sAnima.initialSkinName = "skin2";
		CampMgr.Inst.npc1Vivian.sAnima.Initialize(overwrite: true);
		CampMgr.Inst.npc1Vivian.sAnima_Outline.initialSkinName = "skin2";
		CampMgr.Inst.npc1Vivian.sAnima_Outline.Initialize(overwrite: true);
		CampMgr.Inst.npc1Vivian.Hide();
		CampMgr.Inst.npc2Nimue.HidePlot();
		CampMgr.Inst.npc3.HidePlot();
		CampMgr.Inst.npc4.Show();
		CampMgr.Inst.npc4.HidePlot();
	}

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
			GameUISingletonMono<UIDialogueMgr>.Inst.HDShow(16, HDEvent, delegate
			{
				UIMgr.Inst.uiFade.Show(focusTime, delegate
				{
					CampMgr.Inst.npc4.CheckPlot();
					CampMgr.Inst.npc1Vivian.Show();
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
						GameUISingletonMono<UIUnlockSystem>.ShowInit(UIUnlockSystem.UIUnlockSystemType.Set);
					}
				});
			});
		});
	}

	private void HDEvent(string eventStr)
	{
		if (eventStr == "e1")
		{
			tsf_NPC1.localScale = new Vector3(1f, 1f, 1f);
		}
		else
		{
			Debug.LogError(eventStr);
		}
	}
}
