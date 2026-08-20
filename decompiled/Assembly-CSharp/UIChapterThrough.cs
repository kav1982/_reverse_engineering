using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[GameUISingletonPrefab("UIChapterThrough")]
public class UIChapterThrough : GameUISingletonMono<UIChapterThrough>, IPointerClickHandler, IEventSystemHandler
{
	[Serializable]
	public struct PortraitPointer<T>
	{
		public T type;

		public Sprite[] difficultyIcons;
	}

	private enum UIState
	{
		Idle,
		Show,
		ShowWait,
		Move,
		MoveFinish,
		Hiding,
		ButtonShow,
		ButtonWait,
		ButtonHide
	}

	public CanvasGroup canvasGroupDownUI;

	public RectTransform[] rtsf_Chapters;

	public RectTransform rtsf_Location;

	public Animator anima_Self;

	public Animator anima_Player;

	public float showWaitTime;

	public float showWaitTimeDontMove;

	public float moveSpeed;

	public float moveFinishTime;

	public Image Portrait;

	public PortraitPointer<PlayerLook>[] portraitPointerList = new PortraitPointer<PlayerLook>[0];

	private Dictionary<PlayerLook, Sprite[]> portraitPointers;

	[Header("暖雪头像")]
	public Sprite[] portraitWarmSnowPointers;

	[Header("背包乱斗头像")]
	public Sprite[] portraitReaperPointers;

	[Header("黄老饼头像")]
	public Sprite[] portraitDefaultPointers;

	[Header("戴夫头像")]
	public Sprite[] portraitDavetPointers;

	[Header("Difficulty")]
	public RectTransform rtsf_BtnNormal;

	public RectTransform rtsf_BtnHard;

	public RectTransform rtsf_BtnNightmare;

	public RectTransform rtsf_BtnNightmare1;

	public RectTransform rtsf_BtnNightmare2;

	public RectTransform rtsf_BtnNightmare3;

	public RectTransform rtsf_DifficultySelected;

	public RectTransform rtsf_Buttons;

	public RectTransform text_DifficultyDescBG;

	public float difficultyDescBGTarPosYResset;

	public float difficultyDescBGTarPosYDifficulty0;

	public float difficultyDescBGTarPosY;

	public Text textSelectDifficulty;

	public Text text_DifficultyDesc_P;

	public Text text_DifficultyDesc_N;

	public RectTransform rtsf_Motion;

	public RectTransform rtsf_BG2_Right;

	public CanvasGroup cg_Location4;

	public CanvasGroup cg_Location5;

	public float normalOffsetX;

	public float normalBG2RightX;

	public float hardMotionX;

	public float hardBG2RightX;

	public float nightmareMotionX;

	public float nightmareBG2RightX;

	public float difficultyMoveSpeed;

	public float locationAlphaChangeSpeed;

	public Image imageCloudChapter4;

	public Image imageCloudChapter5;

	private Vector2 cloudChapter4StartPosition;

	public Vector2 cloudChapter4EndPosition;

	private Vector2 cloudChapter5StartPosition;

	public Vector2 cloudChapter5EndPosition;

	public Image BGBlood;

	public Sprite BGBlood1;

	public Sprite BGBlood2;

	public Sprite BGBlood3;

	public Sprite BGBlood1_H;

	public Sprite BGBlood2_H;

	public Sprite BGBlood3_H;

	[Header("Sound")]
	public AudioSource as_Walk;

	[Header("Language")]
	public Text text_Normal;

	public Text text_Hard;

	public Text text_Nightmare;

	public Text text_Nightmare1;

	public Text text_Nightmare2;

	public Text text_Nightmare3;

	public Text ClickAgain;

	public Text textChapter1;

	public Text textChapter2;

	public Text textChapter3;

	public Text textChapter4;

	public Text textChapter5;

	public int tipsCountPC;

	public int tipsCountMobile;

	public Text textTips;

	public int currentTip;

	public UpdatButtonShow[] updatebuttonshows;

	private bool padcontrol;

	public bool CanExit;

	private bool isScrolling;

	private UIState state;

	private int startChapter;

	private bool showSelect;

	private float showWaitTimer;

	private float moveFinishWaitTimer;

	private Action hideFinishAct;

	private DifficultyType selectedDifficulty;

	[Header("手游")]
	public GameObject goCloseButtonMobile;

	public GameObject mobileButtonLeft;

	public GameObject mobileButtonRIght;

	private int tipsTotall
	{
		get
		{
			if (!GameMgr.IsMobile_Static)
			{
				return tipsCountPC;
			}
			return tipsCountMobile;
		}
	}

	public void ActivePadControl()
	{
		padcontrol = true;
	}

	public void DisactivePadControl()
	{
		padcontrol = false;
	}

	protected override void RegistarWhenInit()
	{
		EventMgr.SoundVolumeChange = (Action)Delegate.Combine(EventMgr.SoundVolumeChange, new Action(SoundChange));
		EventMgr.LanguageChange = (Action)Delegate.Combine(EventMgr.LanguageChange, new Action(LanguageChange));
		EventMgr.ControlChange = (Action)Delegate.Combine(EventMgr.ControlChange, new Action(UpdateButtonShow));
		EventMgr.InputChange = (Action)Delegate.Combine(EventMgr.InputChange, new Action(UpdateButtonShow));
	}

	protected override void RegistarOnlyWhenOpen()
	{
		base.inputActions.Player.WASD.performed += GamepadDirectPerformed;
		base.inputActions.Player.GamepadDirect.performed += GamepadDirectPerformed;
		base.inputActions.Player.LeftStick.performed += GamepadDirectPerformed_Stick;
		base.inputActions.Player.Interact.performed += GamepadInteract;
		base.inputActions.Player.GamepadEast.performed += GamepadExit;
	}

	protected override void UnRegistarOnlyWhenHide()
	{
		base.inputActions.Player.WASD.performed -= GamepadDirectPerformed;
		base.inputActions.Player.GamepadDirect.performed -= GamepadDirectPerformed;
		base.inputActions.Player.LeftStick.performed -= GamepadDirectPerformed_Stick;
		base.inputActions.Player.Interact.performed -= GamepadInteract;
		base.inputActions.Player.GamepadEast.performed -= GamepadExit;
	}

	protected override void UnRegistarWhenDestroy()
	{
		EventMgr.SoundVolumeChange = (Action)Delegate.Remove(EventMgr.SoundVolumeChange, new Action(SoundChange));
		EventMgr.LanguageChange = (Action)Delegate.Remove(EventMgr.LanguageChange, new Action(LanguageChange));
		EventMgr.ControlChange = (Action)Delegate.Remove(EventMgr.ControlChange, new Action(UpdateButtonShow));
		EventMgr.InputChange = (Action)Delegate.Remove(EventMgr.InputChange, new Action(UpdateButtonShow));
	}

	private void OnEnable()
	{
		text_DifficultyDescBG.anchoredPosition = new Vector2(text_DifficultyDescBG.anchoredPosition.x, difficultyDescBGTarPosYResset);
		cloudChapter4StartPosition = imageCloudChapter4.transform.localPosition;
		cloudChapter5StartPosition = imageCloudChapter5.transform.localPosition;
		SoundChange();
		LanguageChange();
	}

	protected override IEnumerator OnInit()
	{
		portraitPointers = new Dictionary<PlayerLook, Sprite[]>();
		PortraitPointer<PlayerLook>[] array = portraitPointerList;
		for (int i = 0; i < array.Length; i++)
		{
			PortraitPointer<PlayerLook> portraitPointer = array[i];
			portraitPointers.Add(portraitPointer.type, portraitPointer.difficultyIcons);
		}
		UpdateButtonShow();
		yield return null;
	}

	public void UpdateChangeDifficultyButton()
	{
		mobileButtonRIght.gameObject.SetActive((int)selectedDifficulty < DataMgr.selectedWorldData.finishedDifficulty.Count && selectedDifficulty != DifficultyType.Nightmare3);
		mobileButtonLeft.gameObject.SetActive(selectedDifficulty > DifficultyType.Easy);
	}

	private void UpdateButtonShow()
	{
		UpdatButtonShow[] array = updatebuttonshows;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].UpdateButton();
		}
	}

	private void GamepadExit(InputAction.CallbackContext context)
	{
		if (base.IsOpen && CanExit)
		{
			GameUISingletonMono<UIChapterThrough>.Inst.Hide();
		}
	}

	private void GamepadInteract(InputAction.CallbackContext context)
	{
		_InteractPerformed();
	}

	public void _InteractPerformed()
	{
		goCloseButtonMobile.SetActive(value: false);
		if (GameMgr.IsMobile_Static)
		{
			UIMgr.TryAdditionalMobileHide(base.transform);
		}
		if (padcontrol && base.IsOpen)
		{
			switch (selectedDifficulty)
			{
			case DifficultyType.Easy:
				_DifficultyClick(0);
				break;
			case DifficultyType.Normal:
				_DifficultyClick(1);
				break;
			case DifficultyType.Hard:
				_DifficultyClick(2);
				break;
			case DifficultyType.Nightmare1:
				_DifficultyClick(3);
				break;
			case DifficultyType.Nightmare2:
				_DifficultyClick(4);
				break;
			case DifficultyType.Nightmare3:
				_DifficultyClick(5);
				break;
			}
		}
	}

	private void GamepadDirectPerformed_Stick(InputAction.CallbackContext context)
	{
		if (padcontrol && base.IsOpen)
		{
			Vector2 vector = context.ReadValue<Vector2>();
			vector = ControlMgr.Inst.RampVector2(vector);
			moveDirect(vector);
		}
	}

	private void GamepadDirectPerformed(InputAction.CallbackContext context)
	{
		if (padcontrol && base.IsOpen)
		{
			Vector2 direct = context.ReadValue<Vector2>();
			moveDirect(direct);
		}
	}

	private void moveDirect(Vector2 _direct)
	{
		if (_direct == Vector2.right)
		{
			_RightPerform();
		}
		else if (_direct == Vector2.left)
		{
			_LeftPerform();
		}
	}

	public void _LeftPerform()
	{
		if (state == UIState.ButtonShow && DataMgr.selectedWorldData.finishedDifficulty.Count != 0 && selectedDifficulty > DifficultyType.Easy)
		{
			selectedDifficulty--;
			_SelectDifficulty((int)selectedDifficulty);
		}
	}

	public void _RightPerform()
	{
		if (state == UIState.ButtonShow && DataMgr.selectedWorldData.finishedDifficulty.Count != 0 && selectedDifficulty != (DifficultyType)(Enum.GetNames(selectedDifficulty.GetType()).Length - 1) && (int)selectedDifficulty < DataMgr.selectedWorldData.finishedDifficulty.Count)
		{
			selectedDifficulty++;
			_SelectDifficulty((int)selectedDifficulty);
		}
	}

	private void SoundChange()
	{
		as_Walk.volume = DataMgr.settingData.GetFinalSound();
	}

	private void LanguageChange()
	{
		text_Normal.text = 1002601.GetText();
		text_Hard.text = 1002602.GetText();
		text_Nightmare.text = 1002603.GetText();
		text_Nightmare1.text = 1002605.GetText();
		text_Nightmare2.text = 1002606.GetText();
		text_Nightmare3.text = 1002607.GetText();
		ClickAgain.text = 1002604.GetText();
		textChapter1.text = 1001702.GetText();
		textChapter2.text = 1001703.GetText();
		textChapter3.text = 1001704.GetText();
		textChapter4.text = 1001705.GetText();
		textChapter5.text = 1001706.GetText();
		textSelectDifficulty.text = 1002621.GetText();
		if (currentTip != 0)
		{
			textTips.text = 1003800.GetText() + currentTip.GetText();
		}
	}

	private void Update()
	{
		MainState();
		DifficultyMove();
		if (Input.GetKeyDown(KeyCode.Space))
		{
			GamepadInteract(default(InputAction.CallbackContext));
		}
	}

	private void MainState()
	{
		switch (state)
		{
		case UIState.Show:
			text_DifficultyDescBG.anchoredPosition = new Vector2(text_DifficultyDescBG.anchoredPosition.x, difficultyDescBGTarPosYResset);
			break;
		case UIState.ShowWait:
			showWaitTimer += Time.unscaledDeltaTime;
			if (showSelect)
			{
				if (showWaitTimer >= showWaitTime)
				{
					showWaitTimer = 0f;
					CanExit = true;
					state = UIState.ButtonShow;
					anima_Self.SetTrigger("ButtonShow");
					rtsf_BtnNightmare.gameObject.SetActive(DataMgr.selectedWorldData.finishedDifficulty.Contains(DifficultyType.Normal));
					rtsf_BtnNightmare1.gameObject.SetActive(DataMgr.selectedWorldData.finishedDifficulty.Contains(DifficultyType.Hard));
					rtsf_BtnNightmare2.gameObject.SetActive(DataMgr.selectedWorldData.finishedDifficulty.Contains(DifficultyType.Nightmare1));
					rtsf_BtnNightmare3.gameObject.SetActive(DataMgr.selectedWorldData.finishedDifficulty.Contains(DifficultyType.Nightmare2));
				}
			}
			else if (showWaitTimer >= showWaitTimeDontMove)
			{
				showWaitTimer = 0f;
				state = UIState.Move;
				anima_Player.SetTrigger("Walk");
				as_Walk.Play();
			}
			break;
		case UIState.Move:
			rtsf_Location.position = Vector3.MoveTowards(rtsf_Location.position, rtsf_Chapters[startChapter + 1].position, moveSpeed * Time.unscaledDeltaTime);
			if (rtsf_Location.position == rtsf_Chapters[startChapter + 1].position)
			{
				state = UIState.MoveFinish;
				anima_Player.SetTrigger("Idle");
				as_Walk.Stop();
			}
			break;
		case UIState.MoveFinish:
			moveFinishWaitTimer += Time.unscaledDeltaTime;
			if (moveFinishWaitTimer >= moveFinishTime)
			{
				moveFinishWaitTimer = 0f;
				state = UIState.Hiding;
				UIMgr.Inst.uiFade.Show(HideFinish);
			}
			break;
		default:
			Debug.LogError(state);
			break;
		case UIState.Idle:
		case UIState.Hiding:
		case UIState.ButtonShow:
		case UIState.ButtonWait:
		case UIState.ButtonHide:
			break;
		}
	}

	private void DifficultyMove()
	{
		if (!base.IsOpen)
		{
			return;
		}
		Vector2 vector = Vector2.zero;
		Vector2 vector2 = Vector2.zero;
		Vector2 zero = Vector2.zero;
		float num = 0f;
		float num2 = 0f;
		if (selectedDifficulty > DifficultyType.Easy)
		{
			zero = text_DifficultyDescBG.anchoredPosition;
			zero.y = difficultyDescBGTarPosY;
		}
		else
		{
			zero = text_DifficultyDescBG.anchoredPosition;
			zero.y = difficultyDescBGTarPosYDifficulty0;
		}
		switch (selectedDifficulty)
		{
		case DifficultyType.Easy:
			vector = new Vector2(normalOffsetX, rtsf_Motion.anchoredPosition.y);
			vector2 = new Vector2(normalBG2RightX, rtsf_BG2_Right.anchoredPosition.y);
			num = 0f;
			num2 = 0f;
			break;
		case DifficultyType.Normal:
			vector = new Vector2(hardMotionX, rtsf_Motion.anchoredPosition.y);
			vector2 = new Vector2(hardBG2RightX, rtsf_BG2_Right.anchoredPosition.y);
			num = 1f;
			num2 = 0f;
			break;
		case DifficultyType.Hard:
			vector = new Vector2(nightmareMotionX, rtsf_Motion.anchoredPosition.y);
			vector2 = new Vector2(nightmareBG2RightX, rtsf_BG2_Right.anchoredPosition.y);
			num = 1f;
			num2 = 1f;
			break;
		case DifficultyType.Nightmare1:
			vector = new Vector2(nightmareMotionX, rtsf_Motion.anchoredPosition.y);
			vector2 = new Vector2(nightmareBG2RightX, rtsf_BG2_Right.anchoredPosition.y);
			num = 1f;
			num2 = 1f;
			break;
		case DifficultyType.Nightmare2:
			vector = new Vector2(nightmareMotionX, rtsf_Motion.anchoredPosition.y);
			vector2 = new Vector2(nightmareBG2RightX, rtsf_BG2_Right.anchoredPosition.y);
			num = 1f;
			num2 = 1f;
			break;
		case DifficultyType.Nightmare3:
			vector = new Vector2(nightmareMotionX, rtsf_Motion.anchoredPosition.y);
			vector2 = new Vector2(nightmareBG2RightX, rtsf_BG2_Right.anchoredPosition.y);
			num = 1f;
			num2 = 1f;
			break;
		default:
			Debug.LogError(selectedDifficulty);
			break;
		}
		if (rtsf_Motion.anchoredPosition != vector)
		{
			isScrolling = true;
			CanExit = false;
			rtsf_Motion.anchoredPosition = Vector2.MoveTowards(rtsf_Motion.anchoredPosition, vector, difficultyMoveSpeed * Time.unscaledDeltaTime);
		}
		else if (isScrolling)
		{
			isScrolling = false;
			if (state == UIState.ButtonShow)
			{
				CanExit = true;
			}
		}
		rtsf_Buttons.anchoredPosition = Vector2.MoveTowards(rtsf_Buttons.anchoredPosition, new Vector2((0 - selectedDifficulty) * 130 - 60, rtsf_Buttons.anchoredPosition.y), difficultyMoveSpeed * Time.unscaledDeltaTime * 5f);
		if (text_DifficultyDescBG.anchoredPosition != zero && (DataMgr.selectedWorldData.battleData9 == null || DataMgr.selectedWorldData.battleData9.currentLevel == 0))
		{
			text_DifficultyDescBG.anchoredPosition = Vector2.MoveTowards(text_DifficultyDescBG.anchoredPosition, zero, difficultyMoveSpeed * Time.unscaledDeltaTime * 5f);
		}
		if (rtsf_BG2_Right.anchoredPosition != vector2)
		{
			rtsf_BG2_Right.anchoredPosition = Vector2.MoveTowards(rtsf_BG2_Right.anchoredPosition, vector2, difficultyMoveSpeed * Time.unscaledDeltaTime * 2f);
		}
		if (cg_Location4.alpha != num)
		{
			cg_Location4.alpha = Mathf.MoveTowards(cg_Location4.alpha, num, locationAlphaChangeSpeed * Time.unscaledDeltaTime);
		}
		if (cg_Location5.alpha != num2)
		{
			cg_Location5.alpha = Mathf.MoveTowards(cg_Location5.alpha, num2, locationAlphaChangeSpeed * Time.unscaledDeltaTime);
		}
	}

	public void DifficultyMoveImmediate()
	{
		Vector2 vector = Vector2.zero;
		Vector2 vector2 = Vector2.zero;
		float num = 0f;
		float num2 = 0f;
		selectedDifficulty = DataMgr.selectedWorldData.selectedDifficulty;
		switch (selectedDifficulty)
		{
		case DifficultyType.Easy:
			vector = new Vector2(normalOffsetX, rtsf_Motion.anchoredPosition.y);
			vector2 = new Vector2(normalBG2RightX, rtsf_BG2_Right.anchoredPosition.y);
			num = 0f;
			num2 = 0f;
			break;
		case DifficultyType.Normal:
			vector = new Vector2(hardMotionX, rtsf_Motion.anchoredPosition.y);
			vector2 = new Vector2(hardBG2RightX, rtsf_BG2_Right.anchoredPosition.y);
			num = 1f;
			num2 = 0f;
			break;
		case DifficultyType.Hard:
			vector = new Vector2(nightmareMotionX, rtsf_Motion.anchoredPosition.y);
			vector2 = new Vector2(nightmareBG2RightX, rtsf_BG2_Right.anchoredPosition.y);
			num = 1f;
			num2 = 1f;
			break;
		case DifficultyType.Nightmare1:
			vector = new Vector2(nightmareMotionX, rtsf_Motion.anchoredPosition.y);
			vector2 = new Vector2(nightmareBG2RightX, rtsf_BG2_Right.anchoredPosition.y);
			num = 1f;
			num2 = 1f;
			break;
		case DifficultyType.Nightmare2:
			vector = new Vector2(nightmareMotionX, rtsf_Motion.anchoredPosition.y);
			vector2 = new Vector2(nightmareBG2RightX, rtsf_BG2_Right.anchoredPosition.y);
			num = 1f;
			num2 = 1f;
			break;
		case DifficultyType.Nightmare3:
			vector = new Vector2(nightmareMotionX, rtsf_Motion.anchoredPosition.y);
			vector2 = new Vector2(nightmareBG2RightX, rtsf_BG2_Right.anchoredPosition.y);
			num = 1f;
			num2 = 1f;
			break;
		default:
			Debug.LogError(selectedDifficulty);
			break;
		}
		if (rtsf_Motion.anchoredPosition != vector)
		{
			rtsf_Motion.anchoredPosition = vector;
		}
		if (rtsf_BG2_Right.anchoredPosition != vector2)
		{
			rtsf_BG2_Right.anchoredPosition = vector2;
		}
		if (cg_Location4.alpha != num)
		{
			cg_Location4.alpha = num;
		}
		if (cg_Location5.alpha != num2)
		{
			cg_Location5.alpha = num2;
		}
	}

	private void HideFinish()
	{
		CanExit = true;
		SetIsOpen(isOpen: false);
		state = UIState.Idle;
		anima_Self.SetTrigger("HideDirect");
		if (hideFinishAct != null)
		{
			hideFinishAct();
		}
	}

	private void UpdateSelectedDifficulty()
	{
		ChangeLook();
		switch (selectedDifficulty)
		{
		case DifficultyType.Easy:
			rtsf_DifficultySelected.position = rtsf_BtnNormal.position;
			text_DifficultyDesc_P.text = "";
			text_DifficultyDesc_N.text = "";
			imageCloudChapter4.DOFade(1f, 1f);
			imageCloudChapter4.transform.DOLocalMove(cloudChapter4StartPosition, 1f);
			imageCloudChapter5.DOFade(1f, 1f);
			imageCloudChapter5.transform.DOLocalMove(cloudChapter5StartPosition, 1f);
			BGBlood.sprite = null;
			BGBlood.color = Color.clear;
			break;
		case DifficultyType.Normal:
		{
			rtsf_DifficultySelected.position = rtsf_BtnHard.position;
			text_DifficultyDesc_P.text = "◆\u00a0\u200a" + 1002615.GetText().Replace("int1", "40");
			text_DifficultyDesc_N.text = "◆\u00a0\u200a" + 1002616.GetText();
			Text text6 = text_DifficultyDesc_N;
			text6.text = text6.text + "\n◆\u00a0\u200a" + 1002614.GetText().Replace("string1", 1001705.GetText());
			imageCloudChapter4.DOFade(0f, 1f);
			imageCloudChapter4.transform.DOLocalMove(cloudChapter4EndPosition, 1f);
			imageCloudChapter5.DOFade(1f, 1f);
			imageCloudChapter5.transform.DOLocalMove(cloudChapter5StartPosition, 1f);
			BGBlood.sprite = null;
			BGBlood.color = Color.clear;
			break;
		}
		case DifficultyType.Hard:
		{
			rtsf_DifficultySelected.position = rtsf_BtnNightmare.position;
			text_DifficultyDesc_P.text = "◆\u00a0\u200a" + 1002615.GetText().Replace("int1", "80");
			text_DifficultyDesc_N.text = "◆\u00a0\u200a" + 1002617.GetText();
			Text text11 = text_DifficultyDesc_N;
			text11.text = text11.text + "\n◆\u00a0\u200a" + 1002614.GetText().Replace("string1", 1001705.GetText());
			Text text12 = text_DifficultyDesc_N;
			text12.text = text12.text + "\n◆\u00a0\u200a" + 1002614.GetText().Replace("string1", 1001706.GetText());
			imageCloudChapter5.DOFade(0f, 1f);
			imageCloudChapter5.transform.DOLocalMove(cloudChapter5EndPosition, 1f);
			BGBlood.sprite = null;
			BGBlood.color = Color.clear;
			break;
		}
		case DifficultyType.Nightmare1:
		{
			rtsf_DifficultySelected.position = rtsf_BtnNightmare1.position;
			text_DifficultyDesc_P.text = "◆\u00a0\u200a" + 1002615.GetText().Replace("int1", "120");
			Text text7 = text_DifficultyDesc_P;
			text7.text = text7.text + "\n◆\u00a0\u200a" + 1002618.GetText().Replace("int1", "50");
			text_DifficultyDesc_N.text = "◆\u00a0\u200a" + 1002617.GetText();
			Text text8 = text_DifficultyDesc_N;
			text8.text = text8.text + "\n◆\u00a0\u200a" + 1002614.GetText().Replace("string1", 1001705.GetText());
			Text text9 = text_DifficultyDesc_N;
			text9.text = text9.text + "\n◆\u00a0\u200a" + 1002614.GetText().Replace("string1", 1001706.GetText());
			Text text10 = text_DifficultyDesc_N;
			text10.text = text10.text + "\n◆\u00a0\u200a" + 1002611.GetText();
			BGBlood.sprite = (GameMgr.IsHarmony_Static ? BGBlood1_H : BGBlood1);
			BGBlood.color = Color.white;
			break;
		}
		case DifficultyType.Nightmare2:
		{
			rtsf_DifficultySelected.position = rtsf_BtnNightmare2.position;
			text_DifficultyDesc_P.text = "◆\u00a0\u200a" + 1002615.GetText().Replace("int1", "160");
			Text text13 = text_DifficultyDesc_P;
			text13.text = text13.text + "\n◆\u00a0\u200a" + 1002618.GetText().Replace("int1", "50");
			Text text14 = text_DifficultyDesc_P;
			text14.text = text14.text + "\n◆\u00a0\u200a" + 1002619.GetText();
			text_DifficultyDesc_N.text = "◆\u00a0\u200a" + 1002617.GetText();
			Text text15 = text_DifficultyDesc_N;
			text15.text = text15.text + "\n◆\u00a0\u200a" + 1002614.GetText().Replace("string1", 1001705.GetText());
			Text text16 = text_DifficultyDesc_N;
			text16.text = text16.text + "\n◆\u00a0\u200a" + 1002614.GetText().Replace("string1", 1001706.GetText());
			Text text17 = text_DifficultyDesc_N;
			text17.text = text17.text + "\n◆\u00a0\u200a" + 1002612.GetText();
			BGBlood.sprite = (GameMgr.IsHarmony_Static ? BGBlood2_H : BGBlood2);
			BGBlood.color = Color.white;
			break;
		}
		case DifficultyType.Nightmare3:
		{
			rtsf_DifficultySelected.position = rtsf_BtnNightmare3.position;
			text_DifficultyDesc_P.text = "◆\u00a0\u200a" + 1002615.GetText().Replace("int1", "200");
			Text text = text_DifficultyDesc_P;
			text.text = text.text + "\n◆\u00a0\u200a" + 1002618.GetText().Replace("int1", "100");
			Text text2 = text_DifficultyDesc_P;
			text2.text = text2.text + "\n◆\u00a0\u200a" + 1002620.GetText();
			text_DifficultyDesc_N.text = "◆\u00a0\u200a" + 1002617.GetText();
			Text text3 = text_DifficultyDesc_N;
			text3.text = text3.text + "\n◆\u00a0\u200a" + 1002614.GetText().Replace("string1", 1001705.GetText());
			Text text4 = text_DifficultyDesc_N;
			text4.text = text4.text + "\n◆\u00a0\u200a" + 1002614.GetText().Replace("string1", 1001706.GetText());
			Text text5 = text_DifficultyDesc_N;
			text5.text = text5.text + "\n◆\u00a0\u200a" + 1002613.GetText();
			BGBlood.sprite = (GameMgr.IsHarmony_Static ? BGBlood3_H : BGBlood3);
			BGBlood.color = Color.white;
			break;
		}
		default:
			Debug.LogError(selectedDifficulty);
			break;
		}
		UpdateBloodBG();
		UpdateChangeDifficultyButton();
	}

	private void UpdateBloodBG()
	{
		switch (selectedDifficulty)
		{
		case DifficultyType.Easy:
			BGBlood.sprite = null;
			BGBlood.color = Color.clear;
			break;
		case DifficultyType.Normal:
			BGBlood.sprite = null;
			BGBlood.color = Color.clear;
			break;
		case DifficultyType.Hard:
			BGBlood.sprite = null;
			BGBlood.color = Color.clear;
			break;
		case DifficultyType.Nightmare1:
			BGBlood.sprite = (GameMgr.IsChAge14_Static ? BGBlood1_H : BGBlood1);
			BGBlood.color = Color.white;
			break;
		case DifficultyType.Nightmare2:
			BGBlood.sprite = (GameMgr.IsChAge14_Static ? BGBlood2_H : BGBlood2);
			BGBlood.color = Color.white;
			break;
		case DifficultyType.Nightmare3:
			BGBlood.sprite = (GameMgr.IsChAge14_Static ? BGBlood3_H : BGBlood3);
			BGBlood.color = Color.white;
			break;
		default:
			Debug.LogError(selectedDifficulty);
			break;
		}
	}

	public void Show(int startChapter, Action hideFinishAct)
	{
		RegistarOnlyWhenOpen();
		canvasGroupDownUI.alpha = 0f;
		ShowTips();
		CanExit = false;
		if (DataMgr.selectedWorldData.finishedDifficulty.Count == 0)
		{
			ClickAgain.gameObject.SetActive(value: false);
		}
		ChangeLook();
		UpdateBloodBG();
		this.startChapter = startChapter;
		this.hideFinishAct = hideFinishAct;
		showSelect = false;
		SetIsOpen(isOpen: true);
		state = UIState.Show;
		anima_Self.SetTrigger("Show");
		rtsf_Location.position = rtsf_Chapters[startChapter].position;
		MusicMgr.Inst.ForcePlayMusic("");
	}

	public void ShowAndSelect(Action hideFinishAct)
	{
		if (GameMgr.IsMobile_Static)
		{
			UIMgr.TryAdditionalMobileShow(base.transform);
		}
		RegistarOnlyWhenOpen();
		canvasGroupDownUI.alpha = 1f;
		ShowTips();
		CanExit = false;
		this.hideFinishAct = hideFinishAct;
		startChapter = 0;
		showSelect = true;
		SetIsOpen(isOpen: true);
		state = UIState.Show;
		anima_Self.SetTrigger("Show");
		rtsf_Location.position = rtsf_Chapters[startChapter].position;
		MusicMgr.Inst.ForcePlayMusic("");
		selectedDifficulty = DataMgr.selectedWorldData.selectedDifficulty;
		UpdateSelectedDifficulty();
		float num = difficultyMoveSpeed;
		float num2 = locationAlphaChangeSpeed;
		difficultyMoveSpeed = 999999f;
		locationAlphaChangeSpeed = 999999f;
		DifficultyMove();
		difficultyMoveSpeed = num;
		locationAlphaChangeSpeed = num2;
	}

	public void HideDirect()
	{
		UnRegistarOnlyWhenHide();
		SetIsOpen(isOpen: false);
		CanExit = false;
		state = UIState.Idle;
		anima_Self.SetTrigger("HideDirect");
		as_Walk.Stop();
	}

	public void TryHide()
	{
		Hide();
	}

	public override void Hide()
	{
		if (base.IsOpen && CanExit)
		{
			UnRegistarOnlyWhenHide();
			OnHide();
		}
	}

	protected override void OnHide()
	{
		CanExit = false;
		anima_Self.SetTrigger("Hide");
		as_Walk.Stop();
		MusicMgr.Inst.UpdateCampBGM();
		if (GameMgr.IsMobile_Static)
		{
			UIMgr.TryAdditionalMobileHide(base.transform);
		}
	}

	public void EventSetClosed()
	{
		SetIsOpen(isOpen: false);
		CanExit = false;
		state = UIState.Idle;
		PlayerMgr.Inst.PlayerCtrller.StartMotion();
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (state == UIState.MoveFinish)
		{
			state = UIState.Hiding;
			UIMgr.Inst.uiFade.Show(HideFinish);
		}
	}

	private void ShowTips()
	{
		int num = 0;
		int num2 = 0;
		do
		{
			num2++;
			if (num2 >= 100)
			{
				Debug.LogError("SpecialObj40死循环");
				break;
			}
			num = ((!GameMgr.IsMobile_Static) ? (UnityEngine.Random.Range(0, tipsTotall) + 1003801) : (UnityEngine.Random.Range(0, tipsTotall) + 1003901));
		}
		while (num == currentTip);
		currentTip = num;
		textTips.text = 1003800.GetText() + currentTip.GetText();
	}

	private void _ShowFinish()
	{
		state = UIState.ShowWait;
	}

	private void _ButtonHideFinish()
	{
		state = UIState.Move;
		anima_Player.SetTrigger("Walk");
		as_Walk.Play();
	}

	public void _DifficultyClick(int difficulty)
	{
		if (selectedDifficulty == (DifficultyType)difficulty)
		{
			CanExit = false;
			state = UIState.ButtonHide;
			anima_Self.SetTrigger("ButtonHide");
		}
		else
		{
			selectedDifficulty = (DifficultyType)difficulty;
			DataMgr.selectedWorldData.selectedDifficulty = selectedDifficulty;
			UpdateSelectedDifficulty();
			ClickAgain.gameObject.SetActive(value: true);
		}
		SEMgr.Inst.uiClick.PlaySE();
	}

	private void _SelectDifficulty(int difficulty)
	{
		DifficultyType difficultyType = (selectedDifficulty = (DifficultyType)difficulty);
		DataMgr.selectedWorldData.selectedDifficulty = selectedDifficulty;
		UpdateSelectedDifficulty();
		SEMgr.Inst.uiClick.PlaySE();
	}

	private void ChangeLook()
	{
		WorldData selectedWorldData = DataMgr.selectedWorldData;
		Sprite[] value;
		if (selectedWorldData.selectedSetID == 7)
		{
			value = portraitWarmSnowPointers;
		}
		else if (selectedWorldData.selectedSetID == 8)
		{
			value = portraitReaperPointers;
		}
		else if (selectedWorldData.selectedSetID == 9)
		{
			value = portraitDefaultPointers;
		}
		else if (selectedWorldData.IsDave)
		{
			value = portraitDavetPointers;
		}
		else if (!portraitPointers.TryGetValue(DataMgr.selectedWorldData.playerLook, out value) || value.Length <= (int)selectedDifficulty)
		{
			value = portraitPointers[PlayerLook.Default];
		}
		Portrait.sprite = value[(int)selectedDifficulty];
		Portrait.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(Portrait.sprite.texture.width / 128 * 80, Portrait.sprite.texture.height / 128 * 80);
	}
}
