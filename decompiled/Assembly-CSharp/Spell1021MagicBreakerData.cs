using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct Spell1021MagicBreakerData : IComponentData, IQueryTypeParameter
{
	public bool FlipY;

	public float moveAngle;

	public float lastAngle;

	public float3 BaseDirection;

	public float3 SourceDirection;

	public Spell1021SlashStage SlashStage;

	public float FadeTimer;

	public float SlashTime;

	public bool readyToDestroy;

	public Entity FallTrace;

	public UnityObjectRef<GameObject> TrailEmber;

	public UnityObjectRef<GameObject> FallTrailEmber;
}
