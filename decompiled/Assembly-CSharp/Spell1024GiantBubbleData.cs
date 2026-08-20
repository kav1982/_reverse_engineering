using Unity.Entities;
using UnityEngine;

public struct Spell1024GiantBubbleData : IComponentData, IQueryTypeParameter
{
	public bool IsInit;

	public bool IsCollapse;

	public UnityObjectRef<GameObject> EffectSpell;

	public UnityObjectRef<GameObject> EffectRange;

	public UnityObjectRef<GameObject> EffectRainGround;

	public UnityObjectRef<GameObject> EffectRain;

	public UnityObjectRef<ParticleSystem> ParticleRainGround;

	public UnityObjectRef<ParticleSystem> ParticleRain;

	public float ChargeCollisionRange;

	public const float ScaleMinusSpeed = 1.33f;

	public float EffectRangeInitScale;

	public float EffectSpellInitScale;

	public float CollapseTimer;

	public const float BoomTime = 0.8f;

	public const float ExtraBubbleDurationTime = 2f;

	public const float RainTime = 6.5f;

	public const float BubbleHighOffset = 0.6f;
}
