using Unity.Entities;

public struct EntityCampSkinKeep : IComponentData, IQueryTypeParameter, IEnableableComponent
{
	public Entity ett_Default;

	public Entity ett_Halloween;

	public Entity ett_Spring;

	public Entity ett_Summer;

	public Entity ett_Christmas;
}
