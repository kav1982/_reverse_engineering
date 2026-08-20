using Unity.Entities;
using Unity.Mathematics;

public struct SpecialObj4Chapter3Reposition : IComponentData, IQueryTypeParameter, IEnableableComponent
{
	public bool onChapter3Reposition;

	public float3 repositionValue;
}
