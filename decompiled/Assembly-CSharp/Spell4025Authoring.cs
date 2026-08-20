using Unity.Entities;
using UnityEngine;

internal class Spell4025Authoring : MonoBehaviour
{
	private class Spell4025AuthoringBaker : Baker<Spell4025Authoring>
	{
		public override void Bake(Spell4025Authoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Spell4025RuneSlashData component = default(Spell4025RuneSlashData);
			AddComponent(entity, in component);
		}
	}
}
