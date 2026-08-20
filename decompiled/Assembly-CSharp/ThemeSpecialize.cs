using UnityEngine;

public class ThemeSpecialize : MonoBehaviour
{
	public AbsRoomSpecialize[] absRoomSpecializes;

	public void Initialize(RoomController roomCtrller)
	{
		for (int i = 0; i < absRoomSpecializes.Length; i++)
		{
			absRoomSpecializes[i].RoomSpecializeInitialize(roomCtrller);
		}
		Object.Destroy(this);
	}
}
