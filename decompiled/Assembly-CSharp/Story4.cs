using UnityEngine;

public class Story4 : MonoBehaviour
{
	public Transform tsf_NPC1;

	public Transform tsf_NPC2;

	public Transform triggerStoryCenter;

	public float triggerStoryRadius;

	public float focusSize;

	public float focusTime;

	private bool haveTriggered;

	private int hdID;

	private void Start()
	{
		CampMgr.Inst.npc1Vivian.Hide();
		CampMgr.Inst.npc2Nimue.Hide();
	}

	private void Update()
	{
		if (haveTriggered || !((PlayerMgr.Inst.PlayerPoint - triggerStoryCenter.position).sqrMagnitude < triggerStoryRadius * triggerStoryRadius))
		{
			return;
		}
		haveTriggered = true;
		PlayerMgr.Inst.PlayerCtrller.StopMotion();
		PlayerMgr.Inst.PlayerCtrller.StopFace(isFlip: false);
		UIPlayerDataMgr.Inst.Hide();
		CamController.Inst.FocusOn(focusSize, focusTime, triggerStoryCenter.position);
		UIMgr.Inst.uiFilmBlackEdge.Show(focusTime, delegate
		{
			GameUISingletonMono<UIDialogueMgr>.Inst.HDShow(hdID, HDEvent, delegate
			{
				UIMgr.Inst.uiFade.Show(focusTime, delegate
				{
					CampMgr.Inst.npc1Vivian.Show();
					CampMgr.Inst.npc2Nimue.Show();
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
						GameUISingletonMono<UIUnlockSystem>.ShowInit(UIUnlockSystem.UIUnlockSystemType.TrainingRoom);
					}
				});
			});
		});
	}

	private void HDEvent(string eventStr)
	{
		if (eventStr == "e1")
		{
			tsf_NPC1.localScale = new Vector3(-1f, 1f, 1f);
			tsf_NPC2.localScale = new Vector3(-1f, 1f, 1f);
		}
		else
		{
			Debug.LogError(eventStr);
		}
	}

	public void Initialize(int hdID)
	{
		this.hdID = hdID;
	}
}
