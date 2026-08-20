using Rukhanka.DebugDrawer;
using Unity.Entities;
using UnityEngine.Scripting;

[UpdateInGroup(typeof(SimulationSystemGroup), OrderFirst = true)]
[UpdateBefore(typeof(CurrentRoomEntitiesSingletonSystem))]
[UpdateBefore(typeof(SpellSingletonSystem))]
[UpdateBefore(typeof(RukhankaDebugDrawerFrameStartSystem))]
[UpdateBefore(typeof(PlayerControllerDataSyncSystem))]
public class SceneEnterDoorClearPoolGroup : ComponentSystemGroup
{
	[Preserve]
	public SceneEnterDoorClearPoolGroup()
	{
	}
}
