using Unity.Entities;
using UnityEngine;

public struct Spell2004LineRenderCleanUpData : ICleanupComponentData, IComponentData, IQueryTypeParameter
{
	public UnityObjectRef<LineRenderer> LineRenderer;
}
