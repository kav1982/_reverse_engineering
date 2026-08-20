using Unity.Entities;
using UnityEngine.Scripting;

[UpdateAfter(typeof(FixedStepSimulationSystemGroup))]
public class UnitTakeDamageGroup : ComponentSystemGroup
{
	[Preserve]
	public UnitTakeDamageGroup()
	{
	}
}
