using System;
using PlayerLogger;
using Unity.Entities;
using UnityEngine;

public class SpecialObj222_BallCollision : MonoBehaviour
{
	public SpecialObj222_GameGrabDoll game222;

	public Rigidbody rigid;

	public AudioSource BounceSound;

	public float SoundPlayIntervel;

	private float _SoundIntervel;

	private float addForceIntervalTimer;

	private void Start()
	{
		EventMgr.SoundVolumeChange = (Action)Delegate.Combine(EventMgr.SoundVolumeChange, new Action(SoundVolumeChange));
		SoundVolumeChange();
		BounceSound.Play();
	}

	private void OnDestroy()
	{
		EventMgr.SoundVolumeChange = (Action)Delegate.Remove(EventMgr.SoundVolumeChange, new Action(SoundVolumeChange));
	}

	private void SoundVolumeChange()
	{
		BounceSound.volume = DataMgr.settingData.GetFinalSound();
	}

	private void Update()
	{
		_SoundIntervel -= Time.deltaTime;
		if (_SoundIntervel <= 0f)
		{
			_SoundIntervel = 0f;
		}
		if (!(Math.Abs(rigid.linearVelocity.y) < 0.01f) || !(Math.Abs(rigid.linearVelocity.x) < 0.01f))
		{
			return;
		}
		addForceIntervalTimer += Time.deltaTime;
		if (addForceIntervalTimer > 0.5f)
		{
			addForceIntervalTimer = 0f;
			if ((float)UnityEngine.Random.Range(0, 1) < 0.5f)
			{
				rigid.AddForce(new Vector3(-0.05f, 0f, 0f));
			}
			else
			{
				rigid.AddForce(new Vector3(-0.05f, 0f, 0f));
			}
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.name == "Edge")
		{
			game222.PayInteract.SetCollider();
			UnityEngine.Object.Destroy(base.gameObject);
		}
		else
		{
			_SoundIntervel = SoundPlayIntervel;
			BounceSound.Play();
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		int id = other.gameObject.GetComponent<SpecialObj222_GrabDollReward>().id;
		if (!(other.gameObject.GetComponent<SpecialObj222_GrabDollReward>() != null))
		{
			return;
		}
		for (int i = 0; i < game222.rewards.Count; i++)
		{
			if (game222.rewards[i].rewardtype == other.gameObject.GetComponent<SpecialObj222_GrabDollReward>().rewardType && game222.rewards[i].rewardid == id)
			{
				game222.CreatNewReward(game222.rewards[i].positionid, game222.rewards[i].rewardtype);
				game222.rewards.RemoveAt(i);
				break;
			}
		}
		SEMgr.Inst.so222GetReward.PlaySE();
		switch (other.gameObject.GetComponent<SpecialObj222_GrabDollReward>().rewardType)
		{
		case SpecialObj222_GameGrabDoll.rewardType.SpellCommonlv1:
		case SpecialObj222_GameGrabDoll.rewardType.SpellCommonlv2:
		case SpecialObj222_GameGrabDoll.rewardType.SpellCommonlv3:
		case SpecialObj222_GameGrabDoll.rewardType.SpellRarelv1:
		case SpecialObj222_GameGrabDoll.rewardType.SpellRarelv2:
		case SpecialObj222_GameGrabDoll.rewardType.SpellRarelv3:
		case SpecialObj222_GameGrabDoll.rewardType.SpellEpic:
			LevelMgr.Inst.RoomFinishLogger?.AddCurrentSideRoomReward(PlayerLogger.Item.CreateSpell(id));
			break;
		case SpecialObj222_GameGrabDoll.rewardType.RelicCommon:
		case SpecialObj222_GameGrabDoll.rewardType.RelicRare:
			LevelMgr.Inst.RoomFinishLogger?.AddCurrentSideRoomReward(new PlayerLogger.Item
			{
				type = PlayerLogger.Item.Type.Relic,
				number = 1,
				id = id
			});
			break;
		case SpecialObj222_GameGrabDoll.rewardType.Curse:
			LevelMgr.Inst.RoomFinishLogger?.AddCurrentSideRoomReward(new PlayerLogger.Item
			{
				type = PlayerLogger.Item.Type.Curse,
				number = 1,
				id = id
			});
			break;
		case SpecialObj222_GameGrabDoll.rewardType.Coin:
			LevelMgr.Inst.RoomFinishLogger?.AddCurrentSideRoomReward(new PlayerLogger.Item
			{
				type = PlayerLogger.Item.Type.Coin,
				number = 1,
				id = id
			});
			break;
		case SpecialObj222_GameGrabDoll.rewardType.Dimond:
			LevelMgr.Inst.RoomFinishLogger?.AddCurrentSideRoomReward(new PlayerLogger.Item
			{
				type = PlayerLogger.Item.Type.Coin,
				number = 5,
				id = id
			});
			break;
		case SpecialObj222_GameGrabDoll.rewardType.Potion:
			LevelMgr.Inst.RoomFinishLogger?.AddCurrentSideRoomReward(new PlayerLogger.Item
			{
				type = PlayerLogger.Item.Type.Potion,
				number = 1,
				id = id
			});
			break;
		}
		SpecialObj222_GameGrabDoll.rewardType rewardType = other.gameObject.GetComponent<SpecialObj222_GrabDollReward>().rewardType;
		switch (rewardType)
		{
		case SpecialObj222_GameGrabDoll.rewardType.SpellCommonlv1:
		case SpecialObj222_GameGrabDoll.rewardType.SpellCommonlv2:
		case SpecialObj222_GameGrabDoll.rewardType.SpellCommonlv3:
		case SpecialObj222_GameGrabDoll.rewardType.SpellRarelv1:
		case SpecialObj222_GameGrabDoll.rewardType.SpellRarelv2:
		case SpecialObj222_GameGrabDoll.rewardType.SpellRarelv3:
		case SpecialObj222_GameGrabDoll.rewardType.SpellEpic:
		{
			PlayerItemController itemCtrller5 = PlayerMgr.Inst.ItemCtrller;
			Vector3 worldPoint5 = Tool2D.IgnoreZPoint(base.transform.position);
			Vector3 nextDropPoint6 = game222.nextDropPoint;
			RoomController roomCtrller = game222.gameHolder222.roomCtrller;
			itemCtrller5.RewardDropFly(id, (SpecialObj217.rewardType)rewardType, worldPoint5, nextDropPoint6, null, useParticleColor: true, null, isUI: false, dropItem: true, roomCtrller);
			break;
		}
		case SpecialObj222_GameGrabDoll.rewardType.RelicCommon:
		case SpecialObj222_GameGrabDoll.rewardType.RelicRare:
		{
			other.gameObject.GetComponent<SpecialObj222_GrabDollReward>().TurnToRelic();
			PlayerItemController itemCtrller4 = PlayerMgr.Inst.ItemCtrller;
			Vector3 worldPoint4 = Tool2D.IgnoreZPoint(base.transform.position);
			Vector3 nextDropPoint5 = game222.nextDropPoint;
			RoomController roomCtrller = game222.gameHolder222.roomCtrller;
			itemCtrller4.RewardDropFly(id, (SpecialObj217.rewardType)rewardType, worldPoint4, nextDropPoint5, null, useParticleColor: true, null, isUI: false, dropItem: true, roomCtrller);
			break;
		}
		case SpecialObj222_GameGrabDoll.rewardType.Curse:
			PlayerMgr.Inst.ItemCtrller.CurseAdd(id, Tool2D.IgnoreZPoint(base.transform.position));
			break;
		case SpecialObj222_GameGrabDoll.rewardType.Chest:
		{
			ChestType chestType = (ChestType)UnityEngine.Random.Range(0, 4);
			int id2 = 401;
			switch (chestType)
			{
			case ChestType.NoLock:
				id2 = 404;
				break;
			case ChestType.Lock:
				id2 = 401;
				break;
			case ChestType.Spike:
				id2 = 402;
				break;
			case ChestType.Curse:
				id2 = 403;
				break;
			default:
				Debug.LogError(chestType);
				break;
			}
			Entity entity = QuickCreateSystem.Inst.CreateSpecialObj(id2, Tool2D.IgnoreZPoint(base.transform.position));
			Vector3 nextDropPoint4 = game222.nextDropPoint;
			if (chestType == ChestType.NoLock)
			{
				SpecialObj4NoLock componentData = UnitDotsSyncSystem.GetComponentData<SpecialObj4NoLock>(entity);
				componentData.SetFly(nextDropPoint4);
				UnitDotsSyncSystem.SetComponentData(componentData, entity);
			}
			else
			{
				SpecialObj4_Dots componentData2 = UnitDotsSyncSystem.GetComponentData<SpecialObj4_Dots>(entity);
				componentData2.SetFly(nextDropPoint4);
				UnitDotsSyncSystem.SetComponentData(componentData2, entity);
			}
			IRoomCtrller_Dots componentData3 = UnitDotsSyncSystem.GetComponentData<IRoomCtrller_Dots>(entity);
			componentData3.belongRoom.Value = LevelMgr.Inst.CurrentRoomCtrller;
			componentData3.onRoomEnter = true;
			UnitDotsSyncSystem.SetComponentData(componentData3, entity);
			break;
		}
		case SpecialObj222_GameGrabDoll.rewardType.Coin:
		{
			PlayerItemController itemCtrller3 = PlayerMgr.Inst.ItemCtrller;
			Vector3 worldPoint3 = Tool2D.IgnoreZPoint(base.transform.position);
			Vector3 nextDropPoint3 = game222.nextDropPoint;
			RoomController roomCtrller = game222.gameHolder222.roomCtrller;
			itemCtrller3.RewardDropFly(11, SpecialObj217.rewardType.Coin, worldPoint3, nextDropPoint3, null, useParticleColor: true, null, isUI: false, dropItem: true, roomCtrller);
			break;
		}
		case SpecialObj222_GameGrabDoll.rewardType.Dimond:
		{
			PlayerItemController itemCtrller2 = PlayerMgr.Inst.ItemCtrller;
			Vector3 worldPoint2 = Tool2D.IgnoreZPoint(base.transform.position);
			Vector3 nextDropPoint2 = game222.nextDropPoint;
			RoomController roomCtrller = game222.gameHolder222.roomCtrller;
			itemCtrller2.RewardDropFly(12, SpecialObj217.rewardType.Dimond, worldPoint2, nextDropPoint2, null, useParticleColor: true, null, isUI: false, dropItem: true, roomCtrller);
			break;
		}
		case SpecialObj222_GameGrabDoll.rewardType.Potion:
		{
			PlayerItemController itemCtrller = PlayerMgr.Inst.ItemCtrller;
			Vector3 worldPoint = Tool2D.IgnoreZPoint(base.transform.position);
			Vector3 nextDropPoint = game222.nextDropPoint;
			RoomController roomCtrller = game222.gameHolder222.roomCtrller;
			itemCtrller.RewardDropFly(id, (SpecialObj217.rewardType)rewardType, worldPoint, nextDropPoint, null, useParticleColor: true, null, isUI: false, dropItem: true, roomCtrller);
			break;
		}
		}
		game222.PayInteract.animatorHandle.Play("Recover");
		game222.PayInteract.SetCollider();
		game222.EndPlay();
		UnityEngine.Object.Destroy(other.transform.parent.gameObject);
		UnityEngine.Object.Destroy(base.gameObject);
	}
}
