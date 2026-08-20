using UnityEngine;

public class MainMenuMgr : MonoBehaviour
{
	public GameObject go_StartToDestroy;

	public GameObject go_PiracyWarningWindow;

	public static MainMenuMgr Inst { get; private set; }

	private void Awake()
	{
		Inst = this;
		Object.Destroy(go_StartToDestroy);
		MusicMgr.Inst.ForcePlayMusic("BGM_MainMenu", playAmbient: false);
		UIMgr.Inst.uiFade.Hide();
		MobileMgr.inst.UpdateActiveSkillButton();
		if (ScriptableObjMgr.Inst.testCtrller.CheckSteam)
		{
			go_PiracyWarningWindow.SetActive(ProgramInfo.CheckSteamDLL() != ProgramInfo.SteamDLLState.Safe);
		}
	}

	private async void Start()
	{
		if (BLiveMgr.Inst != null)
		{
			await BLiveMgr.Inst.Disconnect();
		}
		if (!SteamManager.Initialized)
		{
			SteamManager.SteamInit();
		}
	}
}
