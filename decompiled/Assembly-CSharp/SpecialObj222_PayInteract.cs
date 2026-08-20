using System;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

public class SpecialObj222_PayInteract : InteractiveObj
{
	public SpecialObj222 SpecialObj222;

	public GameObject Outlline;

	[Header("UI")]
	public GameObject goDiscount;

	public Text text_CostAfterDiscount;

	public Sprite spriteBroken;

	public MeshRenderer SpriteHandle;

	public Sprite spritePlatBroken1;

	public Sprite spritePlatBroken2;

	public MeshRenderer spriteRenderPlat1Broken;

	public MeshRenderer spriteRenderPlat2Broken;

	public Text text_Cost;

	public Vector3 SpriteOffset = new Vector3(0f, -0.2f, 0f);

	public GameObject CanvasBloodCost;

	[Header("Handle")]
	public GameObject ParticleBroke;

	public Animator animatorHandle;

	public SpecialObj222_PayInteract handle;

	public BoxCollider handleCollider;

	[SerializeField]
	private int Damage = 25;

	[HideInInspector]
	public int _damageCounted;

	public int _damageCountedDiscount;

	public int maxInteractTime = 3;

	private int _interactLeft;

	private float discount = 1f;

	public static SpecialObj222_PayInteract Inst;

	private Entity interactiveEntity;

	public bool HPAndShiledEnough
	{
		get
		{
			if (PlayerMgr.Inst.TryGetPlayerPpt(out var playerPpt))
			{
				return playerPpt.unitCfg.currentHP + playerPpt.unitCfg.shieldTemp + playerPpt.unitCfg.shield > (float)_damageCounted;
			}
			Debug.LogError("为什么没有playerPpt");
			return false;
		}
	}

	public bool HPAndShiledEnoughDiscount
	{
		get
		{
			if (PlayerMgr.Inst.TryGetPlayerPpt(out var playerPpt))
			{
				return playerPpt.unitCfg.currentHP + playerPpt.unitCfg.shieldTemp + playerPpt.unitCfg.shield > (float)_damageCountedDiscount;
			}
			Debug.LogError("为什么没有playerPpt");
			return false;
		}
	}

	public override void OnEnable()
	{
		base.OnEnable();
		EventMgr.PotionUse_Discount = (Action<float>)Delegate.Combine(EventMgr.PotionUse_Discount, new Action<float>(PotionUse_Discount));
	}

	private void OnDisable()
	{
		EventMgr.PotionUse_Discount = (Action<float>)Delegate.Remove(EventMgr.PotionUse_Discount, new Action<float>(PotionUse_Discount));
	}

	private void PotionUse_Discount(float discountRatio)
	{
		if (!(SpecialObj222.roomCtrller != LevelMgr.Inst.CurrentRoomCtrller))
		{
			discount = discountRatio;
		}
	}

	private void Start()
	{
		_damageCounted = Damage;
		_damageCountedDiscount = Damage;
		_interactLeft = maxInteractTime;
		Inst = this;
		interactiveEntity = RegisterDotsInteractiveObj(handleCollider, InteractiveObjType.SpecialObj222_PayInteract);
	}

	public int GetCost()
	{
		return Damage * (maxInteractTime - _interactLeft);
	}

	public int GetCostDiscount()
	{
		return Mathf.CeilToInt((float)(Damage * (maxInteractTime - _interactLeft)) * discount);
	}

	private void Update()
	{
		_damageCounted = GetCost();
		_damageCountedDiscount = GetCostDiscount();
		text_Cost.text = _damageCounted.ToString();
		if (!HPAndShiledEnough)
		{
			text_Cost.color = Color.red;
		}
		else
		{
			text_Cost.color = Color.green;
		}
		if (discount != 1f)
		{
			goDiscount.SetActive(value: true);
			text_CostAfterDiscount.text = GetCostDiscount().ToString();
			if (!HPAndShiledEnoughDiscount)
			{
				text_CostAfterDiscount.color = Color.red;
			}
			else
			{
				text_CostAfterDiscount.color = Color.green;
			}
		}
		CanvasBloodCost.SetActive(value: true);
		if (_interactLeft == 0)
		{
			CanvasBloodCost.SetActive(value: false);
		}
	}

	public override void Interact()
	{
		if (HPAndShiledEnoughDiscount)
		{
			animatorHandle.Play("Trigger");
			if (_damageCounted != 0)
			{
				TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(AttackerType.NothingSpecial);
				info.ignorePlayerInvincibleFrame = true;
				info.ignoreUmbrella = true;
				info.ignoreRelicDodge = true;
				info.ignoreRelicOrCurseDamageRatioChange = true;
				info.damage = GetCostDiscount();
				UnitDotsSyncSystem.AddTakeDamageRequest(PlayerMgr.Inst.PlayerEtt, info);
			}
			_interactLeft--;
			if (_interactLeft == 0)
			{
				SetDotsObjLayer(interactiveEntity, isOpen: false);
				ParticleBroke.SetActive(value: true);
				CanvasBloodCost.SetActive(value: false);
				SpriteHandle.material.SetTexture(GameConstManaged.shaderBaseMapIndex, spriteBroken.texture);
				spriteRenderPlat1Broken.material.SetTexture(GameConstManaged.shaderBaseMapIndex, spritePlatBroken1.texture);
				spriteRenderPlat2Broken.material.SetTexture(GameConstManaged.shaderBaseMapIndex, spritePlatBroken2.texture);
			}
			SetDotsObjLayer(interactiveEntity, isOpen: false);
			SpecialObj222.thisgame.InteractControl();
		}
	}

	public override void Select()
	{
		base.Select();
		Outlline.SetActive(value: true);
	}

	public override void Unselect()
	{
		base.Unselect();
		Outlline.SetActive(value: false);
	}

	public void SetCollider()
	{
		SetDotsObjLayer(interactiveEntity, isOpen: true);
	}
}
