using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIBackpackBtnHover : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	public GameObject panel_Desc;

	private void Start()
	{
		if (!GameMgr.IsMobile_Static)
		{
			panel_Desc.SetActive(value: false);
		}
	}

	private void OnDisable()
	{
		if (!GameMgr.IsMobile_Static)
		{
			panel_Desc.SetActive(value: false);
		}
	}

	public void OnPointerEnter([CanBeNull] PointerEventData eventData)
	{
		if (!GameMgr.IsMobile_Static)
		{
			panel_Desc.gameObject.SetActive(value: true);
		}
	}

	public void OnPointerExit([CanBeNull] PointerEventData eventData)
	{
		if (!GameMgr.IsMobile_Static)
		{
			panel_Desc.gameObject.SetActive(value: false);
		}
	}
}
