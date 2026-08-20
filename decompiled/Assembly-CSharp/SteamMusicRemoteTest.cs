using Steamworks;
using UnityEngine;

public class SteamMusicRemoteTest : MonoBehaviour
{
	private Vector2 m_ScrollPos;

	protected Callback<MusicPlayerRemoteWillActivate_t> m_MusicPlayerRemoteWillActivate;

	protected Callback<MusicPlayerRemoteWillDeactivate_t> m_MusicPlayerRemoteWillDeactivate;

	protected Callback<MusicPlayerRemoteToFront_t> m_MusicPlayerRemoteToFront;

	protected Callback<MusicPlayerWillQuit_t> m_MusicPlayerWillQuit;

	protected Callback<MusicPlayerWantsPlay_t> m_MusicPlayerWantsPlay;

	protected Callback<MusicPlayerWantsPause_t> m_MusicPlayerWantsPause;

	protected Callback<MusicPlayerWantsPlayPrevious_t> m_MusicPlayerWantsPlayPrevious;

	protected Callback<MusicPlayerWantsPlayNext_t> m_MusicPlayerWantsPlayNext;

	protected Callback<MusicPlayerWantsShuffled_t> m_MusicPlayerWantsShuffled;

	protected Callback<MusicPlayerWantsLooped_t> m_MusicPlayerWantsLooped;

	protected Callback<MusicPlayerWantsVolume_t> m_MusicPlayerWantsVolume;

	protected Callback<MusicPlayerSelectsQueueEntry_t> m_MusicPlayerSelectsQueueEntry;

	protected Callback<MusicPlayerSelectsPlaylistEntry_t> m_MusicPlayerSelectsPlaylistEntry;

	protected Callback<MusicPlayerWantsPlayingRepeatStatus_t> m_MusicPlayerWantsPlayingRepeatStatus;

	public void OnEnable()
	{
		m_MusicPlayerRemoteWillActivate = Callback<MusicPlayerRemoteWillActivate_t>.Create(OnMusicPlayerRemoteWillActivate);
		m_MusicPlayerRemoteWillDeactivate = Callback<MusicPlayerRemoteWillDeactivate_t>.Create(OnMusicPlayerRemoteWillDeactivate);
		m_MusicPlayerRemoteToFront = Callback<MusicPlayerRemoteToFront_t>.Create(OnMusicPlayerRemoteToFront);
		m_MusicPlayerWillQuit = Callback<MusicPlayerWillQuit_t>.Create(OnMusicPlayerWillQuit);
		m_MusicPlayerWantsPlay = Callback<MusicPlayerWantsPlay_t>.Create(OnMusicPlayerWantsPlay);
		m_MusicPlayerWantsPause = Callback<MusicPlayerWantsPause_t>.Create(OnMusicPlayerWantsPause);
		m_MusicPlayerWantsPlayPrevious = Callback<MusicPlayerWantsPlayPrevious_t>.Create(OnMusicPlayerWantsPlayPrevious);
		m_MusicPlayerWantsPlayNext = Callback<MusicPlayerWantsPlayNext_t>.Create(OnMusicPlayerWantsPlayNext);
		m_MusicPlayerWantsShuffled = Callback<MusicPlayerWantsShuffled_t>.Create(OnMusicPlayerWantsShuffled);
		m_MusicPlayerWantsLooped = Callback<MusicPlayerWantsLooped_t>.Create(OnMusicPlayerWantsLooped);
		m_MusicPlayerWantsVolume = Callback<MusicPlayerWantsVolume_t>.Create(OnMusicPlayerWantsVolume);
		m_MusicPlayerSelectsQueueEntry = Callback<MusicPlayerSelectsQueueEntry_t>.Create(OnMusicPlayerSelectsQueueEntry);
		m_MusicPlayerSelectsPlaylistEntry = Callback<MusicPlayerSelectsPlaylistEntry_t>.Create(OnMusicPlayerSelectsPlaylistEntry);
		m_MusicPlayerWantsPlayingRepeatStatus = Callback<MusicPlayerWantsPlayingRepeatStatus_t>.Create(OnMusicPlayerWantsPlayingRepeatStatus);
	}

	public void RenderOnGUI()
	{
		GUILayout.BeginVertical("box");
		m_ScrollPos = GUILayout.BeginScrollView(m_ScrollPos, GUILayout.Width(Screen.width - 215), GUILayout.Height(Screen.height - 33));
		if (GUILayout.Button("RegisterSteamMusicRemote(\"Steamworks.NET Test Remote\")"))
		{
			MonoBehaviour.print("SteamMusicRemote.RegisterSteamMusicRemote(\"Steamworks.NET Test Remote\") : " + SteamMusicRemote.RegisterSteamMusicRemote("Steamworks.NET Test Remote"));
		}
		if (GUILayout.Button("DeregisterSteamMusicRemote()"))
		{
			MonoBehaviour.print("SteamMusicRemote.DeregisterSteamMusicRemote() : " + SteamMusicRemote.DeregisterSteamMusicRemote());
		}
		GUILayout.Label("BIsCurrentMusicRemote() : " + SteamMusicRemote.BIsCurrentMusicRemote());
		GUILayout.Label("BActivationSuccess(true) : " + SteamMusicRemote.BActivationSuccess(bValue: true));
		if (GUILayout.Button("SetDisplayName(\"Some Display Name\")"))
		{
			MonoBehaviour.print("SteamMusicRemote.SetDisplayName(\"Some Display Name\") : " + SteamMusicRemote.SetDisplayName("Some Display Name"));
		}
		if (GUILayout.Button("SetPNGIcon_64x64(null, 0)"))
		{
			MonoBehaviour.print(string.Concat(str3: SteamMusicRemote.SetPNGIcon_64x64(null, 0u).ToString(), str0: "SteamMusicRemote.SetPNGIcon_64x64(, ", str1: 0.ToString(), str2: ") : "));
		}
		if (GUILayout.Button("EnablePlayPrevious(true)"))
		{
			MonoBehaviour.print(string.Concat(str3: SteamMusicRemote.EnablePlayPrevious(bValue: true).ToString(), str0: "SteamMusicRemote.EnablePlayPrevious(", str1: true.ToString(), str2: ") : "));
		}
		if (GUILayout.Button("EnablePlayNext(true)"))
		{
			MonoBehaviour.print(string.Concat(str3: SteamMusicRemote.EnablePlayNext(bValue: true).ToString(), str0: "SteamMusicRemote.EnablePlayNext(", str1: true.ToString(), str2: ") : "));
		}
		if (GUILayout.Button("EnableShuffled(true)"))
		{
			MonoBehaviour.print(string.Concat(str3: SteamMusicRemote.EnableShuffled(bValue: true).ToString(), str0: "SteamMusicRemote.EnableShuffled(", str1: true.ToString(), str2: ") : "));
		}
		if (GUILayout.Button("EnableLooped(true)"))
		{
			MonoBehaviour.print(string.Concat(str3: SteamMusicRemote.EnableLooped(bValue: true).ToString(), str0: "SteamMusicRemote.EnableLooped(", str1: true.ToString(), str2: ") : "));
		}
		if (GUILayout.Button("EnableQueue(true)"))
		{
			MonoBehaviour.print(string.Concat(str3: SteamMusicRemote.EnableQueue(bValue: true).ToString(), str0: "SteamMusicRemote.EnableQueue(", str1: true.ToString(), str2: ") : "));
		}
		if (GUILayout.Button("EnablePlaylists(true)"))
		{
			MonoBehaviour.print(string.Concat(str3: SteamMusicRemote.EnablePlaylists(bValue: true).ToString(), str0: "SteamMusicRemote.EnablePlaylists(", str1: true.ToString(), str2: ") : "));
		}
		if (GUILayout.Button("UpdatePlaybackStatus(AudioPlayback_Status.AudioPlayback_Paused)"))
		{
			MonoBehaviour.print(string.Concat(str3: SteamMusicRemote.UpdatePlaybackStatus(AudioPlayback_Status.AudioPlayback_Paused).ToString(), str0: "SteamMusicRemote.UpdatePlaybackStatus(", str1: AudioPlayback_Status.AudioPlayback_Paused.ToString(), str2: ") : "));
		}
		if (GUILayout.Button("UpdateShuffled(true)"))
		{
			MonoBehaviour.print(string.Concat(str3: SteamMusicRemote.UpdateShuffled(bValue: true).ToString(), str0: "SteamMusicRemote.UpdateShuffled(", str1: true.ToString(), str2: ") : "));
		}
		if (GUILayout.Button("UpdateLooped(true)"))
		{
			MonoBehaviour.print(string.Concat(str3: SteamMusicRemote.UpdateLooped(bValue: true).ToString(), str0: "SteamMusicRemote.UpdateLooped(", str1: true.ToString(), str2: ") : "));
		}
		if (GUILayout.Button("UpdateVolume(0.5f)"))
		{
			MonoBehaviour.print(string.Concat(str3: SteamMusicRemote.UpdateVolume(0.5f).ToString(), str0: "SteamMusicRemote.UpdateVolume(", str1: 0.5f.ToString(), str2: ") : "));
		}
		if (GUILayout.Button("CurrentEntryWillChange()"))
		{
			MonoBehaviour.print("SteamMusicRemote.CurrentEntryWillChange() : " + SteamMusicRemote.CurrentEntryWillChange());
		}
		if (GUILayout.Button("CurrentEntryIsAvailable(true)"))
		{
			MonoBehaviour.print(string.Concat(str3: SteamMusicRemote.CurrentEntryIsAvailable(bAvailable: true).ToString(), str0: "SteamMusicRemote.CurrentEntryIsAvailable(", str1: true.ToString(), str2: ") : "));
		}
		if (GUILayout.Button("UpdateCurrentEntryText(\"Current Entry Text\")"))
		{
			MonoBehaviour.print("SteamMusicRemote.UpdateCurrentEntryText(\"Current Entry Text\") : " + SteamMusicRemote.UpdateCurrentEntryText("Current Entry Text"));
		}
		if (GUILayout.Button("UpdateCurrentEntryElapsedSeconds(10)"))
		{
			MonoBehaviour.print(string.Concat(str3: SteamMusicRemote.UpdateCurrentEntryElapsedSeconds(10).ToString(), str0: "SteamMusicRemote.UpdateCurrentEntryElapsedSeconds(", str1: 10.ToString(), str2: ") : "));
		}
		if (GUILayout.Button("UpdateCurrentEntryCoverArt(null, 0)"))
		{
			MonoBehaviour.print(string.Concat(str3: SteamMusicRemote.UpdateCurrentEntryCoverArt(null, 0u).ToString(), str0: "SteamMusicRemote.UpdateCurrentEntryCoverArt(, ", str1: 0.ToString(), str2: ") : "));
		}
		if (GUILayout.Button("CurrentEntryDidChange()"))
		{
			MonoBehaviour.print("SteamMusicRemote.CurrentEntryDidChange() : " + SteamMusicRemote.CurrentEntryDidChange());
		}
		if (GUILayout.Button("QueueWillChange()"))
		{
			MonoBehaviour.print("SteamMusicRemote.QueueWillChange() : " + SteamMusicRemote.QueueWillChange());
		}
		if (GUILayout.Button("ResetQueueEntries()"))
		{
			MonoBehaviour.print("SteamMusicRemote.ResetQueueEntries() : " + SteamMusicRemote.ResetQueueEntries());
		}
		if (GUILayout.Button("SetQueueEntry(0, 0, \"I don't know what I'm doing\")"))
		{
			bool flag = SteamMusicRemote.SetQueueEntry(0, 0, "I don't know what I'm doing");
			MonoBehaviour.print("SteamMusicRemote.SetQueueEntry(" + 0 + ", " + 0 + ", \"I don't know what I'm doing\") : " + flag);
		}
		if (GUILayout.Button("SetCurrentQueueEntry(0)"))
		{
			MonoBehaviour.print(string.Concat(str3: SteamMusicRemote.SetCurrentQueueEntry(0).ToString(), str0: "SteamMusicRemote.SetCurrentQueueEntry(", str1: 0.ToString(), str2: ") : "));
		}
		if (GUILayout.Button("QueueDidChange()"))
		{
			MonoBehaviour.print("SteamMusicRemote.QueueDidChange() : " + SteamMusicRemote.QueueDidChange());
		}
		if (GUILayout.Button("PlaylistWillChange()"))
		{
			MonoBehaviour.print("SteamMusicRemote.PlaylistWillChange() : " + SteamMusicRemote.PlaylistWillChange());
		}
		if (GUILayout.Button("ResetPlaylistEntries()"))
		{
			MonoBehaviour.print("SteamMusicRemote.ResetPlaylistEntries() : " + SteamMusicRemote.ResetPlaylistEntries());
		}
		if (GUILayout.Button("SetPlaylistEntry(0, 0, \"I don't know what I'm doing\")"))
		{
			bool flag2 = SteamMusicRemote.SetPlaylistEntry(0, 0, "I don't know what I'm doing");
			MonoBehaviour.print("SteamMusicRemote.SetPlaylistEntry(" + 0 + ", " + 0 + ", \"I don't know what I'm doing\") : " + flag2);
		}
		if (GUILayout.Button("SetCurrentPlaylistEntry(0)"))
		{
			MonoBehaviour.print(string.Concat(str3: SteamMusicRemote.SetCurrentPlaylistEntry(0).ToString(), str0: "SteamMusicRemote.SetCurrentPlaylistEntry(", str1: 0.ToString(), str2: ") : "));
		}
		if (GUILayout.Button("PlaylistDidChange()"))
		{
			MonoBehaviour.print("SteamMusicRemote.PlaylistDidChange() : " + SteamMusicRemote.PlaylistDidChange());
		}
		GUILayout.EndScrollView();
		GUILayout.EndVertical();
	}

	private void OnMusicPlayerRemoteWillActivate(MusicPlayerRemoteWillActivate_t pCallback)
	{
		Debug.Log("[" + 4101 + " - MusicPlayerRemoteWillActivate]");
	}

	private void OnMusicPlayerRemoteWillDeactivate(MusicPlayerRemoteWillDeactivate_t pCallback)
	{
		Debug.Log("[" + 4102 + " - MusicPlayerRemoteWillDeactivate]");
	}

	private void OnMusicPlayerRemoteToFront(MusicPlayerRemoteToFront_t pCallback)
	{
		Debug.Log("[" + 4103 + " - MusicPlayerRemoteToFront]");
	}

	private void OnMusicPlayerWillQuit(MusicPlayerWillQuit_t pCallback)
	{
		Debug.Log("[" + 4104 + " - MusicPlayerWillQuit]");
	}

	private void OnMusicPlayerWantsPlay(MusicPlayerWantsPlay_t pCallback)
	{
		Debug.Log("[" + 4105 + " - MusicPlayerWantsPlay]");
	}

	private void OnMusicPlayerWantsPause(MusicPlayerWantsPause_t pCallback)
	{
		Debug.Log("[" + 4106 + " - MusicPlayerWantsPause]");
	}

	private void OnMusicPlayerWantsPlayPrevious(MusicPlayerWantsPlayPrevious_t pCallback)
	{
		Debug.Log("[" + 4107 + " - MusicPlayerWantsPlayPrevious]");
	}

	private void OnMusicPlayerWantsPlayNext(MusicPlayerWantsPlayNext_t pCallback)
	{
		Debug.Log("[" + 4108 + " - MusicPlayerWantsPlayNext]");
	}

	private void OnMusicPlayerWantsShuffled(MusicPlayerWantsShuffled_t pCallback)
	{
		Debug.Log("[" + 4109 + " - MusicPlayerWantsShuffled] - " + pCallback.m_bShuffled);
	}

	private void OnMusicPlayerWantsLooped(MusicPlayerWantsLooped_t pCallback)
	{
		Debug.Log("[" + 4110 + " - MusicPlayerWantsLooped] - " + pCallback.m_bLooped);
	}

	private void OnMusicPlayerWantsVolume(MusicPlayerWantsVolume_t pCallback)
	{
		Debug.Log("[" + 4011 + " - MusicPlayerWantsVolume] - " + pCallback.m_flNewVolume);
	}

	private void OnMusicPlayerSelectsQueueEntry(MusicPlayerSelectsQueueEntry_t pCallback)
	{
		Debug.Log("[" + 4012 + " - MusicPlayerSelectsQueueEntry] - " + pCallback.nID);
	}

	private void OnMusicPlayerSelectsPlaylistEntry(MusicPlayerSelectsPlaylistEntry_t pCallback)
	{
		Debug.Log("[" + 4013 + " - MusicPlayerSelectsPlaylistEntry] - " + pCallback.nID);
	}

	private void OnMusicPlayerWantsPlayingRepeatStatus(MusicPlayerWantsPlayingRepeatStatus_t pCallback)
	{
		Debug.Log("[" + 4114 + " - MusicPlayerWantsPlayingRepeatStatus] - " + pCallback.m_nPlayingRepeatStatus);
	}
}
