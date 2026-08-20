using Unity.Entities;

public struct SpecialObj16_Dots : IComponentData, IQueryTypeParameter, IEnableableComponent
{
	public Entity ett_Big;

	public Entity ett_Small;

	public VariableFloat scale;
}
