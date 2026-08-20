using Unity.Entities;
using UnityEngine;

internal class Spell1013MeteorAuthoring : MonoBehaviour
{
	private class Spell1013MeteorAuthoringBaker : Baker<Spell1013MeteorAuthoring>
	{
		public override void Bake(Spell1013MeteorAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Spell1013MeteorData component = new Spell1013MeteorData
			{
				IsInitialized = false
			};
			AddComponent(entity, in component);
		}
	}
}
