using System;
using Spine.Unity;
using UnityEngine;
using UnityEngine.Playables;

public class Story_HardFinishBackCamp : MonoBehaviour
{
	private enum StoryState
	{
		Idle,
		CamFocus,
		FocusWait,
		Talk1,
		WaitPopupUI
	}

	public float bornIdleTime;

	public Vector3 focusPos;

	public float focusSize;

	public float focusTime;

	public float focusWaitTime;

	public GameObject go_Player;

	public GameObject go_npc1;

	public GameObject go_npc4;

	public GameObject go_npc5;

	public PlayableDirector pd_Story;

	public SkeletonAnimation sAnima_Player;

	public SkeletonAnimation sAnima_NPC1;

	public SkeletonAnimation sAnima_NPC2;

	public SkeletonAnimation sAnima_NPC3;

	public SkeletonAnimation sAnima_NPC4;

	public SkeletonAnimation sAnima_NPC5;

	public SkeletonAnimation sAnima_NPC6;

	[Header("LightCreate")]
	public Transform tsf_Drop;

	[Header("Audio")]
	public AudioSource[] ass;

	private StoryState state;

	private float bornIdleTimer;

	private float focusWaitTimer;

	private GameObject go_UIHardFinishLight;

	private void OnEnable()
	{
		EventMgr.SoundVolumeChange = (Action)Delegate.Combine(EventMgr.SoundVolumeChange, new Action(SoundVolumeChange));
	}

	private void OnDisable()
	{
		EventMgr.SoundVolumeChange = (Action)Delegate.Remove(EventMgr.SoundVolumeChange, new Action(SoundVolumeChange));
	}

	private void SoundVolumeChange()
	{
		for (int i = 0; i < ass.Length; i++)
		{
			ass[i].volume = DataMgr.settingData.GetFinalSound();
		}
	}

	private void Start()
	{
		UICampMgr.Inst.uiFinishHardStory = base.gameObject;
		UIPlayerDataMgr.Inst.HideDirect();
		PlayerMgr.Inst.HideAndDisableControl();
		CampMgr.Inst.npc1Vivian.Hide();
		CampMgr.Inst.npc2Nimue.Hide();
		CampMgr.Inst.npc3.Hide();
		CampMgr.Inst.npc4.Hide();
		CampMgr.Inst.npc6.Hide();
		MusicMgr.Inst.ForcePlayMusic("");
		if (!DataMgr.selectedWorldData.story3NPC4Rescued)
		{
			go_npc4.SetActive(value: false);
		}
		if (!DataMgr.selectedWorldData.story4NPC5Rescued)
		{
			go_npc5.SetActive(value: false);
		}
		pd_Story.Stop();
		sAnima_Player.AnimationState.SetAnimation(0, "GroundIdleDown", loop: true);
		sAnima_NPC1.AnimationState.SetAnimation(0, "Idle", loop: true);
		sAnima_NPC2.AnimationState.SetAnimation(0, "Idle", loop: true);
		sAnima_NPC3.AnimationState.SetAnimation(0, "Idle", loop: true);
		sAnima_NPC4.AnimationState.SetAnimation(0, "Idle", loop: true);
		sAnima_NPC5.AnimationState.SetAnimation(0, "Idle", loop: true);
		sAnima_NPC6.AnimationState.SetAnimation(0, "Idle", loop: true);
		if (PlayerMgr.Inst.ItemCtrller.relic_RemoteShoot != null)
		{
			PlayerMgr.Inst.ItemCtrller.relic_RemoteShoot.gameObject.SetActive(value: false);
		}
		if (PlayerMgr.Inst.ItemCtrller.relic_DruidRing != null)
		{
			PlayerMgr.Inst.ItemCtrller.relic_DruidRing.gameObject.SetActive(value: false);
		}
		SoundVolumeChange();
	}

	private void Update()
	{
		switch (state)
		{
		case StoryState.Idle:
			bornIdleTimer += Time.deltaTime;
			if (bornIdleTimer >= bornIdleTime)
			{
				bornIdleTimer = 0f;
				state = StoryState.CamFocus;
				CamController.Inst.FocusOn(focusSize, focusTime, focusPos);
				UIMgr.Inst.uiFilmBlackEdge.Show(focusTime, delegate
				{
					state = StoryState.FocusWait;
				});
			}
			break;
		case StoryState.FocusWait:
			focusWaitTimer += Time.deltaTime;
			if (focusWaitTimer >= focusWaitTime)
			{
				state = StoryState.Talk1;
				GameUISingletonMono<UIDialogueMgr>.Inst.HDShow(68, HDEvent, delegate
				{
					pd_Story.Play();
				});
			}
			break;
		default:
			Debug.LogError(state);
			break;
		case StoryState.CamFocus:
		case StoryState.Talk1:
		case StoryState.WaitPopupUI:
			break;
		}
	}

	private void HDEvent(string eventStr)
	{
		if (eventStr == "e1")
		{
			go_Player.transform.localScale = new Vector3(-1f, 1f, 1f);
			go_npc1.transform.localScale = new Vector3(-1f, 1f, 1f);
		}
		else
		{
			Debug.LogError(eventStr);
		}
	}

	public void _PlayerFaceRight()
	{
		go_Player.transform.localScale = new Vector3(1f, 1f, 1f);
	}

	public void _LightCreate()
	{
		go_UIHardFinishLight = UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/UI/UIHardFinishLight"), UIMgr.Inst.rtsf_Canvas10);
		go_UIHardFinishLight.transform.SetSiblingIndex(7);
	}

	public void _Fade()
	{
		UIMgr.Inst.uiFade.Show(delegate
		{
			CampMgr.Inst.npc1Vivian.Show();
			CampMgr.Inst.npc2Nimue.Show();
			CampMgr.Inst.npc3.Show();
			if (DataMgr.selectedWorldData.story3NPC4Rescued)
			{
				CampMgr.Inst.npc4.Show();
			}
			CampMgr.Inst.npc6.Show();
			CamController.Inst.FocusRecover(0f);
			UIMgr.Inst.uiFilmBlackEdge.Hide(0f);
			PlayerMgr.Inst.ShowAndEnableControl();
			MusicMgr.Inst.UpdateCampBGM();
			UIMgr.Inst.uiFade.Hide(focusTime, delegate
			{
				UIPlayerDataMgr.Inst.Show();
			});
			if (PlayerMgr.Inst.ItemCtrller.relic_RemoteShoot != null)
			{
				PlayerMgr.Inst.ItemCtrller.relic_RemoteShoot.gameObject.SetActive(value: true);
			}
			if (PlayerMgr.Inst.ItemCtrller.relic_DruidRing != null)
			{
				PlayerMgr.Inst.ItemCtrller.relic_DruidRing.gameObject.SetActive(value: true);
			}
			UICampMgr.Inst.uiFinishHardStory = null;
			UnityEngine.Object.Destroy(go_UIHardFinishLight);
			UnityEngine.Object.Destroy(base.gameObject);
		});
	}
}
