using System;
using UnityEngine;

public class UIFade : MonoBehaviour
{
	public Animator anima;

	public float fadeTime;

	private Action showFinishAct;

	private Action hideFinishAct;

	public bool IsOpen { get; private set; }

	public void Show(Action showFinishAct = null)
	{
		Show(fadeTime, showFinishAct);
	}

	public void Show(float time, Action showFinishAct = null)
	{
		this.showFinishAct = showFinishAct;
		IsOpen = true;
		if (time <= 0f)
		{
			anima.SetTrigger("ShowDirect");
			return;
		}
		anima.SetTrigger("Show");
		anima.speed = 1f / time;
	}

	public void Hide(Action hideFinishAct = null)
	{
		Hide(fadeTime, hideFinishAct);
	}

	public void Hide(float time, Action hideFinishAct = null)
	{
		this.hideFinishAct = hideFinishAct;
		if (time <= 0f)
		{
			anima.SetTrigger("HideDirect");
			IsOpen = false;
		}
		else
		{
			anima.SetTrigger("Hide");
			anima.speed = 1f / time;
		}
	}

	private void _AppearFinish()
	{
		if (showFinishAct != null)
		{
			showFinishAct();
		}
	}

	private void _DisappearFinish()
	{
		IsOpen = false;
		if (hideFinishAct != null)
		{
			hideFinishAct();
		}
	}
}
