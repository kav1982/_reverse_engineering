using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UICredites : MonoBehaviour
{
	public Animator Animator;

	public List<Text> texsts;

	public GameObject ScrollObj;

	private float _currentOpenTime;

	public float StartPositiony;

	public float EndPositiony;

	private float _scrollSpeed;

	public float ScrollSpeed;

	public float ScrollSpeedAccelerate;

	public float TimeInterval;

	public float CloseTime;

	public Sprite logoCompanyCN;

	public Sprite logoCompanyEN;

	public Sprite logoBiliCN;

	public Sprite logoBiliEN;

	public Image imageBili;

	public Image imageCompany;

	private InputActions inputActions;

	private float _timeFinishShow;

	private bool gamepadInteract;

	private bool ignoreSpeedUp;

	public bool IsOpen { get; private set; }

	private void OnEnable()
	{
		inputActions = ControlMgr.Inst.inputActions;
		inputActions.Player.GamepadEast.performed += BackPerformed;
		inputActions.Player.Pause.performed += BackPerformed;
		inputActions.Player.Interact.performed += InteractPerformed;
		inputActions.Player.Interact.canceled += InteractCanceled;
		EventMgr.LanguageChange = (Action)Delegate.Combine(EventMgr.LanguageChange, new Action(LanguageChange));
	}

	private void OnDisable()
	{
		inputActions.Player.GamepadEast.performed -= BackPerformed;
		inputActions.Player.Interact.performed -= InteractPerformed;
		inputActions.Player.Interact.canceled -= InteractCanceled;
		inputActions.Player.Pause.performed -= BackPerformed;
		EventMgr.LanguageChange = (Action)Delegate.Remove(EventMgr.LanguageChange, new Action(LanguageChange));
	}

	private void InteractCanceled(InputAction.CallbackContext obj)
	{
		gamepadInteract = false;
	}

	private void InteractPerformed(InputAction.CallbackContext obj)
	{
		gamepadInteract = true;
	}

	private void BackPerformed(InputAction.CallbackContext obj)
	{
		if (UIMgr.Inst.InputType == PlayerInputType.Gamepad && IsOpen)
		{
			Hide();
		}
	}

	private void LanguageChange()
	{
		for (int i = 0; i < texsts.Count; i++)
		{
			if (texsts[i] != null)
			{
				texsts[i].text = (1003601 + i).GetText();
			}
		}
	}

	private void Start()
	{
		gamepadInteract = false;
		_scrollSpeed = ScrollSpeed;
		LanguageChange();
		UpdateLogo();
	}

	private void UpdateLogo()
	{
		LanguageType language = DataMgr.settingData.language;
		if (language == LanguageType.ChineseS || language == LanguageType.ChineseT)
		{
			ShowCNIcon();
		}
		else
		{
			ShowENIcon();
		}
		void ShowCNIcon()
		{
			imageBili.sprite = logoBiliCN;
			imageCompany.sprite = logoCompanyCN;
			imageBili.SetNativeSize();
			imageCompany.SetNativeSize();
		}
		void ShowENIcon()
		{
			imageBili.sprite = logoBiliEN;
			imageCompany.sprite = logoCompanyEN;
			imageBili.SetNativeSize();
			imageCompany.SetNativeSize();
		}
	}

	private void Update()
	{
		if (!IsOpen)
		{
			return;
		}
		if (UIMgr.Inst.InputType != PlayerInputType.Gamepad)
		{
			if (Input.GetMouseButton(0) && !ignoreSpeedUp)
			{
				_scrollSpeed = ScrollSpeedAccelerate;
			}
			else
			{
				_scrollSpeed = ScrollSpeed;
			}
		}
		else if (gamepadInteract && !ignoreSpeedUp)
		{
			_scrollSpeed = ScrollSpeedAccelerate;
		}
		else
		{
			_scrollSpeed = ScrollSpeed;
		}
		_currentOpenTime += Time.deltaTime;
		if (_currentOpenTime <= TimeInterval)
		{
			return;
		}
		if (ScrollObj.transform.localPosition.y <= EndPositiony)
		{
			ScrollObj.transform.localPosition += new Vector3(0f, _scrollSpeed * Time.deltaTime, 0f);
			return;
		}
		_timeFinishShow += Time.deltaTime;
		if (_timeFinishShow > CloseTime)
		{
			Hide();
		}
	}

	public void Show()
	{
		if (!IsOpen)
		{
			IsOpen = true;
			Animator.SetTrigger("Show");
			_currentOpenTime = 0f;
			_timeFinishShow = 0f;
			ScrollObj.transform.localPosition = new Vector3(ScrollObj.transform.localPosition.x, StartPositiony, ScrollObj.transform.localPosition.z);
			UpdateLogo();
		}
	}

	public void Hide()
	{
		IsOpen = false;
		Animator.SetTrigger("Hide");
	}

	public void SetIgnoreSpeedUp(bool isIgnore)
	{
		ignoreSpeedUp = isIgnore;
	}
}
