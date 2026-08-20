using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct SpecialObj101Reroll_Dots : IComponentData, IQueryTypeParameter, IEnableableComponent
{
	public Entity ett_CarpetLayer;

	public Entity ett_Anima;

	public int fixedUsage;

	[Header("Broken")]
	public BlobAssetReference<BlobArray<float>> brokenChance;

	public float3 brokenEFCenter;

	public float3 brokenEFOffset;

	public bool isInitialized;

	public float3 position;

	public int useTimer;

	public bool needCheckUse;

	public bool isBroken;

	public bool UseOnce()
	{
		if (ScriptableObjMgr.Inst.testCtrller.BattleInfiniteReroll)
		{
			useTimer = 0;
		}
		useTimer++;
		needCheckUse = true;
		if (useTimer > fixedUsage)
		{
			int index = useTimer - fixedUsage - 1;
			if (UnityEngine.Random.value <= brokenChance.Value[index])
			{
				isBroken = true;
			}
		}
		return isBroken;
	}
}
