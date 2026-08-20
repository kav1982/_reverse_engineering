using System;
using UnityEngine;
using UnityEngine.UI;

public class SpecialObj10 : InteractiveObj, IRoomCtrller
{
	[Space(50f)]
	public GameObject go_Canvas;

	public SpriteRenderer sr_HighLight;

	public SpriteRenderer sr_Self;

	public Sprite sprite_Overuse;

	public Text text_HPNeed;

	public int maxUseTime;

	public int[] costs;

	public int brokenEFCount;

	public Vector3 brokenEFOffset;

	public float brokenEFRadius;

	[Header("Discount")]
	public GameObject go_Discount;

	public Text text_CostAfterDiscount;

	private int useTimer;

	private float discountRatio = 1f;

	private bool isOveruse;

	private RoomController belongCtrller;

	public bool PlayerHaveCurse => PlayerMgr.Inst.BaData.curseIDs.Count > 0;

	public float HPAndShiledValue => PlayerMgr.Inst.PlayerPpt.unitCfg.currentHP + PlayerMgr.Inst.PlayerPpt.unitCfg.shieldTemp + PlayerMgr.Inst.PlayerPpt.unitCfg.shield;

	private int Cost
	{
		get
		{
			if (useTimer >= costs.Length)
			{
				return costs[costs.Length - 1];
			}
			return costs[useTimer];
		}
	}

	public void SetRoomCtrlller(RoomController levelCtrller)
	{
		belongCtrller = levelCtrller;
	}

	public override void OnEnable()
	{
		base.OnEnable();
		EventMgr.PotionUse_Discount = (Action<float>)Delegate.Combine(EventMgr.PotionUse_Discount, new Action<float>(PotionUse_Discount));
		EventMgr.PlayerHPOrShiledChange = (Action)Delegate.Combine(EventMgr.PlayerHPOrShiledChange, new Action(PlayerHPOrShiledChange));
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
			this.discountRatio = discountRatio;
			PlayerHPOrShiledChange();
		}
	}

	private void PlayerHPOrShiledChange()
	{
		if (isOveruse)
		{
			return;
		}
		if (useTimer >= maxUseTime)
		{
			isOveruse = true;
			for (int i = 0; i < brokenEFCount; i++)
			{
				Vector3 point = base.transform.position + brokenEFOffset + Tool2D.GetDir() * UnityEngine.Random.Range(0f, brokenEFRadius);
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Dead_Smoke", point, 2f);
			}
			base.gameObject.tag = "Untagged";
			SEMgr.Inst.dead_Rock.PlaySE();
			sr_Self.sprite = sprite_Overuse;
			go_Canvas.SetActive(value: false);
			return;
		}
		text_HPNeed.text = Cost.ToString();
		if (HPAndShiledValue > (float)Cost)
		{
			text_HPNeed.color = Color.green;
		}
		else
		{
			text_HPNeed.color = Color.red;
		}
		if (discountRatio != 1f)
		{
			go_Discount.SetActive(value: true);
			int afterDiscountCost = GetAfterDiscountCost();
			text_CostAfterDiscount.text = afterDiscountCost.ToString();
			if (HPAndShiledValue > (float)afterDiscountCost)
			{
				text_CostAfterDiscount.color = Color.green;
			}
			else
			{
				text_CostAfterDiscount.color = Color.red;
			}
		}
	}

	private void Start()
	{
		PlayerHPOrShiledChange();
	}

	private int GetAfterDiscountCost()
	{
		int num = Cost;
		if (discountRatio != 1f)
		{
			num = Mathf.CeilToInt((float)num * discountRatio);
		}
		return num;
	}

	public bool IsHpAndShieldEnoughToBuy()
	{
		return HPAndShiledValue > (float)GetAfterDiscountCost();
	}

	public override void Select()
	{
		sr_HighLight.gameObject.SetActive(value: true);
	}

	public override void Unselect()
	{
		sr_HighLight.gameObject.SetActive(value: false);
	}

	public override void Interact()
	{
		if (PlayerHaveCurse && IsHpAndShieldEnoughToBuy())
		{
			int afterDiscountCost = GetAfterDiscountCost();
			useTimer++;
			PlayerMgr.Inst.ItemCtrller.CurseRemoveByIndex(UnityEngine.Random.Range(0, PlayerMgr.Inst.BaData.curseIDs.Count));
			PlayerMgr.Inst.PlayerPpt.TakeDamage(afterDiscountCost, AttackerType.NothingSpecial, new TakeDamageInfo
			{
				considerPlayerInInvincibleFrame = false,
				considerRelicDodge = false,
				considerRelicOrCurseDamageRatioChange = false,
				considerUmbrella = false
			});
			PlayerHPOrShiledChange();
		}
	}
}
