using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[GameUISingletonPrefab("UICompound")]
public class UICompound : GameUISingletonMono<UICompound>
{
	public GameObject pfb_UIRecastSpell;

	public GameObject go_BtnMask;

	public RectTransform rtsf_Spells;

	public RectTransform rtsf_Center;

	public RectTransform rtsf_Mask;

	public Text text_Tip;

	public Animator anima;

	public UIInfoSpell uiSpellInfo1;

	public UIInfoSpell uiSpellInfo2;

	public float3 followOffset;

	public float compoundDuration;

	public float focusSize;

	public float focusTime;

	public UIGeneralCompoundMaterial GeneralMeterialDataBoard;

	[Header("InputChange")]
	public GameObject panel_Arrow;

	public Image image_Shortcut;

	public Sprite sprite_ShortcutKeyborad;

	public Sprite sprite_ShortcutGamepad;

	[Header("LanguageChange")]
	public Text text_Compound;

	private List<UIReroll_Spell> uiRSs = new List<UIReroll_Spell>();

	private Dictionary<int, int> dic_Splls = new Dictionary<int, int>();

	private bool isRecasting;

	private float recastDurationTimer;

	[Header("ControlChangeUI")]
	public UpdatButtonShow[] updatebuttonshows;

	private UIReroll_Spell generalLV1CompoundMaterialData;

	private UIReroll_Spell generalLV2CompoundMaterialData;

	private int generalMaterialUseCount;

	[SerializeField]
	private GameObject mobileArrow;

	private EntityManager ettMgr;

	private Entity so101CompoundEtt;

	public int SelectedSpellIndex { get; private set; }

	public UIReroll_Spell SelectedUIRS => uiRSs[SelectedSpellIndex];

	private SpellConfig SelectedSpellCfg => SpellConfig.dic[SelectedUIRS.SpellID];

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

	private void OnEnable()
	{
		generalMaterialUseCount = 0;
		generalLV1CompoundMaterialData = null;
		generalLV2CompoundMaterialData = null;
	}

	private void DirectPerformed(InputAction.CallbackContext context)
	{
		if (base.IsOpen)
		{
			Vector2 direct = context.ReadValue<Vector2>();
			MoveDirect(direct);
		}
	}

	private void DirectPerformed_Stick(InputAction.CallbackContext context)
	{
		if (base.IsOpen)
		{
			Vector2 vector = context.ReadValue<Vector2>();
			vector = ControlMgr.Inst.RampVector2(vector);
			MoveDirect(vector);
		}
	}

	private void MoveDirect(Vector2 _direct)
	{
		if (_direct == new Vector2(-1f, 0f))
		{
			InteractLeft();
		}
		else if (_direct == new Vector2(1f, 0f))
		{
			InteractRight();
		}
		else if (_direct == new Vector2(0f, 1f))
		{
			InteractUp();
		}
		else if (_direct == new Vector2(0f, -1f))
		{
			InteractDown();
		}
	}

	private void UpdateGeneralMaterialBoardCoverData()
	{
		GeneralMeterialDataBoard.SetCoverIndex(generalMaterialUseCount);
	}

	public void InteractUp()
	{
		if (GeneralMeterialDataBoard.gameObject.activeInHierarchy)
		{
			generalMaterialUseCount = Mathf.Min(++generalMaterialUseCount, 2);
			UpdateGeneralMaterialBoardCoverData();
			SEMgr.Inst.UIGeneralMaterialSelectUp.PlaySE();
		}
	}

	public void InteractDown()
	{
		if (GeneralMeterialDataBoard.gameObject.activeInHierarchy)
		{
			generalMaterialUseCount = Mathf.Max(--generalMaterialUseCount, 0);
			UpdateGeneralMaterialBoardState();
			SEMgr.Inst.UIGeneralMaterialSelectDown.PlaySE();
		}
	}

	public void InteractLeft()
	{
		if (SelectedSpellIndex > 0)
		{
			generalMaterialUseCount = 0;
			SelectedSpellIndex--;
			uiSpellInfo1.UpdateInfo(SelectedUIRS.SpellID);
			for (int i = 0; i < uiRSs.Count; i++)
			{
				uiRSs[i].SetMove(SelectedSpellIndex);
			}
			UpdateInfo();
			SEMgr.Inst.uiSwitch.PlaySE();
		}
	}

	public void InteractRight()
	{
		if (SelectedSpellIndex < uiRSs.Count - 1)
		{
			generalMaterialUseCount = 0;
			SelectedSpellIndex++;
			uiSpellInfo1.UpdateInfo(SelectedUIRS.SpellID);
			for (int i = 0; i < uiRSs.Count; i++)
			{
				uiRSs[i].SetMove(SelectedSpellIndex);
			}
			UpdateInfo();
			SEMgr.Inst.uiSwitch.PlaySE();
		}
	}

	private void InteractPerformed(InputAction.CallbackContext context)
	{
		if (base.IsOpen && !go_BtnMask.activeSelf)
		{
			_Compound();
		}
	}

	private void LanguageChange()
	{
		text_Compound.text = 1000901.GetText();
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

	private void Start()
	{
		ettMgr = World.DefaultGameObjectInjectionWorld.EntityManager;
		EventMgr.PlayerDead = (Action)Delegate.Combine(EventMgr.PlayerDead, new Action(Hide));
	}

	public override void OnDestroy()
	{
		base.OnDestroy();
		EventMgr.PlayerDead = (Action)Delegate.Remove(EventMgr.PlayerDead, new Action(Hide));
	}

	protected override IEnumerator OnInit()
	{
		LanguageChange();
		InputChange();
		ControlChange();
		yield return null;
	}

	protected override void OnShow(object obj = null)
	{
		if (!(obj is Entity))
		{
			return;
		}
		Entity entity = (so101CompoundEtt = (Entity)obj);
		UIPlayerDataMgr.Inst.UISlotBagExitall();
		anima.SetTrigger("Appear");
		UIMgr.TryAdditionalMobileShow(base.transform);
		generalMaterialUseCount = 0;
		PlayerMgr.Inst.PlayerCtrller.StopMotion();
		CamController.Inst.MouseOffsetPause();
		if (GameMgr.IsMobile_Static)
		{
			LocalTransform componentData = ettMgr.GetComponentData<LocalTransform>(so101CompoundEtt);
			CamController.Inst.FocusOn(focusSize, focusTime, componentData.Position + UIBattleMgr.Inst.UIProcessPositionDefault * CamController.Inst.FocusCamSizeRatio);
		}
		else
		{
			CamController.Inst.FocusOn(focusSize, focusTime);
		}
		uiRSs.Clear();
		dic_Splls.Clear();
		rtsf_Spells.DestroyAllChild();
		SelectedSpellIndex = 0;
		generalLV1CompoundMaterialData = null;
		generalLV2CompoundMaterialData = null;
		for (int i = 0; i < PlayerMgr.Inst.BaData.bagSpellDatas.Count; i++)
		{
			if (PlayerMgr.Inst.BaData.bagSpellDatas[i] != null)
			{
				if (dic_Splls.ContainsKey(PlayerMgr.Inst.BaData.bagSpellDatas[i].id))
				{
					dic_Splls[PlayerMgr.Inst.BaData.bagSpellDatas[i].id]++;
				}
				else
				{
					dic_Splls.Add(PlayerMgr.Inst.BaData.bagSpellDatas[i].id, 1);
				}
			}
		}
		for (int j = 0; j < PlayerMgr.Inst.BaData.wandCfgs.Count; j++)
		{
			if (PlayerMgr.Inst.BaData.wandCfgs[j] == null)
			{
				continue;
			}
			for (int k = 0; k < PlayerMgr.Inst.BaData.wandCfgs[j].normalSlots.Length; k++)
			{
				if (PlayerMgr.Inst.BaData.wandCfgs[j].normalSlots[k] != null && !PlayerMgr.Inst.BaData.wandCfgs[j].normalSlotIsLock[k])
				{
					if (dic_Splls.ContainsKey(PlayerMgr.Inst.BaData.wandCfgs[j].normalSlots[k].id))
					{
						dic_Splls[PlayerMgr.Inst.BaData.wandCfgs[j].normalSlots[k].id]++;
					}
					else
					{
						dic_Splls.Add(PlayerMgr.Inst.BaData.wandCfgs[j].normalSlots[k].id, 1);
					}
				}
			}
			for (int l = 0; l < PlayerMgr.Inst.BaData.wandCfgs[j].postSlots.Length; l++)
			{
				if (PlayerMgr.Inst.BaData.wandCfgs[j].postSlots[l] != null && !PlayerMgr.Inst.BaData.wandCfgs[j].postSlotIsLock[l])
				{
					if (dic_Splls.ContainsKey(PlayerMgr.Inst.BaData.wandCfgs[j].postSlots[l].id))
					{
						dic_Splls[PlayerMgr.Inst.BaData.wandCfgs[j].postSlots[l].id]++;
					}
					else
					{
						dic_Splls.Add(PlayerMgr.Inst.BaData.wandCfgs[j].postSlots[l].id, 1);
					}
				}
			}
		}
		int[][] array = dic_Splls.Select((KeyValuePair<int, int> e) => new int[2] { e.Key, e.Value }).ToArray();
		Array.Sort(array, (int[] a, int[] b) => b[1] - a[1]);
		int[][] array2 = array;
		foreach (int[] array3 in array2)
		{
			if (array3[0] == 0)
			{
				continue;
			}
			UIReroll_Spell component = UnityEngine.Object.Instantiate(pfb_UIRecastSpell, rtsf_Spells).GetComponent<UIReroll_Spell>();
			uiRSs.Add(component);
			component.Initialize(uiRSs.Count - 1, array3[0], array3[1]);
			SpellConfig spellConfig = SpellConfig.dic[array3[0]];
			if (spellConfig != null && spellConfig.abilityType == SpellAbilityType.SpellEmbryo)
			{
				switch (spellConfig.level)
				{
				case 1:
					generalLV1CompoundMaterialData = component;
					break;
				case 2:
					generalLV2CompoundMaterialData = component;
					break;
				}
			}
		}
		UpdateInfo();
	}

	protected override void OnHide()
	{
		anima.SetTrigger("Disappear");
		UIMgr.TryAdditionalMobileHide(base.transform);
		so101CompoundEtt = Entity.Null;
		PlayerMgr.Inst.PlayerCtrller.StartMotion();
		CamController.Inst.MouseOffsetContinue();
		CamController.Inst.FocusRecover(focusTime);
	}

	private void Update()
	{
		Compounding();
		Following();
		UpdateGeneralMaterialBoardState();
		if (base.IsOpen && GameMgr.IsMobile_Static && (bool)mobileArrow)
		{
			GameObject obj = mobileArrow;
			if ((object)obj != null)
			{
				SpellConfig selectedSpellCfg = SelectedSpellCfg;
				obj.SetActive(selectedSpellCfg != null && selectedSpellCfg.canCompound && selectedSpellCfg.level <= 3);
			}
		}
	}

	private void UpdateGeneralMaterialBoardState()
	{
		if (uiRSs.Count <= 0)
		{
			GeneralMeterialDataBoard.BoardToggle(toggle: false);
			return;
		}
		if (SelectedSpellCfg != null && SelectedSpellCfg.canCompound && SelectedSpellCfg.abilityType != SpellAbilityType.SpellEmbryo)
		{
			if (SelectedSpellCfg.level == 1)
			{
				if ((bool)generalLV1CompoundMaterialData && generalLV1CompoundMaterialData.Count > 0)
				{
					GeneralMeterialDataBoard.BoardToggle(toggle: true);
					GeneralMeterialDataBoard.SetGeneralMaterialLevel(SelectedSpellCfg.level, generalLV1CompoundMaterialData.Count);
					GeneralMeterialDataBoard.SetMaterialCount(generalLV1CompoundMaterialData.Count);
					if (generalMaterialUseCount > Mathf.Min(generalLV1CompoundMaterialData.Count, 2))
					{
						generalMaterialUseCount = generalLV1CompoundMaterialData.Count;
						UpdateGeneralMaterialBoardCoverData();
					}
					if (generalMaterialUseCount < 3 - SelectedUIRS.Count)
					{
						generalMaterialUseCount = Mathf.Max(0, 3 - SelectedUIRS.Count);
						UpdateGeneralMaterialBoardCoverData();
					}
				}
				else
				{
					generalLV1CompoundMaterialData = null;
					GeneralMeterialDataBoard.BoardToggle(toggle: false);
				}
			}
			else if (SelectedSpellCfg.level == 2)
			{
				if ((bool)generalLV2CompoundMaterialData && generalLV2CompoundMaterialData.Count > 0)
				{
					GeneralMeterialDataBoard.BoardToggle(toggle: true);
					GeneralMeterialDataBoard.SetGeneralMaterialLevel(SelectedSpellCfg.level, generalLV2CompoundMaterialData.Count);
					GeneralMeterialDataBoard.SetMaterialCount(generalLV2CompoundMaterialData.Count);
					if (generalMaterialUseCount > generalLV2CompoundMaterialData.Count)
					{
						generalMaterialUseCount = generalLV2CompoundMaterialData.Count;
						UpdateGeneralMaterialBoardCoverData();
					}
					if (generalMaterialUseCount < 3 - SelectedUIRS.Count)
					{
						generalMaterialUseCount = Mathf.Max(0, 3 - SelectedUIRS.Count);
						UpdateGeneralMaterialBoardCoverData();
					}
				}
				else
				{
					generalLV2CompoundMaterialData = null;
					GeneralMeterialDataBoard.BoardToggle(toggle: false);
				}
			}
		}
		else
		{
			GeneralMeterialDataBoard.BoardToggle(toggle: false);
			generalMaterialUseCount = 0;
		}
		GeneralMeterialDataBoard.SetUseCount(generalMaterialUseCount);
	}

	private void Compounding()
	{
		if (isRecasting)
		{
			recastDurationTimer += Time.deltaTime;
			if (recastDurationTimer >= compoundDuration)
			{
				isRecasting = false;
				UpdateInfo();
			}
		}
	}

	private void Following()
	{
		if (base.IsOpen)
		{
			LocalTransform componentData = ettMgr.GetComponentData<LocalTransform>(so101CompoundEtt);
			if (GameMgr.IsMobile_Static)
			{
				rtsf_Center.anchoredPosition = GeneralTool.WorldToCanvasLocalPoint_FitDisplay(componentData.Position + followOffset);
			}
			else
			{
				rtsf_Center.anchoredPosition = GeneralTool.WorldToCanvasLocalPoint(componentData.Position + followOffset);
			}
			rtsf_Mask.anchoredPosition = rtsf_Center.anchoredPosition;
		}
	}

	public void UpdateInfo()
	{
		if (uiRSs.Count == 0)
		{
			uiSpellInfo1.gameObject.SetActive(value: false);
			uiSpellInfo2.gameObject.SetActive(value: false);
		}
		else
		{
			uiSpellInfo1.gameObject.SetActive(value: true);
			uiSpellInfo1.UpdateInfo(SelectedUIRS.SpellID);
			if (SelectedSpellCfg.canCompound)
			{
				uiSpellInfo2.gameObject.SetActive(value: true);
				uiSpellInfo2.UpdateInfo(SelectedUIRS.SpellID + 1);
			}
			else
			{
				uiSpellInfo2.gameObject.SetActive(value: false);
			}
		}
		text_Tip.gameObject.SetActive(value: false);
		go_BtnMask.SetActive(value: true);
		if (uiRSs.Count != 0)
		{
			int level = SelectedUIRS.SpellID % 10;
			int num = SelectedUIRS.Count;
			UIReroll_Spell tarGetLevelCompoundMaterialData = GetTarGetLevelCompoundMaterialData(level);
			if ((bool)tarGetLevelCompoundMaterialData)
			{
				num += tarGetLevelCompoundMaterialData.Count;
			}
			if (num >= 3 && SelectedSpellCfg.canCompound)
			{
				go_BtnMask.SetActive(value: false);
			}
			if (!SelectedSpellCfg.canCompound)
			{
				text_Tip.gameObject.SetActive(value: true);
				text_Tip.text = 1000903.GetText();
			}
			else if (num < 3)
			{
				text_Tip.gameObject.SetActive(value: true);
				text_Tip.text = 1000902.GetText();
			}
		}
		generalMaterialUseCount = 0;
	}

	private void FindGeneralCompoundMaterial()
	{
		generalLV1CompoundMaterialData = null;
		generalLV2CompoundMaterialData = null;
		foreach (UIReroll_Spell item in uiRSs)
		{
			SpellConfig spellConfig = SpellConfig.dic[item.SpellID];
			if (spellConfig != null && spellConfig.abilityType == SpellAbilityType.SpellEmbryo)
			{
				switch (spellConfig.level)
				{
				case 1:
					generalLV1CompoundMaterialData = item;
					break;
				case 2:
					generalLV2CompoundMaterialData = item;
					break;
				}
			}
		}
	}

	private UIReroll_Spell GetTarGetLevelCompoundMaterialData(int level)
	{
		UIReroll_Spell uIReroll_Spell = null;
		switch (level)
		{
		case 1:
			if (generalLV1CompoundMaterialData != null)
			{
				uIReroll_Spell = generalLV1CompoundMaterialData;
			}
			break;
		case 2:
			if (generalLV2CompoundMaterialData != null)
			{
				uIReroll_Spell = generalLV2CompoundMaterialData;
			}
			break;
		}
		if ((bool)uIReroll_Spell && uIReroll_Spell.SpellID == SelectedUIRS.SpellID)
		{
			uIReroll_Spell = null;
		}
		return uIReroll_Spell;
	}

	public void _Compound()
	{
		int num = SelectedUIRS.SpellID % 10;
		int num2 = SelectedUIRS.Count;
		UIReroll_Spell tarGetLevelCompoundMaterialData = GetTarGetLevelCompoundMaterialData(num);
		if ((bool)tarGetLevelCompoundMaterialData)
		{
			num2 += tarGetLevelCompoundMaterialData.Count;
		}
		if (num2 < 3)
		{
			return;
		}
		SEMgr.Inst.so101_Compound.PlaySE();
		int num3 = SelectedUIRS.SpellID + 1;
		int num4 = (tarGetLevelCompoundMaterialData ? Mathf.Min(Mathf.Max(generalMaterialUseCount, 3 - SelectedUIRS.Count), tarGetLevelCompoundMaterialData.Count) : 0);
		if ((bool)tarGetLevelCompoundMaterialData)
		{
			DestroyTargetCountSpell(tarGetLevelCompoundMaterialData.SpellID, num4);
			tarGetLevelCompoundMaterialData.ChangeCount(tarGetLevelCompoundMaterialData.Count - num4);
		}
		DestroyTargetCountSpell(SelectedUIRS.SpellID, 3 - num4, InsertNewSpell: true);
		UpdateAfterCompoundData(SelectedUIRS, 3 - num4);
		if ((bool)tarGetLevelCompoundMaterialData && num4 > 0)
		{
			dic_Splls[tarGetLevelCompoundMaterialData.SpellID] -= num4;
			if (num4 > 0)
			{
				GeneralMeterialDataBoard.SpawnFlyUir(SelectedUIRS);
			}
			if (tarGetLevelCompoundMaterialData.Count <= 0)
			{
				bool flag = false;
				switch (num)
				{
				case 1:
					flag = uiRSs.IndexOf(generalLV1CompoundMaterialData) < uiRSs.IndexOf(SelectedUIRS);
					uiRSs.Remove(generalLV1CompoundMaterialData);
					break;
				case 2:
					flag = uiRSs.IndexOf(generalLV2CompoundMaterialData) < uiRSs.IndexOf(SelectedUIRS);
					uiRSs.Remove(generalLV2CompoundMaterialData);
					break;
				}
				dic_Splls.Remove(tarGetLevelCompoundMaterialData.SpellID);
				UnityEngine.Object.Destroy(tarGetLevelCompoundMaterialData.gameObject);
				if (flag)
				{
					SelectedSpellIndex--;
				}
				for (int i = 0; i < uiRSs.Count; i++)
				{
					uiRSs[i].SetMove(SelectedSpellIndex);
					uiRSs[i].ChangeIndex(i, SelectedSpellIndex);
				}
			}
			UpdateInfo();
		}
		FindGeneralCompoundMaterial();
		go_BtnMask.SetActive(value: true);
		isRecasting = true;
		recastDurationTimer = 0f;
		if (SpellConfig.dic[num3].abilityType == SpellAbilityType.UltimateExtender)
		{
			StartCoroutine(RefreshUIIE());
		}
		DataMgr.selectedWorldData.GalleryUnlock(GalleryCategory.Spell, num3);
		if (SpellConfig.dic[num3].level == 3)
		{
			SteamAchievementMgr.UnlockAndUpload(SteamAchievementType.GetLevel3Spell);
		}
		generalMaterialUseCount = 0;
	}

	private void UpdateAfterCompoundData(UIReroll_Spell targetSpell, int useCount)
	{
		int num = targetSpell.Count - useCount;
		int num2 = targetSpell.SpellID + 1;
		bool flag = dic_Splls.ContainsKey(num2);
		if (num == 0)
		{
			dic_Splls.Remove(targetSpell.SpellID);
			SelectedUIRS.ChangeCount(1);
			SelectedUIRS.ChangeID(num2);
			UpdateInfo();
			if (flag)
			{
				dic_Splls[num2]++;
				int num3 = -1;
				for (int i = 0; i < uiRSs.Count; i++)
				{
					if (uiRSs[i].SpellID == num2 && i != SelectedSpellIndex)
					{
						num3 = i;
						break;
					}
				}
				UIReroll_Spell uIReroll_Spell = uiRSs[num3];
				uIReroll_Spell.Fly(SelectedUIRS);
				uiRSs.Remove(uIReroll_Spell);
				if (num3 < SelectedSpellIndex)
				{
					SelectedSpellIndex--;
					for (int j = 0; j < uiRSs.Count; j++)
					{
						uiRSs[j].ChangeIndex(j, SelectedSpellIndex);
					}
				}
				else
				{
					for (int k = SelectedSpellIndex; k < uiRSs.Count; k++)
					{
						uiRSs[k].ChangeIndex(k, SelectedSpellIndex);
					}
				}
			}
			else
			{
				dic_Splls.Add(num2, 1);
			}
			return;
		}
		dic_Splls[SelectedSpellCfg.id] -= useCount;
		SelectedUIRS.ChangeCount(dic_Splls[SelectedSpellCfg.id]);
		UIReroll_Spell component = UnityEngine.Object.Instantiate(pfb_UIRecastSpell, rtsf_Spells).GetComponent<UIReroll_Spell>();
		if (flag)
		{
			dic_Splls[num2]++;
			int num4 = -1;
			for (int l = 0; l < uiRSs.Count; l++)
			{
				if (uiRSs[l].SpellID == num2)
				{
					num4 = l;
					break;
				}
			}
			UIReroll_Spell uIReroll_Spell2 = uiRSs[num4];
			uIReroll_Spell2.Fly(component);
			uiRSs.Remove(uIReroll_Spell2);
			if (num4 < SelectedSpellIndex)
			{
				component.Initialize(SelectedSpellIndex - 1, num2, 1);
				uiRSs.Insert(SelectedSpellIndex - 1, component);
				for (int m = 0; m <= SelectedSpellIndex; m++)
				{
					uiRSs[m].ChangeIndex(m, SelectedSpellIndex);
				}
			}
			else
			{
				component.Initialize(SelectedSpellIndex + 1, num2, 1);
				uiRSs.Insert(SelectedSpellIndex + 1, component);
				for (int n = SelectedSpellIndex + 1; n < uiRSs.Count; n++)
				{
					uiRSs[n].ChangeIndex(n, SelectedSpellIndex);
				}
			}
		}
		else
		{
			dic_Splls.Add(num2, 1);
			component.Initialize(SelectedSpellIndex + 1, num2, 1);
			uiRSs.Insert(SelectedSpellIndex + 1, component);
			for (int num5 = SelectedSpellIndex + 1; num5 < uiRSs.Count; num5++)
			{
				uiRSs[num5].ChangeIndex(num5, SelectedSpellIndex);
			}
		}
	}

	private void DestroyTargetCountSpell(int targetSpellId, int targetCount, bool InsertNewSpell = false)
	{
		if (targetCount <= 0)
		{
			return;
		}
		bool flag = false;
		int num = 0;
		int id = SelectedUIRS.SpellID + 1;
		for (int i = 0; i < PlayerMgr.Inst.BaData.bagSpellDatas.Count; i++)
		{
			if (PlayerMgr.Inst.BaData.bagSpellDatas[i] != null && PlayerMgr.Inst.BaData.bagSpellDatas[i].id == targetSpellId)
			{
				if (flag || !InsertNewSpell)
				{
					PlayerMgr.Inst.BagSpellChange(i, null);
				}
				else
				{
					flag = true;
					PlayerMgr.Inst.BagSpellChange(i, new SlotData(id));
				}
				num++;
				if (num >= targetCount)
				{
					break;
				}
			}
		}
		if (num >= targetCount)
		{
			return;
		}
		for (int j = 0; j < PlayerMgr.Inst.BaData.wandCfgs.Count; j++)
		{
			if (PlayerMgr.Inst.BaData.wandCfgs[j] == null)
			{
				continue;
			}
			for (int k = 0; k < PlayerMgr.Inst.BaData.wandCfgs[j].normalSlots.Length; k++)
			{
				if (PlayerMgr.Inst.BaData.wandCfgs[j].normalSlots[k] != null && !PlayerMgr.Inst.BaData.wandCfgs[j].normalSlotIsLock[k] && PlayerMgr.Inst.BaData.wandCfgs[j].normalSlots[k].id == targetSpellId)
				{
					if (flag || !InsertNewSpell)
					{
						PlayerMgr.Inst.ChangeWandSpell(j, WandSlotType.Normal, k, null);
					}
					else
					{
						flag = true;
						PlayerMgr.Inst.ChangeWandSpell(j, WandSlotType.Normal, k, new SlotData(id));
					}
					num++;
					if (num >= targetCount)
					{
						break;
					}
				}
			}
			if (num < targetCount)
			{
				for (int l = 0; l < PlayerMgr.Inst.BaData.wandCfgs[j].postSlots.Length; l++)
				{
					if (PlayerMgr.Inst.BaData.wandCfgs[j].postSlots[l] != null && !PlayerMgr.Inst.BaData.wandCfgs[j].postSlotIsLock[l] && PlayerMgr.Inst.BaData.wandCfgs[j].postSlots[l].id == targetSpellId)
					{
						if (flag || !InsertNewSpell)
						{
							PlayerMgr.Inst.ChangeWandSpell(j, WandSlotType.Post, l, null);
						}
						else
						{
							flag = true;
							PlayerMgr.Inst.ChangeWandSpell(j, WandSlotType.Post, l, new SlotData(id));
						}
						num++;
						if (num >= targetCount)
						{
							break;
						}
					}
				}
			}
			if (num >= targetCount)
			{
				break;
			}
		}
	}

	private IEnumerator RefreshUIIE()
	{
		Debug.Log("xxx");
		yield return null;
		OnShow(so101CompoundEtt);
		PlayerMgr.Inst.PlayerCtrller.StartMotion();
	}
}
