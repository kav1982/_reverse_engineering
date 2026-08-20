using System.Runtime.InteropServices;
using Unity.Entities;
using Unity.Mathematics;

public struct Spell1018FallExplosionBuffer : IBufferElementData
{
	public SpellColorType spellColorType;

	public float scale;

	public float3 currentPosition;

	public float3 nextPosition;

	[MarshalAs(UnmanagedType.U1)]
	public bool isFinalBound;
}
