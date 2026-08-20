using Unity.Entities;
using UnityEngine;

public class Spell1026ShiningStarAuthoring : MonoBehaviour
{
	public class Spell1026ShiningStarBaker : Baker<Spell1026ShiningStarAuthoring>
	{
		public override void Bake(Spell1026ShiningStarAuthoring shiningStarAuthoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			AddComponent<Spell1026ShiningStarData>(entity);
			AddComponent<Spell1026ShiningStarInitializeTag>(entity);
			AddComponent<Spell1026ShiningStarIsChargingTag>(entity);
			SetComponentEnabled<Spell1026ShiningStarInitializeTag>(entity, enabled: true);
			SetComponentEnabled<Spell1026ShiningStarIsChargingTag>(entity, enabled: true);
		}
	}
}
