using System;
using UnityEngine;

public class RestroomLight : MonoBehaviour
{
	public AudioSource[] as_Malfunctions;

	public Animator anima;

	public Vector2Int roomMapPoint;

	private bool inBathroom;

	private bool isBroken;

	private void OnEnable()
	{
		EventMgr.SoundVolumeChange = (Action)Delegate.Combine(EventMgr.SoundVolumeChange, new Action(SoundVolumeChange));
		SoundVolumeChange();
	}

	private void OnDisable()
	{
		EventMgr.SoundVolumeChange = (Action)Delegate.Remove(EventMgr.SoundVolumeChange, new Action(SoundVolumeChange));
	}

	private void SoundVolumeChange()
	{
		for (int i = 0; i < as_Malfunctions.Length; i++)
		{
			as_Malfunctions[i].volume = DataMgr.settingData.GetFinalSound();
		}
	}

	public void SetInBathroom(bool inBathroom)
	{
		this.inBathroom = inBathroom;
	}

	public void StopAnima()
	{
		anima.SetTrigger("Die");
	}

	private void _PlaySound()
	{
		if (inBathroom && !isBroken)
		{
			as_Malfunctions[UnityEngine.Random.Range(0, as_Malfunctions.Length)].Play();
		}
	}

	private void _LightDead()
	{
		isBroken = true;
	}
}
