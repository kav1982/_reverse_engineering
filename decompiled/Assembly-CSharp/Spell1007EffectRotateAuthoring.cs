using Unity.Entities;
using UnityEngine;

public class Spell1007EffectRotateAuthoring : MonoBehaviour
{
	public class Spell1007EffectRotateAuthoringBaker : Baker<Spell1007EffectRotateAuthoring>
	{
		public override void Bake(Spell1007EffectRotateAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Spell1007EffectRotateComponentData component = new Spell1007EffectRotateComponentData
			{
				Speed = authoring.Speed
			};
			AddComponent(entity, in component);
		}
	}

	public float Speed = 10f;
}
