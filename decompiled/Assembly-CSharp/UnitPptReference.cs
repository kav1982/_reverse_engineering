using Unity.Entities;
using UnityEngine.Scripting;

public class UnitPptReference : IComponentData, IQueryTypeParameter
{
	public UnitProperty unitPpt;

	[Preserve]
	public UnitPptReference()
	{
	}
}
