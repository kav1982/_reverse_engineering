using System;
using UnityEngine;

[Serializable]
public struct VariableInt
{
	public VariableType type;

	public int value1;

	public int value2;

	public int result;

	public VariableInt(VariableType type, int value1, int value2)
	{
		this.type = type;
		this.value1 = value1;
		this.value2 = value2;
		result = 0;
	}

	public int RandomResult()
	{
		switch (type)
		{
		case VariableType.Fixed:
			result = value1;
			break;
		case VariableType.Random:
			result = UnityEngine.Random.Range(value1, value2 + 1);
			break;
		default:
			Debug.LogError("!");
			break;
		}
		return result;
	}
}
