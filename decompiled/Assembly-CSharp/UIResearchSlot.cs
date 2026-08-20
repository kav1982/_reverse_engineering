using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIResearchSlot : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerClickHandler
{
	public GameObject go_ResearchedMask;

	public GameObject go_Select;

	public GameObject go_Blood;

	public GameObject go_Active;

	public Text textIsActive;

	public Color colorTextActive;

	public Color colorTextDisactive;

	public Image image_Icon;

	public Text text_Name;

	public Color colorTextDefaultName;

	public Color colorTextDefaultDes;

	public Color colorTextActiveName;

	public Color colorTextDisactiveName;

	public Color colorTextActiveDes;

	public Color colorTextDisactiveDes;

	public Color colorTextCantActiveUnlockedName;

	public Color colorTextCantActiveUnlockedDes;

	public Text text_Desc;

	public Text text_Cost;

	public Image imageBackground;

	public Sprite spriteBackgroundAlreadyResearched;

	public Animator anima;

	public Text text_New;

	public GameObject researchLevelStar1;

	public GameObject researchLevelStar2;

	public GameObject researchLevelStar3;

	public GameObject activateKnob;

	public Transform tsfActivateKnobActive;

	public Transform tsfActivateKnobDisactive;

	public Image ActivateFill;

	private float KnobAnimaTIme = 0.2f;

	private UIResearch uiResearch;

	private bool skipOnceSE;

	public ResearchConfig Cfg => ResearchConfig.dic[ID];

	public int ID { get; private set; }

	public bool IsResearched => DataMgr.selectedWorldData.researchedIDs.Contains(ID);

	public bool IsActive => !DataMgr.selectedWorldData.researchDisactive.Contains(ID);

	public void Initialize(UIResearch uiResearch, int id)
	{
		this.uiResearch = uiResearch;
		ID = id;
		image_Icon.sprite = ABResources.LoadAsset<Sprite>("Textures/ResearchIcons/" + Cfg.icon);
		UpdateState();
		CheckCost();
		if (Cfg.openType != 0 && !DataMgr.selectedWorldData.researchHoveredIDs.Contains(id))
		{
			text_New.gameObject.SetActive(value: true);
		}
	}

	public void UpdateState(bool anime = false)
	{
		int num = ResearchConfig.HavePostResearch(ID);
		bool flag = false;
		if (num != -1)
		{
			flag = DataMgr.selectedWorldData.researchedIDs.Contains(num);
		}
		if (flag)
		{
			base.gameObject.SetActive(value: false);
		}
		if (IsResearched)
		{
			imageBackground.sprite = spriteBackgroundAlreadyResearched;
			go_Blood.SetActive(value: false);
			if (Cfg.canDisactive)
			{
				go_Active.SetActive(value: true);
				if (IsActive)
				{
					SetUIActivate(anime);
					textIsActive.color = colorTextActive;
					text_Name.color = colorTextActiveName;
					text_Desc.color = colorTextActiveDes;
				}
				else
				{
					SetUIDisactive(anime);
					textIsActive.color = colorTextDisactive;
					text_Name.color = colorTextDisactiveName;
					text_Desc.color = colorTextDisactiveDes;
				}
			}
			else
			{
				go_ResearchedMask.SetActive(value: true);
				text_Name.color = colorTextCantActiveUnlockedName;
				text_Desc.color = colorTextCantActiveUnlockedDes;
			}
		}
		else
		{
			go_ResearchedMask.SetActive(value: false);
			go_Active.SetActive(value: false);
			if (Cfg.preResearchID == 0)
			{
				base.gameObject.SetActive(value: true);
			}
			else if (DataMgr.selectedWorldData.researchedIDs.Contains(Cfg.preResearchID))
			{
				base.gameObject.SetActive(value: true);
			}
			else
			{
				base.gameObject.SetActive(value: false);
			}
		}
	}

	public void UpdateLanguage()
	{
		text_Name.text = Cfg.GetName();
		text_Desc.text = Cfg.GetDesc();
		text_Cost.text = Cfg.cost.ToString();
		text_New.text = 1002107.GetText();
		if (IsResearched && Cfg.canDisactive)
		{
			if (IsActive)
			{
				textIsActive.text = 1002109.GetText();
			}
			else
			{
				textIsActive.text = 1002110.GetText();
			}
		}
	}

	public void CheckCost()
	{
		if (!IsResearched)
		{
			if (Cfg.cost <= DataMgr.selectedWorldData.ancientBloodCount)
			{
				text_Cost.color = Color.green;
			}
			else
			{
				text_Cost.color = Color.red;
			}
		}
	}

	public void SkipOnceSE()
	{
		skipOnceSE = true;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (text_New.gameObject.activeSelf && uiResearch.showFinish)
		{
			DataMgr.selectedWorldData.AddResearchHoveredID(ID);
			text_New.gameObject.SetActive(value: false);
		}
		if (!IsResearched || (IsResearched && Cfg.canDisactive))
		{
			go_Select.SetActive(value: true);
			if (skipOnceSE)
			{
				skipOnceSE = false;
			}
			else
			{
				SEMgr.Inst.uiResearchHover.PlaySE();
			}
		}
	}

	public void OnPointerEnterPad()
	{
		if (text_New.gameObject.activeSelf && uiResearch.showFinish)
		{
			DataMgr.selectedWorldData.AddResearchHoveredID(ID);
			text_New.gameObject.SetActive(value: false);
		}
		if (IsResearched)
		{
			if (ControlMgr.Inst.usingpad)
			{
				go_Select.SetActive(value: true);
			}
		}
		else
		{
			go_Select.SetActive(value: true);
		}
		if (skipOnceSE)
		{
			skipOnceSE = false;
		}
		else
		{
			SEMgr.Inst.uiResearchHover.PlaySE();
		}
	}

	public void OnPointerExitPad()
	{
		go_Select.SetActive(value: false);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		go_Select.SetActive(value: false);
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		uiResearch.SlotClick(this);
		UpdateLanguage();
	}

	public void SetUIActivate(bool anime = false)
	{
		if (anime)
		{
			activateKnob.transform.position = tsfActivateKnobDisactive.position;
			activateKnob.transform.DOMove(tsfActivateKnobActive.position, KnobAnimaTIme);
			ActivateFill.fillAmount = 0f;
			ActivateFill.DOFillAmount(1f, KnobAnimaTIme);
		}
		else
		{
			activateKnob.transform.position = tsfActivateKnobActive.position;
			ActivateFill.fillAmount = 1f;
		}
	}

	public void SetUIDisactive(bool anime = false)
	{
		if (anime)
		{
			activateKnob.transform.position = tsfActivateKnobActive.position;
			activateKnob.transform.DOMove(tsfActivateKnobDisactive.position, KnobAnimaTIme);
			ActivateFill.fillAmount = 1f;
			ActivateFill.DOFillAmount(0f, KnobAnimaTIme);
		}
		else
		{
			activateKnob.transform.position = tsfActivateKnobDisactive.position;
			ActivateFill.fillAmount = 0f;
		}
	}
}
