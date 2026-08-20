using UnityEngine;

public class HideBoundaryBase : MonoBehaviour
{
	protected RoomController roomCtrller;

	protected FourDir dir;

	public virtual void Initialize(RoomController roomCtrller, FourDir dir)
	{
		this.roomCtrller = roomCtrller;
		this.dir = dir;
	}

	public virtual void Disappear()
	{
	}
}
