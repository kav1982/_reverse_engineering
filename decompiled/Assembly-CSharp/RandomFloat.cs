using System;
using Unity.Mathematics;
using UnityEngine;

[Serializable]
public struct RandomFloat
{
	public float value1;

	public float value2;

	public float result;

	public RandomFloat(float value1, float value2)
	{
		this.value1 = value1;
		this.value2 = value2;
		result = 0f;
	}

	public float RandomResult(ref Unity.Mathematics.Random random)
	{
		result = random.NextFloat(value1, value2);
		return result;
	}

	public float RandomResult()
	{
		result = UnityEngine.Random.Range(value1, value2);
		return result;
	}
}
