using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

internal class Monster322Authoring : MonoBehaviour
{
	private class Monster322AuthoringBaker : Baker<Monster322Authoring>
	{
		public override void Bake(Monster322Authoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			EndlessMonsterTag component = new EndlessMonsterTag
			{
				dropCount = 0
			};
			AddComponent(entity, in component);
			Monster322Data component2 = new Monster322Data
			{
				HealPercent = authoring.HealHPPercent,
				HealInterval = authoring.HealInterval,
				HealTimer = 0f,
				HealRange = authoring.HealRange,
				CloseToTargetRange = authoring.CloseToTargetRange,
				TargetEntity = Entity.Null,
				TargetLastFramePosition = float3.zero,
				TargetMoveToRight = true,
				RecheckTimer = 0f,
				RecheckInterval = authoring.RecheckTargetInterval,
				IsInitialized = false,
				ChainEntity = GetEntity(authoring.ChainPrefab, TransformUsageFlags.Dynamic)
			};
			AddComponent(entity, in component2);
		}
	}

	public float HealHPPercent;

	public float HealInterval;

	public float HealRange;

	public float CloseToTargetRange;

	public float RecheckTargetInterval;

	public GameObject ChainPrefab;
}
