using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPanelInteract : MonoBehaviour
{
	public TMP_Text tmp_InteractTip;

	public RectTransform rtsf_InteractBtn;

	public GameObject panel_InteractBtn;

	public GameObject panel_InteractBtnMask;

	public UpdatButtonShow panel_InteractBtnShow;

	[SerializeField]
	private Text text_InteractTip;

	public RectTransform rtsf_InteractButtonText;

	public Text text_InteractButton;

	private float offset;

	public List<GameObject> HideIfControllerUnPlugged = new List<GameObject>();

	public void Init()
	{
		if (offset == 0f)
		{
			offset = rtsf_InteractBtn.sizeDelta.x - text_InteractButton.preferredWidth;
		}
	}

	public void SetText(string text)
	{
		if (GameMgr.IsMobile_Static)
		{
			tmp_InteractTip.text = text;
		}
		else
		{
			text_InteractTip.text = text;
		}
	}

	public void UpdateRect()
	{
		HideIfControllerUnPlugged.ForEach(delegate(GameObject x)
		{
			x.SetActive(GameMgr.IsMobile_Static && MobileMgr.inst.gamepadPlugged);
		});
		if (!GameMgr.IsMobile_Static || MobileMgr.inst.gamepadPlugged)
		{
			Vector2 vector3 = (rtsf_InteractButtonText.sizeDelta = (rtsf_InteractButtonText.sizeDelta = new Vector2(text_InteractButton.preferredWidth, rtsf_InteractButtonText.sizeDelta.y)));
			rtsf_InteractBtn.sizeDelta = new Vector2(offset + text_InteractButton.preferredWidth, rtsf_InteractBtn.sizeDelta.y);
		}
	}
}
