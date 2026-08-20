using System;
using UnityEngine;

public class LoopAudioController : MonoBehaviour
{
	public AudioSource LoopAudio;

	private bool isPauseDisableSound;

	public void OnEnable()
	{
		if (!(LoopAudio == null))
		{
			EventMgr.SoundVolumeChange = (Action)Delegate.Combine(EventMgr.SoundVolumeChange, new Action(SoundVolumeChange));
			SoundVolumeChange();
		}
	}

	public void OnDisable()
	{
		if (!(LoopAudio == null))
		{
			EventMgr.SoundVolumeChange = (Action)Delegate.Remove(EventMgr.SoundVolumeChange, new Action(SoundVolumeChange));
		}
	}

	private void SoundVolumeChange()
	{
		LoopAudio.volume = DataMgr.settingData.GetFinalSound();
	}

	private void Update()
	{
		if (!(LoopAudio == null))
		{
			if (Time.timeScale == 0f && !isPauseDisableSound)
			{
				LoopAudio.Pause();
				isPauseDisableSound = true;
			}
			if (isPauseDisableSound && Time.timeScale != 0f)
			{
				LoopAudio.Play();
				isPauseDisableSound = false;
			}
		}
	}
}
