using Unity.Entities;

public struct Monster315Shield_Dots : IComponentData, IQueryTypeParameter
{
	public bool shieldInactive;

	public Entity Master;

	public Entity ShieldOn;

	public Entity ShieldOn1;
}
