using Unity.Entities;
using UnityEngine;

public class IgnorePlayerSpellHitTagAuthoring : MonoBehaviour
{
	public class IgnorePlayerSpellHitTagBaker : Baker<IgnorePlayerSpellHitTagAuthoring>
	{
		public override void Bake(IgnorePlayerSpellHitTagAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			AddComponent<IgnorePlayerSpellHitTag>(entity);
		}
	}
}
