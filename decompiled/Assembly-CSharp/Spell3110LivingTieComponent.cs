using Unity.Entities;

public struct Spell3110LivingTieComponent : IComponentData, IQueryTypeParameter
{
	public Entity tie1;

	public Entity tie2;

	public Entity tieFire;

	public Entity tieFire2;

	public bool starting;
}
