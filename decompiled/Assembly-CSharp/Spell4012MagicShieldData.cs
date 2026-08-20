using System.Runtime.InteropServices;
using Unity.Entities;

public struct Spell4012MagicShieldData : IComponentData, IQueryTypeParameter
{
	[MarshalAs(UnmanagedType.U1)]
	public bool IsInitialized;

	public int ShieldMaxReduce;
}
