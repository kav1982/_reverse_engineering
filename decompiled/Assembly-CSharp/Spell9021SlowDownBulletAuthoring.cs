using Unity.Entities;
using UnityEngine;

public class Spell9021SlowDownBulletAuthoring : MonoBehaviour
{
	private class Spell9021SlowDownBulletAuthoringBaker : Baker<Spell9021SlowDownBulletAuthoring>
	{
		public override void Bake(Spell9021SlowDownBulletAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Spell9021SlowDownBulletData component = default(Spell9021SlowDownBulletData);
			AddComponent(entity, in component);
		}
	}
}
