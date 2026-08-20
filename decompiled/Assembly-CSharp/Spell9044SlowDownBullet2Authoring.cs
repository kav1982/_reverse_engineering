using Unity.Entities;
using UnityEngine;

public class Spell9044SlowDownBullet2Authoring : MonoBehaviour
{
	private class Spell9044SlowDownBullet2AuthoringBaker : Baker<Spell9044SlowDownBullet2Authoring>
	{
		public override void Bake(Spell9044SlowDownBullet2Authoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Spell9044SlowDownBullet2Data component = default(Spell9044SlowDownBullet2Data);
			AddComponent(entity, in component);
		}
	}
}
