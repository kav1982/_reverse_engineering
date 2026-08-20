using UnityEngine;

public class SpecialObj5_T6 : MonoBehaviour, IRoomCtrller
{
	public Transform tsf_Light;

	public float dontCreateDistance;

	private RoomController belongRoom;

	private Vector2 roomOffset;

	public void SetRoomCtrlller(RoomController roomCtrller)
	{
		belongRoom = roomCtrller;
	}

	private void Start()
	{
		tsf_Light.SetParent(base.transform.parent);
		if (!GameMgr.IsMobile_Static)
		{
			int num = belongRoom.roomCfg.theme6Width / 2;
			int num2 = belongRoom.roomCfg.theme6Height / 2;
			Vector3 centerPoint = belongRoom.CenterPoint;
			Vector3 position = tsf_Light.transform.position;
			if (centerPoint.x + (float)num - position.x < dontCreateDistance)
			{
				Vector3 position2 = tsf_Light.transform.position + new Vector3(-belongRoom.roomCfg.theme6Width, 0f, 0f);
				Object.Instantiate(tsf_Light.gameObject, position2, Quaternion.identity, base.transform.parent);
			}
			if (position.x - (centerPoint.x - (float)num) < dontCreateDistance)
			{
				Vector3 position3 = tsf_Light.transform.position + new Vector3(belongRoom.roomCfg.theme6Width, 0f, 0f);
				Object.Instantiate(tsf_Light.gameObject, position3, Quaternion.identity, base.transform.parent);
			}
			if (position.y - (centerPoint.y - (float)num2) < dontCreateDistance)
			{
				Vector3 position4 = tsf_Light.transform.position + new Vector3(0f, belongRoom.roomCfg.theme6Height, 0f);
				Object.Instantiate(tsf_Light.gameObject, position4, Quaternion.identity, base.transform.parent);
			}
			if (centerPoint.y + (float)num2 - position.y < dontCreateDistance)
			{
				Vector3 position5 = tsf_Light.transform.position + new Vector3(0f, -belongRoom.roomCfg.theme6Height, 0f);
				Object.Instantiate(tsf_Light.gameObject, position5, Quaternion.identity, base.transform.parent);
			}
		}
		Object.Destroy(base.gameObject);
	}
}
