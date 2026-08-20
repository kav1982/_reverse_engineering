using UnityEngine;

public class StoryUnlock : MonoBehaviour
{
	private enum StoryState
	{
		Born,
		WaitHD,
		WaitEnd
	}

	public float dialogue_delay = 0.8f;

	public float camFocusTime;

	public float efWaitTime;

	private StoryState state;

	private StoryUnlockUIType uiType;

	private float waitTimer;

	private int hdID;

	private void Update()
	{
		switch (state)
		{
		case StoryState.Born:
			switch (uiType)
			{
			case StoryUnlockUIType.Research:
				GameUISingletonMono<UIResearch>.Inst.Hide();
				break;
			case StoryUnlockUIType.Trainning:
				GameUISingletonMono<UITraining>.Inst.Hide();
				break;
			default:
				Debug.LogError(uiType);
				break;
			}
			PlayerMgr.Inst.PlayerCtrller.StopMotion();
			PlayerMgr.Inst.InvincibleRegister();
			UIPlayerDataMgr.Inst.Hide();
			state = StoryState.WaitHD;
			break;
		case StoryState.WaitHD:
			waitTimer += Time.deltaTime;
			if (waitTimer >= dialogue_delay)
			{
				waitTimer = 0f;
				state = StoryState.WaitEnd;
				GameUISingletonMono<UIDialogueMgr>.Inst.HDShow(hdID, null, delegate
				{
					UIPlayerDataMgr.Inst.Show();
					PlayerMgr.Inst.PlayerCtrller.StartMotion();
					PlayerMgr.Inst.InvincibleUnregister();
					Object.Destroy(base.gameObject);
				});
			}
			break;
		default:
			Debug.LogError(state);
			break;
		case StoryState.WaitEnd:
			break;
		}
	}

	public void Initialize(int hdID, StoryUnlockUIType uiType)
	{
		this.hdID = hdID;
		this.uiType = uiType;
	}
}
