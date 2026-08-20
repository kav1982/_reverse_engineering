using Unity.Entities;
using Unity.Rendering;

[MaterialProperty("_WaveStrength", -1)]
public struct Monster23_Dots_WaveRatio : IComponentData, IQueryTypeParameter
{
	public float tantacleWaveRatio;
}
