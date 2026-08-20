using Unity.Entities;
using UnityEngine;

public class PathFindingAuthoring : MonoBehaviour
{
	private class Baker : Baker<PathFindingAuthoring>
	{
		public override void Bake(PathFindingAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			PathFinding component = new PathFinding
			{
				moveThreshold = authoring.moveThreshold
			};
			AddComponent(entity, in component);
		}
	}

	public float moveThreshold = 0.1f;
}
