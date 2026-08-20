using System;
using Steamworks;
using UnityEngine;

public class SteamInputTest : MonoBehaviour
{
	private enum EActionSets
	{
		InGameControls,
		MenuControls
	}

	private enum EAnalogActions_InGameControls
	{
		Move,
		Camera,
		Throttle
	}

	private enum EDigitalActions_InGameControls
	{
		fire,
		Jump,
		pause_menu
	}

	private enum EDigitalActions_MenuControls
	{
		menu_up,
		menu_down,
		menu_left,
		menu_right,
		menu_select,
		menu_cancel,
		pause_menu
	}

	private Vector2 m_ScrollPos;

	private bool m_InputInitialized;

	private int m_nInputs;

	protected Callback<SteamInputDeviceConnected_t> m_SteamInputDeviceConnected;

	protected Callback<SteamInputDeviceDisconnected_t> m_SteamInputDeviceDisconnected;

	protected Callback<SteamInputConfigurationLoaded_t> m_SteamInputConfigurationLoaded;

	protected Callback<SteamInputGamepadSlotChange_t> m_SteamInputGamepadSlotChange;

	private int m_nActionSets;

	private int m_nInGameControlsAnalogActions;

	private int m_nInGameControlsDigitalActions;

	private int m_nMenuControlsDigitalActions;

	private string[] m_ActionSetNames;

	private string[] m_InGameControlsAnalogActionNames;

	private string[] m_InGameControlsDigitalActionNames;

	private string[] m_MenuControlsDigitalActionNames;

	private InputActionSetHandle_t[] m_ActionSets;

	private InputAnalogActionHandle_t[] m_InGameControlsAnalogActions;

	private InputDigitalActionHandle_t[] m_InGameControlsDigitalActions;

	private InputDigitalActionHandle_t[] m_MenuControlsDigitalActions;

	private InputHandle_t[] m_InputHandles;

	public void OnEnable()
	{
		m_InputInitialized = SteamInput.Init(bExplicitlyCallRunFrame: false);
		MonoBehaviour.print("SteamInput.Init() - " + m_InputInitialized);
		m_InputHandles = new InputHandle_t[16];
		if (m_InputInitialized)
		{
			SteamInput.EnableDeviceCallbacks();
			Precache();
		}
		m_SteamInputDeviceConnected = Callback<SteamInputDeviceConnected_t>.Create(OnSteamInputDeviceConnected);
		m_SteamInputDeviceDisconnected = Callback<SteamInputDeviceDisconnected_t>.Create(OnSteamInputDeviceDisconnected);
		m_SteamInputConfigurationLoaded = Callback<SteamInputConfigurationLoaded_t>.Create(OnSteamInputConfigurationLoaded);
		m_SteamInputGamepadSlotChange = Callback<SteamInputGamepadSlotChange_t>.Create(OnSteamInputGamepadSlotChange);
	}

	private void OnDisable()
	{
		m_InputInitialized = false;
		MonoBehaviour.print("SteamInput.Shutdown() - " + SteamInput.Shutdown());
	}

	private void Precache()
	{
		m_ActionSetNames = Enum.GetNames(typeof(EActionSets));
		m_nActionSets = m_ActionSetNames.Length;
		m_ActionSets = new InputActionSetHandle_t[m_nActionSets];
		for (int i = 0; i < m_nActionSets; i++)
		{
			m_ActionSets[i] = SteamInput.GetActionSetHandle(m_ActionSetNames[i]);
			string obj = m_ActionSetNames[i];
			InputActionSetHandle_t inputActionSetHandle_t = m_ActionSets[i];
			MonoBehaviour.print("SteamInput.GetActionSetHandle(" + obj + ") - " + inputActionSetHandle_t.ToString());
		}
		m_InGameControlsAnalogActionNames = Enum.GetNames(typeof(EAnalogActions_InGameControls));
		m_nInGameControlsAnalogActions = m_InGameControlsAnalogActionNames.Length;
		m_InGameControlsAnalogActions = new InputAnalogActionHandle_t[m_nInGameControlsAnalogActions];
		for (int j = 0; j < m_nInGameControlsAnalogActions; j++)
		{
			m_InGameControlsAnalogActions[j] = SteamInput.GetAnalogActionHandle(m_InGameControlsAnalogActionNames[j]);
			string obj2 = m_InGameControlsAnalogActionNames[j];
			InputAnalogActionHandle_t inputAnalogActionHandle_t = m_InGameControlsAnalogActions[j];
			MonoBehaviour.print("SteamInput.GetAnalogActionHandle(" + obj2 + ") - " + inputAnalogActionHandle_t.ToString());
		}
		m_InGameControlsDigitalActionNames = Enum.GetNames(typeof(EDigitalActions_InGameControls));
		m_nInGameControlsDigitalActions = m_InGameControlsDigitalActionNames.Length;
		m_InGameControlsDigitalActions = new InputDigitalActionHandle_t[m_nInGameControlsDigitalActions];
		for (int k = 0; k < m_nInGameControlsDigitalActions; k++)
		{
			m_InGameControlsDigitalActions[k] = SteamInput.GetDigitalActionHandle(m_InGameControlsDigitalActionNames[k]);
			string obj3 = m_InGameControlsDigitalActionNames[k];
			InputDigitalActionHandle_t inputDigitalActionHandle_t = m_InGameControlsDigitalActions[k];
			MonoBehaviour.print("SteamInput.GetDigitalActionHandle(" + obj3 + ") - " + inputDigitalActionHandle_t.ToString());
		}
		m_MenuControlsDigitalActionNames = Enum.GetNames(typeof(EDigitalActions_MenuControls));
		m_nMenuControlsDigitalActions = m_MenuControlsDigitalActionNames.Length;
		m_MenuControlsDigitalActions = new InputDigitalActionHandle_t[m_nMenuControlsDigitalActions];
		for (int l = 0; l < m_nMenuControlsDigitalActions; l++)
		{
			m_MenuControlsDigitalActions[l] = SteamInput.GetDigitalActionHandle(m_MenuControlsDigitalActionNames[l]);
			string obj4 = m_MenuControlsDigitalActionNames[l];
			InputDigitalActionHandle_t inputDigitalActionHandle_t = m_MenuControlsDigitalActions[l];
			MonoBehaviour.print("SteamInput.GetDigitalActionHandle(" + obj4 + ") - " + inputDigitalActionHandle_t.ToString());
		}
	}

	public void RenderOnGUI()
	{
		GUILayout.BeginArea(new Rect(Screen.width - 200, 0f, 200f, Screen.height));
		GUILayout.Label("Variables:");
		GUILayout.Label("m_InputInitialized: " + m_InputInitialized);
		GUILayout.Label("m_nInputs: " + m_nInputs);
		GUILayout.EndArea();
		GUILayout.BeginVertical("box");
		m_ScrollPos = GUILayout.BeginScrollView(m_ScrollPos, GUILayout.Width(Screen.width - 215), GUILayout.Height(Screen.height - 33));
		if (!m_InputInitialized)
		{
			return;
		}
		if (GUILayout.Button("SetInputActionManifestFilePath(\"\")"))
		{
			MonoBehaviour.print("SteamInput.SetInputActionManifestFilePath(\"\") : " + SteamInput.SetInputActionManifestFilePath(""));
		}
		GUILayout.Label("BNewDataAvailable() : " + SteamInput.BNewDataAvailable());
		m_nInputs = SteamInput.GetConnectedControllers(m_InputHandles);
		GUILayout.Label("GetConnectedControllers(m_InputHandles) : " + m_nInputs);
		for (int i = 0; i < m_nInputs; i++)
		{
			string text = i.ToString();
			InputHandle_t inputHandle_t = m_InputHandles[i];
			GUILayout.Label("Input " + text + " - " + inputHandle_t.ToString());
			for (int j = 0; j < m_nActionSets; j++)
			{
				if (GUILayout.Button("ActivateActionSet(m_InputHandles[i], m_ActionSets[j])"))
				{
					SteamInput.ActivateActionSet(m_InputHandles[i], m_ActionSets[j]);
					string[] obj = new string[5] { "SteamInput.ActivateActionSet(", null, null, null, null };
					inputHandle_t = m_InputHandles[i];
					obj[1] = inputHandle_t.ToString();
					obj[2] = ", ";
					InputActionSetHandle_t inputActionSetHandle_t = m_ActionSets[j];
					obj[3] = inputActionSetHandle_t.ToString();
					obj[4] = ")";
					MonoBehaviour.print(string.Concat(obj));
				}
			}
			GUILayout.Label("GetCurrentActionSet(m_InputHandles[i]) : " + SteamInput.GetCurrentActionSet(m_InputHandles[i]).ToString());
			GUILayout.Label("InGameControls Digital Actions:");
			for (int k = 0; k < m_nInGameControlsDigitalActions; k++)
			{
				InputDigitalActionData_t digitalActionData = SteamInput.GetDigitalActionData(m_InputHandles[i], m_InGameControlsDigitalActions[k]);
				string[] obj2 = new string[10] { "GetDigitalActionData(", null, null, null, null, null, null, null, null, null };
				inputHandle_t = m_InputHandles[i];
				obj2[1] = inputHandle_t.ToString();
				obj2[2] = ", ";
				InputDigitalActionHandle_t inputDigitalActionHandle_t = m_InGameControlsDigitalActions[k];
				obj2[3] = inputDigitalActionHandle_t.ToString();
				obj2[4] = ") - ";
				obj2[5] = digitalActionData.bState.ToString();
				obj2[6] = " -- ";
				obj2[7] = digitalActionData.bActive.ToString();
				obj2[8] = " -- ";
				obj2[9] = m_InGameControlsDigitalActionNames[k];
				GUILayout.Label(string.Concat(obj2));
			}
			GUILayout.Label("MenuControls Digital Actions:");
			for (int l = 0; l < m_nMenuControlsDigitalActions; l++)
			{
				InputDigitalActionData_t digitalActionData2 = SteamInput.GetDigitalActionData(m_InputHandles[i], m_MenuControlsDigitalActions[l]);
				string[] obj3 = new string[10] { "GetDigitalActionData(", null, null, null, null, null, null, null, null, null };
				inputHandle_t = m_InputHandles[i];
				obj3[1] = inputHandle_t.ToString();
				obj3[2] = ", ";
				InputDigitalActionHandle_t inputDigitalActionHandle_t = m_MenuControlsDigitalActions[l];
				obj3[3] = inputDigitalActionHandle_t.ToString();
				obj3[4] = ") - ";
				obj3[5] = digitalActionData2.bState.ToString();
				obj3[6] = " -- ";
				obj3[7] = digitalActionData2.bActive.ToString();
				obj3[8] = " -- ";
				obj3[9] = m_MenuControlsDigitalActionNames[l];
				GUILayout.Label(string.Concat(obj3));
			}
			if (GUILayout.Button("GetDigitalActionOrigins(m_InputHandles[i], m_ActionSets[(int)EActionSets.InGameControls], m_InGameControlsDigitalActions[(int)EDigitalActions_InGameControls.fire], origins)"))
			{
				EInputActionOrigin[] array = new EInputActionOrigin[8];
				int digitalActionOrigins = SteamInput.GetDigitalActionOrigins(m_InputHandles[i], m_ActionSets[0], m_InGameControlsDigitalActions[0], array);
				string[] obj4 = new string[10] { "SteamInput.GetDigitalActionOrigins(", null, null, null, null, null, null, null, null, null };
				inputHandle_t = m_InputHandles[i];
				obj4[1] = inputHandle_t.ToString();
				obj4[2] = ", ";
				InputActionSetHandle_t inputActionSetHandle_t = m_ActionSets[0];
				obj4[3] = inputActionSetHandle_t.ToString();
				obj4[4] = ", ";
				InputDigitalActionHandle_t inputDigitalActionHandle_t = m_InGameControlsDigitalActions[0];
				obj4[5] = inputDigitalActionHandle_t.ToString();
				obj4[6] = ", ";
				obj4[7] = array?.ToString();
				obj4[8] = ") : ";
				obj4[9] = digitalActionOrigins.ToString();
				MonoBehaviour.print(string.Concat(obj4));
				MonoBehaviour.print(digitalActionOrigins + " origins for: " + m_ActionSetNames[0] + "::" + m_InGameControlsDigitalActionNames[0]);
				for (int m = 0; m < digitalActionOrigins; m++)
				{
					MonoBehaviour.print(m + ": " + array[m]);
				}
			}
			if (GUILayout.Button("GetStringForDigitalActionName(m_InGameControlsDigitalActions[(int)EDigitalActions_InGameControls.fire])"))
			{
				string stringForDigitalActionName = SteamInput.GetStringForDigitalActionName(m_InGameControlsDigitalActions[0]);
				InputDigitalActionHandle_t inputDigitalActionHandle_t = m_InGameControlsDigitalActions[0];
				MonoBehaviour.print("SteamInput.GetStringForDigitalActionName(" + inputDigitalActionHandle_t.ToString() + ") : " + stringForDigitalActionName);
			}
			GUILayout.Label("InGameControls Analog Actions:");
			for (int n = 0; n < m_nInGameControlsAnalogActions; n++)
			{
				GUILayout.Label("GetAnalogActionData(m_InputHandles[i], m_InGameControlsAnalogActions[j]) : " + SteamInput.GetAnalogActionData(m_InputHandles[i], m_InGameControlsAnalogActions[n]));
			}
			if (GUILayout.Button("GetAnalogActionOrigins(m_InputHandles[i], m_ActionSets[(int)EActionSets.InGameControls], m_InGameControlsAnalogActions[(int)EAnalogActions_InGameControls.Throttle], origins)"))
			{
				EInputActionOrigin[] array2 = new EInputActionOrigin[8];
				int analogActionOrigins = SteamInput.GetAnalogActionOrigins(m_InputHandles[i], m_ActionSets[0], m_InGameControlsAnalogActions[2], array2);
				string[] obj5 = new string[10] { "SteamInput.GetAnalogActionOrigins(", null, null, null, null, null, null, null, null, null };
				inputHandle_t = m_InputHandles[i];
				obj5[1] = inputHandle_t.ToString();
				obj5[2] = ", ";
				InputActionSetHandle_t inputActionSetHandle_t = m_ActionSets[0];
				obj5[3] = inputActionSetHandle_t.ToString();
				obj5[4] = ", ";
				InputAnalogActionHandle_t inputAnalogActionHandle_t = m_InGameControlsAnalogActions[2];
				obj5[5] = inputAnalogActionHandle_t.ToString();
				obj5[6] = ", ";
				obj5[7] = array2?.ToString();
				obj5[8] = ") : ";
				obj5[9] = analogActionOrigins.ToString();
				MonoBehaviour.print(string.Concat(obj5));
				MonoBehaviour.print(analogActionOrigins + " origins for: " + m_ActionSetNames[0] + "::" + m_InGameControlsAnalogActionNames[2]);
				for (int num = 0; num < analogActionOrigins; num++)
				{
					MonoBehaviour.print(num + ": " + array2[num]);
				}
			}
			if (GUILayout.Button("GetGlyphPNGForActionOrigin(EInputActionOrigin.k_EInputActionOrigin_XBoxOne_A, ESteamInputGlyphSize.k_ESteamInputGlyphSize_Small, 0)"))
			{
				string glyphPNGForActionOrigin = SteamInput.GetGlyphPNGForActionOrigin(EInputActionOrigin.k_EInputActionOrigin_XBoxOne_A, ESteamInputGlyphSize.k_ESteamInputGlyphSize_Small, 0u);
				MonoBehaviour.print("SteamInput.GetGlyphPNGForActionOrigin(" + EInputActionOrigin.k_EInputActionOrigin_XBoxOne_A.ToString() + ", " + ESteamInputGlyphSize.k_ESteamInputGlyphSize_Small.ToString() + ", " + 0 + ") : " + glyphPNGForActionOrigin);
			}
			if (GUILayout.Button("GetGlyphSVGForActionOrigin(EInputActionOrigin.k_EInputActionOrigin_XBoxOne_A, 0)"))
			{
				string glyphSVGForActionOrigin = SteamInput.GetGlyphSVGForActionOrigin(EInputActionOrigin.k_EInputActionOrigin_XBoxOne_A, 0u);
				MonoBehaviour.print("SteamInput.GetGlyphSVGForActionOrigin(" + EInputActionOrigin.k_EInputActionOrigin_XBoxOne_A.ToString() + ", " + 0 + ") : " + glyphSVGForActionOrigin);
			}
			if (GUILayout.Button("GetGlyphForActionOrigin_Legacy(EInputActionOrigin.k_EInputActionOrigin_XBoxOne_A)"))
			{
				string glyphForActionOrigin_Legacy = SteamInput.GetGlyphForActionOrigin_Legacy(EInputActionOrigin.k_EInputActionOrigin_XBoxOne_A);
				MonoBehaviour.print("SteamInput.GetGlyphForActionOrigin_Legacy(" + EInputActionOrigin.k_EInputActionOrigin_XBoxOne_A.ToString() + ") : " + glyphForActionOrigin_Legacy);
			}
			GUILayout.Label("GetStringForActionOrigin(EInputActionOrigin.k_EInputActionOrigin_XBoxOne_A) : " + SteamInput.GetStringForActionOrigin(EInputActionOrigin.k_EInputActionOrigin_XBoxOne_A));
			GUILayout.Label("GetStringForAnalogActionName(m_InGameControlsAnalogActions[(int)EAnalogActions_InGameControls.Throttle]) : " + SteamInput.GetStringForAnalogActionName(m_InGameControlsAnalogActions[2]));
			GUILayout.Label("InGameControls Analog Actions:");
			for (int num2 = 0; num2 < m_nInGameControlsAnalogActions; num2++)
			{
				if (GUILayout.Button("StopAnalogActionMomentum(m_InputHandles[i], m_InGameControlsAnalogActions[j])"))
				{
					SteamInput.StopAnalogActionMomentum(m_InputHandles[i], m_InGameControlsAnalogActions[num2]);
					string[] obj6 = new string[5] { "SteamInput.StopAnalogActionMomentum(", null, null, null, null };
					inputHandle_t = m_InputHandles[i];
					obj6[1] = inputHandle_t.ToString();
					obj6[2] = ", ";
					InputAnalogActionHandle_t inputAnalogActionHandle_t = m_InGameControlsAnalogActions[num2];
					obj6[3] = inputAnalogActionHandle_t.ToString();
					obj6[4] = ")";
					MonoBehaviour.print(string.Concat(obj6));
				}
			}
			if (GUILayout.Button("GetMotionData(m_InputHandles[i])"))
			{
				InputMotionData_t motionData = SteamInput.GetMotionData(m_InputHandles[i]);
				inputHandle_t = m_InputHandles[i];
				MonoBehaviour.print("SteamInput.GetMotionData(" + inputHandle_t.ToString() + ") : " + motionData);
			}
			if (GUILayout.Button("TriggerVibration(m_InputHandles[i], ushort.MaxValue, ushort.MaxValue)"))
			{
				SteamInput.TriggerVibration(m_InputHandles[i], ushort.MaxValue, ushort.MaxValue);
				string[] obj7 = new string[7] { "SteamInput.TriggerVibration(", null, null, null, null, null, null };
				inputHandle_t = m_InputHandles[i];
				obj7[1] = inputHandle_t.ToString();
				obj7[2] = ", ";
				obj7[3] = ushort.MaxValue.ToString();
				obj7[4] = ", ";
				obj7[5] = ushort.MaxValue.ToString();
				obj7[6] = ")";
				MonoBehaviour.print(string.Concat(obj7));
			}
			if (GUILayout.Button("TriggerVibrationExtended(m_InputHandles[i], ushort.MaxValue, ushort.MaxValue, ushort.MaxValue, ushort.MaxValue)"))
			{
				SteamInput.TriggerVibrationExtended(m_InputHandles[i], ushort.MaxValue, ushort.MaxValue, ushort.MaxValue, ushort.MaxValue);
				string[] obj8 = new string[11]
				{
					"SteamInput.TriggerVibrationExtended(", null, null, null, null, null, null, null, null, null,
					null
				};
				inputHandle_t = m_InputHandles[i];
				obj8[1] = inputHandle_t.ToString();
				obj8[2] = ", ";
				obj8[3] = ushort.MaxValue.ToString();
				obj8[4] = ", ";
				obj8[5] = ushort.MaxValue.ToString();
				obj8[6] = ", ";
				obj8[7] = ushort.MaxValue.ToString();
				obj8[8] = ", ";
				obj8[9] = ushort.MaxValue.ToString();
				obj8[10] = ")";
				MonoBehaviour.print(string.Concat(obj8));
			}
			if (GUILayout.Button("SetLEDColor(m_InputHandles[i], 0, 0, 255, (int)ESteamInputLEDFlag.k_ESteamInputLEDFlag_SetColor)"))
			{
				SteamInput.SetLEDColor(m_InputHandles[i], 0, 0, byte.MaxValue, 0u);
				string[] obj9 = new string[11]
				{
					"SteamInput.SetLEDColor(", null, null, null, null, null, null, null, null, null,
					null
				};
				inputHandle_t = m_InputHandles[i];
				obj9[1] = inputHandle_t.ToString();
				obj9[2] = ", ";
				obj9[3] = 0.ToString();
				obj9[4] = ", ";
				obj9[5] = 0.ToString();
				obj9[6] = ", ";
				obj9[7] = 255.ToString();
				obj9[8] = ", ";
				obj9[9] = 0.ToString();
				obj9[10] = ")";
				MonoBehaviour.print(string.Concat(obj9));
			}
			if (GUILayout.Button("Legacy_TriggerHapticPulse(m_InputHandles[i], ESteamControllerPad.k_ESteamControllerPad_Right, 5000)"))
			{
				SteamInput.Legacy_TriggerHapticPulse(m_InputHandles[i], ESteamControllerPad.k_ESteamControllerPad_Right, 5000);
				string[] obj10 = new string[7] { "SteamInput.Legacy_TriggerHapticPulse(", null, null, null, null, null, null };
				inputHandle_t = m_InputHandles[i];
				obj10[1] = inputHandle_t.ToString();
				obj10[2] = ", ";
				obj10[3] = ESteamControllerPad.k_ESteamControllerPad_Right.ToString();
				obj10[4] = ", ";
				obj10[5] = 5000.ToString();
				obj10[6] = ")";
				MonoBehaviour.print(string.Concat(obj10));
			}
			if (GUILayout.Button("Legacy_TriggerRepeatedHapticPulse(m_InputHandles[i], ESteamControllerPad.k_ESteamControllerPad_Right, 5000, 0, 0, 0)"))
			{
				SteamInput.Legacy_TriggerRepeatedHapticPulse(m_InputHandles[i], ESteamControllerPad.k_ESteamControllerPad_Right, 5000, 0, 0, 0u);
				string[] obj11 = new string[13]
				{
					"SteamInput.Legacy_TriggerRepeatedHapticPulse(", null, null, null, null, null, null, null, null, null,
					null, null, null
				};
				inputHandle_t = m_InputHandles[i];
				obj11[1] = inputHandle_t.ToString();
				obj11[2] = ", ";
				obj11[3] = ESteamControllerPad.k_ESteamControllerPad_Right.ToString();
				obj11[4] = ", ";
				obj11[5] = 5000.ToString();
				obj11[6] = ", ";
				obj11[7] = 0.ToString();
				obj11[8] = ", ";
				obj11[9] = 0.ToString();
				obj11[10] = ", ";
				obj11[11] = 0.ToString();
				obj11[12] = ")";
				MonoBehaviour.print(string.Concat(obj11));
			}
			if (GUILayout.Button("ShowBindingPanel(m_InputHandles[i])"))
			{
				bool flag = SteamInput.ShowBindingPanel(m_InputHandles[i]);
				inputHandle_t = m_InputHandles[i];
				MonoBehaviour.print("SteamInput.ShowBindingPanel(" + inputHandle_t.ToString() + ") : " + flag);
			}
			GUILayout.Label("GetInputTypeForHandle(m_InputHandles[i]) : " + SteamInput.GetInputTypeForHandle(m_InputHandles[i]));
			GUILayout.Label("GetControllerForGamepadIndex(0) : " + SteamInput.GetControllerForGamepadIndex(0).ToString());
			GUILayout.Label("GetGamepadIndexForController(m_InputHandles[i]) : " + SteamInput.GetGamepadIndexForController(m_InputHandles[i]));
			if (GUILayout.Button("GetStringForXboxOrigin(EXboxOrigin.k_EXboxOrigin_A)"))
			{
				string stringForXboxOrigin = SteamInput.GetStringForXboxOrigin(EXboxOrigin.k_EXboxOrigin_A);
				MonoBehaviour.print("SteamInput.GetStringForXboxOrigin(" + EXboxOrigin.k_EXboxOrigin_A.ToString() + ") : " + stringForXboxOrigin);
			}
			if (GUILayout.Button("GetGlyphForXboxOrigin(EXboxOrigin.k_EXboxOrigin_A)"))
			{
				string glyphForXboxOrigin = SteamInput.GetGlyphForXboxOrigin(EXboxOrigin.k_EXboxOrigin_A);
				MonoBehaviour.print("SteamInput.GetGlyphForXboxOrigin(" + EXboxOrigin.k_EXboxOrigin_A.ToString() + ") : " + glyphForXboxOrigin);
			}
			if (GUILayout.Button("GetActionOriginFromXboxOrigin(m_InputHandles[i], EXboxOrigin.k_EXboxOrigin_A)"))
			{
				EInputActionOrigin actionOriginFromXboxOrigin = SteamInput.GetActionOriginFromXboxOrigin(m_InputHandles[i], EXboxOrigin.k_EXboxOrigin_A);
				string[] obj12 = new string[6] { "SteamInput.GetActionOriginFromXboxOrigin(", null, null, null, null, null };
				inputHandle_t = m_InputHandles[i];
				obj12[1] = inputHandle_t.ToString();
				obj12[2] = ", ";
				obj12[3] = EXboxOrigin.k_EXboxOrigin_A.ToString();
				obj12[4] = ") : ";
				obj12[5] = actionOriginFromXboxOrigin.ToString();
				MonoBehaviour.print(string.Concat(obj12));
			}
			if (GUILayout.Button("TranslateActionOrigin(ESteamInputType.k_ESteamInputType_XBoxOneController, EInputActionOrigin.k_EInputActionOrigin_XBoxOne_A)"))
			{
				EInputActionOrigin eInputActionOrigin = SteamInput.TranslateActionOrigin(ESteamInputType.k_ESteamInputType_XBoxOneController, EInputActionOrigin.k_EInputActionOrigin_XBoxOne_A);
				MonoBehaviour.print("SteamInput.TranslateActionOrigin(" + ESteamInputType.k_ESteamInputType_XBoxOneController.ToString() + ", " + EInputActionOrigin.k_EInputActionOrigin_XBoxOne_A.ToString() + ") : " + eInputActionOrigin);
			}
			if (GUILayout.Button("GetDeviceBindingRevision(m_InputHandles[i], out pMajor, out pMinor)"))
			{
				int pMajor;
				int pMinor;
				bool deviceBindingRevision = SteamInput.GetDeviceBindingRevision(m_InputHandles[i], out pMajor, out pMinor);
				string[] obj13 = new string[8] { "SteamInput.GetDeviceBindingRevision(", null, null, null, null, null, null, null };
				inputHandle_t = m_InputHandles[i];
				obj13[1] = inputHandle_t.ToString();
				obj13[2] = ", out pMajor, out pMinor) : ";
				obj13[3] = deviceBindingRevision.ToString();
				obj13[4] = " -- ";
				obj13[5] = pMajor.ToString();
				obj13[6] = " -- ";
				obj13[7] = pMinor.ToString();
				MonoBehaviour.print(string.Concat(obj13));
			}
			if (GUILayout.Button("GetRemotePlaySessionID(m_InputHandles[i])"))
			{
				uint remotePlaySessionID = SteamInput.GetRemotePlaySessionID(m_InputHandles[i]);
				inputHandle_t = m_InputHandles[i];
				MonoBehaviour.print("SteamInput.GetRemotePlaySessionID(" + inputHandle_t.ToString() + ") : " + remotePlaySessionID);
			}
		}
		GUILayout.Label("GetSessionInputConfigurationSettings() : " + SteamInput.GetSessionInputConfigurationSettings());
		GUILayout.EndScrollView();
		GUILayout.EndVertical();
	}

	private void OnSteamInputDeviceConnected(SteamInputDeviceConnected_t pCallback)
	{
		string text = 2801.ToString();
		InputHandle_t ulConnectedDeviceHandle = pCallback.m_ulConnectedDeviceHandle;
		Debug.Log("[" + text + " - SteamInputDeviceConnected] - " + ulConnectedDeviceHandle.ToString());
	}

	private void OnSteamInputDeviceDisconnected(SteamInputDeviceDisconnected_t pCallback)
	{
		string text = 2802.ToString();
		InputHandle_t ulDisconnectedDeviceHandle = pCallback.m_ulDisconnectedDeviceHandle;
		Debug.Log("[" + text + " - SteamInputDeviceDisconnected] - " + ulDisconnectedDeviceHandle.ToString());
	}

	private void OnSteamInputConfigurationLoaded(SteamInputConfigurationLoaded_t pCallback)
	{
		string[] obj = new string[16]
		{
			"[",
			2803.ToString(),
			" - SteamInputConfigurationLoaded] - ",
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null
		};
		AppId_t unAppID = pCallback.m_unAppID;
		obj[3] = unAppID.ToString();
		obj[4] = " -- ";
		InputHandle_t ulDeviceHandle = pCallback.m_ulDeviceHandle;
		obj[5] = ulDeviceHandle.ToString();
		obj[6] = " -- ";
		CSteamID ulMappingCreator = pCallback.m_ulMappingCreator;
		obj[7] = ulMappingCreator.ToString();
		obj[8] = " -- ";
		obj[9] = pCallback.m_unMajorRevision.ToString();
		obj[10] = " -- ";
		obj[11] = pCallback.m_unMinorRevision.ToString();
		obj[12] = " -- ";
		obj[13] = pCallback.m_bUsesSteamInputAPI.ToString();
		obj[14] = " -- ";
		obj[15] = pCallback.m_bUsesGamepadAPI.ToString();
		Debug.Log(string.Concat(obj));
	}

	private void OnSteamInputGamepadSlotChange(SteamInputGamepadSlotChange_t pCallback)
	{
		string[] obj = new string[12]
		{
			"[",
			2804.ToString(),
			" - SteamInputGamepadSlotChange] - ",
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null
		};
		AppId_t unAppID = pCallback.m_unAppID;
		obj[3] = unAppID.ToString();
		obj[4] = " -- ";
		InputHandle_t ulDeviceHandle = pCallback.m_ulDeviceHandle;
		obj[5] = ulDeviceHandle.ToString();
		obj[6] = " -- ";
		obj[7] = pCallback.m_eDeviceType.ToString();
		obj[8] = " -- ";
		obj[9] = pCallback.m_nOldGamepadSlot.ToString();
		obj[10] = " -- ";
		obj[11] = pCallback.m_nNewGamepadSlot.ToString();
		Debug.Log(string.Concat(obj));
	}
}
