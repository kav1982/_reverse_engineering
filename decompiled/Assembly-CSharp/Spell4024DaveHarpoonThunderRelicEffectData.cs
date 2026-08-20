using System.Runtime.InteropServices;
using Unity.Entities;
using Unity.Mathematics;

public struct Spell4024DaveHarpoonThunderRelicEffectData : IComponentData, IQueryTypeParameter
{
	public float Timer;

	public float3 pos1;

	public float3 pos2;

	[MarshalAs(UnmanagedType.U1)]
	public bool IsInitialized;

	[MarshalAs(UnmanagedType.U1)]
	public bool IsFirst;
}
