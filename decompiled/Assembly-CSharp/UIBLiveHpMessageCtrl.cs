using DG.Tweening;
using UnityEngine;

public class UIBLiveHpMessageCtrl : MonoBehaviour
{
	public RectTransform HpRect;

	public RectTransform ShieldRect;

	public RectTransform TempShieldRect;

	private RectTransform self;

	public static UIBLiveHpMessageCtrl Inst { get; private set; }

	private RectTransform FloatTargetRect
	{
		get
		{
			if (TempShieldRect.gameObject.activeInHierarchy)
			{
				return TempShieldRect;
			}
			if (ShieldRect.gameObject.activeInHierarchy)
			{
				return ShieldRect;
			}
			return HpRect;
		}
	}

	private void Start()
	{
		self = GetComponent<RectTransform>();
		if (BLiveMgr.Inst == null)
		{
			base.gameObject.SetActive(value: false);
		}
		Inst = this;
	}

	private void Update()
	{
		float x = FloatTargetRect.rect.width + FloatTargetRect.anchoredPosition.x;
		Vector2 anchoredPosition = self.anchoredPosition;
		anchoredPosition.x = x;
		self.anchoredPosition = anchoredPosition;
	}

	public void NewText()
	{
		GameObject go = ObjPoolMgr.Inst.GetGO("Prefabs/UI/UIBLiveHpMessageText", self);
		RectTransform component = go.GetComponent<RectTransform>();
		component.anchoredPosition = Vector2.zero;
		component.localScale = Vector3.zero;
		DOTween.Sequence(go).Append(component.DOScale(1f, 0.2f)).AppendInterval(0.5f)
			.Append(component.DOLocalMoveY(50f, 0.5f))
			.Join(component.DOScale(0f, 0.5f))
			.AppendCallback(delegate
			{
				ObjPoolMgr.Inst.RecycleGO(go);
			});
	}
}
