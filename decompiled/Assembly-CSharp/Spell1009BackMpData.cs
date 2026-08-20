using System.Runtime.InteropServices;
using Unity.Entities;

public struct Spell1009BackMpData : IComponentData, IQueryTypeParameter
{
	public float NeedBackMp;

	[MarshalAs(UnmanagedType.U1)]
	public bool IsBackedMp;
}
