using UnityEngine;
using UnityEngine.UI;

public class UISell_Item : MonoBehaviour
{
	public RectTransform rtsf_Self;

	public CanvasGroup cg;

	public Image image_Icon;

	public GameObject go_Star1;

	public GameObject go_Star2;

	public Text text_Count;

	public Animator anima;

	public float moveLerp;

	public float lerpThreshold;

	public float flyLerpSpeed;

	public float flyHight;

	public int totalShowSpace;

	public int totalHideSpace;

	public float space;

	private int index;

	private int selectedIndexOfMove;

	private bool isMove;

	public UISellItemType ItemType { get; private set; }

	public int ItemID { get; private set; }

	public int Count { get; private set; }

	private void Update()
	{
		if (isMove)
		{
			Vector2 vector = new Vector2(space, 0f) * (index - selectedIndexOfMove);
			rtsf_Self.anchoredPosition = Vector2.Lerp(rtsf_Self.anchoredPosition, vector, moveLerp * Time.deltaTime);
			if (Vector2.SqrMagnitude(rtsf_Self.anchoredPosition - vector) < lerpThreshold * lerpThreshold)
			{
				isMove = false;
			}
			UpdateAlpha();
		}
	}

	private void UpdateInfo()
	{
		go_Star1.SetActive(value: false);
		go_Star2.SetActive(value: false);
		switch (ItemType)
		{
		case UISellItemType.Spell:
		{
			SpellConfig spellConfig = SpellConfig.dic[ItemID];
			image_Icon.sprite = ABResources.LoadAsset<Sprite>(spellConfig.GetIconPath());
			if (spellConfig.level > 1)
			{
				go_Star1.SetActive(value: true);
			}
			if (spellConfig.level > 2)
			{
				go_Star2.SetActive(value: true);
			}
			break;
		}
		case UISellItemType.Relic:
			image_Icon.sprite = ABResources.LoadAsset<Sprite>(RelicConfig.dic[ItemID].GetIconPath());
			break;
		case UISellItemType.Potion:
			image_Icon.sprite = ABResources.LoadAsset<Sprite>(PotionConfig.dic[ItemID].GetIconPath());
			break;
		default:
			Debug.LogError(ItemType);
			break;
		}
		if (Count == 1)
		{
			text_Count.gameObject.SetActive(value: false);
			return;
		}
		text_Count.gameObject.SetActive(value: true);
		text_Count.text = "×" + Count;
	}

	private void UpdateAlpha()
	{
		float num = Vector2.Distance(rtsf_Self.anchoredPosition, Vector2.zero) / space;
		float num2 = 0f;
		num2 = ((num <= (float)totalShowSpace) ? 1f : ((!(num >= (float)totalHideSpace)) ? Mathf.Lerp(1f, 0f, (num - (float)totalShowSpace) / (float)(totalHideSpace - totalShowSpace)) : 0f));
		cg.alpha = num2;
	}

	public void Initialize(int index, UISellItemType itemType, int itemID, int count)
	{
		this.index = index;
		ItemType = itemType;
		ItemID = itemID;
		Count = count;
		UpdateInfo();
		UpdateAlpha();
		SetMove(0);
	}

	public void SetMove(int selectedIndexOfMove)
	{
		isMove = true;
		this.selectedIndexOfMove = selectedIndexOfMove;
	}

	public void ChangeID(int newID)
	{
		ItemID = newID;
		UpdateInfo();
		anima.SetTrigger("Action");
	}

	public void ChangeCount(int newCount)
	{
		Count = newCount;
		UpdateInfo();
		anima.SetTrigger("Action");
		if (GameUISingletonMono<UICompound>.StaticIsOpen && GameUISingletonMono<UICompound>.Inst.SelectedUIRS == this)
		{
			GameUISingletonMono<UICompound>.Inst.UpdateInfo();
		}
	}

	public void ChangeIndex(int newIndex, int SelectedItemIndex)
	{
		index = newIndex;
		SetMove(SelectedItemIndex);
	}
}
