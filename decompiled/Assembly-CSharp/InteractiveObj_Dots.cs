using Unity.Entities;
using Unity.Mathematics;

public struct InteractiveObj_Dots : IComponentData, IQueryTypeParameter
{
	public Entity ett_Outline;

	public InteractiveObjType type;

	public float3 uiOffset;

	public bool onSelect;

	public bool onDeselect;

	public bool onInteract;
}
