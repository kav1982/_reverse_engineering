using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UICurse : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerClickHandler
{
	public Image image_Outline;

	public Image image_Icon;

	public Text text_level;

	private bool FromBuildShow;

	private int buildShowID;

	public int buildCurseLevel;

	private int index;

	public int ID => DataMgr.selectedWorldData.battleData9.curseIDs[index];

	public int Level => DataMgr.selectedWorldData.battleData9.curseLevels[index];

	public void Initialize(int index, FinishGameBuild build = null)
	{
		this.index = index;
		int num = build?.curseIDs[index] ?? ID;
		int num2 = build?.curseLevels[index] ?? Level;
		if (build != null)
		{
			FromBuildShow = true;
			buildShowID = num;
			buildCurseLevel = num2;
		}
		image_Outline.sprite = ABResources.LoadAsset<Sprite>("Textures/CurseIcons/" + num);
		image_Icon.sprite = image_Outline.sprite;
		if (num2 == 1)
		{
			text_level.text = "";
		}
		else
		{
			text_level.text = num2.ToString();
		}
	}

	public void Hover()
	{
		image_Outline.enabled = true;
	}

	public void Unhover()
	{
		image_Outline.enabled = false;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (!GameMgr.IsMobile_Static || MobileMgr.inst.gamepadPlugged)
		{
			if (FromBuildShow)
			{
				UIPlayerDataMgr.Inst.UICurseEnter(this, buildShowID);
			}
			else
			{
				UIPlayerDataMgr.Inst.UICurseEnter(this);
			}
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (!GameMgr.IsMobile_Static || MobileMgr.inst.gamepadPlugged)
		{
			UIPlayerDataMgr.Inst.UICurseExit(this);
		}
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (!GameMgr.IsMobile_Static)
		{
			return;
		}
		UIPlayerDataMgr.Inst.uiInfoRelicHover.gameObject.SetActive(value: false);
		UIPlayerDataMgr.Inst.UIRelicExit();
		if (FromBuildShow)
		{
			if (UIPlayerDataMgr.Inst.uiCurse_Hover == null)
			{
				UIPlayerDataMgr.Inst.UICurseEnter(this, buildShowID);
			}
			else if (UIPlayerDataMgr.Inst.uiCurse_Hover != null && UIPlayerDataMgr.Inst.uiCurse_Hover != this)
			{
				UIPlayerDataMgr.Inst.UICurseExit();
				UIPlayerDataMgr.Inst.UICurseEnter(this, buildShowID);
			}
			else
			{
				UIPlayerDataMgr.Inst.UICurseExit();
			}
		}
	}
}
