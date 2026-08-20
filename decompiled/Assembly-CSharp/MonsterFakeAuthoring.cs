using Unity.Entities;
using UnityEngine;

public class MonsterFakeAuthoring : MonoBehaviour
{
	public class Baker : Baker<MonsterFakeAuthoring>
	{
		public override void Bake(MonsterFakeAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			MonsterFakePpt component = default(MonsterFakePpt);
			AddComponent(entity, in component);
		}
	}
}
