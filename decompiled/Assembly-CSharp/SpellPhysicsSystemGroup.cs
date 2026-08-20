using Unity.Entities;
using Unity.Physics.Systems;
using UnityEngine.Scripting;

[UpdateAfter(typeof(PhysicsSystemGroup))]
[UpdateBefore(typeof(EndFixedStepSimulationEntityCommandBufferSystem))]
[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
public class SpellPhysicsSystemGroup : ComponentSystemGroup
{
	[Preserve]
	public SpellPhysicsSystemGroup()
	{
	}
}
