using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Newtonsoft.Json;
using PlayerLogger;
using PlayerLogger.Events;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;

[GameUISingletonPrefab("UITalent")]
public class UITalent : GameUISingletonMono<UITalent>
{
	public Animator anima;

	public Text text_MagicCrystal;

	public Text text_EffectWandLimit;

	public Text text_EffectBagLimit;

	public Text text_EffectEnterDoorRecovery;

	public Text text_EffectMaxHP;

	public Text text_EffectHPRoom;

	public Text text_EffectInitialCoin;

	public Text text_EffectCoinRoom;

	public Text text_EffectRelicRoom;

	public Text text_EffectSpellRoom;

	public Text text_EffectMaxMP;

	public Text text_EffectMPRecover;

	public Text text_CostWandLimit;

	public Text text_CostBagLimit;

	public Text text_CostEnterDoorRecovery;

	public Text text_CostMaxHP;

	public Text text_CostHPRoom;

	public Text text_CostInitialCoin;

	public Text text_CostCoinRoom;

	public Text text_CostRelicRoom;

	public Text text_CostSpellRoom;

	public Text text_CostMaxMP;

	public Text text_CostMPRecover;

	public List<Text> text_unlockmore = new List<Text>();

	public GameObject go_MaskReset;

	public GameObject go_MaskWandLimitUp;

	public GameObject go_MaskBagLimitUp;

	public GameObject go_MaskEnterDoorRecoveryUp;

	public GameObject go_MaskMaxHPUp;

	public GameObject go_MaskHPRoomUp;

	public GameObject go_MaskInitialCoinUp;

	public GameObject go_MaskCoinRoomUp;

	public GameObject go_MaskRelicRoomUp;

	public GameObject go_MaskSpellRoomUp;

	public GameObject go_MaskMaxMPUp;

	public GameObject go_MaskMPRecoverUp;

	public Button btn_Reset;

	public Button btn_WandLimitUp;

	public Button btn_WandLimitDown;

	public Button btn_BagLimitUp;

	public Button btn_BagLimitDown;

	public Button btn_EnterDoorRecoveryUp;

	public Button btn_EnterDoorRecoveryDown;

	public Button btn_MaxHPUp;

	public Button btn_MaxHPDown;

	public Button btn_HPRoomUp;

	public Button btn_HPRoomDown;

	public Button btn_InitialCoinUp;

	public Button btn_InitialCoinDown;

	public Button btn_CoinRoomUp;

	public Button btn_CoinRoomDown;

	public Button btn_RelicRoomUp;

	public Button btn_RelicRoomDown;

	public Button btn_SpellRoomUp;

	public Button btn_SpellRoomDown;

	public Button btn_MaxMPUp;

	public Button btn_MaxMPDown;

	public Button btn_MPRecoverUp;

	public Button btn_MPRecoverDown;

	[Header("Unlock")]
	public GameObject go_LockTalent4;

	public GameObject go_LockTalent5;

	public GameObject go_LockTalent6;

	public GameObject go_LockTalent7;

	public GameObject go_LockTalent8;

	public GameObject go_LockTalent9;

	public GameObject go_LockTalent10;

	public GameObject go_LockTalent11;

	public GameObject go_Unlock1;

	public GameObject go_Unlock2;

	public GameObject go_Unlock3;

	public GameObject go_Unlock4;

	public Text text_Unlock1;

	public Text text_Unlock2;

	public Text text_Unlock3;

	public Text text_Unlock4;

	[Header("Gamepad")]
	public Button[] btn_Ups;

	public Button[] btn_Downs;

	public Button btn_Unlock1;

	public Button btn_Unlock2;

	public Button btn_Unlock3;

	public Button btn_Unlock4;

	public GameObject go_GamepadSelect;

	public GameObject go_GamepadUnlockSelect1;

	public GameObject go_GamepadUnlockSelect2;

	public GameObject go_GamepadUnlockSelect3;

	public GameObject go_GamepadUnlockSelect4;

	[Header("LanguageChange")]
	public Text text_Title;

	public Text text_WandLimit;

	public Text text_BagLimit;

	public Text text_EnterDoorRecovery;

	public Text text_MaxHP;

	public Text text_HPRoom;

	public Text text_InitialCoin;

	public Text text_CoinRoom;

	public Text text_RelicRoom;

	public Text text_SpellRoom;

	public Text text_MaxMP;

	public Text text_MPRecover;

	private WorldData worldData;

	private TalentUpgrade2 talentUpgrade2;

	private int gamepadIndex;

	private bool gamepadRight = true;

	private Button btn_LastSelect;

	private TalentChangeLogger talentChangeLogger;

	[Header("和谐")]
	public Image relicSR;

	public Sprite relicSpriteCh14;

	[Header("手游")]
	public GameObject mobileUnlockNext;

	[FormerlySerializedAs("mobileGamepadSelected")]
	public GameObject mobileGamepadUpdateFrame;

	public Text mobileUnlockButtonCost;

	public List<GameObject> unLock1;

	public List<GameObject> unLock2;

	public List<GameObject> unLock3;

	public List<GameObject> unLock4;

	public RectTransform rebuildLayout;

	public float textWidthWhenMax;

	public float textWidthWhenNotMax;

	public List<GameObject> allCrystalShow;

	public List<Text> allText;

	public UpdatButtonShow updateButtonShowReset;

	[Header("pc demo")]
	public GameObject demoBlockPlayer;

	[Header("Mobile Nav")]
	public ScrollRect scrollRect;

	public RectTransform content;

	public void MobileUpdateCrystalImage()
	{
		if (!GameMgr.IsMobile_Static)
		{
			return;
		}
		var source = allText.Select((Text go, int index) => new { go, index });
		string stringMax = 1000512.GetText();
		source.ToList().ForEach(x =>
		{
			int index2 = x.index;
			Vector2 sizeDelta = ((RectTransform)allText[index2].transform).sizeDelta;
			if (allText[index2].text == stringMax)
			{
				allCrystalShow[index2].gameObject.SetActive(value: false);
				sizeDelta.x = textWidthWhenMax;
			}
			else
			{
				allCrystalShow[index2].gameObject.SetActive(value: true);
				sizeDelta.x = textWidthWhenNotMax;
			}
			((RectTransform)allText[index2].transform).sizeDelta = sizeDelta;
		});
	}

	protected override void RegistarWhenInit()
	{
		EventMgr.LanguageChange = (Action)Delegate.Combine(EventMgr.LanguageChange, new Action(LanguageChange));
		EventMgr.InputChange = (Action)Delegate.Combine(EventMgr.InputChange, new Action(InputChange));
		EventMgr.AncienBloodChange = (Action)Delegate.Combine(EventMgr.AncienBloodChange, new Action(AncienBloodChange));
		EventMgr.MagicCrystalChange = (Action)Delegate.Combine(EventMgr.MagicCrystalChange, new Action(MagicCrystalChange));
	}

	protected override void RegistarOnlyWhenOpen()
	{
		base.inputActions.Player.GamepadDirect.performed += GamepadDirectPerformed;
		base.inputActions.Player.LeftStick.performed += GamepadDirectPerformed_Stick;
		base.inputActions.Player.Interact.performed += InteractPerformed;
		if (GameMgr.IsMobile_Static)
		{
			base.inputActions.Player.GamepadWest.performed += ResetPerformed;
		}
	}

	protected override void UnRegistarOnlyWhenHide()
	{
		base.inputActions.Player.GamepadDirect.performed -= GamepadDirectPerformed;
		base.inputActions.Player.LeftStick.performed -= GamepadDirectPerformed_Stick;
		base.inputActions.Player.Interact.performed -= InteractPerformed;
		if (GameMgr.IsMobile_Static)
		{
			base.inputActions.Player.GamepadWest.performed -= ResetPerformed;
		}
	}

	protected override void UnRegistarWhenDestroy()
	{
		EventMgr.LanguageChange = (Action)Delegate.Remove(EventMgr.LanguageChange, new Action(LanguageChange));
		EventMgr.InputChange = (Action)Delegate.Remove(EventMgr.InputChange, new Action(InputChange));
		EventMgr.AncienBloodChange = (Action)Delegate.Remove(EventMgr.AncienBloodChange, new Action(AncienBloodChange));
		EventMgr.MagicCrystalChange = (Action)Delegate.Remove(EventMgr.MagicCrystalChange, new Action(MagicCrystalChange));
	}

	private void Start()
	{
		if (GameMgr.IsHarmony_Static)
		{
			relicSR.sprite = relicSpriteCh14;
		}
	}

	private void GamepadDirectPerformed(InputAction.CallbackContext context)
	{
		if (UIMgr.Inst.InputType == PlayerInputType.Gamepad && base.IsOpen)
		{
			Vector2 vector = context.ReadValue<Vector2>();
			MoveDirec(vector);
			if (vector != Vector2.zero)
			{
				UpdateGamepadSelect();
			}
		}
	}

	private void GamepadDirectPerformed_Stick(InputAction.CallbackContext context)
	{
		if (UIMgr.Inst.InputType == PlayerInputType.Gamepad && base.IsOpen)
		{
			Vector2 vector = context.ReadValue<Vector2>();
			vector = ControlMgr.Inst.RampVector2(vector);
			MoveDirec(vector);
			if (vector != Vector2.zero)
			{
				UpdateGamepadSelect();
			}
		}
	}

	private void ResetPerformed(InputAction.CallbackContext obj)
	{
		if (btn_Reset.interactable)
		{
			_Reset();
			gamepadIndex = 0;
			go_GamepadSelect.transform.position = btn_Ups[0].transform.position;
			UpdateGamepadSelect();
			if (GameMgr.IsMobile_Static)
			{
				mobileGamepadUpdateFrame.SetActive(value: false);
			}
			if (GameMgr.IsMobile_Static && ControlMgr.Inst.InputType == PlayerInputType.Gamepad)
			{
				GeneralTool.ScrollToPadSelected(scrollRect, content, go_GamepadSelect.transform as RectTransform);
			}
		}
	}

	private void MoveDirec(Vector2 _direct)
	{
		if (_direct == Vector2.up)
		{
			if (GameMgr.IsMobile_Static && mobileGamepadUpdateFrame.activeInHierarchy)
			{
				mobileGamepadUpdateFrame.SetActive(value: false);
			}
			if (GameMgr.IsMobile_Static && gamepadIndex == 0)
			{
				return;
			}
			gamepadIndex--;
			if (gamepadIndex == -2)
			{
				gamepadIndex = btn_Ups.Length + 3;
				gamepadRight = true;
			}
			if (gamepadIndex == btn_Ups.Length + 3)
			{
				if (!go_Unlock4.activeSelf)
				{
					gamepadIndex = btn_Ups.Length - 1;
				}
				else if (go_Unlock3.activeSelf)
				{
					gamepadIndex = btn_Ups.Length + 1;
				}
			}
			if (gamepadIndex == btn_Ups.Length + 2)
			{
				if (!go_Unlock3.activeSelf)
				{
					gamepadIndex = btn_Ups.Length - 1;
				}
				else if (go_Unlock2.activeSelf)
				{
					gamepadIndex = btn_Ups.Length + 1;
				}
			}
			if (gamepadIndex == btn_Ups.Length + 1)
			{
				if (!go_Unlock2.activeSelf)
				{
					gamepadIndex = btn_Ups.Length - 3;
				}
				else if (go_Unlock1.activeSelf)
				{
					gamepadIndex = btn_Ups.Length;
				}
			}
			if (gamepadIndex == btn_Ups.Length && !go_Unlock1.activeSelf)
			{
				gamepadIndex = btn_Ups.Length - 5;
				if (!go_Unlock1.activeSelf)
				{
					gamepadIndex = btn_Ups.Length - 5;
				}
			}
			if (-1 < gamepadIndex && gamepadIndex < btn_Ups.Length)
			{
				while (!btn_Ups[gamepadIndex].gameObject.activeInHierarchy)
				{
					gamepadIndex--;
					if (gamepadIndex == -1)
					{
						Debug.LogError("为何遍历所有按钮，都没有一个可用的？");
						break;
					}
				}
			}
		}
		else if (_direct == Vector2.down)
		{
			gamepadIndex++;
			if (gamepadIndex >= btn_Ups.Length)
			{
				gamepadIndex--;
				return;
			}
			if (!btn_Ups[gamepadIndex].gameObject.activeInHierarchy)
			{
				Debug.LogWarning($"没有解锁：{gamepadIndex}");
				gamepadIndex = btn_Ups.Length;
			}
			else if (!gamepadRight && !btn_Downs[gamepadIndex].gameObject.activeInHierarchy)
			{
				gamepadRight = true;
			}
			bool[] array = new bool[4]
			{
				!go_Unlock1.activeSelf,
				!go_Unlock2.activeSelf,
				!go_Unlock3.activeSelf,
				!go_Unlock4.activeSelf
			};
			int num = gamepadIndex - btn_Ups.Length;
			int num2 = -1;
			if (num >= 0)
			{
				if (GameMgr.IsMobile_Static)
				{
					num2 = 1;
					mobileGamepadUpdateFrame.SetActive(value: true);
				}
				for (int i = 0; i < array.Length; i++)
				{
					if (!array[i])
					{
						num2 = i;
						break;
					}
				}
				if (num != 0 && !array[num])
				{
					gamepadIndex++;
				}
				else if (num == 0)
				{
					gamepadIndex = btn_Ups.Length + num2;
				}
				else
				{
					gamepadIndex = -1;
					gamepadRight = true;
				}
			}
		}
		else if (_direct == Vector2.left)
		{
			if (gamepadIndex > -1 && gamepadIndex < btn_Ups.Length)
			{
				if (gamepadRight)
				{
					if (btn_Downs[gamepadIndex].gameObject.activeSelf)
					{
						gamepadRight = !gamepadRight;
					}
				}
				else
				{
					gamepadRight = !gamepadRight;
				}
			}
		}
		else if (_direct == Vector2.right && gamepadIndex > -1 && gamepadIndex < btn_Ups.Length)
		{
			if (gamepadRight)
			{
				if (btn_Downs[gamepadIndex].gameObject.activeSelf)
				{
					gamepadRight = !gamepadRight;
				}
			}
			else
			{
				gamepadRight = !gamepadRight;
			}
		}
		if (GameMgr.IsMobile_Static && ControlMgr.Inst.InputType == PlayerInputType.Gamepad)
		{
			GeneralTool.ScrollToPadSelected(scrollRect, content, go_GamepadSelect.transform as RectTransform);
		}
	}

	private void InteractPerformed(InputAction.CallbackContext context)
	{
		if (UIMgr.Inst.InputType != PlayerInputType.Gamepad || !base.IsOpen)
		{
			return;
		}
		if (gamepadIndex == -1)
		{
			if (btn_Reset.interactable)
			{
				_Reset();
			}
		}
		else if (gamepadIndex < btn_Ups.Length)
		{
			if (gamepadRight)
			{
				if (btn_Ups[gamepadIndex].interactable)
				{
					btn_Ups[gamepadIndex].onClick.Invoke();
				}
				return;
			}
			if (btn_Downs[gamepadIndex].interactable)
			{
				btn_Downs[gamepadIndex].onClick.Invoke();
			}
			if (!btn_Downs[gamepadIndex].gameObject.activeInHierarchy)
			{
				gamepadRight = true;
				UpdateGamepadSelect();
			}
		}
		else
		{
			if (!GameMgr.IsMobile_Static && !ICJNOGPFMAM.MIFJADDOODN)
			{
				return;
			}
			if (GameMgr.IsMobile_Static)
			{
				if (!worldData.isTalentUnlock1)
				{
					btn_Unlock1.onClick.Invoke();
				}
				else if (!worldData.isTalentUnlock2)
				{
					btn_Unlock2.onClick.Invoke();
				}
				else if (!worldData.isTalentUnlock3)
				{
					btn_Unlock3.onClick.Invoke();
				}
				else if (!worldData.isTalentUnlock4)
				{
					btn_Unlock4.onClick.Invoke();
				}
			}
			else if (gamepadIndex == btn_Ups.Length)
			{
				if (go_GamepadUnlockSelect1.activeInHierarchy)
				{
					btn_Unlock1.onClick.Invoke();
				}
			}
			else if (gamepadIndex == btn_Ups.Length + 1)
			{
				if (go_GamepadUnlockSelect2.activeInHierarchy)
				{
					btn_Unlock2.onClick.Invoke();
				}
			}
			else if (gamepadIndex == btn_Ups.Length + 2)
			{
				if (go_GamepadUnlockSelect3.activeInHierarchy)
				{
					btn_Unlock3.onClick.Invoke();
				}
			}
			else if (gamepadIndex == btn_Ups.Length + 3)
			{
				if (go_GamepadUnlockSelect4.activeInHierarchy)
				{
					btn_Unlock4.onClick.Invoke();
				}
			}
			else
			{
				Debug.LogError("下标越界!");
			}
		}
	}

	private void LanguageChange()
	{
		text_Title.text = 1000501.GetText();
		text_WandLimit.text = 1000502.GetText();
		text_BagLimit.text = 1000503.GetText();
		text_MaxHP.text = 1001404.GetText();
		text_EnterDoorRecovery.text = 1000517.GetText();
		text_HPRoom.text = 1000509.GetText();
		text_InitialCoin.text = 1000504.GetText();
		text_CoinRoom.text = 1000508.GetText();
		text_RelicRoom.text = 1000507.GetText();
		text_SpellRoom.text = 1000506.GetText();
		text_MaxMP.text = 1000510.GetText();
		text_MPRecover.text = 1000511.GetText();
		string text = 1000519.GetText();
		foreach (Text item in text_unlockmore)
		{
			item.text = text;
		}
		MagicCrystalChange();
	}

	private void InputChange()
	{
		if (GameMgr.IsMobile_Static)
		{
			Debug.Log(UIMgr.Inst.InputType);
			updateButtonShowReset.UpdateButton();
		}
		Debug.LogWarning(UIMgr.Inst.InputType);
		switch (UIMgr.Inst.InputType)
		{
		case PlayerInputType.Keyboard:
			go_GamepadSelect.SetActive(value: false);
			go_GamepadUnlockSelect1.SetActive(value: false);
			go_GamepadUnlockSelect1.SetActive(value: false);
			go_GamepadUnlockSelect1.SetActive(value: false);
			break;
		case PlayerInputType.Gamepad:
			gamepadIndex = 0;
			gamepadRight = true;
			btn_LastSelect = btn_Ups[gamepadIndex];
			go_GamepadSelect.SetActive(value: true);
			if (GameMgr.IsMobile_Static)
			{
				DOVirtual.DelayedCall(0.1f, delegate
				{
					UpdateGamepadSelect(skipSE: true);
				});
			}
			else
			{
				UpdateGamepadSelect(skipSE: true);
			}
			break;
		default:
			Debug.LogError(UIMgr.Inst.InputType);
			break;
		}
	}

	private void AncienBloodChange()
	{
		text_Unlock1.color = ((worldData.ancientBloodCount >= talentUpgrade2.unlock1Cost) ? Color.green : Color.red);
		text_Unlock2.color = ((worldData.ancientBloodCount >= talentUpgrade2.unlock2Cost) ? Color.green : Color.red);
		text_Unlock3.color = ((worldData.ancientBloodCount >= talentUpgrade2.unlock3Cost) ? Color.green : Color.red);
		text_Unlock4.color = ((worldData.ancientBloodCount >= talentUpgrade2.unlock4Cost) ? Color.green : Color.red);
	}

	private void MagicCrystalChange()
	{
		text_MagicCrystal.text = worldData.magicCrystalCount.ToString();
		if (worldData.levelOfWandLimit == 0)
		{
			text_EffectWandLimit.text = "-";
			text_CostWandLimit.text = talentUpgrade2.wandLimit[0].cost.ToString();
		}
		else
		{
			text_EffectWandLimit.text = "+" + talentUpgrade2.wandLimit[worldData.levelOfWandLimit - 1].value + " " + 1000518.GetText();
			if (worldData.levelOfWandLimit == talentUpgrade2.wandLimit.Length)
			{
				text_CostWandLimit.text = 1000512.GetText();
			}
			else
			{
				text_CostWandLimit.text = talentUpgrade2.wandLimit[worldData.levelOfWandLimit].cost.ToString();
			}
		}
		if (worldData.levelOfBagLimit == 0)
		{
			text_EffectBagLimit.text = "-";
			text_CostBagLimit.text = talentUpgrade2.bagLimit[0].cost.ToString();
		}
		else
		{
			text_EffectBagLimit.text = "+" + talentUpgrade2.bagLimit[worldData.levelOfBagLimit - 1].value + " " + 1000518.GetText();
			if (worldData.levelOfBagLimit == talentUpgrade2.bagLimit.Length)
			{
				text_CostBagLimit.text = 1000512.GetText();
			}
			else
			{
				text_CostBagLimit.text = talentUpgrade2.bagLimit[worldData.levelOfBagLimit].cost.ToString();
			}
		}
		if (worldData.levelOfEnterDoorRecovery == 0)
		{
			text_EffectEnterDoorRecovery.text = "-";
			text_CostEnterDoorRecovery.text = talentUpgrade2.enterDoorRecovery[0].cost.ToString();
		}
		else
		{
			text_EffectEnterDoorRecovery.text = talentUpgrade2.enterDoorRecovery[worldData.levelOfEnterDoorRecovery - 1].value.ToString();
			if (worldData.levelOfEnterDoorRecovery == talentUpgrade2.enterDoorRecovery.Length)
			{
				text_CostEnterDoorRecovery.text = 1000512.GetText();
			}
			else
			{
				text_CostEnterDoorRecovery.text = talentUpgrade2.enterDoorRecovery[worldData.levelOfEnterDoorRecovery].cost.ToString();
			}
		}
		if (worldData.levelOfMaxHP == 0)
		{
			text_EffectMaxHP.text = "-";
			text_CostMaxHP.text = talentUpgrade2.maxHP[0].cost.ToString();
		}
		else
		{
			text_EffectMaxHP.text = "+" + talentUpgrade2.maxHP[worldData.levelOfMaxHP - 1].value + " " + 1000505.GetText();
			if (worldData.levelOfMaxHP == talentUpgrade2.maxHP.Length)
			{
				text_CostMaxHP.text = 1000512.GetText();
			}
			else
			{
				text_CostMaxHP.text = talentUpgrade2.maxHP[worldData.levelOfMaxHP].cost.ToString();
			}
		}
		if (worldData.levelOfHPRoom == 0)
		{
			text_EffectHPRoom.text = "-";
			text_CostHPRoom.text = talentUpgrade2.hpRoom[0].cost.ToString();
		}
		else
		{
			text_EffectHPRoom.text = "+" + talentUpgrade2.hpRoom[worldData.levelOfHPRoom - 1].value + " " + 1000516.GetText();
			if (worldData.levelOfHPRoom == talentUpgrade2.hpRoom.Length)
			{
				text_CostHPRoom.text = 1000512.GetText();
			}
			else
			{
				text_CostHPRoom.text = talentUpgrade2.hpRoom[worldData.levelOfHPRoom].cost.ToString();
			}
		}
		if (worldData.levelOfInitialCoin == 0)
		{
			text_EffectInitialCoin.text = "-";
			text_CostInitialCoin.text = talentUpgrade2.initialCoin[0].cost.ToString();
		}
		else
		{
			text_EffectInitialCoin.text = "+" + talentUpgrade2.initialCoin[worldData.levelOfInitialCoin - 1].value + " " + 1000504.GetText();
			if (worldData.levelOfInitialCoin == talentUpgrade2.initialCoin.Length)
			{
				text_CostInitialCoin.text = 1000512.GetText();
			}
			else
			{
				text_CostInitialCoin.text = talentUpgrade2.initialCoin[worldData.levelOfInitialCoin].cost.ToString();
			}
		}
		if (worldData.levelOfCoinRoom == 0)
		{
			text_EffectCoinRoom.text = "-";
			text_CostCoinRoom.text = talentUpgrade2.coinRoom[0].cost.ToString();
		}
		else
		{
			text_EffectCoinRoom.text = "+" + talentUpgrade2.coinRoom[worldData.levelOfCoinRoom - 1].value + "% " + 1000515.GetText();
			if (worldData.levelOfCoinRoom == talentUpgrade2.coinRoom.Length)
			{
				text_CostCoinRoom.text = 1000512.GetText();
			}
			else
			{
				text_CostCoinRoom.text = talentUpgrade2.coinRoom[worldData.levelOfCoinRoom].cost.ToString();
			}
		}
		if (worldData.levelOfRelicRoom == 0)
		{
			text_EffectRelicRoom.text = "-";
			text_CostRelicRoom.text = talentUpgrade2.relicRoom[0].cost.ToString();
		}
		else
		{
			text_EffectRelicRoom.text = 1000514.GetText() + " ×" + (100 + talentUpgrade2.relicRoom[worldData.levelOfRelicRoom - 1].value) + "%";
			if (worldData.levelOfRelicRoom == talentUpgrade2.relicRoom.Length)
			{
				text_CostRelicRoom.text = 1000512.GetText();
			}
			else
			{
				text_CostRelicRoom.text = talentUpgrade2.relicRoom[worldData.levelOfRelicRoom].cost.ToString();
			}
		}
		if (worldData.levelOfSpellRoom == 0)
		{
			text_EffectSpellRoom.text = "-";
			text_CostSpellRoom.text = talentUpgrade2.spellRoom[0].cost.ToString();
		}
		else
		{
			text_EffectSpellRoom.text = 1000513.GetText() + " ×" + (100 + talentUpgrade2.spellRoom[worldData.levelOfSpellRoom - 1].value) + "%";
			if (worldData.levelOfSpellRoom == talentUpgrade2.spellRoom.Length)
			{
				text_CostSpellRoom.text = 1000512.GetText();
			}
			else
			{
				text_CostSpellRoom.text = talentUpgrade2.spellRoom[worldData.levelOfSpellRoom].cost.ToString();
			}
		}
		if (worldData.levelOfMaxMP == 0)
		{
			text_EffectMaxMP.text = "-";
			text_CostMaxMP.text = talentUpgrade2.maxMP[0].cost.ToString();
		}
		else
		{
			text_EffectMaxMP.text = "+" + talentUpgrade2.maxMP[worldData.levelOfMaxMP - 1].value + " " + 1000510.GetText();
			if (worldData.levelOfMaxMP == talentUpgrade2.maxMP.Length)
			{
				text_CostMaxMP.text = 1000512.GetText();
			}
			else
			{
				text_CostMaxMP.text = talentUpgrade2.maxMP[worldData.levelOfMaxMP].cost.ToString();
			}
		}
		if (worldData.levelOfMPRecover == 0)
		{
			text_EffectMPRecover.text = "-";
			text_CostMPRecover.text = talentUpgrade2.mpRecover[0].cost.ToString();
		}
		else
		{
			text_EffectMPRecover.text = "+" + talentUpgrade2.mpRecover[worldData.levelOfMPRecover - 1].value + " " + 1000511.GetText();
			if (worldData.levelOfMPRecover == talentUpgrade2.mpRecover.Length)
			{
				text_CostMPRecover.text = 1000512.GetText();
			}
			else
			{
				text_CostMPRecover.text = talentUpgrade2.mpRecover[worldData.levelOfMPRecover].cost.ToString();
			}
		}
		if (worldData.levelOfWandLimit > 0 || worldData.levelOfBagLimit > 0 || worldData.levelOfEnterDoorRecovery > 0 || worldData.levelOfMaxHP > 0 || worldData.levelOfHPRoom > 0 || worldData.levelOfInitialCoin > 0 || worldData.levelOfSpellRoom > 0 || worldData.levelOfRelicRoom > 0 || worldData.levelOfCoinRoom > 0 || worldData.levelOfMaxMP > 0 || worldData.levelOfMPRecover > 0)
		{
			btn_Reset.interactable = true;
		}
		else
		{
			btn_Reset.interactable = false;
		}
		go_MaskReset.SetActive(!btn_Reset.interactable);
		if (worldData.levelOfWandLimit < talentUpgrade2.wandLimit.Length && worldData.magicCrystalCount >= talentUpgrade2.wandLimit[worldData.levelOfWandLimit].cost)
		{
			btn_WandLimitUp.interactable = true;
		}
		else
		{
			btn_WandLimitUp.interactable = false;
		}
		if (worldData.levelOfWandLimit > 0)
		{
			btn_WandLimitDown.gameObject.SetActive(value: true);
		}
		else
		{
			btn_WandLimitDown.gameObject.SetActive(value: false);
		}
		go_MaskWandLimitUp.SetActive(!btn_WandLimitUp.interactable);
		if (worldData.levelOfBagLimit < talentUpgrade2.bagLimit.Length && worldData.magicCrystalCount >= talentUpgrade2.bagLimit[worldData.levelOfBagLimit].cost)
		{
			btn_BagLimitUp.interactable = true;
		}
		else
		{
			btn_BagLimitUp.interactable = false;
		}
		if (worldData.levelOfBagLimit > 0)
		{
			btn_BagLimitDown.gameObject.SetActive(value: true);
		}
		else
		{
			btn_BagLimitDown.gameObject.SetActive(value: false);
		}
		go_MaskBagLimitUp.SetActive(!btn_BagLimitUp.interactable);
		if (worldData.levelOfEnterDoorRecovery < talentUpgrade2.enterDoorRecovery.Length && worldData.magicCrystalCount >= talentUpgrade2.enterDoorRecovery[worldData.levelOfEnterDoorRecovery].cost)
		{
			btn_EnterDoorRecoveryUp.interactable = true;
		}
		else
		{
			btn_EnterDoorRecoveryUp.interactable = false;
		}
		if (worldData.levelOfEnterDoorRecovery > 0)
		{
			btn_EnterDoorRecoveryDown.gameObject.SetActive(value: true);
		}
		else
		{
			btn_EnterDoorRecoveryDown.gameObject.SetActive(value: false);
		}
		go_MaskEnterDoorRecoveryUp.SetActive(!btn_EnterDoorRecoveryUp.interactable);
		if (worldData.levelOfMaxHP < talentUpgrade2.maxHP.Length && worldData.magicCrystalCount >= talentUpgrade2.maxHP[worldData.levelOfMaxHP].cost)
		{
			btn_MaxHPUp.interactable = true;
		}
		else
		{
			btn_MaxHPUp.interactable = false;
		}
		if (worldData.levelOfMaxHP > 0)
		{
			btn_MaxHPDown.gameObject.SetActive(value: true);
		}
		else
		{
			btn_MaxHPDown.gameObject.SetActive(value: false);
		}
		go_MaskMaxHPUp.SetActive(!btn_MaxHPUp.interactable);
		if (worldData.levelOfHPRoom < talentUpgrade2.hpRoom.Length && worldData.magicCrystalCount >= talentUpgrade2.hpRoom[worldData.levelOfHPRoom].cost)
		{
			btn_HPRoomUp.interactable = true;
		}
		else
		{
			btn_HPRoomUp.interactable = false;
		}
		if (worldData.levelOfHPRoom > 0)
		{
			btn_HPRoomDown.gameObject.SetActive(value: true);
		}
		else
		{
			btn_HPRoomDown.gameObject.SetActive(value: false);
		}
		go_MaskHPRoomUp.SetActive(!btn_HPRoomUp.interactable);
		if (worldData.levelOfInitialCoin < talentUpgrade2.initialCoin.Length && worldData.magicCrystalCount >= talentUpgrade2.initialCoin[worldData.levelOfInitialCoin].cost)
		{
			btn_InitialCoinUp.interactable = true;
		}
		else
		{
			btn_InitialCoinUp.interactable = false;
		}
		if (worldData.levelOfInitialCoin > 0)
		{
			btn_InitialCoinDown.gameObject.SetActive(value: true);
		}
		else
		{
			btn_InitialCoinDown.gameObject.SetActive(value: false);
		}
		go_MaskInitialCoinUp.SetActive(!btn_InitialCoinUp.interactable);
		if (worldData.levelOfCoinRoom < talentUpgrade2.coinRoom.Length && worldData.magicCrystalCount >= talentUpgrade2.coinRoom[worldData.levelOfCoinRoom].cost)
		{
			btn_CoinRoomUp.interactable = true;
		}
		else
		{
			btn_CoinRoomUp.interactable = false;
		}
		if (worldData.levelOfCoinRoom > 0)
		{
			btn_CoinRoomDown.gameObject.SetActive(value: true);
		}
		else
		{
			btn_CoinRoomDown.gameObject.SetActive(value: false);
		}
		go_MaskCoinRoomUp.SetActive(!btn_CoinRoomUp.interactable);
		if (worldData.levelOfRelicRoom < talentUpgrade2.relicRoom.Length && worldData.magicCrystalCount >= talentUpgrade2.relicRoom[worldData.levelOfRelicRoom].cost)
		{
			btn_RelicRoomUp.interactable = true;
		}
		else
		{
			btn_RelicRoomUp.interactable = false;
		}
		if (worldData.levelOfRelicRoom > 0)
		{
			btn_RelicRoomDown.gameObject.SetActive(value: true);
		}
		else
		{
			btn_RelicRoomDown.gameObject.SetActive(value: false);
		}
		go_MaskRelicRoomUp.SetActive(!btn_RelicRoomUp.interactable);
		if (worldData.levelOfSpellRoom < talentUpgrade2.spellRoom.Length && worldData.magicCrystalCount >= talentUpgrade2.spellRoom[worldData.levelOfSpellRoom].cost)
		{
			btn_SpellRoomUp.interactable = true;
		}
		else
		{
			btn_SpellRoomUp.interactable = false;
		}
		if (worldData.levelOfSpellRoom > 0)
		{
			btn_SpellRoomDown.gameObject.SetActive(value: true);
		}
		else
		{
			btn_SpellRoomDown.gameObject.SetActive(value: false);
		}
		go_MaskSpellRoomUp.SetActive(!btn_SpellRoomUp.interactable);
		if (worldData.levelOfMaxMP < talentUpgrade2.maxMP.Length && worldData.magicCrystalCount >= talentUpgrade2.maxMP[worldData.levelOfMaxMP].cost)
		{
			btn_MaxMPUp.interactable = true;
		}
		else
		{
			btn_MaxMPUp.interactable = false;
		}
		if (worldData.levelOfMaxMP > 0)
		{
			btn_MaxMPDown.gameObject.SetActive(value: true);
		}
		else
		{
			btn_MaxMPDown.gameObject.SetActive(value: false);
		}
		go_MaskMaxMPUp.SetActive(!btn_MaxMPUp.interactable);
		if (worldData.levelOfMPRecover < talentUpgrade2.mpRecover.Length && worldData.magicCrystalCount >= talentUpgrade2.mpRecover[worldData.levelOfMPRecover].cost)
		{
			btn_MPRecoverUp.interactable = true;
		}
		else
		{
			btn_MPRecoverUp.interactable = false;
		}
		if (worldData.levelOfMPRecover > 0)
		{
			btn_MPRecoverDown.gameObject.SetActive(value: true);
		}
		else
		{
			btn_MPRecoverDown.gameObject.SetActive(value: false);
		}
		go_MaskMPRecoverUp.SetActive(!btn_MPRecoverUp.interactable);
		MobileUpdateCrystalImage();
	}

	protected override IEnumerator OnInit()
	{
		worldData = DataMgr.selectedWorldData;
		talentUpgrade2 = ScriptableObjMgr.Inst.talentUpgrade2;
		text_Unlock1.text = talentUpgrade2.unlock1Cost.ToString();
		text_Unlock2.text = talentUpgrade2.unlock2Cost.ToString();
		text_Unlock3.text = talentUpgrade2.unlock3Cost.ToString();
		text_Unlock4.text = talentUpgrade2.unlock4Cost.ToString();
		LanguageChange();
		InputChange();
		AncienBloodChange();
		MagicCrystalChange();
		UpdateLock();
		yield return null;
	}

	private void UpdateLock()
	{
		if (GameMgr.IsMobile_Static)
		{
			mobileUnlockNext.SetActive(!worldData.isTalentUnlock4);
			unLock1.ForEach(delegate(GameObject x)
			{
				x.SetActive(worldData.isTalentUnlock1);
			});
			unLock2.ForEach(delegate(GameObject x)
			{
				x.SetActive(worldData.isTalentUnlock2);
			});
			unLock3.ForEach(delegate(GameObject x)
			{
				x.SetActive(worldData.isTalentUnlock3);
			});
			unLock4.ForEach(delegate(GameObject x)
			{
				x.SetActive(worldData.isTalentUnlock4);
			});
			if (!worldData.isTalentUnlock1)
			{
				mobileUnlockButtonCost.text = talentUpgrade2.unlock1Cost.ToString();
			}
			else if (!worldData.isTalentUnlock2)
			{
				mobileUnlockButtonCost.text = talentUpgrade2.unlock2Cost.ToString();
			}
			else if (!worldData.isTalentUnlock3)
			{
				mobileUnlockButtonCost.text = talentUpgrade2.unlock3Cost.ToString();
			}
			else if (!worldData.isTalentUnlock4)
			{
				mobileUnlockButtonCost.text = talentUpgrade2.unlock4Cost.ToString();
			}
			mobileUnlockButtonCost.color = ((worldData.ancientBloodCount >= int.Parse(mobileUnlockButtonCost.text)) ? Color.green : Color.red);
			LayoutRebuilder.ForceRebuildLayoutImmediate(rebuildLayout);
		}
		else
		{
			go_LockTalent4.SetActive(worldData.isTalentUnlock1);
			go_LockTalent5.SetActive(worldData.isTalentUnlock1);
			go_LockTalent6.SetActive(worldData.isTalentUnlock2);
			go_LockTalent7.SetActive(worldData.isTalentUnlock2);
			go_LockTalent8.SetActive(worldData.isTalentUnlock3);
			go_LockTalent9.SetActive(worldData.isTalentUnlock3);
			go_LockTalent10.SetActive(worldData.isTalentUnlock4);
			go_LockTalent11.SetActive(worldData.isTalentUnlock4);
			go_Unlock1.SetActive(!worldData.isTalentUnlock1);
			go_Unlock2.SetActive(!worldData.isTalentUnlock2);
			go_Unlock3.SetActive(!worldData.isTalentUnlock3);
			go_Unlock4.SetActive(!worldData.isTalentUnlock4);
		}
	}

	public void _MobileUnlockNex()
	{
		if (!worldData.isTalentUnlock1)
		{
			_Unlock(1);
		}
		else if (!worldData.isTalentUnlock2)
		{
			_Unlock(2);
		}
		else if (!worldData.isTalentUnlock3)
		{
			_Unlock(3);
		}
		else if (!worldData.isTalentUnlock4)
		{
			_Unlock(4);
		}
	}

	private void UpdateGamepadSelect(bool skipSE = false)
	{
		if (btn_LastSelect != null)
		{
			btn_LastSelect.animator.SetTrigger("Normal");
		}
		go_GamepadSelect.SetActive(value: false);
		go_GamepadUnlockSelect1.SetActive(value: false);
		go_GamepadUnlockSelect2.SetActive(value: false);
		go_GamepadUnlockSelect3.SetActive(value: false);
		go_GamepadUnlockSelect4.SetActive(value: false);
		if (gamepadIndex == -1)
		{
			btn_LastSelect = btn_Reset;
			go_GamepadSelect.SetActive(value: true);
			go_GamepadSelect.transform.position = btn_LastSelect.transform.position;
		}
		else if (gamepadIndex < btn_Ups.Length)
		{
			if (gamepadRight)
			{
				btn_LastSelect = btn_Ups[gamepadIndex];
			}
			else
			{
				btn_LastSelect = btn_Downs[gamepadIndex];
			}
			go_GamepadSelect.SetActive(value: true);
			go_GamepadSelect.transform.position = btn_LastSelect.transform.position;
		}
		else if (gamepadIndex == btn_Ups.Length)
		{
			btn_LastSelect = btn_Unlock1;
			go_GamepadUnlockSelect1.SetActive(value: true);
		}
		else if (gamepadIndex == btn_Ups.Length + 1)
		{
			btn_LastSelect = btn_Unlock2;
			go_GamepadUnlockSelect2.SetActive(value: true);
		}
		else if (gamepadIndex == btn_Ups.Length + 2)
		{
			btn_LastSelect = btn_Unlock3;
			go_GamepadUnlockSelect3.SetActive(value: true);
		}
		else if (gamepadIndex == btn_Ups.Length + 3)
		{
			btn_LastSelect = btn_Unlock4;
			go_GamepadUnlockSelect4.SetActive(value: true);
		}
		else
		{
			Debug.LogError("下标越界!");
		}
		if (skipSE)
		{
			btn_LastSelect.GetComponent<UIButtonEvent>().SKipOnceSE();
		}
		btn_LastSelect.animator.SetTrigger("Highlighted");
	}

	protected override void OnShow(object obj = null)
	{
		demoBlockPlayer.gameObject.SetActive(!ICJNOGPFMAM.MIFJADDOODN && !GameMgr.IsMobile_Static);
		talentChangeLogger = new TalentChangeLogger
		{
			before_talent = TalentStatus.CreateAuto()
		};
		talentChangeLogger.AutoRecordBeforeResources();
		if (GameMgr.IsMobile_Static)
		{
			UpdateLock();
		}
		UIPlayerDataMgr.Inst.UISlotBagExitall();
		SEMgr.Inst.uiOpen.PlaySE();
		anima.SetTrigger("Appear");
		UIMgr.TryAdditionalMobileShow(base.transform);
		PlayerMgr.Inst.PlayerCtrller.StopMotion();
		CamController.Inst.MouseOffsetPause();
		UIPlayerDataMgr.Inst.ResourceUIPopUp(UIPlayerDataMgr.ResourceUIPop.Blood);
		UIPlayerDataMgr.Inst.ResourceUIPopUp(UIPlayerDataMgr.ResourceUIPop.Crystal);
	}

	protected override void OnHide()
	{
		StopAllCoroutines();
		anima.SetTrigger("Disappear");
		UIMgr.TryAdditionalMobileHide(base.transform);
		PlayerMgr.Inst.PlayerCtrller.StartMotion();
		CamController.Inst.MouseOffsetContinue();
		DataMgr.SaveSelectedWorldData();
		SEMgr.Inst.uiClose.PlaySE();
		talentChangeLogger.after_talent = TalentStatus.CreateAuto();
		talentChangeLogger.AutoRecordAfterResourcesAndFlow();
		talentChangeLogger.Report();
		if (GameMgr.IsMobile_Static && talentChangeLogger.after_talent != talentChangeLogger.before_talent)
		{
			string properties = JsonConvert.SerializeObject(talentChangeLogger);
			PluginActivity.Inst.UploadEvent("talent_change", properties);
		}
		UIPlayerDataMgr.Inst.ResourceUISetToDefault(UIPlayerDataMgr.ResourceUIPop.Blood);
		UIPlayerDataMgr.Inst.ResourceUISetToDefault(UIPlayerDataMgr.ResourceUIPop.Crystal);
	}

	public void _Reset()
	{
		Debug.Log("_Reset");
		int talentSpentCrystal = worldData.GetTalentSpentCrystal();
		worldData.levelOfWandLimit = 0;
		worldData.levelOfBagLimit = 0;
		worldData.levelOfEnterDoorRecovery = 0;
		worldData.levelOfMaxHP = 0;
		worldData.levelOfHPRoom = 0;
		worldData.levelOfInitialCoin = 0;
		worldData.levelOfCoinRoom = 0;
		worldData.levelOfRelicRoom = 0;
		worldData.levelOfSpellRoom = 0;
		worldData.levelOfMaxMP = 0;
		worldData.levelOfMPRecover = 0;
		PlayerMgr.Inst.ChangeMagicCrystal(talentSpentCrystal);
		PlayerMgr.Inst.RefreshPlayer();
		UIPlayerDataMgr.Inst.UpdateAllInfo();
		SEMgr.Inst.uiTalentReset.PlaySE();
		DataMgr.selectedWorldData.CalculateAddingPoints();
		PlayerMgr.Inst.ItemCtrller.PotionChangeSlot(0);
	}

	public void _ChangeLevelWandLimit(bool isUp)
	{
		int value;
		if (isUp)
		{
			value = -talentUpgrade2.wandLimit[worldData.levelOfWandLimit].cost;
			worldData.levelOfWandLimit++;
			if (GameMgr.IsMobile_Static && !MobileMgr.inst.gamepadPlugged && !DataMgr.selectedWorldData.mobileWandDragTutorialShown && worldData.levelOfWandLimit > 0)
			{
				TopUI.inst.mobileWandDragTutorial.gameObject.SetActive(value: true);
			}
		}
		else
		{
			value = talentUpgrade2.wandLimit[worldData.levelOfWandLimit - 1].cost;
			worldData.levelOfWandLimit--;
			if (GameMgr.IsMobile_Static && TopUI.inst.mobileWandDragTutorial.gameObject.activeInHierarchy && worldData.levelOfWandLimit <= 0)
			{
				TopUI.inst.mobileWandDragTutorial.gameObject.SetActive(value: false);
			}
		}
		PlayerMgr.Inst.ChangeMagicCrystal(value);
		PlayerMgr.Inst.RefreshPlayer();
		SEMgr.Inst.uiTalentUpgrade.PlaySE();
	}

	public void _ChangeLevelBagLimit(bool isUp)
	{
		int value;
		if (isUp)
		{
			value = -talentUpgrade2.bagLimit[worldData.levelOfBagLimit].cost;
			worldData.levelOfBagLimit++;
		}
		else
		{
			value = talentUpgrade2.bagLimit[worldData.levelOfBagLimit - 1].cost;
			worldData.levelOfBagLimit--;
		}
		PlayerMgr.Inst.ChangeMagicCrystal(value);
		PlayerMgr.Inst.RefreshPlayer();
		SEMgr.Inst.uiTalentUpgrade.PlaySE();
	}

	public void _ChangeLevelMaxHP(bool isUp)
	{
		int value;
		if (isUp)
		{
			value = -talentUpgrade2.maxHP[worldData.levelOfMaxHP].cost;
			worldData.levelOfMaxHP++;
		}
		else
		{
			value = talentUpgrade2.maxHP[worldData.levelOfMaxHP - 1].cost;
			worldData.levelOfMaxHP--;
		}
		PlayerMgr.Inst.ChangeMagicCrystal(value);
		PlayerMgr.Inst.RefreshPlayer();
		SEMgr.Inst.uiTalentUpgrade.PlaySE();
	}

	public void _ChangeLevelEnterDoorRecovery(bool isUp)
	{
		int value;
		if (isUp)
		{
			value = -talentUpgrade2.enterDoorRecovery[worldData.levelOfEnterDoorRecovery].cost;
			worldData.levelOfEnterDoorRecovery++;
		}
		else
		{
			value = talentUpgrade2.enterDoorRecovery[worldData.levelOfEnterDoorRecovery - 1].cost;
			worldData.levelOfEnterDoorRecovery--;
		}
		PlayerMgr.Inst.ChangeMagicCrystal(value);
		SEMgr.Inst.uiTalentUpgrade.PlaySE();
	}

	public void _ChangeLevelHPRoom(bool isUp)
	{
		int value;
		if (isUp)
		{
			value = -talentUpgrade2.hpRoom[worldData.levelOfHPRoom].cost;
			worldData.levelOfHPRoom++;
		}
		else
		{
			value = talentUpgrade2.hpRoom[worldData.levelOfHPRoom - 1].cost;
			worldData.levelOfHPRoom--;
		}
		PlayerMgr.Inst.ChangeMagicCrystal(value);
		SEMgr.Inst.uiTalentUpgrade.PlaySE();
	}

	public void _ChangeLevelInitialCoin(bool isUp)
	{
		int value;
		if (isUp)
		{
			value = -talentUpgrade2.initialCoin[worldData.levelOfInitialCoin].cost;
			worldData.levelOfInitialCoin++;
		}
		else
		{
			value = talentUpgrade2.initialCoin[worldData.levelOfInitialCoin - 1].cost;
			worldData.levelOfInitialCoin--;
		}
		PlayerMgr.Inst.ChangeMagicCrystal(value);
		PlayerMgr.Inst.RefreshPlayer();
		UIPlayerDataMgr.Inst.UpdateCoin();
		SEMgr.Inst.uiTalentUpgrade.PlaySE();
	}

	public void _ChangeLevelCoinRoom(bool isUp)
	{
		int value;
		if (isUp)
		{
			value = -talentUpgrade2.coinRoom[worldData.levelOfCoinRoom].cost;
			worldData.levelOfCoinRoom++;
		}
		else
		{
			value = talentUpgrade2.coinRoom[worldData.levelOfCoinRoom - 1].cost;
			worldData.levelOfCoinRoom--;
		}
		PlayerMgr.Inst.ChangeMagicCrystal(value);
		SEMgr.Inst.uiTalentUpgrade.PlaySE();
	}

	public void _ChangeLevelRelicRoom(bool isUp)
	{
		int value;
		if (isUp)
		{
			value = -talentUpgrade2.relicRoom[worldData.levelOfRelicRoom].cost;
			worldData.levelOfRelicRoom++;
		}
		else
		{
			value = talentUpgrade2.relicRoom[worldData.levelOfRelicRoom - 1].cost;
			worldData.levelOfRelicRoom--;
		}
		PlayerMgr.Inst.ChangeMagicCrystal(value);
		SEMgr.Inst.uiTalentUpgrade.PlaySE();
	}

	public void _ChangeLevelSpellRoom(bool isUp)
	{
		int value;
		if (isUp)
		{
			value = -talentUpgrade2.spellRoom[worldData.levelOfSpellRoom].cost;
			worldData.levelOfSpellRoom++;
		}
		else
		{
			value = talentUpgrade2.spellRoom[worldData.levelOfSpellRoom - 1].cost;
			worldData.levelOfSpellRoom--;
		}
		PlayerMgr.Inst.ChangeMagicCrystal(value);
		SEMgr.Inst.uiTalentUpgrade.PlaySE();
	}

	public void _ChangeLevelMaxMP(bool isUp)
	{
		int value;
		if (isUp)
		{
			value = -talentUpgrade2.maxMP[worldData.levelOfMaxMP].cost;
			worldData.levelOfMaxMP++;
		}
		else
		{
			value = talentUpgrade2.maxMP[worldData.levelOfMaxMP - 1].cost;
			worldData.levelOfMaxMP--;
		}
		PlayerMgr.Inst.ChangeMagicCrystal(value);
		PlayerMgr.Inst.RefreshPlayer();
		SEMgr.Inst.uiTalentUpgrade.PlaySE();
	}

	public void _ChangeLevelMPRecover(bool isUp)
	{
		int value;
		if (isUp)
		{
			value = -talentUpgrade2.mpRecover[worldData.levelOfMPRecover].cost;
			worldData.levelOfMPRecover++;
		}
		else
		{
			value = talentUpgrade2.mpRecover[worldData.levelOfMPRecover - 1].cost;
			worldData.levelOfMPRecover--;
		}
		PlayerMgr.Inst.ChangeMagicCrystal(value);
		PlayerMgr.Inst.RefreshPlayer();
		SEMgr.Inst.uiTalentUpgrade.PlaySE();
	}

	public override void _Close()
	{
		if (!GameMgr.IsMobile_Static)
		{
			SEMgr.Inst.uiClick.PlaySE();
		}
		Hide();
	}

	public void _Unlock(int stage)
	{
		if (GameMgr.IsMobile_Static)
		{
			mobileGamepadUpdateFrame.SetActive(value: false);
		}
		switch (stage)
		{
		case 1:
			if (worldData.ancientBloodCount >= talentUpgrade2.unlock1Cost)
			{
				PlayerMgr.Inst.ChangeAncientBlood(-talentUpgrade2.unlock1Cost);
				worldData.isTalentUnlock1 = true;
				UpdateLock();
				SEMgr.Inst.uiTalentUnlock.PlaySE();
				if (UIMgr.Inst.InputType == PlayerInputType.Gamepad)
				{
					gamepadIndex = 3;
					gamepadRight = true;
					UpdateGamepadSelect();
				}
			}
			break;
		case 2:
			if (worldData.ancientBloodCount >= talentUpgrade2.unlock2Cost)
			{
				PlayerMgr.Inst.ChangeAncientBlood(-talentUpgrade2.unlock2Cost);
				worldData.isTalentUnlock2 = true;
				UpdateLock();
				SEMgr.Inst.uiTalentUnlock.PlaySE();
				if (UIMgr.Inst.InputType == PlayerInputType.Gamepad)
				{
					gamepadIndex = 5;
					gamepadRight = true;
					UpdateGamepadSelect();
				}
			}
			break;
		case 3:
			if (worldData.ancientBloodCount >= talentUpgrade2.unlock3Cost)
			{
				PlayerMgr.Inst.ChangeAncientBlood(-talentUpgrade2.unlock3Cost);
				worldData.isTalentUnlock3 = true;
				UpdateLock();
				SEMgr.Inst.uiTalentUnlock.PlaySE();
				if (UIMgr.Inst.InputType == PlayerInputType.Gamepad)
				{
					gamepadIndex = 7;
					gamepadRight = true;
					UpdateGamepadSelect();
				}
			}
			break;
		case 4:
			if (worldData.ancientBloodCount >= talentUpgrade2.unlock4Cost)
			{
				PlayerMgr.Inst.ChangeAncientBlood(-talentUpgrade2.unlock4Cost);
				worldData.isTalentUnlock4 = true;
				UpdateLock();
				SEMgr.Inst.uiTalentUnlock.PlaySE();
				if (UIMgr.Inst.InputType == PlayerInputType.Gamepad)
				{
					gamepadIndex = 9;
					gamepadRight = true;
					UpdateGamepadSelect();
				}
			}
			break;
		default:
			Debug.LogError(stage);
			break;
		}
	}
}
