using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class UITidyupButton : MonoBehaviour
{
	public Text label;

	private Animator _anim;

	private UIBackpackBtnHover _hover;

	public static UITidyupButton Inst { get; private set; }

	private void Start()
	{
		Inst = this;
		_anim = GetComponent<Animator>();
		_hover = GetComponent<UIBackpackBtnHover>();
		EventMgr.LanguageChange = (Action)Delegate.Combine(EventMgr.LanguageChange, new Action(UpdateText));
		UpdateText();
	}

	public void OnClick()
	{
		Stopwatch stopwatch = Stopwatch.StartNew();
		TidyUpSpells.TidyUpBagAndAllWand();
		UnityEngine.Debug.Log("法术整理用时：" + stopwatch.ElapsedMilliseconds);
	}

	private void UpdateText()
	{
		if (label != null)
		{
			label.text = 1001201.GetText();
		}
	}

	public void GamepadHover()
	{
		_hover.OnPointerEnter(null);
		_anim.SetTrigger("Highlighted");
	}

	public void GamepadUnHover()
	{
		_hover.OnPointerExit(null);
		_anim.SetTrigger("Normal");
	}
}
