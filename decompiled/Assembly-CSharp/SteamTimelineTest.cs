using Steamworks;
using UnityEngine;

public class SteamTimelineTest : MonoBehaviour
{
	private Vector2 m_ScrollPos;

	public void OnEnable()
	{
	}

	public void RenderOnGUI()
	{
		GUILayout.BeginVertical("box");
		m_ScrollPos = GUILayout.BeginScrollView(m_ScrollPos, GUILayout.Width(Screen.width - 215), GUILayout.Height(Screen.height - 33));
		if (GUILayout.Button("SetTimelineStateDescription(\"Description\", 0.0f)"))
		{
			SteamTimeline.SetTimelineStateDescription("Description", 0f);
			MonoBehaviour.print("SteamTimeline.SetTimelineStateDescription(\"Description\", " + 0f + ")");
		}
		if (GUILayout.Button("ClearTimelineStateDescription(0.0f)"))
		{
			SteamTimeline.ClearTimelineStateDescription(0f);
			MonoBehaviour.print("SteamTimeline.ClearTimelineStateDescription(" + 0f + ")");
		}
		if (GUILayout.Button("AddTimelineEvent(\"steam_marker\", \"Test Event\", \"Test Description\", 0, -5.0f, 5.0f, ETimelineEventClipPriority.k_ETimelineEventClipPriority_Standard)"))
		{
			SteamTimeline.AddTimelineEvent("steam_marker", "Test Event", "Test Description", 0u, -5f, 5f, ETimelineEventClipPriority.k_ETimelineEventClipPriority_Standard);
			MonoBehaviour.print("SteamTimeline.AddTimelineEvent(\"steam_marker\", \"Test Event\", \"Test Description\", " + 0 + ", " + -5f + ", " + 5f + ", " + ETimelineEventClipPriority.k_ETimelineEventClipPriority_Standard.ToString() + ")");
		}
		if (GUILayout.Button("SetTimelineGameMode(ETimelineGameMode.k_ETimelineGameMode_Playing)"))
		{
			SteamTimeline.SetTimelineGameMode(ETimelineGameMode.k_ETimelineGameMode_Playing);
			MonoBehaviour.print("SteamTimeline.SetTimelineGameMode(" + ETimelineGameMode.k_ETimelineGameMode_Playing.ToString() + ")");
		}
		GUILayout.EndScrollView();
		GUILayout.EndVertical();
	}
}
