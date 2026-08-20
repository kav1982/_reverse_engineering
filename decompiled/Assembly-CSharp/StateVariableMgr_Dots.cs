using Unity.Mathematics;

public struct StateVariableMgr_Dots
{
	public int int1;

	public int int2;

	public int int3;

	public float float1;

	public float float2;

	public float float3;

	public float3 v1;

	public float3 v2;

	public float3 v3;

	public bool bool1;

	public bool bool2;

	public bool bool3;

	public void Clear()
	{
		int1 = 0;
		int2 = 0;
		int3 = 0;
		float1 = 0f;
		float2 = 0f;
		float3 = 0f;
		v1 = new float3(0f, 0f, 0f);
		v2 = new float3(0f, 0f, 0f);
		v3 = new float3(0f, 0f, 0f);
		bool1 = false;
		bool2 = false;
		bool3 = false;
	}
}
