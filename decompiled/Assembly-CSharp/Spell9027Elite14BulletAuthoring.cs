using Unity.Entities;
using UnityEngine;

public class Spell9027Elite14BulletAuthoring : MonoBehaviour
{
	private class Spell9027Elite14BulletAuthoringBaker : Baker<Spell9027Elite14BulletAuthoring>
	{
		public override void Bake(Spell9027Elite14BulletAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Spell9027Elite14BulletData component = default(Spell9027Elite14BulletData);
			AddComponent(entity, in component);
		}
	}
}
