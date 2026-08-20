using UnityEngine;

public class StateVariableMgr
{
	private float[] floatPool = new float[10];

	private Vector3[] v3Pool = new Vector3[10];

	private int[] intPool = new int[10];

	private bool[] boolPool = new bool[10];

	public void Clear()
	{
		for (int i = 0; i < 10; i++)
		{
			floatPool[i] = 0f;
			v3Pool[i] = Vector3.zero;
			intPool[i] = 0;
			boolPool[i] = false;
		}
	}

	public ref float RegFloat(int i)
	{
		return ref floatPool[i];
	}

	public ref Vector3 RegV3(int i)
	{
		return ref v3Pool[i];
	}

	public ref int RegInt(int i)
	{
		return ref intPool[i];
	}

	public ref bool RegBool(int i)
	{
		return ref boolPool[i];
	}
}
