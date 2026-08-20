using System.Collections.Generic;
using DG.Tweening;
using Steamworks;
using UnityEngine;
using UnityEngine.UI;

public class UICompanyLogoShow : MonoBehaviour
{
	public Transform transLogo;

	public Sprite CompanyLogoCN;

	public Sprite CompanyLogoEN;

	public Sprite BilibiliLogoCN;

	public Sprite BilibiliLogoEN;

	public Image CompanyLogoImage;

	public Image BilibiliLogoImage;

	public GameObject healthyGaming;

	public CanvasGroup flashScreen;

	public List<GameObject> loadingObjs = new List<GameObject>();

	public bool finishSplashScreen;

	[Header("手游进度条")]
	public GameObject sliderObj;

	public Slider mobileLoadSlider;

	public float sliderSpeed = 0.01f;

	public float showTime => 2f;

	public void Start()
	{
		loadingObjs.ForEach(delegate(GameObject x)
		{
			x.SetActive(value: false);
		});
		ShowChineseIcon();
		if (SteamManager.Initialized && GameMgr.steamLanguageToGameLanguage.TryGetValue(SteamUtils.GetSteamUILanguage(), out var value) && value != 0 && value != LanguageType.ChineseT)
		{
			ShowEnglishIcon();
		}
		transLogo.localScale = Vector3.one * (GameMgr.IsMobile_Static ? 1f : 0.6f);
		CompanyLogoImage.SetNativeSize();
		BilibiliLogoImage.SetNativeSize();
		flashScreen.alpha = 0f;
		BilibiliLogoImage.gameObject.SetActive(value: true);
		CompanyLogoImage.gameObject.SetActive(value: false);
		Sequence sequence = DOTween.Sequence().Append(flashScreen.DOFade(1f, 0.5f)).AppendInterval(showTime)
			.Append(flashScreen.DOFade(0f, 0.5f))
			.SetUpdate(isIndependentUpdate: true)
			.AppendCallback(delegate
			{
				BilibiliLogoImage.gameObject.SetActive(value: false);
				CompanyLogoImage.gameObject.SetActive(value: true);
			})
			.Append(flashScreen.DOFade(1f, 0.5f))
			.AppendInterval(showTime)
			.Append(flashScreen.DOFade(0f, 0.5f))
			.SetUpdate(isIndependentUpdate: true)
			.AppendCallback(delegate
			{
				CompanyLogoImage.gameObject.SetActive(value: false);
				if (GameMgr.IsMobile_Static || ScriptableObjMgr.Inst.testCtrller.publishTesting)
				{
					healthyGaming.gameObject.SetActive(value: true);
				}
			});
		if (GameMgr.IsMobile_Static || ScriptableObjMgr.Inst.testCtrller.publishTesting)
		{
			sequence.Append(flashScreen.DOFade(1f, 1f)).AppendInterval(showTime).SetUpdate(isIndependentUpdate: true)
				.AppendCallback(delegate
				{
				});
		}
		sequence.OnComplete(delegate
		{
			Debug.Log("Complete Splash");
			finishSplashScreen = true;
		});
		sliderObj.SetActive(GameMgr.IsMobile_Static);
	}

	private void Update()
	{
		if (GameMgr.IsMobile_Static)
		{
			mobileLoadSlider.value += sliderSpeed * Time.deltaTime;
		}
	}

	private void ShowEnglishIcon()
	{
		CompanyLogoImage.sprite = CompanyLogoEN;
		BilibiliLogoImage.sprite = BilibiliLogoEN;
	}

	private void ShowChineseIcon()
	{
		CompanyLogoImage.sprite = CompanyLogoCN;
		BilibiliLogoImage.sprite = BilibiliLogoCN;
	}

	private void OnDestroy()
	{
		DOTween.KillAll();
	}

	public void OpenURLBeiAn()
	{
		Application.OpenURL("https://beian.miit.gov.cn");
	}
}
