using System;
using System.Collections;
using DG.Tweening;
using Steamworks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[GameUISingletonPrefab("UIEntryLanguage")]
public class UIEntryLanguage : GameUISingletonMono<UIEntryLanguage>
{
	public Animator anima;

	public Button[] btn_Languages;

	public RectTransform rtsf_LanguageLocator;

	private Button btn_Chinese;

	[Header("LanguageChange")]
	public Text text_ChooseLanguage;

	public Text keyboardRecommanded;

	public Text Warning1;

	public Text Warning2;

	public Text SafemodeDes;

	public Text SafeModeYes;

	public Text SafeModeNo;

	public bool interactLanguage = true;

	public bool interactSafeMode;

	public GameObject goSafemodeSlectionFrameYes;

	public GameObject goSafemodeSlectionFrameNo;

	public CanvasGroup canvasGroupSafeMode;

	public GameObject safemodeShow1;

	public GameObject safemodeShow2;

	protected override IEnumerator OnInit()
	{
		LanguageChange();
		yield return null;
	}

	private void InputChange()
	{
		switch (UIMgr.Inst.InputType)
		{
		case PlayerInputType.Keyboard:
			goSafemodeSlectionFrameYes.SetActive(value: false);
			goSafemodeSlectionFrameNo.SetActive(value: false);
			break;
		default:
			Debug.LogError(UIMgr.Inst.InputType);
			break;
		case PlayerInputType.Gamepad:
			break;
		}
		_SafeMode2ZoomOut();
		_SafeMode1ZoomOut();
	}

	protected override void RegistarWhenInit()
	{
		EventMgr.LanguageChange = (Action)Delegate.Combine(EventMgr.LanguageChange, new Action(LanguageChange));
		EventMgr.InputChange = (Action)Delegate.Combine(EventMgr.InputChange, new Action(InputChange));
	}

	protected override void RegistarOnlyWhenOpen()
	{
		base.inputActions.Player.GamepadDirect.performed += GamepadDirectPerformed;
		base.inputActions.Player.LeftStick.performed += GamepadDirectPerformed_Stick;
		base.inputActions.Player.Interact.performed += InteractPerformed;
	}

	protected override void UnRegistarOnlyWhenHide()
	{
		base.inputActions.Player.GamepadDirect.performed -= GamepadDirectPerformed;
		base.inputActions.Player.LeftStick.performed -= GamepadDirectPerformed_Stick;
		base.inputActions.Player.Interact.performed -= InteractPerformed;
	}

	protected override void UnRegistarWhenDestroy()
	{
		EventMgr.LanguageChange = (Action)Delegate.Remove(EventMgr.LanguageChange, new Action(LanguageChange));
		EventMgr.InputChange = (Action)Delegate.Remove(EventMgr.InputChange, new Action(InputChange));
	}

	private void LanguageChange()
	{
		text_ChooseLanguage.text = 1000302.GetText();
		keyboardRecommanded.text = 1000303.GetText();
		Warning1.text = 1003001.GetText();
		Warning2.text = 1003002.GetText();
		SafemodeDes.text = 1003003.GetFormatText();
		SafeModeYes.text = 1000208.GetText();
		SafeModeNo.text = 1000209.GetText();
	}

	private void GamepadDirectPerformed(InputAction.CallbackContext context)
	{
		if (UIMgr.Inst.InputType == PlayerInputType.Gamepad)
		{
			Vector2 direct = context.ReadValue<Vector2>();
			moveDirect(direct);
		}
	}

	private void GamepadDirectPerformed_Stick(InputAction.CallbackContext context)
	{
		if (UIMgr.Inst.InputType == PlayerInputType.Gamepad)
		{
			Vector2 vector = context.ReadValue<Vector2>();
			vector = ControlMgr.Inst.RampVector2(vector);
			moveDirect(vector);
		}
	}

	private void moveDirect(Vector2 _direct)
	{
		if (interactLanguage)
		{
			if (_direct == new Vector2(0f, 1f))
			{
				btn_Languages[(int)DataMgr.settingData.language].animator.SetTrigger("Normal");
				_ChooseLanguage(DataMgr.settingData.GetPreviousLanguageIndex());
				btn_Languages[(int)DataMgr.settingData.language].animator.SetTrigger("Highlighted");
			}
			else if (_direct == new Vector2(0f, -1f))
			{
				btn_Languages[(int)DataMgr.settingData.language].animator.SetTrigger("Normal");
				_ChooseLanguage(DataMgr.settingData.GetNextLanguageIndex());
				btn_Languages[(int)DataMgr.settingData.language].animator.SetTrigger("Highlighted");
			}
		}
		else if (canvasGroupSafeMode.alpha == 1f && !interactLanguage)
		{
			if (_direct == new Vector2(-1f, 0f))
			{
				interactSafeMode = true;
				goSafemodeSlectionFrameNo.SetActive(value: true);
				goSafemodeSlectionFrameYes.SetActive(value: false);
				_SafeMode1ZoomIn();
				_SafeMode2ZoomOut();
			}
			else if (_direct == new Vector2(1f, 0f))
			{
				interactSafeMode = true;
				goSafemodeSlectionFrameNo.SetActive(value: false);
				goSafemodeSlectionFrameYes.SetActive(value: true);
				_SafeMode2ZoomIn();
				_SafeMode1ZoomOut();
			}
		}
	}

	private void InteractPerformed(InputAction.CallbackContext context)
	{
		if (interactLanguage)
		{
			_ChooseLanguage((int)DataMgr.settingData.language);
		}
		else if (canvasGroupSafeMode.alpha == 1f && interactSafeMode)
		{
			if (goSafemodeSlectionFrameYes.activeSelf)
			{
				_OnClickSafeModeYes();
			}
			else
			{
				_OnClickSafeModeNo();
			}
		}
	}

	private void UpdateLanguageLocatorPoint()
	{
		Debug.Log("UpdateLanguageLocatorPoint");
		rtsf_LanguageLocator.position = btn_Languages[(int)DataMgr.settingData.language].transform.position;
	}

	protected override void OnShow(object obj = null)
	{
		UpdateLanguageLocatorPoint();
		if (SteamManager.Initialized && GameMgr.steamLanguageToGameLanguage.TryGetValue(SteamUtils.GetSteamUILanguage(), out var value) && DataMgr.settingData.language != value)
		{
			_ChooseLanguage((int)value);
		}
		UpdateLanguageLocatorPoint();
		if (GameMgr.IsMobile_Static || ScriptableObjMgr.Inst.testCtrller.publishTesting)
		{
			anima.SetTrigger("ShowMobile");
			StartCoroutine(waitstartgame(0.5f));
		}
		else
		{
			anima.SetTrigger("Show");
		}
		UIPlayerDataMgr.Inst.HideDirect();
	}

	protected override void OnHide()
	{
	}

	public IEnumerator waitstartgame(float time)
	{
		yield return new WaitForSeconds(time);
		UIMgr.Inst.uiFade.Show(delegate
		{
			SceneManager.LoadScene("MainMenu");
			Debug.Log(DataMgr.settingData.language);
		});
	}

	public void _ChooseLanguage(int type)
	{
		Debug.Log((LanguageType)type);
		if (!interactLanguage)
		{
			return;
		}
		if (DataMgr.settingData.language == (LanguageType)type)
		{
			if (DataMgr.settingData.firstCreate)
			{
				DataMgr.settingData.firstCreate = false;
				DataMgr.SaveSettingData();
				UIMgr.Inst.uiSetting._LanguageChange_by_savefile();
				EventMgr.LanguageChange?.Invoke();
				anima.SetTrigger("ShowSafeMode");
				interactLanguage = false;
			}
		}
		else
		{
			DataMgr.settingData.language = (LanguageType)type;
			TextConfig.RegetAllText();
			UpdateLanguageLocatorPoint();
			EventMgr.LanguageChange?.Invoke();
		}
		SEMgr.Inst.uiClick.PlaySE();
	}

	public IEnumerator waitstartgame()
	{
		yield return new WaitForSeconds(10f);
		UIMgr.Inst.uiFade.Show(delegate
		{
			SceneManager.LoadScene("MainMenu");
		});
	}

	public void _OnClickSafeModeYes()
	{
		SEMgr.Inst.uiClick.PlaySE();
		DataMgr.settingData.SafeMode = true;
		anima.SetTrigger("Hide");
		StartCoroutine(waitstartgame());
	}

	public void _OnClickSafeModeNo()
	{
		SEMgr.Inst.uiClick.PlaySE();
		DataMgr.settingData.SafeMode = false;
		anima.SetTrigger("Hide");
		StartCoroutine(waitstartgame());
	}

	public void _SafeMode1ZoomIn()
	{
		SEMgr.Inst.uiButtonHover_Button.PlaySE();
		safemodeShow1.transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.5f);
	}

	public void _SafeMode1ZoomOut()
	{
		safemodeShow1.transform.DOScale(Vector3.one, 0.5f);
	}

	public void _SafeMode2ZoomIn()
	{
		SEMgr.Inst.uiButtonHover_Button.PlaySE();
		safemodeShow2.transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.5f);
	}

	public void _SafeMode2ZoomOut()
	{
		safemodeShow2.transform.DOScale(Vector3.one, 0.5f);
	}
}
