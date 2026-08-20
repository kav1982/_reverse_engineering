using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIBLiveGiftMessageCtrl : MonoBehaviour
{
	public Sprite AddCurseGiftIcon;

	public Sprite RemoveCurseGiftIcon;

	public Sprite AddRelicGiftIcon;

	private readonly Queue<BLiveGiftMessage> _queue = new Queue<BLiveGiftMessage>();

	private const int MAX_QUEUE_SIZE = 20;

	private RectTransform _rect;

	private float _timer;

	public static UIBLiveGiftMessageCtrl Inst { get; private set; }

	private void Start()
	{
		_rect = GetComponent<RectTransform>();
		Inst = this;
		if (BLiveMgr.Inst == null)
		{
			base.gameObject.SetActive(value: false);
		}
	}

	private void Update()
	{
		CostMessageUpdate();
	}

	private void CostMessageUpdate()
	{
		if (_timer >= 0f)
		{
			_timer -= Time.unscaledDeltaTime;
		}
		else if (_queue.Count != 0)
		{
			_timer = (float)Mathf.Max(0, 20 - _queue.Count) / 20f * 0.4f;
			CostMessage();
		}
	}

	private void CostMessage()
	{
		BLiveGiftMessage msg = _queue.Dequeue();
		GameObject go = ObjPoolMgr.Inst.GetGO("Prefabs/UI/UIBLiveGiftMessage", _rect);
		Image iconImg = go.transform.Find("Icon").GetComponent<Image>();
		iconImg.sprite = GetIcon(msg.Type);
		go.GetComponentInChildren<Text>().text = msg.FormatedMessage;
		CanvasGroup component = go.GetComponent<CanvasGroup>();
		component.alpha = 1f;
		RectTransform component2 = go.GetComponent<RectTransform>();
		component2.anchoredPosition = new Vector2(0f, -60f);
		component2.localScale = Vector3.one;
		DOTween.Sequence(go).SetUpdate(isIndependentUpdate: true).Append(component2.DOLocalMoveY(0f, 0.5f))
			.AppendInterval(0.4f)
			.AppendCallback(delegate
			{
				msg.OnAction?.Invoke(iconImg.GetComponent<RectTransform>());
			})
			.AppendInterval(0.6f)
			.Append(component.DOFade(0f, 0.2f))
			.AppendCallback(delegate
			{
				ObjPoolMgr.Inst.RecycleGO(go);
			});
	}

	public void AppendMessage(BLiveGiftMessage message)
	{
		_queue.Enqueue(message);
	}

	public bool CanAppendMessage()
	{
		return _queue.Count < 20;
	}

	private Sprite GetIcon(BLiveGiftType type)
	{
		return type switch
		{
			BLiveGiftType.AddCurse => AddCurseGiftIcon, 
			BLiveGiftType.RemoveCurse => RemoveCurseGiftIcon, 
			BLiveGiftType.AddRelic => AddRelicGiftIcon, 
			_ => throw new ArgumentOutOfRangeException("type", type, null), 
		};
	}
}
