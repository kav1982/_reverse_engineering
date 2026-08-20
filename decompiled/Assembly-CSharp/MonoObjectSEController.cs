using System;
using UnityEngine;

public class MonoObjectSEController : MonoBehaviour
{
	public AudioSource Audio;

	private void SoundVolumeChange()
	{
		Audio.volume = DataMgr.settingData.GetFinalSound();
	}

	public void OnEnable()
	{
		EventMgr.SoundVolumeChange = (Action)Delegate.Combine(EventMgr.SoundVolumeChange, new Action(SoundVolumeChange));
		SoundVolumeChange();
	}

	public void OnDisable()
	{
		EventMgr.SoundVolumeChange = (Action)Delegate.Remove(EventMgr.SoundVolumeChange, new Action(SoundVolumeChange));
	}
}
