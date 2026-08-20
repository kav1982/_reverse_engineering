using Unity.Entities;
using UnityEngine;

public class Spell9022FlowerBulletAuthoring : MonoBehaviour
{
	private class Spell9022FlowerBulletAuthoringBaker : Baker<Spell9022FlowerBulletAuthoring>
	{
		public override void Bake(Spell9022FlowerBulletAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Spell9022FlowerBulletData component = new Spell9022FlowerBulletData
			{
				InitOver = false
			};
			AddComponent(entity, in component);
		}
	}
}
