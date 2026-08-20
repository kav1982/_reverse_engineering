using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

public static class GlobalParticle
{
	public struct Emitter : IComponentData, IQueryTypeParameter, IEnableableComponent
	{
		public GlobalParticleType Type;

		public FixedString32Bytes ParticleName;

		public float RandomPositionOffset;
	}

	public struct EmitTimer : IComponentData, IQueryTypeParameter
	{
		public float Interval;

		public float Timer;
	}

	public struct EmitDistanceCounter : IComponentData, IQueryTypeParameter
	{
		public bool startCounter;

		public float3 lastCountPoint;

		public float3 lastEmitPoint;

		public float Interval;

		public float Counter;
	}
}
