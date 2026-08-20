using Unity.Entities;
using UnityEngine;

public class Spell9043GarbageBulletAuthoring : MonoBehaviour
{
	private class Spell9043GarbageBulletAuthoringBaker : Baker<Spell9043GarbageBulletAuthoring>
	{
		public override void Bake(Spell9043GarbageBulletAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Spell9043GarbageBulletData component = default(Spell9043GarbageBulletData);
			AddComponent(entity, in component);
		}
	}
}
