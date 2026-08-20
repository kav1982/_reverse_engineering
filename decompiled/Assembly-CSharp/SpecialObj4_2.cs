using UnityEngine;

public class SpecialObj4_2 : LayerCorrect, IRoomCtrller
{
	[Space(50f)]
	public Sprite sprite_ChestOpen;

	public SpriteRenderer sr;

	public Animator anima;

	public AnimaEvent animaEvent;

	private RoomController belongCtrller;

	private bool isOpened;

	private void Start()
	{
		animaEvent.DoAction = AnimaAction;
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (!isOpened && collision.transform.tag == "Player")
		{
			isOpened = true;
			anima.SetTrigger("Open");
		}
	}

	public void AnimaAction(string animaName)
	{
		if (animaName == "Open")
		{
			sr.sprite = sprite_ChestOpen;
			SEMgr.Inst.chestOpen.PlaySE();
			int id = ((belongCtrller.roomCfg.id == 108) ? 30121 : 31031);
			QuickCreateSystem.Inst.CreateItem(LevelMgr.Inst.CurrentRoomMapPos, new ItemInfo(ItemType.Spell, id), base.transform.position + new Vector3(0f, -0.1f, 0f));
		}
		else
		{
			Debug.LogError(animaName);
		}
	}

	public virtual void SetRoomCtrlller(RoomController roomCtrller)
	{
		belongCtrller = roomCtrller;
	}
}
