using Unity.Entities;
using Unity.Rendering;

[MaterialProperty("_WaveSpeed", -1)]
public struct Monster23_Dots_WaveSpeed : IComponentData, IQueryTypeParameter
{
	public float waveSpeed;
}
