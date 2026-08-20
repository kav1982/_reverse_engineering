using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;

public struct HammerAttackRangeInWandSingleton : IComponentData, IQueryTypeParameter
{
	[NativeDisableContainerSafetyRestriction]
	public NativeHashMap<UnityObjectRef<Wand>, float> WandsFirstHammerAttackRange;
}
