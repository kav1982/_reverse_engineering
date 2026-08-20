using Unity.Entities;
using UnityEngine;

public class MonsterTriggerAuthoring : MonoBehaviour
{
	public class Baker : Baker<MonsterTriggerAuthoring>
	{
		public override void Bake(MonsterTriggerAuthoring authoring)
		{
			GetEntity(TransformUsageFlags.NonUniformScale);
		}
	}
}
