using System;
using PlayerLogger;
using PlayerLogger.Events;
using Spine.Unity;
using Unity.Entities;
using UnityEngine;

public class NPC5_Trapped : InteractiveObj, IRoomCtrller
{
	private enum NPCState
	{
		Wait,
		TalkIdle,
		Walk,
		Hide
	}

	[Space(50f)]
	public SkeletonAnimation sAnima_Outline;

	public SkeletonAnimation sAnima;

	public Material mat_Original;

	public Material mat_Outline;

	public BoxCollider bc;

	public float focusSize;

	public float focusTime;

	public Vector3 walkToPointOffset;

	public float talkIdleTime;

	public float walkSpeed;

	[Header("Cage")]
	public Animator anima_Cage;

	public Transform tsf_CageUp;

	public Transform tsf_CageDown;

	public Entity interactiveEntity;

	private NPCState state;

	private RoomController belongRoom;

	private Vector3 walkToPoint;

	private float talkIdleTimer;

	private void Start()
	{
		if (belongRoom.roomCfg.isFlipped)
		{
			walkToPointOffset.x = 0f - walkToPointOffset.x;
			base.transform.localScale = Vector3.one;
		}
		else
		{
			base.transform.localScale = new Vector3(-1f, 1f, 1f);
		}
		walkToPoint = base.transform.position + walkToPointOffset;
		anima_Cage.transform.SetParent(base.transform.parent);
		tsf_CageUp.position = Tool2D.GetLayerPoint(base.transform);
		tsf_CageDown.position = Tool2D.GetLayerPoint(base.transform, LayerCorrectType.GroundEffectLow);
		interactiveEntity = RegisterDotsInteractiveObj(bc, InteractiveObjType.NPC4_Trapped);
		if (GameMgr.IsChAge14_Static)
		{
			sAnima.initialSkinName += "_HX";
			sAnima.Initialize(overwrite: true);
		}
	}

	private void Update()
	{
		switch (state)
		{
		case NPCState.TalkIdle:
			talkIdleTimer += Time.deltaTime;
			if (talkIdleTimer >= talkIdleTime)
			{
				state = NPCState.Walk;
				sAnima.AnimationState.SetAnimation(0, "Walk", loop: true);
			}
			break;
		case NPCState.Walk:
			base.transform.position = Vector3.MoveTowards(base.transform.position, walkToPoint, walkSpeed * Time.deltaTime);
			if (base.transform.position == walkToPoint)
			{
				state = NPCState.Hide;
				ObjPoolMgr.Inst.GetGO("Prefabs/Item/Curse_InjuredRandomPoint", base.transform.position, 2f);
				SEMgr.Inst.curseInjuredRandomPoint.PlaySE();
				base.gameObject.SetActive(value: false);
			}
			break;
		default:
			Debug.LogError(state);
			break;
		case NPCState.Wait:
		case NPCState.Hide:
			break;
		}
	}

	private void PlayerEnterRoom()
	{
		DataMgr.selectedWorldData.story4PlayerRoomEnter = true;
		DataMgr.SaveSelectedWorldData();
	}

	public override void Select()
	{
		sAnima_Outline.CustomMaterialOverride.Add(mat_Original, mat_Outline);
	}

	public override void Unselect()
	{
		sAnima_Outline.CustomMaterialOverride.Remove(mat_Original);
	}

	public override void Interact()
	{
		if (!DataMgr.selectedWorldData.story4NPC5Rescued)
		{
			UnlockNPCLogger unlockNPCLogger = new UnlockNPCLogger();
			unlockNPCLogger.npc_id = 5;
			unlockNPCLogger.Report();
			DataMgr.selectedWorldData.story4NPC5Rescued = true;
		}
		DataMgr.SaveSelectedWorldData();
		base.tag = "Untagged";
		PlayerMgr.Inst.PlayerCtrller.StopMotion();
		UIPlayerDataMgr.Inst.Hide();
		bc.enabled = false;
		anima_Cage.SetTrigger("Open");
		SEMgr.Inst.npcCage_Open.PlaySE();
		UnityEngine.Object.Destroy(sAnima_Outline.gameObject);
		SetDotsObjLayer(interactiveEntity, isOpen: false);
		CamController.Inst.FocusOn(focusSize, focusTime, base.transform.position);
		UIMgr.Inst.uiFilmBlackEdge.Show(focusTime, delegate
		{
			GameUISingletonMono<UIDialogueMgr>.Inst.HDShow(21, (Action)delegate
			{
				CamController.Inst.FocusRecover(focusTime);
				int specialRoomSpell = OutputMgr.GetSpecialRoomSpell();
				ItemInfo itemInfo = default(ItemInfo);
				itemInfo.type = ItemType.Spell;
				itemInfo.id = specialRoomSpell;
				ItemInfo info = itemInfo;
				QuickCreateSystem.Inst.CreateItem(LevelMgr.Inst.CurrentRoomMapPos, info, PlayerMgr.Inst.PlayerPoint + new Vector3(0f, -0.5f, 0f));
				LevelMgr.Inst.RoomFinishLogger?.AddCurrentSideRoomReward(PlayerLogger.Item.CreateSpell(specialRoomSpell));
				UIMgr.Inst.uiFilmBlackEdge.Hide(focusTime, delegate
				{
					PlayerMgr.Inst.PlayerCtrller.StartMotion();
					UIPlayerDataMgr.Inst.Show();
					state = NPCState.TalkIdle;
				});
			});
		});
	}

	public void SetRoomCtrlller(RoomController roomCtrller)
	{
		belongRoom = roomCtrller;
		belongRoom.RoomEnterRegister(PlayerEnterRoom);
	}
}
