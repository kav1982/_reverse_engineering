using Unity.Entities;
using UnityEngine;

public class GlobalParticleEmitterAuthoring : MonoBehaviour
{
	private class GlobalParticleEmitterAuthoringBaker : Baker<GlobalParticleEmitterAuthoring>
	{
		public override void Bake(GlobalParticleEmitterAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			GlobalParticle.Emitter component = new GlobalParticle.Emitter
			{
				Type = authoring.Type,
				ParticleName = authoring.ParticleName,
				RandomPositionOffset = authoring.RandomPositionOffset
			};
			AddComponent(entity, in component);
			if (authoring.PreSecondEmitCount > 0f)
			{
				GlobalParticle.EmitTimer component2 = new GlobalParticle.EmitTimer
				{
					Interval = 1f / authoring.PreSecondEmitCount
				};
				AddComponent(entity, in component2);
			}
			if (authoring.PreOneDistanceEmitCount > 0f)
			{
				GlobalParticle.EmitDistanceCounter component3 = new GlobalParticle.EmitDistanceCounter
				{
					Interval = 1f / authoring.PreOneDistanceEmitCount
				};
				AddComponent(entity, in component3);
			}
		}
	}

	public GlobalParticleType Type;

	public string ParticleName;

	public float RandomPositionOffset;

	public float PreSecondEmitCount = 5f;

	public float PreOneDistanceEmitCount;
}
