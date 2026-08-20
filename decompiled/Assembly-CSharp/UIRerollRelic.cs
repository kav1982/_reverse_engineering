using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[GameUISingletonPrefab("UIRerollRelic")]
public class UIRerollRelic : GameUISingletonMono<UIRerollRelic>
{
	public UIRerollRelic_Relic pfb_UIRerollRelic_Relic;

	public RectTransform rtsf_Mask;

	public RectTransform rtsf_Center;

	public RectTransform rtsf_Relics;

	public Button btn_Reroll;

	public Animator anima;

	public UIInfoRelic uiInfoRelic;

	public Vector3 followOffset;

	public float rerollFinishTime;

	[Header("Focus")]
	public Vector3 focusOffset;

	public float focusSize;

	public float focusTime;

	[Header("InputChange")]
	public GameObject panel_Arrow;

	public Image image_Shortcut;

	public Sprite sprite_ShortcutKeyborad;

	public Sprite sprite_ShortcutGamepad;

	[Header("LanguageChange")]
	public Text text_Reroll;

	[Header("ControlChangeUI")]
	public UpdatButtonShow[] updatebuttonshows;

	private List<UIRerollRelic_Relic> uiRRRs = new List<UIRerollRelic_Relic>();

	private bool isUsed;

	public int SelectedIndex { get; private set; }

	private UIRerollRelic_Relic SelectedUIRRR => uiRRRs[SelectedIndex];

	private RelicConfig SelectedRelicCfg => SelectedUIRRR.RelicCfg;

	protected override void RegistarWhenInit()
	{
		EventMgr.LanguageChange = (Action)Delegate.Combine(EventMgr.LanguageChange, new Action(LanguageChange));
		EventMgr.InputChange = (Action)Delegate.Combine(EventMgr.InputChange, new Action(InputChange));
		EventMgr.ControlChange = (Action)Delegate.Combine(EventMgr.ControlChange, new Action(ControlChange));
	}

	protected override void RegistarOnlyWhenOpen()
	{
		base.inputActions.Player.GamepadDirect.performed += DirectPerformed;
		base.inputActions.Player.LeftStick.performed += DirectPerformed_Stick;
		base.inputActions.Player.WASD.performed += DirectPerformed;
		base.inputActions.Player.Interact.performed += InteractPerformed;
	}

	protected override void UnRegistarOnlyWhenHide()
	{
		base.inputActions.Player.GamepadDirect.performed -= DirectPerformed;
		base.inputActions.Player.WASD.performed -= DirectPerformed;
		base.inputActions.Player.Interact.performed -= InteractPerformed;
		base.inputActions.Player.LeftStick.performed -= DirectPerformed_Stick;
	}

	protected override void UnRegistarWhenDestroy()
	{
		EventMgr.LanguageChange = (Action)Delegate.Remove(EventMgr.LanguageChange, new Action(LanguageChange));
		EventMgr.InputChange = (Action)Delegate.Remove(EventMgr.InputChange, new Action(InputChange));
		EventMgr.ControlChange = (Action)Delegate.Remove(EventMgr.ControlChange, new Action(ControlChange));
	}

	private void DirectPerformed(InputAction.CallbackContext context)
	{
		if (base.IsOpen && !isUsed)
		{
			Vector2 direct = context.ReadValue<Vector2>();
			MoveDirect(direct);
		}
	}

	private void DirectPerformed_Stick(InputAction.CallbackContext context)
	{
		if (base.IsOpen && !isUsed)
		{
			Vector2 vector = context.ReadValue<Vector2>();
			vector = ControlMgr.Inst.RampVector2(vector);
			MoveDirect(vector);
		}
	}

	private void MoveDirect(Vector2 _direct)
	{
		if (_direct.x < 0f)
		{
			InteractLeft();
		}
		else if (_direct.x > 0f)
		{
			InteractRight();
		}
	}

	public void InteractLeft()
	{
		if (SelectedIndex > 0)
		{
			SelectedIndex--;
			uiInfoRelic.UpdateInfo(SelectedRelicCfg);
			for (int i = 0; i < uiRRRs.Count; i++)
			{
				uiRRRs[i].SetMove(i);
			}
			SEMgr.Inst.uiSwitch.PlaySE();
		}
	}

	public void InteractRight()
	{
		if (SelectedIndex < uiRRRs.Count - 1)
		{
			SelectedIndex++;
			uiInfoRelic.UpdateInfo(SelectedRelicCfg);
			for (int i = 0; i < uiRRRs.Count; i++)
			{
				uiRRRs[i].SetMove(i);
			}
			SEMgr.Inst.uiSwitch.PlaySE();
		}
	}

	private void InteractPerformed(InputAction.CallbackContext context)
	{
		if (base.IsOpen && !isUsed)
		{
			_Reroll();
		}
	}

	private void LanguageChange()
	{
		text_Reroll.text = 1000904.GetText();
	}

	private void InputChange()
	{
		ControlChange();
		if (!GameMgr.IsMobile_Static)
		{
			switch (UIMgr.Inst.InputType)
			{
			case PlayerInputType.Keyboard:
				panel_Arrow.SetActive(value: true);
				break;
			case PlayerInputType.Gamepad:
				panel_Arrow.SetActive(value: false);
				break;
			default:
				Debug.LogError(UIMgr.Inst.InputType);
				break;
			}
		}
	}

	private void ControlChange()
	{
		if (!GameMgr.IsMobile_Static)
		{
			UpdatButtonShow[] array = updatebuttonshows;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].UpdateButton();
			}
		}
	}

	protected override IEnumerator OnInit()
	{
		LanguageChange();
		InputChange();
		ControlChange();
		yield return null;
	}

	private void Start()
	{
		EventMgr.PlayerDead = (Action)Delegate.Combine(EventMgr.PlayerDead, new Action(Hide));
	}

	public override void OnDestroy()
	{
		base.OnDestroy();
		EventMgr.PlayerDead = (Action)Delegate.Remove(EventMgr.PlayerDead, new Action(Hide));
	}

	private void Update()
	{
		if (base.IsOpen)
		{
			if (GameMgr.IsMobile_Static)
			{
				rtsf_Center.anchoredPosition = GeneralTool.WorldToCanvasLocalPoint_FitDisplay(PlayerMgr.Inst.PlayerPoint + followOffset);
			}
			else
			{
				rtsf_Center.anchoredPosition = GeneralTool.WorldToCanvasLocalPoint(PlayerMgr.Inst.PlayerPoint + followOffset);
			}
			rtsf_Mask.anchoredPosition = rtsf_Center.anchoredPosition;
		}
	}

	protected override void OnShow(object obj = null)
	{
		UIPlayerDataMgr.Inst.UISlotBagExitall();
		if (GameMgr.IsMobile_Static)
		{
			CamController.Inst.FocusOn(focusSize, focusTime, PlayerMgr.Inst.PlayerPointIgnoreZ + focusOffset + (Vector3)UIBattleMgr.Inst.UIRerollRelicPositionMobileDefault * CamController.Inst.FocusCamSizeRatio);
		}
		else
		{
			CamController.Inst.FocusOn(focusSize, focusTime, PlayerMgr.Inst.PlayerPointIgnoreZ + focusOffset);
		}
		anima.SetTrigger("Appear");
		UIMgr.TryAdditionalMobileShow(base.transform);
		PlayerMgr.Inst.PlayerCtrller.StopMotion();
		CamController.Inst.MouseOffsetPause();
		TimeScaleMgr.Inst.Pause();
		rtsf_Relics.DestroyAllChild();
		uiRRRs.Clear();
		SelectedIndex = 0;
		btn_Reroll.gameObject.SetActive(value: true);
		isUsed = false;
		int num = 0;
		for (int i = 0; i < PlayerMgr.Inst.BaData.relicCfgs.Count; i++)
		{
			if (PlayerMgr.Inst.BaData.relicCfgs[i].dropType != 0 && PlayerMgr.Inst.BaData.relicCfgs[i].dropType != ItemDropType.Special)
			{
				UIRerollRelic_Relic uIRerollRelic_Relic = UnityEngine.Object.Instantiate(pfb_UIRerollRelic_Relic, rtsf_Relics);
				uiRRRs.Add(uIRerollRelic_Relic);
				uIRerollRelic_Relic.Initialize(this, num, PlayerMgr.Inst.BaData.relicCfgs[i]);
				num++;
			}
		}
		if (uiRRRs.Count == 0)
		{
			uiInfoRelic.gameObject.SetActive(value: false);
			return;
		}
		uiInfoRelic.gameObject.SetActive(value: true);
		uiInfoRelic.UpdateInfo(SelectedUIRRR.RelicCfg);
	}

	protected override void OnHide()
	{
	}

	public void _Reroll()
	{
		SEMgr.Inst.so101_Reroll.PlaySE();
		int id = SelectedRelicCfg.id;
		int num = 0;
		int num2 = 0;
		while (true)
		{
			num2++;
			if (num2 >= 100)
			{
				Debug.LogError("超过100次");
				num = 999;
				break;
			}
			num = PlayerMgr.Inst.BaData.GetRelicFromPool(SelectedRelicCfg.dropType);
			if (num != SelectedRelicCfg.id && num != 40 && num != 69)
			{
				break;
			}
			PlayerMgr.Inst.BaData.BackRelicToPool(num, 1);
		}
		bool flag = false;
		for (int i = 0; i < PlayerMgr.Inst.BaData.relicCfgs.Count; i++)
		{
			if (PlayerMgr.Inst.BaData.relicCfgs[i].id == num)
			{
				flag = true;
				break;
			}
		}
		if (SelectedUIRRR.RelicCfg.level - 1 == 0)
		{
			PlayerMgr.Inst.ItemCtrller.RelicRemove(id, 1);
			SelectedUIRRR.ChangeConfig(RelicConfig.GetConfig(num));
			if (flag)
			{
				for (int j = 0; j < uiRRRs.Count; j++)
				{
					if (uiRRRs[j].RelicCfg.id == num && uiRRRs[j] != SelectedUIRRR)
					{
						UIRerollRelic_Relic uIRerollRelic_Relic = uiRRRs[j];
						uIRerollRelic_Relic.Fly(SelectedUIRRR);
						uiRRRs.Remove(uIRerollRelic_Relic);
						if (j < SelectedIndex)
						{
							SelectedIndex--;
						}
						break;
					}
				}
				for (int k = 0; k < uiRRRs.Count; k++)
				{
					uiRRRs[k].SetMove(k);
				}
			}
		}
		else
		{
			PlayerMgr.Inst.ItemCtrller.RelicRemove(id, 1);
			SelectedUIRRR.ChangeConfig(SelectedUIRRR.RelicCfg);
			UIRerollRelic_Relic uIRerollRelic_Relic2 = UnityEngine.Object.Instantiate(pfb_UIRerollRelic_Relic, rtsf_Relics);
			uIRerollRelic_Relic2.Initialize(this, SelectedIndex, RelicConfig.GetConfig(num));
			if (flag)
			{
				for (int l = 0; l < uiRRRs.Count; l++)
				{
					if (uiRRRs[l].RelicCfg.id == num)
					{
						UIRerollRelic_Relic uIRerollRelic_Relic3 = uiRRRs[l];
						uIRerollRelic_Relic3.Fly(uIRerollRelic_Relic2);
						uiRRRs.Remove(uIRerollRelic_Relic3);
						break;
					}
				}
			}
			uiRRRs.Insert(SelectedIndex, uIRerollRelic_Relic2);
			for (int m = 0; m < uiRRRs.Count; m++)
			{
				uiRRRs[m].SetMove(m);
			}
		}
		PlayerMgr.Inst.ItemCtrller.RelicAdd(num);
		PlayerMgr.Inst.PlayerCtrller.TextFloatQueueAdd(1002202.GetText() + ":" + RelicConfig.dic[num].GetName(), UITextFloatType.Normal);
		PlayerMgr.Inst.BaData.BackRelicToPool(id, 1);
		uiInfoRelic.UpdateInfo(SelectedUIRRR.RelicCfg);
		isUsed = true;
		btn_Reroll.gameObject.SetActive(value: false);
		DataMgr.selectedWorldData.GalleryUnlock(GalleryCategory.Relic, num);
		StartCoroutine(RerollFinish());
	}

	private IEnumerator RerollFinish()
	{
		yield return new WaitForSecondsRealtime(rerollFinishTime);
		anima.SetTrigger("Disappear");
		UIMgr.TryAdditionalMobileHide(base.transform);
		SetIsOpen(isOpen: false);
		PlayerMgr.Inst.PlayerCtrller.StartMotion();
		CamController.Inst.MouseOffsetContinue();
		CamController.Inst.FocusRecover(focusTime);
		TimeScaleMgr.Inst.Recovery();
	}
}
