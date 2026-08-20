using System.Runtime.CompilerServices;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Scripting;

[CompilerGenerated]
[UpdateBefore(typeof(FixedStepSimulationSystemGroup))]
[UpdateInGroup(typeof(SimulationSystemGroup), OrderFirst = true)]
public class SyncDotsFixedStepWithTimeScaleSystem : SystemBase
{
	[Preserve]
	protected override void OnUpdate()
	{
		base.World.GetOrCreateSystemManaged<FixedStepSimulationSystemGroup>().Timestep = UnityEngine.Time.fixedDeltaTime;
	}

	[Preserve]
	public SyncDotsFixedStepWithTimeScaleSystem()
	{
	}
}
