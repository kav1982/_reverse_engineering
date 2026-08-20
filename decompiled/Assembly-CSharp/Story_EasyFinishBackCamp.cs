using System;
using Spine.Unity;
using UnityEngine;
using UnityEngine.Playables;

public class Story_EasyFinishBackCamp : MonoBehaviour
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

	public StoryPlayerChangeMaterial spcm;

	[Header("Audio")]
	public AudioSource as1;

	private StoryState state;

	private float bornIdleTimer;

	private float focusWaitTimer;

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
		as1.volume = DataMgr.settingData.GetFinalSound();
	}

	private void Start()
	{
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
		sAnima_NPC1.AnimationState.SetAnimation(0, "Idle", loop: true);
		sAnima_NPC2.AnimationState.SetAnimation(0, "Idle", loop: true);
		sAnima_NPC3.AnimationState.SetAnimation(0, "Idle", loop: true);
		sAnima_NPC4.AnimationState.SetAnimation(0, "Idle", loop: true);
		sAnima_NPC5.AnimationState.SetAnimation(0, "Idle", loop: true);
		SoundVolumeChange();
		if (PlayerMgr.Inst.ItemCtrller.relic_RemoteShoot != null)
		{
			PlayerMgr.Inst.ItemCtrller.relic_RemoteShoot.gameObject.SetActive(value: false);
		}
		if (PlayerMgr.Inst.ItemCtrller.relic_DruidRing != null)
		{
			PlayerMgr.Inst.ItemCtrller.relic_DruidRing.gameObject.SetActive(value: false);
		}
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
				GameUISingletonMono<UIDialogueMgr>.Inst.HDShow(37, (Action)delegate
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

	public void _SetPlayerAnimaSlow()
	{
		sAnima_Player.AnimationState.TimeScale = 0.5f;
	}

	public void _SetPlayerAnimaNormal()
	{
		sAnima_Player.AnimationState.TimeScale = 1f;
	}

	public void _NPC6Appear()
	{
		sAnima_Player.AnimationState.SetAnimation(1, "Emoji/Amaze", loop: false);
		spcm.RelicHuangAmaze();
	}

	public void _HD2()
	{
		pd_Story.Pause();
		sAnima_NPC6.AnimationState.SetAnimation(0, "Lie", loop: true);
		GameUISingletonMono<UIDialogueMgr>.Inst.HDShow(38, (Action)delegate
		{
			pd_Story.Play();
		});
	}

	public void _NPC6Cough()
	{
		GameUISingletonMono<UIDialogueMgr>.Inst.SDShow(3901, sAnima_NPC6.transform);
	}

	public void _HD3()
	{
		pd_Story.Pause();
		GameUISingletonMono<UIDialogueMgr>.Inst.HDShow(39, (Action)delegate
		{
			UIMgr.Inst.uiFade.Show(focusTime, delegate
			{
				UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Storys/Story_EasyFinishBackCamp2"));
				CampMgr.Inst.npc2Nimue.Show();
				CampMgr.Inst.npc3.Show();
				if (DataMgr.selectedWorldData.story3NPC4Rescued)
				{
					CampMgr.Inst.npc4.Show();
				}
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
				UnityEngine.Object.Destroy(base.gameObject);
			});
		});
	}
}
