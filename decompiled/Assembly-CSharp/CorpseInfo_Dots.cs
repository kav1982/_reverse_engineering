using Unity.Entities;
using UnityEngine;

public struct CorpseInfo_Dots : IBufferElementData
{
	public CorpseType type;

	public Entity ett;

	public VariableFloat forwardForceNoDirect;

	public VariableFloat forwardForceHaveDirect;

	public VariableFloat upForce;

	public VariableFloat scale;

	public VariableInt bounceTime;

	public VariableFloat rotateSpeed;

	public float angleOffset;

	public float bounceRemainRatio;

	public float gravity;

	public float duration;

	public float reduceAlphaSpeed;

	public float minAlpha;

	public int colorCount;

	public Color color0;

	public Color color1;

	public Color color2;
}
