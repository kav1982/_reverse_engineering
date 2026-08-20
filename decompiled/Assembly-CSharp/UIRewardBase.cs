using Unity.Entities;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class UIRewardBase : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerClickHandler
{
	public RectTransform rtsf_BG;

	protected Entity levelRewardEtt;

	protected int index;

	protected bool interactable = true;

	public Canvas canvas;

	public GraphicRaycaster GraphicRaycaster;

	public virtual void Initialize(Entity levelRewardEtt, int index)
	{
	}

	public virtual void SetShow()
	{
	}

	public virtual void SetHide()
	{
		if ((bool)canvas)
		{
			canvas.overrideSorting = false;
		}
	}

	public virtual void OnPointerEnter(PointerEventData eventData)
	{
	}

	public virtual void OnPointerExit(PointerEventData eventData)
	{
	}

	public virtual void OnPointerClick(PointerEventData eventData)
	{
	}

	public virtual void Select()
	{
		if (GameMgr.IsMobile_Static && (bool)GraphicRaycaster)
		{
			GraphicRaycaster.enabled = false;
		}
	}

	public virtual void Hover()
	{
		if ((bool)canvas)
		{
			canvas.overrideSorting = true;
		}
	}

	public virtual void UnHover()
	{
		if ((bool)canvas)
		{
			canvas.overrideSorting = false;
		}
	}
}
