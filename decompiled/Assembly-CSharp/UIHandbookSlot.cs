using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIHandbookSlot : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerClickHandler
{
	public Coroutine IE_rotate;

	public GameObject FoldImage;

	public bool fold;

	public GameObject go_HoverImage;

	public Text text;

	public Color color_Title;

	public Color color_Content;

	public int sizeTitle = 24;

	public int sizeContent = 22;

	private int categoryItselfTextID;

	private UIHandbook uiHandbook;

	private int index;

	public HandbookConfig HandbookCfg { get; private set; }

	public void InitializeCategoryItself(UIHandbook uiHandbook, int index, int categoryItselfTextID)
	{
		this.uiHandbook = uiHandbook;
		this.index = index;
		this.categoryItselfTextID = categoryItselfTextID;
		text.color = color_Title;
		text.fontSize = sizeTitle;
		UpdateInfo();
	}

	public void InitializeSlot(UIHandbook uiHandbook, int index, HandbookConfig handbookCfg)
	{
		this.uiHandbook = uiHandbook;
		this.index = index;
		HandbookCfg = handbookCfg;
		text.color = color_Content;
		text.fontSize = sizeContent;
		UpdateInfo();
	}

	public void UpdateInfo()
	{
		if (HandbookCfg == null)
		{
			text.text = categoryItselfTextID.GetText();
			LayoutRebuilder.ForceRebuildLayoutImmediate(text.rectTransform);
			Debug.Log(text.rectTransform.sizeDelta.x);
			new Vector2((base.transform.GetComponent<RectTransform>().sizeDelta.x - text.rectTransform.sizeDelta.x) / 2f, 4f);
		}
		else
		{
			text.text = HandbookCfg.GetTitle();
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (!GameMgr.IsMobile_Static || ControlMgr.Inst.InputType == PlayerInputType.Gamepad)
		{
			go_HoverImage.SetActive(value: true);
			uiHandbook.SlotEnter(this);
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (!GameMgr.IsMobile_Static || ControlMgr.Inst.InputType == PlayerInputType.Gamepad)
		{
			go_HoverImage.SetActive(value: false);
			uiHandbook.SlotExit();
		}
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		ClickHandBook();
	}

	public void ClickHandBook()
	{
		UIHandbookSlot uIHandbookSlot = UIMgr.Inst.UIMenu.uihandbook.Slots[UIMgr.Inst.UIMenu.uihandbook.Selectindex];
		UIMgr.Inst.UIMenu.uihandbook.SlotExit();
		uIHandbookSlot.go_HoverImage.SetActive(value: false);
		UIMgr.Inst.UIMenu.uihandbook.Selectindex = index;
		if (HandbookCfg != null)
		{
			go_HoverImage.SetActive(value: true);
			UIMgr.Inst.UIMenu.uihandbook.SlotEnter(this);
		}
	}

	public void Rotate_Fold_Arrow_Down()
	{
		if (IE_rotate != null)
		{
			StopAllCoroutines();
		}
		IE_rotate = StartCoroutine(Rotate_Down());
	}

	public void Rotate_Fold_Arrow_Right()
	{
		if (IE_rotate != null)
		{
			StopAllCoroutines();
		}
		IE_rotate = StartCoroutine(Rotate_Right());
	}

	public IEnumerator Rotate_Down()
	{
		while (FoldImage.transform.localRotation.eulerAngles.z >= 30f)
		{
			FoldImage.transform.Rotate(new Vector3(0f, 0f, -30f), Space.Self);
			yield return new WaitForSecondsRealtime(0.01f);
		}
		FoldImage.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
		IE_rotate = null;
	}

	public IEnumerator Rotate_Right()
	{
		while (FoldImage.transform.localRotation.eulerAngles.z <= 60f)
		{
			FoldImage.transform.Rotate(new Vector3(0f, 0f, 30f), Space.Self);
			yield return new WaitForSecondsRealtime(0.01f);
		}
		FoldImage.transform.localRotation = Quaternion.Euler(0f, 0f, 90f);
		IE_rotate = null;
	}
}
