using System;
using UnityEngine;

public class MusicMgr : MonoBehaviour
{
	private enum MusicMgrState
	{
		mute,
		VolumeUp,
		VolumeDown
	}

	public AudioSource as_Music;

	public AudioSource as_Ambient;

	public float volumeChangeSpeed;

	private MusicMgrState state;

	private bool playAmbient = true;

	private AudioClip clip_TargetMusic;

	private AudioClip clip_TargetAmbient;

	private float currentVolume;

	public static MusicMgr Inst { get; private set; }

	private void OnEnable()
	{
		EventMgr.MusicVolumeChange = (Action)Delegate.Combine(EventMgr.MusicVolumeChange, new Action(MusicVolumeChange));
		EventMgr.SoundVolumeChange = (Action)Delegate.Combine(EventMgr.SoundVolumeChange, new Action(SoundVolumeChange));
	}

	private void OnDisable()
	{
		EventMgr.MusicVolumeChange = (Action)Delegate.Remove(EventMgr.MusicVolumeChange, new Action(MusicVolumeChange));
		EventMgr.SoundVolumeChange = (Action)Delegate.Remove(EventMgr.SoundVolumeChange, new Action(SoundVolumeChange));
	}

	private void MusicVolumeChange()
	{
		as_Music.volume = DataMgr.settingData.GetFinalMusic();
	}

	private void SoundVolumeChange()
	{
		as_Ambient.volume = DataMgr.settingData.GetFinalSound();
	}

	private void Update()
	{
		VolumeChanging();
	}

	private void VolumeChanging()
	{
		switch (state)
		{
		case MusicMgrState.VolumeUp:
			if (currentVolume != 1f)
			{
				currentVolume = Mathf.MoveTowards(currentVolume, 1f, volumeChangeSpeed * Time.unscaledDeltaTime);
				as_Music.volume = DataMgr.settingData.GetFinalMusic() * currentVolume;
				as_Ambient.volume = DataMgr.settingData.GetFinalSound() * currentVolume;
			}
			break;
		case MusicMgrState.VolumeDown:
			currentVolume = Mathf.MoveTowards(currentVolume, 0f, volumeChangeSpeed * Time.unscaledDeltaTime);
			as_Music.volume = DataMgr.settingData.GetFinalMusic() * currentVolume;
			as_Ambient.volume = DataMgr.settingData.GetFinalSound() * currentVolume;
			if (currentVolume == 0f)
			{
				state = MusicMgrState.VolumeUp;
				if (as_Music.clip != clip_TargetMusic)
				{
					as_Music.clip = clip_TargetMusic;
					as_Music.Play();
				}
				if (as_Ambient.clip != clip_TargetAmbient)
				{
					as_Ambient.clip = clip_TargetAmbient;
					as_Ambient.Play();
				}
				if (!playAmbient)
				{
					as_Ambient.clip = null;
				}
			}
			break;
		default:
			Debug.LogError(state);
			break;
		case MusicMgrState.mute:
			break;
		}
	}

	public void Initialize()
	{
		Inst = this;
	}

	public void UpdateThemeMusic()
	{
		playAmbient = true;
		int themeType = (int)LevelMgr.Inst.CurrentRoomCfg.themeType;
		if (LevelMgr.Inst.CurrentRoomCfg.overrideThemeMusic == "")
		{
			clip_TargetMusic = ABResources.LoadAsset<AudioClip>("Sounds/" + ScriptableObjMgr.Inst.themeMusic.strs[themeType]);
		}
		else
		{
			clip_TargetMusic = ABResources.LoadAsset<AudioClip>("Sounds/" + LevelMgr.Inst.CurrentRoomCfg.overrideThemeMusic);
		}
		clip_TargetAmbient = ABResources.LoadAsset<AudioClip>("Sounds/" + ScriptableObjMgr.Inst.themeAmbient.strs[themeType]);
		if (clip_TargetAmbient != as_Ambient.clip)
		{
			as_Ambient.clip = clip_TargetAmbient;
			as_Ambient.Play();
		}
		switch (state)
		{
		case MusicMgrState.mute:
			state = MusicMgrState.VolumeUp;
			as_Music.clip = clip_TargetMusic;
			as_Ambient.clip = clip_TargetAmbient;
			as_Music.Play();
			as_Ambient.Play();
			break;
		case MusicMgrState.VolumeUp:
		case MusicMgrState.VolumeDown:
			if (as_Music.clip != clip_TargetMusic)
			{
				state = MusicMgrState.VolumeDown;
			}
			break;
		default:
			Debug.LogError(state);
			break;
		}
	}

	public void UpdateCampBGM()
	{
		switch (GameMgr.CampSkinType)
		{
		case CampSkinType.Default:
			if (DataMgr.selectedWorldData.IsDave)
			{
				ForcePlayMusic("BGM_Camp_Dave", playAmbient: false);
			}
			else
			{
				ForcePlayMusic("BGM_Camp", playAmbient: false);
			}
			break;
		case CampSkinType.Halloween:
			ForcePlayMusic("BGM_Camp_Halloween", playAmbient: false);
			break;
		case CampSkinType.Spring:
			ForcePlayMusic("BGM_Camp_LunarNewYear", playAmbient: false);
			break;
		case CampSkinType.Summer:
			ForcePlayMusic("BGM_Camp_Summer", playAmbient: false);
			break;
		case CampSkinType.Christmas:
			ForcePlayMusic("BGM_Camp", playAmbient: false);
			break;
		default:
			Debug.LogError(GameMgr.CampSkinType);
			ForcePlayMusic("BGM_Camp", playAmbient: false);
			break;
		}
		if (LevelMgr.Inst.CurrentRoomMapPos.y == -1)
		{
			ForcePlayMusic("BGM_Camp_Endless", playAmbient: false);
		}
	}

	public void ForcePlayMusic(string musicName, bool playAmbient = true)
	{
		this.playAmbient = playAmbient;
		switch (state)
		{
		case MusicMgrState.mute:
			state = MusicMgrState.VolumeUp;
			as_Music.clip = ABResources.LoadAsset<AudioClip>("Sounds/" + musicName);
			as_Music.Play();
			if (!playAmbient)
			{
				as_Ambient.clip = null;
			}
			break;
		case MusicMgrState.VolumeUp:
		case MusicMgrState.VolumeDown:
			state = MusicMgrState.VolumeDown;
			clip_TargetMusic = ABResources.LoadAsset<AudioClip>("Sounds/" + musicName);
			break;
		default:
			Debug.LogError(state);
			break;
		}
	}

	public void ForcePlayAmbient(string ambientName)
	{
		playAmbient = true;
		as_Ambient.clip = ABResources.LoadAsset<AudioClip>("Sounds/" + ambientName);
		as_Ambient.Play();
	}

	public void ChangePitch(float value)
	{
		as_Music.pitch = value;
	}
}
