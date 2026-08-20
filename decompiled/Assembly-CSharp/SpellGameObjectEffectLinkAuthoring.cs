using Unity.Entities;
using UnityEngine;

public class SpellGameObjectEffectLinkAuthoring : MonoBehaviour
{
	public class SpellGameObjectEffectLinkAuthoringBaker : Baker<SpellGameObjectEffectLinkAuthoring>
	{
		public override void Bake(SpellGameObjectEffectLinkAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			AddBuffer<SpellGameObjectEffectLink>(entity);
		}
	}
}
