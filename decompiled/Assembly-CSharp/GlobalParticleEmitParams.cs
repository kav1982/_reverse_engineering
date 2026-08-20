using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct GlobalParticleEmitParams : IBufferElementData
{
	public GlobalParticleType Type;

	public FixedString32Bytes Name;

	public float3 Position;

	public float? Alpha;

	public float? Size;

	public float3? Velocity;

	public GlobalParticleEmitParams(GlobalParticleType type, FixedString32Bytes name, float3 position)
	{
		this = default(GlobalParticleEmitParams);
		Type = type;
		Name = name;
		Position = position;
	}

	public ParticleSystem.EmitParams AsEmitParams(ParticleSystem ps)
	{
		ParticleSystem.EmitParams emitParams = default(ParticleSystem.EmitParams);
		emitParams.position = Position;
		ParticleSystem.EmitParams result = emitParams;
		if (Size.HasValue)
		{
			if (ps.main.startSize.mode == ParticleSystemCurveMode.Constant)
			{
				result.startSize = Size.Value * ps.main.startSize.constant;
			}
			else if (ps.main.startSize.mode == ParticleSystemCurveMode.TwoConstants)
			{
				result.startSize = Size.Value * UnityEngine.Random.Range(ps.main.startSize.constantMin, ps.main.startSize.constantMax);
			}
			else
			{
				result.startSize = Size.Value;
			}
		}
		if (Velocity.HasValue)
		{
			result.velocity = Velocity.Value;
		}
		if (Alpha.HasValue)
		{
			Color color = ps.main.startColor.Evaluate(UnityEngine.Random.Range(0f, 1f));
			color.a *= Alpha.Value;
			result.startColor = color;
		}
		return result;
	}
}
