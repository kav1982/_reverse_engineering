using Unity.Entities;
using UnityEngine;

public struct Spell1019LineData : IComponentData, IQueryTypeParameter
{
	public UnityObjectRef<LineRenderer> LineRenderer;

	public UnityObjectRef<LineRenderer> LineShadowRenderer;

	public Entity StartEntity;

	public Entity EndEntity;
}
