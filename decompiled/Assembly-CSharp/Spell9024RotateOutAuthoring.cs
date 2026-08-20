using Unity.Entities;
using UnityEngine;

public class Spell9024RotateOutAuthoring : MonoBehaviour
{
	private class Spell9022FlowerBulletAuthoringBaker : Baker<Spell9024RotateOutAuthoring>
	{
		public override void Bake(Spell9024RotateOutAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Spell9024RotateOutData component = new Spell9024RotateOutData
			{
				Initialized = false
			};
			AddComponent(entity, in component);
		}
	}
}
