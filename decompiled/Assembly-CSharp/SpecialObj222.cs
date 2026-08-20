using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class SpecialObj222 : LayerCorrect, IRoomCtrller
{
	public bool Paied;

	[HideInInspector]
	public RoomController roomCtrller;

	public SpecialObj222_GameBase thisgame;

	public InputActions inputActions;

	public GameObject goHandle;

	public Vector3 handleOffsetFlipped;

	public NavMeshObstacle obstacle;

	public void SetRoomCtrlller(RoomController roomCtrller)
	{
		this.roomCtrller = roomCtrller;
	}

	private void Start()
	{
		inputActions = ControlMgr.Inst.inputActions;
		inputActions.Player.Shoot.performed += Interact;
		inputActions.Player.Interact.performed += Interact;
		inputActions.Player.Pause.performed += Back;
		inputActions.Player.GamepadEast.performed += Back;
		inputActions.Player.GamepadDirect.performed += GamepadDirectPerformed;
		inputActions.Player.WASD.performed += WASD;
		inputActions.Player.LeftStick.performed += GamepadDirectPerformed_Stick;
		if (!roomCtrller.roomCfg.isFlipped)
		{
			goHandle.transform.position += handleOffsetFlipped;
		}
		if (GameMgr.IsMobile_Static)
		{
			Vector3 size = obstacle.size;
			size.y += 10f;
			obstacle.size = size;
		}
	}

	private void WASD(InputAction.CallbackContext obj)
	{
		Vector2 vector = obj.ReadValue<Vector2>();
		thisgame.DirectionControl(vector);
	}

	private void GamepadDirectPerformed(InputAction.CallbackContext obj)
	{
		Vector2 vector = obj.ReadValue<Vector2>();
		thisgame.DirectionControl(vector);
	}

	private void GamepadDirectPerformed_Stick(InputAction.CallbackContext obj)
	{
		Vector2 vector = obj.ReadValue<Vector2>();
		vector = ControlMgr.Inst.RampVector2(vector);
		thisgame.DirectionControl(vector);
	}

	private void Interact(InputAction.CallbackContext obj)
	{
	}

	private void Back(InputAction.CallbackContext obj)
	{
	}
}
