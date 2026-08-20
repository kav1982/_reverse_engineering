using Unity.Entities;

public struct DotsUnitTrigger : IComponentData, IQueryTypeParameter
{
	public Entity owner;

	public bool needInitialize;

	public float radius;

	public float height;

	public bool active;

	public bool lastActive;

	public void Initialize(Entity owner, float radius, float height, bool active)
	{
		needInitialize = true;
		this.owner = owner;
		this.radius = radius;
		this.height = height;
		this.active = active;
		lastActive = active;
	}
}
