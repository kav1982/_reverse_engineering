using UnityEngine;
using UnityEngine.InputSystem;

public class UIReleaseNoteSmall : MonoBehaviour
{
	public void Awake()
	{
		ControlMgr.Inst.inputActions.Player.Drink.performed += PadShowRealeaseNote;
	}

	private void PadShowRealeaseNote(InputAction.CallbackContext context)
	{
		if (!GameUISingletonMono<UIReleaseNote>.StaticIsOpen && ControlMgr.Inst.InputType != 0)
		{
			ShowRealeaseNote();
		}
	}

	public void ShowRealeaseNote()
	{
		UIMainMenuMgr.Inst.TryShowReleaseNote();
	}

	private void OnDestroy()
	{
		ControlMgr.Inst.inputActions.Player.Drink.performed -= PadShowRealeaseNote;
	}
}
