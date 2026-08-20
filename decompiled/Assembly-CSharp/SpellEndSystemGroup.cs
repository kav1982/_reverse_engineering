using Unity.Entities;
using UnityEngine.Scripting;

[UpdateInGroup(typeof(SimulationSystemGroup), OrderLast = true)]
public class SpellEndSystemGroup : ComponentSystemGroup
{
	[Preserve]
	public SpellEndSystemGroup()
	{
	}
}
