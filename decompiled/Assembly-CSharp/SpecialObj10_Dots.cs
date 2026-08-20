using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct SpecialObj10_Dots : IComponentData, IQueryTypeParameter
{
	public Entity ett_Normal;

	public Entity ett_Used;

	public int maxUseTime;

	public BlobAssetReference<BlobArray<int>> costs;

	public int brokenEFCount;

	public float3 brokenEFOffset;

	public float brokenEFRadius;

	public float discountRatio;

	public UnityObjectRef<SO10Mono> so10Mono;

	public bool isInitialized;

	public bool isOveruse;

	public int useTimer;

	public bool IsPlayerHaveCurse()
	{
		return PlayerMgr.Inst.BaData.curseIDs.Count > 0;
	}

	public int GetCost()
	{
		return costs.Value[useTimer];
	}

	public int GetAfterDiscountCost()
	{
		int num = costs.Value[useTimer];
		if (discountRatio != 1f)
		{
			num = Mathf.CeilToInt((float)num * discountRatio);
		}
		return num;
	}

	public bool IsHpAndShieldEnoughToBuy()
	{
		return GetPlayerHPAndShiledValue() > (float)GetAfterDiscountCost();
	}

	public float GetPlayerHPAndShiledValue()
	{
		if (PlayerMgr.Inst.TryGetPlayerPpt(out var playerPpt))
		{
			return playerPpt.unitCfg.currentHP + playerPpt.unitCfg.shieldTemp + playerPpt.unitCfg.shield;
		}
		Debug.LogError("为什么没有playerPpt");
		return 0f;
	}
}
