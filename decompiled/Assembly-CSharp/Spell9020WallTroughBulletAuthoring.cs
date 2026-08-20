using Unity.Entities;
using UnityEngine;

public class Spell9020WallTroughBulletAuthoring : MonoBehaviour
{
	private class Spell9020WallTroughBulletAuthoringBaker : Baker<Spell9020WallTroughBulletAuthoring>
	{
		public override void Bake(Spell9020WallTroughBulletAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Spell9020WallTroughBulletData component = default(Spell9020WallTroughBulletData);
			AddComponent(entity, in component);
		}
	}
}
