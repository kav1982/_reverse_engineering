using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UISlotMoreInOne : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerClickHandler
{
	public RectTransform rtsf_Self;

	public Image image_BG;

	public Image image_Icon;

	public Image image_Spelllevel2Star;

	public Image image_Spelllevel3Star;

	public float hoverScale;

	public float flySpeed;

	public float blendingLerpSpeed;

	public float blendingMiddlePointDistance;

	private float blendingLerpTimer;

	public UISlotMoreInOneState State { get; private set; }

	public int ID { get; private set; }

	public SpellConfig SpellCfg
	{
		get
		{
			if (ID == 0)
			{
				return null;
			}
			return SpellConfig.dic[ID];
		}
	}

	private void Start()
	{
		image_Icon.transform.SetParent(base.transform.parent);
	}

	private void Update()
	{
		switch (State)
		{
		case UISlotMoreInOneState.SetSpell:
			image_Icon.transform.position = Vector3.MoveTowards(image_Icon.transform.position, base.transform.position, flySpeed * Time.deltaTime);
			if (image_Icon.transform.position == base.transform.position)
			{
				State = UISlotMoreInOneState.Idle;
			}
			break;
		case UISlotMoreInOneState.BackSpell:
			image_Icon.transform.position = Vector3.MoveTowards(image_Icon.transform.position, GameUISingletonMono<UIMoreInOne>.Inst.rtsf_Spells.transform.position, flySpeed * Time.deltaTime);
			if (image_Icon.transform.position == GameUISingletonMono<UIMoreInOne>.Inst.rtsf_Spells.transform.position)
			{
				image_Icon.gameObject.SetActive(value: false);
				int iD = ID;
				ID = 0;
				State = UISlotMoreInOneState.Idle;
				GameUISingletonMono<UIMoreInOne>.Inst.BackSpell(iD);
			}
			break;
		case UISlotMoreInOneState.Blending:
		{
			Vector3 v = rtsf_Self.anchoredPosition + rtsf_Self.anchoredPosition.normalized * blendingMiddlePointDistance;
			blendingLerpTimer += Time.deltaTime;
			image_Icon.rectTransform.anchoredPosition = GeneralTool.QuadraticBezierCurve(rtsf_Self.anchoredPosition, v, Vector3.zero, blendingLerpTimer);
			if (blendingLerpTimer >= 1f)
			{
				image_Icon.gameObject.SetActive(value: false);
				GameUISingletonMono<UIMoreInOne>.Inst.BlendingFinish();
				State = UISlotMoreInOneState.Idle;
			}
			break;
		}
		default:
			Debug.LogError(State);
			break;
		case UISlotMoreInOneState.Idle:
			break;
		}
	}

	private void UpdateInfo()
	{
		image_Icon.gameObject.SetActive(value: false);
		image_Spelllevel2Star.gameObject.SetActive(value: false);
		image_Spelllevel3Star.gameObject.SetActive(value: false);
		if (ID != 0)
		{
			image_Icon.gameObject.SetActive(value: true);
			image_Icon.sprite = ABResources.LoadAsset<Sprite>(SpellCfg.GetIconPath());
			if (SpellCfg.level > 1)
			{
				image_Spelllevel2Star.gameObject.SetActive(value: true);
			}
			if (SpellCfg.level > 2)
			{
				image_Spelllevel3Star.gameObject.SetActive(value: true);
			}
		}
	}

	public void Reset()
	{
		State = UISlotMoreInOneState.Idle;
		image_BG.enabled = true;
		image_Icon.transform.position = base.transform.position;
		blendingLerpTimer = 0f;
		ID = 0;
		UpdateInfo();
	}

	public void SetSpell(int id)
	{
		ID = id;
		UpdateInfo();
		image_Icon.transform.position = GameUISingletonMono<UIMoreInOne>.Inst.rtsf_Spells.transform.position;
		State = UISlotMoreInOneState.SetSpell;
	}

	public void GetSpellDirect(int id)
	{
		ID = id;
		UpdateInfo();
	}

	public void Blending()
	{
		State = UISlotMoreInOneState.Blending;
		image_BG.enabled = false;
	}

	public void Hover()
	{
		image_Icon.transform.localScale = Vector3.one * hoverScale;
	}

	public void Unhover()
	{
		image_Icon.transform.localScale = Vector3.one;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		GameUISingletonMono<UIMoreInOne>.Inst.UISlotMoreInOneEnter(this);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		GameUISingletonMono<UIMoreInOne>.Inst.UISlotMoreInOneExit(this);
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (ID != 0 && State == UISlotMoreInOneState.Idle)
		{
			State = UISlotMoreInOneState.BackSpell;
			OnPointerExit(null);
		}
	}
}
