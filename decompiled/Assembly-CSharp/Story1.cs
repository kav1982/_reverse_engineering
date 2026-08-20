using System;
using Spine.Unity;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Playables;

public class Story1 : MonoBehaviour
{
	private enum SotryState
	{
		Idle,
		Story
	}

	public PlayableDirector pd_Story;

	public float camSizeStart;

	public float focusSize;

	public float focusTime;

	public Transform tsf_PlayerRoot;

	public Transform tsf_VivianRoot;

	public Transform tsf_NimueRoot;

	public SkeletonAnimation sAnima_Player;

	public SkeletonAnimation sAnima_Vivian;

	public SkeletonAnimation sAnima_Nimue;

	public float uiFadeTime;

	public Vector3 hd3FocusPoint;

	public ShockParam lieUpShock;

	public float uiFadeTimeFinish;

	[Header("Audio")]
	public AudioSource as_Timeline;

	public float3 bonfireTempPosition;

	private void OnEnable()
	{
		EventMgr.SoundVolumeChange = (Action)Delegate.Combine(EventMgr.SoundVolumeChange, new Action(SoundVolumeChange));
		SoundVolumeChange();
		EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		entityQueryBuilder = entityQueryBuilder.WithAll<LocalTransform, CampBonfireTag_Dots>();
		EntityQuery entityQuery = entityQueryBuilder.Build(entityManager);
		using (NativeArray<Entity> nativeArray = entityQuery.ToEntityArray(Allocator.Temp))
		{
			foreach (Entity item in nativeArray)
			{
				if (entityManager.HasComponent<LocalTransform>(item))
				{
					LocalTransform componentData = entityManager.GetComponentData<LocalTransform>(item);
					bonfireTempPosition = componentData.Position;
					entityManager.SetComponentData(item, componentData.WithPosition(componentData.Position + new float3(0f, 2f, 0f)));
				}
			}
		}
		entityQuery.Dispose();
	}

	private void OnDisable()
	{
		EventMgr.SoundVolumeChange = (Action)Delegate.Remove(EventMgr.SoundVolumeChange, new Action(SoundVolumeChange));
		EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		entityQueryBuilder = entityQueryBuilder.WithAll<LocalTransform, CampBonfireTag_Dots>();
		EntityQuery entityQuery = entityQueryBuilder.Build(entityManager);
		using (NativeArray<Entity> nativeArray = entityQuery.ToEntityArray(Allocator.Temp))
		{
			foreach (Entity item in nativeArray)
			{
				if (entityManager.HasComponent<LocalTransform>(item))
				{
					entityManager.SetComponentData(item, entityManager.GetComponentData<LocalTransform>(item).WithPosition(bonfireTempPosition));
				}
			}
		}
		entityQuery.Dispose();
	}

	private void SoundVolumeChange()
	{
		as_Timeline.volume = DataMgr.settingData.GetFinalSound();
	}

	private void Start()
	{
		UIPlayerDataMgr.Inst.HideDirect();
		PlayerMgr.Inst.HideAndDisableControl();
		CampMgr.Inst.npc1Vivian.Hide();
		CampMgr.Inst.npc2Nimue.Hide();
		CamController.Inst.FocusOn(camSizeStart, 0f, CampMgr.Inst.playerBornPoint);
		UIMgr.Inst.uiFilmBlackEdge.Show(0f);
		MusicMgr.Inst.ForcePlayMusic("");
	}

	public void _CamFocus()
	{
		CamController.Inst.FocusOn(focusSize, focusTime, CampMgr.Inst.playerBornPoint);
	}

	public void _HD1()
	{
		GameUISingletonMono<UIDialogueMgr>.Inst.HDShow(5, HD1Finish);
		sAnima_Vivian.AnimationState.SetAnimation(0, "Idle", loop: true);
		sAnima_Nimue.AnimationState.SetAnimation(0, "Idle", loop: true);
		pd_Story.Pause();
	}

	private void HD1Finish()
	{
		pd_Story.Play();
	}

	public void _HD2()
	{
		GameUISingletonMono<UIDialogueMgr>.Inst.HDShow(6, HD2Finish);
		sAnima_Nimue.AnimationState.SetAnimation(0, "Idle", loop: true);
		pd_Story.Pause();
	}

	private void HD2Finish()
	{
		UIMgr.Inst.uiFade.Show(uiFadeTime, delegate
		{
			pd_Story.Play();
		});
	}

	public void _FadeHide()
	{
		UIMgr.Inst.uiFade.Hide(uiFadeTime);
		CampMgr.Inst.CampStage0SetActive(active: true);
		EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
		LocalTransform componentData = entityManager.GetComponentData<LocalTransform>(CampMgr.Inst.CurrentCampSkin.ett_Decoration_CampStage0);
		componentData.Position += (float3)CampMgr.Inst.campFirePositionMoveDown;
		entityManager.SetComponentData(CampMgr.Inst.CurrentCampSkin.ett_Decoration_CampStage0, componentData);
		CampMgr.Inst.SetEttEnable(CampMgr.Inst.CurrentCampSkin.ett_CampMirror, enable: true);
		CamController.Inst.FocusOn(focusSize, 0f, hd3FocusPoint);
	}

	public void _HD3()
	{
		GameUISingletonMono<UIDialogueMgr>.Inst.HDShow(7, HD3Finish);
		sAnima_Vivian.AnimationState.SetAnimation(0, "Idle", loop: true);
		sAnima_Nimue.AnimationState.SetAnimation(0, "Idle", loop: true);
		pd_Story.Pause();
	}

	private void HD3Finish()
	{
		pd_Story.Play();
	}

	public void _PlayerScreaming()
	{
		GameUISingletonMono<UIDialogueMgr>.Inst.SDShow(900301, tsf_PlayerRoot);
		CamController.Inst.SetShock(lieUpShock);
	}

	public void _NPCScreaming()
	{
		GameUISingletonMono<UIDialogueMgr>.Inst.SDShow(900301, tsf_VivianRoot);
		GameUISingletonMono<UIDialogueMgr>.Inst.SDShow(900301, tsf_NimueRoot);
	}

	public void _HD4()
	{
		GameUISingletonMono<UIDialogueMgr>.Inst.HDShow(8, HD4Finish);
		pd_Story.Pause();
	}

	private void HD4Finish()
	{
		sAnima_Player.AnimationState.SetAnimation(0, "Emoji/Normal", loop: false);
		sAnima_Vivian.AnimationState.SetAnimation(0, "Idle", loop: true);
		sAnima_Nimue.AnimationState.SetAnimation(0, "Idle", loop: true);
		UIMgr.Inst.uiFade.Show(uiFadeTimeFinish, delegate
		{
			PlayerMgr.Inst.SetPlayerPoint(hd3FocusPoint);
			PlayerMgr.Inst.ShowAndEnableControl();
			CampMgr.Inst.npc1Vivian.Show();
			CampMgr.Inst.npc2Nimue.Show();
			CamController.Inst.FocusRecover(0f);
			UIMgr.Inst.uiFilmBlackEdge.Hide(0f);
			MusicMgr.Inst.UpdateCampBGM();
			UnityEngine.Object.Destroy(base.gameObject);
			UIMgr.Inst.uiFade.Hide(uiFadeTimeFinish, delegate
			{
				UIPlayerDataMgr.Inst.Show();
				UIPlaceNameMgr.Inst.Show(PlaceNameType.Camp);
			});
		});
	}
}
