using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIDialogueBubble_Middle : MonoBehaviour
{
	public CanvasGroup canvasGroup;

	public Animator anima;

	public Text text_Content;

	public RectTransform rtsf_BG;

	public RectTransform rtsf_BG2;

	public float bgExtraWidht;

	public float showTimeBase;

	public float[] showTimePerLength;

	private float timePerLength;

	[Header("Middle")]
	public Text text_ContentDisplay;

	private Transform tsf_Speaker;

	private Action act_DialogueFinish;

	private bool isFlip;

	private float offset;

	private float duration;

	private float durationTimer;

	private bool hiding;

	private float showCounter;

	[Header("移动端粗糙调整")]
	public float mobileScale = 1.5f;

	public RectTransform scaleByScripts;

	public Vector3 mobileScalePosition;

	private void LateUpdate()
	{
		if (GameMgr.IsMobile_Static)
		{
			if (isFlip)
			{
				base.transform.localPosition = GeneralTool.WorldToCanvasLocalPoint_FitDisplay(tsf_Speaker.position + new Vector3(0f, 0f - offset, 0f));
			}
			else
			{
				base.transform.localPosition = GeneralTool.WorldToCanvasLocalPoint_FitDisplay(tsf_Speaker.position + new Vector3(0f, offset, 0f));
			}
			Vector3 position = base.transform.position;
			UIMgr.UIElementFollowFitSelf(base.gameObject, rtsf_BG, scaleByScripts.localScale, rtsf_BG.pivot);
			rtsf_BG2.transform.position = new Vector3(position.x, rtsf_BG2.transform.position.y, rtsf_BG2.transform.position.z);
		}
		else if (isFlip)
		{
			base.transform.localPosition = GeneralTool.WorldToCanvasLocalPoint(tsf_Speaker.position + new Vector3(0f, 0f - offset, 0f));
		}
		else
		{
			base.transform.localPosition = GeneralTool.WorldToCanvasLocalPoint(tsf_Speaker.position + new Vector3(0f, offset, 0f));
		}
		if (hiding)
		{
			return;
		}
		if (text_ContentDisplay.text != text_Content.text)
		{
			showCounter += 1f / timePerLength * Time.unscaledDeltaTime;
			int num = (int)showCounter;
			if (text_ContentDisplay.text.Length < num)
			{
				if (num <= text_Content.text.Length)
				{
					text_ContentDisplay.text = text_Content.text.Substring(0, num);
				}
				else
				{
					text_ContentDisplay.text = text_Content.text.Substring(0, text_Content.text.Length);
				}
				if (num % 2 == 0)
				{
					SEMgr.Inst.dialogueAppearMiddle.PlaySE();
				}
			}
		}
		durationTimer += Time.unscaledDeltaTime;
		if (durationTimer >= duration)
		{
			durationTimer = 0f;
			anima.SetTrigger("Disappear");
			hiding = true;
		}
	}

	public void Initialize(int textID, Transform tsf_Speaker, float offset, bool isFlip, Action act_DialogueFinish)
	{
		StartCoroutine(InitializeIE(textID, tsf_Speaker, offset, isFlip, act_DialogueFinish));
	}

	private IEnumerator InitializeIE(int textID, Transform tsf_Speaker, float offset, bool isFlip, Action act_DialogueFinish)
	{
		canvasGroup.alpha = 0f;
		this.tsf_Speaker = tsf_Speaker;
		this.offset = offset;
		this.act_DialogueFinish = act_DialogueFinish;
		this.isFlip = isFlip;
		text_Content.text = textID.GetText();
		timePerLength = showTimePerLength[0];
		if (showTimePerLength.Length >= Enum.GetNames(typeof(LanguageType)).Length)
		{
			timePerLength = showTimePerLength[(int)DataMgr.settingData.language];
		}
		duration = showTimeBase + (float)text_Content.text.Length * timePerLength;
		durationTimer = 0f;
		hiding = false;
		anima.SetTrigger("Appear");
		if (isFlip)
		{
			base.transform.localScale = new Vector3(1f, -1f, 1f);
			text_Content.transform.localScale = new Vector3(1f, -1f, 1f);
		}
		else
		{
			base.transform.localScale = Vector3.one;
			text_Content.transform.localScale = Vector3.one;
		}
		text_ContentDisplay.text = "";
		showCounter = 0f;
		if (GameMgr.IsMobile_Static)
		{
			scaleByScripts.localScale = new Vector3(mobileScale, mobileScale, 1f);
			scaleByScripts.localPosition = mobileScalePosition;
		}
		yield return null;
		rtsf_BG.sizeDelta = new Vector2(text_Content.rectTransform.sizeDelta.x + bgExtraWidht, rtsf_BG.sizeDelta.y);
	}

	public void DisappearDirect()
	{
		anima.SetTrigger("DisappearDirect");
		_DisappearFinish();
	}

	private void _DisappearFinish()
	{
		if (act_DialogueFinish != null)
		{
			act_DialogueFinish();
		}
		GameUISingletonMono<UIDialogueMgr>.Inst.MDUnregister(tsf_Speaker);
	}
}
