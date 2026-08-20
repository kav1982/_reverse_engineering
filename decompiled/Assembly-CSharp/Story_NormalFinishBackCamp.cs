using System;
using Spine.Unity;
using UnityEngine;
using UnityEngine.Playables;

public class Story_NormalFinishBackCamp : MonoBehaviour
{
	private enum StoryState
	{
		Idle,
		CamFocus,
		FocusWait,
		Talk1,
		NPC7RunToDoor,
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

	public GameObject go_npc7;

	public PlayableDirector pd_Story;

	public SkeletonAnimation sAnima_Player;

	public SkeletonAnimation sAnima_NPC1;

	public SkeletonAnimation sAnima_NPC2;

	public SkeletonAnimation sAnima_NPC3;

	public SkeletonAnimation sAnima_NPC4;

	public SkeletonAnimation sAnima_NPC5;

	public SkeletonAnimation sAnima_NPC6;

	public StoryPlayerChangeMaterial spcm;

	[Header("npc7run")]
	public GameObject go_TransferEF;

	public Vector3 npc7RunToDoorPoint;

	public Vector3 npc7RunToDoorPointMobile;

	public float npc7RunToDoorTime;

	public float camMoveToDoorTime;

	public float camBackTime;

	[Header("Audio")]
	public AudioSource as1;

	private StoryState state;

	private float bornIdleTimer;

	private float focusWaitTimer;

	private float npc7RunToDoorSpeed;

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
		if (GameMgr.IsMobile_Static)
		{
			go_TransferEF.transform.position = npc7RunToDoorPointMobile;
		}
		else
		{
			go_TransferEF.transform.position = npc7RunToDoorPoint;
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
				GameUISingletonMono<UIDialogueMgr>.Inst.HDShow(52, HDEvent, delegate
				{
					pd_Story.Play();
				});
			}
			break;
		case StoryState.NPC7RunToDoor:
			if (GameMgr.IsMobile_Static)
			{
				go_npc7.transform.position = Vector3.MoveTowards(go_npc7.transform.position, npc7RunToDoorPointMobile, npc7RunToDoorSpeed * Time.deltaTime);
				if (go_npc7.transform.position == npc7RunToDoorPointMobile)
				{
					state = StoryState.WaitPopupUI;
				}
			}
			else
			{
				go_npc7.transform.position = Vector3.MoveTowards(go_npc7.transform.position, npc7RunToDoorPoint, npc7RunToDoorSpeed * Time.deltaTime);
				if (go_npc7.transform.position == npc7RunToDoorPoint)
				{
					state = StoryState.WaitPopupUI;
				}
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
			spcm.RelicHuangFaceLeft();
		}
		else
		{
			Debug.LogError(eventStr);
		}
	}

	public void _PlayerFaceRight()
	{
		go_Player.transform.localScale = new Vector3(1f, 1f, 1f);
		spcm.RelicHuangFaceRight();
	}

	public void _NPC1FaceRight()
	{
		go_npc1.transform.localScale = new Vector3(1f, 1f, 1f);
	}

	public void _PlayerAmaze()
	{
		sAnima_Player.AnimationState.SetAnimation(1, "Emoji/Amaze", loop: false);
		spcm.RelicHuangAmaze();
	}

	public void _NPC7Cough()
	{
		GameUISingletonMono<UIDialogueMgr>.Inst.SDShow(3901, go_npc7.transform);
	}

	public void _NPC7StartToRun()
	{
		state = StoryState.NPC7RunToDoor;
		if (GameMgr.IsMobile_Static)
		{
			npc7RunToDoorSpeed = Vector3.Distance(go_npc7.transform.position, npc7RunToDoorPointMobile) / npc7RunToDoorTime;
		}
		else
		{
			npc7RunToDoorSpeed = Vector3.Distance(go_npc7.transform.position, npc7RunToDoorPoint) / npc7RunToDoorTime;
		}
	}

	public void _CamMoveToDoor()
	{
		if (GameMgr.IsMobile_Static)
		{
			CamController.Inst.FocusOn(focusSize, camMoveToDoorTime, npc7RunToDoorPointMobile);
		}
		else
		{
			CamController.Inst.FocusOn(focusSize, camMoveToDoorTime, npc7RunToDoorPoint);
		}
		if (GameMgr.IsMobile_Static)
		{
			CamController.Inst.FocusOn(focusSize, camMoveToDoorTime, npc7RunToDoorPointMobile);
		}
		else
		{
			CamController.Inst.FocusOn(focusSize, camMoveToDoorTime, npc7RunToDoorPoint);
		}
	}

	public void _CamBack()
	{
		CamController.Inst.FocusOn(focusSize, camBackTime, focusPos);
	}

	public void _PlayerIdle()
	{
		sAnima_Player.AnimationState.SetAnimation(1, "Emoji/Normal", loop: false);
		spcm.RelicHuangIdle();
	}

	public void _HD2()
	{
		sAnima_Player.AnimationState.SetAnimation(0, "GroundIdleDown", loop: true);
		sAnima_NPC1.AnimationState.SetAnimation(0, "Idle", loop: true);
		sAnima_NPC2.AnimationState.SetAnimation(0, "Idle", loop: true);
		sAnima_NPC3.AnimationState.SetAnimation(0, "Idle", loop: true);
		sAnima_NPC4.AnimationState.SetAnimation(0, "Idle", loop: true);
		sAnima_NPC5.AnimationState.SetAnimation(0, "Idle", loop: true);
		sAnima_NPC6.AnimationState.SetAnimation(0, "Idle", loop: true);
		GameUISingletonMono<UIDialogueMgr>.Inst.HDShow(53, (Action)delegate
		{
			UIMgr.Inst.uiFade.Show(focusTime, delegate
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
				UnityEngine.Object.Destroy(base.gameObject);
			});
		});
	}
}
