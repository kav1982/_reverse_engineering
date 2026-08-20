using Unity.Entities;

public struct FuseHeadData : IComponentData, IQueryTypeParameter
{
	public Entity RootEntity;

	public Entity FireEffectEntity;

	public Entity SafeFireEffectEntity;

	public Entity HeadEntity;

	public Entity SafeHeadEntity;
}
