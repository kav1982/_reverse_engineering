using UnityEngine;

public class Spell1020ManaCoin : SpellBase
{
	public int usedCoin { get; private set; }

	public override void InitializeCallback()
	{
		usedCoin = 0;
		ApplySpeedToVelocity();
		UnitType unitType = ownerPpt.unitCfg.unitType;
		if (unitType == UnitType.Player || unitType == UnitType.Teammate || unitType == UnitType.TeammateNotAttack)
		{
			usedCoin = Mathf.CeilToInt((float)PlayerMgr.Inst.CoinCount * base.spellCfg.float1 / 100f);
			float manaCoinDamageRatio_FinalPlayerValue = CanShootSpellUtils.GetManaCoinDamageRatio_FinalPlayerValue(base.spellCfg, PlayerMgr.Inst.CoinCount);
			base.spellCfg.damage = Mathf.Ceil(SpellConfig.dic[base.spellCfg.id].damage * (1f + manaCoinDamageRatio_FinalPlayerValue) * base.damageRatio * base.finalDamageRatio) + base.InitialParameter.finalDamageExtra;
			PlayerMgr.Inst.ChangeCoin(-usedCoin);
		}
		UpdateSizeByDamageAndVolumeRatio();
		PlaySE("Shoot");
		CorrectLayerOnce();
	}

	protected override void UpdateSizeByDamageAndVolumeRatio()
	{
		if (SpellConfig.dic[base.spellCfg.id].damage != 0f)
		{
			float num = Mathf.Pow(base.damageRatio * base.finalDamageRatio, 0.3333f);
			base.transform.localScale = Vector3.one * num * base.spellVolumeRatio;
		}
	}

	public override void Update()
	{
		base.Update();
		if (base.SIP.spellIsFall)
		{
			return;
		}
		base.DurationTimer += Time.deltaTime;
		if (!(base.DurationTimer > base.spellCfg.duration))
		{
			return;
		}
		if (!base.isFlyFinish)
		{
			base.isFlyFinish = true;
			rigid.linearVelocity = Vector3.zero;
			base.CurrentSpeed = 0f;
		}
		if (base.SpellHoverTime > 0f && base.SpellHoverTimer < base.SpellHoverTime)
		{
			base.SpellHoverTimer += Time.deltaTime;
			return;
		}
		if ((!base.spellCfg.isSplitSpell && base.spellSplitCount != 0) || base.TriggerCtrl.HasOnOverTrigger())
		{
			PoolRecycle();
			return;
		}
		tsf_Layer.localScale = Vector3.one * (tsf_Layer.localScale.x - 5f * Time.deltaTime);
		if (tsf_Layer.localScale.x <= 0f)
		{
			PoolRecycle();
		}
	}

	public override void PoolRecycle()
	{
		DropCoin();
		base.PoolRecycle();
	}

	public void DropCoin()
	{
		if (usedCoin <= 0)
		{
			return;
		}
		Vector3 navMeshPointIngoreZ = Tool2D.GetNavMeshPointIngoreZ(base.transform.position);
		Vector3 centerPoint = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint;
		RoomConfig roomCfg = LevelMgr.Inst.CurrentRoomCtrller.roomCfg;
		if (roomCfg.themeType == RoomThemeType.Theme6_Chapter3 || roomCfg.themeType == RoomThemeType.Theme22_Chapter3_Shortcut1)
		{
			if (navMeshPointIngoreZ.x < centerPoint.x - (float)roomCfg.theme6Width / 2f + 1f)
			{
				navMeshPointIngoreZ.x = centerPoint.x - (float)roomCfg.theme6Width / 2f + 1f;
			}
			else if (navMeshPointIngoreZ.x > centerPoint.x + (float)roomCfg.theme6Width / 2f - 1f)
			{
				navMeshPointIngoreZ.x = centerPoint.x + (float)roomCfg.theme6Width / 2f - 1f;
			}
			if (navMeshPointIngoreZ.y < centerPoint.y - (float)roomCfg.theme6Height / 2f + 1f)
			{
				navMeshPointIngoreZ.y = centerPoint.y - (float)roomCfg.theme6Height / 2f + 1f;
			}
			else if (navMeshPointIngoreZ.y > centerPoint.y + (float)roomCfg.theme6Height / 2f - 1f)
			{
				navMeshPointIngoreZ.y = centerPoint.y + (float)roomCfg.theme6Height / 2f - 1f;
			}
		}
		ObjPoolMgr.Inst.GetGO("Prefabs/Spell/10201/10201CoinOnGround", navMeshPointIngoreZ).GetComponent<Spell1020CoinOnGround>().Initialize(usedCoin, base.transform.localScale);
		usedCoin = 0;
	}

	protected override void OnFallingGround()
	{
		EffectBase.PlayFallingGroundSound();
		MakeFallingGroundDamageToAround();
		SpellEffectBase.FallingExplosionEffectSetting fallingExplosionEffectMode = EffectBase.FallingExplosionEffectMode;
		EffectBase.FallingExplosionEffectMode = SpellEffectBase.FallingExplosionEffectSetting.CommonFall;
		EffectBase.CreateFallingExplosion();
		EffectBase.FallingExplosionEffectMode = fallingExplosionEffectMode;
		OnFallingGroundTryReboundOrRecycle();
	}
}
