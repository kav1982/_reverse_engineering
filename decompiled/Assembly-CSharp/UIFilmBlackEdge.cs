using System;
using UnityEngine;

public class UIFilmBlackEdge : MonoBehaviour
{
	public Animator anima;

	public float normalDuration;

	public GameObject blackEdgeUp;

	public GameObject blackEdigeDown;

	private Action showFinishAct;

	private Action hideFinishAct;

	public void Show(float time, Action showFinishAct = null)
	{
		if (time <= 0f)
		{
			anima.SetTrigger("ShowDirect");
			showFinishAct?.Invoke();
		}
		else
		{
			this.showFinishAct = showFinishAct;
			anima.speed = normalDuration / time;
			anima.SetTrigger("Show");
		}
		if (GameMgr.IsMobile_Static)
		{
			SetBlackEdgeSize(0.7f);
		}
		else
		{
			SetBlackEdgeSize(1f);
		}
	}

	public void Hide(Action hideFinishAct = null)
	{
		Hide(normalDuration, hideFinishAct);
	}

	public void Hide(float time, Action hideFinishAct = null)
	{
		if (time <= 0f)
		{
			anima.SetTrigger("HideDirect");
			hideFinishAct?.Invoke();
		}
		else
		{
			this.hideFinishAct = hideFinishAct;
			anima.speed = normalDuration / time;
			anima.SetTrigger("Hide");
		}
	}

	private void _ShowFinish()
	{
		if (showFinishAct != null)
		{
			showFinishAct();
		}
	}

	private void _HideFinish()
	{
		if (hideFinishAct != null)
		{
			hideFinishAct();
		}
		ResetBlackEdgeSize();
	}

	public void ResetBlackEdgeSize()
	{
		blackEdgeUp.transform.localScale = Vector3.one;
		blackEdigeDown.transform.localScale = Vector3.one;
	}

	public void SetBlackEdgeSize(float ratio)
	{
		blackEdgeUp.transform.localScale = new Vector3(1f, ratio, 1f);
		blackEdigeDown.transform.localScale = new Vector3(1f, ratio, 1f);
	}
}
