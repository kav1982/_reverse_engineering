using Unity.Entities;
using UnityEngine;

internal class Monster321Authoring : MonoBehaviour
{
	private class Monster321AuthoringBaker : Baker<Monster321Authoring>
	{
		public override void Bake(Monster321Authoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			EndlessMonsterTag component = new EndlessMonsterTag
			{
				dropCount = 0
			};
			AddComponent(entity, in component);
			Monster321Data component2 = new Monster321Data
			{
				ExplosionBaseDamage = authoring.ExplosionBaseDamage,
				ExplosionRange = authoring.ExplosionRange,
				CloseToTargetForceExplosionRange = authoring.CloseToTargetForceExplosionRange,
				PreExplosionDuration = authoring.PreExplosionDuration,
				IsInitialized = false
			};
			AddComponent(entity, in component2);
		}
	}

	public float ExplosionBaseDamage;

	public float ExplosionRange;

	public float CloseToTargetForceExplosionRange;

	public float PreExplosionDuration;
}
