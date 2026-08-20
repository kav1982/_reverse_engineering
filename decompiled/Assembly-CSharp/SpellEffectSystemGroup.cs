using Unity.Entities;
using UnityEngine.Scripting;

[UpdateAfter(typeof(EnhanceSpellSystemGroup))]
[UpdateInGroup(typeof(SpellSimulationSystemGroup))]
public class SpellEffectSystemGroup : ComponentSystemGroup
{
	[Preserve]
	public SpellEffectSystemGroup()
	{
	}
}
