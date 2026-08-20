using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Video;

[GameUISingletonPrefab("UIReleaseNote")]
public class UIReleaseNote : GameUISingletonMono<UIReleaseNote>
{
	[Serializable]
	public class ReleaseImage
	{
		public Sprite sprite;

		public Vector2 Res;
	}

	public RealeaseNoteImageSO so;

	public Text UpdateTitle;

	public RectTransform rectTransformScrollView;

	public float GamepadScrollAmount;

	public float GamepadScrollSpeed;

	public Animator animator;

	public GameObject Content;

	public GameObject pfbText;

	public GameObject pfbLine;

	public GameObject pfbImage;

	public GameObject pfbRawImage;

	public List<VideoPlayer> videoPlayers;

	public int size1;

	public int size2;

	public int size3;

	private Vector2 scrollingDir;

	protected override IEnumerator OnInit()
	{
		LanguageChange();
		yield return null;
	}

	protected override void RegistarWhenInit()
	{
		EventMgr.LanguageChange = (Action)Delegate.Combine(EventMgr.LanguageChange, new Action(LanguageChange));
	}

	protected override void RegistarOnlyWhenOpen()
	{
		base.inputActions.Player.GamepadDirect.performed += GamepadDirectPerformed;
		base.inputActions.Player.LeftStick.performed += GamepadDirectPerformed_Stick;
		base.inputActions.Player.GamepadDirect.canceled += StopScrolling;
		base.inputActions.Player.LeftStick.canceled += StopScrolling;
	}

	protected override void UnRegistarOnlyWhenHide()
	{
		base.inputActions.Player.GamepadDirect.performed -= GamepadDirectPerformed;
		base.inputActions.Player.LeftStick.performed -= GamepadDirectPerformed_Stick;
		base.inputActions.Player.GamepadDirect.canceled -= StopScrolling;
		base.inputActions.Player.LeftStick.canceled -= StopScrolling;
	}

	protected override void UnRegistarWhenDestroy()
	{
		EventMgr.LanguageChange = (Action)Delegate.Remove(EventMgr.LanguageChange, new Action(LanguageChange));
	}

	private void LanguageChange()
	{
		GeneratContent();
		UpdateTitle.text = 1006002.GetText();
	}

	protected override void OnHide()
	{
		SEMgr.Inst.uiClick.PlaySE();
		animator.Play("Hide");
		if ((bool)UIMainMenuMgr.Inst)
		{
			UIMainMenuMgr.Inst.ShowParticle();
		}
	}

	public void GeneratContent()
	{
		for (int i = 0; i < videoPlayers.Count; i++)
		{
			videoPlayers[i].targetTexture.Release();
			videoPlayers[i].Stop();
			UnityEngine.Object.Destroy(videoPlayers[i]);
		}
		videoPlayers.Clear();
		Content.transform.DestroyAllChild();
		string[] array = 1006001.GetText().Split("\n");
		for (int j = 0; j < array.Length; j++)
		{
			string text = array[j];
			if (text.StartsWith("###"))
			{
				object arg = size3;
				string text2 = text;
				text = $"<b><size={arg}>{text2.Substring(3, text2.Length - 3).Trim()}</size></b>";
			}
			else if (text.StartsWith("##"))
			{
				object arg2 = size2;
				string text2 = text;
				text = $"<b><size={arg2}>{text2.Substring(2, text2.Length - 2).Trim()}</size></b>";
			}
			else if (text.StartsWith("#"))
			{
				object arg3 = size1;
				string text2 = text;
				text = $"<b><size={arg3}>{text2.Substring(1, text2.Length - 1)}</size></b>";
			}
			int num = text.IndexOf("**", StringComparison.Ordinal);
			bool flag = false;
			while (num >= 0)
			{
				string value = (flag ? "</b>" : "<b>");
				flag = !flag;
				text = text.Remove(num, 2);
				text = text.Insert(num, value);
				num = text.IndexOf("**", StringComparison.Ordinal);
			}
			array[j] = text;
		}
		int num2 = 0;
		Text text3 = null;
		string[] array2 = array;
		foreach (string text4 in array2)
		{
			if (text4.Trim().Length == 0)
			{
				continue;
			}
			if (text4.Trim().ToLower() == "@media")
			{
				text3 = null;
				LoadImageOrSpriteObj(num2);
				num2++;
				continue;
			}
			if (text4.Trim() == "---")
			{
				text3 = null;
				UnityEngine.Object.Instantiate(pfbLine, Vector3.one, Quaternion.identity, Content.transform).transform.localPosition = Vector3.zero;
				continue;
			}
			if (text3 == null)
			{
				text3 = UnityEngine.Object.Instantiate(pfbText, Vector3.one, Quaternion.identity, Content.transform).GetComponent<Text>();
				text3.text = "";
				text3.transform.localPosition = Vector3.zero;
			}
			text3.text = (text3.text + "\n\n" + text4).Trim();
		}
	}

	private void LoadImageOrSpriteObj(int i)
	{
		List<RealeaseNoteImageSO.SingleLanguangeImage> list = DataMgr.settingData.language switch
		{
			LanguageType.ChineseS => so.ChineseS, 
			LanguageType.ChineseT => so.ChineseS, 
			LanguageType.English => so.English, 
			_ => so.English, 
		};
		if (i < list.Count)
		{
			if ((bool)list[i].sprite)
			{
				LoadSprite(list[i].sprite, list[i].Res1);
			}
			else if ((bool)list[i].mp4)
			{
				LoadMp4(list[i].mp4, list[i].Res1);
			}
			else if (so.ChineseS[i].sprite != null)
			{
				LoadSprite(so.ChineseS[i].sprite, so.ChineseS[i].Res1);
			}
			else if (so.ChineseS[i].mp4 != null)
			{
				LoadMp4(so.ChineseS[i].mp4, so.ChineseS[i].Res1);
			}
			else
			{
				Debug.Log("图片和视频都不存在");
			}
		}
		void LoadMp4(VideoClip videoClip, Vector2 Res)
		{
			GameObject obj = UnityEngine.Object.Instantiate(pfbRawImage, Vector3.one, Quaternion.identity, Content.transform);
			obj.transform.localPosition = Vector3.zero;
			RawImage component = obj.GetComponent<RawImage>();
			RenderTexture renderTexture = new RenderTexture((int)videoClip.width, (int)videoClip.height, 0);
			VideoPlayer videoPlayer = this.AddComponent<VideoPlayer>();
			component.texture = renderTexture;
			if (Res == Vector2.zero)
			{
				component.SetNativeSize();
			}
			else
			{
				component.GetComponent<RectTransform>().sizeDelta = GetRes(Res, videoClip.height, videoClip.width, mp4: true);
			}
			videoPlayers.Add(videoPlayer);
			videoPlayer.clip = videoClip;
			videoPlayer.time = 0.0;
			videoPlayer.targetTexture = renderTexture;
			videoPlayer.Play();
			videoPlayer.isLooping = true;
		}
		void LoadSprite(Sprite sprite, Vector2 Res)
		{
			GameObject obj2 = UnityEngine.Object.Instantiate(pfbImage, Vector3.one, Quaternion.identity, Content.transform);
			obj2.transform.localPosition = Vector3.zero;
			Image component2 = obj2.GetComponent<Image>();
			component2.sprite = sprite;
			if (Res == Vector2.zero)
			{
				component2.SetNativeSize();
			}
			else
			{
				component2.GetComponent<RectTransform>().sizeDelta = GetRes(Res, (uint)sprite.texture.height, (uint)sprite.texture.width);
			}
		}
	}

	private Vector2 GetRes(Vector2 reff, uint height, uint width, bool mp4 = false)
	{
		if (reff.y == -1f)
		{
			if (mp4)
			{
				return new Vector2(600f, (float)height / (float)width * 600f);
			}
			if (width > 1000)
			{
				return new Vector2(800f, (float)height / (float)width * 800f);
			}
			if (height > 1000)
			{
				return new Vector2((float)width / (float)height * 800f, 800f);
			}
			if (width < 400 || height < 400)
			{
				return new Vector2(width, height);
			}
			return new Vector2(reff.x, (float)height / (float)width * reff.x);
		}
		return reff;
	}

	private void GamepadDirectPerformed_Stick(InputAction.CallbackContext context)
	{
		if (UIMgr.Inst.InputType == PlayerInputType.Gamepad && base.IsOpen)
		{
			Vector2 vector = context.ReadValue<Vector2>();
			vector = ControlMgr.Inst.RampVector2(vector);
			MoveDireStick(vector);
		}
	}

	private void GamepadDirectPerformed(InputAction.CallbackContext context)
	{
		if (UIMgr.Inst.InputType == PlayerInputType.Gamepad && base.IsOpen)
		{
			Vector2 direct = context.ReadValue<Vector2>();
			if (!(Content.GetComponent<RectTransform>().sizeDelta.y - rectTransformScrollView.sizeDelta.y < 0f))
			{
				MoveDire(direct);
			}
		}
	}

	private void MoveDire(Vector2 _direct)
	{
		RectTransform component = Content.GetComponent<RectTransform>();
		float num = 0f;
		if (_direct == Vector2.up)
		{
			num = 0f - GamepadScrollAmount;
		}
		else if (_direct == Vector2.down)
		{
			num = GamepadScrollAmount;
		}
		else if (_direct == Vector2.left)
		{
			num = (0f - GamepadScrollAmount) * 6f;
		}
		else if (_direct == Vector2.right)
		{
			num = GamepadScrollAmount * 6f;
		}
		float y = Mathf.Clamp(component.anchoredPosition.y + num, 0f, component.sizeDelta.y - rectTransformScrollView.sizeDelta.y);
		component.anchoredPosition = new Vector2(0f, y);
	}

	private void MoveDireStick(Vector2 _direct)
	{
		if (_direct == Vector2.up)
		{
			StartScrollingUp();
		}
		else if (_direct == Vector2.down)
		{
			StartScrollingDown();
		}
	}

	private void StopScrolling(InputAction.CallbackContext context)
	{
		StopAllCoroutines();
	}

	private void StartScrollingUp()
	{
		StopAllCoroutines();
		scrollingDir = Vector2.down * GamepadScrollSpeed;
		StartCoroutine(Scroll());
	}

	private void StartScrollingDown()
	{
		StopAllCoroutines();
		scrollingDir = Vector2.up * GamepadScrollSpeed;
		StartCoroutine(Scroll());
	}

	private IEnumerator Scroll()
	{
		while (true)
		{
			Content.GetComponent<RectTransform>().anchoredPosition += scrollingDir;
			yield return new WaitForEndOfFrame();
		}
	}

	protected override void OnShow(object obj = null)
	{
		SEMgr.Inst.uiClick.PlaySE();
		if ((bool)UIMainMenuMgr.Inst)
		{
			UIMainMenuMgr.Inst.HideParticle();
		}
		animator.Play("Show");
	}
}
