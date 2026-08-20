using System;
using Spine.Unity;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class NPC7AppearanceMgr : MonoBehaviour
{
	private enum MgrState
	{
		BornIdle,
		Focus,
		WaitEat,
		WaitFinish
	}

	public GameObject go_StartToDestroy;

	public PlayableDirector pd_Timeline;

	public Transform tsf_CamInitialPoint;

	public Transform tsf_NPC6;

	public Transform tsf_NPC7;

	public Transform tsf_CamFocusPoint;

	public SkeletonAnimation sAnima_NPC6;

	public SkeletonAnimation sAnima_NPC7;

	public float bornIdleTime;

	public float camFocusSize;

	public float camFocusTime;

	public float talk1FinishFadeTime;

	public AudioSource as1;

	public AudioSource as2;

	[Header("Talk2")]
	public float waitEatTime;

	public Vector3 talk2NPC6Point;

	public Vector3 talk2NPC7Point;

	private MgrState state;

	private float bornIdleTimer;

	private float waitEatTimer;

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
		as2.volume = DataMgr.settingData.GetFinalSound();
	}

	private void Start()
	{
		UnityEngine.Object.Destroy(go_StartToDestroy);
		CamController.Inst.SetFollow(tsf_CamInitialPoint);
		CamController.Inst.CorrectCamera();
		UIMgr.Inst.uiFade.Hide(talk1FinishFadeTime);
		MusicMgr.Inst.ForcePlayMusic("");
		MusicMgr.Inst.ForcePlayAmbient(ScriptableObjMgr.Inst.themeAmbient.strs[14]);
		pd_Timeline.Stop();
		SoundVolumeChange();
		World.DefaultGameObjectInjectionWorld.EntityManager.DestroyEntity(PlayerMgr.Inst.PlayerEtt);
		DataMgr.selectedWorldData.storyHardFinishNPC7Appearance = true;
		DataMgr.selectedWorldData.npc6ImportantPlot.SetNewState(62);
	}

	private void Update()
	{
		switch (state)
		{
		case MgrState.BornIdle:
			bornIdleTimer += Time.deltaTime;
			if (bornIdleTimer >= 0.5f)
			{
				bornIdleTimer = 0f;
				state = MgrState.Focus;
				CamController.Inst.FocusOn(camFocusSize, camFocusTime, tsf_CamFocusPoint.position);
				UIMgr.Inst.uiFilmBlackEdge.Show(camFocusTime, delegate
				{
					pd_Timeline.Play();
				});
			}
			break;
		case MgrState.WaitEat:
			waitEatTimer += Time.deltaTime;
			if (!(waitEatTimer >= waitEatTime))
			{
				break;
			}
			waitEatTimer = 0f;
			state = MgrState.WaitFinish;
			GameUISingletonMono<UIDialogueMgr>.Inst.HDShow(61, (Action)delegate
			{
				UIMgr.Inst.uiFade.Show(talk1FinishFadeTime, delegate
				{
					CamController.Inst.FocusRecover(0f);
					UIMgr.Inst.uiFilmBlackEdge.Hide(0f);
					SceneManager.LoadScene("Battle");
				});
			});
			break;
		default:
			Debug.LogError(state);
			break;
		case MgrState.Focus:
		case MgrState.WaitFinish:
			break;
		}
	}

	public void _Amaze()
	{
		GameUISingletonMono<UIDialogueMgr>.Inst.SDShow(900501, tsf_NPC6);
	}

	public void _Talk()
	{
		pd_Timeline.Pause();
		GameUISingletonMono<UIDialogueMgr>.Inst.HDShow(60, (Action)delegate
		{
			UIMgr.Inst.uiFade.Show(talk1FinishFadeTime, delegate
			{
				tsf_NPC6.position = talk2NPC6Point;
				tsf_NPC7.position = talk2NPC7Point;
				sAnima_NPC6.AnimationState.SetAnimation(0, "EatFruitSit", loop: true);
				sAnima_NPC7.AnimationState.SetAnimation(0, "EatFruitSit", loop: true);
				UIMgr.Inst.uiFade.Hide(talk1FinishFadeTime, delegate
				{
					state = MgrState.WaitEat;
				});
			});
		});
	}
}
