using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[GameUISingletonPrefab("UICampMirror")]
public class UICampMirror : GameUISingletonMono<UICampMirror>
{
	public List<CampMirrorCharactor> campMirrorCharactors;

	public Button btn_Close;

	public Image image_Portrait;

	public Animator anima;

	public Text _textReward;

	public GameObject gameobject_ShowKeyLeft;

	public GameObject gameobject_ShowKeyRight;

	public UpdatButtonShow[] updatebuttonshows;

	public int textflowSpeed;

	public float activetimerate;

	public float heightRandmOffset;

	public List<Transform> TextFlowTransform = new List<Transform>();

	public List<GameObject> currentTextsobjects = new List<GameObject>();

	private List<int> currentTextBulltes = new List<int>();

	private int selectedPlayerLookIndex;

	private int activeTextCount;

	private float activeTimeRateTimer;

	private int currentTsfIndex;

	protected override void RegistarWhenInit()
	{
		EventMgr.LanguageChange = (Action)Delegate.Combine(EventMgr.LanguageChange, new Action(LanguageChange_UIsetting));
		EventMgr.InputChange = (Action)Delegate.Combine(EventMgr.InputChange, new Action(InputChange));
	}

	protected override void RegistarOnlyWhenOpen()
	{
		base.inputActions.Player.GamepadDirect.performed += GamepadDirectPerformed;
		base.inputActions.Player.LeftStick.performed += GamepadDirectPerformed_Stick;
		base.inputActions.Player.WASD.performed += GamepadDirectPerformed;
	}

	protected override void UnRegistarOnlyWhenHide()
	{
		base.inputActions.Player.GamepadDirect.performed -= GamepadDirectPerformed;
		base.inputActions.Player.LeftStick.performed -= GamepadDirectPerformed_Stick;
		base.inputActions.Player.WASD.performed -= GamepadDirectPerformed;
	}

	protected override void UnRegistarWhenDestroy()
	{
		EventMgr.LanguageChange = (Action)Delegate.Remove(EventMgr.LanguageChange, new Action(LanguageChange_UIsetting));
		EventMgr.InputChange = (Action)Delegate.Remove(EventMgr.InputChange, new Action(InputChange));
	}

	private void OnEnable()
	{
		InputChange();
		if (!ICJNOGPFMAM.FIKDMCBJPCO)
		{
			SafeRemoveSkin(PlayerLook.Halloween);
		}
		if (!ICJNOGPFMAM.MADIIMLEMNP)
		{
			SafeRemoveSkin(PlayerLook.SnowMan);
		}
		if (!ICJNOGPFMAM.ACPKKMJKOJD)
		{
			SafeRemoveSkin(PlayerLook.Horse);
		}
		if (!ICJNOGPFMAM.BHEHHIFGJOE)
		{
			SafeRemoveSkin(PlayerLook.SummerBoy);
			SafeRemoveSkin(PlayerLook.SummerGirl);
		}
		if (GameMgr.IsMobile_Static && GameMgr.IsUseBiliOneSDK)
		{
			SafeRemoveSkin(PlayerLook.HaoYou);
			SafeRemoveSkin(PlayerLook.TapTap);
		}
		else
		{
			SafeRemoveSkin(PlayerLook.HaoYou);
			SafeRemoveSkin(PlayerLook.TapTap);
		}
		selectedPlayerLookIndex = Mathf.Clamp(selectedPlayerLookIndex, 0, campMirrorCharactors.Count - 1);
	}

	private void SafeRemoveSkin(PlayerLook playerLook)
	{
		try
		{
			if (DataMgr.selectedWorldData.playerLook == playerLook)
			{
				DataMgr.selectedWorldData.playerLook = PlayerLook.Default;
			}
			campMirrorCharactors = campMirrorCharactors.Where((CampMirrorCharactor x) => x.playerlook != playerLook).ToList();
		}
		catch (InvalidOperationException)
		{
			Debug.LogWarning($"未找到要移除的皮肤 {playerLook}");
		}
	}

	protected override IEnumerator OnInit()
	{
		currentTsfIndex = 0;
		activeTimeRateTimer = 0f;
		btn_Close.onClick.AddListener(_Close);
		try
		{
			selectedPlayerLookIndex = campMirrorCharactors.Select((CampMirrorCharactor character, int index) => new { character, index }).First(x => x.character.playerlook == DataMgr.selectedWorldData.playerLook).index;
		}
		catch (Exception ex)
		{
			Debug.Log("找到错误的皮肤id " + ex);
			selectedPlayerLookIndex = 0;
		}
		if (selectedPlayerLookIndex > campMirrorCharactors.Count)
		{
			selectedPlayerLookIndex = 0;
			DataMgr.selectedWorldData.playerLook = PlayerLook.Default;
		}
		ResetTextBullets();
		image_Portrait.sprite = Resources.Load<Sprite>("Textures/Portraits/" + campMirrorCharactors[selectedPlayerLookIndex].portraitID);
		image_Portrait.gameObject.SetActive(value: true);
		LanguageChange_UIsetting();
		yield return null;
	}

	private void ResetTextBullets()
	{
		currentTextBulltes.Clear();
		for (int i = campMirrorCharactors[selectedPlayerLookIndex].bulletIDFrom; i <= campMirrorCharactors[selectedPlayerLookIndex].bulletIDTo; i++)
		{
			currentTextBulltes.Add(i);
		}
	}

	private void InputChange()
	{
		ControlChange();
		if (!GameMgr.IsMobile_Static)
		{
			switch (UIMgr.Inst.InputType)
			{
			case PlayerInputType.Keyboard:
				gameobject_ShowKeyLeft.SetActive(value: true);
				gameobject_ShowKeyRight.SetActive(value: true);
				break;
			case PlayerInputType.Gamepad:
				gameobject_ShowKeyLeft.SetActive(value: false);
				gameobject_ShowKeyRight.SetActive(value: false);
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

	private void LanguageChange_UIsetting()
	{
		if (selectedPlayerLookIndex < campMirrorCharactors.Count)
		{
			_textReward.text = campMirrorCharactors[selectedPlayerLookIndex].textInfo.GetText();
		}
		else
		{
			_textReward.text = "";
		}
	}

	private void GamepadDirectPerformed_Stick(InputAction.CallbackContext context)
	{
		if (GameUISingletonMono<UICampMirror>.StaticIsOpen)
		{
			Vector2 vector = context.ReadValue<Vector2>();
			vector = ControlMgr.Inst.RampVector2(vector);
			MoveDirectionNav(vector);
		}
	}

	private void GamepadDirectPerformed(InputAction.CallbackContext context)
	{
		if (GameUISingletonMono<UICampMirror>.StaticIsOpen)
		{
			MoveDirectionNav(context.ReadValue<Vector2>());
		}
	}

	private void MoveDirectionNav(Vector2 direct)
	{
		if (!(anima.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1f))
		{
			if (direct == Vector2.left)
			{
				LeftButton();
			}
			else if (direct == Vector2.right)
			{
				RightButton();
			}
		}
	}

	public void LeftButton()
	{
		if (!(anima.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1f))
		{
			SEMgr.Inst.uiSwitch.PlaySE();
			if (selectedPlayerLookIndex == 0)
			{
				selectedPlayerLookIndex = campMirrorCharactors.Count - 1;
			}
			else
			{
				selectedPlayerLookIndex--;
			}
			ChangeSkinMotion();
		}
	}

	public void RightButton()
	{
		if (!(anima.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1f))
		{
			SEMgr.Inst.uiSwitch.PlaySE();
			if (selectedPlayerLookIndex == campMirrorCharactors.Count - 1)
			{
				selectedPlayerLookIndex = 0;
			}
			else
			{
				selectedPlayerLookIndex++;
			}
			ChangeSkinMotion();
		}
	}

	public void ChangeSkinMotion()
	{
		anima.Play("ChangeSkin", 0, 0f);
		ResetTextBullets();
		PlayerMgr.Inst.PlayerCtrller.SetToNormalAnime();
	}

	protected override void OnShow(object obj = null)
	{
		PlayerMgr.Inst.PlayerCtrller.SetToNormalAnime();
		UIPlayerDataMgr.Inst.UISlotBagExitall();
		anima.Play("Show");
		UIMgr.TryAdditionalMobileShow(base.transform);
		InputChange();
		PlayerMgr.Inst.PlayerCtrller.StopMotion();
		SEMgr.Inst.uiOpen.PlaySE();
	}

	protected override void OnHide()
	{
		StopAllCoroutines();
		anima.Play("Hide");
		UIMgr.TryAdditionalMobileHide(base.transform);
		PlayerMgr.Inst.PlayerCtrller.StartMotion();
		DataMgr.SaveSelectedWorldData();
		SEMgr.Inst.uiClose.PlaySE();
	}

	public override void _Close()
	{
		if (!GameMgr.IsMobile_Static)
		{
			SEMgr.Inst.uiClick.PlaySE();
		}
		Hide();
	}

	public override void Hide()
	{
		base.Hide();
	}

	private void Update()
	{
		if (GameMgr.IsHarmony_Static || !GameUISingletonMono<UICampMirror>.StaticIsOpen || campMirrorCharactors[selectedPlayerLookIndex].bulletIDFrom == 0)
		{
			return;
		}
		activeTimeRateTimer += Time.deltaTime;
		if (activeTimeRateTimer >= activetimerate)
		{
			activeTimeRateTimer = 0f;
			if (!currentTextsobjects[0].gameObject.activeSelf)
			{
				SetTextFlow();
				activeTextCount++;
			}
			else
			{
				SetTextFlow();
			}
		}
		if (activeTextCount <= 0)
		{
			return;
		}
		for (int num = currentTextsobjects.Count - 1; num >= currentTextsobjects.Count - activeTextCount; num--)
		{
			if (currentTextsobjects[num].gameObject.activeSelf)
			{
				currentTextsobjects[num].transform.Translate(Vector3.left * textflowSpeed * Time.deltaTime);
			}
		}
	}

	private void SetTextFlow()
	{
		currentTextsobjects[0].gameObject.transform.position = TextFlowTransform[currentTsfIndex].transform.position + new Vector3(UnityEngine.Random.Range(0f, heightRandmOffset), UnityEngine.Random.Range(0f, heightRandmOffset), 0f);
		if (campMirrorCharactors[selectedPlayerLookIndex].bulletIDFrom != 0)
		{
			int index = UnityEngine.Random.Range(0, currentTextBulltes.Count);
			currentTextsobjects[0].gameObject.GetComponent<Text>().text = currentTextBulltes[index].GetText();
			currentTextBulltes.RemoveAt(index);
			if (currentTextBulltes.Count == 0)
			{
				ResetTextBullets();
			}
		}
		if (currentTsfIndex < TextFlowTransform.Count - 1)
		{
			currentTsfIndex++;
		}
		else
		{
			currentTsfIndex = 0;
		}
		currentTextsobjects[0].gameObject.SetActive(value: true);
		currentTextsobjects.Add(currentTextsobjects[0]);
		currentTextsobjects.RemoveAt(0);
	}

	public void UpdateSkinMirrorShow()
	{
		image_Portrait.gameObject.SetActive(value: true);
		foreach (GameObject currentTextsobject in currentTextsobjects)
		{
			currentTextsobject.SetActive(value: false);
		}
		activeTextCount = 0;
		DataMgr.selectedWorldData.playerLook = campMirrorCharactors[selectedPlayerLookIndex].playerlook;
		activeTimeRateTimer = activetimerate;
		if (GameUISingletonMono<UISet>.Inited)
		{
			GameUISingletonMono<UISet>.Inst.resetSet = true;
		}
		PlayerMgr.Inst.UpdateSkin();
		for (int i = 0; i < PlayerMgr.Inst.Wands.Count; i++)
		{
			PlayerMgr.Inst.Wands[i].UpdateHandDisplay();
		}
		image_Portrait.sprite = Resources.Load<Sprite>("Textures/Portraits/" + campMirrorCharactors[selectedPlayerLookIndex].portraitID);
		LanguageChange_UIsetting();
	}
}
