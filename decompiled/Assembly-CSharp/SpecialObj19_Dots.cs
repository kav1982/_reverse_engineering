using Unity.Entities;

public struct SpecialObj19_Dots : IComponentData, IQueryTypeParameter, IEnableableComponent
{
	public LayerCorrectType type;

	public Entity ett_Layer;

	public float offset;
}
