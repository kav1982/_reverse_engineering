using Unity.Entities;
using UnityEngine;

public class ParabolaSpellAuthoring : MonoBehaviour
{
	public class ParabolaSpellAuthoringBaker : Baker<ParabolaSpellAuthoring>
	{
		public override void Bake(ParabolaSpellAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			SpellParabolaComponentData component = new SpellParabolaComponentData
			{
				BounceRatio = authoring.bounceRatio
			};
			AddComponent(entity, in component);
		}
	}

	public float bounceRatio = 0.5f;
}
