using UnityEngine;

public class SpcialObj213 : LayerCorrect, IRoomCtrller
{
	private enum State
	{
		Idle,
		Trigger,
		Running
	}

	[Space(50f)]
	public Animator anima;

	private State state;

	private RoomController belongCtrller;

	private void Update()
	{
		if (state == State.Trigger)
		{
			state = State.Running;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.IsPlayerTrigger())
		{
			anima.SetTrigger("On");
			state = State.Trigger;
		}
	}

	public void SetRoomCtrlller(RoomController levelCtrller)
	{
		belongCtrller = levelCtrller;
	}
}
