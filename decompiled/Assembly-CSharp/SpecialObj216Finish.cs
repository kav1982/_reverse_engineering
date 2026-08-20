using PlayerLogger;
using UnityEngine;

public class SpecialObj216Finish : LayerCorrect, IRoomCtrller
{
	[Space(50f)]
	public Animator anima;

	public GameObject pfb_Matrix01;

	protected RoomController belongRoom;

	protected bool isEntered;

	public virtual void OnTriggerEnter(Collider other)
	{
		if (!isEntered && other.IsPlayerTrigger())
		{
			SO205PlayerEntered();
		}
	}

	public virtual void SO205PlayerEntered()
	{
		isEntered = true;
		anima.SetTrigger("On");
		ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Puzzle_Correct", base.transform.position, 2f);
		int specialRoomSpell = OutputMgr.GetSpecialRoomSpell();
		QuickCreateSystem.Inst.CreateItem(LevelMgr.Inst.CurrentRoomMapPos, new ItemInfo(ItemType.Spell, specialRoomSpell), base.transform.position);
		LevelMgr.Inst.RoomFinishLogger?.AddCurrentSideRoomReward(PlayerLogger.Item.CreateSpell(specialRoomSpell));
		belongRoom.SetAllTrapInvalid();
		SEMgr.Inst.puzzleSucceed.PlaySE();
		Vector3 position = base.transform.position + new Vector3(2f, 0f, 0f);
		Object.Instantiate(pfb_Matrix01, position, Quaternion.identity, base.transform.parent).GetComponent<SpecialObj216Matrix01>().SetFinish(isEntered: true, base.transform.position);
	}

	public void SetRoomCtrlller(RoomController roomCtrller)
	{
		belongRoom = roomCtrller;
	}
}
