using Unity.Entities;
using UnityEngine;

public class Spell9028EnterTheGungeonBulletAuthoring : MonoBehaviour
{
	private class Spell9028EnterTheGungeonBulletAuthoringBaker : Baker<Spell9028EnterTheGungeonBulletAuthoring>
	{
		public override void Bake(Spell9028EnterTheGungeonBulletAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Spell9028EnterTheGungeonBulletData component = default(Spell9028EnterTheGungeonBulletData);
			AddComponent(entity, in component);
		}
	}
}
