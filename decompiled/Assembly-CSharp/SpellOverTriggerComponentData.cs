using Unity.Collections;
using Unity.Entities;
using UnityEngine;

public struct SpellOverTriggerComponentData : IEnableableComponent, IComponentData, IQueryTypeParameter
{
	public FixedList32Bytes<byte> TransferDamageRatios;

	public int Count => TransferDamageRatios.Length;

	public void AddRatio(float ratio)
	{
		if (Count >= TransferDamageRatios.Capacity)
		{
			Debug.LogWarning("装了太多的二重奏，存不下了");
			return;
		}
		ref FixedList32Bytes<byte> transferDamageRatios = ref TransferDamageRatios;
		byte item = (byte)(ratio * 100f);
		transferDamageRatios.Add(in item);
	}

	public readonly float GetRatio(int index)
	{
		return (float)(int)TransferDamageRatios[index] / 100f;
	}
}
