using Unity.Entities;
using UnityEngine.Scripting;

public class UnitTriggerReference : IComponentData, IQueryTypeParameter
{
	public IDotsTriggerReceiver reference;

	[Preserve]
	public UnitTriggerReference()
	{
	}
}
