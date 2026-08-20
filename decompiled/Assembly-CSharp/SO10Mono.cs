using System;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

public class SO10Mono : MonoBehaviour
{
	public GameObject go_Canvas;

	public GameObject go_Discount;

	public Text text_Cost;

	public Text text_CostAfterDiscount;

	public Color color_CostEnough;

	public Color color_CostNotEnough;

	public Entity so10Ett;

	private EntityManager ettMgr;

	private RoomController belongCtrller;

	private void Awake()
	{
		ettMgr = World.DefaultGameObjectInjectionWorld.EntityManager;
	}

	public void Initialize(Entity ett)
	{
		so10Ett = ett;
		PlayerHPOrShiledChange();
	}

	private void OnEnable()
	{
		belongCtrller = LevelMgr.Inst.CurrentRoomCtrller;
		EventMgr.PotionUse_Discount = (Action<float>)Delegate.Combine(EventMgr.PotionUse_Discount, new Action<float>(PotionUse_Discount));
		EventMgr.PlayerHPOrShiledChange = (Action)Delegate.Combine(EventMgr.PlayerHPOrShiledChange, new Action(PlayerHPOrShiledChange));
		go_Canvas.SetActive(value: true);
	}

	private void OnDisable()
	{
		EventMgr.PotionUse_Discount = (Action<float>)Delegate.Remove(EventMgr.PotionUse_Discount, new Action<float>(PotionUse_Discount));
		EventMgr.PlayerHPOrShiledChange = (Action)Delegate.Remove(EventMgr.PlayerHPOrShiledChange, new Action(PlayerHPOrShiledChange));
	}

	private void PotionUse_Discount(float discountRatio)
	{
		if (!(belongCtrller != LevelMgr.Inst.CurrentRoomCtrller))
		{
			SpecialObj10_Dots componentData = ettMgr.GetComponentData<SpecialObj10_Dots>(so10Ett);
			componentData.discountRatio = discountRatio;
			ettMgr.SetComponentData(so10Ett, componentData);
			PlayerHPOrShiledChange();
		}
	}

	public void PlayerHPOrShiledChange()
	{
		SpecialObj10_Dots componentData = ettMgr.GetComponentData<SpecialObj10_Dots>(so10Ett);
		if (componentData.isOveruse)
		{
			if (go_Canvas.activeSelf)
			{
				go_Canvas.SetActive(value: false);
			}
			return;
		}
		text_Cost.text = componentData.GetCost().ToString();
		if (componentData.GetPlayerHPAndShiledValue() > (float)componentData.GetCost())
		{
			text_Cost.color = Color.green;
		}
		else
		{
			text_Cost.color = Color.red;
		}
		if (componentData.discountRatio != 1f)
		{
			go_Discount.SetActive(value: true);
			int afterDiscountCost = componentData.GetAfterDiscountCost();
			text_CostAfterDiscount.text = afterDiscountCost.ToString();
			if (componentData.GetPlayerHPAndShiledValue() > (float)afterDiscountCost)
			{
				text_CostAfterDiscount.color = Color.green;
			}
			else
			{
				text_CostAfterDiscount.color = Color.red;
			}
		}
	}
}
