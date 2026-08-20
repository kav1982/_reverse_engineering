using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIActivateGirlSlot : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerClickHandler
{
	public Image image_BG;

	public Sprite sprite_BGActive;

	public Sprite sprite_BGPassive;

	public Image image_Line;

	public Image image_Icon;

	public Image image_Outline;

	public Image image_Unlocked;

	public Color lockColor;

	private UIActivateGirl uiActivateGirl;

	public int ID { get; private set; }

	public ActivateGirlConfig Config => ActivateGirlConfig.dic[ID];

	public bool CanInteract
	{
		get
		{
			if (Config.specialType == ActivateGirlSpecialType.TentacleGirlReaction && uiActivateGirl.ActivateCount >= ScriptableObjMgr.Inst.activateGirlLayerNeed.ints[Config.belongLayer])
			{
				if (DataMgr.selectedWorldData.storyHardFinishNPC7OpenFunction)
				{
					return true;
				}
				return false;
			}
			if (uiActivateGirl.ActivateCount >= ScriptableObjMgr.Inst.activateGirlLayerNeed.ints[Config.belongLayer] && (Config.preactivateID == 0 || DataMgr.selectedWorldData.activateGirlActivatedIDs2.Contains(Config.preactivateID)))
			{
				return true;
			}
			return false;
		}
	}

	public bool IsActivated
	{
		get
		{
			if (Config.specialType != ActivateGirlSpecialType.TentacleGirlReaction)
			{
				return DataMgr.selectedWorldData.activateGirlActivatedIDs2.Contains(ID);
			}
			return DataMgr.selectedWorldData.storyHardFinishNPC7OpenFunction;
		}
	}

	public void Initialize(int id, UIActivateGirl uiActivateGirl)
	{
		ID = id;
		this.uiActivateGirl = uiActivateGirl;
		image_BG.sprite = ((ActivateGirlConfig.dic[id].specialType == ActivateGirlSpecialType.TentacleGirlReaction) ? sprite_BGPassive : sprite_BGActive);
		image_Icon.sprite = ABResources.LoadAsset<Sprite>("Textures/ActivateGirlIcons/" + id);
		image_Outline.sprite = image_Icon.sprite;
		if (Config.preactivateID != 0)
		{
			image_Line.gameObject.SetActive(value: true);
			image_Line.transform.SetParent(base.transform.parent);
			image_Line.transform.SetSiblingIndex(0);
		}
	}

	public void CheckLock()
	{
		if (uiActivateGirl.ActivateCount >= ScriptableObjMgr.Inst.activateGirlLayerNeed.ints[Config.belongLayer])
		{
			if (Config.preactivateID == 0)
			{
				if (Config.specialType == ActivateGirlSpecialType.TentacleGirlReaction)
				{
					image_Icon.color = (DataMgr.selectedWorldData.storyHardFinishNPC7OpenFunction ? Color.white : lockColor);
				}
				else
				{
					image_Icon.color = Color.white;
				}
			}
			else
			{
				image_Icon.color = (DataMgr.selectedWorldData.activateGirlActivatedIDs2.Contains(Config.preactivateID) ? Color.white : lockColor);
			}
		}
		else
		{
			image_Icon.color = lockColor;
		}
		if (DataMgr.selectedWorldData.activateGirlActivatedIDs2.Contains(Config.id))
		{
			image_Unlocked.gameObject.SetActive(value: true);
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (CanInteract)
		{
			image_Outline.gameObject.SetActive(value: true);
			SEMgr.Inst.uiButtonSwitch.PlaySE();
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (CanInteract)
		{
			image_Outline.gameObject.SetActive(value: false);
		}
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (CanInteract)
		{
			image_Outline.gameObject.SetActive(value: true);
			SEMgr.Inst.uiActivateGirl_SlotEnter.PlaySE();
		}
		else
		{
			if (!GameMgr.IsMobile_Static)
			{
				return;
			}
			string text = "项目锁定:";
			if (Config.specialType == ActivateGirlSpecialType.TentacleGirlReaction)
			{
				if (uiActivateGirl.ActivateCount < ScriptableObjMgr.Inst.activateGirlLayerNeed.ints[Config.belongLayer])
				{
					text = text + "已解锁数量不足" + GetColoredString("(" + uiActivateGirl.ActivateCount + "/" + ScriptableObjMgr.Inst.activateGirlLayerNeed.ints[Config.belongLayer] + ")") + ",";
				}
				if (!DataMgr.selectedWorldData.storyHardFinishNPC7OpenFunction)
				{
					text = text + "未解锁" + GetColoredString("所需角色") + ",";
				}
			}
			else
			{
				if (Config.preactivateID != 0 && !DataMgr.selectedWorldData.activateGirlActivatedIDs2.Contains(Config.preactivateID))
				{
					text = text + "未解锁" + GetColoredString(GetSlotName(Config.preactivateID)) + ",";
				}
				if (uiActivateGirl.ActivateCount < ScriptableObjMgr.Inst.activateGirlLayerNeed.ints[Config.belongLayer])
				{
					text = text + "已解锁数量不足" + GetColoredString("(" + uiActivateGirl.ActivateCount + "/" + ScriptableObjMgr.Inst.activateGirlLayerNeed.ints[Config.belongLayer] + ")") + ",";
				}
			}
			if (text.EndsWith(","))
			{
				text = text.Remove(text.Length - 1) + ".";
			}
			uiActivateGirl.CantUnlockTextFlow(text);
		}
		uiActivateGirl.ShowInfo(this);
	}

	private string GetColoredString(string text)
	{
		return "<color=#" + ColorUtility.ToHtmlStringRGB(uiActivateGirl.corlorSlotName) + ">" + text + "</color>";
	}

	private string GetSlotName(int slotID)
	{
		return (slotID + 11000000).GetText();
	}
}
