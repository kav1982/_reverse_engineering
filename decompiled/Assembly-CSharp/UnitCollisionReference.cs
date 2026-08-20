using Unity.Entities;
using UnityEngine.Scripting;

public class UnitCollisionReference : IComponentData, IQueryTypeParameter
{
	public IDotsCollisionReceiver reference;

	[Preserve]
	public UnitCollisionReference()
	{
	}
}
