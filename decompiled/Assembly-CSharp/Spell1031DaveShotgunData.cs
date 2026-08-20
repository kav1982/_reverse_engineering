using System.Runtime.InteropServices;
using Unity.Entities;
using Unity.Mathematics;

public struct Spell1031DaveShotgunData : IComponentData, IQueryTypeParameter
{
	[MarshalAs(UnmanagedType.U1)]
	public bool IsInitialized;

	public bool CreateDestroyEffected;

	public float3 LastFramePosition;

	public float MoveDistance;

	public float MoveDistanceMax;
}
