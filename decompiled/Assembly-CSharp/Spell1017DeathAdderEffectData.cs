using System.Runtime.InteropServices;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct Spell1017DeathAdderEffectData : IComponentData, IQueryTypeParameter
{
	[MarshalAs(UnmanagedType.U1)]
	public bool GroundEffectCreated;

	public UnityObjectRef<GameObject> ChainEffect;

	public UnityObjectRef<ParticleGroupPlayController> ExplosionParticleCtrl;

	public UnityObjectRef<ParticleGroupPlayController> ChargeParticleCtrl;

	public float ExplosionSpeed;

	public float HoverDuration;

	public SpellColorType ColorType;

	public float GroundScale;

	public float AroundRadius;

	public UnityObjectRef<LineRenderer> LineRenderer;

	public UnityObjectRef<LineRenderer> ShadowLineRenderer;

	public float LineWidth;

	public float3 BeginPosition;

	public float3 BoomPosition;

	public float3 CenterPoint;

	public SpellSpecialMovementType Type;

	public bool IsFallSpell;

	public float BaseHeight;

	public float RandomAngle;

	public float3 LerpPos1;

	public float3 LerpPos2;

	public float3 LerpPos1ShiftDirection;

	public float3 LerpPos2ShiftDirection;

	public float DissolveProcess;

	public float EffectExistTimer;
}
