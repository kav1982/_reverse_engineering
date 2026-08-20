using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct SpecialObj21_Dots : IComponentData, IQueryTypeParameter, IEnableableComponent
{
	public Entity ett_Normal;

	public Entity ett_Used;

	public Entity ett_Anima;

	public int fixedUsage;

	public BlobAssetReference<BlobArray<float>> brokenChance;

	public float3 brokenEFCenter;

	public float3 brokenEFOffset;

	public bool isInitialized;

	public int useTimer;

	public bool onSell;

	public bool onBeforeBroken;

	public bool onBroken;

	public bool UseOnce()
	{
		useTimer++;
		if (useTimer <= fixedUsage)
		{
			if (useTimer < fixedUsage)
			{
				onSell = true;
			}
			else
			{
				onBeforeBroken = true;
			}
			return true;
		}
		int num = useTimer - fixedUsage;
		if (UnityEngine.Random.value <= brokenChance.Value[num - 1])
		{
			onBroken = true;
			return false;
		}
		onBeforeBroken = true;
		return true;
	}

	public string GetName()
	{
		return 1001320.GetText();
	}

	public string GetDesc()
	{
		return 1001321.GetText();
	}
}
