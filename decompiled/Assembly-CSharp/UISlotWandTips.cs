using UnityEngine;
using UnityEngine.UI;

public class UISlotWandTips : MonoBehaviour
{
	public Vector2 offsetMobile = new Vector2(0f, 32f);

	public Vector2 offsetPC = new Vector2(64f, 32f);

	public GameObject image_UnableToCastSlotSpellAlert;

	public GameObject image_MimicError;

	public GameObject image_Unused;

	public Text text_UnableToCastSlotSpellAlert;

	public Text text_MimicError;

	public Text text_Unused;

	private void Start()
	{
		if (GameMgr.IsMobile_Static)
		{
			base.gameObject.GetComponent<RectTransform>().anchoredPosition = offsetMobile;
		}
		else
		{
			base.gameObject.GetComponent<RectTransform>().anchoredPosition = offsetPC;
		}
	}
}
