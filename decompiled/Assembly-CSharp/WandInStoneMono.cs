using System;
using Unity.Entities;
using UnityEngine;

public class WandInStoneMono : LayerCorrect
{
	private enum SelfState
	{
		Wait,
		PlayerToPoint,
		Picking
	}

	[Space(50f)]
	public GameObject go_Highlight;

	public SpriteRenderer sr;

	public AudioSource as1;

	public AudioSource as2;

	private SelfState state;

	public override void OnEnable()
	{
		base.OnEnable();
		EventMgr.SoundVolumeChange = (Action)Delegate.Combine(EventMgr.SoundVolumeChange, new Action(SoundVolumeChange));
		SoundVolumeChange();
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

	private void Update()
	{
		switch (state)
		{
		case SelfState.PlayerToPoint:
		{
			Vector3 playerPoint = Vector3.MoveTowards(PlayerMgr.Inst.PlayerT.position, GuideMgr.Inst.playerWalkPoint, PlayerMgr.Inst.PlayerPpt.MoveSpeed * Time.deltaTime);
			PlayerMgr.Inst.SetPlayerPoint(playerPoint);
			if (PlayerMgr.Inst.PlayerT.position == GuideMgr.Inst.playerWalkPoint)
			{
				GuideMgr.Inst.pd_Story4.Play();
				state = SelfState.Picking;
				PlayerMgr.Inst.HideAndDisableControl();
				PlayerMgr.Inst.PlayerCtrller.SetFlip(isFlip: false);
				PlayerMgr.Inst.SetPlayerPoint(GuideMgr.Inst.playerShowPoint);
			}
			break;
		}
		default:
			Debug.LogError(state);
			break;
		case SelfState.Wait:
		case SelfState.Picking:
			break;
		}
	}

	public void Select()
	{
		go_Highlight.SetActive(value: true);
	}

	public void Unselect()
	{
		go_Highlight.SetActive(value: false);
	}

	public void Interact()
	{
		if (ScriptableObjMgr.Inst.testCtrller.GuideSkipPickWand)
		{
			PlayerMgr.Inst.WandPickUp(WandConfig.GetConfig(1));
			EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
			using EntityQuery entityQuery = entityManager.CreateEntityQuery(typeof(DoorCampGuide));
			Entity singletonEntity = entityQuery.GetSingletonEntity();
			DoorCampGuide singleton = entityQuery.GetSingleton<DoorCampGuide>();
			singleton.onHideMask = true;
			entityManager.SetComponentData(singletonEntity, singleton);
			return;
		}
		state = SelfState.PlayerToPoint;
		PlayerMgr.Inst.PlayerCtrller.StopMotion();
		PlayerMgr.Inst.PlayerCtrller.StopFace(PlayerMgr.Inst.PlayerDir.x < 0f);
		PlayerMgr.Inst.PlayerCtrller.SetEmoji(PlayerEmojiType.Other);
		PlayerMgr.Inst.PlayerCtrller.SetBodyAnima(PlayerBodyAnima.GroundWalkDown);
		PlayerMgr.Inst.PlayerCtrller.SetFlip((PlayerMgr.Inst.PlayerPoint - base.transform.position).x < 0f);
		CamController.Inst.FocusOn(GuideMgr.Inst.pickWandFocusSize, GuideMgr.Inst.pickWandFocusTime, GuideMgr.Inst.playerShowPoint);
		UIMgr.Inst.uiFilmBlackEdge.Show(GuideMgr.Inst.pickWandFocusTime);
	}
}
