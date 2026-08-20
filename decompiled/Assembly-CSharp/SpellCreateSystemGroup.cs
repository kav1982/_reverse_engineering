using Unity.Entities;
using UnityEngine.Scripting;

[UpdateInGroup(typeof(SpellSimulationSystemGroup), OrderFirst = true)]
public class SpellCreateSystemGroup : ComponentSystemGroup
{
	[Preserve]
	public SpellCreateSystemGroup()
	{
	}
}
