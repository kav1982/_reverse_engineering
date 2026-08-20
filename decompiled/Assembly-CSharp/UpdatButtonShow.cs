using System;
using UnityEngine;
using UnityEngine.UI;

public class UpdatButtonShow : MonoBehaviour
{
	public bool setbuttonOnStart;

	public bool setbuttonOnEnable;

	public UISetting.btnEnum btnEnum;

	public UISetting.controlEnum controlEnum;

	public Image buttonimage;

	public Text buttonshow;

	public RectTransform rect;

	public bool updatekey = true;

	public bool updategamepad = true;

	public int heightoffset = 10;

	public int widthoffset = 20;

	private void OnEnable()
	{
		if (setbuttonOnEnable)
		{
			UpdateButton();
		}
	}

	private void Start()
	{
		if (setbuttonOnStart)
		{
			UpdateButton();
		}
	}

	public void UpdateButton()
	{
		if (!UIMgr.Inst)
		{
			return;
		}
		if ((bool)buttonimage)
		{
			buttonimage.sprite = null;
			buttonimage.color = Color.white;
		}
		if (buttonshow != null)
		{
			buttonshow.text = "";
		}
		if ((UIMgr.Inst.InputType == PlayerInputType.Gamepad && updategamepad) || (UIMgr.Inst.InputType == PlayerInputType.Keyboard && updatekey))
		{
			try
			{
				ControlMgr.GetAndShowControlKey(rect, buttonimage, buttonshow, btnEnum, controlEnum, heightoffset, widthoffset);
				return;
			}
			catch (Exception message)
			{
				Debug.Log(message);
				return;
			}
		}
		HideButtonShow();
	}

	public void HideButtonShow()
	{
		buttonimage.sprite = null;
		buttonimage.color = Color.clear;
		if (buttonshow != null)
		{
			buttonshow.text = "";
		}
	}
}
