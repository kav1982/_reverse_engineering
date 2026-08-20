using System.Collections;
using UnityEngine;

public abstract class GameUI : MonoBehaviour
{
	[HideInInspector]
	public bool isDraging;

	protected InputActions inputActions => ControlMgr.Inst.inputActions;

	public bool init { get; private set; }

	public bool IsOpen { get; private set; }

	protected void ShowInit(object obj = null)
	{
		if (!init)
		{
			StartCoroutine(ShowInitIE(obj));
		}
		else if (!IsOpen)
		{
			Show(obj);
		}
	}

	public void Show()
	{
		Show(null);
	}

	public virtual void Show(object obj = null)
	{
		if (!IsOpen)
		{
			RegistarOnlyWhenOpen();
			IsOpen = true;
			OnShow(obj);
		}
	}

	protected virtual void OnShow(object obj = null)
	{
	}

	public virtual void Hide()
	{
		if (IsOpen)
		{
			IsOpen = false;
			UnRegistarOnlyWhenHide();
			OnHide();
		}
	}

	protected abstract void OnHide();

	private IEnumerator ShowInitIE(object obj = null)
	{
		yield return StartCoroutine(Init());
		init = true;
		Show(obj);
	}

	public void OnlyInit()
	{
		StartCoroutine(OnlyInitIE());
	}

	private IEnumerator OnlyInitIE()
	{
		yield return StartCoroutine(Init());
		init = true;
	}

	protected IEnumerator Init()
	{
		RegistarWhenInit();
		yield return StartCoroutine(OnInit());
	}

	protected virtual IEnumerator OnInit()
	{
		yield return null;
	}

	public virtual void OnDestroy()
	{
		UnRegistarOnlyWhenHide();
		UnRegistarWhenDestroy();
	}

	protected abstract void RegistarWhenInit();

	protected abstract void RegistarOnlyWhenOpen();

	protected abstract void UnRegistarOnlyWhenHide();

	protected abstract void UnRegistarWhenDestroy();

	public virtual void _Close()
	{
		Hide();
	}

	protected void SetIsOpen(bool isOpen)
	{
		IsOpen = isOpen;
	}
}
