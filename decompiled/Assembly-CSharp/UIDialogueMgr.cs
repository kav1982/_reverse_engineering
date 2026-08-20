using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[GameUISingletonPrefab("UIDialogueMgr")]
public class UIDialogueMgr : GameUISingletonMono<UIDialogueMgr>
{
	public enum HDState
	{
		Hide,
		Show,
		TextAppear,
		TextAppearFinish,
		Change
	}

	[Header("HardDialogue")]
	public Image image_HDPortrait;

	public Text text_HDContent;

	public Text text_Namecard;

	public Animator anima_HD;

	public AnimaEvent animaEvent_HD;

	public float hdAppearSpeed;

	public GameObject go_HDNext;

	public GameObject go_HDNextKeyboard;

	public GameObject go_HDNextGamepad;

	public GameObject go_HDNextGamepad_PS;

	public GameObject go_HDNextMobile;

	public GameObject go_DialogueOptionPrefab;

	public Transform tsfDialogueOptions;

	public CanvasGroup canvasgroupOptions;

	public CanvasGroup canvasgroup_uiDIalogueMgr;

	public Color colorChoosen;

	public Color textColorChoosen;

	public bool backToOptions;

	public Dictionary<int, bool[]> conversationRecord = new Dictionary<int, bool[]>();

	public int optionGamepadSelectIndex;

	private int defaultFontSize;

	private int currentFontSize;

	private Dictionary<Transform, UIDialogueBubble_Soft> sdBubbles = new Dictionary<Transform, UIDialogueBubble_Soft>();

	private Dictionary<Transform, UIDialogueBubble_Middle> mdBubbles = new Dictionary<Transform, UIDialogueBubble_Middle>();

	private new InputActions inputActions;

	private HDState hdState;

	private int hdID;

	private int hdCurrentIndex;

	private string hdTargetText;

	private string hdLastText;

	private float hdAppearCounter;

	private List<int> hdXMLFormatIndexs = new List<int>();

	private List<string> hdXMLFormatStrings = new List<string>();

	private Action<string> act_HDEvent;

	private Action act_HDFinish;

	public new bool IsOpen
	{
		get
		{
			if (canvasgroup_uiDIalogueMgr.alpha == 0f)
			{
				return canvasgroupOptions.alpha != 0f;
			}
			return true;
		}
	}

	public bool IsOptionsOpen
	{
		get
		{
			if (tsfDialogueOptions.gameObject.activeInHierarchy)
			{
				return canvasgroupOptions.alpha != 0f;
			}
			return false;
		}
	}

	private bool IsOptionsOpened
	{
		get
		{
			if (canvasgroupOptions.interactable)
			{
				return canvasgroupOptions.alpha == 1f;
			}
			return false;
		}
	}

	private HardDialogueConfig HDCfg => HardDialogueConfig.dic[hdID];

	public (int, int, HDState) GetCurrentHdInfo()
	{
		return (hdID, hdCurrentIndex, hdState);
	}

	protected override void OnHide()
	{
	}

	protected override IEnumerator OnInit()
	{
		animaEvent_HD.DoAction = HDAnimaEvent;
		InputChange();
		yield return null;
	}

	protected override void RegistarWhenInit()
	{
		inputActions = new InputActions();
		inputActions.Enable();
		EventMgr.InputChange = (Action)Delegate.Combine(EventMgr.InputChange, new Action(InputChange));
		EventMgr.PlayerDead = (Action)Delegate.Combine(EventMgr.PlayerDead, new Action(ClearDialogueRecord));
	}

	protected override void RegistarOnlyWhenOpen()
	{
		inputActions.Player.Interact.performed += InteractPerformed;
		inputActions.Player.Space.performed += SpacePerformed;
		inputActions.Player.Shoot.performed += ShootPerformed;
		inputActions.Player.Pause.performed += StopDialogue;
		inputActions.Player.GamepadEast.performed += StopDialogue;
		inputActions.Player.GamepadDirect.performed += GamepadDirect;
		inputActions.Player.LeftStick.performed += GamepadDirectStick;
	}

	protected override void UnRegistarOnlyWhenHide()
	{
		inputActions.Player.Interact.performed -= InteractPerformed;
		inputActions.Player.Space.performed -= SpacePerformed;
		inputActions.Player.Shoot.performed -= ShootPerformed;
		inputActions.Player.Pause.performed -= StopDialogue;
		inputActions.Player.GamepadEast.performed -= StopDialogue;
		inputActions.Player.GamepadDirect.performed -= GamepadDirect;
		inputActions.Player.LeftStick.performed -= GamepadDirectStick;
	}

	protected override void UnRegistarWhenDestroy()
	{
		inputActions.Disable();
		EventMgr.InputChange = (Action)Delegate.Remove(EventMgr.InputChange, new Action(InputChange));
		EventMgr.PlayerDead = (Action)Delegate.Remove(EventMgr.PlayerDead, new Action(ClearDialogueRecord));
	}

	private void OnEnable()
	{
		defaultFontSize = text_HDContent.fontSize;
	}

	private void GamepadDirect(InputAction.CallbackContext context)
	{
		if (IsOptionsOpened)
		{
			Vector2 dir = context.ReadValue<Vector2>();
			moveDir(dir);
		}
	}

	private void GamepadDirectStick(InputAction.CallbackContext context)
	{
		if (IsOptionsOpened)
		{
			Vector2 vector = context.ReadValue<Vector2>();
			vector = ControlMgr.Inst.RampVector2(vector, ControlMgr.rampType.UpDown);
			moveDir(vector);
		}
	}

	private void moveDir(Vector2 _dir)
	{
		if (_dir == Vector2.down)
		{
			tsfDialogueOptions.GetChild(optionGamepadSelectIndex).GetComponent<UIDialogueOption>().GamepadUnselected();
			if (optionGamepadSelectIndex < tsfDialogueOptions.childCount - 1)
			{
				optionGamepadSelectIndex++;
			}
			tsfDialogueOptions.GetChild(optionGamepadSelectIndex).GetComponent<UIDialogueOption>().GamepadSelected();
		}
		else if (_dir == Vector2.up)
		{
			tsfDialogueOptions.GetChild(optionGamepadSelectIndex).GetComponent<UIDialogueOption>().GamepadUnselected();
			if (optionGamepadSelectIndex > 0)
			{
				optionGamepadSelectIndex--;
			}
			tsfDialogueOptions.GetChild(optionGamepadSelectIndex).GetComponent<UIDialogueOption>().GamepadSelected();
		}
	}

	private void InteractPerformed(InputAction.CallbackContext context)
	{
		if (UIMgr.Inst.InputType == PlayerInputType.Gamepad && canvasgroupOptions.interactable)
		{
			SelectCurrentOption();
		}
		else
		{
			SpacePerformed(context);
		}
	}

	private void SelectCurrentOption()
	{
		tsfDialogueOptions.GetChild(optionGamepadSelectIndex).GetComponent<UIDialogueOption>().OnClick();
	}

	private void ShootPerformed(InputAction.CallbackContext context)
	{
		if (!GameMgr.IsMobile_Static)
		{
			SpacePerformed(context);
		}
	}

	private void ClearDialogueRecord()
	{
		conversationRecord.Clear();
		optionGamepadSelectIndex = 0;
	}

	private void SpacePerformed(InputAction.CallbackContext context)
	{
		if (hdState == HDState.TextAppear)
		{
			hdAppearCounter = 999f;
		}
		if (hdState != HDState.TextAppearFinish)
		{
			return;
		}
		if (hdCurrentIndex >= HDCfg.portraits.Length - 1)
		{
			if (tsfDialogueOptions.transform.childCount != 0 && backToOptions)
			{
				ShowOptions();
				hdState = HDState.Hide;
			}
			else if (HDCfg.endOptions != null)
			{
				CreateAllOptions();
				ShowOptions();
			}
			else
			{
				ClearDialogueRecord();
				hdState = HDState.Hide;
				anima_HD.SetTrigger("Hide");
			}
		}
		else
		{
			hdState = HDState.Change;
			int num = HDCfg.portraits[hdCurrentIndex];
			hdCurrentIndex++;
			if (num == HDCfg.portraits[hdCurrentIndex])
			{
				anima_HD.SetTrigger("ChangeTextOnly");
			}
			else
			{
				anima_HD.SetTrigger("Change");
			}
			ResetHDTargetText();
		}
	}

	private void CreateAllOptions()
	{
		for (int i = 0; i < HDCfg.endOptions.Length; i++)
		{
			int num = HDCfg.endOptions[i];
			UIDialogueOption component = UnityEngine.Object.Instantiate(go_DialogueOptionPrefab, tsfDialogueOptions).GetComponent<UIDialogueOption>();
			component.actionInherit = act_HDFinish;
			component.id = num;
			component.siblingIndex = i;
			try
			{
				component.optionText.text = (HardDialogueConfig.dic[num].textIDs[0] - 1).GetText();
			}
			catch
			{
				Debug.LogError("忘了配置选项的标题?");
				component.optionText.text = HardDialogueConfig.dic[num].textIDs[0].GetText();
			}
			component.returnSibling = HDCfg.canOptionBackSibling[i];
			component.parentID = HDCfg.id;
			hdState = HDState.Hide;
			if (conversationRecord.ContainsKey(HDCfg.id) && conversationRecord[HDCfg.id][i])
			{
				component.background.color = colorChoosen;
				component.optionText.color = textColorChoosen;
			}
		}
		if (HDCfg.canForceStop && !conversationRecord.ContainsKey(HDCfg.id))
		{
			conversationRecord.Add(HDCfg.id, new bool[HDCfg.endOptions.Count()]);
		}
	}

	private void StopDialogue(InputAction.CallbackContext context)
	{
		if (IsOptionsOpen && HDCfg.canForceStop)
		{
			HideOptions();
			anima_HD.SetTrigger("HideDirect");
			OnHideFinish();
			tsfDialogueOptions.DestroyAllChild();
		}
	}

	private void ShowOptions()
	{
		tsfDialogueOptions.gameObject.SetActive(value: true);
		anima_HD.SetTrigger("HideWithoutEvent");
		canvasgroupOptions.DOFade(1f, 0.5f).OnComplete(delegate
		{
			canvasgroupOptions.interactable = true;
		}).SetEase(Ease.Linear);
		if (UIMgr.Inst.InputType == PlayerInputType.Gamepad)
		{
			tsfDialogueOptions.GetChild(optionGamepadSelectIndex).GetComponent<UIDialogueOption>().GamepadSelected();
		}
	}

	public void HideOptions()
	{
		canvasgroupOptions.interactable = false;
		canvasgroupOptions.DOFade(0f, 0.5f).SetEase(Ease.Linear);
		tsfDialogueOptions.gameObject.SetActive(value: false);
	}

	private void InputChange()
	{
		if (GameMgr.IsMobile_Static)
		{
			go_HDNextMobile.SetActive(value: true);
			go_HDNextKeyboard.SetActive(value: false);
			go_HDNextGamepad.SetActive(value: false);
			go_HDNextGamepad_PS.SetActive(value: false);
			return;
		}
		switch (UIMgr.Inst.InputType)
		{
		case PlayerInputType.Keyboard:
			if (GameMgr.IsSteamDeck_Static)
			{
				go_HDNextKeyboard.SetActive(value: false);
				go_HDNextGamepad.SetActive(value: false);
				go_HDNextGamepad_PS.SetActive(value: true);
			}
			else
			{
				go_HDNextKeyboard.SetActive(value: true);
				go_HDNextGamepad.SetActive(value: false);
				go_HDNextGamepad_PS.SetActive(value: false);
			}
			if (tsfDialogueOptions.childCount != 0)
			{
				tsfDialogueOptions.GetChild(optionGamepadSelectIndex).GetComponent<UIDialogueOption>().GamepadUnselected();
			}
			optionGamepadSelectIndex = 0;
			break;
		case PlayerInputType.Gamepad:
			if (ControlMgr.Inst.GetControllerType() == ControlMgr.controllertype.PS)
			{
				go_HDNextKeyboard.SetActive(value: false);
				go_HDNextGamepad.SetActive(value: false);
				go_HDNextGamepad_PS.SetActive(value: true);
			}
			else
			{
				go_HDNextKeyboard.SetActive(value: false);
				go_HDNextGamepad.SetActive(value: true);
				go_HDNextGamepad_PS.SetActive(value: false);
			}
			if (tsfDialogueOptions.childCount != 0)
			{
				tsfDialogueOptions.GetChild(optionGamepadSelectIndex).GetComponent<UIDialogueOption>().GamepadSelected();
			}
			break;
		default:
			Debug.LogError(UIMgr.Inst.InputType);
			break;
		}
	}

	private void Update()
	{
		switch (hdState)
		{
		case HDState.TextAppear:
		{
			hdAppearCounter += hdAppearSpeed * Time.unscaledDeltaTime;
			int num = (int)hdAppearCounter;
			if (num >= hdTargetText.Length)
			{
				num = hdTargetText.Length;
				hdLastText = hdTargetText;
				string text = hdTargetText.Substring(0, num);
				for (int num2 = hdXMLFormatIndexs.Count - 1; num2 >= 0; num2--)
				{
					text = ((text.Length >= hdXMLFormatIndexs[num2]) ? text.Insert(hdXMLFormatIndexs[num2], hdXMLFormatStrings[num2]) : text.Insert(text.Length, hdXMLFormatStrings[num2]));
				}
				text_HDContent.text = text;
				if (num % 2 == 1)
				{
					SEMgr.Inst.dialogueAppearHard.PlaySE();
				}
				hdState = HDState.TextAppearFinish;
				go_HDNext.SetActive(value: true);
			}
			else if (hdLastText.Length != num)
			{
				hdLastText = hdTargetText.Substring(0, num);
				string text2 = hdLastText;
				for (int num3 = hdXMLFormatIndexs.Count - 1; num3 >= 0; num3--)
				{
					text2 = ((text2.Length >= hdXMLFormatIndexs[num3]) ? text2.Insert(hdXMLFormatIndexs[num3], hdXMLFormatStrings[num3]) : text2.Insert(text2.Length, hdXMLFormatStrings[num3]));
				}
				text_HDContent.text = text2;
				if (num % 2 == 1)
				{
					SEMgr.Inst.dialogueAppearHard.PlaySE();
				}
			}
			break;
		}
		default:
			Debug.LogError(hdState);
			break;
		case HDState.Hide:
		case HDState.Show:
		case HDState.TextAppearFinish:
		case HDState.Change:
			break;
		}
	}

	public void UIClickContinue()
	{
		if (GameMgr.IsMobile_Static)
		{
			SpacePerformed(default(InputAction.CallbackContext));
		}
	}

	private void HDAnimaEvent(string animaName)
	{
		switch (animaName)
		{
		case "HideFinish":
			OnHideFinish();
			if (act_HDFinish != null)
			{
				act_HDFinish();
			}
			break;
		case "ShowFinish":
			hdState = HDState.TextAppear;
			ResetSubtitle();
			break;
		case "Change":
		{
			text_HDContent.text = "";
			int portraitID = HDCfg.portraits[hdCurrentIndex];
			ResetName(portraitID);
			ResetPortrait(portraitID);
			break;
		}
		case "ChangeFinish":
			hdState = HDState.TextAppear;
			ResetSubtitle();
			if (HDCfg.eventStrs[hdCurrentIndex] != "" && act_HDEvent != null)
			{
				act_HDEvent(HDCfg.eventStrs[hdCurrentIndex]);
			}
			break;
		default:
			Debug.LogError(animaName);
			break;
		}
	}

	private void OnHideFinish()
	{
		EndDialoguePart();
		CamController.Inst.MouseOffsetContinue();
		UnRegistarOnlyWhenHide();
	}

	public static void EndDialoguePart()
	{
		if (PlayerMgr.Inst.PlayerCtrller != null)
		{
			PlayerMgr.Inst.PlayerCtrller.StartMotion();
			PlayerMgr.Inst.InvincibleUnregister();
		}
	}

	private void ResetSubtitle()
	{
		text_HDContent.text = "";
		text_HDContent.fontSize = currentFontSize;
	}

	public UIDialogueBubble_Soft SDShow(int textID, Transform speaker, Action act_DialogueFinish = null)
	{
		return SDShow(textID, speaker, 0f, isFlip: false, act_DialogueFinish);
	}

	public UIDialogueBubble_Soft SDShow(int textID, Transform speaker, float offset, Action act_DialogueFinish = null)
	{
		return SDShow(textID, speaker, offset, isFlip: false, act_DialogueFinish);
	}

	public UIDialogueBubble_Soft SDShow(int textID, Transform speaker, bool isFlip, Action act_DialogueFinish = null)
	{
		return SDShow(textID, speaker, 0f, isFlip, act_DialogueFinish);
	}

	public UIDialogueBubble_Soft SDShow(int textID, Transform speaker, float offset, bool isFlip, Action act_DialogueFinish = null)
	{
		UIDialogueBubble_Soft uIDialogueBubble_Soft = null;
		if (sdBubbles.ContainsKey(speaker))
		{
			uIDialogueBubble_Soft = sdBubbles[speaker];
			uIDialogueBubble_Soft.Initialize(textID, speaker, offset, isFlip, act_DialogueFinish);
		}
		else
		{
			uIDialogueBubble_Soft = ObjPoolMgr.Inst.GetUIGO("Prefabs/UI/UIDialogueBubble_Soft").GetComponent<UIDialogueBubble_Soft>();
			uIDialogueBubble_Soft.Initialize(textID, speaker, offset, isFlip, act_DialogueFinish);
			sdBubbles.Add(speaker, uIDialogueBubble_Soft);
		}
		return uIDialogueBubble_Soft;
	}

	public void SDUnregister(Transform tsf_Speaker)
	{
		if (!sdBubbles.ContainsKey(tsf_Speaker))
		{
			Debug.LogError("!");
			return;
		}
		sdBubbles[tsf_Speaker].gameObject.SetActive(value: false);
		sdBubbles.Remove(tsf_Speaker);
	}

	public UIDialogueBubble_Middle MDShow(int textID, Transform speaker, Action act_DialogueFinish = null)
	{
		return MDShow(textID, speaker, 0f, isYFlip: false, act_DialogueFinish);
	}

	public UIDialogueBubble_Middle MDShow(int textID, Transform speaker, float offset, Action act_DialogueFinish = null)
	{
		return MDShow(textID, speaker, offset, isYFlip: false, act_DialogueFinish);
	}

	public UIDialogueBubble_Middle MDShow(int textID, Transform speaker, bool isYFlip, Action act_DialogueFinish = null)
	{
		return MDShow(textID, speaker, 0f, isYFlip, act_DialogueFinish);
	}

	public UIDialogueBubble_Middle MDShow(int textID, Transform speaker, float offset, bool isYFlip, Action act_DialogueFinish = null)
	{
		UIDialogueBubble_Middle uIDialogueBubble_Middle = null;
		if (mdBubbles.ContainsKey(speaker))
		{
			uIDialogueBubble_Middle = mdBubbles[speaker];
			uIDialogueBubble_Middle.Initialize(textID, speaker, offset, isYFlip, act_DialogueFinish);
		}
		else
		{
			uIDialogueBubble_Middle = ((!GameMgr.IsMobile_Static) ? ObjPoolMgr.Inst.GetUIGO("Prefabs/UI/UIDialogueBubble_MiddleMobile").GetComponent<UIDialogueBubble_Middle>() : ObjPoolMgr.Inst.GetUIGO("Prefabs/UI/UIDialogueBubble_Middle").GetComponent<UIDialogueBubble_Middle>());
			uIDialogueBubble_Middle.Initialize(textID, speaker, offset, isYFlip, act_DialogueFinish);
			mdBubbles.Add(speaker, uIDialogueBubble_Middle);
		}
		return uIDialogueBubble_Middle;
	}

	public void MDUnregister(Transform tsf_Speaker)
	{
		if (!mdBubbles.ContainsKey(tsf_Speaker))
		{
			Debug.LogError("!");
			return;
		}
		mdBubbles[tsf_Speaker].gameObject.SetActive(value: false);
		mdBubbles.Remove(tsf_Speaker);
	}

	private void ResetHDTargetText()
	{
		hdTargetText = HDCfg.textIDs[hdCurrentIndex].GetText();
		hdXMLFormatIndexs.Clear();
		hdXMLFormatStrings.Clear();
		for (int i = 0; i < 20; i++)
		{
			int num = hdTargetText.LastIndexOf('<');
			int num2 = hdTargetText.LastIndexOf('>');
			if (num == -1 || num2 == -1)
			{
				break;
			}
			hdXMLFormatIndexs.Add(num);
			hdXMLFormatStrings.Add(hdTargetText.Substring(num, num2 - num + 1));
			hdTargetText = hdTargetText.Remove(num, num2 - num + 1);
		}
		TextGenerator textGenerator = new TextGenerator();
		TextGenerationSettings generationSettings = text_HDContent.GetGenerationSettings(new Vector2(((RectTransform)text_HDContent.transform).rect.width, ((RectTransform)text_HDContent.transform).rect.height));
		generationSettings.fontSize = defaultFontSize;
		textGenerator = GeneralTool.PreRenderTextInRect(text_HDContent, hdTargetText, generationSettings);
		while (textGenerator.GetPreferredHeight(hdTargetText, generationSettings) > ((RectTransform)text_HDContent.transform).rect.height)
		{
			generationSettings.fontSize--;
			textGenerator = GeneralTool.PreRenderTextInRect(text_HDContent, hdTargetText, generationSettings);
		}
		currentFontSize = generationSettings.fontSize;
		List<int> _hdXMLFormatIndexsTemp;
		List<string> _hdXMLFormatStringsTemp;
		if (GameMgr.IsHarmony_Static)
		{
			_hdXMLFormatIndexsTemp = hdXMLFormatIndexs.Copy();
			_hdXMLFormatStringsTemp = hdXMLFormatStrings.Copy();
			string _hdTargetText2 = hdTargetText;
			ProcessText(ref _hdTargetText2);
			hdTargetText = _hdTargetText2;
			textGenerator = GeneralTool.PreRenderTextInRect(text_HDContent, _hdTargetText2, generationSettings);
			if (textGenerator.GetPreferredHeight(hdTargetText, generationSettings) > ((RectTransform)text_HDContent.transform).rect.height)
			{
				generationSettings.fontSize--;
				currentFontSize = generationSettings.fontSize;
				_hdTargetText2 = hdTargetText;
				ProcessText(ref _hdTargetText2);
				hdTargetText = _hdTargetText2;
			}
			hdXMLFormatIndexs = _hdXMLFormatIndexsTemp.Copy();
			hdXMLFormatStrings = _hdXMLFormatStringsTemp.Copy();
		}
		hdAppearCounter = 0f;
		hdLastText = "";
		go_HDNext.SetActive(value: false);
		void ProcessText(ref string _hdTargetText)
		{
			textGenerator.Populate(_hdTargetText, generationSettings);
			int lineCount = textGenerator.lineCount;
			for (int j = 0; j < lineCount; j++)
			{
				UILineInfo uILineInfo = textGenerator.lines[j];
				if (_hdTargetText.Count() == 0)
				{
					Debug.LogWarning("是不是少了多语言文案");
					break;
				}
				if (!GeneralTool.CharCanLineStart(_hdTargetText[uILineInfo.startCharIdx]) && uILineInfo.startCharIdx != 0)
				{
					int num3 = uILineInfo.startCharIdx;
					int num4 = 0;
					for (int num5 = uILineInfo.startCharIdx - 1; num5 > 0; num5--)
					{
						num4++;
						if (GeneralTool.CharCanLineStart(_hdTargetText[num5]))
						{
							num3 = num5;
							break;
						}
					}
					if (num3 == uILineInfo.startCharIdx)
					{
						Debug.LogError("出错");
					}
					_hdTargetText = _hdTargetText.Insert(num3, "\n");
					textGenerator.Populate(_hdTargetText, generationSettings);
					lineCount = textGenerator.lineCount;
					int num6 = num3;
					for (int num7 = _hdXMLFormatIndexsTemp.Count - 1; num7 >= 0; num7--)
					{
						if (_hdXMLFormatIndexsTemp[num7] <= num6)
						{
							num6 += _hdXMLFormatStringsTemp[num7].Length;
						}
						else if (_hdXMLFormatIndexsTemp[num7] > num6)
						{
							break;
						}
					}
					for (int k = 0; k < _hdXMLFormatIndexsTemp.Count; k++)
					{
						if (_hdXMLFormatIndexsTemp[k] > num6)
						{
							_hdXMLFormatIndexsTemp[k]++;
						}
					}
				}
			}
		}
	}

	public void HDShow(int hdID, Action act_HDFinish = null)
	{
		UIPlayerDataMgr.Inst.UISlotBagExitall();
		if (!backToOptions)
		{
			tsfDialogueOptions.DestroyAllChild();
		}
		HDShow(hdID, null, act_HDFinish);
	}

	public void HDShow(int hdID, Action<string> act_HDEvent, Action act_HDFinish = null)
	{
		this.hdID = hdID;
		this.act_HDEvent = act_HDEvent;
		this.act_HDFinish = act_HDFinish;
		if (PlayerMgr.Inst.PlayerCtrller != null)
		{
			PlayerMgr.Inst.PlayerCtrller.StopMotion();
			PlayerMgr.Inst.InvincibleRegister();
		}
		CamController.Inst.MouseOffsetPause();
		RegistarOnlyWhenOpen();
		if (HDCfg.canForceStop && conversationRecord.ContainsKey(HDCfg.id))
		{
			anima_HD.SetTrigger("HideDirect");
			hdState = HDState.TextAppearFinish;
			hdCurrentIndex = HDCfg.portraits.Length;
			CreateAllOptions();
			tsfDialogueOptions.gameObject.SetActive(value: true);
			canvasgroupOptions.DOFade(1f, 0.5f).OnComplete(delegate
			{
				canvasgroupOptions.interactable = true;
			}).SetEase(Ease.Linear);
			if (UIMgr.Inst.InputType == PlayerInputType.Gamepad)
			{
				tsfDialogueOptions.GetChild(optionGamepadSelectIndex).GetComponent<UIDialogueOption>().GamepadSelected();
			}
		}
		else
		{
			hdState = HDState.Show;
			anima_HD.SetTrigger("Show");
			hdCurrentIndex = 0;
			text_HDContent.text = "";
			int portraitID = HDCfg.portraits[hdCurrentIndex];
			ResetName(portraitID);
			ResetPortrait(portraitID);
			ResetHDTargetText();
		}
	}

	public void HDShowCommon(int hdIDl, Action act_HDFinish = null)
	{
		GameUISingletonMono<UIDialogueMgr>.Inst.HDShow(hdIDl, null, delegate
		{
			act_HDFinish?.Invoke();
			CamController.Inst.FocusRecover(0f);
		});
	}

	public void ResetName(int portraitID)
	{
		switch (portraitID)
		{
		case 100:
			text_Namecard.text = 1005010.GetText();
			break;
		case 101:
			text_Namecard.text = 1005011.GetText();
			break;
		case 200:
			text_Namecard.text = 1005020.GetText();
			break;
		case 201:
			text_Namecard.text = 1005021.GetText();
			break;
		case 301:
			text_Namecard.text = 1005031.GetText();
			break;
		case 401:
			text_Namecard.text = 1005041.GetText();
			break;
		case 501:
			text_Namecard.text = 1005051.GetText();
			break;
		case 601:
			text_Namecard.text = 1005061.GetText();
			break;
		case 701:
			text_Namecard.text = 1005071.GetText();
			break;
		case 801:
			text_Namecard.text = 1005081.GetText();
			break;
		case 901:
			text_Namecard.text = 1005091.GetText();
			break;
		case 9901:
		case 9902:
		case 9903:
			text_Namecard.text = 1005002.GetText();
			break;
		case 9991:
			text_Namecard.text = 1005911.GetText();
			break;
		case 9992:
			text_Namecard.text = 1005902.GetText();
			break;
		case 9999:
			text_Namecard.text = 1005003.GetText();
			break;
		default:
			text_Namecard.text = 1005901.GetText();
			break;
		}
	}

	public void ResetPortrait(int portraitID)
	{
		switch (portraitID)
		{
		case 10001:
			if (DataMgr.selectedWorldData.selectedSetID == 7)
			{
				portraitID = 20001;
				break;
			}
			if (DataMgr.selectedWorldData.selectedSetID == 8)
			{
				portraitID = 20011;
				break;
			}
			if (DataMgr.selectedWorldData.selectedSetID == 9)
			{
				portraitID = 20021;
				break;
			}
			if (DataMgr.selectedWorldData.IsDave)
			{
				portraitID = 20031;
				break;
			}
			switch (DataMgr.selectedWorldData.playerLook)
			{
			case PlayerLook.Jojo:
				portraitID = 10011;
				break;
			case PlayerLook.PrettyGril:
				portraitID = 10021;
				break;
			case PlayerLook.TVMan:
				portraitID = 10031;
				break;
			case PlayerLook.Nvliu:
				portraitID = 10041;
				break;
			case PlayerLook.Tomato:
				portraitID = 10051;
				break;
			case PlayerLook.Frog:
				portraitID = 10061;
				break;
			case PlayerLook.Halloween:
				portraitID = 10071;
				break;
			case PlayerLook.TapTap:
				portraitID = 10081;
				break;
			case PlayerLook.HaoYou:
				portraitID = 10091;
				break;
			case PlayerLook.MaoNiang:
				portraitID = 10101;
				break;
			case PlayerLook.XingNan:
				portraitID = 10111;
				break;
			case PlayerLook.Horse:
				portraitID = 10121;
				break;
			case PlayerLook.SummerBoy:
				portraitID = 10131;
				break;
			case PlayerLook.SummerGirl:
				portraitID = 10141;
				break;
			case PlayerLook.SnowMan:
				portraitID = 10151;
				break;
			default:
				Debug.LogError(DataMgr.selectedWorldData.playerLook);
				break;
			case PlayerLook.Default:
				break;
			}
			break;
		case 10002:
			if (DataMgr.selectedWorldData.selectedSetID == 7)
			{
				portraitID = 20002;
				break;
			}
			if (DataMgr.selectedWorldData.selectedSetID == 8)
			{
				portraitID = 20012;
				break;
			}
			if (DataMgr.selectedWorldData.selectedSetID == 9)
			{
				portraitID = 20022;
				break;
			}
			if (DataMgr.selectedWorldData.IsDave)
			{
				portraitID = 20032;
				break;
			}
			switch (DataMgr.selectedWorldData.playerLook)
			{
			case PlayerLook.Jojo:
				portraitID = 10012;
				break;
			case PlayerLook.PrettyGril:
				portraitID = 10022;
				break;
			case PlayerLook.TVMan:
				portraitID = 10032;
				break;
			case PlayerLook.Nvliu:
				portraitID = 10042;
				break;
			case PlayerLook.Tomato:
				portraitID = 10052;
				break;
			case PlayerLook.Frog:
				portraitID = 10062;
				break;
			case PlayerLook.Halloween:
				portraitID = 10072;
				break;
			case PlayerLook.TapTap:
				portraitID = 10082;
				break;
			case PlayerLook.HaoYou:
				portraitID = 10092;
				break;
			case PlayerLook.MaoNiang:
				portraitID = 10102;
				break;
			case PlayerLook.XingNan:
				portraitID = 10112;
				break;
			case PlayerLook.Horse:
				portraitID = 10122;
				break;
			case PlayerLook.SummerBoy:
				portraitID = 10132;
				break;
			case PlayerLook.SummerGirl:
				portraitID = 10142;
				break;
			case PlayerLook.SnowMan:
				portraitID = 10152;
				break;
			default:
				Debug.LogError(DataMgr.selectedWorldData.playerLook);
				break;
			case PlayerLook.Default:
				break;
			}
			break;
		}
		string text = "Textures/Portraits/" + portraitID;
		Sprite sprite = ((GameMgr.CampSkinType != 0) ? ABResources.LoadAsset<Sprite>(text + ((GameMgr.CampSkinType == CampSkinType.Default) ? "" : ("T" + (int)GameMgr.CampSkinType))) : ((!GameMgr.IsHarmony_Static || !GameMgr.IsChAge14_Static) ? ABResources.LoadAsset<Sprite>(text) : ABResources.LoadAsset<Sprite>(text + "H14")));
		if (portraitID == 101 && (bool)CampMgr.Inst && CampMgr.Inst.npc1Vivian.sAnima.initialSkinName == "skin2")
		{
			portraitID = 102;
			sprite = ABResources.LoadAsset<Sprite>("Textures/Portraits/" + portraitID);
		}
		if (sprite == null)
		{
			sprite = ABResources.LoadAsset<Sprite>(text);
		}
		image_HDPortrait.sprite = sprite;
	}

	public void HDHideDirect()
	{
		hdState = HDState.Hide;
		anima_HD.SetTrigger("HideDirect");
	}

	public static GameObject GetHDBubble(Transform targetT)
	{
		GameObject obj = UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/UI/UIDialogueBubble_Hard"));
		obj.SetActive(value: true);
		obj.GetComponent<UIDialogueBubble_Hard>().Initialize(targetT);
		obj.transform.localScale = Vector3.one;
		obj.transform.SetSiblingIndex(0);
		return obj;
	}
}
