using UnityEngine;

public class Boundary_T5_Water : AbsRoomSpecialize
{
	public Transform tsf_Layer;

	public SpriteRenderer sr_Water;

	public override void RoomSpecializeInitialize(RoomController roomCtrller)
	{
		tsf_Layer.position = Tool2D.GetLayerPoint(roomCtrller.transform.position, LayerCorrectType.Lava0);
		sr_Water.transform.localScale = new Vector3(60f, 60f, 1f);
		Object.Destroy(this);
	}
}
