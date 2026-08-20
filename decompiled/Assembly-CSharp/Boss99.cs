using System;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

public class Boss99 : UnitBase
{
	private enum UnitState
	{
		Sit,
		Transition,
		TransitionFinishIdle,
		RunReady,
		RunToPlayer,
		Laugh
	}

	[Space(50f)]
	public Boss99Interaction go_Interaction;

	public Transform tsf_ShadowScale;

	public Shadow shadow;

	public float dialogueOffsetY;

	[Header("Run")]
	public float transitionFinishTime;

	public float runReadyTime;

	public float moveSpeedRatioAcceleration;

	private UnitState state;

	private float originalColliderRadius;

	private float originalColliderHeight;

	private float transitionFinishTimer;

	private float runReadyTimer;

	private float currentMoveSpeedRatio = 1f;

	private float runTimer;

	private void OnEnable()
	{
		EventMgr.PlayerDead = (Action)Delegate.Combine(EventMgr.PlayerDead, new Action(PlayerDead));
	}

	private void OnDisable()
	{
		EventMgr.PlayerDead = (Action)Delegate.Remove(EventMgr.PlayerDead, new Action(PlayerDead));
	}

	private void PlayerDead()
	{
		if (state != 0)
		{
			GameUISingletonMono<UIDialogueMgr>.Inst.SDShow(900407, base.transform, dialogueOffsetY);
			state = UnitState.Laugh;
			base.Anima.Play("Laugh");
		}
	}

	public override void SingleInitialCallback()
	{
		originalColliderRadius = myPpt.CC_Self.radius;
		originalColliderHeight = myPpt.CC_Self.height;
	}

	public override void EveryInitialCallback()
	{
		UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
		componentData.unitCfg.maxHP = UnitConfig.map[myPpt.unitCfg.id].maxHP;
		componentData.unitCfg.currentHP = componentData.unitCfg.maxHP;
		componentData.CanBeTarget = false;
		componentData.CanTouch = false;
		componentData.InvincibleRegister();
		SetComponentData(componentData);
		myPpt.CC_Self.enabled = false;
		SetDotsCCEnable(isOpen: false);
		GameUISingletonMono<UIBossHP>.HideIfInited();
		MusicMgr.Inst.UpdateThemeMusic();
		myPpt.tsf_Layer.gameObject.SetActive(value: false);
		go_Interaction.gameObject.SetActive(value: true);
		go_Interaction.transform.SetParent(LevelMgr.Inst.CurrentRoomT);
		myPpt.RegetSR();
		if (LevelMgr.Inst.CurrentRoomMapPos.x > 0)
		{
			SetFlip(-1f);
			go_Interaction.go_Outline.transform.localScale = new Vector3(-1f, 1f, 1f);
		}
		SetFlip(-1f);
		go_Interaction.go_Outline.transform.localScale = new Vector3(-1f, 1f, 1f);
		LevelMgr.Inst.CurrentRoomCtrller.MaskFinish();
	}

	public override void Frame2InitialCallback()
	{
		LevelMgr.Inst.CurrentRoomCtrller.monsterEttList.Remove(myPpt.myEntity);
		LevelMgr.Inst.CurrentRoomCtrller.targetableEttList.Remove(myPpt.myEntity);
		LevelMgr.Inst.CurrentRoomCtrller.AllAccessOpenDirect();
	}

	public unsafe override void Update()
	{
		base.Update();
		if (base.IsLocked)
		{
			return;
		}
		switch (state)
		{
		case UnitState.Transition:
		{
			shadow.ShadowGO.transform.localScale = tsf_ShadowScale.localScale;
			myPpt.CC_Self.radius = originalColliderRadius * tsf_ShadowScale.localScale.x;
			myPpt.CC_Self.height = originalColliderHeight * tsf_ShadowScale.localScale.x;
			PhysicsCollider componentData = GetComponentData<PhysicsCollider>();
			Unity.Physics.CapsuleCollider* colliderPtr = (Unity.Physics.CapsuleCollider*)componentData.ColliderPtr;
			CapsuleGeometry geometry = colliderPtr->Geometry;
			geometry.Radius = base.CC_Self.radius;
			geometry.Vertex0 = base.CC_Self.center + new Vector3(0f, 0f, base.CC_Self.height / 2f);
			geometry.Vertex1 = base.CC_Self.center - new Vector3(0f, 0f, base.CC_Self.height / 2f);
			colliderPtr->Geometry = geometry;
			SetComponentData(componentData);
			break;
		}
		case UnitState.TransitionFinishIdle:
			SetMove(Vector3.zero);
			transitionFinishTimer += Time.deltaTime;
			if (transitionFinishTimer >= transitionFinishTime)
			{
				state = UnitState.RunReady;
				base.Anima.Play("RunReady");
				GameUISingletonMono<UIDialogueMgr>.Inst.MDShow(900403, base.transform, dialogueOffsetY);
			}
			break;
		case UnitState.RunReady:
			SetMove(Vector3.zero);
			SetFlip(ToPointDir(PlayerMgr.Inst.PlayerPointIgnoreZ).x);
			runReadyTimer += Time.deltaTime;
			if (runReadyTimer >= runReadyTime)
			{
				state = UnitState.RunToPlayer;
				base.Anima.Play("Run");
			}
			break;
		case UnitState.RunToPlayer:
			currentMoveSpeedRatio += moveSpeedRatioAcceleration * Time.deltaTime;
			SetFlip(ToPointDir(PlayerMgr.Inst.PlayerPointIgnoreZ).x);
			base.transform.position = Vector3.MoveTowards(base.transform.position, PlayerMgr.Inst.PlayerPointIgnoreZ, base.MoveSpeed * currentMoveSpeedRatio * Time.deltaTime);
			SyncDotsPosition();
			base.Anima.speed = currentMoveSpeedRatio;
			break;
		case UnitState.Laugh:
			SetMove(Vector3.zero);
			break;
		default:
			Debug.LogError(state);
			break;
		case UnitState.Sit:
			break;
		}
	}

	public override void AnimaAction(string animaName)
	{
		switch (animaName)
		{
		case "TransitionFinish":
		{
			UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
			componentData.unitCfg.unitType = UnitType.Boss;
			LevelMgr.Inst.CurrentRoomCtrller.UnitRegister(myPpt.myEntity);
			LevelMgr.Inst.CurrentRoomCtrller.MaskNoFinish();
			shadow.ShadowGO.transform.localScale = tsf_ShadowScale.localScale;
			componentData.CanBeTarget = true;
			componentData.CanTouch = true;
			componentData.InvincibleUnregister();
			SetComponentData(componentData);
			SetDotsCCEnable(isOpen: true);
			myPpt.CC_Self.enabled = true;
			GameUISingletonMono<UIBossHP>.ShowInit(myPpt.myEntity);
			GameUISingletonMono<UIBossShow>.ShowInit(myPpt.myEntity);
			state = UnitState.TransitionFinishIdle;
			break;
		}
		default:
			Debug.LogError(animaName);
			break;
		case "FingerSnap":
			break;
		case "StandFinish":
			break;
		}
	}

	protected override void BossDeadStay()
	{
		GameUISingletonMono<UIDialogueMgr>.Inst.MDShow(900408, base.transform, dialogueOffsetY);
		base.Anima.Play("Lose");
		base.BossDeadStay();
	}

	public override void AfterDead(ref TakeDamageInfo_Dots info)
	{
		base.AfterDead(ref info);
		SteamAchievementMgr.UnlockAndUpload(SteamAchievementType.ProducerKiller);
		QuickCreateSystem.Inst.CreateMixedEtt("BackCampPortal", Tool2D.GetNavMeshPointIngoreZ(LevelMgr.Inst.CurrentRoomCtrller.CenterPoint));
	}

	public void Transition()
	{
		if (LevelMgr.Inst.CurrentRoomCtrller.roomCfg.themeType == RoomThemeType.theme29_Chapter5Boss_Dave)
		{
			EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
			using EntityQuery entityQuery = entityManager.CreateEntityQuery(typeof(SpecialObj45));
			SpecialObj45 singleton = entityQuery.GetSingleton<SpecialObj45>();
			LocalTransform componentData = entityManager.GetComponentData<LocalTransform>(singleton.ett_WallRight);
			componentData.Scale = 1f;
			componentData.Position -= new float3(0f, 1000f, 1000f);
			using EntityQuery entityQuery2 = entityManager.CreateEntityQuery(typeof(SpecialObj45BloodRoom));
			SpecialObj45BloodRoom singleton2 = entityQuery2.GetSingleton<SpecialObj45BloodRoom>();
			LocalTransform componentData2 = entityManager.GetComponentData<LocalTransform>(singleton2.ett_Wall);
			entityManager.SetComponentData(singleton.ett_WallRight, componentData);
			componentData2.Scale = 1f;
			componentData2.Position -= new float3(0f, 1000f, 0f);
			entityManager.SetComponentData(singleton2.ett_Wall, componentData2);
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Theme29CloseSideRoom", (Vector3)entityManager.GetComponentData<LocalTransform>(LevelMgr.Inst.CurrentRoomCtrller.accessEttList[0]).Position + Vector3.right, 2f);
		}
		state = UnitState.Transition;
		LevelMgr.Inst.CurrentRewardType = LevelRewardType.Store;
		LevelMgr.Inst.CurrentRoomCtrller.AllAccessClose();
		LevelMgr.Inst.CurrentRoomCtrller.accessEttList.Clear();
		go_Interaction.gameObject.SetActive(value: false);
		myPpt.tsf_Layer.gameObject.SetActive(value: true);
		base.Anima.Play("Transition");
	}
}
