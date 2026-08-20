using System;
using PlayerLogger;
using Unity.Entities;
using Unity.Physics;
using UnityEngine;

public class SpecialObj205 : LayerCorrect, IRoomCtrller, IDotsTriggerReceiver, IDotsPhysicsReciever
{
	[Space(50f)]
	public Animator anima;

	protected RoomController belongRoom;

	protected bool isEntered;

	public UnityEngine.BoxCollider thisCollider;

	Entity IDotsPhysicsReciever.thisEntity { get; set; }

	public event Action OnGameClear;

	protected void Start()
	{
		CollisionFilter collisionFilter = default(CollisionFilter);
		collisionFilter.BelongsTo = 67108864u;
		collisionFilter.CollidesWith = 512u;
		collisionFilter.GroupIndex = 0;
		CollisionFilter filter = collisionFilter;
		UnitPhysicsSyncSystem.RegisterReciever(this, filter, thisCollider);
	}

	protected void OnDestroy()
	{
		UnitPhysicsSyncSystem.UnregisterReciever(this);
	}

	public virtual void SO205PlayerEntered()
	{
		isEntered = true;
		anima.SetTrigger("On");
		ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Puzzle_Correct", base.transform.position, 2f);
		int specialRoomSpell = OutputMgr.GetSpecialRoomSpell();
		ItemInfo itemInfo = default(ItemInfo);
		itemInfo.type = ItemType.Spell;
		itemInfo.id = specialRoomSpell;
		ItemInfo info = itemInfo;
		QuickCreateSystem.Inst.CreateItem(LevelMgr.Inst.CurrentRoomMapPos, info, base.transform.position);
		LevelMgr.Inst.RoomFinishLogger?.AddCurrentSideRoomReward(PlayerLogger.Item.CreateSpell(specialRoomSpell));
		belongRoom.SetAllTrapInvalid();
		SEMgr.Inst.puzzleSucceed.PlaySE();
		if (this.OnGameClear != null)
		{
			this.OnGameClear();
			this.OnGameClear = null;
		}
	}

	public void SetRoomCtrlller(RoomController roomCtrller)
	{
		belongRoom = roomCtrller;
	}

	void IDotsTriggerReceiver.OnTriggerEnter_Dots(Entity other)
	{
		if (!isEntered && other == PlayerMgr.Inst.PlayerEtt)
		{
			SO205PlayerEntered();
		}
	}

	void IDotsTriggerReceiver.OnTriggerStay_Dots(Entity other)
	{
	}

	void IDotsTriggerReceiver.OnTriggerExit_Dots(Entity other)
	{
	}
}
