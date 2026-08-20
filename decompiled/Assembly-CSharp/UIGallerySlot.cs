using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIGallerySlot : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerClickHandler
{
	public Image image_BG;

	public Image image_Icon;

	public Image image_SpellStar1;

	public Image image_SpellStar2;

	public GameObject SpellBaned;

	public Sprite sprite_Hover;

	public Sprite sprite_Unhover;

	private UIGallery uiGallery;

	public GalleryCategory Category { get; private set; }

	public int Level1ID { get; private set; }

	public bool IsLocked { get; private set; }

	public void Initialize(UIGallery uiGallery, GalleryCategory category, int level1ID)
	{
		this.uiGallery = uiGallery;
		Category = category;
		Level1ID = level1ID;
		switch (category)
		{
		case GalleryCategory.Monster:
			image_Icon.sprite = ABResources.LoadAsset<Sprite>(UnitConfig.map[level1ID].GetIconPath());
			IsLocked = !DataMgr.selectedWorldData.galleryUnlockedMonsters.Contains(level1ID);
			break;
		case GalleryCategory.Boss:
			image_Icon.sprite = ABResources.LoadAsset<Sprite>(UnitConfig.map[level1ID].GetIconPath());
			IsLocked = !DataMgr.selectedWorldData.galleryUnlockedBosses.Contains(level1ID);
			break;
		case GalleryCategory.Spell:
			image_Icon.sprite = ABResources.LoadAsset<Sprite>(SpellConfig.dic[level1ID].GetIconPath());
			SpellUpdate(1);
			break;
		case GalleryCategory.Wand:
			image_Icon.sprite = ABResources.LoadAsset<Sprite>(WandConfig.dic[level1ID].GetIconPath());
			IsLocked = !DataMgr.selectedWorldData.galleryUnlockedWands.Contains(level1ID);
			break;
		case GalleryCategory.Relic:
			image_Icon.sprite = ABResources.LoadAsset<Sprite>(RelicConfig.dic[level1ID].GetIconPath());
			IsLocked = !DataMgr.selectedWorldData.galleryUnlockedRelics.Contains(level1ID);
			break;
		case GalleryCategory.Potion:
			image_Icon.sprite = ABResources.LoadAsset<Sprite>(PotionConfig.dic[level1ID].GetIconPath());
			IsLocked = !DataMgr.selectedWorldData.galleryUnlockedPotions.Contains(level1ID);
			break;
		case GalleryCategory.Curse:
			image_Icon.sprite = ABResources.LoadAsset<Sprite>(CurseConfig.dic[level1ID].GetIconPath());
			IsLocked = !DataMgr.selectedWorldData.galleryUnlockedCurses.Contains(level1ID);
			break;
		default:
			Debug.LogError(category);
			break;
		}
		if (ScriptableObjMgr.Inst.testCtrller.UnlockAllGallery)
		{
			IsLocked = false;
		}
		if (IsLocked)
		{
			image_Icon.color = Color.black;
		}
		else
		{
			image_Icon.color = Color.white;
		}
	}

	public void SpellUpdate(int level)
	{
		IsLocked = !DataMgr.selectedWorldData.galleryUnlockedSpells.Contains(Level1ID + level - 1);
		if ((SpellConfig.dic[Level1ID].dropType == ItemDropType.Epic || SpellConfig.dic[Level1ID].dropType == ItemDropType.Special) && Level1ID != 10171)
		{
			IsLocked = !DataMgr.selectedWorldData.galleryUnlockedSpells.Contains(Level1ID);
		}
		if (ScriptableObjMgr.Inst.testCtrller.UnlockAllGallery)
		{
			IsLocked = false;
		}
		image_SpellStar1.gameObject.SetActive(value: false);
		image_SpellStar2.gameObject.SetActive(value: false);
		if (IsLocked)
		{
			image_Icon.color = Color.black;
			return;
		}
		image_Icon.color = Color.white;
		if ((SpellConfig.dic[Level1ID].dropType != ItemDropType.Epic && SpellConfig.dic[Level1ID].dropType != ItemDropType.Special) || Level1ID == 10171)
		{
			if (level >= 2)
			{
				image_SpellStar1.gameObject.SetActive(value: true);
			}
			if (level >= 3)
			{
				image_SpellStar2.gameObject.SetActive(value: true);
			}
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
		if (GameMgr.IsMobile_Static)
		{
			if (eventData != null)
			{
				return;
			}
			OnPointerClick(null);
		}
		uiGallery.SlotEnter(this);
		if (eventData != null)
		{
			SEMgr.Inst.uiButtonSwitch.PlaySE();
		}
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (GameMgr.IsMobile_Static)
		{
			uiGallery.SlotEnter(this);
			if (eventData != null)
			{
				SEMgr.Inst.uiButtonSwitch.PlaySE();
			}
		}
	}
}
