using Unity.Entities;
using UnityEngine;

public class SpellHoverDamageAuthoring : MonoBehaviour
{
	public class SpellHoverDamageAuthoringBaker : Baker<SpellHoverDamageAuthoring>
	{
		public override void Bake(SpellHoverDamageAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			SpellHoverDamageData component = new SpellHoverDamageData
			{
				Interval = authoring.Interval,
				ShowHitEffect = authoring.ShowHitEffect
			};
			AddComponent(entity, in component);
		}
	}

	public float Interval = 0.33f;

	public bool ShowHitEffect = true;
}
