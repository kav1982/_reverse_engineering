using System.Collections.Generic;
using UnityEngine;

public class TimeScaleMgr : MonoBehaviour
{
	public enum ManagerState
	{
		FadeTo,
		Progress,
		FadeOut
	}

	private class TimescaleChangeData
	{
		public float fadeSpeed;

		public float targetTimeScale;

		public float durationLeft;

		public float currentScale = 1f;

		public float endScale = 1f;

		public float fadeOutSpeed;

		public bool affectSEPitch = true;

		public ManagerState timeState;
	}

	private float fixDeltaTime = 0.02f;

	private float currentTimeScale;

	public int GamePauseCounter;

	private float pauseBeforeTimeScale;

	private List<TimescaleChangeData> timeScaleDataList = new List<TimescaleChangeData>();

	public static TimeScaleMgr Inst { get; private set; }

	public bool GamePaused => GamePauseCounter > 0;

	public void OverrideFixDeltaTime(float time)
	{
		fixDeltaTime = time;
		Time.fixedDeltaTime = fixDeltaTime * Time.timeScale;
	}

	private void LateUpdate()
	{
		UpdateTimeScaleDate();
	}

	public void UpdateTimeScaleDate(bool withoutTimer = false)
	{
		if (GamePaused)
		{
			return;
		}
		for (int i = 0; i < timeScaleDataList.Count; i++)
		{
			TimescaleChangeData timescaleChangeData = timeScaleDataList[i];
			switch (timescaleChangeData.timeState)
			{
			case ManagerState.FadeTo:
				timescaleChangeData.currentScale = Mathf.MoveTowards(timescaleChangeData.currentScale, timescaleChangeData.targetTimeScale, timescaleChangeData.fadeSpeed * Time.unscaledDeltaTime);
				if (timescaleChangeData.currentScale == timescaleChangeData.targetTimeScale)
				{
					timescaleChangeData.timeState = ManagerState.Progress;
				}
				break;
			case ManagerState.Progress:
				if (!withoutTimer)
				{
					timescaleChangeData.durationLeft -= Time.unscaledDeltaTime;
				}
				if (timescaleChangeData.durationLeft <= 0f)
				{
					timescaleChangeData.timeState = ManagerState.FadeOut;
				}
				break;
			case ManagerState.FadeOut:
				timescaleChangeData.currentScale = Mathf.MoveTowards(timescaleChangeData.currentScale, timescaleChangeData.endScale, timescaleChangeData.fadeOutSpeed * Time.unscaledDeltaTime);
				if (timescaleChangeData.currentScale == timescaleChangeData.endScale)
				{
					timeScaleDataList.Remove(timescaleChangeData);
				}
				break;
			}
			Time.timeScale = timescaleChangeData.currentScale;
			Time.fixedDeltaTime = fixDeltaTime * Time.timeScale;
			if (timescaleChangeData.affectSEPitch)
			{
				MusicMgr.Inst.ChangePitch(Time.timeScale);
			}
		}
	}

	public void Initialize()
	{
		Inst = this;
	}

	public void AddNewTimeScaleModifyRequest(float targetScale, float effectDuration, float timeFadeSpeed, ManagerState startProgress = ManagerState.FadeTo, float startScale = 1f, float endScale = 1f, float timeFadeOutSpeed = 0f, bool affectSEPitch = true)
	{
		TimescaleChangeData timescaleChangeData = new TimescaleChangeData();
		timescaleChangeData.fadeSpeed = timeFadeSpeed;
		timescaleChangeData.targetTimeScale = targetScale;
		timescaleChangeData.durationLeft = effectDuration;
		timescaleChangeData.currentScale = startScale;
		timescaleChangeData.timeState = startProgress;
		timescaleChangeData.endScale = endScale;
		timescaleChangeData.affectSEPitch = affectSEPitch;
		timescaleChangeData.fadeOutSpeed = ((timeFadeOutSpeed == 0f) ? timeFadeSpeed : timeFadeOutSpeed);
		timeScaleDataList.Add(timescaleChangeData);
		UpdateTimeScaleDate(withoutTimer: true);
	}

	public void ClearAllTimeScaleModifyRequest()
	{
		timeScaleDataList.Clear();
		Time.timeScale = 1f;
		MusicMgr.Inst.ChangePitch(Time.timeScale);
		Time.fixedDeltaTime = fixDeltaTime * Time.timeScale;
	}

	public void Pause()
	{
		GamePauseCounter++;
		if (GamePauseCounter == 1)
		{
			pauseBeforeTimeScale = Time.timeScale;
			Time.timeScale = 0f;
			SEMgr.Inst.PauseAllLoopSE();
		}
	}

	public void Recovery()
	{
		GamePauseCounter--;
		if (GamePauseCounter == 0)
		{
			Time.timeScale = pauseBeforeTimeScale;
			SEMgr.Inst.UnPauseAllLoopSE();
		}
	}

	public void ForceRecover()
	{
		GamePauseCounter = 0;
		Time.timeScale = 1f;
	}
}
