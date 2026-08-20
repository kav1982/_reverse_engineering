using Unity.Entities;
using UnityEngine.Scripting;

[UpdateInGroup(typeof(SimulationSystemGroup))]
[UpdateBefore(typeof(UnitTakeDamageGroup))]
[UpdateAfter(typeof(VariableRateSimulationSystemGroup))]
public class SpellSimulationSystemGroup : ComponentSystemGroup
{
	[Preserve]
	public SpellSimulationSystemGroup()
	{
	}
}
