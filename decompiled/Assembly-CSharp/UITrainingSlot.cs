using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UITrainingSlot : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerClickHandler
{
	public Image image_BG;

	public Image image_Icon;

	public Sprite sprite_Hover;

	public Sprite sprite_Unhover;

	public GameObject go_Plus;

	public GameObject go_Plus2;

	public GameObject cantGetImage;

	private UITraining uiTraining;

	public int ID { get; private set; }

	public bool Unlocked { get; private set; }

	public int indexType { get; private set; }

	public int Index { get; private set; }

	public GalleryCategory Category { get; private set; }

	public void InitializeWand(UITraining uiTraining, GalleryCategory category, int id, int index)
	{
		this.uiTraining = uiTraining;
		Category = category;
		ID = id;
		Index = index;
		indexType = indexType;
		Unlocked = DataMgr.selectedWorldData.galleryUnlockedWands.Contains(ID);
		if (ScriptableObjMgr.Inst.testCtrller.UnlockAllGallery)
		{
			Unlocked = true;
		}
		image_Icon.sprite = ABResources.LoadAsset<Sprite>(WandConfig.dic[id].GetIconPath());
		UpdateIcon();
	}

	public void InitializeSpellOrRelic(UITraining uiTraining, GalleryCategory category, int id, int index, int indexType = 0, int spellLevel = 1)
	{
		this.uiTraining = uiTraining;
		Category = category;
		ID = id;
		Index = index;
		this.indexType = indexType;
		switch (category)
		{
		case GalleryCategory.Spell:
			image_Icon.sprite = ABResources.LoadAsset<Sprite>(SpellConfig.dic[id].GetIconPath());
			UpdateLevel(spellLevel);
			break;
		case GalleryCategory.Relic:
			image_Icon.sprite = ABResources.LoadAsset<Sprite>(RelicConfig.dic[id].GetIconPath());
			Unlocked = DataMgr.selectedWorldData.galleryUnlockedRelics.Contains(ID);
			if (ScriptableObjMgr.Inst.testCtrller.UnlockAllGallery)
			{
				Unlocked = true;
			}
			UpdateIcon();
			break;
		}
	}

	public void UpdateLevel(int newLevel)
	{
		SpellConfig spellConfig = SpellConfig.dic[ID];
		if (spellConfig.id == 40201 && newLevel > 2)
		{
			Unlocked = DataMgr.selectedWorldData.galleryUnlockedSpells.Contains(ID + newLevel - 2);
		}
		else if (spellConfig.dropType == ItemDropType.Common || spellConfig.dropType == ItemDropType.Rare || spellConfig.id == 10171)
		{
			Unlocked = DataMgr.selectedWorldData.galleryUnlockedSpells.Contains(ID + newLevel - 1);
		}
		else
		{
			Unlocked = DataMgr.selectedWorldData.galleryUnlockedSpells.Contains(ID);
		}
		if (ScriptableObjMgr.Inst.testCtrller.UnlockAllGallery)
		{
			Unlocked = true;
		}
		go_Plus.SetActive(value: false);
		go_Plus2.SetActive(value: false);
		if (Unlocked)
		{
			if (spellConfig.id == 40201)
			{
				if (newLevel >= 2)
				{
					go_Plus.SetActive(value: true);
				}
			}
			else if (SpellConfig.dic[ID].dropType == ItemDropType.Common || SpellConfig.dic[ID].dropType == ItemDropType.Rare || spellConfig.id == 10171)
			{
				if (newLevel >= 2)
				{
					go_Plus.SetActive(value: true);
				}
				if (newLevel >= 3)
				{
					go_Plus2.SetActive(value: true);
				}
			}
		}
		if (Unlocked)
		{
			image_Icon.color = Color.white;
		}
		else
		{
			image_Icon.color = Color.black;
		}
	}

	public void UpdateIcon()
	{
		go_Plus.SetActive(value: false);
		go_Plus2.SetActive(value: false);
		cantGetImage.SetActive(value: false);
		if (Unlocked)
		{
			image_Icon.color = Color.white;
		}
		else
		{
			image_Icon.color = Color.black;
		}
	}

	public void Hover()
	{
		image_BG.sprite = sprite_Hover;
	}

	public void Unhover()
	{
		image_BG.sprite = sprite_Unhover;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (!GameMgr.IsMobile_Static)
		{
			uiTraining.SlotEnter(this);
			if (eventData != null)
			{
				SEMgr.Inst.uiButtonSwitch.PlaySE();
			}
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (!GameMgr.IsMobile_Static)
		{
			uiTraining.SlotExit(this);
		}
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			uiTraining.SlotClick(this);
		}
	}
}
