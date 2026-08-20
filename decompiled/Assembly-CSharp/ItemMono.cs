using System;
using Unity.Entities;
using Unity.Physics;
using UnityEngine;
using UnityEngine.UI;

public class ItemMono : MonoBehaviour
{
	public Animator anima;

	public AnimaEvent animaEvent;

	public MeshRenderer mrIcon;

	public MeshRenderer mrOutline;

	public GameObject go_SpellStar1;

	public GameObject go_SpellStar2;

	[Header("EF")]
	public GameObject go_EF_Common;

	public GameObject go_EF_Rare;

	public GameObject go_EF_Epic;

	public GameObject go_EF_Special;

	public GameObject go_WandEF;

	public Transform tsf_move;

	[Header("Store")]
	public GameObject go_Canvas;

	public GameObject go_Discount;

	public Image image_CostType;

	public Image image_CostTypeDiscount;

	public Text text_CostCount;

	public Text text_CostCountDiscount;

	public Sprite sprite_Coin;

	public Sprite sprite_HP;

	public Color color_CostEnough;

	public Color color_CostNotEnough;

	[Header("Flash")]
	public float flashInterval;

	public float flashOriginalOffset;

	public float flashFinalOffset;

	public float flashSpeed;

	public Entity itemEtt;

	private EntityManager ettMgr;

	private bool isSingleInitialize;

	private GameObject go_ResourceEF;

	private bool isFlashing;

	private float flashIntervalTimer;

	private float flashCurrentOffset;

	private void Awake()
	{
		ettMgr = World.DefaultGameObjectInjectionWorld.EntityManager;
	}

	private void OnEnable()
	{
		EventMgr.coinCountChange = (Action)Delegate.Combine(EventMgr.coinCountChange, new Action(CoinCountChange));
		tsf_move.gameObject.SetActive(value: true);
	}

	private void OnDisable()
	{
		EventMgr.coinCountChange = (Action)Delegate.Remove(EventMgr.coinCountChange, new Action(CoinCountChange));
	}

	private void CoinCountChange()
	{
		UpdatePrice();
	}

	private void SingleInitialize()
	{
		if (!isSingleInitialize)
		{
			isSingleInitialize = true;
			animaEvent.DoAction = AnimaAction;
		}
	}

	private void AnimaAction(string animaName)
	{
		if (!(animaName == "FlyHighest"))
		{
			if (animaName == "FlyFinish")
			{
				if (ettMgr.HasComponent<Item>(itemEtt))
				{
					PhysicsCollider pc = ettMgr.GetComponentData<PhysicsCollider>(itemEtt);
					DTool.SetCollider(in pc, 262144u);
					Item componentData = ettMgr.GetComponentData<Item>(itemEtt);
					switch (componentData.info.type)
					{
					case ItemType.Wand:
						SEMgr.Inst.itemDropBase.PlaySE(base.transform.position);
						break;
					case ItemType.Spell:
						SEMgr.Inst.itemDropBase.PlaySE(base.transform.position);
						break;
					case ItemType.Relic:
						SEMgr.Inst.itemDropBase.PlaySE(base.transform.position);
						break;
					case ItemType.Potion:
						SEMgr.Inst.itemDropPotion.PlaySE(base.transform.position);
						break;
					case ItemType.Resource:
						SEMgr.Inst.PlaySE(ResourceConfig.GetConfig(componentData.info.id).dropSE);
						break;
					case ItemType.Curse:
						SEMgr.Inst.itemDropBase.PlaySE(base.transform.position);
						break;
					default:
						Debug.LogError(componentData.info.type);
						break;
					}
					if (componentData.info.type == ItemType.Relic || componentData.info.type == ItemType.Spell)
					{
						AnimaHover();
					}
				}
			}
			else
			{
				Debug.LogError(animaName);
			}
		}
		else if (PlayerMgr.Inst.ItemCtrller.curse_IsDiamondToCion && ettMgr.HasComponent<Item>(itemEtt))
		{
			Item componentData2 = ettMgr.GetComponentData<Item>(itemEtt);
			if (componentData2.info.type == ItemType.Resource && componentData2.info.id == 12)
			{
				componentData2.info.id = 11;
				ettMgr.SetComponentData(itemEtt, componentData2);
				Texture value = ABResources.LoadAsset<Texture>("Textures/ResourceIcons/" + componentData2.info.id);
				mrIcon.material.SetTexture("_MainTex", value);
				mrOutline.material.SetTexture("_MainTex", value);
			}
		}
	}

	private void Update()
	{
		if (isFlashing)
		{
			flashCurrentOffset = Mathf.MoveTowards(flashCurrentOffset, flashFinalOffset, flashSpeed * Time.deltaTime);
			mrIcon.material.SetFloat("_Offset", flashCurrentOffset);
			if (flashCurrentOffset == flashFinalOffset)
			{
				flashCurrentOffset = flashOriginalOffset;
				isFlashing = false;
			}
		}
		else
		{
			flashIntervalTimer += Time.deltaTime;
			if (flashIntervalTimer >= flashInterval)
			{
				flashIntervalTimer = 0f;
				isFlashing = true;
			}
		}
	}

	public void UpdateDisplay(ItemInfo info)
	{
		SingleInitialize();
		mrIcon.gameObject.SetActive(value: true);
		go_EF_Common.SetActive(value: false);
		go_EF_Rare.SetActive(value: false);
		go_EF_Epic.SetActive(value: false);
		go_EF_Special.SetActive(value: false);
		go_WandEF.SetActive(value: false);
		go_SpellStar1.SetActive(value: false);
		go_SpellStar2.SetActive(value: false);
		if (go_ResourceEF != null)
		{
			UnityEngine.Object.Destroy(go_ResourceEF);
		}
		go_Canvas.SetActive(value: false);
		go_Discount.SetActive(value: false);
		string text;
		switch (info.type)
		{
		case ItemType.Wand:
			text = WandConfig.dic[info.id].GetIconPath();
			go_WandEF.SetActive(value: true);
			break;
		case ItemType.Spell:
			text = SpellConfig.dic[info.id].GetIconPath();
			if (SpellConfig.dic[info.id].level > 1)
			{
				go_SpellStar1.SetActive(value: true);
			}
			if (SpellConfig.dic[info.id].level > 2)
			{
				go_SpellStar2.SetActive(value: true);
			}
			switch (SpellConfig.dic[info.id].dropType)
			{
			case ItemDropType.Common:
				go_EF_Common.SetActive(value: true);
				break;
			case ItemDropType.Rare:
				go_EF_Rare.SetActive(value: true);
				break;
			case ItemDropType.Epic:
				go_EF_Epic.SetActive(value: true);
				break;
			case ItemDropType.Special:
				go_EF_Special.SetActive(value: true);
				break;
			}
			break;
		case ItemType.Relic:
			text = RelicConfig.dic[info.id].GetIconPath();
			switch (RelicConfig.dic[info.id].dropType)
			{
			case ItemDropType.Common:
				go_EF_Common.SetActive(value: true);
				break;
			case ItemDropType.Rare:
				go_EF_Rare.SetActive(value: true);
				break;
			case ItemDropType.Epic:
				go_EF_Epic.SetActive(value: true);
				break;
			case ItemDropType.Special:
				go_EF_Special.SetActive(value: true);
				break;
			}
			break;
		case ItemType.Potion:
			text = PotionConfig.dic[info.id].GetIconPath();
			break;
		case ItemType.Resource:
			text = "Textures/ResourceIcons/" + info.id;
			switch (ResourceConfig.dic[info.id].abilityType)
			{
			case ResourceAbilityType.MagicCrystal:
			case ResourceAbilityType.AcientBlood:
			case ResourceAbilityType.ChaosCore:
			case ResourceAbilityType.Gear:
				mrIcon.gameObject.SetActive(value: false);
				go_ResourceEF = UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Resource/" + info.id), tsf_move.position, Quaternion.identity, tsf_move);
				break;
			default:
				Debug.LogError(ResourceConfig.dic[info.id].abilityType);
				break;
			case ResourceAbilityType.Coin:
			case ResourceAbilityType.Key:
			case ResourceAbilityType.HP:
			case ResourceAbilityType.Shield:
			case ResourceAbilityType.RuneWizardRune:
				break;
			}
			break;
		case ItemType.RuneWizardRune:
			text = "Textures/ResourceIcons/" + info.id;
			break;
		case ItemType.MaxHp:
			text = "Textures/ResourceIcons/" + info.id;
			break;
		default:
			text = "";
			Debug.LogError(info.type);
			break;
		}
		if (DataMgr.selectedWorldData.IsDave && (info.id == 31 || info.id == 32 || info.id == 33))
		{
			text += "_Dave";
		}
		Texture value = ABResources.LoadAsset<Texture>(text);
		mrIcon.material.SetTexture("_MainTex", value);
		mrOutline.material.SetTexture("_MainTex", value);
		Item componentData = ettMgr.GetComponentData<Item>(itemEtt);
		if (componentData.isStore)
		{
			go_Canvas.SetActive(value: true);
			if (componentData.priceFactor != 1f)
			{
				go_Discount.SetActive(value: true);
				image_CostTypeDiscount.sprite = image_CostType.sprite;
				text_CostCountDiscount.text = " " + componentData.GetPrice(considerDiscount: true);
				if (componentData.IsAffordable())
				{
					text_CostCountDiscount.color = color_CostEnough;
				}
				else
				{
					text_CostCountDiscount.color = color_CostNotEnough;
				}
			}
		}
		if (componentData.curseID != 0)
		{
			go_Canvas.SetActive(value: false);
		}
	}

	public void AnimaIdle()
	{
		anima.SetTrigger("Idle");
	}

	public void AnimaFly()
	{
		anima.SetTrigger("Fly");
	}

	public void AnimaHover()
	{
		anima.SetTrigger("Hover");
	}

	public void UpdatePrice()
	{
		Item componentData = ettMgr.GetComponentData<Item>(itemEtt);
		if (!componentData.isStore)
		{
			return;
		}
		text_CostCount.text = " " + componentData.GetPrice(considerDiscount: false);
		if (componentData.info.type == ItemType.Relic)
		{
			if (RelicConfig.dic[componentData.info.id].dropType == ItemDropType.Epic)
			{
				image_CostType.sprite = sprite_HP;
				if (PlayerMgr.Inst.TryGetPlayerPpt(out var playerPpt))
				{
					if (playerPpt.unitCfg.maxHP > (float)componentData.GetPrice(considerDiscount: false))
					{
						text_CostCount.color = color_CostEnough;
					}
					else
					{
						text_CostCount.color = color_CostNotEnough;
					}
				}
			}
			else
			{
				image_CostType.sprite = sprite_Coin;
				if (PlayerMgr.Inst.BaData.coinCount >= componentData.GetPrice(considerDiscount: false))
				{
					text_CostCount.color = color_CostEnough;
				}
				else
				{
					text_CostCount.color = color_CostNotEnough;
				}
			}
		}
		else
		{
			image_CostType.sprite = sprite_Coin;
			if (PlayerMgr.Inst.BaData.coinCount >= componentData.GetPrice(considerDiscount: false))
			{
				text_CostCount.color = color_CostEnough;
			}
			else
			{
				text_CostCount.color = color_CostNotEnough;
			}
		}
		if (componentData.priceFactor != 1f)
		{
			go_Discount.SetActive(value: true);
			image_CostTypeDiscount.sprite = image_CostType.sprite;
			text_CostCountDiscount.text = " " + componentData.GetPrice(considerDiscount: true);
			if (componentData.IsAffordable())
			{
				text_CostCountDiscount.color = color_CostEnough;
			}
			else
			{
				text_CostCountDiscount.color = color_CostNotEnough;
			}
		}
	}

	public void OnSelect()
	{
		mrOutline.gameObject.SetActive(value: true);
	}

	public void OnDeselect()
	{
		mrOutline.gameObject.SetActive(value: false);
	}
}
