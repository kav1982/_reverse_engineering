using Unity.Entities;
using UnityEngine;

internal class Spell1007BlackHoleAuthoring : MonoBehaviour
{
	private class Spell1007BlackHoleAuthoringBaker : Baker<Spell1007BlackHoleAuthoring>
	{
		public override void Bake(Spell1007BlackHoleAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Spell1007BlackHoleData component = new Spell1007BlackHoleData
			{
				implosionBonusDamageRatio = 1f
			};
			AddComponent(entity, in component);
		}
	}
}
