using Unity.Entities;
using UnityEngine;

public class Spell1004LaserAuthoring : MonoBehaviour
{
	private class Spell1004LaserAuthoringBaker : Baker<Spell1004LaserAuthoring>
	{
		public override void Bake(Spell1004LaserAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Spell1004LaserData component = default(Spell1004LaserData);
			AddComponent(entity, in component);
			AddBuffer<LaserBodyPoint>(entity);
		}
	}
}
