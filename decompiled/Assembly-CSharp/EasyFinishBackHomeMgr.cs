using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EasyFinishBackHomeMgr : MonoBehaviour
{
	public GameObject go_StartToDestroy;

	public float camFocusSize;

	public float camFocusTime;

	public Transform tsf_Story1Computer;

	public float story1ComputerMDOffset;

	public static EasyFinishBackHomeMgr Inst { get; private set; }

	private void Awake()
	{
		Inst = this;
	}

	private void OnDestroy()
	{
		Inst = null;
	}

	private void Start()
	{
		PlayerMgr.Inst.CreatePlayer();
		Dictionary<Vector2Int, RoomConfig> dictionary = new Dictionary<Vector2Int, RoomConfig>();
		dictionary.Add(Vector2Int.zero, RoomConfig.GetConfig(102));
		LevelMgr.Inst.CreateLevel(dictionary, LevelRewardType.None, LevelRewardType.None, LevelRewardType.None, fadeDisappear: true, CreateLevelFinishAct);
		Object.Destroy(go_StartToDestroy);
		CamController.Inst.SetFollow(PlayerMgr.Inst.PlayerT);
	}

	private void CreateLevelFinishAct()
	{
		PlayerMgr.Inst.HideAndDisableControl();
		MusicMgr.Inst.ForcePlayMusic("");
		MusicMgr.Inst.ForcePlayAmbient("Ambinet_Guide");
		foreach (KeyValuePair<Vector2Int, RoomController> roomCtrller in LevelMgr.Inst.RoomCtrllers)
		{
			Object.Destroy(roomCtrller.Value.transform.Find("ThemeSpecialize(Clone)").gameObject);
		}
		LevelMgr.Inst.CurrentRoomT.position = new Vector3(0f, 1f, 0f);
		LevelMgr.Inst.CurrentRoomCtrller.Initialize2();
	}

	public void _CamFocus()
	{
		CamController.Inst.FocusOn(camFocusSize, camFocusTime, Vector3.zero);
	}

	public void _StoryComputerTalk(int textID)
	{
		GameUISingletonMono<UIDialogueMgr>.Inst.MDShow(textID, tsf_Story1Computer, story1ComputerMDOffset);
	}

	public void _StoryFinish()
	{
		UIMgr.Inst.uiFade.Show(delegate
		{
			TimeScaleMgr.Inst.ClearAllTimeScaleModifyRequest();
			SceneManager.LoadScene("Camp");
		});
	}
}
