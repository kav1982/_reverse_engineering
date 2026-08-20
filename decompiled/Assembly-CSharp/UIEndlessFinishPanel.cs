using System;
using UnityEngine;
using UnityEngine.UI;

public class UIEndlessFinishPanel : MonoBehaviour
{
	[SerializeField]
	private Button openUIFinishBuildShowButton;

	[SerializeField]
	private Button closeButton;

	[SerializeField]
	private Animator anim;

	[SerializeField]
	private GlitchTypewriterOldText gameOverText;

	[SerializeField]
	private GlitchTypewriterOldText levelText;

	[SerializeField]
	private GlitchTypewriterOldText maxlvText;

	[SerializeField]
	private GlitchTypewriterOldText GetGearText;

	[SerializeField]
	private GlitchTypewriterOldText rankText;

	[SerializeField]
	private GlitchTypewriterOldText maxRankText;

	[SerializeField]
	private GlitchTypewriterOldText openUIFinishBuildText;

	[SerializeField]
	private GlitchTypewriterOldText gobackToCampText;

	[SerializeField]
	private Color NumberColor;

	private bool IsInAnimation;

	private Action OnFinishHideAction;

	private Action ShowBuild;

	private FinishEndlessGameBuild currentBuild;

	private bool isEndlessFinish;

	private float typeSpeed = 1f;

	public bool IsOpen { get; private set; }

	private string numColor => ColorUtility.ToHtmlStringRGBA(NumberColor);

	public void Show(Action showBuild, FinishEndlessGameBuild build, Action _OnFinishHideAction = null, bool IsGameFinish = true, float _typeSpeed = 1f)
	{
		if (!IsInAnimation && !IsOpen)
		{
			typeSpeed = _typeSpeed;
			isEndlessFinish = IsGameFinish;
			if (IsGameFinish)
			{
				base.transform.SetAsLastSibling();
			}
			OnFinishHideAction = _OnFinishHideAction;
			base.gameObject.SetActive(value: true);
			IsOpen = true;
			IsInAnimation = true;
			ShowBuild = showBuild;
			anim.Play("Show");
			currentBuild = build;
			closeButton.onClick.RemoveListener(Close);
			openUIFinishBuildShowButton.onClick.RemoveListener(OpenUIFinishBuildShow);
		}
	}

	public void OnOpenAnimFinished()
	{
		gameOverText.originalText = (isEndlessFinish ? 1007309.GetText() : 1007310.GetText());
		gameOverText.transform.gameObject.SetActive(value: true);
		gameOverText.StartType(ShowCurrentLv, typeSpeed);
	}

	private void ShowCurrentLv()
	{
		levelText.transform.parent.gameObject.SetActive(value: true);
		levelText.originalText = string.Format(1007301.GetText(), numColor, currentBuild.EndlessLevel);
		levelText.StartType(ShowMaxLv, typeSpeed);
	}

	private void ShowMaxLv()
	{
		maxlvText.transform.parent.gameObject.SetActive(value: true);
		int num = DataMgr.currentSelectWorldIndex switch
		{
			1 => DataMgr.WorldData1.BestEndlessLevel, 
			0 => DataMgr.WorldData0.BestEndlessLevel, 
			_ => DataMgr.WorldData2.BestEndlessLevel, 
		};
		maxlvText.originalText = string.Format(1007302.GetText(), numColor, num);
		maxlvText.StartType(ShowGetGearCount, typeSpeed);
	}

	private void ShowGetGearCount()
	{
		GetGearText.transform.parent.gameObject.SetActive(value: true);
		GetGearText.originalText = string.Format(1007303.GetText(), numColor, currentBuild.GetGearCount);
		GetGearText.StartType(ShowMaxRank, typeSpeed);
	}

	private void ShowRankCurrent()
	{
		rankText.transform.parent.gameObject.SetActive(value: true);
		rankText.originalText = string.Format(1007304.GetText(), numColor, currentBuild.GetGearCount);
		rankText.StartType(ShowMaxRank, typeSpeed);
	}

	private void ShowMaxRank()
	{
		maxRankText.transform.parent.gameObject.SetActive(value: true);
		maxRankText.originalText = string.Format(1007305.GetText(), numColor, (DataMgr.finishEndlessGameBuilds.MyBestRank == -1) ? "???" : ((object)DataMgr.finishEndlessGameBuilds.MyBestRank));
		maxRankText.StartType(ShowButton1, typeSpeed);
	}

	private void ShowButton1()
	{
		openUIFinishBuildText.transform.parent.gameObject.SetActive(value: true);
		openUIFinishBuildText.originalText = 1007306.GetText();
		openUIFinishBuildText.StartType(ShowButton2, typeSpeed);
	}

	private void ShowButton2()
	{
		gobackToCampText.transform.parent.gameObject.SetActive(value: true);
		gobackToCampText.originalText = (isEndlessFinish ? 1007307.GetText() : 1007308.GetText());
		gobackToCampText.StartType(FinishAnim, typeSpeed);
	}

	private void FinishAnim()
	{
		IsInAnimation = false;
		openUIFinishBuildShowButton.onClick.AddListener(OpenUIFinishBuildShow);
		closeButton.onClick.AddListener(Close);
	}

	public void OnCloseAnimationFinished()
	{
		IsOpen = false;
		IsInAnimation = false;
		base.gameObject.SetActive(value: false);
		OnFinishHideAction?.Invoke();
	}

	private void OpenUIFinishBuildShow()
	{
		ShowBuild?.Invoke();
	}

	public void Close()
	{
		if (!IsInAnimation && IsOpen)
		{
			gameOverText.gameObject.SetActive(value: false);
			openUIFinishBuildShowButton.gameObject.SetActive(value: false);
			closeButton.gameObject.SetActive(value: false);
			levelText.transform.parent.gameObject.SetActive(value: false);
			maxlvText.transform.parent.gameObject.SetActive(value: false);
			GetGearText.transform.parent.gameObject.SetActive(value: false);
			rankText.transform.parent.gameObject.SetActive(value: false);
			maxRankText.transform.parent.gameObject.SetActive(value: false);
			closeButton.onClick.RemoveListener(Close);
			openUIFinishBuildShowButton.onClick.RemoveListener(OpenUIFinishBuildShow);
			IsInAnimation = true;
			anim.Play("Hide");
		}
	}
}
