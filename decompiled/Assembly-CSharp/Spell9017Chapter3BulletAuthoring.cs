using Unity.Entities;
using UnityEngine;

public class Spell9017Chapter3BulletAuthoring : MonoBehaviour
{
	private class Spell9017Chapter3BulletAuthoringBaker : Baker<Spell9017Chapter3BulletAuthoring>
	{
		public override void Bake(Spell9017Chapter3BulletAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Spell9017Chapter3BulletData component = default(Spell9017Chapter3BulletData);
			AddComponent(entity, in component);
		}
	}
}
