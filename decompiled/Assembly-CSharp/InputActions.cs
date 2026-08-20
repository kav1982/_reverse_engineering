using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class InputActions : IInputActionCollection2, IInputActionCollection, IEnumerable<InputAction>, IEnumerable, IDisposable
{
	public struct PlayerActions
	{
		private InputActions m_Wrapper;

		public InputAction AnyKey => m_Wrapper.m_Player_AnyKey;

		public InputAction AnyMouseKeyboardInput => m_Wrapper.m_Player_AnyMouseKeyboardInput;

		public InputAction AnyGamepadInput => m_Wrapper.m_Player_AnyGamepadInput;

		public InputAction Space => m_Wrapper.m_Player_Space;

		public InputAction WASD => m_Wrapper.m_Player_WASD;

		public InputAction LeftStick => m_Wrapper.m_Player_LeftStick;

		public InputAction RightStick => m_Wrapper.m_Player_RightStick;

		public InputAction Shoot => m_Wrapper.m_Player_Shoot;

		public InputAction Drink => m_Wrapper.m_Player_Drink;

		public InputAction Interact => m_Wrapper.m_Player_Interact;

		public InputAction Alpha1 => m_Wrapper.m_Player_Alpha1;

		public InputAction Alpha2 => m_Wrapper.m_Player_Alpha2;

		public InputAction Alpha3 => m_Wrapper.m_Player_Alpha3;

		public InputAction Alpha4 => m_Wrapper.m_Player_Alpha4;

		public InputAction Alpha5 => m_Wrapper.m_Player_Alpha5;

		public InputAction Alpha6 => m_Wrapper.m_Player_Alpha6;

		public InputAction Alpha7 => m_Wrapper.m_Player_Alpha7;

		public InputAction WandUp => m_Wrapper.m_Player_WandUp;

		public InputAction WandDown => m_Wrapper.m_Player_WandDown;

		public InputAction Pause => m_Wrapper.m_Player_Pause;

		public InputAction Bag => m_Wrapper.m_Player_Bag;

		public InputAction GamepadDirect => m_Wrapper.m_Player_GamepadDirect;

		public InputAction GamepadLB => m_Wrapper.m_Player_GamepadLB;

		public InputAction GamepadLT => m_Wrapper.m_Player_GamepadLT;

		public InputAction GamepadRT => m_Wrapper.m_Player_GamepadRT;

		public InputAction GamepadRB => m_Wrapper.m_Player_GamepadRB;

		public InputAction GamepadEast => m_Wrapper.m_Player_GamepadEast;

		public InputAction GamepadWest => m_Wrapper.m_Player_GamepadWest;

		public InputAction GamepadDpad => m_Wrapper.m_Player_GamepadDpad;

		public InputAction KeyboardQ => m_Wrapper.m_Player_KeyboardQ;

		public InputAction KeyboardE => m_Wrapper.m_Player_KeyboardE;

		public InputAction KeyboardA => m_Wrapper.m_Player_KeyboardA;

		public InputAction KeyboardD => m_Wrapper.m_Player_KeyboardD;

		public InputAction CustomAction => m_Wrapper.m_Player_CustomAction;

		public InputAction Alpha => m_Wrapper.m_Player_Alpha;

		public InputAction PotionUp => m_Wrapper.m_Player_PotionUp;

		public InputAction PotionDown => m_Wrapper.m_Player_PotionDown;

		public InputAction QuickRemove => m_Wrapper.m_Player_QuickRemove;

		public InputAction Drop => m_Wrapper.m_Player_Drop;

		public InputAction AnyTouchScreen => m_Wrapper.m_Player_AnyTouchScreen;

		public InputAction Sprint => m_Wrapper.m_Player_Sprint;

		public InputAction QuickPanel => m_Wrapper.m_Player_QuickPanel;

		public InputAction KillSummon => m_Wrapper.m_Player_KillSummon;

		public bool enabled => Get().enabled;

		public PlayerActions(InputActions wrapper)
		{
			m_Wrapper = wrapper;
		}

		public InputActionMap Get()
		{
			return m_Wrapper.m_Player;
		}

		public void Enable()
		{
			Get().Enable();
		}

		public void Disable()
		{
			Get().Disable();
		}

		public static implicit operator InputActionMap(PlayerActions set)
		{
			return set.Get();
		}

		public void AddCallbacks(IPlayerActions instance)
		{
			if (instance != null && !m_Wrapper.m_PlayerActionsCallbackInterfaces.Contains(instance))
			{
				m_Wrapper.m_PlayerActionsCallbackInterfaces.Add(instance);
				AnyKey.started += instance.OnAnyKey;
				AnyKey.performed += instance.OnAnyKey;
				AnyKey.canceled += instance.OnAnyKey;
				AnyMouseKeyboardInput.started += instance.OnAnyMouseKeyboardInput;
				AnyMouseKeyboardInput.performed += instance.OnAnyMouseKeyboardInput;
				AnyMouseKeyboardInput.canceled += instance.OnAnyMouseKeyboardInput;
				AnyGamepadInput.started += instance.OnAnyGamepadInput;
				AnyGamepadInput.performed += instance.OnAnyGamepadInput;
				AnyGamepadInput.canceled += instance.OnAnyGamepadInput;
				Space.started += instance.OnSpace;
				Space.performed += instance.OnSpace;
				Space.canceled += instance.OnSpace;
				WASD.started += instance.OnWASD;
				WASD.performed += instance.OnWASD;
				WASD.canceled += instance.OnWASD;
				LeftStick.started += instance.OnLeftStick;
				LeftStick.performed += instance.OnLeftStick;
				LeftStick.canceled += instance.OnLeftStick;
				RightStick.started += instance.OnRightStick;
				RightStick.performed += instance.OnRightStick;
				RightStick.canceled += instance.OnRightStick;
				Shoot.started += instance.OnShoot;
				Shoot.performed += instance.OnShoot;
				Shoot.canceled += instance.OnShoot;
				Drink.started += instance.OnDrink;
				Drink.performed += instance.OnDrink;
				Drink.canceled += instance.OnDrink;
				Interact.started += instance.OnInteract;
				Interact.performed += instance.OnInteract;
				Interact.canceled += instance.OnInteract;
				Alpha1.started += instance.OnAlpha1;
				Alpha1.performed += instance.OnAlpha1;
				Alpha1.canceled += instance.OnAlpha1;
				Alpha2.started += instance.OnAlpha2;
				Alpha2.performed += instance.OnAlpha2;
				Alpha2.canceled += instance.OnAlpha2;
				Alpha3.started += instance.OnAlpha3;
				Alpha3.performed += instance.OnAlpha3;
				Alpha3.canceled += instance.OnAlpha3;
				Alpha4.started += instance.OnAlpha4;
				Alpha4.performed += instance.OnAlpha4;
				Alpha4.canceled += instance.OnAlpha4;
				Alpha5.started += instance.OnAlpha5;
				Alpha5.performed += instance.OnAlpha5;
				Alpha5.canceled += instance.OnAlpha5;
				Alpha6.started += instance.OnAlpha6;
				Alpha6.performed += instance.OnAlpha6;
				Alpha6.canceled += instance.OnAlpha6;
				Alpha7.started += instance.OnAlpha7;
				Alpha7.performed += instance.OnAlpha7;
				Alpha7.canceled += instance.OnAlpha7;
				WandUp.started += instance.OnWandUp;
				WandUp.performed += instance.OnWandUp;
				WandUp.canceled += instance.OnWandUp;
				WandDown.started += instance.OnWandDown;
				WandDown.performed += instance.OnWandDown;
				WandDown.canceled += instance.OnWandDown;
				Pause.started += instance.OnPause;
				Pause.performed += instance.OnPause;
				Pause.canceled += instance.OnPause;
				Bag.started += instance.OnBag;
				Bag.performed += instance.OnBag;
				Bag.canceled += instance.OnBag;
				GamepadDirect.started += instance.OnGamepadDirect;
				GamepadDirect.performed += instance.OnGamepadDirect;
				GamepadDirect.canceled += instance.OnGamepadDirect;
				GamepadLB.started += instance.OnGamepadLB;
				GamepadLB.performed += instance.OnGamepadLB;
				GamepadLB.canceled += instance.OnGamepadLB;
				GamepadLT.started += instance.OnGamepadLT;
				GamepadLT.performed += instance.OnGamepadLT;
				GamepadLT.canceled += instance.OnGamepadLT;
				GamepadRT.started += instance.OnGamepadRT;
				GamepadRT.performed += instance.OnGamepadRT;
				GamepadRT.canceled += instance.OnGamepadRT;
				GamepadRB.started += instance.OnGamepadRB;
				GamepadRB.performed += instance.OnGamepadRB;
				GamepadRB.canceled += instance.OnGamepadRB;
				GamepadEast.started += instance.OnGamepadEast;
				GamepadEast.performed += instance.OnGamepadEast;
				GamepadEast.canceled += instance.OnGamepadEast;
				GamepadWest.started += instance.OnGamepadWest;
				GamepadWest.performed += instance.OnGamepadWest;
				GamepadWest.canceled += instance.OnGamepadWest;
				GamepadDpad.started += instance.OnGamepadDpad;
				GamepadDpad.performed += instance.OnGamepadDpad;
				GamepadDpad.canceled += instance.OnGamepadDpad;
				KeyboardQ.started += instance.OnKeyboardQ;
				KeyboardQ.performed += instance.OnKeyboardQ;
				KeyboardQ.canceled += instance.OnKeyboardQ;
				KeyboardE.started += instance.OnKeyboardE;
				KeyboardE.performed += instance.OnKeyboardE;
				KeyboardE.canceled += instance.OnKeyboardE;
				KeyboardA.started += instance.OnKeyboardA;
				KeyboardA.performed += instance.OnKeyboardA;
				KeyboardA.canceled += instance.OnKeyboardA;
				KeyboardD.started += instance.OnKeyboardD;
				KeyboardD.performed += instance.OnKeyboardD;
				KeyboardD.canceled += instance.OnKeyboardD;
				CustomAction.started += instance.OnCustomAction;
				CustomAction.performed += instance.OnCustomAction;
				CustomAction.canceled += instance.OnCustomAction;
				Alpha.started += instance.OnAlpha;
				Alpha.performed += instance.OnAlpha;
				Alpha.canceled += instance.OnAlpha;
				PotionUp.started += instance.OnPotionUp;
				PotionUp.performed += instance.OnPotionUp;
				PotionUp.canceled += instance.OnPotionUp;
				PotionDown.started += instance.OnPotionDown;
				PotionDown.performed += instance.OnPotionDown;
				PotionDown.canceled += instance.OnPotionDown;
				QuickRemove.started += instance.OnQuickRemove;
				QuickRemove.performed += instance.OnQuickRemove;
				QuickRemove.canceled += instance.OnQuickRemove;
				Drop.started += instance.OnDrop;
				Drop.performed += instance.OnDrop;
				Drop.canceled += instance.OnDrop;
				AnyTouchScreen.started += instance.OnAnyTouchScreen;
				AnyTouchScreen.performed += instance.OnAnyTouchScreen;
				AnyTouchScreen.canceled += instance.OnAnyTouchScreen;
				Sprint.started += instance.OnSprint;
				Sprint.performed += instance.OnSprint;
				Sprint.canceled += instance.OnSprint;
				QuickPanel.started += instance.OnQuickPanel;
				QuickPanel.performed += instance.OnQuickPanel;
				QuickPanel.canceled += instance.OnQuickPanel;
				KillSummon.started += instance.OnKillSummon;
				KillSummon.performed += instance.OnKillSummon;
				KillSummon.canceled += instance.OnKillSummon;
			}
		}

		private void UnregisterCallbacks(IPlayerActions instance)
		{
			AnyKey.started -= instance.OnAnyKey;
			AnyKey.performed -= instance.OnAnyKey;
			AnyKey.canceled -= instance.OnAnyKey;
			AnyMouseKeyboardInput.started -= instance.OnAnyMouseKeyboardInput;
			AnyMouseKeyboardInput.performed -= instance.OnAnyMouseKeyboardInput;
			AnyMouseKeyboardInput.canceled -= instance.OnAnyMouseKeyboardInput;
			AnyGamepadInput.started -= instance.OnAnyGamepadInput;
			AnyGamepadInput.performed -= instance.OnAnyGamepadInput;
			AnyGamepadInput.canceled -= instance.OnAnyGamepadInput;
			Space.started -= instance.OnSpace;
			Space.performed -= instance.OnSpace;
			Space.canceled -= instance.OnSpace;
			WASD.started -= instance.OnWASD;
			WASD.performed -= instance.OnWASD;
			WASD.canceled -= instance.OnWASD;
			LeftStick.started -= instance.OnLeftStick;
			LeftStick.performed -= instance.OnLeftStick;
			LeftStick.canceled -= instance.OnLeftStick;
			RightStick.started -= instance.OnRightStick;
			RightStick.performed -= instance.OnRightStick;
			RightStick.canceled -= instance.OnRightStick;
			Shoot.started -= instance.OnShoot;
			Shoot.performed -= instance.OnShoot;
			Shoot.canceled -= instance.OnShoot;
			Drink.started -= instance.OnDrink;
			Drink.performed -= instance.OnDrink;
			Drink.canceled -= instance.OnDrink;
			Interact.started -= instance.OnInteract;
			Interact.performed -= instance.OnInteract;
			Interact.canceled -= instance.OnInteract;
			Alpha1.started -= instance.OnAlpha1;
			Alpha1.performed -= instance.OnAlpha1;
			Alpha1.canceled -= instance.OnAlpha1;
			Alpha2.started -= instance.OnAlpha2;
			Alpha2.performed -= instance.OnAlpha2;
			Alpha2.canceled -= instance.OnAlpha2;
			Alpha3.started -= instance.OnAlpha3;
			Alpha3.performed -= instance.OnAlpha3;
			Alpha3.canceled -= instance.OnAlpha3;
			Alpha4.started -= instance.OnAlpha4;
			Alpha4.performed -= instance.OnAlpha4;
			Alpha4.canceled -= instance.OnAlpha4;
			Alpha5.started -= instance.OnAlpha5;
			Alpha5.performed -= instance.OnAlpha5;
			Alpha5.canceled -= instance.OnAlpha5;
			Alpha6.started -= instance.OnAlpha6;
			Alpha6.performed -= instance.OnAlpha6;
			Alpha6.canceled -= instance.OnAlpha6;
			Alpha7.started -= instance.OnAlpha7;
			Alpha7.performed -= instance.OnAlpha7;
			Alpha7.canceled -= instance.OnAlpha7;
			WandUp.started -= instance.OnWandUp;
			WandUp.performed -= instance.OnWandUp;
			WandUp.canceled -= instance.OnWandUp;
			WandDown.started -= instance.OnWandDown;
			WandDown.performed -= instance.OnWandDown;
			WandDown.canceled -= instance.OnWandDown;
			Pause.started -= instance.OnPause;
			Pause.performed -= instance.OnPause;
			Pause.canceled -= instance.OnPause;
			Bag.started -= instance.OnBag;
			Bag.performed -= instance.OnBag;
			Bag.canceled -= instance.OnBag;
			GamepadDirect.started -= instance.OnGamepadDirect;
			GamepadDirect.performed -= instance.OnGamepadDirect;
			GamepadDirect.canceled -= instance.OnGamepadDirect;
			GamepadLB.started -= instance.OnGamepadLB;
			GamepadLB.performed -= instance.OnGamepadLB;
			GamepadLB.canceled -= instance.OnGamepadLB;
			GamepadLT.started -= instance.OnGamepadLT;
			GamepadLT.performed -= instance.OnGamepadLT;
			GamepadLT.canceled -= instance.OnGamepadLT;
			GamepadRT.started -= instance.OnGamepadRT;
			GamepadRT.performed -= instance.OnGamepadRT;
			GamepadRT.canceled -= instance.OnGamepadRT;
			GamepadRB.started -= instance.OnGamepadRB;
			GamepadRB.performed -= instance.OnGamepadRB;
			GamepadRB.canceled -= instance.OnGamepadRB;
			GamepadEast.started -= instance.OnGamepadEast;
			GamepadEast.performed -= instance.OnGamepadEast;
			GamepadEast.canceled -= instance.OnGamepadEast;
			GamepadWest.started -= instance.OnGamepadWest;
			GamepadWest.performed -= instance.OnGamepadWest;
			GamepadWest.canceled -= instance.OnGamepadWest;
			GamepadDpad.started -= instance.OnGamepadDpad;
			GamepadDpad.performed -= instance.OnGamepadDpad;
			GamepadDpad.canceled -= instance.OnGamepadDpad;
			KeyboardQ.started -= instance.OnKeyboardQ;
			KeyboardQ.performed -= instance.OnKeyboardQ;
			KeyboardQ.canceled -= instance.OnKeyboardQ;
			KeyboardE.started -= instance.OnKeyboardE;
			KeyboardE.performed -= instance.OnKeyboardE;
			KeyboardE.canceled -= instance.OnKeyboardE;
			KeyboardA.started -= instance.OnKeyboardA;
			KeyboardA.performed -= instance.OnKeyboardA;
			KeyboardA.canceled -= instance.OnKeyboardA;
			KeyboardD.started -= instance.OnKeyboardD;
			KeyboardD.performed -= instance.OnKeyboardD;
			KeyboardD.canceled -= instance.OnKeyboardD;
			CustomAction.started -= instance.OnCustomAction;
			CustomAction.performed -= instance.OnCustomAction;
			CustomAction.canceled -= instance.OnCustomAction;
			Alpha.started -= instance.OnAlpha;
			Alpha.performed -= instance.OnAlpha;
			Alpha.canceled -= instance.OnAlpha;
			PotionUp.started -= instance.OnPotionUp;
			PotionUp.performed -= instance.OnPotionUp;
			PotionUp.canceled -= instance.OnPotionUp;
			PotionDown.started -= instance.OnPotionDown;
			PotionDown.performed -= instance.OnPotionDown;
			PotionDown.canceled -= instance.OnPotionDown;
			QuickRemove.started -= instance.OnQuickRemove;
			QuickRemove.performed -= instance.OnQuickRemove;
			QuickRemove.canceled -= instance.OnQuickRemove;
			Drop.started -= instance.OnDrop;
			Drop.performed -= instance.OnDrop;
			Drop.canceled -= instance.OnDrop;
			AnyTouchScreen.started -= instance.OnAnyTouchScreen;
			AnyTouchScreen.performed -= instance.OnAnyTouchScreen;
			AnyTouchScreen.canceled -= instance.OnAnyTouchScreen;
			Sprint.started -= instance.OnSprint;
			Sprint.performed -= instance.OnSprint;
			Sprint.canceled -= instance.OnSprint;
			QuickPanel.started -= instance.OnQuickPanel;
			QuickPanel.performed -= instance.OnQuickPanel;
			QuickPanel.canceled -= instance.OnQuickPanel;
			KillSummon.started -= instance.OnKillSummon;
			KillSummon.performed -= instance.OnKillSummon;
			KillSummon.canceled -= instance.OnKillSummon;
		}

		public void RemoveCallbacks(IPlayerActions instance)
		{
			if (m_Wrapper.m_PlayerActionsCallbackInterfaces.Remove(instance))
			{
				UnregisterCallbacks(instance);
			}
		}

		public void SetCallbacks(IPlayerActions instance)
		{
			foreach (IPlayerActions playerActionsCallbackInterface in m_Wrapper.m_PlayerActionsCallbackInterfaces)
			{
				UnregisterCallbacks(playerActionsCallbackInterface);
			}
			m_Wrapper.m_PlayerActionsCallbackInterfaces.Clear();
			AddCallbacks(instance);
		}
	}

	public interface IPlayerActions
	{
		void OnAnyKey(InputAction.CallbackContext context);

		void OnAnyMouseKeyboardInput(InputAction.CallbackContext context);

		void OnAnyGamepadInput(InputAction.CallbackContext context);

		void OnSpace(InputAction.CallbackContext context);

		void OnWASD(InputAction.CallbackContext context);

		void OnLeftStick(InputAction.CallbackContext context);

		void OnRightStick(InputAction.CallbackContext context);

		void OnShoot(InputAction.CallbackContext context);

		void OnDrink(InputAction.CallbackContext context);

		void OnInteract(InputAction.CallbackContext context);

		void OnAlpha1(InputAction.CallbackContext context);

		void OnAlpha2(InputAction.CallbackContext context);

		void OnAlpha3(InputAction.CallbackContext context);

		void OnAlpha4(InputAction.CallbackContext context);

		void OnAlpha5(InputAction.CallbackContext context);

		void OnAlpha6(InputAction.CallbackContext context);

		void OnAlpha7(InputAction.CallbackContext context);

		void OnWandUp(InputAction.CallbackContext context);

		void OnWandDown(InputAction.CallbackContext context);

		void OnPause(InputAction.CallbackContext context);

		void OnBag(InputAction.CallbackContext context);

		void OnGamepadDirect(InputAction.CallbackContext context);

		void OnGamepadLB(InputAction.CallbackContext context);

		void OnGamepadLT(InputAction.CallbackContext context);

		void OnGamepadRT(InputAction.CallbackContext context);

		void OnGamepadRB(InputAction.CallbackContext context);

		void OnGamepadEast(InputAction.CallbackContext context);

		void OnGamepadWest(InputAction.CallbackContext context);

		void OnGamepadDpad(InputAction.CallbackContext context);

		void OnKeyboardQ(InputAction.CallbackContext context);

		void OnKeyboardE(InputAction.CallbackContext context);

		void OnKeyboardA(InputAction.CallbackContext context);

		void OnKeyboardD(InputAction.CallbackContext context);

		void OnCustomAction(InputAction.CallbackContext context);

		void OnAlpha(InputAction.CallbackContext context);

		void OnPotionUp(InputAction.CallbackContext context);

		void OnPotionDown(InputAction.CallbackContext context);

		void OnQuickRemove(InputAction.CallbackContext context);

		void OnDrop(InputAction.CallbackContext context);

		void OnAnyTouchScreen(InputAction.CallbackContext context);

		void OnSprint(InputAction.CallbackContext context);

		void OnQuickPanel(InputAction.CallbackContext context);

		void OnKillSummon(InputAction.CallbackContext context);
	}

	private readonly InputActionMap m_Player;

	private List<IPlayerActions> m_PlayerActionsCallbackInterfaces = new List<IPlayerActions>();

	private readonly InputAction m_Player_AnyKey;

	private readonly InputAction m_Player_AnyMouseKeyboardInput;

	private readonly InputAction m_Player_AnyGamepadInput;

	private readonly InputAction m_Player_Space;

	private readonly InputAction m_Player_WASD;

	private readonly InputAction m_Player_LeftStick;

	private readonly InputAction m_Player_RightStick;

	private readonly InputAction m_Player_Shoot;

	private readonly InputAction m_Player_Drink;

	private readonly InputAction m_Player_Interact;

	private readonly InputAction m_Player_Alpha1;

	private readonly InputAction m_Player_Alpha2;

	private readonly InputAction m_Player_Alpha3;

	private readonly InputAction m_Player_Alpha4;

	private readonly InputAction m_Player_Alpha5;

	private readonly InputAction m_Player_Alpha6;

	private readonly InputAction m_Player_Alpha7;

	private readonly InputAction m_Player_WandUp;

	private readonly InputAction m_Player_WandDown;

	private readonly InputAction m_Player_Pause;

	private readonly InputAction m_Player_Bag;

	private readonly InputAction m_Player_GamepadDirect;

	private readonly InputAction m_Player_GamepadLB;

	private readonly InputAction m_Player_GamepadLT;

	private readonly InputAction m_Player_GamepadRT;

	private readonly InputAction m_Player_GamepadRB;

	private readonly InputAction m_Player_GamepadEast;

	private readonly InputAction m_Player_GamepadWest;

	private readonly InputAction m_Player_GamepadDpad;

	private readonly InputAction m_Player_KeyboardQ;

	private readonly InputAction m_Player_KeyboardE;

	private readonly InputAction m_Player_KeyboardA;

	private readonly InputAction m_Player_KeyboardD;

	private readonly InputAction m_Player_CustomAction;

	private readonly InputAction m_Player_Alpha;

	private readonly InputAction m_Player_PotionUp;

	private readonly InputAction m_Player_PotionDown;

	private readonly InputAction m_Player_QuickRemove;

	private readonly InputAction m_Player_Drop;

	private readonly InputAction m_Player_AnyTouchScreen;

	private readonly InputAction m_Player_Sprint;

	private readonly InputAction m_Player_QuickPanel;

	private readonly InputAction m_Player_KillSummon;

	public InputActionAsset asset { get; }

	public InputBinding? bindingMask
	{
		get
		{
			return asset.bindingMask;
		}
		set
		{
			asset.bindingMask = value;
		}
	}

	public ReadOnlyArray<InputDevice>? devices
	{
		get
		{
			return asset.devices;
		}
		set
		{
			asset.devices = value;
		}
	}

	public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

	public IEnumerable<InputBinding> bindings => asset.bindings;

	public PlayerActions Player => new PlayerActions(this);

	public InputActions()
	{
		asset = InputActionAsset.FromJson("{\r\n    \"version\": 1,\r\n    \"name\": \"InputActions\",\r\n    \"maps\": [\r\n        {\r\n            \"name\": \"Player\",\r\n            \"id\": \"bbe0761a-db82-4d33-87ca-2ff888654a24\",\r\n            \"actions\": [\r\n                {\r\n                    \"name\": \"AnyKey\",\r\n                    \"type\": \"Button\",\r\n                    \"id\": \"7dc003de-f0cb-461c-8879-82dcb224dbb3\",\r\n                    \"expectedControlType\": \"Button\",\r\n                    \"processors\": \"\",\r\n                    \"interactions\": \"\",\r\n                    \"initialStateCheck\": false\r\n                },\r\n                {\r\n                    \"name\": \"AnyMouseKeyboardInput\",\r\n                    \"type\": \"Value\",\r\n                    \"id\": \"ae78b4c8-5a42-43ac-a452-e479545bfbec\",\r\n                    \"expectedControlType\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"interactions\": \"\",\r\n                    \"initialStateCheck\": true\r\n                },\r\n                {\r\n                    \"name\": \"AnyGamepadInput\",\r\n                    \"type\": \"Value\",\r\n                    \"id\": \"a569ccae-6ea5-48c4-8d0b-317bcf018b08\",\r\n                    \"expectedControlType\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"interactions\": \"\",\r\n                    \"initialStateCheck\": true\r\n                },\r\n                {\r\n                    \"name\": \"Space\",\r\n                    \"type\": \"Button\",\r\n                    \"id\": \"5136117c-f283-4586-a0b9-151ddd5fc819\",\r\n                    \"expectedControlType\": \"Button\",\r\n                    \"processors\": \"\",\r\n                    \"interactions\": \"\",\r\n                    \"initialStateCheck\": false\r\n                },\r\n                {\r\n                    \"name\": \"WASD\",\r\n                    \"type\": \"Value\",\r\n                    \"id\": \"7998d7ff-0a04-428c-a62f-dc69e082e9fd\",\r\n                    \"expectedControlType\": \"Vector2\",\r\n                    \"processors\": \"\",\r\n                    \"interactions\": \"\",\r\n                    \"initialStateCheck\": true\r\n                },\r\n                {\r\n                    \"name\": \"LeftStick\",\r\n                    \"type\": \"Value\",\r\n                    \"id\": \"a76758cc-bc5c-4c30-83bf-1af5df8f1bfd\",\r\n                    \"expectedControlType\": \"Stick\",\r\n                    \"processors\": \"\",\r\n                    \"interactions\": \"\",\r\n                    \"initialStateCheck\": true\r\n                },\r\n                {\r\n                    \"name\": \"RightStick\",\r\n                    \"type\": \"Value\",\r\n                    \"id\": \"816e5f25-6aed-42e3-a7c9-d49df4ae55e2\",\r\n                    \"expectedControlType\": \"Stick\",\r\n                    \"processors\": \"\",\r\n                    \"interactions\": \"\",\r\n                    \"initialStateCheck\": true\r\n                },\r\n                {\r\n                    \"name\": \"Shoot\",\r\n                    \"type\": \"Button\",\r\n                    \"id\": \"27e951d5-4528-4608-bcdc-ba6e6150c70e\",\r\n                    \"expectedControlType\": \"Button\",\r\n                    \"processors\": \"\",\r\n                    \"interactions\": \"\",\r\n                    \"initialStateCheck\": false\r\n                },\r\n                {\r\n                    \"name\": \"Drink\",\r\n                    \"type\": \"Button\",\r\n                    \"id\": \"59d5b72e-573f-4b8a-8d7e-73c9cae43b26\",\r\n                    \"expectedControlType\": \"Button\",\r\n                    \"processors\": \"\",\r\n                    \"interactions\": \"\",\r\n                    \"initialStateCheck\": false\r\n                },\r\n                {\r\n                    \"name\": \"Interact\",\r\n                    \"type\": \"Button\",\r\n                    \"id\": \"592da234-7b58-4eb9-9a51-db5a6d548ce7\",\r\n                    \"expectedControlType\": \"Button\",\r\n                    \"processors\": \"\",\r\n                    \"interactions\": \"\",\r\n                    \"initialStateCheck\": true\r\n                },\r\n                {\r\n                    \"name\": \"Alpha1\",\r\n                    \"type\": \"Button\",\r\n                    \"id\": \"61d8ecc3-d144-42aa-abf9-c136dfb69884\",\r\n                    \"expectedControlType\": \"Button\",\r\n                    \"processors\": \"\",\r\n                    \"interactions\": \"\",\r\n                    \"initialStateCheck\": false\r\n                },\r\n                {\r\n                    \"name\": \"Alpha2\",\r\n                    \"type\": \"Button\",\r\n                    \"id\": \"0ed0fa88-67ff-4b98-8d13-6f025022b5a8\",\r\n                    \"expectedControlType\": \"Button\",\r\n                    \"processors\": \"\",\r\n                    \"interactions\": \"\",\r\n                    \"initialStateCheck\": false\r\n                },\r\n                {\r\n                    \"name\": \"Alpha3\",\r\n                    \"type\": \"Button\",\r\n                    \"id\": \"5f2e22de-a674-47f1-990d-b408edebc7fb\",\r\n                    \"expectedControlType\": \"Button\",\r\n                    \"processors\": \"\",\r\n                    \"interactions\": \"\",\r\n                    \"initialStateCheck\": false\r\n                },\r\n                {\r\n                    \"name\": \"Alpha4\",\r\n                    \"type\": \"Button\",\r\n                    \"id\": \"6c550682-7892-427c-9a67-4ea8ba8cd29b\",\r\n                    \"expectedControlType\": \"Button\",\r\n                    \"processors\": \"\",\r\n                    \"interactions\": \"\",\r\n                    \"initialStateCheck\": false\r\n                },\r\n                {\r\n                    \"name\": \"Alpha5\",\r\n                    \"type\": \"Button\",\r\n                    \"id\": \"21b57392-a04a-4231-8fc6-b75a39084943\",\r\n                    \"expectedControlType\": \"Button\",\r\n                    \"processors\": \"\",\r\n                    \"interactions\": \"\",\r\n                    \"initialStateCheck\": false\r\n                },\r\n                {\r\n                    \"name\": \"Alpha6\",\r\n                    \"type\": \"Button\",\r\n                    \"id\": \"4058805e-6d95-4d90-bf02-924486eab7af\",\r\n                    \"expectedControlType\": \"Button\",\r\n                    \"processors\": \"\",\r\n                    \"interactions\": \"\",\r\n                    \"initialStateCheck\": false\r\n                },\r\n                {\r\n                    \"name\": \"Alpha7\",\r\n                    \"type\": \"Button\",\r\n                    \"id\": \"ffcfea9b-0da6-4ea0-b4bb-475df15845d3\",\r\n                    \"expectedControlType\": \"Button\",\r\n                    \"processors\": \"\",\r\n                    \"interactions\": \"\",\r\n                    \"initialStateCheck\": false\r\n                },\r\n                {\r\n                    \"name\": \"WandUp\",\r\n                    \"type\": \"Button\",\r\n                    \"id\": \"2166a079-56ee-4ece-861f-9e620e5b3104\",\r\n                    \"expectedControlType\": \"Button\",\r\n                    \"processors\": \"\",\r\n                    \"interactions\": \"\",\r\n                    \"initialStateCheck\": true\r\n                },\r\n                {\r\n                    \"name\": \"WandDown\",\r\n                    \"type\": \"Button\",\r\n                    \"id\": \"553920b2-9e8c-496c-9fd6-197e26314f97\",\r\n                    \"expectedControlType\": \"Button\",\r\n                    \"processors\": \"\",\r\n                    \"interactions\": \"\",\r\n                    \"initialStateCheck\": true\r\n                },\r\n                {\r\n                    \"name\": \"Pause\",\r\n                    \"type\": \"Button\",\r\n                    \"id\": \"18bb96b3-fa2b-4b09-827b-6f18c51cc0f5\",\r\n                    \"expectedControlType\": \"Button\",\r\n                    \"processors\": \"\",\r\n                    \"interactions\": \"\",\r\n                    \"initialStateCheck\": false\r\n                },\r\n                {\r\n                    \"name\": \"Bag\",\r\n                    \"type\": \"Button\",\r\n                    \"id\": \"5cb35935-6edb-4043-845e-fe2d1eb718e1\",\r\n                    \"expectedControlType\": \"Button\",\r\n                    \"processors\": \"\",\r\n                    \"interactions\": \"\",\r\n                    \"initialStateCheck\": false\r\n                },\r\n                {\r\n                    \"name\": \"GamepadDirect\",\r\n                    \"type\": \"Value\",\r\n                    \"id\": \"666200a0-f60c-4bd4-bf84-ab5bb86ee9f6\",\r\n                    \"expectedControlType\": \"Vector2\",\r\n                    \"processors\": \"\",\r\n                    \"interactions\": \"\",\r\n                    \"initialStateCheck\": true\r\n                },\r\n                {\r\n                    \"name\": \"GamepadLB\",\r\n                    \"type\": \"Button\",\r\n                    \"id\": \"08f63824-e81c-4c56-8ea4-fd1bb36d58ac\",\r\n                    \"expectedControlType\": \"Button\",\r\n                    \"processors\": \"\",\r\n                    \"interactions\": \"\",\r\n                    \"initialStateCheck\": false\r\n                },\r\n                {\r\n                    \"name\": \"GamepadLT\",\r\n                    \"type\": \"Button\",\r\n                    \"id\": \"5e0456ca-2725-4b38-81ce-b34e826da641\",\r\n                    \"expectedControlType\": \"Button\",\r\n                    \"processors\": \"\",\r\n                    \"interactions\": \"\",\r\n                    \"initialStateCheck\": false\r\n                },\r\n                {\r\n                    \"name\": \"GamepadRT\",\r\n                    \"type\": \"Button\",\r\n                    \"id\": \"9cf78df2-e4de-4bc3-a481-a79d106d97b4\",\r\n                    \"expectedControlType\": \"Button\",\r\n                    \"processors\": \"\",\r\n                    \"interactions\": \"\",\r\n                    \"initialStateCheck\": false\r\n                },\r\n                {\r\n                    \"name\": \"GamepadRB\",\r\n                    \"type\": \"Button\",\r\n                    \"id\": \"d94210eb-8720-47c4-b33b-aff08239c6d5\",\r\n                    \"expectedControlType\": \"Button\",\r\n                    \"processors\": \"\",\r\n                    \"interactions\": \"\",\r\n                    \"initialStateCheck\": false\r\n                },\r\n                {\r\n                    \"name\": \"GamepadEast\",\r\n                    \"type\": \"Button\",\r\n                    \"id\": \"b570b2af-8732-4c3d-bdde-b75af1235e55\",\r\n                    \"expectedControlType\": \"Button\",\r\n                    \"processors\": \"\",\r\n                    \"interactions\": \"\",\r\n                    \"initialStateCheck\": false\r\n                },\r\n                {\r\n                    \"name\": \"GamepadWest\",\r\n                    \"type\": \"Button\",\r\n                    \"id\": \"58d09d9b-29f9-4bc4-9f9e-39ac2b2578c7\",\r\n                    \"expectedControlType\": \"Button\",\r\n                    \"processors\": \"\",\r\n                    \"interactions\": \"\",\r\n                    \"initialStateCheck\": false\r\n                },\r\n                {\r\n                    \"name\": \"GamepadDpad\",\r\n                    \"type\": \"Value\",\r\n                    \"id\": \"e6442f69-61f0-4c6e-908e-443a7d9cd613\",\r\n                    \"expectedControlType\": \"Vector2\",\r\n                    \"processors\": \"\",\r\n                    \"interactions\": \"\",\r\n                    \"initialStateCheck\": true\r\n                },\r\n                {\r\n                    \"name\": \"KeyboardQ\",\r\n                    \"type\": \"Button\",\r\n                    \"id\": \"3089d4e8-0273-4d8a-9ea3-e7bf004682c6\",\r\n                    \"expectedControlType\": \"Button\",\r\n                    \"processors\": \"\",\r\n                    \"interactions\": \"\",\r\n                    \"initialStateCheck\": false\r\n                },\r\n                {\r\n                    \"name\": \"KeyboardE\",\r\n                    \"type\": \"Button\",\r\n                    \"id\": \"93e0f431-9521-44e5-9513-26501086f2af\",\r\n                    \"expectedControlType\": \"Button\",\r\n                    \"processors\": \"\",\r\n                    \"interactions\": \"\",\r\n                    \"initialStateCheck\": false\r\n                },\r\n                {\r\n                    \"name\": \"KeyboardA\",\r\n                    \"type\": \"Button\",\r\n                    \"id\": \"039d7996-c8d8-4580-95dc-2ce16040b08a\",\r\n                    \"expectedControlType\": \"Button\",\r\n                    \"processors\": \"\",\r\n                    \"interactions\": \"\",\r\n                    \"initialStateCheck\": false\r\n                },\r\n                {\r\n                    \"name\": \"KeyboardD\",\r\n                    \"type\": \"Button\",\r\n                    \"id\": \"8c7261f6-88d1-4819-b511-fc2dbe9aafbf\",\r\n                    \"expectedControlType\": \"Button\",\r\n                    \"processors\": \"\",\r\n                    \"interactions\": \"\",\r\n                    \"initialStateCheck\": false\r\n                },\r\n                {\r\n                    \"name\": \"CustomAction\",\r\n                    \"type\": \"Button\",\r\n                    \"id\": \"32648c78-cfa5-4c5c-bd42-c5ce4a284033\",\r\n                    \"expectedControlType\": \"Button\",\r\n                    \"processors\": \"\",\r\n                    \"interactions\": \"\",\r\n                    \"initialStateCheck\": false\r\n                },\r\n                {\r\n                    \"name\": \"Alpha\",\r\n                    \"type\": \"Button\",\r\n                    \"id\": \"70d3c80a-57d7-4bff-a3c8-c178e3fef1f5\",\r\n                    \"expectedControlType\": \"Button\",\r\n                    \"processors\": \"\",\r\n                    \"interactions\": \"\",\r\n                    \"initialStateCheck\": false\r\n                },\r\n                {\r\n                    \"name\": \"PotionUp\",\r\n                    \"type\": \"Button\",\r\n                    \"id\": \"58bb4182-0b90-44f6-b259-0b807cdcf7c9\",\r\n                    \"expectedControlType\": \"Button\",\r\n                    \"processors\": \"\",\r\n                    \"interactions\": \"\",\r\n                    \"initialStateCheck\": false\r\n                },\r\n                {\r\n                    \"name\": \"PotionDown\",\r\n                    \"type\": \"Button\",\r\n                    \"id\": \"78cb1632-9c22-46f8-a64c-9017cd727476\",\r\n                    \"expectedControlType\": \"Button\",\r\n                    \"processors\": \"\",\r\n                    \"interactions\": \"\",\r\n                    \"initialStateCheck\": false\r\n                },\r\n                {\r\n                    \"name\": \"QuickRemove\",\r\n                    \"type\": \"Button\",\r\n                    \"id\": \"47ab1aa4-0af8-4fca-9084-7defbb5cf530\",\r\n                    \"expectedControlType\": \"Button\",\r\n                    \"processors\": \"\",\r\n                    \"interactions\": \"\",\r\n                    \"initialStateCheck\": false\r\n                },\r\n                {\r\n                    \"name\": \"Drop\",\r\n                    \"type\": \"Button\",\r\n                    \"id\": \"ca96bf7a-c537-443a-aada-db64e3a0acd0\",\r\n                    \"expectedControlType\": \"Button\",\r\n                    \"processors\": \"\",\r\n                    \"interactions\": \"\",\r\n                    \"initialStateCheck\": false\r\n                },\r\n                {\r\n                    \"name\": \"AnyTouchScreen\",\r\n                    \"type\": \"Button\",\r\n                    \"id\": \"509a60bb-56c2-4d30-a1ce-3ca1059c7d2b\",\r\n                    \"expectedControlType\": \"Button\",\r\n                    \"processors\": \"\",\r\n                    \"interactions\": \"\",\r\n                    \"initialStateCheck\": false\r\n                },\r\n                {\r\n                    \"name\": \"Sprint\",\r\n                    \"type\": \"Button\",\r\n                    \"id\": \"35e6dc07-d79a-4a24-96ed-e4b3e159289d\",\r\n                    \"expectedControlType\": \"Button\",\r\n                    \"processors\": \"\",\r\n                    \"interactions\": \"\",\r\n                    \"initialStateCheck\": false\r\n                },\r\n                {\r\n                    \"name\": \"QuickPanel\",\r\n                    \"type\": \"Button\",\r\n                    \"id\": \"2c426768-9cf2-4283-aba9-0c8cb5ee2bcb\",\r\n                    \"expectedControlType\": \"Button\",\r\n                    \"processors\": \"\",\r\n                    \"interactions\": \"\",\r\n                    \"initialStateCheck\": false\r\n                },\r\n                {\r\n                    \"name\": \"KillSummon\",\r\n                    \"type\": \"Button\",\r\n                    \"id\": \"1917e26c-4326-4386-a680-a315ccbcdffd\",\r\n                    \"expectedControlType\": \"Button\",\r\n                    \"processors\": \"\",\r\n                    \"interactions\": \"\",\r\n                    \"initialStateCheck\": false\r\n                }\r\n            ],\r\n            \"bindings\": [\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"b0a26270-a5d1-42e6-9700-d268d86df46c\",\r\n                    \"path\": \"<Mouse>/leftButton\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"Shoot\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"f9d2dd1e-56c4-4b8a-95c3-8f0393df8e71\",\r\n                    \"path\": \"<Gamepad>/rightTrigger\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"Shoot\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"3c72eb24-e2a8-41d4-814f-73c70a0bd103\",\r\n                    \"path\": \"<Mouse>/rightButton\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"Drink\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"21be9274-d88d-4066-8c3e-9f5bc45157db\",\r\n                    \"path\": \"<Gamepad>/buttonNorth\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"Drink\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"WASD\",\r\n                    \"id\": \"edb22b93-7f93-4e95-9901-4b74ae1f5ee3\",\r\n                    \"path\": \"2DVector\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"WASD\",\r\n                    \"isComposite\": true,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"up\",\r\n                    \"id\": \"7e3fded2-bff2-4ca4-bbdd-2ad7ad7687c2\",\r\n                    \"path\": \"<Keyboard>/w\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"WASD\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": true\r\n                },\r\n                {\r\n                    \"name\": \"down\",\r\n                    \"id\": \"17c425fc-ce75-40e5-bbe3-2d0c5682d2b0\",\r\n                    \"path\": \"<Keyboard>/s\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"WASD\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": true\r\n                },\r\n                {\r\n                    \"name\": \"left\",\r\n                    \"id\": \"d6415da9-29c5-4662-aea4-1670137c442c\",\r\n                    \"path\": \"<Keyboard>/a\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"WASD\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": true\r\n                },\r\n                {\r\n                    \"name\": \"right\",\r\n                    \"id\": \"e10bdb5c-b9d7-4610-b350-1469dc8ffb07\",\r\n                    \"path\": \"<Keyboard>/d\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"WASD\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": true\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"de518e2d-4473-4a48-b0d7-c70f83bbe756\",\r\n                    \"path\": \"<Keyboard>/e\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"Interact\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"0e26b43d-c9b8-487a-a606-5648a92f32fb\",\r\n                    \"path\": \"<Gamepad>/buttonSouth\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"Interact\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"e7f5bfdf-f8fb-4032-8b52-f2de6567b12c\",\r\n                    \"path\": \"<Keyboard>/1\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"Alpha1\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"bc021a40-fa9e-4319-8111-a1e6d36ada1c\",\r\n                    \"path\": \"<Keyboard>/2\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"Alpha2\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"fabfb103-dab5-40ea-8e3b-d86a7778d015\",\r\n                    \"path\": \"<Keyboard>/3\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"Alpha3\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"b1adc4fb-5f4c-47e9-adf1-c1b541767466\",\r\n                    \"path\": \"<Keyboard>/4\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"Alpha4\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"e9d583ed-5c4a-45f2-b165-dea65f5f5897\",\r\n                    \"path\": \"<Keyboard>/5\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"Alpha5\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"3e6075b3-daf7-402d-9c94-08f86f31f9b4\",\r\n                    \"path\": \"<Keyboard>/6\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"Alpha6\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"11bd8412-510a-435d-90df-f1ba74a1d5a8\",\r\n                    \"path\": \"<Mouse>/scroll/up\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"AxisDeadzone\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"WandUp\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"d371fd6e-0f56-4af9-93f2-7e4a198bff70\",\r\n                    \"path\": \"<Mouse>/scroll/down\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"AxisDeadzone\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"WandDown\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"90d91e9d-d33a-46a7-b54e-ba5e160b231d\",\r\n                    \"path\": \"<Gamepad>/rightShoulder\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"WandDown\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"1cef63a4-2841-4985-873a-9db2c8901154\",\r\n                    \"path\": \"<Keyboard>/escape\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"Pause\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"efb5bb31-96d6-439b-8ed4-7bfcd1bb911a\",\r\n                    \"path\": \"<Gamepad>/start\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"Pause\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"e0ff3b98-91e1-44c2-9a82-e9c125bd1cfd\",\r\n                    \"path\": \"<Keyboard>/tab\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"Bag\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"8bd1a23f-308c-4d7f-927c-1e8befe6203b\",\r\n                    \"path\": \"<Gamepad>/leftTrigger\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"Bag\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"571a0c63-f7f0-416f-8a79-9aa1a73b608b\",\r\n                    \"path\": \"<Mouse>/backButton\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"AnyMouseKeyboardInput\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"fd483f97-bfaf-4b88-a396-fcb254593e45\",\r\n                    \"path\": \"<Mouse>/delta\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"AnyMouseKeyboardInput\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"a6f8f5a5-225c-4bbe-8202-bfe0ea8518bf\",\r\n                    \"path\": \"<Mouse>/forwardButton\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"AnyMouseKeyboardInput\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"78132c8a-92a0-44e9-a12b-f9f57c05e98d\",\r\n                    \"path\": \"<Mouse>/leftButton\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"AnyMouseKeyboardInput\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"bc60c671-6f42-402f-b838-8399c22dfd0f\",\r\n                    \"path\": \"<Mouse>/middleButton\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"AnyMouseKeyboardInput\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"72386bde-0cdd-4560-a4c2-0a7d58bc07f7\",\r\n                    \"path\": \"<Mouse>/pointerId\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"AnyMouseKeyboardInput\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"8e0c8ef1-f9be-42b3-934d-a65dcebb5e4a\",\r\n                    \"path\": \"<Mouse>/position\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"AnyMouseKeyboardInput\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"f82615e4-4f66-487d-be67-78b8ff6fa58b\",\r\n                    \"path\": \"<Mouse>/press\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"AnyMouseKeyboardInput\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"c638b1c0-0f64-4aad-9d22-0bb355499248\",\r\n                    \"path\": \"<Mouse>/pressure\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"AnyMouseKeyboardInput\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"f8366b07-3168-4758-acad-dce66730b8c0\",\r\n                    \"path\": \"<Mouse>/radius\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"AnyMouseKeyboardInput\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"a3d4399c-d67e-438f-ab3d-76c055913433\",\r\n                    \"path\": \"<Mouse>/rightButton\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"AnyMouseKeyboardInput\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"b7865deb-602e-4da9-b3a2-a9a2c0cdffa8\",\r\n                    \"path\": \"<Mouse>/scroll\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"AnyMouseKeyboardInput\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"a12ccee2-75d5-4b9c-9196-13ff0fb467a4\",\r\n                    \"path\": \"<Gamepad>/buttonEast\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"AnyGamepadInput\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"d8e23f08-bdb8-4a2a-bf57-61f9acaa9e9f\",\r\n                    \"path\": \"<Gamepad>/buttonNorth\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"AnyGamepadInput\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"171807d3-0961-4b26-bce1-584197cbe819\",\r\n                    \"path\": \"<Gamepad>/buttonSouth\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"AnyGamepadInput\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"a7a2da8f-cde4-40d0-8af2-bf8d1daedecc\",\r\n                    \"path\": \"<Gamepad>/buttonWest\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"AnyGamepadInput\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"8a27f49b-48f4-4aea-ab94-b1c0f9b4778f\",\r\n                    \"path\": \"<Gamepad>/dpad\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"AnyGamepadInput\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"d5a7299f-59d9-4079-9961-c9e4e1ce6581\",\r\n                    \"path\": \"<Gamepad>/leftShoulder\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"AnyGamepadInput\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"2d83ea7f-6bd5-42f3-8729-c2afa200d1e6\",\r\n                    \"path\": \"<Gamepad>/leftStick\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"AnyGamepadInput\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"9d1cf0b9-c67f-4abf-afc3-17faf3abe880\",\r\n                    \"path\": \"<Gamepad>/leftStickPress\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"AnyGamepadInput\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"ca349018-ea72-4200-87f7-1f750be4ea01\",\r\n                    \"path\": \"<Gamepad>/leftTrigger\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"AnyGamepadInput\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"5befbd63-1bf8-4ae7-8438-35229727af0f\",\r\n                    \"path\": \"<Gamepad>/rightShoulder\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"AnyGamepadInput\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"d3fc149f-c313-440a-99fc-50e343534045\",\r\n                    \"path\": \"<Gamepad>/rightStick\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"AnyGamepadInput\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"7fcb9e1d-dd09-4b16-a6b4-dd48c07fc456\",\r\n                    \"path\": \"<Gamepad>/rightStickPress\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"AnyGamepadInput\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"7849d2a3-8a16-41e1-acc8-5964615e1e1e\",\r\n                    \"path\": \"<Gamepad>/rightTrigger\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"AnyGamepadInput\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"d003d36a-d9ef-4206-8d9b-286a4a53f60b\",\r\n                    \"path\": \"<Gamepad>/select\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"AnyGamepadInput\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"662b9d29-9214-48cd-98a7-faa6ed2d5d99\",\r\n                    \"path\": \"<Gamepad>/start\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"AnyGamepadInput\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"Dpad\",\r\n                    \"id\": \"a2c8fc25-196f-427d-a554-55ab3f0afd54\",\r\n                    \"path\": \"2DVector\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"GamepadDirect\",\r\n                    \"isComposite\": true,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"up\",\r\n                    \"id\": \"d89b8123-af14-49b2-9587-9041df42bfac\",\r\n                    \"path\": \"<Gamepad>/dpad/up\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"GamepadDirect\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": true\r\n                },\r\n                {\r\n                    \"name\": \"down\",\r\n                    \"id\": \"fedeb537-9108-492d-aa88-f09a2a5c944e\",\r\n                    \"path\": \"<Gamepad>/dpad/down\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"GamepadDirect\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": true\r\n                },\r\n                {\r\n                    \"name\": \"left\",\r\n                    \"id\": \"066310c8-7fc0-4dba-b5fe-0d50bf01787a\",\r\n                    \"path\": \"<Gamepad>/dpad/left\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"GamepadDirect\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": true\r\n                },\r\n                {\r\n                    \"name\": \"right\",\r\n                    \"id\": \"65d67acc-1778-4231-b086-5bb75f61637e\",\r\n                    \"path\": \"<Gamepad>/dpad/right\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"GamepadDirect\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": true\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"d53d71ed-e30c-471d-a030-6c775c9bb113\",\r\n                    \"path\": \"<Gamepad>/leftShoulder\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"GamepadLB\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"7aa2960e-c9ad-4051-8b1f-a97d32dfec37\",\r\n                    \"path\": \"<Gamepad>/rightShoulder\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"GamepadRB\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"865d62da-de19-4218-91ec-233eac7299cc\",\r\n                    \"path\": \"<Gamepad>/buttonEast\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"GamepadEast\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"5800ff80-3176-45bb-bd1e-13a86b7f9ec5\",\r\n                    \"path\": \"<Keyboard>/space\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"Space\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"06f54b22-0f92-4127-8fd6-1223450c89cc\",\r\n                    \"path\": \"<Gamepad>/leftStick\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"LeftStick\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"0e283396-86ad-4680-8a54-1e1b73c80d23\",\r\n                    \"path\": \"<Gamepad>/rightStick\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"RightStick\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"6f5fcf55-e99f-48c6-bc49-e826d1545a61\",\r\n                    \"path\": \"<Gamepad>/buttonWest\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"GamepadWest\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"cef76848-6716-4692-b95f-6b2f433ae8ca\",\r\n                    \"path\": \"<Gamepad>/dpad\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"GamepadDpad\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"7347fd6d-8fea-4a19-99fa-68cc1cb2497e\",\r\n                    \"path\": \"<Keyboard>/q\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"KeyboardQ\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"c1139a44-d00a-4977-800b-df222ef0a695\",\r\n                    \"path\": \"<Keyboard>/anyKey\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"CustomAction\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"0711b944-2afa-4519-ba84-82e6bb0cba12\",\r\n                    \"path\": \"<Keyboard>/1\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"Alpha\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"2c78c318-252c-40bd-a820-a93468c9e42a\",\r\n                    \"path\": \"<Keyboard>/2\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"Alpha\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"a0bafa49-87fb-4ed3-a53a-a6f2c64e5e81\",\r\n                    \"path\": \"<Keyboard>/3\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"Alpha\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"4003e373-5bea-4d64-b2e3-571548e1c25a\",\r\n                    \"path\": \"<Keyboard>/4\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"Alpha\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"35149aca-d70d-4c8a-89cd-55e3692f303c\",\r\n                    \"path\": \"<Keyboard>/5\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"Alpha\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"99784abe-80a6-4579-8ebe-ef59ae2b9990\",\r\n                    \"path\": \"<Keyboard>/6\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"Alpha\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"809ee134-218c-4321-8d5c-b47866960aaa\",\r\n                    \"path\": \"<Keyboard>/z\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"PotionUp\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"8a473b10-ff75-488f-b3c7-029b9d248f1b\",\r\n                    \"path\": \"<Keyboard>/x\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"PotionDown\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"15bea574-a726-4a96-9e89-93d24439e84c\",\r\n                    \"path\": \"<Mouse>/rightButton\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"QuickRemove\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"45a5a455-5ea3-4e9c-9da3-c55dd5496375\",\r\n                    \"path\": \"\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"Drop\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"4773c639-2d30-4716-9275-87443a8f7592\",\r\n                    \"path\": \"<Gamepad>/leftShoulder\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"WandUp\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"fef8715a-78bc-43ef-b4a9-ec36914e9bbb\",\r\n                    \"path\": \"<Keyboard>/e\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"KeyboardE\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"055d349b-77df-4e5b-b7d7-b5ba03be1d68\",\r\n                    \"path\": \"<Keyboard>/anyKey\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"AnyKey\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"9966c262-5715-4ecb-8a6d-7722e8a5f74a\",\r\n                    \"path\": \"<Touchscreen>/touch0/press\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"AnyTouchScreen\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"8d49b6ee-ad39-4663-9e7f-78321bd29f65\",\r\n                    \"path\": \"<Keyboard>/space\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"Sprint\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"aa803601-4197-4979-9214-f050be7c2668\",\r\n                    \"path\": \"<Gamepad>/rightStickPress\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"Sprint\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"9a676143-98fb-4797-904b-5cdca73af611\",\r\n                    \"path\": \"<Keyboard>/7\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"Alpha7\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"a1f94b75-c26f-4832-b3c7-ad2f6375dd33\",\r\n                    \"path\": \"<Gamepad>/leftTrigger\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"GamepadLT\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"2b652d5d-c2b4-4c0a-b9fe-e545cf6dcea8\",\r\n                    \"path\": \"<Gamepad>/rightTrigger\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"GamepadRT\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"1d2de8c6-e4e1-4317-a363-8d97f12214e8\",\r\n                    \"path\": \"<Keyboard>/a\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"KeyboardA\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"aa9b58eb-48a1-47fb-84b8-23440cd5976f\",\r\n                    \"path\": \"<Keyboard>/d\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"KeyboardD\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"13dbc6fb-271b-4fc0-84ad-595335ce94f1\",\r\n                    \"path\": \"<Keyboard>/q\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"QuickPanel\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"9e7fb941-0b8f-44fd-b2b3-c4b4211585df\",\r\n                    \"path\": \"<Gamepad>/select\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"QuickPanel\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"b6a25439-aae8-4233-9f05-f7419c78bfc4\",\r\n                    \"path\": \"<Keyboard>/v\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"KillSummon\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"7261d69a-a71b-4bc4-929e-dc88bd4971cf\",\r\n                    \"path\": \"<Gamepad>/leftStickPress\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"KillSummon\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                }\r\n            ]\r\n        }\r\n    ],\r\n    \"controlSchemes\": []\r\n}");
		m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
		m_Player_AnyKey = m_Player.FindAction("AnyKey", throwIfNotFound: true);
		m_Player_AnyMouseKeyboardInput = m_Player.FindAction("AnyMouseKeyboardInput", throwIfNotFound: true);
		m_Player_AnyGamepadInput = m_Player.FindAction("AnyGamepadInput", throwIfNotFound: true);
		m_Player_Space = m_Player.FindAction("Space", throwIfNotFound: true);
		m_Player_WASD = m_Player.FindAction("WASD", throwIfNotFound: true);
		m_Player_LeftStick = m_Player.FindAction("LeftStick", throwIfNotFound: true);
		m_Player_RightStick = m_Player.FindAction("RightStick", throwIfNotFound: true);
		m_Player_Shoot = m_Player.FindAction("Shoot", throwIfNotFound: true);
		m_Player_Drink = m_Player.FindAction("Drink", throwIfNotFound: true);
		m_Player_Interact = m_Player.FindAction("Interact", throwIfNotFound: true);
		m_Player_Alpha1 = m_Player.FindAction("Alpha1", throwIfNotFound: true);
		m_Player_Alpha2 = m_Player.FindAction("Alpha2", throwIfNotFound: true);
		m_Player_Alpha3 = m_Player.FindAction("Alpha3", throwIfNotFound: true);
		m_Player_Alpha4 = m_Player.FindAction("Alpha4", throwIfNotFound: true);
		m_Player_Alpha5 = m_Player.FindAction("Alpha5", throwIfNotFound: true);
		m_Player_Alpha6 = m_Player.FindAction("Alpha6", throwIfNotFound: true);
		m_Player_Alpha7 = m_Player.FindAction("Alpha7", throwIfNotFound: true);
		m_Player_WandUp = m_Player.FindAction("WandUp", throwIfNotFound: true);
		m_Player_WandDown = m_Player.FindAction("WandDown", throwIfNotFound: true);
		m_Player_Pause = m_Player.FindAction("Pause", throwIfNotFound: true);
		m_Player_Bag = m_Player.FindAction("Bag", throwIfNotFound: true);
		m_Player_GamepadDirect = m_Player.FindAction("GamepadDirect", throwIfNotFound: true);
		m_Player_GamepadLB = m_Player.FindAction("GamepadLB", throwIfNotFound: true);
		m_Player_GamepadLT = m_Player.FindAction("GamepadLT", throwIfNotFound: true);
		m_Player_GamepadRT = m_Player.FindAction("GamepadRT", throwIfNotFound: true);
		m_Player_GamepadRB = m_Player.FindAction("GamepadRB", throwIfNotFound: true);
		m_Player_GamepadEast = m_Player.FindAction("GamepadEast", throwIfNotFound: true);
		m_Player_GamepadWest = m_Player.FindAction("GamepadWest", throwIfNotFound: true);
		m_Player_GamepadDpad = m_Player.FindAction("GamepadDpad", throwIfNotFound: true);
		m_Player_KeyboardQ = m_Player.FindAction("KeyboardQ", throwIfNotFound: true);
		m_Player_KeyboardE = m_Player.FindAction("KeyboardE", throwIfNotFound: true);
		m_Player_KeyboardA = m_Player.FindAction("KeyboardA", throwIfNotFound: true);
		m_Player_KeyboardD = m_Player.FindAction("KeyboardD", throwIfNotFound: true);
		m_Player_CustomAction = m_Player.FindAction("CustomAction", throwIfNotFound: true);
		m_Player_Alpha = m_Player.FindAction("Alpha", throwIfNotFound: true);
		m_Player_PotionUp = m_Player.FindAction("PotionUp", throwIfNotFound: true);
		m_Player_PotionDown = m_Player.FindAction("PotionDown", throwIfNotFound: true);
		m_Player_QuickRemove = m_Player.FindAction("QuickRemove", throwIfNotFound: true);
		m_Player_Drop = m_Player.FindAction("Drop", throwIfNotFound: true);
		m_Player_AnyTouchScreen = m_Player.FindAction("AnyTouchScreen", throwIfNotFound: true);
		m_Player_Sprint = m_Player.FindAction("Sprint", throwIfNotFound: true);
		m_Player_QuickPanel = m_Player.FindAction("QuickPanel", throwIfNotFound: true);
		m_Player_KillSummon = m_Player.FindAction("KillSummon", throwIfNotFound: true);
	}

	~InputActions()
	{
	}

	public void Dispose()
	{
		UnityEngine.Object.Destroy(asset);
	}

	public bool Contains(InputAction action)
	{
		return asset.Contains(action);
	}

	public IEnumerator<InputAction> GetEnumerator()
	{
		return asset.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	public void Enable()
	{
		asset.Enable();
	}

	public void Disable()
	{
		asset.Disable();
	}

	public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
	{
		return asset.FindAction(actionNameOrId, throwIfNotFound);
	}

	public int FindBinding(InputBinding bindingMask, out InputAction action)
	{
		return asset.FindBinding(bindingMask, out action);
	}
}
