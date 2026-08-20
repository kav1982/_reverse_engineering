using UnityEngine;

public class SpecialObj222_GameBase : LayerCorrect, IRoomCtrller
{
	public enum state
	{
		stoped,
		starting,
		playing
	}

	public SpecialObj222 GameController;

	public RoomController roomCtrller;

	public state gameState;

	private void Start()
	{
		Initialize();
	}

	public virtual void Update()
	{
	}

	public virtual void Initialize()
	{
	}

	public virtual void DirectionControl(Vector2 vector2)
	{
	}

	public virtual void InteractControl()
	{
	}

	public virtual void BackControl()
	{
	}

	public virtual void SetRoomCtrlller(RoomController roomCtrller)
	{
		this.roomCtrller = roomCtrller;
	}

	public virtual void EndPlay()
	{
	}
}
