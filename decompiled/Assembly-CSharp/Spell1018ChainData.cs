using System.Runtime.InteropServices;
using Unity.Entities;
using Unity.Mathematics;

public struct Spell1018ChainData : IComponentData, IQueryTypeParameter
{
	public float3 Position1;

	public float3 Position2;

	public float duration;

	[MarshalAs(UnmanagedType.U1)]
	public bool IsInitialized;

	[MarshalAs(UnmanagedType.U1)]
	public bool IsFirstChain;
}
