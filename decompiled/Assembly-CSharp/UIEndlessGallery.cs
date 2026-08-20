using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[GameUISingletonPrefab("UIEndlessGallery")]
public class UIEndlessGallery : GameUISingletonMono<UIEndlessGallery>
{
	public GameObject rootObj;

	public CanvasGroup CanvasGroup;

	public Animator anima;

	public Toggle toggle_Monster;

	public Toggle toggle_Elite;

	public Toggle toggle_Boss;

	public GameObject go_ToggleOnMonster;

	public GameObject go_ToggleOnElite;

	public GameObject go_ToggleOnBoss;

	public GameObject panel_Monster;

	public GameObject panel_Elite;

	public GameObject panel_Boss;

	public Text text_StatisticsMonster;

	public Text text_StatisticsElite;

	public Text text_StatisticsBoss;

	public RectTransform rtsf_SlotParentMonster;

	public RectTransform rtsf_SlotParentElite;

	public RectTransform rtsf_SlotParentBoss;

	public GridLayoutGroup glg_Monster;

	public GridLayoutGroup glg_Elite;

	public GridLayoutGroup glg_Boss;

	[Header("Desc")]
	public Image image_UnitIcon;

	public Text text_UnitName;

	public Text text_UnitDesc;

	public Text text_UnitKilled;

	private UIEndlessGallerySlot hoveredSlot;

	private int ToggleIndex;

	private UIEndlessGallerySlot[] firstSlot = new UIEndlessGallerySlot[3];

	protected override void RegistarWhenInit()
	{
		EventMgr.LanguageChange = (Action)Delegate.Combine(EventMgr.LanguageChange, new Action(LanguageChange));
		EventMgr.InputChange = (Action)Delegate.Combine(EventMgr.InputChange, new Action(InputChange));
	}

	protected override void RegistarOnlyWhenOpen()
	{
	}

	protected override void UnRegistarOnlyWhenHide()
	{
	}

	protected override void UnRegistarWhenDestroy()
	{
		EventMgr.LanguageChange = (Action)Delegate.Remove(EventMgr.LanguageChange, new Action(LanguageChange));
		EventMgr.InputChange = (Action)Delegate.Remove(EventMgr.InputChange, new Action(InputChange));
	}

	private void LanguageChange()
	{
	}

	private void InputChange()
	{
	}

	protected override IEnumerator OnInit()
	{
		LanguageChange();
		InputChange();
		rootObj.SetActive(value: true);
		CanvasGroup.alpha = 1f;
		anima.enabled = true;
		ToggleIndex = 0;
		toggle_Monster.onValueChanged.AddListener(OnToggleChange);
		toggle_Elite.onValueChanged.AddListener(OnToggleChange);
		toggle_Boss.onValueChanged.AddListener(OnToggleChange);
		rtsf_SlotParentMonster.DestroyAllChildImmediate();
		rtsf_SlotParentElite.DestroyAllChildImmediate();
		rtsf_SlotParentBoss.DestroyAllChildImmediate();
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		int num5 = 0;
		int num6 = 0;
		foreach (UnitConfig item in UnitConfig.list)
		{
			if (item.appearChapter != 500f)
			{
				continue;
			}
			UIEndlessGallerySlot component = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Prefabs/UI/UIEndlessGallerySlot")).GetComponent<UIEndlessGallerySlot>();
			bool flag = false;
			switch (item.unitType)
			{
			case UnitType.Monster:
				if (firstSlot[0] == null)
				{
					firstSlot[0] = component;
				}
				component.transform.SetParent(rtsf_SlotParentMonster);
				flag = DataMgr.selectedWorldData.galleryUnlockedMonsters.Contains(item.id);
				num++;
				if (flag)
				{
					num4++;
				}
				break;
			case UnitType.Elite:
				if (firstSlot[1] == null)
				{
					firstSlot[1] = component;
				}
				component.transform.SetParent(rtsf_SlotParentElite);
				flag = DataMgr.selectedWorldData.galleryUnlockedBosses.Contains(item.id);
				num2++;
				if (flag)
				{
					num5++;
				}
				break;
			case UnitType.Boss:
				if (firstSlot[2] == null)
				{
					firstSlot[2] = component;
				}
				component.transform.SetParent(rtsf_SlotParentBoss);
				flag = DataMgr.selectedWorldData.galleryUnlockedBosses.Contains(item.id);
				num3++;
				if (flag)
				{
					num6++;
				}
				break;
			default:
				Debug.LogError(item.unitType);
				break;
			}
			component.Initialize(this, item.id, flag);
			component.transform.localScale = Vector3.one;
		}
		text_StatisticsMonster.text = num4 + "/" + num;
		text_StatisticsElite.text = num5 + "/" + num2;
		text_StatisticsBoss.text = num6 + "/" + num3;
		yield return null;
		yield return null;
		yield return null;
	}

	protected override void OnShow(object obj = null)
	{
		SEMgr.Inst.uiChangeLabel.PlaySE();
		UIPlayerDataMgr.Inst.UISlotBagExitall();
		InputChange();
		anima.SetTrigger("Appear");
		UIMgr.TryAdditionalMobileShow(base.transform);
		PlayerMgr.Inst.PlayerCtrller.StopMotion();
		PlayerMgr.Inst.PlayerCtrller.StopFace(PlayerMgr.Inst.PlayerDir.x < 0f);
		base.inputActions.Player.KeyboardA.performed += APerformed;
		base.inputActions.Player.KeyboardD.performed += DPerformed;
		UpdateToggle();
	}

	private void APerformed(InputAction.CallbackContext obj)
	{
		ToggleIndex--;
		ToggleIndex = ((ToggleIndex < 0) ? 2 : ToggleIndex);
		UpdateToggle();
	}

	private void DPerformed(InputAction.CallbackContext obj)
	{
		ToggleIndex++;
		ToggleIndex = ((ToggleIndex <= 2) ? ToggleIndex : 0);
		UpdateToggle();
	}

	private void UpdateToggle()
	{
		if (ToggleIndex == 0)
		{
			toggle_Boss.isOn = false;
			toggle_Elite.isOn = false;
			toggle_Monster.isOn = true;
		}
		else if (ToggleIndex == 1)
		{
			toggle_Boss.isOn = false;
			toggle_Monster.isOn = false;
			toggle_Elite.isOn = true;
		}
		else if (ToggleIndex == 2)
		{
			toggle_Monster.isOn = false;
			toggle_Elite.isOn = false;
			toggle_Boss.isOn = true;
		}
	}

	protected override void OnHide()
	{
		StopAllCoroutines();
		anima.SetTrigger("Disappear");
		UIMgr.TryAdditionalMobileHide(base.transform);
		PlayerMgr.Inst.PlayerCtrller.StartMotion();
		SEMgr.Inst.uiChangeLabelClose.PlaySE();
		base.inputActions.Player.KeyboardA.performed -= APerformed;
		base.inputActions.Player.KeyboardD.performed -= DPerformed;
	}

	private void OnToggleChange(bool value)
	{
		if (value)
		{
			panel_Monster.SetActive(toggle_Monster.isOn);
			panel_Elite.SetActive(toggle_Elite.isOn);
			panel_Boss.SetActive(toggle_Boss.isOn);
			go_ToggleOnMonster.SetActive(toggle_Monster.isOn);
			go_ToggleOnElite.SetActive(toggle_Elite.isOn);
			go_ToggleOnBoss.SetActive(toggle_Boss.isOn);
			int toggleIndex = ToggleIndex;
			ToggleIndex = (toggle_Boss.isOn ? 2 : (toggle_Elite.isOn ? 1 : 0));
			if (toggleIndex != ToggleIndex)
			{
				SlotEnter(firstSlot[ToggleIndex]);
			}
		}
	}

	public void SlotEnter(UIEndlessGallerySlot slot)
	{
		if (slot == null)
		{
			return;
		}
		if (hoveredSlot != null)
		{
			hoveredSlot.Unhover();
		}
		hoveredSlot = slot;
		hoveredSlot.Hover();
		image_UnitIcon.sprite = ABResources.LoadAsset<Sprite>(UnitConfig.map[slot.UnitID].GetModelPath());
		if (slot.IsUnlocked)
		{
			image_UnitIcon.color = Color.black;
			text_UnitName.text = "???";
			text_UnitKilled.gameObject.SetActive(value: false);
			text_UnitDesc.text = "";
			return;
		}
		image_UnitIcon.color = Color.white;
		text_UnitName.text = UnitConfig.map[slot.UnitID].GetName();
		if (ScriptableObjMgr.Inst.testCtrller.ShowItemID)
		{
			text_UnitName.text += $"({slot.UnitID})";
		}
		text_UnitKilled.gameObject.SetActive(value: true);
		text_UnitKilled.text = 1000409.GetText() + ": ";
		if (DataMgr.selectedWorldData.galleryKilledMonsterCounts.ContainsKey(slot.UnitID))
		{
			text_UnitKilled.text += DataMgr.selectedWorldData.galleryKilledMonsterCounts[slot.UnitID];
		}
		else
		{
			text_UnitKilled.text += "0";
		}
		text_UnitDesc.text = UnitConfig.map[slot.UnitID].GetDesc();
		text_UnitDesc.text = GeneralTool.FormatTextIfPublishTest(text_UnitDesc, text_UnitDesc.text);
	}

	public override void _Close()
	{
		SEMgr.Inst.uiButtonHover_Button.PlaySE();
		Hide();
	}
}
