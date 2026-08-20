using System;
using System.Runtime.CompilerServices;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[GameUISingletonPrefab("UIWhiteScreen")]
public class UIWhiteScreen : GameUISingletonMono<UIWhiteScreen>
{
	private enum WhiteScreenType
	{
		WhiteScreen,
		Video
	}

	public RawImage rawImage;

	public GameObject videoGo;

	public Transform videoArea;

	public CanvasGroup canvasGroup;

	private float hideDuration;

	public Action actionOnClose;

	public bool canClose;

	private WhiteScreenType whiteScreenType;

	private string musicAfterVideo = "";

	protected override void OnShow(object obj = null)
	{
		base.OnShow(obj);
		Debug.Log(obj);
		if (obj is ITuple tuple)
		{
			switch (tuple.Length)
			{
			case 3:
			{
				object obj2 = tuple[0];
				if (!(obj2 is float))
				{
					break;
				}
				float num = (float)obj2;
				object obj3 = tuple[1];
				if (obj3 is float)
				{
					float whiteScreenTime = (float)obj3;
					object obj4 = tuple[2];
					if (obj4 is float)
					{
						float num2 = (float)obj4;
						HandleShowWhiteScreen(num, whiteScreenTime, num2);
						videoGo.SetActive(value: false);
						return;
					}
				}
				break;
			}
			case 4:
			{
				object obj2 = tuple[0];
				if (!(obj2 is float))
				{
					break;
				}
				float num = (float)obj2;
				object obj3 = tuple[1];
				if (!(obj3 is string videoPrefabPath2))
				{
					break;
				}
				object obj4 = tuple[2];
				if (obj4 is float)
				{
					float num2 = (float)obj4;
					object obj5 = tuple[3];
					if (obj5 is bool)
					{
						bool flag = (bool)obj5;
						float showDuration2 = num;
						float hideDuration2 = num2;
						HandleShowVideo(showDuration2, videoPrefabPath2, hideDuration2, flag);
						videoGo.SetActive(value: true);
						return;
					}
				}
				break;
			}
			case 5:
			{
				object obj2 = tuple[0];
				if (!(obj2 is float))
				{
					break;
				}
				float num = (float)obj2;
				object obj3 = tuple[1];
				if (!(obj3 is string text))
				{
					break;
				}
				object obj4 = tuple[2];
				if (!(obj4 is float))
				{
					break;
				}
				float num2 = (float)obj4;
				object obj5 = tuple[3];
				if (obj5 is bool)
				{
					bool flag = (bool)obj5;
					if (tuple[4] is string currentMusic)
					{
						float showDuration = num;
						string videoPrefabPath = text;
						float hideDuration = num2;
						bool canEscClose = flag;
						HandleShowVideo(showDuration, videoPrefabPath, hideDuration, canEscClose, currentMusic);
						videoGo.SetActive(value: true);
						return;
					}
				}
				break;
			}
			}
		}
		Debug.LogError("参数错误");
	}

	private void HandleShowWhiteScreen(float showDuration, float whiteScreenTime, float hideDuration)
	{
		whiteScreenType = WhiteScreenType.WhiteScreen;
		this.hideDuration = hideDuration;
		if ((bool)PlayerMgr.Inst.PlayerCtrller)
		{
			PlayerMgr.Inst.PlayerCtrller.StopMotion();
		}
		DOTween.Sequence().Append(canvasGroup.DOFade(1f, showDuration)).AppendInterval(whiteScreenTime)
			.SetUpdate(isIndependentUpdate: true)
			.OnComplete(Hide);
	}

	private void HandleShowVideo(float showDuration2, string videoPrefabPath, float hideDuration2, bool canEscClose, string currentMusic = "")
	{
		whiteScreenType = WhiteScreenType.Video;
		musicAfterVideo = currentMusic;
		canClose = canEscClose;
		MusicMgr.Inst.ForcePlayMusic("");
		if ((bool)PlayerMgr.Inst.PlayerCtrller)
		{
			PlayerMgr.Inst.PlayerCtrller.StopMotion();
		}
		GameObject prefab = ABResources.LoadAsset<GameObject>(videoPrefabPath);
		hideDuration = hideDuration2;
		DOTween.Sequence().Append(canvasGroup.DOFade(1f, showDuration2)).AppendCallback(delegate
		{
			UnityEngine.Object.Instantiate(prefab, videoArea).GetComponent<UIImageVideoPlayer>();
		})
			.AppendInterval(prefab.GetComponent<UIImageVideoPlayer>().TotalTime)
			.SetUpdate(isIndependentUpdate: true)
			.OnComplete(Hide);
	}

	protected override void OnHide()
	{
		canvasGroup.DOFade(0f, hideDuration).SetUpdate(isIndependentUpdate: true).OnComplete(delegate
		{
			if ((bool)PlayerMgr.Inst.PlayerCtrller)
			{
				PlayerMgr.Inst.PlayerCtrller.StartMotion();
			}
			SetIsOpen(isOpen: false);
			WhiteScreenType whiteScreenType = this.whiteScreenType;
			if (whiteScreenType != 0 && whiteScreenType == WhiteScreenType.Video)
			{
				if (musicAfterVideo != "")
				{
					MusicMgr.Inst.ForcePlayMusic(musicAfterVideo);
					musicAfterVideo = "";
				}
				else
				{
					MusicMgr.Inst.UpdateThemeMusic();
				}
			}
			rawImage.texture = null;
			actionOnClose?.Invoke();
			actionOnClose = null;
			GameUISingletonMono<UIWhiteScreen>.DestroyUI();
		});
	}

	public override void Hide()
	{
		base.Hide();
		SetIsOpen(isOpen: true);
		videoArea.DestroyAllChild();
	}

	protected override void RegistarWhenInit()
	{
	}

	protected override void RegistarOnlyWhenOpen()
	{
	}

	protected override void UnRegistarOnlyWhenHide()
	{
	}

	protected override void UnRegistarWhenDestroy()
	{
	}

	public override void OnDestroy()
	{
		base.OnDestroy();
		videoArea.DestroyAllChild();
	}
}
