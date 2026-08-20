using Steamworks;
using UnityEngine;

public class SteamMusicTest : MonoBehaviour
{
	private Vector2 m_ScrollPos;

	protected Callback<PlaybackStatusHasChanged_t> m_PlaybackStatusHasChanged;

	protected Callback<VolumeHasChanged_t> m_VolumeHasChanged;

	public void OnEnable()
	{
		m_PlaybackStatusHasChanged = Callback<PlaybackStatusHasChanged_t>.Create(OnPlaybackStatusHasChanged);
		m_VolumeHasChanged = Callback<VolumeHasChanged_t>.Create(OnVolumeHasChanged);
	}

	public void RenderOnGUI()
	{
		GUILayout.BeginVertical("box");
		m_ScrollPos = GUILayout.BeginScrollView(m_ScrollPos, GUILayout.Width(Screen.width - 215), GUILayout.Height(Screen.height - 33));
		GUILayout.Label("BIsEnabled() : " + SteamMusic.BIsEnabled());
		GUILayout.Label("BIsPlaying() : " + SteamMusic.BIsPlaying());
		GUILayout.Label("GetPlaybackStatus() : " + SteamMusic.GetPlaybackStatus());
		if (GUILayout.Button("Play()"))
		{
			SteamMusic.Play();
			MonoBehaviour.print("SteamMusic.Play()");
		}
		if (GUILayout.Button("Pause()"))
		{
			SteamMusic.Pause();
			MonoBehaviour.print("SteamMusic.Pause()");
		}
		if (GUILayout.Button("PlayPrevious()"))
		{
			SteamMusic.PlayPrevious();
			MonoBehaviour.print("SteamMusic.PlayPrevious()");
		}
		if (GUILayout.Button("PlayNext()"))
		{
			SteamMusic.PlayNext();
			MonoBehaviour.print("SteamMusic.PlayNext()");
		}
		if (GUILayout.Button("SetVolume(1.0f)"))
		{
			SteamMusic.SetVolume(1f);
			MonoBehaviour.print("SteamMusic.SetVolume(" + 1f + ")");
		}
		GUILayout.Label("GetVolume() : " + SteamMusic.GetVolume());
		GUILayout.EndScrollView();
		GUILayout.EndVertical();
	}

	private void OnPlaybackStatusHasChanged(PlaybackStatusHasChanged_t pCallback)
	{
		Debug.Log("[" + 4001 + " - PlaybackStatusHasChanged]");
	}

	private void OnVolumeHasChanged(VolumeHasChanged_t pCallback)
	{
		Debug.Log("[" + 4002 + " - VolumeHasChanged] - " + pCallback.m_flNewVolume);
	}
}
