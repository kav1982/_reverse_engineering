using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using PlayerLogger.Events;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[GameUISingletonPrefab("UIActivateGirl")]
public class UIActivateGirl : GameUISingletonMono<UIActivateGirl>
{
	public Animator anima;

	public Text text_Core;

	[SerializeField]
	private Vector3 infoOffsetPC;

	[SerializeField]
	private Vector3 infoOffsetMobile;

	[Header("Layout")]
	public UIActivateGirlSlot pfb_Slot;

	public GameObject pfb_LockLine;

	public RectTransform rtsf_Slots;

	public Vector2 startPoint;

	public float spaceX;

	public float spaceY;

	private Dictionary<int, int[]> _layerCount = new Dictionary<int, int[]>();

	[Header("Info")]
	public float infoWidthMobile1;

	public float infoWidthMobile2;

	public CanvasGroup canvasGroupInfo;

	public RectTransform rtsf_InfoRoot;

	public RectTransform rtsf_InfoPanel;

	public Image image_InfoIconOutline;

	public Image image_InfoIcon;

	public Image image_InfoBlood;

	public Text text_InfoCost;

	public Text text_InfoName;

	public Text text_InfoDesc;

	public Text text_ActiveButton;

	[Header("ActivateShow")]
	public Text text_Activated;

	public GameObject goTextActivated;

	public GameObject goButtonActivate;

	public Animator anima_InfoCore;

	public UIInfoSpell uiInfoSpell;

	public UIInfoRelic uiInfoRelic;

	public UIActivateGirlShowItem pfb_ShowItem;

	public Transform tsf_ShowItemParent;

	public Vector3 showItemInfoOffset_Spell;

	public Vector3 showItemInfoOffset_Relic;

	[Header("Multilangual")]
	public Text text_Title;

	[Header("Controler")]
	private int Controler_SelectLayer;

	private int Controler_SelectRoll;

	private int Controler_SelectIndex;

	private List<UIActivateGirlSlot> slots = new List<UIActivateGirlSlot>();

	private List<GameObject> lockLineGOs = new List<GameObject>();

	[Header("Controller")]
	private int itemShowIndex = -1;

	public GameObject ConfirmButtonOutline;

	private ActivateChangeLogger activateChangeLogger;

	public GameObject textCantUnlockTextFlow;

	public Transform textCantUnlockTextFlowRoot;

	public Color corlorSlotName;

	public int ActivateCount { get; private set; }

	private UIActivateGirlSlot activatingSlot { get; set; }

	protected override void RegistarWhenInit()
	{
		EventMgr.ChaosCoreChange = (Action)Delegate.Combine(EventMgr.ChaosCoreChange, new Action(ChaosCoreChange));
		EventMgr.LanguageChange = (Action)Delegate.Combine(EventMgr.LanguageChange, new Action(LanguageChance));
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
		EventMgr.ChaosCoreChange = (Action)Delegate.Remove(EventMgr.ChaosCoreChange, new Action(ChaosCoreChange));
		EventMgr.LanguageChange = (Action)Delegate.Remove(EventMgr.LanguageChange, new Action(LanguageChance));
		EventMgr.InputChange = (Action)Delegate.Remove(EventMgr.InputChange, new Action(InputChange));
	}

	private void InteractPerformed(InputAction.CallbackContext context)
	{
		if (base.IsOpen && UIMgr.Inst.InputType == PlayerInputType.Gamepad && base.IsOpen)
		{
			if (activatingSlot == null)
			{
				rtsf_Slots.GetChild(GetControllerSelectIndex(Controler_SelectLayer, Controler_SelectRoll, Controler_SelectIndex)).GetComponent<UIActivateGirlSlot>().OnPointerClick(null);
			}
			else if (ConfirmButtonOutline.activeInHierarchy)
			{
				_UnLock();
			}
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

	private void GamepadDirectPerformed(InputAction.CallbackContext context)
	{
		if (UIMgr.Inst.InputType == PlayerInputType.Gamepad && base.IsOpen)
		{
			Vector2 direct = context.ReadValue<Vector2>();
			MoveDirect(direct);
		}
	}

	private void MoveDirect(Vector2 _direct)
	{
		if (rtsf_InfoRoot.gameObject.activeSelf)
		{
			if (_direct == Vector2.down)
			{
				if (itemShowIndex != -1 && tsf_ShowItemParent.childCount > 0)
				{
					tsf_ShowItemParent.GetChild(itemShowIndex).GetComponent<UIActivateGirlShowItem>().OnPointerExit(null);
				}
				itemShowIndex = -1;
				ConfirmButtonOutline.SetActive(value: true);
			}
			else if (itemShowIndex == -1)
			{
				if (_direct == Vector2.up && tsf_ShowItemParent.childCount > 0)
				{
					itemShowIndex = 0;
					tsf_ShowItemParent.GetChild(itemShowIndex).GetComponent<UIActivateGirlShowItem>().OnPointerEnter(null);
					ConfirmButtonOutline.SetActive(value: false);
				}
			}
			else
			{
				if (tsf_ShowItemParent.childCount <= 0)
				{
					return;
				}
				if (_direct == Vector2.left)
				{
					if (itemShowIndex > 0)
					{
						tsf_ShowItemParent.GetChild(itemShowIndex).GetComponent<UIActivateGirlShowItem>().OnPointerExit(null);
						itemShowIndex--;
						tsf_ShowItemParent.GetChild(itemShowIndex).GetComponent<UIActivateGirlShowItem>().OnPointerEnter(null);
					}
					SEMgr.Inst.uiButtonHover_Button.PlaySE();
				}
				else if (_direct == Vector2.right)
				{
					if (itemShowIndex < tsf_ShowItemParent.childCount - 1)
					{
						tsf_ShowItemParent.GetChild(itemShowIndex).GetComponent<UIActivateGirlShowItem>().OnPointerExit(null);
						itemShowIndex++;
						tsf_ShowItemParent.GetChild(itemShowIndex).GetComponent<UIActivateGirlShowItem>().OnPointerEnter(null);
					}
					SEMgr.Inst.uiButtonHover_Button.PlaySE();
				}
			}
		}
		else if (Controler_SelectLayer == -1)
		{
			Controler_SelectLayer = 0;
			if (rtsf_Slots.GetChild(GetControllerSelectIndex(0, 0, 0)).GetComponent<UIActivateGirlSlot>().CanInteract)
			{
				rtsf_Slots.GetChild(GetControllerSelectIndex(0, 0, 0)).GetComponent<UIActivateGirlSlot>().OnPointerEnter(null);
			}
		}
		else if (_direct == Vector2.up)
		{
			if (CanMoveUp(Controler_SelectLayer, Controler_SelectRoll, Controler_SelectIndex) && rtsf_Slots.GetChild(GetControllerSelectIndex(Controler_SelectLayer - 1, Controler_SelectRoll, Controler_SelectIndex)).GetComponent<UIActivateGirlSlot>().CanInteract)
			{
				rtsf_Slots.GetChild(GetControllerSelectIndex(Controler_SelectLayer, Controler_SelectRoll, Controler_SelectIndex)).GetComponent<UIActivateGirlSlot>().OnPointerExit(null);
				Controler_SelectLayer--;
				rtsf_Slots.GetChild(GetControllerSelectIndex(Controler_SelectLayer, Controler_SelectRoll, Controler_SelectIndex)).GetComponent<UIActivateGirlSlot>().OnPointerEnter(null);
				SEMgr.Inst.uiButtonHover_Button.PlaySE();
			}
		}
		else if (_direct == Vector2.down)
		{
			if (CanMoveDown(Controler_SelectLayer, Controler_SelectRoll, Controler_SelectIndex) && rtsf_Slots.GetChild(GetControllerSelectIndex(Controler_SelectLayer + 1, Controler_SelectRoll, Controler_SelectIndex)).GetComponent<UIActivateGirlSlot>().CanInteract)
			{
				rtsf_Slots.GetChild(GetControllerSelectIndex(Controler_SelectLayer, Controler_SelectRoll, Controler_SelectIndex)).GetComponent<UIActivateGirlSlot>().OnPointerExit(null);
				Controler_SelectLayer++;
				rtsf_Slots.GetChild(GetControllerSelectIndex(Controler_SelectLayer, Controler_SelectRoll, Controler_SelectIndex)).GetComponent<UIActivateGirlSlot>().OnPointerEnter(null);
				SEMgr.Inst.uiButtonHover_Button.PlaySE();
			}
		}
		else if (_direct == Vector2.left)
		{
			if (CanMoveLeft(Controler_SelectRoll, Controler_SelectIndex))
			{
				int controler_SelectIndex = Controler_SelectIndex;
				int num = Controler_SelectRoll;
				if (Controler_SelectIndex > 0)
				{
					controler_SelectIndex--;
				}
				else if (_layerCount[Controler_SelectLayer][Controler_SelectRoll - 1] != 0)
				{
					num--;
					controler_SelectIndex = _layerCount[Controler_SelectLayer][num] - 1;
				}
				else
				{
					num -= 2;
					controler_SelectIndex = _layerCount[Controler_SelectLayer][num] - 1;
				}
				if (rtsf_Slots.GetChild(GetControllerSelectIndex(Controler_SelectLayer, num, controler_SelectIndex)).GetComponent<UIActivateGirlSlot>().CanInteract)
				{
					rtsf_Slots.GetChild(GetControllerSelectIndex(Controler_SelectLayer, Controler_SelectRoll, Controler_SelectIndex)).GetComponent<UIActivateGirlSlot>().OnPointerExit(null);
					Controler_SelectIndex = controler_SelectIndex;
					Controler_SelectRoll = num;
					rtsf_Slots.GetChild(GetControllerSelectIndex(Controler_SelectLayer, Controler_SelectRoll, Controler_SelectIndex)).GetComponent<UIActivateGirlSlot>().OnPointerEnter(null);
					SEMgr.Inst.uiButtonHover_Button.PlaySE();
				}
			}
			SEMgr.Inst.uiButtonHover_Button.PlaySE();
		}
		else
		{
			if (!(_direct == Vector2.right))
			{
				return;
			}
			if (CanMoveRight(Controler_SelectLayer, Controler_SelectRoll, Controler_SelectIndex))
			{
				int controler_SelectIndex2 = Controler_SelectIndex;
				int num2 = Controler_SelectRoll;
				if (Controler_SelectIndex < _layerCount[Controler_SelectLayer][Controler_SelectRoll] - 1)
				{
					controler_SelectIndex2++;
				}
				else if (_layerCount[Controler_SelectLayer][Controler_SelectRoll + 1] != 0)
				{
					controler_SelectIndex2 = 0;
					num2++;
				}
				else
				{
					controler_SelectIndex2 = 0;
					num2 += 2;
				}
				if (rtsf_Slots.GetChild(GetControllerSelectIndex(Controler_SelectLayer, num2, controler_SelectIndex2)).GetComponent<UIActivateGirlSlot>().CanInteract)
				{
					rtsf_Slots.GetChild(GetControllerSelectIndex(Controler_SelectLayer, Controler_SelectRoll, Controler_SelectIndex)).GetComponent<UIActivateGirlSlot>().OnPointerExit(null);
					Controler_SelectIndex = controler_SelectIndex2;
					Controler_SelectRoll = num2;
					rtsf_Slots.GetChild(GetControllerSelectIndex(Controler_SelectLayer, Controler_SelectRoll, Controler_SelectIndex)).GetComponent<UIActivateGirlSlot>().OnPointerEnter(null);
					SEMgr.Inst.uiButtonHover_Button.PlaySE();
				}
			}
			SEMgr.Inst.uiButtonHover_Button.PlaySE();
		}
		bool CanMoveDown(int layerIndex, int Column, int index)
		{
			if (layerIndex >= 3)
			{
				return false;
			}
			switch (Column)
			{
			case 1:
				if (index >= _layerCount[layerIndex + 1][1] || _layerCount[layerIndex + 1][1] == 0)
				{
					break;
				}
				goto case 0;
			case 0:
			case 2:
				return true;
			}
			return false;
		}
		static bool CanMoveLeft(int Column, int index)
		{
			switch (Column)
			{
			case 0:
				return false;
			case 1:
			case 2:
				return true;
			default:
				return false;
			}
		}
		bool CanMoveRight(int layerIndex, int Column, int index)
		{
			switch (Column)
			{
			case 1:
				if ((index != _layerCount[layerIndex][1] - 1 || _layerCount[layerIndex][2] <= 0) && index >= _layerCount[layerIndex][1] - 1)
				{
					break;
				}
				goto case 0;
			case 0:
				return true;
			case 2:
				return false;
			}
			return false;
		}
		bool CanMoveUp(int layerIndex, int Column, int index)
		{
			if (layerIndex <= 0)
			{
				return false;
			}
			switch (Column)
			{
			case 1:
				if (index >= _layerCount[layerIndex - 1][1])
				{
					break;
				}
				goto case 0;
			case 2:
				if (_layerCount[layerIndex][2] > _layerCount[layerIndex - 1][2])
				{
					break;
				}
				goto case 0;
			case 0:
				return true;
			}
			return false;
		}
	}

	private void InputChange()
	{
		if (base.IsOpen)
		{
			switch (UIMgr.Inst.InputType)
			{
			case PlayerInputType.Keyboard:
				rtsf_Slots.GetChild(GetControllerSelectIndex(Controler_SelectLayer, Controler_SelectLayer, Controler_SelectIndex)).GetComponent<UIActivateGirlSlot>().OnPointerExit(null);
				ConfirmButtonOutline.SetActive(value: false);
				break;
			case PlayerInputType.Gamepad:
				Controler_SelectLayer = 0;
				Controler_SelectIndex = 0;
				Controler_SelectLayer = 0;
				rtsf_Slots.GetChild(GetControllerSelectIndex(Controler_SelectLayer, Controler_SelectLayer, Controler_SelectIndex)).GetComponent<UIActivateGirlSlot>().OnPointerEnter(null);
				break;
			default:
				Debug.LogError(UIMgr.Inst.InputType);
				break;
			}
		}
	}

	private void ChaosCoreChange()
	{
		text_Core.text = DataMgr.selectedWorldData.chaosCoreCount.ToString();
	}

	private void LanguageChance()
	{
		text_Title.text = 1003301.GetText();
		text_ActiveButton.text = 1003311.GetText();
		UpdateLockLine();
	}

	protected override IEnumerator OnInit()
	{
		InitialUI();
		UpdateLockLine();
		ChaosCoreChange();
		LanguageChance();
		InputChange();
		yield return null;
	}

	private void InitialUI()
	{
		_layerCount = new Dictionary<int, int[]>
		{
			{
				0,
				new int[3]
			},
			{
				1,
				new int[3]
			},
			{
				2,
				new int[3]
			},
			{
				3,
				new int[3]
			}
		};
		Controler_SelectLayer = -1;
		foreach (ActivateGirlConfig item in ActivateGirlConfig.list)
		{
			UIActivateGirlSlot uIActivateGirlSlot = UnityEngine.Object.Instantiate(pfb_Slot, rtsf_Slots);
			switch (item.specialType)
			{
			case ActivateGirlSpecialType.None:
				_layerCount[item.belongLayer][1]++;
				((RectTransform)uIActivateGirlSlot.transform).anchoredPosition = new Vector2(startPoint.x + (float)(_layerCount[item.belongLayer][0] + _layerCount[item.belongLayer][1] + _layerCount[item.belongLayer][2] - 1) * spaceX, startPoint.y + (float)(1 - item.belongLayer) * spaceY);
				break;
			case ActivateGirlSpecialType.Choice3To2:
			case ActivateGirlSpecialType.Relic:
			case ActivateGirlSpecialType.ChoiceLock:
			case ActivateGirlSpecialType.Choice4To2:
				_layerCount[item.belongLayer][0]++;
				((RectTransform)uIActivateGirlSlot.transform).anchoredPosition = new Vector2(startPoint.x + -1f * spaceX, startPoint.y + (float)(1 - item.belongLayer) * spaceY);
				break;
			case ActivateGirlSpecialType.TentacleGirlReaction:
				_layerCount[item.belongLayer][2]++;
				((RectTransform)uIActivateGirlSlot.transform).anchoredPosition = new Vector2(startPoint.x + 3.5f * spaceX, startPoint.y + (float)(1 - item.belongLayer) * spaceY);
				break;
			default:
				Debug.LogError(item.specialType);
				break;
			}
			uIActivateGirlSlot.Initialize(item.id, this);
			slots.Add(uIActivateGirlSlot);
			if (DataMgr.selectedWorldData.activateGirlActivatedIDs2.Contains(item.id))
			{
				ActivateCount++;
			}
		}
		foreach (UIActivateGirlSlot slot in slots)
		{
			slot.CheckLock();
		}
		for (int i = 1; i < ScriptableObjMgr.Inst.activateGirlLayerNeed.ints.Length; i++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(pfb_LockLine, rtsf_Slots);
			lockLineGOs.Add(gameObject);
			((RectTransform)gameObject.transform).anchoredPosition = new Vector2(0f, startPoint.y + (1.5f - (float)i) * spaceY);
		}
	}

	private int GetControllerSelectIndex(int layer, int column, int index)
	{
		int num = 0;
		num += 3;
		switch (column)
		{
		case 0:
			num += _layerCount[0][1];
			num += _layerCount[1][1];
			num += _layerCount[2][1];
			num += _layerCount[3][1];
			if (layer > 0)
			{
				num += _layerCount[0][0];
			}
			if (layer > 1)
			{
				num += _layerCount[1][0];
			}
			if (layer > 2)
			{
				num += _layerCount[2][0];
			}
			return num;
		case 1:
			if (layer > 0)
			{
				num += _layerCount[0][1];
			}
			if (layer > 1)
			{
				num += _layerCount[1][1];
			}
			if (layer > 2)
			{
				num += _layerCount[2][1];
			}
			return num + index;
		case 2:
			num += _layerCount[0][0] + _layerCount[0][1];
			num += _layerCount[1][0] + _layerCount[1][1];
			num += _layerCount[2][0] + _layerCount[2][1];
			num += _layerCount[3][0] + _layerCount[3][1];
			if (layer > 0)
			{
				num += _layerCount[0][2];
			}
			if (layer > 1)
			{
				num += _layerCount[1][2];
			}
			if (layer > 2)
			{
				num += _layerCount[2][2];
			}
			return num;
		default:
			Debug.LogError("错误");
			return 3;
		}
	}

	private void UnLockSelectedSlot()
	{
		ActivateCount++;
		if (!DataMgr.selectedWorldData.activateGirlActivatedIDs2.Contains(activatingSlot.ID))
		{
			PlayerMgr.Inst.ChangeChaosCore(-activatingSlot.Config.cost);
			DataMgr.selectedWorldData.activateGirlActivatedIDs2.Add(activatingSlot.ID);
			DataMgr.SaveSelectedWorldData();
		}
		foreach (UIActivateGirlSlot slot in slots)
		{
			slot.CheckLock();
		}
		UpdateLockLine();
		if (ScriptableObjMgr.Inst.activateGirlLayerNeed.ints.Any((int t) => t == ActivateCount))
		{
			SEMgr.Inst.uiActivateGirl_UnlockLine.PlaySE();
		}
		else
		{
			SEMgr.Inst.uiActivateGirl_Activate.PlaySE();
		}
	}

	private void UpdateLockLine()
	{
		for (int i = 0; i < lockLineGOs.Count; i++)
		{
			if (ActivateCount >= ScriptableObjMgr.Inst.activateGirlLayerNeed.ints[i + 1])
			{
				lockLineGOs[i].SetActive(value: false);
			}
			else if (ScriptableObjMgr.Inst.activateGirlLayerNeed.ints[i + 1] == 999)
			{
				lockLineGOs[i].GetComponentInChildren<Text>().text = 1002501.GetText();
			}
			else
			{
				lockLineGOs[i].GetComponentInChildren<Text>().text = ActivateCount + "/" + ScriptableObjMgr.Inst.activateGirlLayerNeed.ints[i + 1];
			}
		}
	}

	protected override void OnShow(object obj = null)
	{
		base.OnShow(obj);
		activateChangeLogger = new ActivateChangeLogger
		{
			before_unlocked = DataMgr.selectedWorldData.activateGirlActivatedIDs2.ToList()
		};
		activateChangeLogger.AutoRecordBeforeResources();
		UIPlayerDataMgr.Inst.UISlotBagExitall();
		PlayerMgr.Inst.PlayerCtrller.StopMotion();
		SEMgr.Inst.uiChangeLabel.PlaySE();
		anima.Play("Show");
		UIMgr.TryAdditionalMobileShow(base.transform);
		UIPlayerDataMgr.Inst.ResourceUIPopUp(UIPlayerDataMgr.ResourceUIPop.Cores);
	}

	public override void Hide()
	{
		if (GameUISingletonMono<UIActivateGirl>.Inst.rtsf_InfoRoot.gameObject.activeSelf)
		{
			_HideInfo();
		}
		else
		{
			base.Hide();
		}
	}

	protected override void OnHide()
	{
		SEMgr.Inst.uiChangeLabelClose.PlaySE();
		PlayerMgr.Inst.PlayerCtrller.StartMotion();
		anima.Play("Hide");
		UIMgr.TryAdditionalMobileHide(base.transform);
		activateChangeLogger.after_unlocked = DataMgr.selectedWorldData.activateGirlActivatedIDs2.ToList();
		activateChangeLogger.AutoRecordAfterResourcesAndFlow();
		activateChangeLogger.Report();
		UIPlayerDataMgr.Inst.ResourceUISetToDefault(UIPlayerDataMgr.ResourceUIPop.Cores);
	}

	public void ShowInfo(UIActivateGirlSlot slot)
	{
		StartCoroutine(SlotEnterIE(slot));
	}

	private IEnumerator SlotEnterIE(UIActivateGirlSlot slot)
	{
		canvasGroupInfo.blocksRaycasts = false;
		canvasGroupInfo.alpha = 0f;
		if (!slot.CanInteract)
		{
			yield break;
		}
		activatingSlot = slot;
		image_InfoIcon.sprite = slot.image_Icon.sprite;
		image_InfoIconOutline.sprite = slot.image_Icon.sprite;
		text_InfoName.text = slot.Config.GetName();
		switch (slot.Config.specialType)
		{
		case ActivateGirlSpecialType.None:
			text_InfoDesc.text = 1003303.GetText();
			break;
		case ActivateGirlSpecialType.Choice3To2:
			text_InfoDesc.text = slot.Config.GetDesc();
			break;
		case ActivateGirlSpecialType.Relic:
			text_InfoDesc.text = 1003304.GetText();
			break;
		case ActivateGirlSpecialType.ChoiceLock:
			text_InfoDesc.text = slot.Config.GetDesc();
			break;
		case ActivateGirlSpecialType.Choice4To2:
			text_InfoDesc.text = slot.Config.GetDesc();
			break;
		case ActivateGirlSpecialType.TentacleGirlReaction:
			text_InfoDesc.text = slot.Config.GetDesc();
			break;
		default:
			Debug.LogError(slot.Config.specialType);
			break;
		}
		tsf_ShowItemParent.DestroyAllChildImmediate();
		if (activatingSlot.Config.spellIDs != null)
		{
			rtsf_InfoRoot.gameObject.SetActive(value: false);
			int[] spellIDs = activatingSlot.Config.spellIDs;
			foreach (int num in spellIDs)
			{
				bool flag = activatingSlot.Config.specialType == ActivateGirlSpecialType.None;
				if (!flag || SpellConfig.dic[num].dropType != 0)
				{
					UnityEngine.Object.Instantiate(pfb_ShowItem, tsf_ShowItemParent).GetComponent<UIActivateGirlShowItem>().Initialize(this, num, flag);
				}
			}
		}
		tsf_ShowItemParent.gameObject.SetActive(tsf_ShowItemParent.childCount > 0);
		if (GameMgr.IsMobile_Static)
		{
			Vector2 sizeDelta = rtsf_InfoRoot.GetComponent<RectTransform>().sizeDelta;
			sizeDelta.x = ((tsf_ShowItemParent.childCount > 0) ? infoWidthMobile2 : infoWidthMobile1);
			rtsf_InfoPanel.GetComponent<RectTransform>().sizeDelta = sizeDelta;
		}
		if (slot.IsActivated)
		{
			goButtonActivate.gameObject.SetActive(value: false);
			goTextActivated.gameObject.SetActive(value: true);
			text_Activated.text = 1003305.GetText();
		}
		else
		{
			goButtonActivate.gameObject.SetActive(value: true);
			goTextActivated.gameObject.SetActive(value: false);
			if (slot.Config.specialType == ActivateGirlSpecialType.TentacleGirlReaction)
			{
				image_InfoBlood.gameObject.SetActive(value: false);
			}
			else
			{
				image_InfoBlood.gameObject.SetActive(value: true);
				text_InfoCost.text = slot.Config.cost.ToString();
				text_InfoCost.color = ((DataMgr.selectedWorldData.chaosCoreCount >= slot.Config.cost) ? Color.green : Color.red);
			}
		}
		rtsf_InfoRoot.gameObject.SetActive(value: true);
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		LayoutRebuilder.ForceRebuildLayoutImmediate(rtsf_InfoRoot.transform.GetChild(0).transform as RectTransform);
		canvasGroupInfo.DOFade(1f, 0.3f);
		canvasGroupInfo.blocksRaycasts = true;
		itemShowIndex = -1;
		ConfirmButtonOutline.SetActive(value: false);
	}

	public void ShowItemEnter(UIActivateGirlShowItem showItem)
	{
		if (showItem.IsSpell)
		{
			uiInfoSpell.gameObject.SetActive(value: true);
			uiInfoSpell.UpdateInfo(showItem.ID);
			if (GameMgr.IsMobile_Static)
			{
				UIMgr.AutoPivot(showItem.transform.position, uiInfoSpell.GetComponent<RectTransform>(), new Vector2(0f, 1f), useNewPivot: false, new Vector3(0f, 0f, 0f), showItemInfoOffset_Spell);
			}
			else
			{
				uiInfoSpell.transform.position = showItem.transform.position + showItemInfoOffset_Spell;
			}
		}
		else
		{
			uiInfoRelic.gameObject.SetActive(value: true);
			uiInfoRelic.UpdateInfo(RelicConfig.GetConfig(showItem.ID));
			if (GameMgr.IsMobile_Static)
			{
				UIMgr.AutoPivot(showItem.transform.position, uiInfoRelic.GetComponent<RectTransform>(), new Vector2(0f, 1f), useNewPivot: false, new Vector3(0f, 0f, 0f), showItemInfoOffset_Relic);
			}
			else
			{
				uiInfoRelic.transform.position = showItem.transform.position + showItemInfoOffset_Relic;
			}
		}
	}

	public void ShowItemExit()
	{
		uiInfoSpell.gameObject.SetActive(value: false);
		uiInfoRelic.gameObject.SetActive(value: false);
	}

	public void _UnLock()
	{
		if (activatingSlot.CanInteract && !activatingSlot.IsActivated && activatingSlot.Config.specialType != ActivateGirlSpecialType.TentacleGirlReaction)
		{
			if (DataMgr.selectedWorldData.chaosCoreCount >= activatingSlot.Config.cost)
			{
				UnLockSelectedSlot();
				ShowInfo(activatingSlot);
				SEMgr.Inst.uiActivateGirl_Activate.PlaySE();
			}
			else
			{
				anima_InfoCore.Play("Shock", 0, 0f);
				SEMgr.Inst.uiResearchWrong.PlaySE();
			}
		}
	}

	public void _HideInfo()
	{
		ShowItemExit();
		activatingSlot = null;
		rtsf_InfoRoot.gameObject.SetActive(value: false);
	}

	public void CantUnlockTextFlow(string textShow)
	{
		GameObject newTextFlow = UnityEngine.Object.Instantiate(textCantUnlockTextFlow, textCantUnlockTextFlowRoot);
		newTextFlow.gameObject.SetActive(value: true);
		CanvasGroup component = newTextFlow.GetComponent<CanvasGroup>();
		newTextFlow.GetComponentInChildren<Text>().text = textShow;
		component.DOFade(0f, 2f);
		((RectTransform)newTextFlow.transform).DOAnchorPosY(60f, 2f).OnComplete(delegate
		{
			UnityEngine.Object.DestroyImmediate(newTextFlow);
		});
	}
}
