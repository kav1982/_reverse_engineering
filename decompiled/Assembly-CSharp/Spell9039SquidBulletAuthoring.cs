using Unity.Entities;
using UnityEngine;

public class Spell9039SquidBulletAuthoring : MonoBehaviour
{
	private class Spell9039SquidBulletAuthoringBaker : Baker<Spell9039SquidBulletAuthoring>
	{
		public override void Bake(Spell9039SquidBulletAuthoring squidBulletAuthoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Spell9039SquidBulletData component = default(Spell9039SquidBulletData);
			AddComponent(entity, in component);
		}
	}
}
