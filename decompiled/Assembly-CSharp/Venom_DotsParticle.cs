using Unity.Entities;

public struct Venom_DotsParticle : ICleanupComponentData, IComponentData, IQueryTypeParameter
{
	public UnityObjectRef<VenomParticle> particle;
}
