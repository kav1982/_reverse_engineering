using Steamworks;
using UnityEngine;

public class SteamParentalSettingsTest : MonoBehaviour
{
	private Vector2 m_ScrollPos;

	protected Callback<SteamParentalSettingsChanged_t> m_SteamParentalSettingsChanged;

	public void OnEnable()
	{
		m_SteamParentalSettingsChanged = Callback<SteamParentalSettingsChanged_t>.Create(OnSteamParentalSettingsChanged);
	}

	public void RenderOnGUI()
	{
		GUILayout.BeginVertical("box");
		m_ScrollPos = GUILayout.BeginScrollView(m_ScrollPos, GUILayout.Width(Screen.width - 215), GUILayout.Height(Screen.height - 33));
		GUILayout.Label("BIsParentalLockEnabled() : " + SteamParentalSettings.BIsParentalLockEnabled());
		GUILayout.Label("BIsParentalLockLocked() : " + SteamParentalSettings.BIsParentalLockLocked());
		GUILayout.Label("BIsAppBlocked(SteamUtils.GetAppID()) : " + SteamParentalSettings.BIsAppBlocked(SteamUtils.GetAppID()));
		GUILayout.Label("BIsAppInBlockList(SteamUtils.GetAppID()) : " + SteamParentalSettings.BIsAppInBlockList(SteamUtils.GetAppID()));
		GUILayout.Label("BIsFeatureBlocked(EParentalFeature.k_EFeatureTest) : " + SteamParentalSettings.BIsFeatureBlocked(EParentalFeature.k_EFeatureTest));
		GUILayout.Label("BIsFeatureInBlockList(EParentalFeature.k_EFeatureTest) : " + SteamParentalSettings.BIsFeatureInBlockList(EParentalFeature.k_EFeatureTest));
		GUILayout.EndScrollView();
		GUILayout.EndVertical();
	}

	private void OnSteamParentalSettingsChanged(SteamParentalSettingsChanged_t pCallback)
	{
		Debug.Log("[" + 5001 + " - SteamParentalSettingsChanged]");
	}
}
