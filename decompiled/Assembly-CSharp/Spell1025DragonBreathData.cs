using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct Spell1025DragonBreathData : IComponentData, IQueryTypeParameter
{
	public float minAttackDistance;

	public float baseDamageAddRatio;

	public float currentAttackDistance;

	public float powerUpStackDamageRatio;

	public float maxAttackDistance;

	public float3 LastFrameDirection;

	public float FallDamageRange;

	public UnityObjectRef<GameObject> SpellEffectObj;

	public UnityObjectRef<ParticleSystem> FireParticle;

	public UnityObjectRef<ParticleSystem> SmokeParticle;

	public UnityObjectRef<ParticleSystem> EmberParticle;

	public UnityObjectRef<ParticleSystem> VoidFireParticle;

	public float GlobalParticleEmitTimer;

	public bool IsInitialized;

	public float FallGroundEffectTimer;

	public float ShootTimer;
}
