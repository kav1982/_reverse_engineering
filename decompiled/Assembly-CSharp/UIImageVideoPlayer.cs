using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class UIImageVideoPlayer : MonoBehaviour
{
	public Texture[] Frames;

	public int FPS;

	public AudioSource Audio;

	public Action OnOver;

	[HideInInspector]
	public float Time;

	private RawImage _image;

	private int _frame;

	public float TotalTime => (float)Frames.Length * FrameInterval;

	private float FrameInterval => 1f / (float)FPS;

	private void Awake()
	{
		_image = GetComponent<RawImage>();
		_image.texture = Frames[0];
	}

	private void Update()
	{
		if (Time == 0f)
		{
			RestartAudio();
		}
		Time += UnityEngine.Time.unscaledDeltaTime;
		if (Time > TotalTime)
		{
			Time = 0f;
			RestartAudio();
			OnOver?.Invoke();
		}
		_image.texture = Frames[(int)(Time / FrameInterval)];
		if ((bool)Audio)
		{
			if (DataMgr.settingData != null)
			{
				Audio.volume = DataMgr.settingData.mainvolume * DataMgr.settingData.music;
			}
			if (Mathf.Abs(Audio.time - Time) > 0.1f)
			{
				Audio.time = Time;
			}
		}
	}

	private void RestartAudio()
	{
		if ((bool)Audio)
		{
			Audio.Stop();
			Audio.Play();
		}
	}
}
