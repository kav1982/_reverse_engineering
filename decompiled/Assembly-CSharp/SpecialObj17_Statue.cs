using UnityEngine;

public class SpecialObj17_Statue : LayerCorrect, IRoomCtrller
{
	public void SetRoomCtrlller(RoomController roomCtrller)
	{
		if (roomCtrller.roomCfg.isFlipped)
		{
			base.transform.localScale = new Vector3(-1f, 1f, 1f);
		}
	}
}
