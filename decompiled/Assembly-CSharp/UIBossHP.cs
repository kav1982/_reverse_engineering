using System;
using System.Collections;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

[GameUISingletonPrefab("UIBossHP")]
public class UIBossHP : GameUISingletonMono<UIBossHP>
{
	public float hpLerpSpeed = 0.2f;

	[Header("HP1")]
	public Slider slider_Hp;

	public Slider slider_HPLerp;

	public RectTransform sliderRect;

	public Image image_Frame;

	public Image image_Fill;

	public Text textName;

	public Text boss1HpValue;

	[Header("HP2")]
	public Slider slider_Hp2;

	public RectTransform sliderRect2;

	public Slider slider_HPLerp2;

	public Image image_Frame2;

	public Image image_Fill2;

	public Text textName2;

	public Text boss2HpValue;

	public float doubleHPOffsetX;

	[Header("Frame")]
	public Sprite sprite_FrameElite;

	public Sprite sprite_FrameBoss;

	public Sprite sprite_FrameElite_H;

	public Sprite sprite_FrameBoss_H;

	public Color BossHpColor;

	public Color BossHpColorAge14;

	public Color EliteHpColor;

	public Color EliteHpColorAge14;

	private EntityManager ettMgr;

	private Entity boss1Ett;

	private Entity boss2Ett;

	private Canvas mobileCanvas;

	protected override void RegistarWhenInit()
	{
		EventMgr.LanguageChange = (Action)Delegate.Combine(EventMgr.LanguageChange, new Action(OnLanguageChange));
	}

	protected override void RegistarOnlyWhenOpen()
	{
	}

	protected override void UnRegistarOnlyWhenHide()
	{
	}

	protected override void UnRegistarWhenDestroy()
	{
		EventMgr.LanguageChange = (Action)Delegate.Remove(EventMgr.LanguageChange, new Action(OnLanguageChange));
	}

	public override void Hide()
	{
		boss1Ett = Entity.Null;
		boss2Ett = Entity.Null;
		base.Hide();
	}

	private void OnLanguageChange()
	{
		if (slider_Hp.gameObject.activeSelf)
		{
			UnitProperty_Dots componentData = ettMgr.GetComponentData<UnitProperty_Dots>(boss1Ett);
			textName.text = componentData.unitCfg.GetName();
		}
		if (slider_Hp2.gameObject.activeSelf)
		{
			UnitProperty_Dots componentData2 = ettMgr.GetComponentData<UnitProperty_Dots>(boss2Ett);
			textName2.text = componentData2.unitCfg.GetName();
		}
	}

	protected override IEnumerator OnInit()
	{
		if (GameMgr.IsMobile_Static && mobileCanvas == null)
		{
			mobileCanvas = base.gameObject.AddComponent<Canvas>();
			mobileCanvas.overrideSorting = true;
		}
		yield return null;
	}

	private void Start()
	{
		ettMgr = World.DefaultGameObjectInjectionWorld.EntityManager;
	}

	private void Update()
	{
		if (GameMgr.IsMobile_Static && (bool)mobileCanvas)
		{
			mobileCanvas.sortingOrder = (UIPlayerDataMgr.Inst.IsBagOpen ? 2 : 3);
		}
		bool flag = PlayerMgr.Inst.ItemCtrller.relicCfg_ShowUnitHPUI != null && PlayerMgr.Inst.ItemCtrller.relicCfg_ShowUnitHPUI.level >= 2;
		if (slider_Hp.gameObject.activeSelf)
		{
			if (boss1Ett == Entity.Null || !ettMgr.HasComponent<UnitProperty_Dots>(boss1Ett))
			{
				slider_Hp.gameObject.SetActive(value: false);
				return;
			}
			UnitProperty_Dots componentData = ettMgr.GetComponentData<UnitProperty_Dots>(boss1Ett);
			if (componentData.unitCfg.currentHP <= 0f && componentData.unitCfg.id != 301401)
			{
				HideHP1();
			}
			else
			{
				slider_Hp.value = componentData.unitCfg.currentHP / componentData.unitCfg.maxHP;
				slider_HPLerp.value = Mathf.Lerp(slider_HPLerp.value, slider_Hp.value, hpLerpSpeed * Time.deltaTime);
			}
			boss1HpValue.text = "";
			if (flag)
			{
				boss1HpValue.text = componentData.unitCfg.currentHP.ToString("F0") + "/" + componentData.unitCfg.maxHP.ToString("F0");
			}
		}
		if (!slider_Hp2.gameObject.activeSelf)
		{
			return;
		}
		_ = boss2Ett;
		if (!ettMgr.HasComponent<UnitProperty_Dots>(boss2Ett))
		{
			slider_Hp2.gameObject.SetActive(value: false);
			return;
		}
		UnitProperty_Dots componentData2 = ettMgr.GetComponentData<UnitProperty_Dots>(boss2Ett);
		if (componentData2.unitCfg.currentHP <= 0f)
		{
			HideHP2();
		}
		else
		{
			slider_Hp2.value = componentData2.unitCfg.currentHP / componentData2.unitCfg.maxHP;
			slider_HPLerp2.value = Mathf.Lerp(slider_HPLerp2.value, slider_Hp2.value, hpLerpSpeed * Time.deltaTime);
		}
		if (flag)
		{
			boss2HpValue.text = componentData2.unitCfg.currentHP.ToString("F0") + "/" + componentData2.unitCfg.maxHP.ToString("F0");
		}
	}

	private void HideHP1()
	{
		slider_Hp.gameObject.SetActive(value: false);
		boss1Ett = Entity.Null;
		if (!slider_Hp2.gameObject.activeSelf)
		{
			BothHpIsHide();
		}
	}

	private void HideHP2()
	{
		slider_Hp2.gameObject.SetActive(value: false);
		boss2Ett = Entity.Null;
		if (!slider_Hp.gameObject.activeSelf)
		{
			BothHpIsHide();
		}
	}

	private void BothHpIsHide()
	{
		Hide();
	}

	protected override void OnHide()
	{
		slider_Hp.gameObject.SetActive(value: false);
		slider_Hp2.gameObject.SetActive(value: false);
	}

	public override void Show(object obj = null)
	{
		if (ettMgr == default(EntityManager))
		{
			ettMgr = World.DefaultGameObjectInjectionWorld.EntityManager;
		}
		OnShow(obj);
		if (!base.IsOpen)
		{
			SetIsOpen(isOpen: true);
			RegistarOnlyWhenOpen();
		}
	}

	protected override void OnShow(object obj = null)
	{
		if (!(obj is Entity))
		{
			return;
		}
		Entity entity = (Entity)obj;
		if (!ettMgr.Exists(boss1Ett))
		{
			boss1Ett = entity;
			UnitProperty_Dots componentData = ettMgr.GetComponentData<UnitProperty_Dots>(boss1Ett);
			slider_Hp.value = componentData.unitCfg.currentHP / componentData.unitCfg.maxHP;
			slider_HPLerp.value = slider_Hp.value;
			slider_Hp.gameObject.SetActive(value: true);
			textName.text = componentData.unitCfg.GetName();
			RectTransform component = slider_Hp.GetComponent<RectTransform>();
			component.anchoredPosition = new Vector2(0f, component.anchoredPosition.y);
			if (componentData.unitCfg.unitType == UnitType.Boss)
			{
				image_Fill.color = (GameMgr.IsChAge14_Static ? BossHpColorAge14 : BossHpColor);
				image_Frame.sprite = (GameMgr.IsChAge14_Static ? sprite_FrameBoss_H : sprite_FrameBoss);
			}
			else
			{
				image_Fill.color = (GameMgr.IsChAge14_Static ? EliteHpColorAge14 : EliteHpColor);
				image_Frame.sprite = (GameMgr.IsChAge14_Static ? sprite_FrameElite_H : sprite_FrameElite);
			}
			return;
		}
		boss2Ett = entity;
		UnitProperty_Dots componentData2 = ettMgr.GetComponentData<UnitProperty_Dots>(boss2Ett);
		slider_Hp2.value = componentData2.unitCfg.currentHP / componentData2.unitCfg.maxHP;
		slider_HPLerp2.value = slider_Hp2.value;
		slider_Hp2.gameObject.SetActive(value: true);
		textName2.text = componentData2.unitCfg.GetName();
		RectTransform component2 = slider_Hp.GetComponent<RectTransform>();
		RectTransform component3 = slider_Hp2.GetComponent<RectTransform>();
		component2.anchoredPosition = new Vector2(0f - doubleHPOffsetX, component2.anchoredPosition.y);
		component3.anchoredPosition = new Vector2(doubleHPOffsetX, component3.anchoredPosition.y);
		if (componentData2.unitCfg.unitType == UnitType.Boss)
		{
			image_Fill2.color = (GameMgr.IsHarmony_Static ? BossHpColorAge14 : BossHpColor);
			image_Frame2.sprite = (GameMgr.IsHarmony_Static ? sprite_FrameBoss_H : sprite_FrameBoss);
		}
		else
		{
			image_Fill2.color = (GameMgr.IsHarmony_Static ? EliteHpColorAge14 : EliteHpColor);
			image_Frame2.sprite = (GameMgr.IsHarmony_Static ? sprite_FrameElite_H : sprite_FrameElite);
		}
	}
}
