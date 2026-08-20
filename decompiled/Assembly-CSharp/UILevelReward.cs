using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PlayerLogger.Events;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[GameUISingletonPrefab("UILevelReward")]
public class UILevelReward : GameUISingletonMono<UILevelReward>
{
	public bool isShowingWand;

	[SerializeField]
	private GameObject pfb_UIRewardWandPC;

	[SerializeField]
	private GameObject pfb_UIRewardWandMobile;

	[SerializeField]
	private GameObject pfb_UIRewardRelicPC;

	[SerializeField]
	private GameObject pfb_UIRewardRelicMobile;

	[SerializeField]
	private GameObject pfb_UIRewardSpellPC;

	[SerializeField]
	private GameObject pfb_UIRewardSpellMobile;

	public Image UIRewardLight;

	public CanvasGroup canvasGroup;

	public Transform tsf_UIParent;

	public Animator anima;

	public Text text_Title;

	public RectTransform reward_background;

	public float widthoffset;

	public float wandSpace;

	[SerializeField]
	private float[] relicSpacesPC;

	[SerializeField]
	private float[] relicSpacesMobile;

	public float[] spellSpaces;

	[Header("RerollRelic")]
	public Button btn_RerollRelic;

	public Button btn_RerollSpell;

	public Text text_RerollShow;

	public Text text_RerollTime;

	public GameObject Gamepad_RerollShow;

	[Header("ControlChangeUI")]
	public UpdatButtonShow[] updatebuttonshows;

	[Header("\ufffd\ufffd\ufffdδ\ufffd\ufffd\ufffd")]
	private int selected = -1;

	public LayoutGroup Layout;

	private float mobileWandPositionY = 180f;

	private List<UIRewardBase> uiRewardBases = new List<UIRewardBase>();

	private int hoverIndex;

	private int selectSpellIndex = -1;

	private bool lockRoll;

	private readonly List<int> selectedRelicIds = new List<int>();

	public Text mobileDoubleClickDes;

	private EntityManager ettMgr;

	private Entity levelRewardEtt;

	public LevelRewardType type { get; set; }

	private GameObject pfb_UIRewardWand
	{
		get
		{
			if (!GameMgr.IsMobile_Static)
			{
				return pfb_UIRewardWandPC;
			}
			return pfb_UIRewardWandMobile;
		}
	}

	private GameObject pfb_UIRewardRelic
	{
		get
		{
			if (!GameMgr.IsMobile_Static)
			{
				return pfb_UIRewardRelicPC;
			}
			return pfb_UIRewardRelicMobile;
		}
	}

	private GameObject pfb_UIRewardSpell
	{
		get
		{
			if (!GameMgr.IsMobile_Static)
			{
				return pfb_UIRewardSpellPC;
			}
			return pfb_UIRewardSpellMobile;
		}
	}

	private float[] relicSpaces
	{
		get
		{
			if (!GameMgr.IsMobile_Static)
			{
				return relicSpacesPC;
			}
			return relicSpacesMobile;
		}
	}

	protected override void RegistarWhenInit()
	{
		EventMgr.InputChange = (Action)Delegate.Combine(EventMgr.InputChange, new Action(InputChange));
		EventMgr.ControlChange = (Action)Delegate.Combine(EventMgr.ControlChange, new Action(ControlChange));
		EventMgr.LanguageChange = (Action)Delegate.Combine(EventMgr.LanguageChange, new Action(LanguageChange));
	}

	protected override void RegistarOnlyWhenOpen()
	{
		base.inputActions.Player.GamepadDirect.performed += GamepadDirectPerformed;
		base.inputActions.Player.LeftStick.performed += GamepadDirectPerformed_Stick;
		base.inputActions.Player.Interact.performed += InteractPerformed;
		base.inputActions.Player.GamepadWest.performed += GamePadWestPerformed;
	}

	protected override void UnRegistarOnlyWhenHide()
	{
		base.inputActions.Player.GamepadDirect.performed -= GamepadDirectPerformed;
		base.inputActions.Player.Interact.performed -= InteractPerformed;
		base.inputActions.Player.LeftStick.performed -= GamepadDirectPerformed_Stick;
		base.inputActions.Player.GamepadWest.performed -= GamePadWestPerformed;
	}

	protected override void UnRegistarWhenDestroy()
	{
		EventMgr.InputChange = (Action)Delegate.Remove(EventMgr.InputChange, new Action(InputChange));
		EventMgr.ControlChange = (Action)Delegate.Remove(EventMgr.ControlChange, new Action(ControlChange));
		EventMgr.LanguageChange = (Action)Delegate.Remove(EventMgr.LanguageChange, new Action(LanguageChange));
	}

	private void OnEnable()
	{
		ControlChange();
		GeneralTool.InitialImageMaterial(UIRewardLight);
	}

	private void Update()
	{
		if (UIRewardLight != null)
		{
			UIRewardLight.material.SetFloat("_Progress", UIRewardLight.material.GetFloat("_Progress") + Time.unscaledDeltaTime);
		}
	}

	private void GamepadDirectPerformed(InputAction.CallbackContext context)
	{
		if (UIMgr.Inst.InputType == PlayerInputType.Gamepad && base.IsOpen)
		{
			Vector2 direct = context.ReadValue<Vector2>();
			MoveDirect(direct);
		}
	}

	private void GamepadDirectPerformed_Stick(InputAction.CallbackContext context)
	{
		if (UIMgr.Inst.InputType == PlayerInputType.Gamepad && base.IsOpen)
		{
			Vector2 vector = context.ReadValue<Vector2>();
			vector = ControlMgr.Inst.RampVector2(vector);
			MoveDirect(vector);
		}
	}

	private void MoveDirect(Vector2 _direct, int endAt = -1)
	{
		if (levelRewardEtt == Entity.Null || !ettMgr.HasComponent<LevelReward>(levelRewardEtt))
		{
			return;
		}
		LevelReward componentData = ettMgr.GetComponentData<LevelReward>(levelRewardEtt);
		DynamicBuffer<LevelRewardInfoBED> buffer = ettMgr.GetBuffer<LevelRewardInfoBED>(levelRewardEtt);
		if (_direct == Vector2.down)
		{
			if (componentData.rewardType == LevelRewardType.Wand)
			{
				SelectNextWand();
			}
			else if (componentData.rewardType == LevelRewardType.Spell && !lockRoll && DataMgr.selectedWorldData.ActivateGirlHaveSpellLock())
			{
				lockRoll = true;
				UIRewardSpell obj = (UIRewardSpell)uiRewardBases[hoverIndex];
				obj.bgHover.OnPointerExit(null);
				obj.lockHover.OnPointerEnter(null);
			}
		}
		else if (_direct == Vector2.up)
		{
			if (componentData.rewardType == LevelRewardType.Wand)
			{
				SelectPreviousWand();
			}
			else if (componentData.rewardType == LevelRewardType.Spell && lockRoll && DataMgr.selectedWorldData.ActivateGirlHaveSpellLock())
			{
				lockRoll = false;
				UIRewardSpell obj2 = (UIRewardSpell)uiRewardBases[hoverIndex];
				obj2.bgHover.OnPointerEnter(null);
				obj2.lockHover.OnPointerExit(null);
			}
		}
		else if (_direct == Vector2.left)
		{
			if (componentData.rewardType == LevelRewardType.Relic)
			{
				uiRewardBases[hoverIndex].OnPointerExit(null);
				hoverIndex--;
				if (hoverIndex < 0)
				{
					hoverIndex = uiRewardBases.Count - 1;
				}
				uiRewardBases[hoverIndex].OnPointerEnter(null);
				UIRewardRelic uIRewardRelic = (UIRewardRelic)uiRewardBases[hoverIndex];
				if (uIRewardRelic.Picked)
				{
					if (endAt == -1)
					{
						MoveDirect(Vector2.left, hoverIndex + 1);
					}
					else if (hoverIndex != endAt)
					{
						MoveDirect(Vector2.left, endAt);
					}
				}
				else
				{
					uIRewardRelic.OnPointerEnter(null);
				}
			}
			else if (componentData.rewardType == LevelRewardType.Wand)
			{
				if (GameMgr.IsMobile_Static)
				{
					SelectPreviousWand();
					return;
				}
				if (selectSpellIndex != -1)
				{
					uiRewardBases[hoverIndex].GetComponent<UIRewardWand>().rtsf_Spells.GetChild(selectSpellIndex).GetComponent<UISlotWandExternal>().OnPointerExit(null);
				}
				selectSpellIndex = GetPreviousSpellOfWand();
				if (selectSpellIndex != -1)
				{
					uiRewardBases[hoverIndex].GetComponent<UIRewardWand>().rtsf_Spells.GetChild(selectSpellIndex).GetComponent<UISlotWandExternal>().OnPointerEnter(null);
				}
			}
			else
			{
				if (componentData.rewardType != LevelRewardType.Spell)
				{
					return;
				}
				if (lockRoll)
				{
					((UIRewardSpell)uiRewardBases[hoverIndex]).lockHover.OnPointerExit(null);
				}
				else
				{
					((UIRewardSpell)uiRewardBases[hoverIndex]).bgHover.OnPointerExit(null);
				}
				hoverIndex--;
				if (hoverIndex < 0)
				{
					hoverIndex = uiRewardBases.Count - 1;
				}
				UIRewardSpell uIRewardSpell = (UIRewardSpell)uiRewardBases[hoverIndex];
				if (buffer[hoverIndex].isPicked)
				{
					if (endAt == -1)
					{
						MoveDirect(Vector2.left, hoverIndex + 1);
					}
					else if (hoverIndex != endAt)
					{
						MoveDirect(Vector2.left, endAt);
					}
				}
				else if (lockRoll)
				{
					uIRewardSpell.lockHover.OnPointerEnter(null);
				}
				else
				{
					uIRewardSpell.bgHover.OnPointerEnter(null);
				}
			}
		}
		else
		{
			if (!(_direct == Vector2.right))
			{
				return;
			}
			if (componentData.rewardType == LevelRewardType.Relic)
			{
				uiRewardBases[hoverIndex].OnPointerExit(null);
				hoverIndex++;
				if (hoverIndex >= uiRewardBases.Count)
				{
					hoverIndex = 0;
				}
				uiRewardBases[hoverIndex].OnPointerEnter(null);
				UIRewardRelic uIRewardRelic2 = (UIRewardRelic)uiRewardBases[hoverIndex];
				if (uIRewardRelic2.Picked)
				{
					if (endAt == -1)
					{
						MoveDirect(Vector2.right, hoverIndex - 1);
					}
					else if (hoverIndex != endAt)
					{
						MoveDirect(Vector2.right, endAt);
					}
				}
				else
				{
					uIRewardRelic2.OnPointerEnter(null);
				}
			}
			else if (componentData.rewardType == LevelRewardType.Wand)
			{
				if (GameMgr.IsMobile_Static)
				{
					SelectNextWand();
					return;
				}
				if (selectSpellIndex != -1)
				{
					uiRewardBases[hoverIndex].GetComponent<UIRewardWand>().rtsf_Spells.GetChild(selectSpellIndex).GetComponent<UISlotWandExternal>().OnPointerExit(null);
				}
				selectSpellIndex = GetNextSpellOfWand();
				if (selectSpellIndex != -1)
				{
					uiRewardBases[hoverIndex].GetComponent<UIRewardWand>().rtsf_Spells.GetChild(selectSpellIndex).GetComponent<UISlotWandExternal>().OnPointerEnter(null);
				}
			}
			else
			{
				if (componentData.rewardType != LevelRewardType.Spell)
				{
					return;
				}
				if (lockRoll)
				{
					((UIRewardSpell)uiRewardBases[hoverIndex]).lockHover.OnPointerExit(null);
				}
				else
				{
					((UIRewardSpell)uiRewardBases[hoverIndex]).bgHover.OnPointerExit(null);
				}
				hoverIndex++;
				if (hoverIndex >= uiRewardBases.Count)
				{
					hoverIndex = 0;
				}
				UIRewardSpell uIRewardSpell2 = (UIRewardSpell)uiRewardBases[hoverIndex];
				if (buffer[hoverIndex].isPicked)
				{
					if (endAt == -1)
					{
						MoveDirect(Vector2.right, hoverIndex - 1);
					}
					else if (hoverIndex == endAt)
					{
						((UIRewardSpell)uiRewardBases[hoverIndex]).bgHover.OnPointerEnter(null);
					}
					else
					{
						MoveDirect(Vector2.right, endAt);
					}
				}
				else if (lockRoll)
				{
					uIRewardSpell2.lockHover.OnPointerEnter(null);
				}
				else
				{
					uIRewardSpell2.bgHover.OnPointerEnter(null);
				}
			}
		}
	}

	public void SelectNextWand()
	{
		if (selectSpellIndex != -1)
		{
			uiRewardBases[hoverIndex].GetComponent<UIRewardWand>().rtsf_Spells.GetChild(selectSpellIndex).GetComponent<UISlotWandExternal>().OnPointerExit(null);
		}
		uiRewardBases[hoverIndex].OnPointerExit(null);
		hoverIndex--;
		if (hoverIndex < 0)
		{
			hoverIndex = uiRewardBases.Count - 1;
		}
		uiRewardBases[hoverIndex].OnPointerEnter(null);
		selectSpellIndex = -1;
	}

	public void SelectPreviousWand()
	{
		if (selectSpellIndex != -1)
		{
			uiRewardBases[hoverIndex].GetComponent<UIRewardWand>().rtsf_Spells.GetChild(selectSpellIndex).GetComponent<UISlotWandExternal>().OnPointerExit(null);
		}
		uiRewardBases[hoverIndex].OnPointerExit(null);
		hoverIndex++;
		if (hoverIndex >= uiRewardBases.Count)
		{
			hoverIndex = 0;
		}
		uiRewardBases[hoverIndex].OnPointerEnter(null);
		selectSpellIndex = -1;
	}

	private void InteractPerformed(InputAction.CallbackContext context)
	{
		if (UIMgr.Inst.InputType != PlayerInputType.Gamepad || !base.IsOpen || levelRewardEtt == Entity.Null || !ettMgr.HasComponent<LevelReward>(levelRewardEtt))
		{
			return;
		}
		LevelReward componentData = ettMgr.GetComponentData<LevelReward>(levelRewardEtt);
		if (componentData.rewardType != 0 && componentData.rewardType != LevelRewardType.Relic && componentData.rewardType != LevelRewardType.Spell)
		{
			return;
		}
		switch (componentData.rewardType)
		{
		case LevelRewardType.Wand:
			((UIRewardWand)uiRewardBases[hoverIndex]).OnPointerClick(null);
			break;
		case LevelRewardType.Relic:
			((UIRewardRelic)uiRewardBases[hoverIndex]).OnPointerClick(null);
			break;
		case LevelRewardType.Spell:
		{
			UIRewardSpell uIRewardSpell = (UIRewardSpell)uiRewardBases[hoverIndex];
			if (lockRoll)
			{
				uIRewardSpell.lockHover.OnPointerClick(null);
			}
			else
			{
				uIRewardSpell.bgHover.OnPointerClick(null);
			}
			break;
		}
		default:
			Debug.LogError(componentData.rewardType);
			break;
		}
	}

	private void GamePadWestPerformed(InputAction.CallbackContext context)
	{
		if (levelRewardEtt == Entity.Null || !ettMgr.HasComponent<LevelReward>(levelRewardEtt))
		{
			return;
		}
		LevelReward componentData = ettMgr.GetComponentData<LevelReward>(levelRewardEtt);
		if (base.IsOpen && btn_RerollRelic.gameObject.activeSelf)
		{
			if (componentData.rewardType == LevelRewardType.Relic)
			{
				_RerollRelic();
			}
		}
		else if (base.IsOpen && btn_RerollSpell.gameObject.activeSelf && componentData.rewardType == LevelRewardType.Spell)
		{
			_RerollSpell();
		}
	}

	private void InputChange()
	{
		ControlChange();
		switch (UIMgr.Inst.InputType)
		{
		case PlayerInputType.Keyboard:
			Gamepad_RerollShow.SetActive(value: false);
			break;
		case PlayerInputType.Gamepad:
			Gamepad_RerollShow.SetActive(value: true);
			break;
		default:
			Debug.LogError(UIMgr.Inst.InputType);
			break;
		}
		if (base.IsOpen)
		{
			switch (UIMgr.Inst.InputType)
			{
			case PlayerInputType.Keyboard:
				uiRewardBases[hoverIndex].OnPointerExit(null);
				break;
			case PlayerInputType.Gamepad:
				hoverIndex = 0;
				uiRewardBases[hoverIndex].OnPointerEnter(null);
				Debug.LogWarning("打开");
				break;
			default:
				Debug.LogError(UIMgr.Inst.InputType);
				break;
			}
		}
	}

	private void ControlChange()
	{
		UpdatButtonShow[] array = updatebuttonshows;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].UpdateButton();
		}
	}

	private void UpdateTitleText()
	{
		if (ettMgr.HasComponent<LevelReward>(levelRewardEtt))
		{
			LevelReward componentData = ettMgr.GetComponentData<LevelReward>(levelRewardEtt);
			switch (componentData.rewardType)
			{
			case LevelRewardType.Wand:
				text_Title.text = 1000801.GetText();
				break;
			case LevelRewardType.Relic:
			{
				int num3 = GetMaxRelicPickCount() - componentData.relicPickCounter;
				text_Title.text = 1000803.GetText().Replace("int1", num3.ToString());
				break;
			}
			case LevelRewardType.Spell:
			{
				int num2 = GetMaxSpellPickCount() - componentData.spellPickCounter;
				text_Title.text = 1000802.GetText().Replace("int1", num2.ToString());
				break;
			}
			case LevelRewardType.RuneWizardRune:
			{
				int num = math.clamp(GetMaxSpellPickCount() - componentData.spellPickCounter - 1, 1, 3);
				text_Title.text = 1000802.GetText().Replace("int1", num.ToString());
				break;
			}
			}
		}
	}

	private void LanguageChange()
	{
		UpdateTitleText();
		if (GameMgr.IsMobile_Static && (bool)mobileDoubleClickDes)
		{
			mobileDoubleClickDes.text = 1000911.GetText();
		}
		if (levelRewardEtt == Entity.Null || !ettMgr.HasComponent<LevelReward>(levelRewardEtt))
		{
			return;
		}
		LevelReward componentData = ettMgr.GetComponentData<LevelReward>(levelRewardEtt);
		text_RerollShow.text = 1000804.GetText();
		switch (componentData.rewardType)
		{
		case LevelRewardType.Wand:
			foreach (UIRewardBase uiRewardBasis in uiRewardBases)
			{
				if (uiRewardBasis is UIRewardWand)
				{
					((UIRewardWand)uiRewardBasis).UpdateInfo();
				}
			}
			break;
		case LevelRewardType.Relic:
			foreach (UIRewardBase uiRewardBasis2 in uiRewardBases)
			{
				if (uiRewardBasis2 is UIRewardRelic)
				{
					((UIRewardRelic)uiRewardBasis2).UpdateInfo();
				}
			}
			break;
		case LevelRewardType.Spell:
			foreach (UIRewardBase uiRewardBasis3 in uiRewardBases)
			{
				if (uiRewardBasis3 is UIRewardSpell)
				{
					((UIRewardSpell)uiRewardBasis3).UpdateInfo();
				}
			}
			break;
		default:
			Debug.LogError(componentData.rewardType);
			break;
		}
		StartCoroutine(ChangebackgroundSize());
		if (GameMgr.IsMobile_Static && (bool)mobileDoubleClickDes)
		{
			mobileDoubleClickDes.text = 1000911.GetText();
		}
	}

	private void Awake()
	{
		ettMgr = World.DefaultGameObjectInjectionWorld.EntityManager;
	}

	public override void Show(object obj = null)
	{
		RegistarOnlyWhenOpen();
		OnShow(obj);
	}

	protected override void OnShow(object obj = null)
	{
		if (obj is Entity)
		{
			Entity ett = (Entity)obj;
			StartCoroutine(ShowRewardIE(ett));
		}
	}

	private IEnumerator ShowRewardIE(Entity ett)
	{
		InputChange();
		LanguageChange();
		SetIsOpen(isOpen: true);
		UIMgr.Inst.MoveUpHoverLayer();
		_ = GameMgr.IsMobile_Static;
		UIPlayerDataMgr.Inst.UISlotBagExitall();
		anima.SetTrigger(GameMgr.IsMobile_Static ? "AppearMobile" : "Appear");
		SEMgr.Inst.uiChangeLabel.PlaySE();
		if (levelRewardEtt == ett)
		{
			for (int i = 0; i < uiRewardBases.Count; i++)
			{
				uiRewardBases[i].SetShow();
			}
			yield break;
		}
		selectedRelicIds.Clear();
		levelRewardEtt = ett;
		LevelReward componentData = ettMgr.GetComponentData<LevelReward>(levelRewardEtt);
		DynamicBuffer<LevelRewardInfoBED> buffer = ettMgr.GetBuffer<LevelRewardInfoBED>(levelRewardEtt);
		tsf_UIParent.DestroyAllChild();
		uiRewardBases.Clear();
		btn_RerollRelic.gameObject.SetActive(value: false);
		btn_RerollSpell.gameObject.SetActive(value: false);
		text_RerollTime.gameObject.SetActive(value: false);
		Layout.enabled = false;
		UpdateTitleText();
		switch (componentData.rewardType)
		{
		case LevelRewardType.Wand:
		{
			for (int num14 = 0; num14 < buffer.Length; num14++)
			{
				UIRewardBase component4 = UnityEngine.Object.Instantiate(pfb_UIRewardWand, tsf_UIParent).GetComponent<UIRewardBase>();
				component4.Initialize(levelRewardEtt, num14);
				uiRewardBases.Add(component4);
			}
			List<int> list3 = new List<int>();
			for (int num15 = 0; num15 < buffer.Length; num15++)
			{
				list3.Add(buffer[num15].info.id);
			}
			LevelMgr.Inst.RoomFinishLogger.rewards.Add(new RoomFinishLogger.Reward.Wand
			{
				options = list3
			});
			yield return null;
			float num16 = 0f - wandSpace;
			float num17 = uiRewardBases[0].rtsf_BG.sizeDelta.x * uiRewardBases[0].transform.localScale.x;
			float num18 = uiRewardBases[1].rtsf_BG.sizeDelta.x * uiRewardBases[1].transform.localScale.x;
			float num19 = uiRewardBases[0].rtsf_BG.sizeDelta.y * uiRewardBases[0].transform.localScale.y;
			float num20 = uiRewardBases[1].rtsf_BG.sizeDelta.y * uiRewardBases[1].transform.localScale.y;
			float num21 = ((num17 > num18) ? num17 : num18);
			float num22 = (num16 + (num19 + num20 + wandSpace * 2f)) / 2f;
			if (GameMgr.IsMobile_Static)
			{
				((RectTransform)uiRewardBases[0].transform).anchoredPosition = new Vector2(-250f, mobileWandPositionY);
				((RectTransform)uiRewardBases[1].transform).anchoredPosition = new Vector2(250f, mobileWandPositionY);
				break;
			}
			for (int num23 = 0; num23 < uiRewardBases.Count; num23++)
			{
				RectTransform rectTransform = (RectTransform)uiRewardBases[num23].transform;
				rectTransform.anchoredPosition = new Vector2(0f, num22);
				num22 -= uiRewardBases[num23].rtsf_BG.sizeDelta.y + wandSpace;
				if (rectTransform.sizeDelta.x < num21)
				{
					rectTransform.anchoredPosition = new Vector2((0f - (num21 - rectTransform.sizeDelta.x)) / 2f, rectTransform.anchoredPosition.y);
				}
			}
			break;
		}
		case LevelRewardType.Relic:
		{
			for (int num8 = 0; num8 < buffer.Length; num8++)
			{
				UIRewardBase component3 = UnityEngine.Object.Instantiate(pfb_UIRewardRelic, tsf_UIParent).GetComponent<UIRewardBase>();
				component3.Initialize(levelRewardEtt, num8);
				component3.gameObject.transform.localScale = (GameMgr.IsMobile_Static ? (Vector3.one * 0.8f) : Vector3.one);
				uiRewardBases.Add(component3);
			}
			List<int> list2 = new List<int>();
			for (int num9 = 0; num9 < buffer.Length; num9++)
			{
				list2.Add(buffer[num9].info.id);
			}
			LevelMgr.Inst.RoomFinishLogger.rewards.Add(new RoomFinishLogger.Reward.Relic
			{
				options = list2,
				selected = new List<int>()
			});
			yield return null;
			buffer = ettMgr.GetBuffer<LevelRewardInfoBED>(levelRewardEtt);
			float num10 = ((buffer.Length <= relicSpaces.Length) ? relicSpaces[buffer.Length - 1] : relicSpaces[relicSpaces.Length - 1]);
			float num11 = 0f - num10;
			for (int num12 = 0; num12 < uiRewardBases.Count; num12++)
			{
				num11 += uiRewardBases[num12].rtsf_BG.sizeDelta.x + num10;
			}
			for (int num13 = 0; num13 < uiRewardBases.Count; num13++)
			{
				((RectTransform)uiRewardBases[num13].transform).anchoredPosition = new Vector2((0f - num11) / 2f + uiRewardBases[num13].rtsf_BG.sizeDelta.x / 2f + (uiRewardBases[num13].rtsf_BG.sizeDelta.x + num10) * (float)num13, 0f);
			}
			UpdateRerollTime_Relic();
			break;
		}
		case LevelRewardType.Spell:
		{
			for (int n = 0; n < buffer.Length; n++)
			{
				UIRewardBase component2 = UnityEngine.Object.Instantiate(pfb_UIRewardSpell, tsf_UIParent).GetComponent<UIRewardBase>();
				component2.Initialize(levelRewardEtt, n);
				uiRewardBases.Add(component2);
			}
			List<int> list = new List<int>();
			for (int num3 = 0; num3 < buffer.Length; num3++)
			{
				list.Add(buffer[num3].info.id);
			}
			LevelMgr.Inst.RoomFinishLogger.rewards.Add(new RoomFinishLogger.Reward.Spell
			{
				options = list,
				selected = new List<int>()
			});
			yield return null;
			if (uiRewardBases.Count >= 4 && GameMgr.IsMobile_Static)
			{
				Layout.enabled = true;
			}
			else
			{
				buffer = ettMgr.GetBuffer<LevelRewardInfoBED>(levelRewardEtt);
				float num4 = ((buffer.Length <= spellSpaces.Length) ? spellSpaces[buffer.Length - 1] : spellSpaces[spellSpaces.Length - 1]);
				float num5 = 0f - num4;
				for (int num6 = 0; num6 < uiRewardBases.Count; num6++)
				{
					num5 += uiRewardBases[num6].rtsf_BG.sizeDelta.x + num4;
				}
				for (int num7 = 0; num7 < uiRewardBases.Count; num7++)
				{
					((RectTransform)uiRewardBases[num7].transform).anchoredPosition = new Vector2((0f - num5) / 2f + uiRewardBases[num7].rtsf_BG.sizeDelta.x / 2f + (uiRewardBases[num7].rtsf_BG.sizeDelta.x + num4) * (float)num7, 0f);
				}
			}
			UpdateRerollTime_Spell();
			break;
		}
		case LevelRewardType.RuneWizardRune:
		{
			for (int j = 0; j < buffer.Length; j++)
			{
				UIRewardBase component = UnityEngine.Object.Instantiate(pfb_UIRewardSpell, tsf_UIParent).GetComponent<UIRewardBase>();
				component.Initialize(levelRewardEtt, j);
				uiRewardBases.Add(component);
			}
			List<int> list = new List<int>();
			for (int k = 0; k < buffer.Length; k++)
			{
				list.Add(buffer[k].info.id);
			}
			LevelMgr.Inst.RoomFinishLogger.rewards.Add(new RoomFinishLogger.Reward.Spell
			{
				options = list,
				selected = new List<int>()
			});
			yield return null;
			if (uiRewardBases.Count >= 4 && GameMgr.IsMobile_Static)
			{
				Layout.enabled = true;
				break;
			}
			buffer = ettMgr.GetBuffer<LevelRewardInfoBED>(levelRewardEtt);
			float num = ((buffer.Length <= spellSpaces.Length) ? spellSpaces[buffer.Length - 1] : spellSpaces[spellSpaces.Length - 1]);
			float num2 = 0f - num;
			for (int l = 0; l < uiRewardBases.Count; l++)
			{
				num2 += uiRewardBases[l].rtsf_BG.sizeDelta.x + num;
			}
			for (int m = 0; m < uiRewardBases.Count; m++)
			{
				((RectTransform)uiRewardBases[m].transform).anchoredPosition = new Vector2((0f - num2) / 2f + uiRewardBases[m].rtsf_BG.sizeDelta.x / 2f + (uiRewardBases[m].rtsf_BG.sizeDelta.x + num) * (float)m, 0f);
			}
			break;
		}
		default:
			Debug.LogError(componentData.rewardType);
			break;
		}
		reward_background.sizeDelta = new Vector2(text_Title.preferredWidth + widthoffset, reward_background.sizeDelta.y);
	}

	private IEnumerator ChangebackgroundSize()
	{
		yield return new WaitForSeconds(0.1f);
		reward_background.sizeDelta = new Vector2(text_Title.preferredWidth + widthoffset, reward_background.sizeDelta.y);
	}

	private void UpdateRerollTime_Relic()
	{
		if (!ettMgr.HasComponent<LevelReward>(levelRewardEtt))
		{
			return;
		}
		LevelReward componentData = ettMgr.GetComponentData<LevelReward>(levelRewardEtt);
		btn_RerollRelic.gameObject.SetActive(value: false);
		text_RerollTime.gameObject.SetActive(value: false);
		if (PlayerMgr.Inst.ItemCtrller.relicCfg_RerollRelic != null)
		{
			int num = PlayerMgr.Inst.ItemCtrller.relicCfg_RerollRelic.int1.result - componentData.rerollTimer;
			if (num > 0)
			{
				btn_RerollRelic.gameObject.SetActive(value: true);
				text_RerollTime.gameObject.SetActive(value: true);
				text_RerollTime.text = "×" + num;
			}
		}
	}

	private void UpdateRerollTime_Spell()
	{
		if (levelRewardEtt == Entity.Null || !ettMgr.HasComponent<LevelReward>(levelRewardEtt))
		{
			return;
		}
		LevelReward componentData = ettMgr.GetComponentData<LevelReward>(levelRewardEtt);
		btn_RerollSpell.gameObject.SetActive(value: false);
		text_RerollTime.gameObject.SetActive(value: false);
		if (PlayerMgr.Inst.ItemCtrller.relicCfg_RerollSpell != null)
		{
			int num = PlayerMgr.Inst.ItemCtrller.relicCfg_RerollSpell.int1.result - componentData.rerollTimer;
			if (num > 0)
			{
				btn_RerollSpell.gameObject.SetActive(value: true);
				text_RerollTime.gameObject.SetActive(value: true);
				text_RerollTime.text = "×" + num;
			}
		}
	}

	public void RewardMobileSelect(int index, UIRewardBase rewardSelect)
	{
		if (selected != index)
		{
			if (selected != -1)
			{
				uiRewardBases[selected].UnHover();
			}
			selected = index;
			rewardSelect.Hover();
			Debug.Log("hover " + index);
		}
		else
		{
			rewardSelect.Select();
			selected = -1;
		}
	}

	public void RewardSelect(int index)
	{
		if (!ettMgr.HasComponent<LevelReward>(levelRewardEtt))
		{
			return;
		}
		LevelReward componentData = ettMgr.GetComponentData<LevelReward>(levelRewardEtt);
		DynamicBuffer<LevelRewardInfoBED> buffer = ettMgr.GetBuffer<LevelRewardInfoBED>(levelRewardEtt);
		switch (componentData.rewardType)
		{
		case LevelRewardType.Wand:
		{
			if (GameMgr.IsMobile_Static && type == LevelRewardType.Wand)
			{
				isShowingWand = false;
				UIPlayerDataMgr.Inst.MobileUpdateWandFold();
				UIPlayerDataMgr.Inst.UpdateWandLayout();
				UIPlayerDataMgr.Inst.UpdateBagUiSizeMobile();
				UIPlayerDataMgr.Inst.CanvasWandOnly.sortingOrder = 1;
				UIPlayerDataMgr.Inst.CanvasWandOnly.overrideSorting = false;
			}
			WandConfig _wandConfig = WandConfig.GetConfig(buffer[index].info.id).Copy();
			int pickWandIndex = PlayerMgr.Inst.GetPickWandIndex();
			for (int i = 0; i < buffer.Length; i++)
			{
				if (i != index)
				{
					PlayerMgr.Inst.BaData.BackWandToPool(buffer[i].info.id);
				}
			}
			Vector3 position = UIPlayerDataMgr.Inst.uiWands[pickWandIndex].image_Icon.transform.position;
			Vector2 screenPoint3 = RectTransformUtility.WorldToScreenPoint(null, position);
			RectTransformUtility.ScreenPointToLocalPointInRectangle(UIMgr.Inst.canvas_10.GetComponent<RectTransform>(), screenPoint3, null, out var localPoint3);
			PlayerMgr.Inst.ItemCtrller.UIRewardFly(UIMgr.Inst.canvas_3, _wandConfig.id, RollRewardFly.DropType.Wand, uiRewardBases[index].transform.position + new Vector3(0f, -2f), localPoint3 + new Vector2(-10f, -10f), null, useParticleColor: true, delegate
			{
				PlayerMgr.Inst.WandPickUp(_wandConfig);
			}, isUI: true, dropOnEnd: false, null, CamController.Inst.cam_UI);
			try
			{
				((RoomFinishLogger.Reward.Wand)LevelMgr.Inst.RoomFinishLogger.rewards.Find((RoomFinishLogger.Reward reward) => reward.type == "Wands")).selected = buffer[index].info.id;
			}
			catch (Exception value)
			{
				Debug.LogError("法杖打点报错了!叫韩永宁来瞅瞅");
				Console.WriteLine(value);
			}
			break;
		}
		case LevelRewardType.Relic:
		{
			PlayerMgr.Inst.PlayerCtrller.TextFloatQueueAdd(1002202.GetText() + ":" + RelicConfig.dic[buffer[index].info.id].GetName(haveLevel: false), UITextFloatType.Normal);
			PlayerMgr.Inst.ItemCtrller.AddRewardFly(buffer[index].info.id, RollRewardFly.DropType.Relic, ((UIRewardRelic)uiRewardBases[index]).image_Icon.transform.position, CamController.Inst.cam_UI);
			RoomFinishLogger.Reward.Relic relic = (RoomFinishLogger.Reward.Relic)LevelMgr.Inst.RoomFinishLogger.rewards.Find((RoomFinishLogger.Reward reward) => reward.type == "Relics");
			relic.selected.Add(buffer[index].info.id);
			if (PlayerMgr.Inst.ItemCtrller.relicCfg_RerollRelic != null)
			{
				int num3 = (relic.remainRerollTime = PlayerMgr.Inst.ItemCtrller.relicCfg_RerollRelic.int1.result - componentData.rerollTimer);
			}
			relic.rerollTime = componentData.rerollTimer;
			componentData.relicPickCounter++;
			selectedRelicIds.Add(buffer[index].info.id);
			if (buffer[index].info.id != 69 && componentData.relicPickCounter < GetMaxRelicPickCount())
			{
				ettMgr.SetComponentData(levelRewardEtt, componentData);
				UpdateTitleText();
				return;
			}
			break;
		}
		case LevelRewardType.Spell:
		{
			Vector3 worldPoint2 = ((!GameMgr.IsMobile_Static) ? UIPlayerDataMgr.Inst.image_BagBtn.transform.position : UIPlayerDataMgr.Inst.image_BagBtn.transform.position);
			Vector2 screenPoint2 = RectTransformUtility.WorldToScreenPoint(null, worldPoint2);
			RectTransformUtility.ScreenPointToLocalPointInRectangle(UIMgr.Inst.canvas_10.GetComponent<RectTransform>(), screenPoint2, null, out var localPoint2);
			int _id = buffer[index].info.id;
			PlayerMgr.Inst.ItemCtrller.UIRewardFly(UIMgr.Inst.canvas_3, _id, RollRewardFly.DropType.Spell, ((UIRewardSpell)uiRewardBases[index]).image_Icon.transform.position, localPoint2 + new Vector2(-10f, -10f), null, useParticleColor: true, delegate
			{
				PlayerMgr.Inst.SpellPick(new SlotData(_id));
				UIPlayerDataMgr.Inst.BagShakeButton();
			}, isUI: true, dropOnEnd: false, null, CamController.Inst.cam_UI);
			SEMgr.Inst.uiLevelRewardPickSpell.PlaySE();
			RoomFinishLogger.Reward.Spell spell = (RoomFinishLogger.Reward.Spell)LevelMgr.Inst.RoomFinishLogger.rewards.Find((RoomFinishLogger.Reward reward) => reward.type == "Spells");
			spell.selected.Add(buffer[index].info.id);
			if (PlayerMgr.Inst.ItemCtrller.relicCfg_RerollSpell != null)
			{
				int num2 = (spell.remainRerollTime = PlayerMgr.Inst.ItemCtrller.relicCfg_RerollSpell.int1.result - componentData.rerollTimer);
			}
			spell.rerollTime = componentData.rerollTimer;
			componentData.spellPickCounter++;
			if (componentData.spellPickCounter < GetMaxSpellPickCount())
			{
				ettMgr.SetComponentData(levelRewardEtt, componentData);
				UpdateTitleText();
				return;
			}
			break;
		}
		case LevelRewardType.RuneWizardRune:
		{
			Vector3 worldPoint = ((!GameMgr.IsMobile_Static) ? UIPlayerDataMgr.Inst.image_BagBtn.transform.position : UIPlayerDataMgr.Inst.image_BagBtn.transform.position);
			Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(null, worldPoint);
			RectTransformUtility.ScreenPointToLocalPointInRectangle(UIMgr.Inst.canvas_10.GetComponent<RectTransform>(), screenPoint, null, out var localPoint);
			int _id2 = buffer[index].info.id;
			PlayerMgr.Inst.ItemCtrller.UIRewardFly(UIMgr.Inst.canvas_3, _id2, RollRewardFly.DropType.Spell, ((UIRewardSpell)uiRewardBases[index]).image_Icon.transform.position, localPoint + new Vector2(-10f, -10f), null, useParticleColor: true, delegate
			{
				PlayerMgr.Inst.SpellPick(new SlotData(_id2));
				UIPlayerDataMgr.Inst.BagShakeButton();
			}, isUI: true, dropOnEnd: false, null, CamController.Inst.cam_UI);
			SEMgr.Inst.uiLevelRewardPickSpell.PlaySE();
			RoomFinishLogger.Reward.Spell spell = (RoomFinishLogger.Reward.Spell)LevelMgr.Inst.RoomFinishLogger.rewards.Find((RoomFinishLogger.Reward reward) => reward.type == "Spells");
			spell.selected.Add(buffer[index].info.id);
			if (PlayerMgr.Inst.ItemCtrller.relicCfg_RerollSpell != null)
			{
				int num = (spell.remainRerollTime = PlayerMgr.Inst.ItemCtrller.relicCfg_RerollSpell.int1.result - componentData.rerollTimer);
			}
			spell.rerollTime = componentData.rerollTimer;
			componentData.spellPickCounter++;
			if (componentData.spellPickCounter < GetMaxSpellPickCount() - 1)
			{
				ettMgr.SetComponentData(levelRewardEtt, componentData);
				UpdateTitleText();
				return;
			}
			break;
		}
		default:
			Debug.LogError(componentData.rewardType);
			break;
		}
		if (componentData.rewardType == LevelRewardType.Relic)
		{
			for (int j = 0; j < buffer.Length; j++)
			{
				if (j != index && !selectedRelicIds.Contains(buffer[j].info.id))
				{
					PlayerMgr.Inst.BaData.BackRelicToPool(buffer[j].info.id, 1);
				}
			}
		}
		componentData.UIUse(playerSE: false);
		ettMgr.SetComponentData(levelRewardEtt, componentData);
		foreach (UIRewardBase item in uiRewardBases.Where((UIRewardBase t) => t != null))
		{
			item.SetHide();
		}
		for (int k = 0; k < uiRewardBases.Count; k++)
		{
			if (k != index && uiRewardBases[k] != null)
			{
				uiRewardBases[k].SetHide();
			}
		}
		anima.SetTrigger("Disappear");
		Time.timeScale = 1f;
		SetIsOpen(isOpen: false);
		hoverIndex = 0;
		PlayerMgr.Inst.PlayerCtrller.StartMotion();
	}

	public override void Hide()
	{
		OnHide();
	}

	protected override void OnHide()
	{
		if (!canvasGroup.interactable || !base.IsOpen)
		{
			return;
		}
		UIMgr.Inst.MoveDownHoverLayer();
		SetIsOpen(isOpen: false);
		UnRegistarOnlyWhenHide();
		MobileResetHover();
		if (selectSpellIndex != -1)
		{
			uiRewardBases[hoverIndex].GetComponent<UIRewardWand>().rtsf_Spells.GetChild(selectSpellIndex).GetComponent<UISlotWandExternal>().OnPointerExit(null);
		}
		hoverIndex = 0;
		for (int i = 0; i < uiRewardBases.Count; i++)
		{
			if (uiRewardBases[i] != null)
			{
				uiRewardBases[i].SetHide();
			}
		}
		anima.SetTrigger("Disappear");
		Time.timeScale = 1f;
		PlayerMgr.Inst.PlayerCtrller.StartMotion();
		SEMgr.Inst.uiChangeLabelClose.PlaySE();
	}

	private int GetNextSpellOfWand()
	{
		int num = selectSpellIndex;
		do
		{
			num++;
			if (num >= uiRewardBases[hoverIndex].GetComponent<UIRewardWand>().rtsf_Spells.childCount)
			{
				num = 0;
			}
			if (num == selectSpellIndex)
			{
				return selectSpellIndex;
			}
			if (selectSpellIndex == -1 && num == uiRewardBases[hoverIndex].GetComponent<UIRewardWand>().rtsf_Spells.childCount - 1)
			{
				if (uiRewardBases[hoverIndex].GetComponent<UIRewardWand>().rtsf_Spells.GetChild(num).GetComponent<UISlotWandExternal>() == null || !uiRewardBases[hoverIndex].GetComponent<UIRewardWand>().rtsf_Spells.GetChild(num).GetComponent<UISlotWandExternal>().image_SpellIcon.isActiveAndEnabled)
				{
					return selectSpellIndex;
				}
				return num;
			}
		}
		while (uiRewardBases[hoverIndex].GetComponent<UIRewardWand>().rtsf_Spells.GetChild(num).GetComponent<UISlotWandExternal>() == null || !uiRewardBases[hoverIndex].GetComponent<UIRewardWand>().rtsf_Spells.GetChild(num).GetComponent<UISlotWandExternal>().image_SpellIcon.isActiveAndEnabled);
		return num;
	}

	private int GetPreviousSpellOfWand()
	{
		int num = selectSpellIndex;
		do
		{
			num--;
			if (num < 0)
			{
				num = uiRewardBases[hoverIndex].GetComponent<UIRewardWand>().rtsf_Spells.childCount - 1;
			}
			if (num == selectSpellIndex)
			{
				return selectSpellIndex;
			}
			if (selectSpellIndex == -1 && num == 0)
			{
				if (uiRewardBases[hoverIndex].GetComponent<UIRewardWand>().rtsf_Spells.GetChild(num).GetComponent<UISlotWandExternal>() == null || !uiRewardBases[hoverIndex].GetComponent<UIRewardWand>().rtsf_Spells.GetChild(num).GetComponent<UISlotWandExternal>().image_SpellIcon.isActiveAndEnabled)
				{
					return selectSpellIndex;
				}
				return num;
			}
		}
		while (uiRewardBases[hoverIndex].GetComponent<UIRewardWand>().rtsf_Spells.GetChild(num).GetComponent<UISlotWandExternal>() == null || !uiRewardBases[hoverIndex].GetComponent<UIRewardWand>().rtsf_Spells.GetChild(num).GetComponent<UISlotWandExternal>().image_SpellIcon.isActiveAndEnabled);
		return num;
	}

	public void _RerollRelic()
	{
		if (!btn_RerollRelic.gameObject.activeSelf || !ettMgr.HasComponent<LevelReward>(levelRewardEtt))
		{
			return;
		}
		LevelReward componentData = ettMgr.GetComponentData<LevelReward>(levelRewardEtt);
		DynamicBuffer<LevelRewardInfoBED> buffer = ettMgr.GetBuffer<LevelRewardInfoBED>(levelRewardEtt);
		for (int i = 0; i < buffer.Length; i++)
		{
			if (!selectedRelicIds.Contains(buffer[i].info.id))
			{
				PlayerMgr.Inst.BaData.BackRelicToPool(buffer[i].info.id, 1);
			}
		}
		selectedRelicIds.Clear();
		buffer.Clear();
		foreach (ItemInfo item in OutputMgr_Dots.GetLevelReward(LevelRewardType.Relic))
		{
			buffer.Add(new LevelRewardInfoBED(item.type, item.id));
		}
		Show(levelRewardEtt);
		componentData.rerollTimer++;
		ettMgr.SetComponentData(levelRewardEtt, componentData);
		UpdateRerollTime_Relic();
		SEMgr.Inst.relic_Reroll.PlaySE();
	}

	public void _RerollSpell()
	{
		if (!btn_RerollSpell.gameObject.activeSelf || levelRewardEtt == Entity.Null || !ettMgr.HasComponent<LevelReward>(levelRewardEtt))
		{
			return;
		}
		LevelReward componentData = ettMgr.GetComponentData<LevelReward>(levelRewardEtt);
		DynamicBuffer<LevelRewardInfoBED> buffer = ettMgr.GetBuffer<LevelRewardInfoBED>(levelRewardEtt);
		List<ItemInfo> list;
		switch (LevelMgr.Inst.CurrentRewardType)
		{
		case LevelRewardType.Spell:
		case LevelRewardType.Shortcut:
			list = OutputMgr_Dots.GetLevelReward(LevelRewardType.Spell);
			break;
		case LevelRewardType.Elite:
			list = ((BattleMgr.Inst.CurrentStage != 9 && BattleMgr.Inst.CurrentStage != 10) ? OutputMgr_Dots.GetLevelReward(LevelRewardType.Elite) : OutputMgr_Dots.GetLevelReward(LevelRewardType.Spell));
			break;
		default:
			list = OutputMgr_Dots.GetLevelReward(LevelRewardType.Spell);
			Debug.LogError(LevelMgr.Inst.CurrentRewardType);
			break;
		}
		for (int i = 0; i < uiRewardBases.Count; i++)
		{
			UIRewardSpell uIRewardSpell = (UIRewardSpell)uiRewardBases[i];
			if (!buffer[i].isLock)
			{
				uIRewardSpell.Reroll(list[i].id);
			}
		}
		componentData.rerollTimer++;
		ettMgr.SetComponentData(levelRewardEtt, componentData);
		UpdateRerollTime_Spell();
		SEMgr.Inst.relic_Reroll.PlaySE();
	}

	private void MobileResetHover()
	{
		if (GameMgr.IsMobile_Static)
		{
			if (selected != -1)
			{
				uiRewardBases[selected].UnHover();
			}
			selected = -1;
		}
	}

	private static int GetMaxSpellPickCount()
	{
		int num = 2;
		if (PlayerMgr.Inst.ItemCtrller.relicCfg_PickMoreSpell != null)
		{
			num += PlayerMgr.Inst.ItemCtrller.relicCfg_PickMoreSpell.int1.result;
		}
		return Mathf.Min(num, OutputMgr.GetSpellOptionCount());
	}

	private static int GetMaxRelicPickCount()
	{
		int num = 1;
		if (PlayerMgr.Inst.ItemCtrller.relicCfg_PickMoreRelic != null)
		{
			num += PlayerMgr.Inst.ItemCtrller.relicCfg_PickMoreRelic.int1.result;
		}
		return Mathf.Min(num, OutputMgr.GetRelicOptionCount());
	}
}
