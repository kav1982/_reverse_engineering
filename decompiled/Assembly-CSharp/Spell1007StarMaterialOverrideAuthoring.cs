using Unity.Entities;
using UnityEngine;

public class Spell1007StarMaterialOverrideAuthoring : MonoBehaviour
{
	public class Spell1007StarMaterialOverrideBaker : Baker<Spell1007StarMaterialOverrideAuthoring>
	{
		public override void Bake(Spell1007StarMaterialOverrideAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Spell1007StarMaterialOverride component = new Spell1007StarMaterialOverride
			{
				Value = authoring.Offset
			};
			AddComponent(entity, in component);
		}
	}

	[Range(0f, 1f)]
	public float Offset;
}
