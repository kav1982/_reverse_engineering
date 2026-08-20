using Unity.Entities;
using UnityEngine;

public class Spell1010SnakeAuthoring : MonoBehaviour
{
	private class Spell1010SnakeAuthoringBaker : Baker<Spell1010SnakeAuthoring>
	{
		public override void Bake(Spell1010SnakeAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Spell1010SnakeData component = default(Spell1010SnakeData);
			AddComponent(entity, in component);
			AddBuffer<SnakeBodyPoint>(entity);
			AddBuffer<SnakeTouchGroundPoint>(entity);
		}
	}
}
