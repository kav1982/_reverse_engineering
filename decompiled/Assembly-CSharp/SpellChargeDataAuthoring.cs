using Unity.Entities;
using UnityEngine;

public class SpellChargeDataAuthoring : MonoBehaviour
{
	public class SpellChargeDataBaker : Baker<SpellChargeDataAuthoring>
	{
		public override void Bake(SpellChargeDataAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			AddComponent<SpellChargeData>(entity);
			AddComponent<SpellChargingTag>(entity);
			SetComponentEnabled<SpellChargingTag>(entity, enabled: false);
		}
	}
}
