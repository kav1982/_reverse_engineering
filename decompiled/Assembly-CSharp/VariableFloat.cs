using System;
using UnityEngine;

[Serializable]
public struct VariableFloat
{
	public VariableType type;

	public float value1;

	public float value2;

	public float result;

	public VariableFloat(VariableType type, float value1, float value2)
	{
		this.type = type;
		this.value1 = value1;
		this.value2 = value2;
		result = 0f;
	}

	public float RandomResult()
	{
		switch (type)
		{
		case VariableType.Fixed:
			result = value1;
			break;
		case VariableType.Random:
			result = UnityEngine.Random.Range(value1, value2);
			break;
		default:
			Debug.LogError("!");
			break;
		}
		return result;
	}
}
