using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

internal class Spell1006GhostFireAuthoring : MonoBehaviour
{
	private class Spell1006GhostFireAuthoringBaker : Baker<Spell1006GhostFireAuthoring>
	{
		public override void Bake(Spell1006GhostFireAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Spell1006GhostFireData component = new Spell1006GhostFireData
			{
				SelfMimicTimer = 0f,
				SelfMimicInterval = 0f,
				MinSpeed = authoring.minSpeed,
				IsInitialize = false,
				PullForceByOtherGhostFire = new float3(0f, 0f, 0f),
				InitialSpeed = 0f
			};
			AddComponent(entity, in component);
		}
	}

	public float selfMimicInterval;

	public float slowdownLerp;

	public float minSpeed;
}
