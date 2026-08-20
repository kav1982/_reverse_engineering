using Unity.Entities;

namespace Unity.Physics.Stateful;

public struct StatefulCollisionEventDetails : IComponentData, IQueryTypeParameter
{
	public bool CalculateDetails;
}
