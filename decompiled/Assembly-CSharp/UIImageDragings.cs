using UnityEngine;
using UnityEngine.UI;

public class UIImageDragings : MonoBehaviour
{
	public Image image_SlotDraging;

	public Image image_SlotDragingStar1;

	public Image image_SlotDragingStar2;

	public Image image_WandDraging;

	public Image image_PotionDraging;

	public Canvas CanvasDrag;

	public Canvas CanvasWandDrag;

	private void Start()
	{
		if (GameMgr.IsMobile_Static)
		{
			image_SlotDraging.transform.localScale = Vector3.one * 2f;
			image_WandDraging.transform.localScale = Vector3.one * 2f;
			image_PotionDraging.transform.localScale = Vector3.one * 2f;
		}
	}
}
