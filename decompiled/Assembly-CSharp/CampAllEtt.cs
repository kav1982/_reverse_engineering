using Unity.Entities;

public struct CampAllEtt : IComponentData, IQueryTypeParameter
{
	public Entity ett_EndlessCamp;

	public Entity ett_EndlessGate;

	public Entity ett_EndlessGallery;

	public Entity ett_EndlessRankingList;
}
