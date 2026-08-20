using Unity.Entities;
using UnityEngine;

public class Spell1002BeHitMaterialAuthoring : MonoBehaviour
{
	private class Spell1002OnHitMaterialAuthoringBaker : Baker<Spell1002BeHitMaterialAuthoring>
	{
		public override void Bake(Spell1002BeHitMaterialAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			AddComponent<Spell1002BeHitMaterialProperty>(entity);
		}
	}
}
