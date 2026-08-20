using Unity.Entities;
using UnityEngine.Scripting;

[UpdateInGroup(typeof(SpellSimulationSystemGroup))]
public class SpacialSpellSystemGroup : ComponentSystemGroup
{
	[Preserve]
	public SpacialSpellSystemGroup()
	{
	}
}
