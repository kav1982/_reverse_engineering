using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using DG.Tweening;
using GameServer;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class UINeedUpdatePenel : MonoBehaviour
{
	public UpdateNoticeType updateNoticeType;

	public GameObject uiRootObj;

	public CanvasGroup canvasGroup;

	public Action ActionOnClose;

	[Header("更新公告")]
	public GameObject updateNotice;

	public GameObject updateContent;

	public GameObject ButtonForceUpdate;

	public GameObject ButtonNotForceUpdate;

	[Header("纯公告")]
	public Text titleNotice;

	public GameObject notice;

	public GameObject noticeContent;

	public const float ImgMaxWidth = 800f;

	private string updateUrl;

	private string defaultUrl;

	public bool UseNotice
	{
		get
		{
			UpdateNoticeType updateNoticeType = this.updateNoticeType;
			return updateNoticeType == UpdateNoticeType.CurrentVersionNotice || updateNoticeType == UpdateNoticeType.Login || updateNoticeType == UpdateNoticeType.Maintain;
		}
	}

	public void Show(object obj = null)
	{
		if (!(obj is int num))
		{
			if (!(obj is ITuple tuple) || tuple.Length != 2)
			{
				return;
			}
			object obj2 = tuple[0];
			if (obj2 is string text)
			{
				object obj3 = tuple[1];
				if (obj3 is UpdateNoticeType)
				{
					UpdateNoticeType updateNoticeType = (this.updateNoticeType = (UpdateNoticeType)obj3);
					Debug.Log("UINeedUpdatePenel.Show -> 请求公告" + text);
					IEnumerator noticeById = ServerAPI.GetNoticeById(text, OnNoticeDetailResponse, OnNoticeDetailErr);
					StartCoroutine(noticeById);
				}
			}
			else if (obj2 is ServerAPI.Notice obj4)
			{
				object obj3 = tuple[1];
				if (obj3 is UpdateNoticeType)
				{
					UpdateNoticeType updateNoticeType = (UpdateNoticeType)obj3;
					UpdateNoticeType updateNoticeType2 = (this.updateNoticeType = updateNoticeType);
					UpdateNotice(obj4);
				}
			}
		}
		else
		{
			this.updateNoticeType = UpdateNoticeType.CurrentVersionNotice;
			Debug.Log($"UINeedUpdatePenel.Show -> 请求公告{num}");
			IEnumerator noticeByVersion = ServerAPI.GetNoticeByVersion(num, OnNoticeDetailResponse, OnNoticeDetailErr);
			StartCoroutine(noticeByVersion);
		}
	}

	private void OnNoticeDetailErr(UnityWebRequest obj)
	{
		Hide();
		Debug.LogError("UINeedUpdatePenel.OnNoticeDetailErr -> " + obj.error);
	}

	private void UpdateNotice(ServerAPI.Notice obj)
	{
		if (obj == null && updateNoticeType == UpdateNoticeType.CurrentVersionNotice)
		{
			Hide();
			GameUISingletonMono<UICommonHint>.ShowInit("当前版本没有公告");
			return;
		}
		Debug.Log("UINeedUpdatePenel.OnNoticeDetailResponse -> 公告消息返回");
		canvasGroup.alpha = 0f;
		canvasGroup.blocksRaycasts = true;
		canvasGroup.DOFade(1f, 0.5f);
		notice.SetActive(UseNotice);
		updateNotice.SetActive(!UseNotice);
		ButtonNotForceUpdate.SetActive(updateNoticeType == UpdateNoticeType.Update);
		ButtonForceUpdate.SetActive(updateNoticeType == UpdateNoticeType.ForceUpdate);
		switch (updateNoticeType)
		{
		case UpdateNoticeType.Maintain:
			titleNotice.text = "维护公告";
			break;
		case UpdateNoticeType.Login:
			titleNotice.text = "游戏公告";
			break;
		case UpdateNoticeType.CurrentVersionNotice:
			titleNotice.text = "当前版本";
			break;
		}
		GameObject gameObject = (UseNotice ? noticeContent : this.updateContent);
		foreach (Transform item in gameObject.transform)
		{
			UnityEngine.Object.Destroy(item.gameObject);
		}
		GameObject gameObject2 = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Prefabs/UI/NoticeTitle"), gameObject.transform);
		string text = "";
		switch (updateNoticeType)
		{
		case UpdateNoticeType.Maintain:
			text = "维护中";
			break;
		case UpdateNoticeType.Login:
			text = "暂无公告";
			break;
		}
		Text component = gameObject2.transform.Find("Title").GetComponent<Text>();
		component.text = text;
		if (obj == null)
		{
			return;
		}
		component.text = obj.title;
		TextImageParser.UpdateContent updateContent = TextImageParser.GetUpdateContent(obj.rich_text);
		if (!int.TryParse(PluginActivity.channleID, out var result))
		{
			result = 1001;
		}
		if (int.TryParse(PluginActivity.adChannleID, out var result2))
		{
			updateUrl = updateContent.IdUrlMap.GetValueOrDefault((result, result2), "");
		}
		else
		{
			updateUrl = updateContent.IdUrlMap.GetValueOrDefault((result, 0), "");
		}
		defaultUrl = updateContent.DefaultUrl;
		Debug.Log(updateContent.DefaultUrl);
		foreach (TextImageParser.ContentBlock content in TextImageParser.ParseMixedContent(updateContent.OutsideText))
		{
			switch (content.Type)
			{
			case TextImageParser.ContentBlock.ContentType.Text:
				UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Prefabs/UI/NoticeText"), gameObject.transform).GetComponent<Text>().text = content.Content;
				break;
			case TextImageParser.ContentBlock.ContentType.Image:
			{
				Texture2D texture2D = GeneralTool.LoadTexturesFromBase64(content.Content);
				RawImage component3 = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Prefabs/UI/NoticeImg"), gameObject.transform).transform.Find("RawImg").GetComponent<RawImage>();
				component3.texture = texture2D;
				component3.SetNativeSize();
				if (!((float)texture2D.width <= 800f))
				{
					float num = (float)texture2D.width * 1f / (float)texture2D.height;
					float y = 800f / num;
					component3.GetComponent<RectTransform>().sizeDelta = new Vector2(800f, y);
				}
				break;
			}
			case TextImageParser.ContentBlock.ContentType.Link:
			{
				GameObject obj2 = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Prefabs/UI/NoticeLink"), gameObject.transform);
				Text componentInChildren = obj2.GetComponentInChildren<Text>();
				Button component2 = obj2.GetComponent<Button>();
				component2.onClick.RemoveAllListeners();
				Debug.Log(content.Content2);
				component2.onClick.AddListener(delegate
				{
					Application.OpenURL(content.Content2);
				});
				componentInChildren.text = content.Content;
				break;
			}
			default:
				throw new ArgumentOutOfRangeException();
			}
		}
	}

	private void OnNoticeDetailResponse(Response<ServerAPI.Notice> obj)
	{
		UpdateNotice(obj.data);
	}

	public void Hide()
	{
		ActionOnClose?.Invoke();
		ActionOnClose = null;
		canvasGroup.blocksRaycasts = false;
		canvasGroup.DOFade(0f, 0.5f);
		ButtonNotForceUpdate.SetActive(value: false);
		ButtonForceUpdate.SetActive(value: false);
	}

	public void _Update()
	{
		EventMgr.RoleItemChange?.Invoke();
		Debug.Log("更新渠道:" + PluginActivity.adChannleID);
		if (!string.IsNullOrWhiteSpace(updateUrl))
		{
			Application.OpenURL(updateUrl);
		}
		else if (!string.IsNullOrWhiteSpace(defaultUrl))
		{
			Application.OpenURL(defaultUrl);
		}
	}

	public void _DontUpdate()
	{
		Hide();
	}
}
