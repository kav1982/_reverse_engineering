using System.Runtime.InteropServices;
using Unity.Entities;
using UnityEngine;

public struct Spell1004LaserData : IComponentData, IQueryTypeParameter
{
	[MarshalAs(UnmanagedType.U1)]
	public bool IsInitialize;

	[MarshalAs(UnmanagedType.U1)]
	public bool IsSetLinePos;

	public float SpellSpeed;

	[MarshalAs(UnmanagedType.U1)]
	public bool IsHoverHit;

	[MarshalAs(UnmanagedType.U1)]
	public bool IsLaserHit;

	public float totalTime;

	public UnityObjectRef<Spell1004LaserCustomCtrl> LineRendererCtrl;

	public UnityObjectRef<LineRenderer> ShadowLineRenderer;
}
