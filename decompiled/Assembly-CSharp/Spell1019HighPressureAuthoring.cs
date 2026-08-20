using Unity.Entities;
using UnityEngine;

public class Spell1019HighPressureAuthoring : MonoBehaviour
{
	private class Spell1019HighPressureAuthoringBaker : Baker<Spell1019HighPressureAuthoring>
	{
		public override void Bake(Spell1019HighPressureAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			AddComponent<Spell1019InitializedTag>(entity);
			SetComponentEnabled<Spell1019InitializedTag>(entity, enabled: true);
			AddComponent<Spell1019HighPressureData>(entity);
			AddComponent<Spell1019BulletData>(entity);
			SetComponentEnabled<Spell1019BulletData>(entity, enabled: false);
			AddComponent<Spell1019LastShootEntityData>(entity);
			SetComponentEnabled<Spell1019LastShootEntityData>(entity, enabled: false);
		}
	}
}
