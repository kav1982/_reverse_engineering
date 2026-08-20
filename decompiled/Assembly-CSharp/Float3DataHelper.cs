using Unity.Mathematics;

public static class Float3DataHelper
{
	public static float3 GetFloat3(this in Float3Data f3Data)
	{
		return new float3(f3Data.x, f3Data.y, f3Data.z);
	}
}
