using Unity.Entities;

internal class NewBakerScriptBaker : Baker<NewBakerScript>
{
	public override void Bake(NewBakerScript authoring)
	{
		Entity entity = GetEntity(TransformUsageFlags.Dynamic);
		NewIComponentDataScript component = new NewIComponentDataScript
		{
			moveSpeed = authoring.moveSpeed
		};
		AddComponent(entity, in component);
	}
}
