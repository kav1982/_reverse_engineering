using Unity.Entities;
using UnityEngine;

public class Spell1015ArcaneNovaAuthoring : MonoBehaviour
{
	private class Spell1015ArcaneNovaAuthoringBaker : Baker<Spell1015ArcaneNovaAuthoring>
	{
		public override void Bake(Spell1015ArcaneNovaAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			AddComponent<Spell1015ArcaneNovaComponentData>(entity);
		}
	}
}
