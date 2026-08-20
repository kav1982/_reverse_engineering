using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[GameUISingletonPrefab("UIUnlockSystem")]
public class UIUnlockSystem : GameUISingletonMono<UIUnlockSystem>
{
	public enum UIUnlockSystemType
	{
		Talent,
		Research,
		Set,
		TrainingRoom,
		ActivateGirl,
		SpellDisable,
		UnlockDLC1
	}

	public GameObject UIRoot;

	public Image imageShow1;

	public Image imageShow2;

	public CanvasGroup canvasGroup;

	public Text text;

	public Text textInfo;

	public GameObject spinBG;

	public List<GameObject> stars;

	public Image BgGlowImage;

	public float rotationSpeed = 50f;

	public Vector3 rotationAxis = Vector3.forward;

	public Sprite spriteTalent;

	public Sprite spriteResearch;

	public Sprite spriteSet;

	public Sprite spriteTrainingRoom;

	public Sprite spriteSpellDisable;

	public Sprite spriteActivateGirl;

	public Sprite spriteDLC1;

	private void Update()
	{
		spinBG.transform.Rotate(rotationAxis, rotationSpeed * Time.deltaTime);
	}

	protected override void OnShow(object obj = null)
	{
		SEMgr.Inst.UIUnlockSystem.PlaySE();
		if (obj is UIUnlockSystemType)
		{
			switch ((UIUnlockSystemType)obj)
			{
			case UIUnlockSystemType.Talent:
				text.text = 1000501.GetText();
				imageShow1.sprite = spriteTalent;
				imageShow2.sprite = spriteTalent;
				break;
			case UIUnlockSystemType.Research:
				text.text = 1002101.GetText();
				imageShow1.sprite = spriteResearch;
				imageShow2.sprite = spriteResearch;
				textInfo.text = 1006601.GetText();
				break;
			case UIUnlockSystemType.Set:
				text.text = 1002102.GetText();
				imageShow1.sprite = spriteSet;
				imageShow2.sprite = spriteSet;
				textInfo.text = 1006602.GetText();
				break;
			case UIUnlockSystemType.TrainingRoom:
				text.text = 1002406.GetText();
				imageShow1.sprite = spriteTrainingRoom;
				imageShow2.sprite = spriteTrainingRoom;
				textInfo.text = 1006603.GetText();
				break;
			case UIUnlockSystemType.SpellDisable:
				text.text = 1003501.GetText();
				imageShow1.sprite = spriteSpellDisable;
				imageShow2.sprite = spriteSpellDisable;
				textInfo.text = 1006605.GetText();
				break;
			case UIUnlockSystemType.ActivateGirl:
				text.text = 1003301.GetText();
				imageShow1.sprite = spriteActivateGirl;
				imageShow2.sprite = spriteActivateGirl;
				textInfo.text = 1006604.GetText();
				break;
			case UIUnlockSystemType.UnlockDLC1:
				text.text = "解锁万圣节主题DLC";
				imageShow1.sprite = spriteDLC1;
				imageShow2.sprite = spriteDLC1;
				textInfo.text = "可在营地镜子切换主题";
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}
		base.OnShow(obj);
		canvasGroup.alpha = 0f;
		PlayerMgr.Inst.PlayerCtrller.StopMotion();
		UIRoot.SetActive(value: true);
		canvasGroup.DOFade(1f, 0.5f).SetUpdate(isIndependentUpdate: true);
		BgGlowImage.DOFade(0.5f, 0.8f).SetLoops(-1, LoopType.Yoyo).SetUpdate(isIndependentUpdate: true);
		stars.ForEach(delegate(GameObject x)
		{
			float delay = UnityEngine.Random.Range(0, 1);
			x.transform.DOScale(Vector3.one * 1.3f, 1f).SetLoops(-1, LoopType.Yoyo).SetUpdate(isIndependentUpdate: true)
				.SetDelay(delay);
		});
	}

	protected override void OnHide()
	{
		PlayerMgr.Inst.PlayerCtrller.StartMotion();
		canvasGroup.DOFade(0f, 0.5f).OnComplete(delegate
		{
			UIRoot.SetActive(value: false);
		}).SetUpdate(isIndependentUpdate: true);
		DOTween.KillAll(this);
		UIPlaceNameMgr.Inst.Show(PlaceNameType.Camp);
	}

	protected override void RegistarWhenInit()
	{
	}

	protected override void RegistarOnlyWhenOpen()
	{
		base.inputActions.Player.Interact.performed += InteractPerformed;
	}

	protected override void UnRegistarOnlyWhenHide()
	{
		base.inputActions.Player.Interact.performed += InteractPerformed;
	}

	private void InteractPerformed(InputAction.CallbackContext obj)
	{
		GameUISingletonMono<UIUnlockSystem>.Inst.Hide();
	}

	protected override void UnRegistarWhenDestroy()
	{
	}
}
