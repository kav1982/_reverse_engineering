using Unity.Entities;

public struct Monster326_Dots : IComponentData, IQueryTypeParameter
{
	public float tailSwiggleAngle;

	public float tailSwiggleSpeed;

	public float currentSwiggleValue;

	public float tailSpacing;

	public bool IsInitialized;
}
