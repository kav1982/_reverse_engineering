using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIDialogueBubble_Soft : MonoBehaviour
{
	public Animator anima;

	public Text text_Content;

	public RectTransform rtsf_BG;

	public float bgExtraWidht;

	public float showTimeBase;

	public float showTimePerLength;

	private Transform tsf_Speaker;

	private Action act_DialogueFinish;

	private bool isFlip;

	private float offset;

	private float duration;

	private float durationTimer;

	private bool disappearing;

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
		}
		else if (isFlip)
		{
			base.transform.localPosition = GeneralTool.WorldToCanvasLocalPoint(tsf_Speaker.position + new Vector3(0f, 0f - offset, 0f));
		}
		else
		{
			base.transform.localPosition = GeneralTool.WorldToCanvasLocalPoint(tsf_Speaker.position + new Vector3(0f, offset, 0f));
		}
		if (!disappearing)
		{
			durationTimer += Time.unscaledDeltaTime;
			if (durationTimer >= duration)
			{
				durationTimer = 0f;
				anima.SetTrigger("Disappear");
				disappearing = true;
			}
		}
	}

	public void Initialize(int textID, Transform tsf_Speaker, float offset, bool isFlip, Action act_DialogueFinish)
	{
		StartCoroutine(InitializeIE(textID, tsf_Speaker, offset, isFlip, act_DialogueFinish));
	}

	private IEnumerator InitializeIE(int textID, Transform tsf_Speaker, float offset, bool isFlip, Action act_DialogueFinish)
	{
		this.tsf_Speaker = tsf_Speaker;
		this.offset = offset;
		this.act_DialogueFinish = act_DialogueFinish;
		this.isFlip = isFlip;
		text_Content.text = textID.GetText();
		duration = showTimeBase + (float)text_Content.text.Length * showTimePerLength;
		durationTimer = 0f;
		disappearing = false;
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
		if (GameMgr.IsMobile_Static)
		{
			scaleByScripts.localScale = new Vector3(mobileScale, mobileScale, 1f);
			scaleByScripts.localPosition = mobileScalePosition;
		}
		yield return null;
		rtsf_BG.sizeDelta = new Vector2(text_Content.rectTransform.sizeDelta.x + bgExtraWidht, rtsf_BG.sizeDelta.y);
	}

	private void _DisappearFinish()
	{
		if (act_DialogueFinish != null)
		{
			act_DialogueFinish();
		}
		GameUISingletonMono<UIDialogueMgr>.Inst.SDUnregister(tsf_Speaker);
	}
}
