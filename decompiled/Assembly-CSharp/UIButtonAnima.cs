using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIButtonAnima : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerClickHandler, IPointerExitHandler
{
	public UIButtonAnimaSE se;

	public bool playEnterSE = true;

	private Button belongButton;

	private void Start()
	{
		belongButton = GetComponent<Button>();
		if (belongButton == null)
		{
			Debug.LogError(base.gameObject.name + " 该Gameobj上没有Button组件");
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (belongButton.interactable)
		{
			base.transform.DOScale(1.2f, 0.05f).SetUpdate(isIndependentUpdate: true);
			switch (se)
			{
			case UIButtonAnimaSE.Button:
				SEMgr.Inst.uiButtonHover_Button.PlaySE();
				break;
			case UIButtonAnimaSE.Dice:
				SEMgr.Inst.uiButtonHover_Dice.PlaySE();
				break;
			default:
				Debug.LogError(se);
				break;
			}
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (belongButton.interactable)
		{
			base.transform.DOScale(1f, 0.05f).SetUpdate(isIndependentUpdate: true);
		}
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (belongButton.interactable)
		{
			DOTween.Sequence().Append(base.transform.DOScale(1.2f, 0.05f)).Append(base.transform.DOScale(1f, 0.05f))
				.SetUpdate(isIndependentUpdate: true);
		}
	}
}
