using Unity.Entities;
using UnityEngine.Scripting;

[UpdateAfter(typeof(SpacialSpellSystemGroup))]
[UpdateInGroup(typeof(SpellSimulationSystemGroup))]
public class EnhanceSpellSystemGroup : ComponentSystemGroup
{
	[Preserve]
	public EnhanceSpellSystemGroup()
	{
	}
}
